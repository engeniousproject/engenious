using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using engenious.Helper;
using engenious.Utility;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

namespace engenious.Graphics
{
    /// <summary>
    /// The graphics device for rendering on the GPU.
    /// </summary>
    public class GraphicsDevice : IDisposable
    {
        private BlendState? _blendState;
        private DepthStencilState? _depthStencilState;
        private RasterizerState? _rasterizerState;
        private Rectangle _scissorRectangle;
        private Viewport _viewport;
        internal readonly IGraphicsContext Context;

        private Thread _graphicsThread;

        private EffectPass? _effectPass;
        private VertexBuffer? _vertexBuffer;
        private IndexBuffer? _indexBuffer;
        

#if DEBUG
        DebugProc? DebugCallbackInstance;
#endif

        static void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {

            Console.WriteLine("[GL] {0}; {1}; {2}; ", id, severity, Marshal.PtrToStringAnsi(message));
            if (severity >= DebugSeverity.DebugSeverityHigh && severity <= DebugSeverity.DebugSeverityHigh)
            {
                var s = new StackTrace(1, true);
                Console.WriteLine(s.ToString());
            }
            
        }

        /// <summary>
        /// Gets whether the calling thread matches the current graphics thread.
        /// </summary>
        public bool IsOnGraphicsThread
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Thread.CurrentThread == _graphicsThread;

            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ValidateUiGraphicsThread()
        {
            
#if DEBUG
            if (!IsOnGraphicsThread)
            {
                ThrowHelper.ThrowNotOnGraphicsThreadException();
            }
#endif
        }

        internal readonly IGame Game;
        internal readonly HashSet<string> Extensions = new HashSet<string>();
        internal readonly string? DriverVendor;
        internal readonly Version? DriverVersion;
        internal readonly Version? GlslVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsDevice"/> class.
        /// </summary>
        /// <param name="game">The base game this <see cref="GraphicsDevice"/> is used for.</param>
        /// <param name="context">The graphics context of this device.</param>
        public GraphicsDevice(IGame game, IGraphicsContext context)
        {
            _graphicsThread = Thread.CurrentThread;
            Context = context;
            Context.MakeCurrent();
            Game = game;

            GL.GetInteger(GetPName.NumExtensions, out var count);
            for (var i = 0; i < count; i++)
            {
                var extension = GL.GetString(StringNameIndexed.Extensions, i);
                Extensions.Add(extension);
            }

            (DriverVendor, DriverVersion, GlslVersion) = ReadOpenGlVersion();
#if DEBUG
            if (Extensions.Contains("GL_ARB_debug_output"))
            {
                //_context.ErrorChecking = true;
                GL.Enable(EnableCap.DebugOutput);
                GL.Enable(EnableCap.DebugOutputSynchronous);
                CheckError();
                DebugCallbackInstance = DebugCallback;
                //var ptr = Marshal.GetFunctionPointerForDelegate(DebugCallbackInstance);
                //var c = Marshal.GetDelegateForFunctionPointer<DebugProc>(ptr);
                
                GL.DebugMessageCallback(DebugCallbackInstance, IntPtr.Zero);
                GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare,
                    DebugSeverityControl.DontCare, 0, new int[0], true);
                GL.DebugMessageInsert(DebugSourceExternal.DebugSourceApplication, DebugType.DebugTypeMarker, 0, DebugSeverity.DebugSeverityNotification, -1, "Debug output enabled");
            }
#endif

            Capabilities = new GraphicsCapabilities(this);

            Textures = new TextureCollection();

            Debug = new DebugRendering(this);

            UiThread = new GraphicsThread(this);

            _fenceThread = new GraphicsThread(Context);

            Threads = new List<GraphicsThread>();
            // Make current again as GraphicsThread creates a new context.
            Context.MakeCurrent();
            
            CheckError();
            //TODO: samplerstate
        }

        internal void RemoveFromUiThread()
        {
            Context.MakeNoneCurrent();
        }

        internal void SwitchUiThread()
        {
            _graphicsThread = Thread.CurrentThread;
            UiThread = new GraphicsThread(this);
            Context.MakeCurrent();
        }

