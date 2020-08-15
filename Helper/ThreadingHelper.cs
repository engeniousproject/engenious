using System;
using System.Collections.Generic;
using System.Threading;
using OpenToolkit.Graphics;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using Monitor = System.Threading.Monitor;

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
                        ThreadingHelper.Context.MakeNoneCurrent();
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

                if (ThreadingHelper.UiThreadId == Thread.CurrentThread.ManagedThreadId || ThreadingHelper.Context.IsCurrent)
                {
                    ex.WasOnUiThread = true;
                    return ex;
                }

                ex.WasOnUiThread = false;
                
                Monitor.Enter(ThreadingHelper.Context, ref ex.LockAcquired);

                try
                {
                    if(!ThreadingHelper.Context.IsCurrent)
                        ThreadingHelper.Context.MakeCurrent();
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

    public static class ThreadingHelper
    {
        private static readonly GlSynchronizationContext Sync;
        internal static int UiThreadId;
        internal static IGraphicsContext Context;

        static ThreadingHelper()
        {
            var uiThread = Thread.CurrentThread;
            UiThreadId = uiThread.ManagedThreadId;
            SynchronizationContext.SetSynchronizationContext(Sync = new GlSynchronizationContext());
            //System.Threading.Thread.CurrentThread.Syn
        }

        public static void Initialize(IGraphicsContext shareContext, NativeWindow windowInfo)
        {
            //GraphicsContextFlags flags = GraphicsContextFlags.
            //Context = new GraphicsContext(null, null, major, minor, contextFlags);
            
            Context = windowInfo?.Context;
            Context?.MakeCurrent();
   
            //((IGraphicsContextInternal) Context).LoadAll();
        }

        public static void RunUiThread()
        {
            Sync.RunOnCurrentThread();
        }

        public static bool IsOnUiThread()
        {
            return UiThreadId == Thread.CurrentThread.ManagedThreadId;
        }

        public static void OnUiThread(SendOrPostCallback callback, object obj)
        {
            if (IsOnUiThread())
                callback(obj);
            else
                Sync.Post(callback,obj);
        }
    }
}

