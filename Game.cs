using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using engenious.Graphics;
using engenious.Content;
using System.Linq;
using engenious.Input;

namespace engenious
{
    public delegate void KeyPressDelegate(object sender,char Key);
    public abstract class Game : GraphicsResource
    {
        public event KeyPressDelegate KeyPress;
        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler Exiting;

        private GameTime gameTime;

        public Game()
        {
            Window = new GameWindow(1280, 720);
            //Window.Location = new System.Drawing.Point();
            Mouse = new MouseDevice(Window.Mouse);
            engenious.Input.Mouse.UpdateWindow(Window);
            Window.FocusedChanged += Window_FocusedChanged;
            Window.Closing += delegate(object sender, System.ComponentModel.CancelEventArgs e)
            {
                Exiting?.Invoke(this, new EventArgs());
            };
            
            gameTime = new GameTime(new TimeSpan(), new TimeSpan());
            Window.UpdateFrame += delegate(object sender, FrameEventArgs e)
            {
                Components.Sort();

                gameTime.Update(e.Time);

                Update(gameTime);
            };
            Window.RenderFrame += delegate(object sender, FrameEventArgs e)
            {

                GraphicsDevice.Clear(Color.CornflowerBlue);
                Draw(gameTime);

                GraphicsDevice.Present();
                //TODO:Draw();
            };
            Window.Resize += delegate(object sender, EventArgs e)
            {
                GraphicsDevice.Viewport = new Viewport(Window.ClientRectangle);
                GL.Viewport(Window.ClientRectangle);

                OnResize(this, e);
            };
            Window.Load += delegate(object sender, EventArgs e)
            {
                Initialize();
                LoadContent();
            };
            Window.Closing += delegate(object sender, System.ComponentModel.CancelEventArgs e)
            {
                OnExiting(this, new EventArgs());
            };
            Window.KeyPress += delegate(object sender, KeyPressEventArgs e)
            {
                KeyPress?.Invoke(this, e.KeyChar);
            };
            

            GraphicsDevice = new GraphicsDevice(Window.Context);
            Content = new engenious.Content.ContentManager(GraphicsDevice);
            Components = new GameComponentCollection();
            ThreadingHelper.Initialize(Window.WindowInfo);


        }

        void Window_FocusedChanged(object sender, EventArgs e)
        {
            if (Window.Focused)
                Activated?.Invoke(this, e);
            else
                Deactivated?.Invoke(this, e);
        }

        protected virtual void Initialize()
        {
            foreach (var component in Components)
            {
                component.Initialize();
            }
        }

        public void Exit()
        {
            Window.Close();
        }

        protected virtual void OnResize(object sender, EventArgs e)
        {
        }

        protected virtual void OnExiting(object sender, EventArgs e)
        {
        }

        public ContentManager Content{ get; private set; }

        
        internal GameWindow Window{ get; private set; }

        public MouseDevice Mouse{ get; private set; }

        public string Title{ get { return Window.Title; } set { Window.Title = value; } }

        public bool IsMouseVisible
        {
            get
            {
                return Window.CursorVisible;
            }
            set
            {
                //Window.CursorVisible = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return Window.Focused;
            }
        }

        public void Run()
        {
            Window.Run();
        }

        public GameComponentCollection Components{ get; private set; }

        public virtual void LoadContent()
        {
            foreach (var component in Components)
            {
                component.Load();
            }
        }

        public virtual void UnloadContent()
        {
            foreach (var component in Components)
            {
                component.Unload();
            }
        }

        public virtual void Update(GameTime gameTime)
        {

            foreach (var component in Components.Updatables)
            {
                if (!component.Enabled)
                    break;
                component.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (var component in Components.Drawables)
            {
                if (!component.Visible)
                    break;
                component.Draw(gameTime);
            }
        }

        #region IDisposable

        public override void Dispose()
        {
            Window.Dispose();

            base.Dispose();
        }

        #endregion
    }
}

