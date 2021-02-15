using System;
using System.ComponentModel;
using System.Drawing;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace engenious
{
    /// <summary>
    /// Specifies a window as a game rendering view.
    /// </summary>
    public class Window : IRenderingSurface
    {
        internal readonly IWindowWrapper BaseWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="baseWindow">The base window.</param>
        public Window(IWindowWrapper baseWindow)
        {
            BaseWindow = baseWindow;
        }

        /// <inheritdoc />
        public Point PointToScreen(Point pt)
        {
            return BaseWindow.PointToScreen(pt);
        }

        /// <inheritdoc />
        public Point PointToClient(Point pt)
        {
            return BaseWindow.PointToClient(pt);
        }

        /// <inheritdoc />
        public Vector2 Vector2ToScreen(Vector2 pt)
        {
            var integerPart = new Point((int) pt.X, (int) pt.Y);
            var tmp = PointToScreen(integerPart);
            return  tmp.ToVector2() + pt - integerPart.ToVector2();
        }

        /// <inheritdoc />
        public Vector2 Vector2ToClient(Vector2 pt)
        {
            var integerPart = new Point((int) pt.X, (int) pt.Y);
            var tmp = PointToClient(integerPart);
            return  tmp.ToVector2() + pt - integerPart.ToVector2();
        }

        /// <inheritdoc />
        public Rectangle ClientRectangle{
            get => BaseWindow.ClientRectangle;
            set => BaseWindow.ClientRectangle = value;
        }

        /// <inheritdoc />
        public Size ClientSize{
            get => new Size(BaseWindow.ClientSize.X,BaseWindow.ClientSize.Y);
            set => BaseWindow.Size = new Vector2i(value.Width,value.Height);
        }

        /// <summary>
        /// Gets or sets the <see cref="Icon"/> of this <see cref="Window"/>.
        /// </summary>
        public WindowIcon? Icon { get => BaseWindow.Icon;
            set => BaseWindow.Icon = value;
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Window"/> is visible.
        /// </summary>
        public bool Visible{
            get => BaseWindow.IsVisible;
            set => BaseWindow.IsVisible = value;
        }

        /// <inheritdoc />
        public IntPtr Handle => throw new NotSupportedException();

        /// <inheritdoc />
        public IWindowWrapper WindowInfo => BaseWindow;

        /// <summary>
        /// Gets or sets whether the <see cref="Window"/> is in focus.
        /// </summary>
        public bool Focused => BaseWindow.IsFocused;
        
        /// <summary>
        /// Gets or sets whether the mouse cursor is visible on this <see cref="Window"/>.
        /// </summary>
        public bool CursorVisible{
            get => BaseWindow.CursorVisible;
            set => BaseWindow.CursorVisible = value;
        }

        /// <summary>
        /// Gets or sets whether the mouse cursor is grabbed on this <see cref="Window"/>.
        /// </summary>
        public bool CursorGrabbed
        {
            get => BaseWindow.CursorGrabbed;
            set => BaseWindow.CursorGrabbed = value;
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Window"/> is without a border.
        /// </summary>
        public bool IsBorderless{
            get => BaseWindow.WindowBorder != WindowBorder.Hidden;
            set{
                if (value)
                    BaseWindow.WindowBorder = WindowBorder.Hidden;
                else
                    BaseWindow.WindowBorder = _allowUserResizing ? WindowBorder.Resizable : WindowBorder.Fixed;
            }
        }
        private bool _allowUserResizing;

        /// <summary>
        /// Gets or sets whether the user is able to resize this <see cref="Window"/>.
        /// </summary>
        public bool AllowUserResizing{
            get => _allowUserResizing;
            set{
                _allowUserResizing = value;
                if (IsBorderless)
                    return;
                BaseWindow.WindowBorder = _allowUserResizing ? WindowBorder.Resizable : WindowBorder.Fixed;
            }
        }
        private bool _fullscreen;
        private WindowState _oWindowState= WindowState.Normal;

        /// <summary>
        /// Gets or sets whether the <see cref="Window"/> is in fullscreen mode.
        /// </summary>
        public bool Fullscreen{
            get => _fullscreen;
            set{
                if (!_fullscreen)
                    _oWindowState = BaseWindow.WindowState;
                _fullscreen = value;
                BaseWindow.WindowState = _fullscreen ? WindowState.Fullscreen : _oWindowState;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Window"/> title.
        /// </summary>
        public string Title{
            get => BaseWindow.Title;
            set => BaseWindow.Title = value;
        }

        /// <summary>
        /// Gets or sets the position of the <see cref="Window"/>.
        /// </summary>
        public Point Position{
            get => new Point(BaseWindow.Location.X,BaseWindow.Location.Y);
            set{
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal position of the <see cref="Window"/>.
        /// </summary>
        public int X{
            get => BaseWindow.Location.X;
            set => BaseWindow.Location = new Vector2i(value, BaseWindow.Location.Y);
        }

        /// <summary>
        /// Gets or sets the vertical position of the <see cref="Window"/>.
        /// </summary>
        public int Y{
            get => BaseWindow.Location.Y;
            set => BaseWindow.Location = new Vector2i(BaseWindow.Location.X, value);
        }

        /// <summary>
        /// Closes the <see cref="Window"/>.
        /// </summary>
        public void Close()
        {
            BaseWindow.Close();
        }

        #region IDisposable implementation

        /// <inheritdoc />
        public void Dispose()
        {
            BaseWindow.Dispose();
        }

        #endregion

        event Action<FrameEventArgs>? IControlInternals.RenderFrame
        {
            add => BaseWindow.RenderFrame += value;
            remove => BaseWindow.RenderFrame -= value;
        }

        event Action<FrameEventArgs>? IControlInternals.UpdateFrame
        {
            add => BaseWindow.UpdateFrame += value;
            remove => BaseWindow.UpdateFrame -= value;
        }

        event Action<CancelEventArgs>? IControlInternals.Closing
        {
            add => BaseWindow.Closing += value;
            remove => BaseWindow.Closing -= value;
        }

        event Action<FocusedChangedEventArgs>? IControlInternals.FocusedChanged
        {
            add => BaseWindow.FocusedChanged += value;
            remove => BaseWindow.FocusedChanged -= value;
        }
        
        event Action<ResizeEventArgs>? IControlInternals.Resize
        {
            add => BaseWindow.Resize += value;
            remove => BaseWindow.Resize -= value;
        }
        
        event Action? IControlInternals.Load
        {
            add => BaseWindow.Load += value;
            remove => BaseWindow.Load -= value;
        }
        
        event Action<TextInputEventArgs>? IControlInternals.KeyPress
        {
            add => BaseWindow.KeyPress += value;
            remove => BaseWindow.KeyPress -= value;
        }
                
        event Action<MouseWheelEventArgs>? IControlInternals.MouseWheel
        {
            add => BaseWindow.MouseWheel += value;
            remove => BaseWindow.MouseWheel -= value;
        }
    }
}

