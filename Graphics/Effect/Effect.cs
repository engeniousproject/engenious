namespace engenious.Graphics
{
	/// <summary>
	/// Base class for effects.
	/// </summary>
	public class Effect : GraphicsResource, IEffect
	{
        private EffectTechnique? _currentTechnique;

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
		public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;
		/// <summary>
		/// Initializes a new instance of the <see cref="Effect"/> class.
		/// </summary>
		/// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
		public Effect (GraphicsDevice graphicsDevice)
			: base (graphicsDevice)
		{
			Techniques = new EffectTechniqueCollection ();

			Parameters = new EffectParameterCollection (GraphicsDevice);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Effect"/> class..
		/// </summary>
		protected internal virtual void Initialize ()
		{
			foreach (var technique in Techniques)
			{
				technique.Initialize();
			}

			Parameters.Initialize(Techniques);
		}

		/// <summary>
		/// Gets the effects parameters.
		/// </summary>
        public EffectParameterCollection Parameters { get; }

		/// <summary>
		/// Gets the effects techniques.
		/// </summary>
        public EffectTechniqueCollection Techniques { get; }

        /// <summary>
        /// Gets or sets the current set technique.
        /// </summary>
        public EffectTechnique CurrentTechnique
        {
            get => _currentTechnique!;
            set => _currentTechnique = value;
        }

        /// <summary>
        /// Applies all passes of this <see cref="Effect"/>.
        /// </summary>
        protected internal virtual void OnApply ()
		{
            foreach (var pass in CurrentTechnique.Passes) {
				pass.Apply ();
			}
		}
        

        /// <inheritdoc />
        public override void Dispose()
        {
            foreach (var t in Techniques)
            {
                t.Dispose();
            }

            base.Dispose();
        }
	}
}

