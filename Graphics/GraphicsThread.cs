using System;
using System.Threading;
using engenious.Helper;
using engenious.Utility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace engenious.Graphics
{
    public class GraphicsThread : IDisposable
    {
        private readonly INativeWindow _windowInfo;
        private readonly IGraphicsContext _context;
        private readonly GlSynchronizationContext _sync;

        private readonly Thread _thread;
        private CancellationTokenSource CancellationTokenSource { get; }
        private readonly CancellationToken _cancellationToken;

        private static NativeWindow CreateSharedContext(IGraphicsContext sharedContext)
        {
            var settings = new NativeWindowSettings() {SharedContext = sharedContext as IGLFWGraphicsContext, StartVisible = false};
            var window = new GameWindow(GameWindowSettings.Default, settings);
            window.Context.MakeNoneCurrent();
            return window;
        }
        public GraphicsThread(IGraphicsContext sharedContext)
            : this(CreateSharedContext(sharedContext))
        {
        }

        private GraphicsThread()
        {
            CancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = CancellationTokenSource.Token;
        }

        internal GraphicsThread(GraphicsDevice graphicsDevice)
            : this()
        {
            _windowInfo = graphicsDevice.Game.RenderingSurface.WindowInfo;
            _sync = new GlSynchronizationContext();
            _context = graphicsDevice._context;
            _cancellationToken.Register(() => CancellationTokenSource.Dispose());
            
            SynchronizationContext.SetSynchronizationContext(_sync);
            _thread = Thread.CurrentThread;
        }
        public GraphicsThread(NativeWindow windowInfo)
            : this()
        {
            _windowInfo = windowInfo;
            _sync = new GlSynchronizationContext();
            _context = windowInfo.Context;
            _cancellationToken.Register(() => _sync.CancelWait());
            _thread = new Thread(ThreadQueueLoop);

            _thread.Start();
        }

        private void Init()
        {
            SynchronizationContext.SetSynchronizationContext(_sync);
            
            _context.MakeCurrent();
        }

        private void ThreadQueueLoop()
        {
            Init();

            while (!_cancellationToken.IsCancellationRequested)
            {
                RunThread();
                
                _sync.WaitForWork();
            }
        }
        
        /// <summary>
        /// Runs all queued up work.
        /// </summary>
        internal void RunThread()
        {
            _sync.RunOnCurrentThread();
        }
        
        /// <summary>
        /// Checks whether the current thread is this graphics thread.
        /// </summary>
        /// <returns>Whether the current thread is this graphics thread.</returns>
        public bool IsOnGraphicsThread()
        {
            return _thread == Thread.CurrentThread;
        }

        /// <summary>
        /// Call a callback on the graphics thread.
        /// </summary>
        /// <param name="callback">The callback to call on the graphics thread.</param>
        /// <param name="obj">The object to pass to the callback</param>
        public void QueueWork(SendOrPostCallback callback, object obj)
        {
            if (IsOnGraphicsThread())
                callback(obj);
            else
                _sync.Post(callback,obj);
        }
        
        /// <summary>
        /// Call a callback on the graphics thread.
        /// <remarks>
        /// When <paramref name="passOwnership" /> is <c>true</c> <paramref name="callback"/> can't be reused;
        /// otherwise it needs to be disposed manually.
        /// </remarks>
        /// </summary>
        /// <param name="callback">The callback to call on the graphics thread.</param>
        /// <param name="passOwnership">Whether to pass the ownership to the <see cref="GraphicsThread"/> or not.</param>
        /// <returns>
        /// An <see cref="AutoResetEvent"/> that gets set when the work is done.
        /// <remarks>Do not dispose this and only use it once. After that it will be reused by this API.</remarks>
        /// </returns>
        public AutoResetEvent QueueWork(CapturingDelegate callback, bool passOwnership = true)
        {
            if (IsOnGraphicsThread())
            {
                callback.Call();
                if (passOwnership)
                    callback.Capture.Dispose();
                return null;
            }

            return _sync.Post(callback, passOwnership);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            var token = CancellationTokenSource.Token;
            var cancellation = token.WaitHandle;
            CancellationTokenSource.Cancel();
            cancellation.WaitOne();
            CancellationTokenSource.Dispose();
            _windowInfo.Dispose();
        }
    }
}