using System;
using System.ComponentModel;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace engenious
{
    /// <summary>
    /// A wrapper for a native surface.
    /// </summary>
    public class NativeSurfaceWrapper : IRenderingSurface
    {
        
        /// <summary>
        /// Initializes an new instance of the <see cref="NativeSurfaceWrapper"/> class.
        /// </summary>
        /// <param name="windowInfo">The native window info to wrap.</param>
        /// <param name="contextFlags">Thre graphics context flags</param>
        /// <exception cref="NotImplementedException">This class is currently not implemented.</exception>
        public NativeSurfaceWrapper(INativeWindow windowInfo, ContextFlags contextFlags)
        {
            //TODO implement
            WindowInfo = windowInfo;
            Context = windowInfo.Context;
            Focused = false;
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public INativeWindow WindowInfo { get; }

        /// <summary>
        /// Gets the graphics context for this surface wrapper.
        /// </summary>
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

        /// <inheritdoc />
        public bool CursorGrabbed { get; set; }

        /// <inheritdoc />
        public bool Visible { get; set; }

        /// <inheritdoc />
        public IntPtr Handle => throw new NotSupportedException();


        /// <inheritdoc />
        public event Action<FrameEventArgs>? RenderFrame;

        /// <inheritdoc />
        public event Action<FrameEventArgs>? UpdateFrame;

        /// <inheritdoc />
        public event Action<CancelEventArgs>? Closing;

        /// <inheritdoc />
        public event Action<FocusedChangedEventArgs>? FocusedChanged;

        /// <inheritdoc />
        public event Action<TextInputEventArgs>? KeyPress;

        /// <inheritdoc />
        public event Action<ResizeEventArgs>? Resize;

        /// <inheritdoc />
        public event Action? Load;

        /// <inheritdoc />
        public event Action<MouseWheelEventArgs>? MouseWheel;
    }
}