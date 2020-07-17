using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace engenious.Helper
{
    internal static class Execute
    {
        internal struct UiExecutor : IDisposable
        {
            internal bool WasOnUiThread;
            internal bool LockAcquired;
            public void Dispose()
            {
                if (WasOnUiThread) return;
                try
                {
                    GL.Flush();
                    GL.Finish();
                    try
                    {
                        ThreadingHelper.Context.MakeCurrent(null);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                finally
                {
                    if (LockAcquired)
                        Monitor.Exit(ThreadingHelper.Context);
                }
            }
        }

        public static UiExecutor OnUiContext
        {
            get
            {
                var ex = new UiExecutor();

                if (ThreadingHelper.UiThreadId == Thread.CurrentThread.ManagedThreadId)
                {
                    ex.WasOnUiThread = true;
                    return ex;
                }

                ex.WasOnUiThread = false;
                
                Monitor.Enter(ThreadingHelper.Context, ref ex.LockAcquired);

                try
                {
                    if(!ThreadingHelper.Context.IsCurrent)
                        ThreadingHelper.Context.MakeCurrent(ThreadingHelper.WindowInfo);
                }
                catch (Exception)
                {
                    if (ex.LockAcquired)
                    {
                        Monitor.Exit(ThreadingHelper.Context);
                        ex.LockAcquired = false;
                    }
                }

                return ex;
            }
        }
    }

    internal static class ThreadingHelper
    {
        private static readonly GlSynchronizationContext Sync;
        internal static int UiThreadId;
        internal static IGraphicsContext Context;
        internal static IWindowInfo WindowInfo;

        static ThreadingHelper()
        {
            var uiThread = Thread.CurrentThread;
            UiThreadId = uiThread.ManagedThreadId;
            SynchronizationContext.SetSynchronizationContext(Sync = new GlSynchronizationContext());
            //System.Threading.Thread.CurrentThread.Syn
        }

        public static void Initialize(IGraphicsContext shareContext,IWindowInfo windowInfo, int major, int minor, GraphicsContextFlags contextFlags)
        {
            //GraphicsContextFlags flags = GraphicsContextFlags.
            //Context = new GraphicsContext(null, null, major, minor, contextFlags);
            Context = new GraphicsContext(shareContext?.GraphicsMode, windowInfo, major, minor, contextFlags);
            Context.MakeCurrent(windowInfo);
   
            //((IGraphicsContextInternal) Context).LoadAll();
            WindowInfo = windowInfo;
        }

        public static void RunUiThread()
        {
            Sync.RunOnCurrentThread();
        }

        public static bool IsOnUiThread()
        {
            return UiThreadId == Thread.CurrentThread.ManagedThreadId;
        }

        internal static void OnUiThread(SendOrPostCallback callback, object obj)
        {
            if (IsOnUiThread())
                callback(obj);
            else
                Sync.Post(callback,obj);
        }
    }
}

