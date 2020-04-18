namespace engenious.Graphics
{
	/// <summary>
	/// Interface describing a fog effect.
	/// </summary>
	public interface IEffectFog
	{
		/// <summary>
		/// Gets or sets the fog color.
		/// </summary>
		Vector3 FogColor{get;set;}

		/// <summary>
		/// Gets or sets the fogs starting distance.
		/// </summary>
		float FogStart{get;set;}

		/// <summary>
		/// Gets or sets the fogs end distance.
		/// </summary>
		float FogEnd{get;set;}

		/// <summary>
		/// Gets or sets whether the fog effect is enabled.
		/// </summary>
		bool FogEnabled{get;set;}
	}
}

