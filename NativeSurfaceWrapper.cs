using System;
using System.ComponentModel;
using OpenToolkit.Windowing.Common;

namespace engenious
{
    public class NativeSurfaceWrapper : IRenderingSurface
    {
        
        public NativeSurfaceWrapper(INativeWindow windowInfo, ContextFlags contextFlags)
        {
            //TODO implement
            throw new NotImplementedException();
            //WindowInfo = windowInfo;
            //Context = new GraphicsContext(GraphicsMode.Default, WindowInfo,0,0, contextFlags);
        }

        /// <inheritdoc />
        public INativeWindow WindowInfo { get; }

        public IGraphicsContext Context { get; }
        
        /// <inheritdoc />
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Point PointToScreen(Point pt)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Point PointToClient(Point pt)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Vector2 Vector2ToScreen(Vector2 pt)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Vector2 Vector2ToClient(Vector2 pt)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Rectangle ClientRectangle { get; set; }

        /// <inheritdoc />
        public Size ClientSize { get; set; }

        /// <inheritdoc />
        public bool Focused { get; }

        /// <inheritdoc />
        public bool CursorVisible { get; set; }

        public bool CursorGrabbed { get; set; }

        /// <inheritdoc />
        public bool Visible { get; set; }

        /// <inheritdoc />
        public IntPtr Handle => throw new NotSupportedException();


        /// <inheritdoc />
        public event Action<FrameEventArgs> RenderFrame;

        /// <inheritdoc />
        public event Action<FrameEventArgs> UpdateFrame;

        /// <inheritdoc />
        public event Action<CancelEventArgs> Closing;

        /// <inheritdoc />
        public event Action<FocusedChangedEventArgs> FocusedChanged;

        /// <inheritdoc />
        public event Action<TextInputEventArgs> KeyPress;

        /// <inheritdoc />
        public event Action<ResizeEventArgs> Resize;

        /// <inheritdoc />
        public event Action Load;

        /// <inheritdoc />
        public event Action<MouseWheelEventArgs> MouseWheel;
    }
}