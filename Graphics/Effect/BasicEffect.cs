using System;
using OpenTK;

namespace engenious.Graphics
{
    public class BasicEffect : Effect,IEffectMatrices
    {
        private const string vertexShader = 
            @"
#version 440

in vec3 position;
in vec4 color;
in vec2 textureCoordinate;
out vec4 psColor;
out vec2 psTexCoord;

uniform mat4 WorldViewProj;
void main(void)
{
   gl_Position = WorldViewProj*vec4(position, 1.0);
   psColor = color;
   psTexCoord = textureCoordinate;
}
";
        private const string pixelShader = 
            @"
#version 440
in vec4 psColor;
in vec2 psTexCoord;
uniform sampler2D text;
uniform int textEnabled,colorEnabled;
void main(void)
{
   gl_FragColor = vec4(1.0);
   if (textEnabled == 1)
     gl_FragColor = gl_FragColor * texture2D(text,psTexCoord);
   if (colorEnabled == 1)
     gl_FragColor = gl_FragColor * psColor;
   
}
";

        public BasicEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            var technique = new EffectTechnique("Basic");

            Shader[] shaders = new Shader[]
            {
                new Shader(ShaderType.VertexShader, vertexShader),
                new Shader(ShaderType.FragmentShader, pixelShader)
            };
            
            foreach (Shader shader in shaders)
                shader.Compile();
            EffectPass pass = new EffectPass("Basic");
            pass.AttachShaders(shaders);
            pass.BindAttribute(VertexElementUsage.Color, "color");
            pass.BindAttribute(VertexElementUsage.TextureCoordinate, "textureCoordinate");
            pass.BindAttribute(VertexElementUsage.Position, "position");
            pass.Link();

            technique.Passes.Add(pass);
            Techniques.Add(technique);
            CurrentTechnique = technique;

            

            Initialize();

            TextureEnabled = true;
            VertexColorEnabled = true;
        }

        #region IEffectMatrices implementation

        public Matrix Projection
        {
            get;
            set;
        }

        public Matrix View
        {
            get;
            set;
        }

        public Matrix World
        {
            get;
            set;
        }

        public Texture Texture
        {

            set
            {
                GraphicsDevice.Textures[0] = value;
                Parameters["text"].SetValue(0);
            }
        }

        public bool TextureEnabled
        {

            set
            {
                Parameters["textEnabled"].SetValue(value ? 1 : 0);
            }
        }

        public bool VertexColorEnabled
        {

            set
            {
                Parameters["colorEnabled"].SetValue(value ? 1 : 0);
            }
        }

        #endregion
    }
}

