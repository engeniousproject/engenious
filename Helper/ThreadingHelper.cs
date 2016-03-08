using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace engenious
{
    internal static class ThreadingHelper
    {
        private static GLSynchronizationContext sync;
        private static int uiThreadId;
        private static System.Threading.Thread uiThread;
        private static IGraphicsContext context;
        private static OpenTK.Platform.IWindowInfo windowInfo;

        static ThreadingHelper()
        {
            uiThread = System.Threading.Thread.CurrentThread;
            uiThreadId = uiThread.ManagedThreadId;
            System.Threading.SynchronizationContext.SetSynchronizationContext(sync = new GLSynchronizationContext());
            //System.Threading.Thread.CurrentThread.Syn
        }

        public static void Initialize(OpenTK.Platform.IWindowInfo windowInfo, int major, int minor, GraphicsContextFlags contextFlags)
        {
            //GraphicsContextFlags flags = GraphicsContextFlags.
            ThreadingHelper.context = new GraphicsContext(GraphicsMode.Default, windowInfo, major, minor, contextFlags);
            ThreadingHelper.windowInfo = windowInfo;
            //ThreadingHelper.context.LoadAll();

        }

        public static void RunUIThread()
        {
            sync.RunOnCurrentThread();
        }

        public static bool IsOnUIThread()
        {
            return uiThreadId == System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        internal static void BlockOnUIThread(Action action, bool needsUI = false, bool isCheck = false)
        {
            /*if (action == null)
                throw new ArgumentNullException("action");*/

            // If we are already on the UI thread, just call the action and be done with it
            if (IsOnUIThread())
            {
                action();
                return;
            }

            lock (context)
            {
               
                if (needsUI)
                {
                    sync.Post(new System.Threading.SendOrPostCallback(delegate(object state)
                            {
                                action();
                            }), null);
                    return;
                }
                try
                {
                    context.MakeCurrent(windowInfo);
                }
                catch (Exception ex)
                {
                }

                action();

                GL.Flush();

                try
                {
                    context.MakeCurrent(null);
                }
                catch (Exception ex)
                {
                }
            }

        }
    }
}

