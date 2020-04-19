using System;
using System.Drawing;
using engenious.Audio;
using engenious.Content;
using engenious.Graphics;
using engenious.Helper;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace engenious
{
    /// <inheritdoc />
    public delegate void KeyPressDelegate(object sender,char key);

    /// <summary>
    /// Provides basic logic for <see cref="GraphicsDevice"/> initialization, game content management and rendering.
    /// </summary>
    public abstract class Game : GraphicsResource
    {
        /// <summary>
        /// Occurs when a key is pressed while the <see cref="Game"/> is in focus.
        /// </summary>
        public event KeyPressDelegate KeyPress;

        /// <summary>
        /// Occurs when the <see cref="Game"/> is getting focus.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// Occurs when the <see cref="Game"/> is losing focus.
        /// </summary>
        public event EventHandler Deactivated;

        /// <summary>
        /// Occurs when the <see cref="Game"/> is exiting.
        /// </summary>
        public event EventHandler Exiting;

        /// <summary>
        /// Occurs when the <see cref="Game"/> game rendering view is being resized.
        /// </summary>
        public event EventHandler Resized;

        internal readonly GraphicsContextFlags ContextFlags;
        private IGraphicsContext _context;
        private readonly AudioDevice _audio;
        private void ConstructContext()
        {
            var windowInfo = Window.BaseWindow.WindowInfo;
            /*if (Window.BaseWindow.Context == null)
            {
                var flags = GraphicsContextFlags.ForwardCompatible;
                var major = 1;
                var minor = 0;

                if (Environment.OSVersion.Platform == PlatformID.Win32NT ||
                    Environment.OSVersion.Platform == PlatformID.Win32S ||
                    Environment.OSVersion.Platform == PlatformID.Win32Windows ||
                    Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    major = 4;
                    minor = 4;
                }
                if (_context == null || _context.IsDisposed)
                {

                    var colorFormat = new ColorFormat(8, 8, 8, 8);
                    var depth = 24;
                    var stencil = 8;
                    var samples = 4;

                    var mode =
                        new GraphicsMode(colorFormat, depth, stencil, samples);
                    try
                    {
                        _context = new GraphicsContext(mode, windowInfo, major, minor, flags);
                        //this.Context = Window.Context;
                    }
                    catch (Exception)
                    {
                        mode = GraphicsMode.Default;
                        major = 1;
                        minor = 0;
                        flags = GraphicsContextFlags.Default;
                        _context = new GraphicsContext(mode, windowInfo, major, minor, flags);
                    }
                }
            }
            else*/
            _context = Window.BaseWindow.Context;

            _context.MakeCurrent(windowInfo);
            (_context as IGraphicsContextInternal)?.LoadAll();


        }

        private void CreateSharedContext()
        {
            if (GraphicsDevice?.DriverVendor == null || GraphicsDevice.DriverVendor.IndexOf("amd", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                var secondwindow = new GameWindow();
                ThreadingHelper.Initialize(_context,secondwindow.WindowInfo, 0,0, ContextFlags);
            }
            else
            {
                ThreadingHelper.Initialize(_context,Window.BaseWindow.WindowInfo, 0,0, ContextFlags);
            }
            _context.MakeCurrent(Window.BaseWindow.WindowInfo);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            _audio = new AudioDevice();

            ContextFlags = GraphicsContextFlags.Default;
            
            #if DEBUG
            ContextFlags |= GraphicsContextFlags.Debug;
            #endif
            
            var window = new GameWindow(1280, 720,GraphicsMode.Default,"engenious Game Window",GameWindowFlags.Default,DisplayDevice.Default,0,0,ContextFlags);
            
            GraphicsContext.ShareContexts = true;

            Window = new Window(window);
            ConstructContext();

            GraphicsDevice = new GraphicsDevice(this, _context);

            CreateSharedContext();
            
            GraphicsDevice.Viewport = new Viewport(window.ClientRectangle);
            window.Context.MakeCurrent(window.WindowInfo);
            window.Context.LoadAll();
            GL.Viewport(window.ClientRectangle);
            //Window.Location = new System.Drawing.Point();
            
            //Mouse = new MouseDevice();
            Input.Mouse.UpdateWindow(window);
            window.FocusedChanged += Window_FocusedChanged;
            window.Closing += delegate
            {
                Exiting?.Invoke(this, EventArgs.Empty);
            };

            var gameTime = new GameTime(new TimeSpan(), new TimeSpan());

            window.UpdateFrame += delegate(object sender, FrameEventArgs e)
            {
                Components.Sort();

                gameTime.Update(e.Time);

                Update(gameTime);
            };
            window.RenderFrame += delegate
            {
                ThreadingHelper.RunUiThread();
                GraphicsDevice.Clear(Color.CornflowerBlue);
                Draw(gameTime);

                GraphicsDevice.Present();
            };
            window.Resize += delegate(object sender, EventArgs e)
            {
                GraphicsDevice.Viewport = new Viewport(window.ClientRectangle);
                GL.Viewport(window.ClientRectangle);

                Resized?.Invoke(sender,e);
                OnResize(e);
            };
            window.Load += delegate
            {
                Initialize();
                LoadContent();
            };
            window.Closing += delegate
            {
                OnExiting(EventArgs.Empty);
            };
            window.KeyPress += delegate(object sender, KeyPressEventArgs e)
            {
                KeyPress?.Invoke(this, e.KeyChar);
            };

            //Window.Context.MakeCurrent(Window.WindowInfo);

            Content = new ContentManager(GraphicsDevice);
            Components = new GameComponentCollection();


        }

        void Window_FocusedChanged(object sender, EventArgs e)
        {
            if (Window.Focused)
                Activated?.Invoke(this, e);
            else
                Deactivated?.Invoke(this, e);
        }

        /// <summary>
        /// Initializes game components and the rendering view.
        /// </summary>
        protected virtual void Initialize()
        {
            Window.Visible = true;
            GL.Enable(EnableCap.Blend);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            //GraphicsDevice.SamplerStates = new SamplerStateCollection(GraphicsDevice);

            foreach (var component in Components)
            {
                component.Initialize();
            }
        }

        /// <summary>
        /// Closes the rendering view.
        /// </summary>
        public void Exit()
        {
            Window.Close();
        }

        /// <summary>
        /// Raises the <see cref="Resized"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnResize(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="Exiting"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnExiting(EventArgs e)
        {
        }

        /// <summary>
        /// Gets a <see cref="ContentManager"/> for basic game content management.
        /// </summary>
        public ContentManager Content{ get; private set; }

        /// <summary>
        /// Gets or sets an <see cref="Icon"/> associated with the rendering view.
        /// <remarks>This sets or gets the window icon, if the rendering view is a <see cref="Window"/>.</remarks>
        /// </summary>
        public Icon Icon { get { return Window.Icon; } set { Window.Icon = value; } }

        /// <summary>
        /// Gets a Window(rendering view) associated with this <see cref="Game"/>.
        /// </summary>
        public Window Window{ get; private set; }

        /// <summary>
        /// Gets or sets a title associated with the rendering view.
        /// <remarks>This sets or gets the window title, if the rendering view is a <see cref="Window"/>.</remarks>
        /// </summary>
        public string Title{ get { return Window.Title; } set { Window.Title = value; } }

        /// <summary>
        /// Gets or sets whether the mouse cursor is visible while on the rendering view.
        /// </summary>
        public bool IsMouseVisible
        {
            get
            {
                return Window.CursorVisible;
            }
            set
            {
                Window.CursorVisible = value;
            }
        }

        /// <summary>
        /// Gets whether the rendering view is currently in focus.
        /// </summary>
        public bool IsActive => Window.Focused;

        /// <summary>
        /// Runs the rendering view's message loop, as well as update and rendering loop.
        /// </summary>
        public void Run()
        {
            Window.BaseWindow.Run();
        }

        /// <summary>
        /// Runs the rendering view's message loop, as well as update and rendering loop within given parameters.
        /// </summary>
        /// <param name="updatesPerSec">The frequency at which the update loop should run.</param>
        /// <param name="framesPerSec">The frequency at which the rendering loop should run.</param>
        public void Run(double updatesPerSec, double framesPerSec)
        {
            Window.BaseWindow.Run(updatesPerSec, framesPerSec);
        }

        /// <summary>
        /// Gets a collection of game components associated with this <see cref="Game"/>.
        /// </summary>
        public GameComponentCollection Components{ get; private set; }
        
        /// <summary>
        /// Called when <see cref="Game"/> related content should be loaded.
        /// </summary>
        public virtual void LoadContent()
        {
            foreach (var component in Components)
            {
                component.Load();
            }
        }

        /// <summary>
        ///Called when <see cref="Game"/> related content should be unloaded.
        /// </summary>
        public virtual void UnloadContent()
        {
            foreach (var component in Components)
            {
                component.Unload();
            }
        }

        /// <summary>
        /// Executes a single update tick.
        /// </summary>
        /// <param name="gameTime">Contains the elapsed time since the last update, as well as total elapsed time.</param>
        public virtual void Update(GameTime gameTime)
        {

            foreach (var component in Components.Updatables)
            {
                if (!component.Enabled)
                    continue;
                component.Update(gameTime);
            }
        }

        /// <summary>
        /// Executes a single frame render.
        /// </summary>
        /// <param name="gameTime">Contains the elapsed time since the last render, as well as total elapsed time.</param>
        public virtual void Draw(GameTime gameTime)
        {
            foreach (var component in Components.Drawables)
            {
                if (!component.Visible)
                    continue;
                component.Draw(gameTime);
            }
        }

        #region IDisposable

        /// <inheritdoc />
        public override void Dispose()
        {
            Window.Dispose();
            _audio.Dispose();
            SoundSourceManager.Instance.Dispose();
            base.Dispose();
        }

        #endregion
    }
}

