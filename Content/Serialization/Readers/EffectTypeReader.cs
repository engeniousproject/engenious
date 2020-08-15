﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Content type reader to load <see cref="Effect"/> instances.
    /// </summary>
    [ContentTypeReader(typeof(Effect))]
    public class EffectTypeReader : ContentTypeReader<Effect>
    {
        private static Dictionary<string, Type> _effectTypes;

        private static HashSet<Assembly> _cachedAssemblies;
        static EffectTypeReader()
        {
            _effectTypes = new Dictionary<string, Type>();
            _cachedAssemblies = new HashSet<Assembly>();
            RecacheAssemblies();
        }

        private static void RecacheAssemblies()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (_cachedAssemblies.Contains(asm))
                    continue;
                _cachedAssemblies.Add(asm);
                if (!asm.IsDynamic)
                {
                    Type[]? exportedTypes = null;
                    try
                    {
                        exportedTypes = asm.GetExportedTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        exportedTypes = e.Types;
                    }

                    if (exportedTypes != null)
                    {
                        foreach (var type in exportedTypes)
                        {
                            try
                            {
                                if (type != null && (typeof(Effect).IsAssignableFrom(type)||typeof(EffectTechnique).IsAssignableFrom(type)||typeof(EffectPass).IsAssignableFrom(type)))
                                {
                                    _effectTypes.Add(type.FullName ?? "", type);
                                }
                            }
                            catch (Exception)
                            {
                                
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override Effect Read(ContentManager manager, ContentReader reader, Type customType = null)
        {
            var useCustomType = reader.ReadBoolean();
            bool canUseCustomType = false;
            var effectType = typeof(Effect);
            if (useCustomType)
            {
                RecacheAssemblies();
                canUseCustomType = true;
                var customTypeName = reader.ReadString();
                canUseCustomType = _effectTypes.TryGetValue(customTypeName, out effectType);
                effectType = effectType ?? typeof(Effect);
            }
            
            Effect effect = null;
            if (customType != null && effectType.IsAssignableFrom(customType))
            {
                try
                {
                    effect = (Effect) Activator.CreateInstance(customType, manager.GraphicsDevice);
                    //effectType = customType;
                    useCustomType = true;
                }
                catch (Exception)
                {
                    
                }
            }

            if (canUseCustomType && effect == null)
            {
                try
                {
                    effect = (Effect) Activator.CreateInstance(effectType, manager.GraphicsDevice);
                }
                catch (Exception)
                {

                }
            }

            if (effect == null)
                effect= new Effect(manager.GraphicsDevice);

            var techniqueCount = reader.ReadInt32();
            for (var techniqueIndex = 0; techniqueIndex < techniqueCount; techniqueIndex++)
            {
                EffectTechnique technique = null;
                string techniqueName = reader.ReadString();

                if (useCustomType)
                {
                    try
                    {
                        string customTypeName = reader.ReadString();

                        if (!_effectTypes.TryGetValue(customType.FullName + "+" + customTypeName, out var techniqueType))
                            techniqueType = _effectTypes[effectType.FullName + "+" + customTypeName];
                        technique = (EffectTechnique) Activator.CreateInstance(techniqueType, techniqueName);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                if (technique == null)
                    technique = new EffectTechnique(techniqueName);
                var passCount = reader.ReadInt32();
                for (var passIndex = 0; passIndex < passCount; passIndex++)
                {
                    var passName = reader.ReadString();
                    EffectPass pass = null;
                    if (useCustomType)
                    {
                        try
                        {
                            string customTypeName = technique.GetType().FullName + "+" + passName + "Impl";

                            var passType = _effectTypes[customTypeName];
                            pass = (EffectPass) Activator.CreateInstance(passType, manager.GraphicsDevice, passName);
                        }
                        catch (Exception)
                        {
                        
                        }
                    }
                    if (pass == null)
                        pass = new EffectPass(manager.GraphicsDevice, passName);
                    pass.BlendState = reader.Read<BlendState>(manager);
                    pass.DepthStencilState = reader.Read<DepthStencilState>(manager);
                    pass.RasterizerState = reader.Read<RasterizerState>(manager);
                    int shaderCount = reader.ReadByte();
                    
                    for (var shaderIndex = 0; shaderIndex < shaderCount; shaderIndex++)
                    {
                        var shader = new Shader(manager.GraphicsDevice,(ShaderType)reader.ReadUInt16(), reader.ReadString());
                        shader.Compile();
                        pass.AttachShader(shader);
                    }

                    int attrCount = reader.ReadByte();
                    for (var attrIndex = 0; attrIndex < attrCount; attrIndex++)//TODO: perhaps needs to be done later?
                    {
                        var usage = (VertexElementUsage)reader.ReadByte();
                        pass.BindAttribute(usage, reader.ReadString());
                    }
                    pass.Link();
                    technique.Passes.Add(pass);
                }
                effect.Techniques.Add(technique);
            }

            effect.CurrentTechnique = effect.Techniques.FirstOrDefault();
            effect.Initialize();
            return effect;
        }
    }
}

