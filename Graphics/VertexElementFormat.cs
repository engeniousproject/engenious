namespace engenious.Graphics
{
	/// <summary>
	/// Specifies the format of a vertex element.
	/// </summary>
	public enum VertexElementFormat
	{
		/// <summary>
		/// 32-bit floating point.
		/// </summary>
		Single,
		/// <summary>
		/// 2D 32-bit floating point vector.
		/// </summary>
		Vector2,
		/// <summary>
		/// 3D 32-bit floating point vector.
		/// </summary>
		Vector3,
		/// <summary>
		/// 4D 32-bit floating point vector.
		/// </summary>
		Vector4,
		/// <summary>
		/// 2d 16-bit floating point vector.
		/// </summary>
		HalfVector2,
		/// <summary>
		/// 4d 16-bit floating point vector.
		/// </summary>
		HalfVector4,
		/// <summary>
		/// 64-bit RGBA.
		/// </summary>
		Rgba64,
		/// <summary>
		/// 32-bit color.
		/// </summary>
		Color,
		/// <summary>
		/// 32-bit RGBA.
		/// </summary>
		Rgba32,
		/// <summary>
		/// 32-bit RG.
		/// </summary>
		Rg32,
		/// <summary>
		/// Normalized 16-bit short.
		/// </summary>
		NormalizedShort2,
		/// <summary>
		/// 2 normalized 16-bit shorts.
		/// </summary>
		NormalizedShort4,
		/// <summary>
		/// 3 normalized 10-bit values packed into an int.
		/// </summary>
		Normalized101010,
		/// <summary>
		/// 16-bit short.
		/// </summary>
		Short2,
		/// <summary>
		/// 2 16-bit shorts.
		/// </summary>
		Short4,
		/// <summary>
		/// 4 bytes.
		/// </summary>
		Byte4,
		/// <summary>
		/// 3 normalized 10-bit values packed into an unsigned int.
		/// </summary>
		UInt101010

	}
}

