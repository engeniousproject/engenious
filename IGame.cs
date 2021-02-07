using System;
using System.Drawing;
using engenious.Content;
using engenious.Graphics;

namespace engenious
{
    public interface IGame : IDisposable
    {
        /// <summary>
        /// Occurs when a key is pressed while the <see cref="Game"/> is in focus.
        /// </summary>
        event KeyPressDelegate KeyPress;

        /// <summary>
        /// Occurs when the <see cref="Game"/> is getting focus.
        /// </summary>
        event EventHandler Activated;

        /// <summary>
        /// Occurs when the <see cref="Game"/> is losing focus.
        /// </summary>
        event EventHandler Deactivated;

        /// <summary>
        /// Occurs when the <see cref="Game"/> is exiting.
        /// </summary>
        event EventHandler Exiting;

        /// <summary>
        /// Occurs when the <see cref="Game"/> game rendering view is being resized.
        /// </summary>
        event EventHandler Resized;

        /// <summary>
        /// Gets a <see cref="ContentManagerBase"/> for basic game content management.
        /// </summary>
        ContentManagerBase Content { get; }

        /// <summary>
        /// Gets or sets whether the mouse cursor is visible while on the rendering view.
        /// </summary>
        bool IsMouseVisible { get; set; }

        /// <summary>
        /// Gets or sets whether the mouse cursor is grabbed while on the rendering view.
        /// </summary>
        bool IsCursorGrabbed { get; set; }

        /// <summary>
        /// Gets whether the rendering view is currently in focus.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets a collection of game components associated with this <see cref="Game"/>.
        /// </summary>
        GameComponentCollection Components { get; }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> the resource is allocated on.
        /// </summary>
        GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Gets or sets the name of the <see cref="GraphicsResource"/>.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a tag of the <see cref="GraphicsResource"/>.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Gets whether the <see cref="GraphicsResource"/> is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets a rendering view associated with this <see cref="IGame"/>.
        /// </summary>
        IRenderingSurface RenderingSurface { get; }

        /// <summary>
        /// Called when <see cref="Game"/> related content should be loaded.
        /// </summary>
        void LoadContent();

        /// <summary>
        ///Called when <see cref="Game"/> related content should be unloaded.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Executes a single update tick.
        /// </summary>
        /// <param name="gameTime">Contains the elapsed time since the last update, as well as total elapsed time.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Executes a single frame render.
        /// </summary>
        /// <param name="gameTime">Contains the elapsed time since the last render, as well as total elapsed time.</param>
        void Draw(GameTime gameTime);
    }
}