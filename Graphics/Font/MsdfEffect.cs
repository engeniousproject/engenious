namespace engenious.Graphics
{
    
    /// <summary>
    /// A sdf font effect implementation.
    /// </summary>
    public class MsdfEffect : Effect,IModelEffect
    {
        private const string VertexShader =
            @"
in vec3 position;
in vec4 color;
in vec2 textureCoordinate;
in vec2 texSize;
out vec4 psColor;
out vec2 psTexCoord;
flat out vec2 psTexSize;

uniform mat4 World;
uniform mat4 View;
uniform mat4 Proj;

void main(void)
{
   gl_Position = Proj*(View*(World*vec4(position, 1.0)));
   psColor = color;
   psTexCoord = textureCoordinate;
   psTexSize = texSize;
}
";
        private const string PixelShader =
            @"
in vec4 psColor;
in vec2 psTexCoord;
flat in vec2 psTexSize;
out vec4 outColor;
uniform sampler2D text;

float median(float r, float g, float b) {
    return max(min(r, g), min(max(r, g), b));
}
void main(void)
{
    const float pxRange = 4;
    const float _Cutoff = 0.55;
    vec4 samplePoint = texture(text, psTexCoord);

    float dist = (_Cutoff - median(samplePoint.r, samplePoint.g, samplePoint.b)) * pxRange;
    
    vec2 duv = fwidth(psTexCoord);
    
    float dtex = length(duv * vec2(textureSize(text, 0)));
    
    float pixelDist = dist * 2 / dtex;

    outColor = psColor;
    outColor.a *= clamp(0.9 - pixelDist, 0.0, 1.0);
    //outColor.a = 1.0;
}
";

        /// <summary>
        /// Effect technique used for rendering msdf data.
        /// </summary>
        public class MsdfTechnique : EffectTechnique, IModelTechnique
        {
            private readonly GraphicsDevice _graphicsDevice;

            /// <inheritdoc />
            protected internal MsdfTechnique(GraphicsDevice graphicsDevice, string name) : base(name)
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
        /// Initializes a new instance of the <see cref="MsdfEffect"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        public MsdfEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            MultiSignedTechnique = new MsdfTechnique(graphicsDevice, "MtsdfTechnique");

            GraphicsDevice.ValidateUiGraphicsThread();
            Shader[] shaders = {
                new Shader(graphicsDevice,ShaderType.VertexShader, VertexShader),
                new Shader(graphicsDevice,ShaderType.FragmentShader, PixelShader)
            };

            foreach (var shader in shaders)
                shader.Compile();
            var pass = new EffectPass(graphicsDevice, "MtsdfPass");
            pass.AttachShaders(shaders);
            pass.BindAttribute(VertexElementUsage.Color, "color");
            pass.BindAttribute(VertexElementUsage.TextureCoordinate, "textureCoordinate");
            pass.BindAttribute(VertexElementUsage.Position, "position");
            pass.BindAttribute(VertexElementUsage.Normal, "texSize");
            pass.Link();

            MultiSignedTechnique.Passes.Add(pass);
            Techniques.Add(MultiSignedTechnique);

            CurrentTechnique = MultiSignedTechnique;
            
            Initialize();

            World = View = Projection = Matrix.Identity;


        }

        #region IEffectMatrices implementation
        
        /// <summary>
        ///     Gets the technique for rendering msdf data.
        /// </summary>
        public MsdfTechnique MultiSignedTechnique { get; }

        /// <inheritdoc />
        public Matrix Projection
        {
            get => MultiSignedTechnique.Projection;
            set => MultiSignedTechnique.Projection = value;
        }

        /// <inheritdoc />
        public Matrix View
        {
            get => MultiSignedTechnique.View;
            set => MultiSignedTechnique.View = value;
        }

        /// <inheritdoc />
        public Matrix World
        {
            get => MultiSignedTechnique.World;
            set => MultiSignedTechnique.World = value;
        }

        /// <inheritdoc />
        public Texture Texture
        {
            set => MultiSignedTechnique.Texture = value;
        }
        
        #endregion
    }
}