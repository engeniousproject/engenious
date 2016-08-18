using System;
using OpenTK;

namespace engenious
{
    public class Window : IDisposable
    {
        internal GameWindow window;
        internal Window(GameWindow window)
        {
            this.window = window;
            window.KeyDown += delegate(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
            {
                if (e.Key == OpenTK.Input.Key.F4 && e.Alt)
                {
                    Close();
                    return;
                }
            };

        }
        public Rectangle ClientRectangle{
            get{
                return new Rectangle(window.ClientRectangle.X,window.ClientRectangle.Y,window.ClientRectangle.Width,window.ClientRectangle.Height);
            }
            set{
                window.ClientRectangle = new System.Drawing.Rectangle(value.X,value.Y,value.Width,value.Height);
            }
        }
        public Size ClientSize{
            get{
                return new Size(window.ClientSize.Width,window.ClientSize.Height);
            }
            set{
                window.ClientSize = new System.Drawing.Size(value.Width,value.Height);
            }
        }
        public System.Drawing.Icon Icon { get { return window.Icon; } set { window.Icon = value; } }
        public bool Visible{
            get{
                return window.Visible;
            }
            set{
                window.Visible = value;
            }
        }
        public bool Focused{
            get{
                return window.Focused;
            }
        }
        public bool CursorVisible{
            get{
                return window.CursorVisible;
            }
            set{
                window.CursorVisible = value;
            }
        }
        public bool IsBorderless{
            get{
                return window.WindowBorder != WindowBorder.Hidden;
            }
            set{
                if (value)
                    window.WindowBorder = WindowBorder.Hidden;
                else
                    window.WindowBorder = allowUserResizing ? WindowBorder.Resizable : WindowBorder.Fixed;
            }
        }
        private bool allowUserResizing;
        public bool AllowUserResizing{
            get{
                return allowUserResizing;
            }
            set{
                allowUserResizing = value;
                if (IsBorderless)
                    return;
                window.WindowBorder = allowUserResizing ? WindowBorder.Resizable : WindowBorder.Fixed;
            }
        }
        private bool fullscreen;
        private WindowState oWindowState= WindowState.Normal;
        public bool Fullscreen{
            get{
                return fullscreen;
            }
            set{
                if (!fullscreen)
                    oWindowState = window.WindowState;
                fullscreen = value;
                window.WindowState = fullscreen ? WindowState.Fullscreen : oWindowState;
            }
        }
        public string Title{
            get{
                return window.Title;
            }
            set{
                window.Title = value;
            }
        }
        public Point Position{
            get{
                return new Point(window.X,window.Y);
            }
            set{
                X = value.X;
                Y = value.Y;
            }
        }
        public int X{
            get{
                return window.X;
            }
            set{
                window.X = value;
            }
        }
        public int Y{
            get{
                return window.Y;
            }
            set{
                window.Y = value;
            }
        }



        public void Close()
        {
            window.Close();
        }

        #region IDisposable implementation

        public void Dispose()
        {
            window.Dispose();
        }

        #endregion
    }
}

