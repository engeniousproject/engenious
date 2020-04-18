namespace engenious.Graphics
{
	/// <summary>
	/// Interface describing an effect with ambient and directional lights.
	/// </summary>
	public interface IEffectLights
	{
		/// <summary>
		/// Gets or sets the ambient light color.
		/// </summary>
		Vector3 AmbientLightColor{get;set;}

		/// <summary>
		/// Gets or sets whether the lighting effect is enabled.
		/// </summary>
		bool LightingEnabled{get;set;}

		/// <summary>
		/// Gets or sets the first directional light of this effect.
		/// </summary>
		DirectionalLight DirectionalLight0{get;set;}

		/// <summary>
		/// Gets or sets the second directional light of this effect.
		/// </summary>
		DirectionalLight DirectionalLight1{get;set;}

		/// <summary>
		/// Gets or sets the third directional light of this effect.
		/// </summary>
		DirectionalLight DirectionalLight2{get;set;}

		/// <summary>
		/// Initializes lights with default parameters.
		/// </summary>
		void EnableDefaultLighting();
	}
}

