namespace engenious.Graphics
{
	/// <summary>
	/// Specifies the way a vertex element ought to be used.
	/// </summary>
	public enum VertexElementUsage
	{
		/// <summary>
		/// As normals.
		/// </summary>
		Normal=0,
		/// <summary>
		/// As positions.
		/// </summary>
		Position=1,
		/// <summary>
		/// As texture coordinates.
		/// </summary>
		TextureCoordinate=2,
		/// <summary>
		/// As colors.
		/// </summary>
		Color=3,
		/// <summary>
		/// As custom data.
		/// </summary>
		Custom=4
	}
}

