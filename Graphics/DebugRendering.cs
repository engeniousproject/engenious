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

            _vertexBuffer = new VertexBuffer(graphicsDevice, VertexPosition.VertexDeclaration, 10);
            Span<VertexPosition> vertexData = stackalloc [] {
                new VertexPosition(new Vector3(-1f, +1f, +1f)),
                new VertexPosition(new Vector3(+1f, +1f, +1f)),
                new VertexPosition(new Vector3(-1f, -1f, +1f)),
                new VertexPosition(new Vector3(+1f, -1f, +1f)),
                new VertexPosition(new Vector3(-1f, +1f, -1f)),
                new VertexPosition(new Vector3(+1f, +1f, -1f)),
                new VertexPosition(new Vector3(-1f, -1f, -1f)),
                new VertexPosition(new Vector3(+1f, -1f, -1f)),
                new VertexPosition(new Vector3(+0f, +0f, +0f)),
                new VertexPosition(new Vector3(+1f, +1f, +1f)),
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
        /// Render a line.
        /// </summary>
        /// <param name="start">The start point of the line to render.</param>
        /// <param name="end">The end point of the line to render.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        public void RenderLine(Vector3 start, Vector3 end, Matrix world, Matrix view, Matrix projection)
            => RenderLine(start, end, world, view, projection, Color.White);

        /// <summary>
        /// Render a line.
        /// </summary>
        /// <param name="start">The start point of the line to render.</param>
        /// <param name="end">The end point of the line to render.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="color">The color of the line to render.</param>
        public void RenderLine(Vector3 start, Vector3 end, Matrix world, Matrix view, Matrix projection, Color color)
        {
            _graphicsDevice.VertexBuffer = _vertexBuffer;
            _graphicsDevice.IndexBuffer = _indexBuffer;
            _graphicsDevice.RasterizerState = RasterizerState.CullNone;
            _effect.World = world * Matrix.CreateTranslation(start) * Matrix.CreateScaling(end - start);
            _effect.View = view;
            _effect.Projection = projection;
            _effect.VertexColorEnabled = true;
            _effect.TextureEnabled = false;
            foreach (EffectPass pass in _effect.CurrentTechnique!.Passes)
            {
                pass.Apply();
                GL.VertexAttrib4(3, color.R, color.G, color.B, color.A);
                _graphicsDevice.DrawPrimitives(PrimitiveType.Lines, 8, 2);
            }
        }

        /// <summary>
        /// Render a <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum"/> to render.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        public void RenderBoundingFrustum(BoundingFrustum frustum, Matrix world, Matrix view, Matrix projection) =>
            RenderBoundingFrustum(frustum, world, view, projection, Color.White);

        /// <summary>
        /// Render a <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum"/> to render.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="color">The color of the <see cref="BoundingFrustum"/>.</param>
        public void RenderBoundingFrustum(BoundingFrustum frustum, Matrix world, Matrix view, Matrix projection, Color color)
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

        /// <summary>
        /// Gets or sets the line width for the rendering of lines.
        /// <remarks>Not supported on every hardware; Does nothing if not supported.</remarks>
        /// </summary>
        public float LineWidth
        {
            get => GL.GetFloat(GetPName.LineWidth);
            set => GL.LineWidth(value);
        }
    }
}