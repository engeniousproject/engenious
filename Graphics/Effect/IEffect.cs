namespace engenious.Graphics
{
    /// <summary>
    /// Interface describing a graphic effect.
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// Gets the effects parameters.
        /// </summary>
        EffectParameterCollection Parameters {
            get;
        }

        /// <summary>
        /// Gets the effects techniques.
        /// </summary>
        EffectTechniqueCollection Techniques {
            get;
        }

        /// <summary>
        /// Gets or sets the current technique of the effect.
        /// </summary>
        EffectTechnique CurrentTechnique {
            get;
            set;
        }
    }
}

