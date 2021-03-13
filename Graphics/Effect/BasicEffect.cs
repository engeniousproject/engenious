﻿using engenious.Helper;

namespace engenious.Graphics
{
    /// <summary>
    /// A universal basic effect implementation.
    /// </summary>
    public class BasicEffect : Effect,IModelEffect
    {
        private const string VertexShader =
            @"
#if __VERSION__ >= 300

in vec3 position;
in vec4 color;
in vec2 textureCoordinate;
out vec4 psColor;
out vec2 psTexCoord;

uniform mat4 World;
uniform mat4 View;
uniform mat4 Proj;

void main(void)
{
   gl_Position = Proj*(View*(World*vec4(position, 1.0)));
   psColor = color;
   psTexCoord = textureCoordinate;
}
#else
attribute vec3 position;
attribute vec4 color;
attribute vec2 textureCoordinate;
varying vec4 psColor;
varying vec2 psTexCoord;

uniform mat4 World;
uniform mat4 View;
uniform mat4 Proj;
void main(void)
{
   gl_Position = Proj*(View*(World*vec4(position, 1.0)));
   psColor = color;
   psTexCoord = textureCoordinate;
}
#endif
";
        private const string PixelShader =
            @"
#if __VERSION__ >= 300
in vec4 psColor;
in vec2 psTexCoord;
out vec4 outColor;
uniform sampler2D text;
uniform int textEnabled,colorEnabled;
void main(void)
{
   outColor = vec4(1.0,1.0,1.0,1.0);
   if (textEnabled == 1)
     outColor = outColor * texture2D(text,psTexCoord);
   if (colorEnabled == 1)
     outColor = outColor * psColor;
   
}
#else
varying vec4 psColor;
varying vec2 psTexCoord;
uniform sampler2D text;
uniform int textEnabled,colorEnabled;
void main(void)
{
   gl_FragColor = vec4(1.0,1.0,1.0,1.0);
   if (textEnabled == 1)
     gl_FragColor = gl_FragColor * texture2D(text,psTexCoord);
   if (colorEnabled == 1)
     gl_FragColor = gl_FragColor * psColor;
   
}
#endif
";

        public class BasicTechnique : EffectTechnique, IModelTechnique
        {
            private readonly GraphicsDevice _graphicsDevice;

            /// <inheritdoc />
            protected internal BasicTechnique(GraphicsDevice graphicsDevice, string name) : base(name)
            {
                _graphicsDevice = graphicsDevice;
            }

            private Matrix _world, _view, _projection;

            /// <inheritdoc />
            public Matrix Projection
            {
                get => _projection;
                set
                {
                    // if (_projection != value)
                    {
                        _projection = value;
                        foreach(var p in Passes)
                            p.Parameters["Proj"].SetValue(value);
                    }
                }
            }

            /// <inheritdoc />
            public Matrix View
            {
                get => _view;
                set
                {
                    // if (_view != value)
                    {
                        _view = value;
                        foreach(var p in Passes)
                            p.Parameters["View"].SetValue(value);
                    }
                }
            }

            /// <inheritdoc />
            public Matrix World
            {
                get => _world;
                set
                {
                    // if (_world != value)
                    {
                        _world = value;
                        foreach(var p in Passes)
                            p.Parameters["World"].SetValue(value);
                    }
                }
            }

            /// <inheritdoc />
            public Texture Texture
            {
                set
                {
                    _graphicsDevice.Textures[0] = value;
                    foreach(var p in Passes)
                        p. Parameters["text"].SetValue(0);
                }
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicEffect"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        public BasicEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            MainTechnique = new BasicTechnique(graphicsDevice, "Basic");

            GraphicsDevice.ValidateUiGraphicsThread();
            Shader[] shaders = {
                new Shader(graphicsDevice,ShaderType.VertexShader, VertexShader),
                new Shader(graphicsDevice,ShaderType.FragmentShader, PixelShader)
            };

            foreach (var shader in shaders)
                shader.Compile();
            var pass = new EffectPass(graphicsDevice, "Basic");
            pass.AttachShaders(shaders);
            pass.BindAttribute(VertexElementUsage.Color, "color");
            pass.BindAttribute(VertexElementUsage.TextureCoordinate, "textureCoordinate");
            pass.BindAttribute(VertexElementUsage.Position, "position");
            pass.Link();

            MainTechnique.Passes.Add(pass);
            Techniques.Add(MainTechnique);

            CurrentTechnique = MainTechnique;
            
            Initialize();

            World = View = Projection = Matrix.Identity;


        }

        #region IEffectMatrices implementation
        
        public BasicTechnique MainTechnique { get; }

        /// <inheritdoc />
        public Matrix Projection
        {
            get => MainTechnique.Projection;
            set => MainTechnique.Projection = value;
        }

        /// <inheritdoc />
        public Matrix View
        {
            get => MainTechnique.View;
            set => MainTechnique.View = value;
        }

        /// <inheritdoc />
        public Matrix World
        {
            get => MainTechnique.World;
            set => MainTechnique.World = value;
        }

        /// <inheritdoc />
        public Texture Texture
        {
            set => MainTechnique.Texture = value;
        }

        /// <summary>
        /// Sets whether texture rendering is enabled.
        /// </summary>
        public bool TextureEnabled
        {

            set => Parameters["textEnabled"].SetValue(value ? 1 : 0);
        }

        /// <summary>
        /// Sets whether vertex coloring is enabled.
        /// </summary>
        public bool VertexColorEnabled
        {

            set => Parameters["colorEnabled"].SetValue(value ? 1 : 0);
        }

        #endregion
    }
}

