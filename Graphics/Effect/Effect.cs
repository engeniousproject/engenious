﻿namespace engenious.Graphics
{
	/// <summary>
	/// Base class for effects.
	/// </summary>
	public class Effect : GraphicsResource, IEffect
	{
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
		/// Initializes the <see cref="Effect"/>.
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
		public EffectTechnique? CurrentTechnique {
			get;
			set;
		}

		/// <summary>
		/// Applies all passes of this <see cref="Effect"/>.
		/// </summary>
		protected internal virtual void OnApply ()
		{
			if (CurrentTechnique == null)
				return;
			foreach (var pass in CurrentTechnique.Passes) {
				pass.Apply ();
			}
		}
	}
}

