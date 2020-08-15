using System;
using engenious.Audio;
using engenious.Content;
using engenious.Graphics;
using engenious.Helper;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;

namespace engenious
{
    /// <inheritdoc />
    public delegate void KeyPressDelegate(object sender,char key);

    /// <summary>
    /// Provides basic logic for <see cref="GraphicsDevice"/> initialization, game content management and rendering.
    /// </summary>
    public abstract class Game<TControl> : GraphicsResource, IGame
        where TControl : class, IRenderingSurface
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

        protected readonly ContextFlags ContextFlags;
        protected IGraphicsContext _context;
        private readonly AudioDevice _audio;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        protected Game()
        {
            _audio = new AudioDevice();
            ContextFlags = ContextFlags.Default;
            
#if DEBUG
            ContextFlags |= ContextFlags.Debug;
#endif
        }

        /// <summary>
        /// Assigns a context to a given <see cref="IWindowInfo"/>.
        /// </summary>
        /// <param name="windowInfo">The <see cref="IWindowInfo"/>.</param>
        /// <param name="context">The context.</param>
        protected void ConstructContext(INativeWindow windowInfo, IGraphicsContext context)
        {
            _context = context;

            _context.MakeCurrent();
            
            GraphicsDevice = new GraphicsDevice(this, _context);
            
        }

        /// <summary>
        /// Creates a shared context for a given <see cref="IWindowInfo"/>.
        /// </summary>
        /// <param name="windowInfo">The <see cref="IWindowInfo"/>.</param>
        protected void CreateSharedContext(NativeWindow windowInfo)
        {
            if (GraphicsDevice?.DriverVendor == null || GraphicsDevice.DriverVendor.IndexOf("amd", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                // TODO: context sharing needs shared context
                var secondwindow = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);
                ThreadingHelper.Initialize(_context,secondwindow);
            }
            else
            {
                ThreadingHelper.Initialize(_context,windowInfo);
            }
            _context.MakeCurrent();
        }
        void Window_FocusedChanged(FocusedChangedEventArgs e)
        {
            if (e.IsFocused)
                Activated?.Invoke(this, EventArgs.Empty);
            else
                Deactivated?.Invoke(this, EventArgs.Empty);
        }

        private void OnRenderFrame(GameTime gameTime)
        {
            ThreadingHelper.RunUiThread();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Draw(gameTime);

            GraphicsDevice.Present();
        }

        protected void InitializeControl(TControl control)
        {
            Control = control;
            GraphicsDevice.Viewport = new Viewport(new Rectangle(new Point(), Control.ClientSize));

            Input.Mouse.UpdateWindow(Control);
            Input.Keyboard.UpdateWindow(Control);
            
            Content = new ContentManager(GraphicsDevice);
            Components = new GameComponentCollection();
            
            Control.FocusedChanged += Window_FocusedChanged;
            Control.Closing += delegate
            {
                Exiting?.Invoke(this, EventArgs.Empty);
            };

            var gameTime = new GameTime(new TimeSpan(), new TimeSpan());

            Control.UpdateFrame += delegate(FrameEventArgs e)
            {
                gameTime.Update(e.Time);

                Update(gameTime);
            };
            Control.RenderFrame += delegate
            {
                OnRenderFrame(gameTime);
            };
            Control.Resize += delegate(ResizeEventArgs e)
            {
                GraphicsDevice.Viewport = new Viewport(new Rectangle(new Point(), new Size(e.Width, e.Height)));

                Resized?.Invoke(this, EventArgs.Empty);
                OnResize(EventArgs.Empty);
            };
            Control.Load += delegate
            {
                Initialize();
                LoadContent();
            };
            Control.Closing += delegate
            {
                OnExiting(EventArgs.Empty);
            };
            Control.KeyPress += delegate(TextInputEventArgs e)
            {
                KeyPress?.Invoke(this, (char)e.Unicode);
            };
        }
        
        /// <summary>
        /// Initializes game components and the rendering view.
        /// </summary>
        protected virtual void Initialize()
        {
            Control.Visible = true;
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
        /// Gets a rendering view associated with this <see cref="Game"/>.
        /// </summary>
        public TControl Control { get; private set; }

        /// <inheritdoc />
        public IRenderingSurface RenderingSurface => Control;

        /// <summary>
        /// Gets or sets whether the mouse cursor is visible while on the rendering view.
        /// </summary>
        public bool IsMouseVisible
        {
            get => Control.CursorVisible;
            set => Control.CursorVisible = value;
        }
        
        /// <summary>
        /// Gets or sets whether the mouse cursor is grabbed while on the rendering view.
        /// </summary>
        public bool IsCursorGrabbed
        {
            get => Control.CursorGrabbed;
            set => Control.CursorGrabbed = value;
        }

        /// <summary>
        /// Gets whether the rendering view is currently in focus.
        /// </summary>
        public bool IsActive => Control.Focused;

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
            Control.Dispose();
            _audio.Dispose();
            SoundSourceManager.Instance.Dispose();
            base.Dispose();
        }

        #endregion
    }
}

