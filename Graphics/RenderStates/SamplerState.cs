using System;
using OpenTK.Graphics.OpenGL4;

namespace engenious.Graphics
{
    public class SamplerState:GraphicsResource
    {
        public static readonly SamplerState LinearClamp;
        public static readonly SamplerState LinearWrap;

        static SamplerState()
        {
            LinearClamp = new SamplerState();
            LinearWrap = new SamplerState();
            LinearWrap.AddressU = LinearWrap.AddressV = LinearWrap.AddressW = TextureWrapMode.Repeat;
        }

        public SamplerState()
        {
            //TODO: implement completly	
            this.AddressU = this.AddressV = this.AddressW = TextureWrapMode.ClampToEdge;
        }

        public TextureWrapMode AddressU{ get; set; }

        public TextureWrapMode AddressV{ get; set; }

        public TextureWrapMode AddressW{ get; set; }
    }
}

