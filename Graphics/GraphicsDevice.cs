using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace engenious.Graphics
{
    public class GraphicsDevice:IDisposable
    {
        private BlendState blendState;
        private DepthStencilState depthStencilState;
        private RasterizerState rasterizerState;
        private Rectangle scissorRectangle;
        private Viewport viewport;
        private OpenTK.Graphics.IGraphicsContext context;

        static void DebugCallback(DebugSource source, DebugType type, int id,
                                  DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            //string messageStr = Marshal.PtrToStringAnsi (message);

        }

        public GraphicsDevice(OpenTK.Graphics.IGraphicsContext context)
        {
            this.context = context;
#if DEBUG
            this.context.ErrorChecking = true;
            //GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);
            //GL.Enable(EnableCap.DebugOutputSynchronous);
            //GL.Enable(EnableCap.DebugOutput);

#endif

            this.BlendState = BlendState.AlphaBlend;
            this.DepthStencilState = new DepthStencilState();
            this.RasterizerState = RasterizerState.CullNone;
            this.SamplerStates = new SamplerStateCollection(this);
            GL.Enable(EnableCap.Blend);


            Textures = new TextureCollection();
            //TODO: samplerstate
        }

        public void Clear(ClearBufferMask mask, System.Drawing.Color color)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.Clear(mask);
                    GL.ClearColor(color);
                });
        }

        public void Clear(Color color)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.ClearColor(color.R, color.G, color.B, color.A);
                });
        }

        public void Present()
        {
            context.SwapBuffers();
        }

        public Viewport Viewport
        {
            get
            {
                return viewport;
            }
            set
            {
                if (viewport.Bounds != value.Bounds)
                {
                    ThreadingHelper.BlockOnUIThread(() =>
                        {
                            GL.Viewport(viewport.X, viewport.Y, viewport.Width, viewport.Height);
                            GL.Scissor(scissorRectangle.X, Viewport.Height - scissorRectangle.Bottom, scissorRectangle.Width, scissorRectangle.Height);
                        });
                    viewport = value;
                }
            }
        }

        [Obsolete("Do not use this function")]
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount)
        {
            IVertexType tp = Activator.CreateInstance<T>() as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
            DrawUserPrimitives(primitiveType, vertexData, vertexOffset, primitiveCount, tp.VertexDeclaration);
        }

        [Obsolete("Do not use this function")]
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount, VertexDeclaration vertexDeclaration)
        {

            VertexBuffer old = VertexBuffer;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    VertexBuffer current = new VertexBuffer(this, vertexDeclaration, vertexData.Length);

                    this.VertexBuffer = current;

                    DrawPrimitives(primitiveType, vertexOffset, primitiveCount);

                    this.VertexBuffer = old;

                    current.Dispose();
                });
        }

        [Obsolete("Do not use this function")]
        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount)
        {
            IVertexType tp = Activator.CreateInstance<T>() as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
            VertexBuffer old = VertexBuffer;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    VertexBuffer current = new VertexBuffer(this, tp.VertexDeclaration, vertexData.Length);

                    this.VertexBuffer = current;

                    VertexBuffer.vao.Bind();
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                    GL.DrawElements((OpenTK.Graphics.OpenGL4.PrimitiveType)primitiveType, primitiveCount * 3, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedShort, indexData);

                    this.VertexBuffer = old;

                    current.Dispose();
                });
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    VertexBuffer.vao.Bind();


                    GL.DrawArrays((OpenTK.Graphics.OpenGL4.PrimitiveType)primitiveType, startVertex, primitiveCount * 3);
                });
        }

        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    VertexBuffer.vao.Bind();
                    IndexBuffer.Bind();

                    GL.DrawElements((OpenTK.Graphics.OpenGL4.PrimitiveType)primitiveType, primitiveCount * 3, (OpenTK.Graphics.OpenGL4.DrawElementsType)IndexBuffer.IndexElementSize, IntPtr.Zero);

                });
        }

        public void SetRenderTarget(RenderTarget2D target)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    if (target == null)
                    {
                        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                    }
                    else
                    {
                        target.BindFBO();
                    }
                });
        }

        public TextureCollection Textures{ get; private set; }

        public BlendState BlendState
        {
            get
            {
                return blendState;
            }
            set
            {
                if (blendState != value)
                {
                    
                    blendState = value == null ? BlendState.AlphaBlend : value;
                    ThreadingHelper.BlockOnUIThread(() =>
                        {
                            //TODO:apply more?
                            GL.BlendFuncSeparate((OpenTK.Graphics.OpenGL4.BlendingFactorSrc)blendState.ColorSourceBlend, (OpenTK.Graphics.OpenGL4.BlendingFactorDest)blendState.ColorDestinationBlend, (OpenTK.Graphics.OpenGL4.BlendingFactorSrc)blendState.AlphaSourceBlend, (OpenTK.Graphics.OpenGL4.BlendingFactorDest)blendState.AlphaDestinationBlend);
                            GL.BlendEquationSeparate((OpenTK.Graphics.OpenGL4.BlendEquationMode)blendState.ColorBlendFunction, (OpenTK.Graphics.OpenGL4.BlendEquationMode)blendState.AlphaBlendFunction);
                        });
                }
            }
        }

        public DepthStencilState DepthStencilState
        {
            get
            {
                return depthStencilState;
            }
            set
            {
                if (depthStencilState != value)
                {
                    depthStencilState = value == null ? DepthStencilState.Default : value;
                    ThreadingHelper.BlockOnUIThread(() =>
                        {
                            if (depthStencilState.DepthBufferEnable)
                                GL.Enable(EnableCap.DepthTest);
                            else
                                GL.Disable(EnableCap.DepthTest);
                        });
                    //TODO:apply more
                }
            }
        }

        public RasterizerState RasterizerState
        {
            get
            {
                return rasterizerState;
            }
            set
            {
                if (rasterizerState != value)
                {
                    rasterizerState = value == null ? RasterizerState.CullNone : value;
                    //TODO:apply more
                    ThreadingHelper.BlockOnUIThread(() =>
                        {
                            GL.CullFace((OpenTK.Graphics.OpenGL4.CullFaceMode)rasterizerState.CullMode);

                            GL.PolygonMode((MaterialFace)rasterizerState.CullMode, (OpenTK.Graphics.OpenGL4.PolygonMode)rasterizerState.FillMode);

                            if (rasterizerState.MultiSampleAntiAlias)
                                GL.Enable(EnableCap.Multisample);
                            else
                                GL.Disable(EnableCap.Multisample);

                            if (rasterizerState.ScissorTestEnable)
                                GL.Enable(EnableCap.ScissorTest);
                            else
                                GL.Disable(EnableCap.ScissorTest);
                        });
                }
            }
        }

        public SamplerStateCollection SamplerStates
        {
            get;
            private set;
        }

        public Rectangle ScissorRectangle
        {
            get
            {
                return scissorRectangle;
            }
            set
            {

                if (scissorRectangle != value)
                {
                    scissorRectangle = value;
                    ThreadingHelper.BlockOnUIThread(() =>
                        {
                            GL.Scissor(scissorRectangle.X, Viewport.Height - scissorRectangle.Bottom, scissorRectangle.Width, scissorRectangle.Height);
                        });
                    //GL.Scissor(scissorRectangle.X, scissorRectangle.Y, scissorRectangle.Width, -scissorRectangle.Height);
                }
               

            }
        }

        public VertexBuffer VertexBuffer{ get; set; }

        public IndexBuffer IndexBuffer{ get; set; }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}

