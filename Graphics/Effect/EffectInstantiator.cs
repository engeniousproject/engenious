using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using engenious.Content.Serialization;

namespace engenious.Graphics
{
    /// <summary>
    /// Class for instantiating <see cref="Effect"/> classes with specific settings.
    /// </summary>
    public class EffectInstantiator : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="engenious.Graphics.GraphicsDevice"/> to associate with the effect.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectInstantiator"/> class.
        /// </summary>
        /// <param name="graphicsDevice">
        /// The <see cref="engenious.Graphics.GraphicsDevice"/> to associate with the effect.
        /// </param>
        /// <param name="customTypeName">The custom type name to use for creating the instance; or <c>null</c>.</param>
        public EffectInstantiator(GraphicsDevice graphicsDevice, string? customTypeName)
        {
            GraphicsDevice = graphicsDevice;
            CustomTypeName = customTypeName;
            Techniques = new();
        }

        /// <summary>
        /// Create an instance of the <see cref="Effect"/> with default settings.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Effect"/> to create.</typeparam>
        /// <returns>The created instance of the <see cref="Effect"/>.</returns>
        public T CreateInstance<T>()
            where T : Effect
        {
            return (T)CreateInstance(typeof(T));
        }

        /// <summary>
        /// Create an instance of the <see cref="Effect"/> with default settings.
        /// </summary>
        /// <param name="settings">
        /// The settings to instantiate the <see cref="Effect"/> with; <c>null</c> for default settings.
        /// </param>
        /// <typeparam name="T">The type of the <see cref="Effect"/> to create.</typeparam>
        /// <typeparam name="TSettings">The type of the <paramref name="settings"/> to create with.</typeparam>
        /// <returns>The created instance of the <see cref="Effect"/>.</returns>
        public T CreateInstance<T, TSettings>(TSettings? settings)
            where T : Effect
            where TSettings : IEffectSettings
        {
            return (T)CreateInstance(settings, typeof(T));
        }

        /// <summary>
        /// Create an instance of the <see cref="Effect"/> with default settings.
        /// </summary>
        /// <param name="customType">The type of the <see cref="Effect"/> to create.</param>
        /// <returns>The created instance of the <see cref="Effect"/>.</returns>
        public Effect CreateInstance(Type? customType = null)
        {
            return CreateInstance<IEffectSettings>(null, customType);
        }

        /// <summary>
        /// Create an instance of the <see cref="Effect"/> with default settings.
        /// </summary>
        /// <param name="settings">
        /// The settings to instantiate the <see cref="Effect"/> with; <c>null</c> for default settings.
        /// </param>
        /// <param name="customType">The type of the <see cref="Effect"/> to create.</param>
        /// <typeparam name="TSettings">The type of the <paramref name="settings"/> to create with.</typeparam>
        /// <returns>The created instance of the <see cref="Effect"/>.</returns>
        public Effect CreateInstance<TSettings>(TSettings? settings, Type? customType = null)
            where TSettings : IEffectSettings
        {
            return CreateInstance(settings?.ToCode() ?? "", customType);
        }

        /// <summary>
        /// Create an instance of the <see cref="Effect"/> with default settings.
        /// </summary>
        /// <param name="additional">Additional code to insert after the shader head(used for setting variables).</param>
        /// <param name="customType">The type of the <see cref="Effect"/> to create.</param>
        /// <returns>The created instance of the <see cref="Effect"/>.</returns>
        public Effect CreateInstance(string additional, Type? customType)
        {
            bool useCustomType = false;
            var effectType = typeof(Effect);
            var canUseCustomType = CustomTypeName is not null;
            if (CustomTypeName is not null)
            {
                canUseCustomType = EffectInstantiatorTypeReader.EffectTypes.TryGetValue(CustomTypeName, out effectType);
                effectType ??= typeof(Effect);
            }
            
            Effect? effect = null;
            if (canUseCustomType && customType != null && effectType != typeof(Effect) && effectType.IsAssignableFrom(customType))
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
                        Debug.Assert(t.CustomTypeName != null, "t.CustomTypeName != null");
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

            var currentTechn = effect.Techniques.FirstOrDefault();
            if (currentTechn is null)
                throw new InvalidOperationException("Effect does not contain any technique!");
            

            effect.CurrentTechnique = currentTechn;
            effect.Initialize();
            return effect;
        }

