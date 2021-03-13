namespace engenious.Graphics
{
	/// <summary>
	/// Describes a effect technique consisting of multiple render passes.
	/// </summary>
	public class EffectTechnique : IEffectTechnique
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EffectTechnique"/> class.
		/// </summary>
		/// <param name="name">The technique name.</param>
		protected internal EffectTechnique (string name)
		{
			Name = name;
			Passes = new EffectPassCollection ();
		}

		/// <summary>
		/// Gets the name of the technique.
		/// </summary>
		public string Name { get; }

		/// <inheritdoc />
		public EffectPassCollection Passes{ get; }

		/// <summary>
		/// Initializes this technique.
		/// </summary>
		protected internal virtual void Initialize()
		{
			
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}
	}
}

