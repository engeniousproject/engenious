using System;
using OpenTK.Graphics.OpenGL4;

namespace engenious.Graphics
{
    public class RenderTarget2D : Texture2D
    {
        int fbo;
        int depth;

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, PixelInternalFormat surfaceFormat)
            : base(graphicsDevice, width, height, surfaceFormat, PixelFormat.Rgba)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    fbo = GL.GenFramebuffer();

                    GL.GenRenderbuffers(1, out depth);

                    GL.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, depth);
                    GL.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage)All.DepthComponent32, width, height);

                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
                    //GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Rgba8, width, height);
                    //GL.FramebufferParameter(FramebufferTarget.Framebuffer, FramebufferDefaultParameter.FramebufferDefaultWidth, width);
                    //GL.FramebufferParameter(FramebufferTarget.Framebuffer, FramebufferDefaultParameter.FramebufferDefaultHeight, height);

                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);

                    GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depth);
                    //GL.DrawBuffers(1, new DrawBuffersEnum[]{ DrawBuffersEnum.ColorAttachment0,DrawBuffersEnum.});
                    ErrorHandling();
                });
        }

        internal void BindFBO()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
        }

        private void ErrorHandling()
        {
            switch (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer))
            {
                case FramebufferErrorCode.FramebufferComplete:
                    break;// The framebuffer is complete and valid for rendering.
                case FramebufferErrorCode.FramebufferIncompleteAttachment:
                    throw new ArgumentException("FBO: One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachment:
                    throw new ArgumentException("FBO: There are no attachments.");
            /* case  FramebufferErrorCode.GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT: 
                 {
                     throw new ArgumentException("FBO: An object has been attached to more than one attachment point.");
                     break;
                 }*/
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                    throw new ArgumentException("FBO: Attachments are of different size. All attachments must have the same width and height.");
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                    throw new ArgumentException("FBO: The color attachments have different format. All color attachments must have the same format.");
                case FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                    throw new ArgumentException("FBO: An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                case FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                    throw new ArgumentException("FBO: The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
                case FramebufferErrorCode.FramebufferUnsupportedExt:
                    throw new ArgumentException("FBO: This particular FBO configuration is not supported by the implementation.");
                default:
                    throw new ArgumentException("FBO: Status unknown. (yes, this is really bad.)");
            }
        }

        public override void Dispose()
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.DeleteFramebuffer(fbo);
                });
            base.Dispose();
        }
    }
}

