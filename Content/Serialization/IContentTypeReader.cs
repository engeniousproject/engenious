using System;

namespace engenious.Content.Serialization
{
	/// <summary>
	/// The interface for content type readers.
	/// </summary>
	public interface IContentTypeReader
	{
		/// <summary>
		/// Reads a content object.
		/// </summary>
		/// <param name="managerBase">The content manager to use to load the content object.</param>
		/// <param name="reader">The content reader containing the content data.</param>
		/// <param name="customType">The custom type the content object should be loaded as.</param>
		/// <returns>The loaded content object.</returns>
		object? Read(ContentManagerBase managerBase, ContentReader reader,Type? customType = null);
		
		/// <summary>
		/// Gets the content version this content reader can read.
		/// </summary>
		public uint ContentVersion { get; }
	}
}

