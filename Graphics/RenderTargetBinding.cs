using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// A render target binding to render to one or multiple textures.
    /// </summary>
    public class RenderTargetBinding : IDisposable
    {
        private readonly uint _depthBuffer;

        /// <summary>
        /// A target slice to render to.
        /// </summary>
        public struct RenderTargetSlice
        {
            private RenderTargetSlice(Texture texture, int glTexture, int width, int height, int slice = 0)
            {
                Texture = texture;
                Width = width;
                Height = height;
                Slice = slice;
                GlTexture = glTexture;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RenderTargetSlice"/> struct.
            /// </summary>
            /// <param name="texture">The texture this render target slice targets.</param>
            /// <param name="width">The width of this texture target.</param>
            /// <param name="height">The height of this texture target.</param>
            public RenderTargetSlice(Texture2D texture, int width, int height)
                : this(texture, texture.Texture, width, height, 0)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RenderTargetSlice"/> struct.
            /// </summary>
            /// <param name="texture">The texture this render target slice targets.</param>
            /// <param name="width">The width of this texture target.</param>
            /// <param name="height">The height of this texture target.</param>
            /// <param name="slice">The slice of the <see cref="Texture2DArray"/> to render to.</param>
            public RenderTargetSlice(Texture2DArray texture, int width, int height, int slice = 0)
                : this(texture, texture.Texture, width, height, slice)
            {
            }

            internal int GlTexture { get; }

            /// <summary>
            /// Gets the texture this render target slice targets.
            /// </summary>
            public Texture Texture { get; }

            /// <summary>
            /// Gets the width of this texture target.
            /// </summary>
            public int Width { get; }

            /// <summary>
            /// Gets the height of this texture target.
            /// </summary>
            public int Height { get; }

            /// <summary>
            /// Gets the slice of the <see cref="Texture2DArray"/> to render to. <c>0</c> for <see cref="Texture2D"/>.
            /// </summary>
            public int Slice { get; }
        }
        private readonly int _fbo;
        /// <summary>
        /// Gets the width of the render targets.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the render targets.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the bounds for the render targets.
        /// </summary>
        /// <seealso cref="Width"/>
        /// <seealso cref="Height"/>
        public Rectangle Bounds => new(0, 0, Width, Height);
        
        private static RenderTargetSlice[] CreateSlices(Texture2D[] textures)
        {
            var slices = new RenderTargetSlice[textures.Length];

            for (int i = 0; i < slices.Length; i++)
            {
                slices[i] = CreateSlice(textures[i]);
            }

            return slices;
        }
        private static RenderTargetSlice[] CreateSlices(Texture[] textures)
        {
            var slices = new List<RenderTargetSlice>(textures.Length);

            foreach (var t in textures)
            {
                slices.AddRange(CreateSlices(t));
            }

            return slices.ToArray();
        }
        private static RenderTargetSlice CreateSlice(Texture2D texture)
        {
            return new RenderTargetSlice(texture, texture.Width, texture.Height);
        }
        private static RenderTargetSlice[] CreateSlices(Texture2DArray textures)
        {
            var slices = new RenderTargetSlice[textures.LayerCount];

            for (int i = 0; i < slices.Length; i++)
            {
                slices[i] = new RenderTargetSlice(textures, textures.Width, textures.Height, i);
            }

            return slices;
        }
        private static RenderTargetSlice[] CreateSlices(Texture textures)
        {
            if (textures is Texture2DArray array)
                return CreateSlices(array);
            if (textures is Texture2D texture2D)
                return new[] { CreateSlice(texture2D) };
            throw new NotSupportedException("Only render targets for 'Texture2D' and 'Texture2DArray' are supported.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBinding"/> class.
        /// </summary>
        /// <param name="textures">The textures to render to.</param>
        /// <param name="generateDepthBuffer">
        /// Whether to generate a depth buffer for this render target. Default: <c>true</c>.
        /// </param>
        /// <remarks><see cref="Texture2DArray"/> renders to each texture in the array as a new render target.</remarks>
        public RenderTargetBinding(Texture[] textures, bool generateDepthBuffer = true)
            : this(CreateSlices(textures), generateDepthBuffer)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBinding"/> class.
        /// </summary>
        /// <param name="textures">The textures to render to.</param>
        /// <param name="generateDepthBuffer">
        /// Whether to generate a depth buffer for this render target. Default: <c>true</c>.
        /// </param>
        public RenderTargetBinding(Texture2D[] textures, bool generateDepthBuffer = true)
            : this(CreateSlices(textures), generateDepthBuffer)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBinding"/> class.
        /// </summary>
        /// <param name="textures">The texture array to render to.</param>
        /// <param name="generateDepthBuffer">
        /// Whether to generate a depth buffer for this render target. Default: <c>true</c>.
        /// </param>
        /// <remarks><see cref="Texture2DArray"/> renders to each texture in the array as a new render target.</remarks>
        public RenderTargetBinding(Texture2DArray textures, bool generateDepthBuffer = true)
            : this(CreateSlices(textures), generateDepthBuffer)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBinding"/> class.
        /// </summary>
        /// <param name="slices">The texture slices to render to.</param>
        /// <param name="depthTexture">The depth texture to render to or <c>null</c> to render without depth texture.</param>
        public RenderTargetBinding(RenderTargetSlice[] slices, Texture2D? depthTexture)
            : this((uint)(depthTexture?.Texture ?? 0), false, depthTexture?.Width ?? 0, depthTexture?.Height ?? 0, slices)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBinding"/> class.
        /// </summary>
        /// <param name="slices">The texture slices to render to.</param>
        /// <param name="depthTexture">The depth texture array to render to or <c>null</c> to render without depth texture.</param>
        public RenderTargetBinding(RenderTargetSlice[] slices, Texture2DArray depthTexture)
            : this((uint)(depthTexture?.Texture ?? 0), false, depthTexture?.Width ?? 0, depthTexture?.Height ?? 0, slices)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBinding"/> class.
        /// </summary>
        /// <param name="slices">The texture slices to render to.</param>
        /// <param name="generateDepthBuffer">
        /// Whether to generate a depth buffer for this render target. Default: <c>true</c>.
        /// </param>
        public RenderTargetBinding(RenderTargetSlice[] slices, bool generateDepthBuffer)
            : this(GenerateDepthBuffer(generateDepthBuffer, slices), true, slices[0].Width, slices[0].Height, slices)
        {

        }

        private static uint GenerateDepthBuffer(bool generateDepthBuffer, RenderTargetSlice[] slices)
        {
            if (!generateDepthBuffer)
                return 0;
            
            if (slices.Length == 0)
                throw new ArgumentException(
                    "Cannot automatically generate a depth buffer without a single render target.", nameof(slices));
            var firstSlice = slices[0];
            var width = firstSlice.Width;
            var height = firstSlice.Height;
                
            GL.GenRenderbuffers(1, out uint depth);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, (RenderbufferStorage) All.DepthComponent32, width, height);
            return depth;

        }

        private RenderTargetBinding(uint depth, bool isBuffer, int width, int height, RenderTargetSlice[] slices)
        {
            _depthBuffer = isBuffer ? depth : 0;
            _fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

            if (depth == 0)
            {
                if (slices.Length == 0)
                    throw new ArgumentException(
                        "Either a depth texture/buffer or at least one RenderTargetSlice is required.", nameof(slices));
                Width = slices[0].Width;
                Height = slices[0].Height;
            }
            else
            {
                Width = width;
                Height = height;
                if (isBuffer)
                {
                    GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depth);
                }
                else
                {
                    GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depth, 0);
                }
            }

            if (slices.Length == 0)
            {
                GL.DrawBuffer(DrawBufferMode.None);
            }
            else
            {
                var array = new DrawBuffersEnum[slices.Length];
                for (int i = 0; i < slices.Length; i++)
                {
                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, slices[i].GlTexture, 0);
                    array[i] = DrawBuffersEnum.ColorAttachment0 + i;
                }
                GL.DrawBuffers(array.Length, array);
            }
            
            RenderTarget2D.ErrorHandling();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        internal void BindFbo()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _fbo);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_depthBuffer != 0)
            {
                GL.DeleteBuffer(_depthBuffer);
            }
        }
    }
}