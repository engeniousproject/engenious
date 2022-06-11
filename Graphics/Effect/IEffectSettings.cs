namespace engenious.Graphics
{
    /// <summary>
    /// Base interface for effect settings.
    /// </summary>
    public interface IEffectSettings
    {
        /// <summary>
        /// Translates the effect settings to shader code.
        /// </summary>
        /// <returns>The shader code generated from these effect settings.</returns>
        string ToCode();
    }
}