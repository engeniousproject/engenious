using System;
using System.Drawing;
using OpenTK;
using OpenTK.Input;

namespace engenious
{
    /// <summary>
    /// Specifies a window as a game rendering view.
    /// </summary>
    public class Window : IDisposable
    {
        internal readonly GameWindow BaseWindow;
        internal Window(GameWindow baseWindow)
        {
            BaseWindow = baseWindow;
            baseWindow.KeyDown += delegate(object sender, KeyboardKeyEventArgs e)
            {
                if (e.Key == Key.F4 && e.Alt)
                {
                    Close();
                }
            };

        }

        /// <summary>
        /// Calculates a <see cref="Point"/> in client coordinates to screen coordinates.
        /// </summary>
        /// <param name="pt">The <see cref="Point"/> in client coordinates.</param>
        /// <returns>The <see cref="Point"/> translated into screen coordinates.</returns>
        public Point PointToScreen(Point pt)
        {
            return BaseWindow.PointToScreen(new System.Drawing.Point(pt.X,pt.Y));
        }

        /// <summary>
        /// Calculates a <see cref="Point"/> in screen coordinates to client coordinates.
        /// </summary>
        /// <param name="pt">The <see cref="Point"/> in screen coordinates.</param>
        /// <returns>The <see cref="Point"/> translated into client coordinates.</returns>
        public Point PointToClient(Point pt)
        {
            return BaseWindow.PointToClient(new System.Drawing.Point(pt.X,pt.Y));
        }

        /// <summary>
        /// Gets or sets a <see cref="Rectangle"/> for this windows client area.
        /// </summary>
        public Rectangle ClientRectangle{
            get{
                return new Rectangle(BaseWindow.ClientRectangle.X,BaseWindow.ClientRectangle.Y,BaseWindow.ClientRectangle.Width,BaseWindow.ClientRectangle.Height);
            }
            set{
                BaseWindow.ClientRectangle = new System.Drawing.Rectangle(value.X,value.Y,value.Width,value.Height);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Size"/> of this windows client area.
        /// </summary>
        public Size ClientSize{
            get{
                return new Size(BaseWindow.ClientSize.Width,BaseWindow.ClientSize.Height);
            }
            set{
                BaseWindow.ClientSize = new System.Drawing.Size(value.Width,value.Height);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Icon"/> of this <see cref="Window"/>.
        /// </summary>
        public Icon Icon { get { return BaseWindow.Icon; } set { BaseWindow.Icon = value; } }

        /// <summary>
        /// Gets or sets whether the <see cref="Window"/> is visible.
        /// </summary>
        public bool Visible{
            get{
                return BaseWindow.Visible;
            }
            set{
                BaseWindow.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Window"/> is in focus.
        /// </summary>
        public bool Focused => BaseWindow.Focused;
        
        /// <summary>
        /// Gets or sets whether the mouse cursor is visible on this <see cref="Window"/>.
        /// </summary>
        public bool CursorVisible{
            get{
                return BaseWindow.CursorVisible;
            }
            set{
                BaseWindow.CursorVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Window"/> is without a border.
        /// </summary>
        public bool IsBorderless{
            get{
                return BaseWindow.WindowBorder != WindowBorder.Hidden;
            }
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
            get{
                return _allowUserResizing;
            }
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
            get{
                return _fullscreen;
            }
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
            get{
                return BaseWindow.Title;
            }
            set{
                BaseWindow.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the <see cref="Window"/>.
        /// </summary>
        public Point Position{
            get{
                return new Point(BaseWindow.X,BaseWindow.Y);
            }
            set{
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal position of the <see cref="Window"/>.
        /// </summary>
        public int X{
            get{
                return BaseWindow.X;
            }
            set{
                BaseWindow.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the vertical position of the <see cref="Window"/>.
        /// </summary>
        public int Y{
            get{
                return BaseWindow.Y;
            }
            set{
                BaseWindow.Y = value;
            }
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
    }
}

