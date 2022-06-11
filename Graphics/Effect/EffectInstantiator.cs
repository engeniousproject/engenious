using System;
using System.Collections.Generic;
using System.Linq;
using engenious.Content.Serialization;

namespace engenious.Graphics
{
    public class EffectInstantiator : IDisposable
    {
        public GraphicsDevice GraphicsDevice { get; }
        public EffectInstantiator(GraphicsDevice graphicsDevice, string? customTypeName)
        {
            GraphicsDevice = graphicsDevice;
            CustomTypeName = customTypeName;
            Techniques = new();
        }

        public T CreateInstance<T>()
            where T : Effect
        {
            return (T)CreateInstance(typeof(T));
        }
        public T CreateInstance<T, TSettings>(TSettings? settings)
            where T : Effect
            where TSettings : IEffectSettings
        {
            return (T)CreateInstance(settings, typeof(T));
        }

        public Effect CreateInstance(Type? customType = null)
        {
            return CreateInstance<IEffectSettings>(null, customType);
        }
        public Effect CreateInstance<TSettings>(TSettings? settings, Type? customType = null)
            where TSettings : IEffectSettings
        {
            return CreateInstance(settings?.ToCode() ?? "", customType);
        }

        public Effect CreateInstance(string additional, Type? customType)
        {
            bool canUseCustomType = false;
            var effectType = typeof(Effect);
            var useCustomType = CustomTypeName is not null;
            if (CustomTypeName is not null)
            {
                canUseCustomType = true;
                canUseCustomType = EffectInstantiatorTypeReader.EffectTypes.TryGetValue(CustomTypeName, out effectType);
                effectType ??= typeof(Effect);
            }
            
            Effect? effect = null;
            if (customType != null && effectType.IsAssignableFrom(customType))
            {
                try
                {
                    effect = (Effect?) Activator.CreateInstance(customType, GraphicsDevice);
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
                    effect = (Effect?) Activator.CreateInstance(effectType, GraphicsDevice);
                }
                catch (Exception)
                {

                }
            }

            effect ??= new Effect(GraphicsDevice);

            foreach(var t in Techniques)
            {
                EffectTechnique? technique = null;

                if (useCustomType)
                {
                    try
                    {
                        string customTypeName = t.CustomTypeName;

                        if (!EffectInstantiatorTypeReader.EffectTypes.TryGetValue(customType == null ? string.Empty : customType.FullName + "+" + customTypeName, out var techniqueType))
                            techniqueType = EffectInstantiatorTypeReader.EffectTypes[effectType.FullName + "+" + customTypeName];
                        technique = (EffectTechnique?) Activator.CreateInstance(techniqueType, t.Name);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                
                technique ??= new EffectTechnique(t.Name);
                
                foreach(var p in t.Passes)
                {
                    var passName = p.Name;
                    EffectPass? pass = null;
                    if (useCustomType)
                    {
                        try
                        {
                            string customTypeName = technique.GetType().FullName + "+" + passName + "Impl";

                            var passType = EffectInstantiatorTypeReader.EffectTypes[customTypeName];
                            pass = (EffectPass?) Activator.CreateInstance(passType, GraphicsDevice, passName);
                        }
                        catch (Exception)
                        {
                        
                        }
                    }
                    
                    pass ??= new EffectPass(GraphicsDevice, passName);
                    
                    pass.BlendState = p.BlendState;
                    pass.DepthStencilState = p.DepthStencilState;
                    pass.RasterizerState = p.RasterizerState;
                    
                    foreach(var s in p.Shaders)
                    {
                        var shader = new Shader(GraphicsDevice, s.ShaderType, s.HeadLineCount, s.Head, additional, s.Source);
                        shader.Compile();
                        pass.AttachShader(shader);
                    }

                    foreach(var (attributeName, usage) in p.Attributes)//TODO: perhaps needs to be done later?
                    {
                        pass.BindAttribute(usage, attributeName);
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

        public string? CustomTypeName { get; }
        
        public List<InstantiableTechnique> Techniques { get; }

        public class InstantiableTechnique
        {
            public InstantiableTechnique(string name, string? customTypeName)
            {
                Name = name;
                CustomTypeName = customTypeName;
                Passes = new();
            }

            public string Name { get; }
            public string? CustomTypeName { get; }
            
            public List<InstantiablePass> Passes { get; }
        }

        public class InstantiablePass
        {
            public InstantiablePass(string name, string? customTypeName, BlendState? blendState, DepthStencilState? depthStencilState, RasterizerState? rasterizerState)
            {
                Name = name;
                CustomTypeName = customTypeName;
                BlendState = blendState;
                DepthStencilState = depthStencilState;
                RasterizerState = rasterizerState;

                Shaders = new();
                Attributes = new();
            }

            public string Name { get; }
            public string? CustomTypeName { get; }
            public BlendState? BlendState { get; }
            public DepthStencilState? DepthStencilState { get; }
            public RasterizerState? RasterizerState { get; }
            
            public List<InstantiableShader> Shaders { get; }
            public List<(string name, VertexElementUsage usage)> Attributes { get; }
        }

        public class InstantiableShader
        {
            public InstantiableShader(ShaderType shaderType, int headLineCount, string head, string source)
            {
                ShaderType = shaderType;
                HeadLineCount = headLineCount;
                Head = head;
                Source = source;
            }

            public ShaderType ShaderType { get; }
            public int HeadLineCount { get; }

            public string Head { get; }
            public string Source { get; }
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}