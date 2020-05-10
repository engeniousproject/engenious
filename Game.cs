using System;
using System.Drawing;
using engenious.Graphics;
using engenious.Helper;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace engenious
{
    /// <inheritdoc />
    public abstract class Game : Game<Window>
    {
        private void CreateWindow()
        {
            var baseWindow = new GameWindow(1280, 720,GraphicsMode.Default,"engenious Game Window",GameWindowFlags.Default,DisplayDevice.Default,0,0,ContextFlags);
            GraphicsContext.ShareContexts = true;
            Window = new Window(baseWindow);
            ConstructContext(baseWindow.WindowInfo, baseWindow.Context);

            CreateSharedContext(baseWindow.WindowInfo);
            
            baseWindow.Context.MakeCurrent(baseWindow.WindowInfo);
            baseWindow.Context.LoadAll();

        }

        /// <inheritdoc />
        protected Game()
        {
            CreateWindow();
            
            InitializeControl(Window);
        }

        /// <summary>
        /// Gets a Window(rendering view) associated with this <see cref="Game"/>.
        /// </summary>
        public Window Window{ get; private set; }

        /// <summary>
        /// Gets or sets an <see cref="Icon"/> associated with the rendering view.
        /// <remarks>This sets or gets the window icon, if the rendering view is a <see cref="Window"/>.</remarks>
        /// </summary>
        public Icon Icon { get { return Window.Icon; } set { Window.Icon = value; } }

        /// <summary>
        /// Gets or sets a title associated with the rendering view.
        /// <remarks>This sets or gets the window title, if the rendering view is a <see cref="Window"/>.</remarks>
        /// </summary>
        public string Title{ get { return Window.Title; } set { Window.Title = value; } }

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
        /// Closes the rendering view.
        /// </summary>
        public void Exit()
        {
            Window.Close();
        }
    }
}