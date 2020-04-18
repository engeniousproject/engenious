namespace engenious
{
    /// <summary>
    /// An interface for updatedable components.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Gets or sets the update order of this game component.
        /// </summary>
        /// <remarks>Smaller valued components get updated first.</remarks>
        int UpdateOrder { get; }

        /// <summary>
        /// Gets whether this component is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Updates the drawable game component.
        /// </summary>
        /// <param name="gameTime">The <see cref="GameTime"/> for this update loop.</param>
        void Update(GameTime gameTime);
    }
}

