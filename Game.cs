using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using Image = OpenTK.Windowing.Common.Input.Image;

namespace engenious
{
    /// <inheritdoc />
    public abstract class Game : Game<Window>
    {
        private readonly GameSettings _settings;
        private Icon[] _icons;
        private void CreateWindow()
        {
            var nativeWindowSettings = new NativeWindowSettings
            {
                NumberOfSamples = _settings.NumberOfSamples,
                Flags = ContextFlags,
                Size = new Vector2i(1024, 768)
            };
            var baseWindow = new GameWindow(GameWindowSettings.Default, nativeWindowSettings);
            //GraphicsContext.ShareContexts = true;
            Window = new Window(baseWindow);
            ConstructContext(baseWindow, baseWindow.Context);

            CreateSharedContext(baseWindow);

            baseWindow.Context.MakeCurrent();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings with which the rendering environment should be initialized.
        /// <c>null</c> is equivalent to <see cref="GameSettings.Default"/>.
        /// </param>
        protected Game(GameSettings settings = null)
        {
            _settings = settings ?? GameSettings.Default;
            CreateWindow();
            
            InitializeControl(Window);
        }

        /// <summary>
        /// Gets a Window(rendering view) associated with this <see cref="Game"/>.
        /// </summary>
        public Window Window{ get; private set; }

        /// <summary>
        /// Gets or sets an <see cref="Icons"/> associated with the rendering view.
        /// <remarks>This sets or gets the window icon, if the rendering view is a <see cref="Window"/>.</remarks>
        /// </summary>
        public Icon[] Icons
        {
            get => _icons;
            set
            {
                if (_icons != value)
                {
                    if (_icons != null)
                    {
                        foreach (var i in _icons)
                        {
                            i.Dispose();
                        }
                    }
                    _icons = value;
                    ReloadIcons();
                }
            }
        }

        private void ReloadIcons()
        {
            if (_icons != null)
            {
                var img = new Image[_icons.Length];
                for (int i = 0; i < _icons.Length; i++)
                {
                    using var bmp = _icons[i].ToBitmap();
                    var bmpData =
                        bmp.LockBits(new System.Drawing.Rectangle(new System.Drawing.Point(), bmp.Size),
                            ImageLockMode.ReadOnly, bmp.PixelFormat);
                    var data = new byte[bmpData.Height * bmpData.Stride];
                    if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                        throw new NotSupportedException("Currently only 32bppArgb is supported");
                    unsafe
                    {
                        var srcPtr = (byte*) bmpData.Scan0;
                        for (int p = 0; p < data.Length; p += 4)
                        {
                            data[p + 0] = srcPtr[p + 2];
                            data[p + 1] = srcPtr[p + 1];
                            data[p + 2] = srcPtr[p + 0];
                            data[p + 3] = srcPtr[p + 3];
                        }
                    }
                    bmp.UnlockBits(bmpData);
                    img[i] = new Image(_icons[i].Width, _icons[i].Height, data);
                }
                Window.BaseWindow.Icon = new WindowIcon(img);
            }
            else
            {
                Window.BaseWindow.Icon = null;
            }
        }

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
            Window.BaseWindow.Run();
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