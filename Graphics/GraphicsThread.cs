using System;
using System.Threading;
using engenious.Helper;
using engenious.Utility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace engenious.Graphics
{
    /// <summary>
    /// A thread used for executing graphics commands in a threaded graphics context.
    /// </summary>
    public class GraphicsThread : IDisposable
    {
        private readonly IWindowWrapper _windowInfo;
        private readonly IGraphicsContext _context;
        private readonly GlSynchronizationContext _sync;

        private readonly bool _isIndependentThread;

        private readonly Thread _thread;
        private CancellationTokenSource CancellationTokenSource { get; }
        private readonly CancellationToken _cancellationToken;

        private static IWindowWrapper CreateSharedContext(IGraphicsContext sharedContext)
        {
            var settings = new NativeWindowSettings() {SharedContext = sharedContext as IGLFWGraphicsContext, StartVisible = false};
            var window = new OpenTkWindowWrapper(new GameWindow(GameWindowSettings.Default, settings));
            window.Context.MakeNoneCurrent();
            return window;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsThread"/> class.
        /// </summary>
        /// <param name="sharedContext">The graphics context to share the data with.</param>
        public GraphicsThread(IGraphicsContext sharedContext)
            : this(CreateSharedContext(sharedContext))
        {
        }

        private GraphicsThread(IWindowWrapper windowInfo, IGraphicsContext context, Thread workingThread)
        {
            CancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = CancellationTokenSource.Token;
            _windowInfo = windowInfo;
            _context = context;
            
            _sync = new GlSynchronizationContext();
            _thread = workingThread;
        }

        internal GraphicsThread(GraphicsDevice graphicsDevice)
            : this(graphicsDevice.Game.RenderingSurface.WindowInfo!, graphicsDevice.Context, Thread.CurrentThread)
        {
            _cancellationToken.Register(() =>
            {
                _sync.CancelWait();
            });

            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(_sync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsThread"/> class.
        /// </summary>
        /// <param name="windowInfo">The native window wrapper to create the graphics thread for.</param>
        public GraphicsThread(IWindowWrapper windowInfo)
            : this(windowInfo, windowInfo.Context, null!)
        {
            _cancellationToken.Register(() => _sync.CancelWait());
            _thread = new Thread(ThreadQueueLoop);
            _isIndependentThread = true;

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
        public AutoResetEvent? QueueWork(CapturingDelegate callback, bool passOwnership = true)
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
            if (_isIndependentThread)
                _thread.Join();
            CancellationTokenSource.Dispose();
            _windowInfo.Dispose();
        }
    }
}