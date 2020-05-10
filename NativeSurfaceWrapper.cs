using System;
using System.ComponentModel;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using OpenTK.Platform.X11;
namespace engenious
{
    public class NativeSurfaceWrapper : IRenderingSurface
    {
        
        public NativeSurfaceWrapper(GraphicsMode graphicsMode, IWindowInfo windowInfo, GraphicsContextFlags contextFlags)
        {
            WindowInfo = windowInfo;
            Context = new GraphicsContext(GraphicsMode.Default, WindowInfo,0,0, contextFlags);
        }

        /// <inheritdoc />
        public IWindowInfo WindowInfo { get; }

        public GraphicsContext Context { get; }
        
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
        public Rectangle ClientRectangle { get; set; }

        /// <inheritdoc />
        public Size ClientSize { get; set; }

        /// <inheritdoc />
        public bool Focused { get; }

        /// <inheritdoc />
        public bool CursorVisible { get; set; }

        /// <inheritdoc />
        public bool Visible { get; set; }

        /// <inheritdoc />
        public IntPtr Handle => WindowInfo.Handle;


        /// <inheritdoc />
        public event EventHandler<FrameEventArgs> RenderFrame;

        /// <inheritdoc />
        public event EventHandler<FrameEventArgs> UpdateFrame;

        /// <inheritdoc />
        public event EventHandler<CancelEventArgs> Closing;

        /// <inheritdoc />
        public event EventHandler<EventArgs> FocusedChanged;

        /// <inheritdoc />
        public event EventHandler<KeyPressEventArgs> KeyPress;

        /// <inheritdoc />
        public event EventHandler<EventArgs> Resize;

        /// <inheritdoc />
        public event EventHandler<EventArgs> Load;

        /// <inheritdoc />
        public event EventHandler<MouseWheelEventArgs> MouseWheel;
    }
}