namespace engenious.Graphics
{
	/// <summary>
	/// Base class for effects.
	/// </summary>
	public class Effect:GraphicsResource,IEffect
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Effect"/> class.
		/// </summary>
		/// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
		public Effect (GraphicsDevice graphicsDevice)
			: base (graphicsDevice)
		{
			Techniques = new EffectTechniqueCollection ();

		}

		/// <summary>
		/// Initializes the <see cref="Effect"/>.
		/// </summary>
		protected internal virtual void Initialize ()
		{
			Parameters = new EffectParameterCollection (Techniques);
			foreach (var technique in Techniques)
			{
				technique.Initialize();
			}
		}

		/// <summary>
		/// Gets the effects parameters.
		/// </summary>
		public EffectParameterCollection Parameters {
			get;
			private set;
		}

		/// <summary>
		/// Gets the effects techniques.
		/// </summary>
		public EffectTechniqueCollection Techniques {
			get;
			private set;
		}

		/// <summary>
		/// Gets the current set technique.
		/// </summary>
		public EffectTechnique CurrentTechnique {
			get;
			set;
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
	}
}