        /// <summary>
        /// Gets the custom type name to use for creating the instance; or <c>null</c>.
        /// </summary>
        public string? CustomTypeName { get; }
        
        /// <summary>
        /// Gets a list of instantiable techniques for this effect.
        /// </summary>
        public List<InstantiableTechnique> Techniques { get; }

        /// <summary>
        /// A technique that can be instantiated using <see cref="EffectInstantiator"/>.
        /// </summary>
        public class InstantiableTechnique
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InstantiableTechnique"/> class.
            /// </summary>
            /// <param name="name">The name of this technique.</param>
            /// <param name="customTypeName">The custom type name to use for creating the instance; or <c>null</c>.</param>
            public InstantiableTechnique(string name, string? customTypeName)
            {
                Name = name;
                CustomTypeName = customTypeName;
                Passes = new();
            }

            /// <summary>
            /// Gets the name of this technique.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets the custom type name to use for creating the instance; or <c>null</c>.
            /// </summary>
            public string? CustomTypeName { get; }
            
            /// <summary>
            /// Gets a list of instantiable passes for this technique.
            /// </summary>
            public List<InstantiablePass> Passes { get; }
        }

        /// <summary>
        /// A pass that can be instantiated using <see cref="EffectInstantiator"/>.
        /// </summary>
        public class InstantiablePass
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InstantiablePass"/> class.
            /// </summary>
            /// <param name="name">The name of this pass.</param>
            /// <param name="customTypeName">The custom type name to use for creating the instance; or <c>null</c>.</param>
            /// <param name="blendState">The <see cref="engenious.Graphics.BlendState"/> to be used for this pass.</param>
            /// <param name="depthStencilState">The <see cref="engenious.Graphics.DepthStencilState"/> to be used for this pass.</param>
            /// <param name="rasterizerState">The <see cref="engenious.Graphics.RasterizerState"/> to be used for this pass.</param>
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

            /// <summary>
            /// Gets the name of this pass.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets the custom type name to use for creating the instance; or <c>null</c>.
            /// </summary>
            public string? CustomTypeName { get; }

            /// <summary>
            /// Gets the <see cref="engenious.Graphics.BlendState"/> to be used for this pass.
            /// </summary>
            public BlendState? BlendState { get; }

            /// <summary>
            /// Gets the <see cref="engenious.Graphics.DepthStencilState"/> to be used for this pass.
            /// </summary>
            public DepthStencilState? DepthStencilState { get; }

            /// <summary>
            /// Gets the <see cref="engenious.Graphics.RasterizerState"/> to be used for this pass.
            /// </summary>
            public RasterizerState? RasterizerState { get; }
            
            /// <summary>
            /// Gets a list of instantiable shaders which are to be compiled for this pass.
            /// </summary>
            public List<InstantiableShader> Shaders { get; }

            /// <summary>
            /// Gets a list of the attribute inputs for this pass.
            /// </summary>
            public List<(string name, VertexElementUsage usage)> Attributes { get; }
        }

        /// <summary>
        /// A shader that can be instantiated using <see cref="EffectInstantiator"/>.
        /// </summary>
        public class InstantiableShader
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InstantiableShader"/> class.
            /// </summary>
            /// <param name="shaderType">The shader type.</param>
            /// <param name="headLineCount">The number of lines in the shader head.</param>
            /// <param name="head">The shader source code head.</param>
            /// <param name="source">The shader main source.</param>
            public InstantiableShader(ShaderType shaderType, int headLineCount, string head, string source)
            {
                ShaderType = shaderType;
                HeadLineCount = headLineCount;
                Head = head;
                Source = source;
            }

            /// <summary>
            /// Gets the shader type.
            /// </summary>
            public ShaderType ShaderType { get; }

            /// <summary>
            /// Gets the number of lines in the shader head.
            /// </summary>
            public int HeadLineCount { get; }

            /// <summary>
            /// Gets the shader source code head.
            /// </summary>
            public string Head { get; }

            /// <summary>
            /// Gets the shader main source.
            /// </summary>
            public string Source { get; }
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}