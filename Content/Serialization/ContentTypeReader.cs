using System;

namespace engenious.Content.Serialization
{
	/// <summary>
	/// Abstract content type reader base class
	/// </summary>
	/// <typeparam name="T">The type the content type reader is able to read.</typeparam>
	public abstract class ContentTypeReader<T> : IContentTypeReader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTypeReader{T}"/> class.
		/// </summary>
		/// <param name="contentVersion">The content version this content reader is able to read.</param>
		protected ContentTypeReader(uint contentVersion)
		{
			ContentVersion = contentVersion;
		}

		/// <inheritdoc cref="IContentTypeReader.Read"/>
	    public abstract T? Read (ContentManagerBase managerBase, ContentReader reader,Type? customType = null);

		/// <inheritdoc />
		public uint ContentVersion { get; }

		object? IContentTypeReader.Read (ContentManagerBase managerBase, ContentReader reader, Type? customType)
		{
			return Read (managerBase, reader,customType);
		}
	}
}

