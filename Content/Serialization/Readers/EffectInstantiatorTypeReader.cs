using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Content type reader to load <see cref="EffectInstantiator"/> instances.
    /// </summary>
    [ContentTypeReader(typeof(EffectInstantiator))]
    public class EffectInstantiatorTypeReader : ContentTypeReader<EffectInstantiator>
    {
        internal static readonly Dictionary<string, Type> EffectTypes;

        private static readonly HashSet<Assembly> CachedAssemblies;
        static EffectInstantiatorTypeReader()
        {
            EffectTypes = new Dictionary<string, Type>();
            CachedAssemblies = new HashSet<Assembly>();
            RecacheAssemblies();
        }

        private static void RecacheAssemblies()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (CachedAssemblies.Contains(asm))
                    continue;
                CachedAssemblies.Add(asm);
                if (!asm.IsDynamic)
                {
                    Type?[]? exportedTypes = null;
                    try
                    {
                        exportedTypes = asm.GetExportedTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        exportedTypes = e.Types;
                    }

                    foreach (var type in exportedTypes)
                    {
                        if (type == null)
                            continue;
                        try
                        {
                            if (typeof(Effect).IsAssignableFrom(type) || typeof(EffectTechnique).IsAssignableFrom(type) || typeof(EffectPass).IsAssignableFrom(type))
                            {
                                EffectTypes.Add(type.FullName ?? "", type);
                            }
                        }
                        catch (Exception)
                        {
                             
                        }
                    }
                }
            }
        }
        /// <inheritdoc />
        public EffectInstantiatorTypeReader()
            : base(1)
        {
        }

        /// <inheritdoc />
        public override EffectInstantiator Read(ContentManagerBase managerBase, ContentReader reader, Type? customType = null)
        {
            var useCustomType = reader.ReadBoolean();
            string? customTypeName = null;
            if (useCustomType)
            {
                customTypeName = reader.ReadString();
            }
            
            var effect = new EffectInstantiator(managerBase.GraphicsDevice, customTypeName);

            var techniqueCount = reader.ReadInt32();
            for (var techniqueIndex = 0; techniqueIndex < techniqueCount; techniqueIndex++)
            {
                string techniqueName = reader.ReadString();

                string? customTechniqueTypeName = null;
                if (useCustomType)
                {
                    customTechniqueTypeName = reader.ReadString();
                }
                
                var technique = new EffectInstantiator.InstantiableTechnique(techniqueName, customTechniqueTypeName);
                
                var passCount = reader.ReadInt32();
                for (var passIndex = 0; passIndex < passCount; passIndex++)
                {
                    var passName = reader.ReadString();
                    string? customPassTypeName = null;
                    if (useCustomType)
                    {
                        customPassTypeName = technique.GetType().FullName + "+" + passName + "Impl";
                    }
                    
                    var blendState = reader.Read<BlendState>(managerBase);
                    var depthStencilState = reader.Read<DepthStencilState>(managerBase);
                    var rasterizerState = reader.Read<RasterizerState>(managerBase);
                    
                    var pass = new EffectInstantiator.InstantiablePass(passName, customPassTypeName, blendState, depthStencilState, rasterizerState);

                    int shaderCount = reader.ReadByte();
                    
                    for (var shaderIndex = 0; shaderIndex < shaderCount; shaderIndex++)
                    {
                        var shaderType = (ShaderType)reader.ReadUInt16();
                        var headLineCount = reader.ReadInt32();
                        var head = reader.ReadString();
                        var source = reader.ReadString();
                        var shader = new EffectInstantiator.InstantiableShader(shaderType, headLineCount, head, source);
                        pass.Shaders.Add(shader);
                    }

                    int attrCount = reader.ReadByte();
                    for (var attrIndex = 0; attrIndex < attrCount; attrIndex++)
                    {
                        var usage = (VertexElementUsage)reader.ReadByte();
                        pass.Attributes.Add((reader.ReadString(), usage));
                    }
                    technique.Passes.Add(pass);
                }
                effect.Techniques.Add(technique);
            }

            return effect;
        }
    }
}