        /// <summary>
        /// Adds a new graphics thread to the <see cref="Threads"/> list.
        /// </summary>
        public void AddGraphicsThread()
        {
            Threads.Add(new GraphicsThread(Context));
        }
        
        /// <summary>
        /// Gets a list of the associated secondary graphics threads.
        /// </summary>
        public List<GraphicsThread> Threads { get; }

        private static void WaitForFence(IntPtr fence)
        {
            while (GL.ClientWaitSync(fence, ClientWaitSyncFlags.None, 100000000) ==
                   WaitSyncStatus.TimeoutExpired)
                Thread.Yield();
            GL.DeleteSync(fence);
        }
        /// <summary>
        /// Creates a new asynchronous fence which can be waited on using the returned <see cref="AutoResetEvent"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="AutoResetEvent"/> with which one can wait for the fence to be reached.
        /// <remarks>This event gets invalid as soon as it was fired once and cannot be reused.</remarks>
        /// </returns>
        public unsafe AutoResetEvent? CreateFenceAsync()
        {
            ValidateUiGraphicsThread();
            
            var fence = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);
            GL.Flush();

            return _fenceThread.QueueWork(CapturingDelegate.Create(&WaitForFence, fence));
        }
        /// <summary>
        /// Creates a fence pointer which can be used using OpenTK GL.[Client]WaitSync.
        /// </summary>
        /// <returns>The <see cref="IntPtr"/> pointing to the fence object.</returns>
        public unsafe IntPtr CreateFence()
        {
            ValidateUiGraphicsThread();
            
            var fence = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);
            GL.Flush();

            return fence;
        }

        private readonly GraphicsThread _fenceThread;
        internal void SynchronizeUiThread()
        {
            UiThread.RunThread();
        }

        private (string? driverVendor, Version? driverVersion, Version? glslVersion) ReadOpenGlVersion()
        {
            string? driverVendor = null;
            Version? driverVersion = null, glslVersion = null;
            try
            {
                string fullVersion = GL.GetString(StringName.Version);
                var splt = fullVersion.Split(new []{' '}, StringSplitOptions.None);
                string? versionString = splt.FirstOrDefault();
                if (versionString != null)
                    driverVersion = new Version(versionString);
                driverVendor = GL.GetString(StringName.Vendor);
                fullVersion = GL.GetString(StringName.ShadingLanguageVersion);
                versionString = fullVersion.Split(new []{' '}, StringSplitOptions.None).FirstOrDefault();
                if (versionString != null)
                    glslVersion = new Version(versionString);

            }
            catch (ArgumentException)
            {
                if (!Extensions.Contains("VERSION_1_2"))
                {
                    driverVersion = new Version(1,0);
                    glslVersion = new Version(1,0);//throw new Exception(string.Join("\r\n\t",Extensions.Keys)+$"\r\ncan't parse version: {versionString} fullversion{fullVersion}", ex);
                    return (driverVendor, driverVersion, glslVersion);
                }

                driverVersion = new Version(GL.GetInteger(GetPName.MajorVersion), GL.GetInteger(GetPName.MinorVersion));
                if (driverVersion.Major == 2)
                {
                    glslVersion = driverVersion.Minor == 0 ? new Version(1, 10) : new Version(1, 20);
                }
                else if (driverVersion.Major == 3 && driverVersion.Minor <= 2)
                {
                    glslVersion = new Version(1, 30 + 10 * driverVersion.Minor);
                }
                else if (driverVersion.Major >= 3)
                {
                    glslVersion = new Version(driverVersion.Major, driverVersion.Minor * 10);
                }
                else
                {
                    glslVersion = new Version(1, 0);
                }
            }
            
            return (driverVendor, driverVersion, glslVersion);
        }

        /// <summary>
        /// Clears the backbuffer using a clearing mask.
        /// </summary>
        /// <param name="mask">A value indicating which buffers to clear.</param>
        public void Clear(ClearBufferMask mask)
        {
            ValidateUiGraphicsThread();
            
            GL.Clear((OpenTK.Graphics.OpenGL.ClearBufferMask) mask);
        }

        /// <summary>
        /// Clears the backbuffer using a clearing mask with a specified color.
        /// </summary>
        /// <param name="mask">A value indicating which buffers to clear.</param>
        /// <param name="color">A value indicating the color with which to clear the color buffer.</param>
        public void Clear(ClearBufferMask mask, System.Drawing.Color color)
        {
            ValidateUiGraphicsThread();

            GL.ClearColor(color);
            GL.Clear((OpenTK.Graphics.OpenGL.ClearBufferMask) mask);
        }

        internal void CheckError()
        {
            //return;
#if DEBUG
            ValidateUiGraphicsThread();
            var code = GL.GetError();
            if (code == ErrorCode.NoError)
                return;
            var frame = new StackTrace(true).GetFrame(1);
            if (frame == null)
                return;
            string? filename = frame.GetFileName();
            int line = frame.GetFileLineNumber();
            string? method = frame.GetMethod()?.Name;
            System.Diagnostics.Debug.WriteLine("[GL] " + filename + ":" + method + " - " + line.ToString() + ":" + code.ToString());
            /*var frame = new System.Diagnostics.StackTrace(true).GetFrame(1);
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
                }, true);*/
#endif
        }

        /// <summary>
        /// Clears the color backbuffer  with a specified color.
        /// </summary>
        /// <param name="color">A value indicating the color with which to clear the color buffer.</param>
        public void Clear(Color color)
        {
            ValidateUiGraphicsThread();
            GL.ClearColor(color.R, color.G, color.B, color.A);
            GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit |
                     OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit);

            CheckError();
        }

        /// <summary>
        /// Swaps the backbuffer with the front buffer to present the renderings on the screen.
        /// </summary>
        public void Present()
        {
            ValidateUiGraphicsThread();
            CheckError();
            Context.SwapBuffers();
        }

        /// <summary>
        /// Gets the graphics capabilities of this <see cref="GraphicsDevice"/>.
        /// </summary>
        public GraphicsCapabilities Capabilities { get; }

        /// <summary>
        /// Gets the uis' <see cref="GraphicsThread"/> used for synchronization.
        /// </summary>
        public GraphicsThread UiThread { get; private set; }

        /// <summary>
        /// Gets or sets the current <see cref="Viewport"/>.
        /// </summary>
        public Viewport Viewport
        {
            get => _viewport;
            set
            {
                if (_viewport.Bounds != value.Bounds)
                {
                    _viewport = value;
                    ValidateUiGraphicsThread();
                    //GL.Viewport(viewport.X, game.Window.ClientSize.Height - viewport.Y - viewport.Height, viewport.Width, viewport.Height);
                    GL.Scissor(_scissorRectangle.X, Viewport.Height - _scissorRectangle.Bottom,
                        _scissorRectangle.Width, _scissorRectangle.Height);
                    GL.Viewport(_viewport.X, _viewport.Y, _viewport.Width, _viewport.Height);

                }
            }
        }

        private readonly Dictionary<VertexDeclaration, VertexBuffer> _userBuffers = new Dictionary<VertexDeclaration, VertexBuffer>();

        /// <summary>
        /// Renders passed in vertex buffer data.
        /// </summary>
        /// <param name="primitiveType">The <see cref="PrimitiveType"/> to use for rendering.</param>
        /// <param name="vertexData">The vertex data to use for rendering.</param>
        /// <param name="vertexOffset">The vertex offset to start rendering at.</param>
        /// <param name="primitiveCount">The number of primitives to render.</param>
        /// <typeparam name="T">The vertex data type.</typeparam>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T"/> is not a valid vertex type.</exception>
        [Obsolete("Do not use this function")]
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset,
            int primitiveCount) where T : unmanaged
        {
            var tp = Activator.CreateInstance<T>() as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
            DrawUserPrimitives(primitiveType, vertexData, vertexOffset, primitiveCount, tp.VertexDeclaration);
            CheckError();
        }

        /// <summary>
        /// Renders passed in vertex buffer data.
        /// </summary>
        /// <param name="primitiveType">The <see cref="PrimitiveType"/> to use for rendering.</param>
        /// <param name="vertexData">The vertex data to use for rendering.</param>
        /// <param name="vertexOffset">The vertex offset to start rendering at.</param>
        /// <param name="primitiveCount">The number of primitives to render.</param>
        /// <param name="vertexDeclaration">The vertex declaration used for describing the vertex data structure.</param>
        /// <typeparam name="T">The vertex data type.</typeparam>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T"/> is not a valid vertex type.</exception>
        [Obsolete("Do not use this function")]
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset,
            int primitiveCount, VertexDeclaration vertexDeclaration) where T : unmanaged
        {
            var old = VertexBuffer;
            
            ValidateUiGraphicsThread();
            
            if (!_userBuffers.TryGetValue(vertexDeclaration, out var current))
            {
                current = new VertexBuffer(this, vertexDeclaration, vertexData.Length);
                _userBuffers.Add(vertexDeclaration, current);
            }
            else if (current.VertexCount < vertexData.Length)
            {
                if (!current.IsDisposed)
                    current.Dispose();
                current = new VertexBuffer(this, vertexDeclaration, vertexData.Length);
                _userBuffers[vertexDeclaration] = current;
            }

            current.SetData(vertexData);

            VertexBuffer = current;

            DrawPrimitives(primitiveType, vertexOffset, primitiveCount);

            VertexBuffer = old;
            CheckError();
        }

        /// <summary>
        /// Renders passed in vertex buffer and index buffer data.
        /// </summary>
        /// <param name="primitiveType">The <see cref="PrimitiveType"/> to use for rendering.</param>
        /// <param name="vertexData">The vertex data to use for rendering.</param>
        /// <param name="vertexOffset">The vertex offset to start rendering at.</param>
        /// <param name="numVertices">The number of vertices used for rendering</param>
        /// <param name="indexData">The indexing data used for indexing the vertices.</param>
        /// <param name="indexOffset">The index offset to start rendering at.</param>
        /// <param name="primitiveCount">The number of primitives to render.</param>
        /// <typeparam name="T">The vertex data type.</typeparam>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T"/> is not a valid vertex type.</exception>
        [Obsolete("Do not use this function")]
        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset,
            int numVertices, short[] indexData, int indexOffset, int primitiveCount)
        {
            throw new NotImplementedException();
            //TODO:
            /*IVertexType tp = Activator.CreateInstance<T>() as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
            VertexBuffer old = VertexBuffer;
            using (Execute.OnUiContext)
            {
                VertexBuffer current = new VertexBuffer(this, tp.VertexDeclaration, vertexData.Length);

                VertexBuffer = current;

                VertexBuffer.Vao.Bind();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                GL.DrawElements(
                    (OpenTK.Graphics.OpenGL.PrimitiveType) primitiveType,
                    primitiveCount * 3,
                    OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedShort,
                    indexData);

                VertexBuffer = old;

                current.Dispose();
            }
            CheckError();*/
        }

        /// <summary>
        /// Renders primitives using the <see cref="VertexBuffer"/> for vertices.
        /// </summary>
        /// <param name="primitiveType">The <see cref="PrimitiveType"/> to use for rendering.</param>
        /// <param name="startVertex">The vertex to start rendering at.</param>
        /// <param name="vertexCount">The number of vertices to render.</param>
        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int vertexCount)
        {
            if (VertexBuffer == null)
                return;
            VertexBuffer.EnsureVao();
            VertexBuffer.Vao!.Bind();


            GL.DrawArrays((OpenTK.Graphics.OpenGL.PrimitiveType) primitiveType, startVertex, vertexCount);
            CheckError();
        }

        /// <summary>
        /// Renders primitives using the <see cref="VertexBuffer"/> for vertices and <see cref="IndexBuffer"/> for indices.
        /// </summary>
        /// <param name="primitiveType">The <see cref="PrimitiveType"/> to use for rendering.</param>
        /// <param name="baseVertex">The base vertex offset to start indexing at.</param>
        /// <param name="minVertexIndex">Suggestive minimal vertex parameter for more optimal rendering.</param>
        /// <param name="numVertices">The number of vertices used in indexing.</param>
        /// <param name="startIndex">The index to start rendering at.</param>
        /// <param name="primitiveCount">The numbers of primitives to render.</param>
        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex,
            int numVertices, int startIndex, int primitiveCount)
        {
            if (VertexBuffer == null || IndexBuffer == null)
                return;
            CheckError();
            VertexBuffer.EnsureVao();
            if (VertexBuffer.Bind())
            {
                IndexBuffer.Bind();

                GL.DrawElementsBaseVertex((OpenTK.Graphics.OpenGL.PrimitiveType) primitiveType, primitiveCount * 3,
                    (OpenTK.Graphics.OpenGL.DrawElementsType) IndexBuffer.IndexElementSize, IntPtr.Zero, baseVertex);
            }

            CheckError();
        }

        /// <summary>
        /// Renders primitives using the <see cref="VertexBuffer"/> for vertices and <see cref="IndexBuffer"/> for indices.
        /// </summary>
        /// <param name="primitiveType">The <see cref="PrimitiveType"/> to use for rendering.</param>
        /// <param name="baseVertex">The base vertex offset to start indexing at.</param>
        /// <param name="startIndex">The index to start rendering at.</param>
        /// <param name="primitiveCount">The numbers of primitives to render.</param>
        /// <param name="instanceCount">The number of instances to render.</param>
        public void DrawInstancedPrimitives(PrimitiveType primitiveType, int baseVertex, int startIndex,
            int primitiveCount, int instanceCount)
        {
            if (VertexBuffer == null || IndexBuffer == null)
                return;
            VertexBuffer.EnsureVao();
            VertexBuffer.Vao!.Bind();
            GL.DrawElementsInstancedBaseVertex((OpenTK.Graphics.OpenGL.PrimitiveType) primitiveType, primitiveCount * 3,
                (OpenTK.Graphics.OpenGL.DrawElementsType)IndexBuffer.IndexElementSize, IntPtr.Zero, instanceCount,
                baseVertex);
        }

        /// <summary>
        /// Renders primitives using the <see cref="VertexBuffer"/> for vertices and <see cref="IndexBuffer"/> for indices.
        /// </summary>
        /// <param name="primitiveType">The <see cref="PrimitiveType"/> to use for rendering.</param>
        /// <param name="baseVertex">The base vertex offset to start indexing at.</param>
        /// <param name="startIndex">The index to start rendering at.</param>
        /// <param name="primitiveCount">The numbers of primitives to render.</param>
        public void MultiDrawElementsBaseVertex(PrimitiveType primitiveType, int[] baseVertex, int startIndex,
            int[] primitiveCount)
        {
            if (VertexBuffer == null || IndexBuffer == null)
                return;
            VertexBuffer.EnsureVao();
            VertexBuffer.Vao!.Bind();
            GL.MultiDrawElementsBaseVertex((OpenTK.Graphics.OpenGL.PrimitiveType)primitiveType, primitiveCount,
                (OpenTK.Graphics.OpenGL.DrawElementsType)IndexBuffer.IndexElementSize, IntPtr.Zero, primitiveCount.Length,
                baseVertex);
        }

        /// <summary>
        /// Sets the current render target.
        /// </summary>
        /// <param name="target">The render target to render to or <c>null</c> to use the default render target.</param>
        public void SetRenderTarget(RenderTarget2D? target)
        {
            ValidateUiGraphicsThread();
            if (target == null)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                Viewport = new Viewport(new Rectangle(new Point(), Game.RenderingSurface.ClientSize));
            }
            else
            {
                target.BindFbo();
                Viewport = new Viewport(target.Bounds);
                ScissorRectangle = target.Bounds;
            }

            CheckError();
        }

        /// <summary>
        /// Sets the current render target.
        /// </summary>
        /// <param name="target">The render target to render to or <c>null</c> to use the default render target.</param>
        public void SetRenderTarget(RenderTargetBinding? target)
        {
            ValidateUiGraphicsThread();
            if (target == null)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                Viewport = new Viewport(new Rectangle(new Point(), Game.RenderingSurface.ClientSize));
            }
            else
            {
                target.BindFbo();
                Viewport = new Viewport(target.Bounds);
                ScissorRectangle = target.Bounds;
            }

            CheckError();
        }

        /// <summary>
        /// Gets a collection of currently active textures.
        /// </summary>
        public TextureCollection Textures { get; private set; }

        /// <summary>
        /// Gets or sets the current blend state state used for blending.
        /// </summary>
        public BlendState? BlendState
        {
            get => _blendState;
            set
            {
                if (_blendState != value)
                {
                    _blendState?.Unbind();
                    var blendState = value ?? BlendState.AlphaBlend;
                    blendState.Bind(this);
                    _blendState = blendState;
                }
            }
        }

        /// <summary>
        /// Sets the current blend state state used for blending in the specified frame buffer index.
        /// </summary>
        /// <param name="buffer">The draw buffer index to set the blend state for.</param>
        /// <param name="value">The value to set the blend state to.</param>
        public void SetBlendState(int buffer, BlendState? value)
        {
            var blendState = value ?? BlendState.AlphaBlend;
            blendState.Bind(this, buffer);
        }

        /// <summary>
        /// Gets or sets the current depth stencil state.
        /// </summary>
        public DepthStencilState? DepthStencilState
        {
            get => _depthStencilState;
            set
            {
                if (_depthStencilState != value)
                {
                    _depthStencilState?.Unbind();
                    var depthStencilState = value ?? DepthStencilState.Default;
                    depthStencilState.Bind(this);
                    _depthStencilState = depthStencilState;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current rasterizer state.
        /// </summary>
        public RasterizerState? RasterizerState
        {
            get => _rasterizerState;
            set
            {
                if (_rasterizerState != value)
                {
                    _rasterizerState?.Unbind();
                    var rasterizerState = value ?? RasterizerState.CullClockwise;
                    rasterizerState.Bind(this);
                    _rasterizerState = rasterizerState;
                }
            }
        }

        //public SamplerStateCollection SamplerStates
        //{
        //    get;
        //    internal set;
        //}

        /// <summary>
        /// Gets or sets the region outside of which rendering ought to be cropped to.
        /// </summary>
        public Rectangle ScissorRectangle
        {
            get => _scissorRectangle;
            set
            {
                if (_scissorRectangle != value)
                {
                    _scissorRectangle = value;
                    ValidateUiGraphicsThread();
                    GL.Scissor(_scissorRectangle.X, Viewport.Height - _scissorRectangle.Bottom,
                        _scissorRectangle.Width, _scissorRectangle.Height);

                    //GL.Scissor(scissorRectangle.X, scissorRectangle.Y, scissorRectangle.Width, -scissorRectangle.Height);
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the currently active <see cref="engenious.Graphics.EffectPass"/>.
        /// </summary>
        public EffectPass? EffectPass
        {
            get => _effectPass;
            set
            {
                if (_effectPass == value) return;
                _effectPass = value;
                GL.UseProgram(value?.Program ?? 0);
            }
        }
        
        /// <summary>
        /// Gets or sets the currently active <see cref="engenious.Graphics.VertexBuffer"/>.
        /// </summary>
        public VertexBuffer? VertexBuffer
        {
            get => _vertexBuffer;
            set
            {
                if (_vertexBuffer == value) return;
                _vertexBuffer = value;
                value?.Bind();
            }
        }

        /// <summary>
        /// Gets or sets the currently active <see cref="engenious.Graphics.IndexBuffer"/>.
        /// </summary>
        public IndexBuffer? IndexBuffer
        {
            get => _indexBuffer;
            set
            {
                if (_indexBuffer == value) return;
                _indexBuffer = value;
                value?.Bind();
            }
        }

        /// <summary>
        /// Gets a class used for rendering helpful debug polygons(Not high performance).
        /// </summary>
        public DebugRendering Debug { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            UiThread.Dispose();
            _fenceThread.Dispose();
            foreach(var graphicsThread in Threads)
                graphicsThread.Dispose();
        }
    }
}