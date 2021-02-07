using System;

namespace engenious.Content.Serialization
{
	/// <summary>
	/// Abstract content type reader base class
	/// </summary>
	/// <typeparam name="T">The type the content type reader is able to read.</typeparam>
	public abstract class ContentTypeReader<T> : IContentTypeReader
	{
		/// <inheritdoc cref="IContentTypeReader.Read"/>
	    public abstract T Read (ContentManagerBase managerBase, ContentReader reader,Type customType = null);

		object IContentTypeReader.Read (ContentManagerBase managerBase, ContentReader reader,Type customType)
		{
			return Read (managerBase, reader,customType);
		}
	}
}

