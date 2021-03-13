namespace engenious.Graphics
{
    /// <summary>
    /// Interface describing a graphic effect technique.
    /// </summary>
    public interface IEffectTechnique
    {
        /// <summary>
        /// Gets a collection of render passes for this technique.
        /// </summary>
        public EffectPassCollection Passes { get; }
    }
}