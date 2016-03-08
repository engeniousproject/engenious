using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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


        DebugProc DebugCallbackInstance = DebugCallback;

        static void DebugCallback(DebugSource source, DebugType type, int id,
                                  DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            string msg = Marshal.PtrToStringAnsi(message);
            Console.WriteLine("[GL] {0}; {1}; {2}; {3}; {4}",
                source, type, id, severity, msg);
        }


        Dictionary<string, bool> extensions = new Dictionary<string, bool>();

        public GraphicsDevice(OpenTK.Graphics.IGraphicsContext context)
        {
            this.context = context;
            
#if DEBUG
            int count = GL.GetInteger(GetPName.NumExtensions);
            for (int i = 0; i < count; i++)
            {
                string extension = GL.GetString(StringNameIndexed.Extensions, i);
                extensions.Add(extension, true);
            }
            if (extensions.ContainsKey("GL_ARB_debug_output"))
            {
                this.context.ErrorChecking = true;
                //GL.Enable(EnableCap.DebugOutput);
                //GL.Enable(EnableCap.DebugOutputSynchronous);
                //GL.DebugMessageCallback(DebugCallbackInstance, IntPtr.Zero);
            }


#endif



            Textures = new TextureCollection();
            CheckError();
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

        internal void CheckError()
        {
            //return;
            #if DEBUG
            return;//TODO:
            var frame = new System.Diagnostics.StackTrace(true).GetFrame(1);
            ErrorCode code = ErrorCode.InvalidValue;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    code = GL.GetError();
                    if (code != ErrorCode.NoError)
                    {
                        
                        string filename = frame.GetFileName();
                        int line = frame.GetFileLineNumber();
                        string method = frame.GetMethod().Name;
                        Debug.WriteLine("[GL] " + filename + ":" + method + " - " + line.ToString() + ":" + code.ToString());
                    }
                }, true, true);
            #endif
        }

        public void Clear(Color color)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.ClearColor(color.R, color.G, color.B, color.A);
                });
            CheckError();
        }

        public void Present()
        {
            CheckError();
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

        Dictionary<VertexDeclaration,VertexBuffer> userBuffers = new Dictionary<VertexDeclaration, VertexBuffer>();

        [Obsolete("Do not use this function")]
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount)where T : struct
        {
            IVertexType tp = Activator.CreateInstance<T>() as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
            DrawUserPrimitives(primitiveType, vertexData, vertexOffset, primitiveCount, tp.VertexDeclaration);
            CheckError();
        }

        [Obsolete("Do not use this function")]
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount, VertexDeclaration vertexDeclaration)  where T : struct
        {
            VertexBuffer old = VertexBuffer;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    VertexBuffer current;
                    if (!userBuffers.TryGetValue(vertexDeclaration, out current))
                    {
                        current = new VertexBuffer(this, vertexDeclaration, vertexData.Length);
                        userBuffers.Add(vertexDeclaration, current);
                    }
                    else if (current.VertexCount < vertexData.Length)
                    {
                        if (current != null && !current.IsDisposed)
                            current.Dispose();
                        current = new VertexBuffer(this, vertexDeclaration, vertexData.Length); 
                        userBuffers[vertexDeclaration] = current;
                    }

                    current.SetData<T>(vertexData);

                    this.VertexBuffer = current;

                    DrawPrimitives(primitiveType, vertexOffset, primitiveCount);
                });
            this.VertexBuffer = old;
            CheckError();
        }

        [Obsolete("Do not use this function")]
        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount)
        {
            return;
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
            CheckError();
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    VertexBuffer.vao.Bind();


                    GL.DrawArrays((OpenTK.Graphics.OpenGL4.PrimitiveType)primitiveType, startVertex, primitiveCount * 3);
                });
            CheckError();
        }

        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount)
        {
            CheckError();
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    CheckError();
                    if (VertexBuffer.Bind())
                    {
                        IndexBuffer.Bind();

                        GL.DrawElements((OpenTK.Graphics.OpenGL4.PrimitiveType)primitiveType, primitiveCount * 3, (OpenTK.Graphics.OpenGL4.DrawElementsType)IndexBuffer.IndexElementSize, IntPtr.Zero);
                    }
                });
            CheckError();
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
            CheckError();
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

                            //GL.PolygonMode((MaterialFace)rasterizerState.CullMode, (OpenTK.Graphics.OpenGL4.PolygonMode)rasterizerState.FillMode);

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
            internal set;
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

