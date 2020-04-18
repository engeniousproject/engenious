namespace engenious.Graphics
{
	/// <summary>
	/// Interface for vertex definitions.
	/// </summary>
	public interface IVertexType
	{
		/// <summary>
		/// Gets the <see cref="VertexDeclaration"/> of this vertex type.
		/// </summary>
		VertexDeclaration VertexDeclaration { get; }
	}
}

