using System;

namespace engenious.Graphics
{
    public class RasterizerState : GraphicsResource
    {
        public static readonly RasterizerState CullNone;
        public static readonly RasterizerState CullClockwise;
        public static readonly RasterizerState CullCounterClockwise;

        static RasterizerState()
        {
            CullNone = new RasterizerState();
            CullNone.CullMode = CullFaceMode.FrontAndBack;
            CullNone.FillMode = PolygonMode.Fill;

            CullClockwise = new RasterizerState();
            CullClockwise.CullMode = CullFaceMode.Back;
            CullClockwise.FillMode = PolygonMode.Fill;

            CullCounterClockwise = new RasterizerState();
            CullCounterClockwise.CullMode = CullFaceMode.Front;
            CullCounterClockwise.FillMode = PolygonMode.Fill;


        }

        public RasterizerState()
        {
            this.CullMode = CullFaceMode.Back;
            this.FillMode = PolygonMode.Fill;
        }

        public CullFaceMode CullMode{ get; set; }

        public PolygonMode FillMode{ get; set; }

        public bool MultiSampleAntiAlias{ get; set; }

        public bool ScissorTestEnable{ get; set; }
    }
}

