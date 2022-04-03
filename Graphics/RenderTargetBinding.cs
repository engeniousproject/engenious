using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    public class RenderTargetBinding : IDisposable
    {
        private readonly uint _depthBuffer;

        public struct RenderTargetSlice
        {
            private RenderTargetSlice(Texture texture, int glTexture, int width, int height, int slice = 0)
            {
                Texture = texture;
                Width = width;
                Height = height;
                Slice = slice;
                GLTexture = glTexture;
            }
            public RenderTargetSlice(Texture2D texture, int width, int height, int slice = 0)
                : this(texture, texture.Texture, width, height, slice)
            {
            }
            public RenderTargetSlice(Texture2DArray texture, int width, int height, int slice = 0)
                : this(texture, texture.Texture, width, height, slice)
            {
            }

            public Texture Texture { get; }
            public int Width { get; }
            public int Height { get; }
            
            internal int GLTexture { get; }
            public int Slice { get; }
        }
        private readonly int _fbo;
        public int Width { get; }
        public int Height { get; }

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
        public RenderTargetBinding(Texture[] textures)
            : this(CreateSlices(textures))
        {
            
        }
        public RenderTargetBinding(Texture2D[] textures)
            : this(CreateSlices(textures))
        {
            
        }
        public RenderTargetBinding(TextureArray textures)
            : this(CreateSlices(textures))
        {
            
        }
        public RenderTargetBinding(RenderTargetSlice[] slices)
            : this(true, slices)
        {
            
        }
        public RenderTargetBinding(Texture2D depthTexture, RenderTargetSlice[] slices)
            : this((uint)depthTexture.Texture, false, depthTexture.Width, depthTexture.Height, slices)
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
        public RenderTargetBinding(bool generateDepthBuffer, params RenderTargetSlice[] slices)
            : this(GenerateDepthBuffer(generateDepthBuffer, slices), true, slices[0].Width, slices[0].Height, slices)
        {

        }

        private RenderTargetBinding(uint depth, bool isBuffer, int width, int height, params RenderTargetSlice[] slices)
        {
            _depthBuffer = isBuffer ? depth : 0;
            Width = width;
            Height = height;
            _fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

            if (isBuffer)
            {
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depth);
            }
            else
            {
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depth, 0);
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
                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, slices[i].GLTexture, 0);
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