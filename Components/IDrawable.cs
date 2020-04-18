namespace engenious
{
    /// <summary>
    /// An interface for drawable game components.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Gets whether the game component is visible or not.
        /// </summary>
        bool Visible{ get; }

        /// <summary>
        /// Gets or sets the drawing order of this game component.
        /// </summary>
        /// <remarks>Smaller valued components get drawn first.</remarks>
        int DrawOrder{ get; }

        /// <summary>
        /// Draws the drawable game component.
        /// </summary>
        /// <param name="gameTime">The <see cref="GameTime"/> for this rendering loop.</param>
        void Draw(GameTime gameTime);
    }
}

