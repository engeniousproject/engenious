using System;

namespace engenious
{
    /// <summary>
    /// Defines a game component.
    /// </summary>
    public abstract class GameComponent : IGameComponent,IUpdateable,IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameComponent"/> class.
        /// </summary>
        /// <param name="game">The <see cref="Game"/> to create the component for</param>
        protected GameComponent(Game game)
        {
            Game = game;
            Enabled = true;
        }

        #region IUpdateable implementation

        /// <inheritdoc />
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <inheritdoc />
        public int UpdateOrder
        {
            get;
            set;
        }

        /// <inheritdoc />
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Game"/> the component is part of.
        /// </summary>
        public Game Game { get; private set; }

        #endregion


        #region IGameComponent implementation

        /// <summary>
        /// Called when <see cref="Game"/> related content should be loaded.
        /// </summary>
        protected virtual void LoadContent()
        {
        }

        /// <summary>
        ///Called when <see cref="Game"/> related content should be unloaded.
        /// </summary>
        protected virtual void UnloadContent()
        {
        }

        internal void Load()
        {
            LoadContent();
        }

        internal void Unload()
        {
            UnloadContent();
        }

        /// <inheritdoc />
        public virtual void Initialize()
        {
        }

        #endregion

        #region IDisposable implementation

        /// <inheritdoc />
        public virtual void Dispose()
        {
        }

        #endregion
    }
}

