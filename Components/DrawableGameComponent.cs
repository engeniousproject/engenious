using System;
using engenious.Graphics;

namespace engenious
{
    /// <summary>
    /// Defines a drawable game component.
    /// </summary>
    public abstract class DrawableGameComponent : GameComponent, IDrawable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableGameComponent"/> class.
        /// </summary>
        /// <param name="game">The <see cref="Game"/> to create the component for.</param>
        protected DrawableGameComponent(IGame game)
            : base(game)
        {
            GraphicsDevice = game.GraphicsDevice ?? throw new ArgumentException("Game not yet initialized sufficiently", nameof(game));
            Visible = true;
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/>.
        /// </summary>
        public GraphicsDevice GraphicsDevice{ get; private set; }

        #region IDrawable implementation

        /// <inheritdoc />
        public virtual void Draw(GameTime gameTime)
        {
        }

        /// <inheritdoc />
        public bool Visible
        {
            get;
            set;
        }

        /// <inheritdoc />
        public int DrawOrder
        {
            get;
            set;
        }


        #endregion

        /// <inheritdoc />
        protected override void LoadContent()
        {
           
        }

        /// <inheritdoc />
        protected override void UnloadContent()
        {

        }
    }
}

