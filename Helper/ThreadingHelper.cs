using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace engenious
{
    internal static class ThreadingHelper
    {
        private static int uiThread;
        private static IGraphicsContext context;
        private static OpenTK.Platform.IWindowInfo windowInfo;

        static ThreadingHelper()
        {
            uiThread = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        public static void Initialize(OpenTK.Platform.IWindowInfo windowInfo)
        {
            ThreadingHelper.context = new GraphicsContext(GraphicsMode.Default, windowInfo);
            ThreadingHelper.windowInfo = windowInfo;
        }

        public static bool IsOnUIThread()
        {
            return uiThread == System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        internal static void BlockOnUIThread(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            // If we are already on the UI thread, just call the action and be done with it
            if (IsOnUIThread())
            {
                action();
                return;
            }


            lock (context)
            {
                // Make the context current on this thread
                context.MakeCurrent(windowInfo);
                // Execute the action
                action();
                // Must flush the GL calls so the texture is ready for the main context to use
                GL.Flush();
                //TODO:GraphicsExtensions.CheckGLError();
                // Must make the context not current on this thread or the next thread will get error 170 from the MakeCurrent call
                context.MakeCurrent(null);
            }

        }
    }
}

