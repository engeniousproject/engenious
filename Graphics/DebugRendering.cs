using System;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// A useful class used for rendering debug information.
    /// </summary>
    public class DebugRendering
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;
        private readonly BasicEffect _effect;
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugRendering"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to render on.</param>
        public DebugRendering(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            
            _effect = new BasicEffect(graphicsDevice);

            _vertexBuffer = new VertexBuffer(graphicsDevice, VertexPosition.VertexDeclaration, 8);
            Span<VertexPosition> vertexData = stackalloc [] {
                new VertexPosition(new Vector3(-1f, +1f, +1f)),
                new VertexPosition(new Vector3(+1f, +1f, +1f)),
                new VertexPosition(new Vector3(-1f, -1f, +1f)),
                new VertexPosition(new Vector3(+1f, -1f, +1f)),
                new VertexPosition(new Vector3(-1f, +1f, -1f)),
                new VertexPosition(new Vector3(+1f, +1f, -1f)),
                new VertexPosition(new Vector3(-1f, -1f, -1f)),
                new VertexPosition(new Vector3(+1f, -1f, -1f)),
            };
            _vertexBuffer.SetData<VertexPosition>(vertexData);
            
            _indexBuffer = new IndexBuffer(graphicsDevice, DrawElementsType.UnsignedShort, 24);
            _indexBuffer.SetData<ushort>(stackalloc ushort[]
            {
                0, 1, 0, 2, 1, 3, 2, 3,
                4, 5, 4, 6, 5, 7, 6, 7,
                0, 4, 1, 5, 2, 6, 3, 7
            });
        }
        
        
        /// <summary>
        /// Render a <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to render.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        public void RenderBoundingBox(BoundingBox box, Matrix world, Matrix view, Matrix projection)
        {
            RenderBoundingBox(box, world, view, projection, Color.White);
        }

        /// <summary>
        /// Render a <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to render.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="color">The color of the <see cref="BoundingBox"/>.</param>
        public void RenderBoundingBox(BoundingBox box, Matrix world, Matrix view, Matrix projection, Color color)
        {
            GL.LineWidth(2);
            _graphicsDevice.VertexBuffer = _vertexBuffer;
            _graphicsDevice.IndexBuffer = _indexBuffer;
            _graphicsDevice.RasterizerState = RasterizerState.CullNone;
            //graphicsDevice.DepthStencilState = DepthStencilState.None;

            var center = (box.Min + box.Max) / 2;
            var scale = (box.Max - box.Min) / 2;
            
            _effect.World = world * Matrix.CreateTranslation(center) * Matrix.CreateScaling(scale);
            _effect.View = view;
            _effect.Projection = projection;
            _effect.VertexColorEnabled = true;
            _effect.TextureEnabled = false;
            
            foreach (var p in _effect.CurrentTechnique!.Passes)
            {
                p.Apply();
                
                GL.VertexAttrib4((int)VertexElementUsage.Color, color.R, color.G, color.B, color.A);

                _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.Lines, 0, 0,8, 0, 8);

            }
        }

        /// <summary>
        /// Render a <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum"/> to render.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        public void RenderBoundingFrustum(BoundingFrustum frustum, Matrix world, Matrix view, Matrix projection)
        {
            var m = Matrix.Invert(frustum.Matrix);
            
            _graphicsDevice.VertexBuffer = _vertexBuffer;
            _graphicsDevice.IndexBuffer = _indexBuffer;
            _graphicsDevice.RasterizerState = RasterizerState.CullNone;
            //graphicsDevice.DepthStencilState = DepthStencilState.None;

            _effect.World = world * m;
            _effect.View = view;
            _effect.Projection = projection;
            _effect.VertexColorEnabled = false;
            _effect.TextureEnabled = false;
            foreach (var p in _effect.CurrentTechnique!.Passes)
            {
                p.Apply();

                _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.Lines, 0, 0,8, 0, 8);

            }
        }
    }
}