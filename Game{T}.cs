using System;
using System.Text;
using engenious.Audio;
using engenious.Content;
using engenious.Graphics;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace engenious
{
    /// <summary>
    /// Represents the method that will handle the <see cref="Game{TControl}.KeyPress"/> event of a <see cref="Game{TControl}"/>.
    /// </summary>
    /// <param name="sender">The object the key press occured on.</param>
    /// <param name="key">The key that was pressed.</param>
    public delegate void KeyPressDelegate(object sender, Rune key);

    /// <summary>
    /// Provides basic logic for <see cref="GraphicsDevice"/> initialization, game content management and rendering.
    /// </summary>
    public abstract class Game<TControl> : IGame
        where TControl : class, IRenderingSurface
    {
        /// <inheritdoc />
        public event KeyPressDelegate? KeyPress;

        /// <inheritdoc />
        public event EventHandler? Activated;

        /// <inheritdoc />
        public event EventHandler? Deactivated;
        
        /// <inheritdoc />
        public event EventHandler? Exiting;


        /// <inheritdoc />
        public event EventHandler? Resized;

        /// <summary>
        /// The <see cref="ContextFlags"/> of the associated <see cref="Context"/>-
        /// </summary>
        protected readonly ContextFlags ContextFlags;

        /// <summary>
        /// The <see cref="IGraphicsContext"/> associated with this <see cref="Game{TControl}"/>.
        /// </summary>
        protected IGraphicsContext? Context;
        private readonly AudioDevice _audio;

        private GraphicsDevice? _graphicsDevice;

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
            
            Components = new GameComponentCollection();
            Control = null!;
            Content = null!;
        }

        /// <summary>
        /// Assigns a context to a given <typeparamref name="TControl"/>.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="context">The context.</param>
        protected void ConstructContext(TControl control, IGraphicsContext context)
        {
            Control = control;
            Context = context;

            Context.MakeCurrent();
            
            _graphicsDevice = new GraphicsDevice(this, Context);
            
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
            GraphicsDevice.SynchronizeUiThread();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Draw(gameTime);

            GraphicsDevice.Present();
        }

        /// <summary>
        /// Initializes the control events and properties.
        /// </summary>
        protected void InitializeControl()
        {
            GraphicsDevice.Viewport = new Viewport(new Rectangle(new Point(), Control.ClientSize));

            Input.Mouse.UpdateWindow(Control);
            Input.Keyboard.UpdateWindow(Control);
            
            Content = new AggregateContentManager(GraphicsDevice, "Content");
            
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
                KeyPress?.Invoke(this, new Rune(e.Unicode));
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

        /// <inheritdoc cref="IGame.GraphicsDevice"/>
        public GraphicsDevice GraphicsDevice => _graphicsDevice!;

        /// <summary>
        /// Gets a <see cref="ContentManagerBase"/> for basic game content management.
        /// </summary>
        public ContentManagerBase Content{ get; private set; }

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

            foreach (var component in Components.Updateables)
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
        public void Dispose()
        {
            Control.Dispose();
            GraphicsDevice.Dispose();
            _audio.Dispose();
            SoundSourceManager.Instance.Dispose();
        }

        #endregion
    }
}

