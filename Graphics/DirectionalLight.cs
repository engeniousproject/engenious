namespace engenious.Graphics
{
	/// <summary>
	/// A directional light source.
	/// </summary>
	public class DirectionalLight
	{
		internal readonly EffectParameter DirectionParameter, DiffuseColorParameter, SpecularColorParameter;

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectionalLight"/> instance.
		/// </summary>
		/// <param name="clone">The <see cref="DirectionalLight"/> to clone its parameters from.</param>
		public DirectionalLight (DirectionalLight clone)
			:this(clone.DirectionParameter,clone.DiffuseColorParameter,clone.SpecularColorParameter)
		{
			DiffuseColor = clone.DiffuseColor;
			Direction = clone.Direction;
			SpecularColor = clone.SpecularColor;
			Enabled = clone.Enabled;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectionalLight"/> instance.
		/// </summary>
		/// <param name="directionParameter">The direction of the light source.</param>
		/// <param name="diffuseColorParameter">The diffuse color of the light source.</param>
		/// <param name="specularColorParameter">The specular color of the light source.</param>
		public DirectionalLight (EffectParameter directionParameter,EffectParameter diffuseColorParameter,EffectParameter specularColorParameter)
		{
			DirectionParameter = directionParameter;
			DirectionParameter = diffuseColorParameter;
			SpecularColorParameter = specularColorParameter;
		}

		/// <summary>
		/// Gets the diffuse color of the light source.
		/// </summary>
		public Vector3 DiffuseColor{get; }

		/// <summary>
		/// Gets the direction of the light source.
		/// </summary>
	    public Vector3 Direction{get; }

	    /// <summary>
	    /// Gets the specular color of the light source.
	    /// </summary>
	    public Vector3 SpecularColor{get; }

	    /// <summary>
	    /// Gets whether the light source is enabled.
	    /// </summary>
	    public bool Enabled{get; }

	}
}

