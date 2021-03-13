namespace engenious.Graphics
{
    /// <summary>
    /// Interface describing an effect which renders a texture.
    /// </summary>
    public interface ITextured
    {
        /// <summary>
        /// Sets the texture to render.
        /// </summary>
        Texture Texture{set;}
    }
}

