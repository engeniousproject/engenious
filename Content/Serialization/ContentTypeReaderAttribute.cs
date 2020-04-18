using System;

namespace engenious.Content.Serialization
{
	/// <summary>
	/// Attribute to mark content type readers for discovery.
	/// </summary>
	[AttributeUsage (AttributeTargets.Class)]
	public class ContentTypeReaderAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTypeReaderAttribute"/>.
		/// </summary>
		/// <param name="outputType">The type the marked content type reader, can read.</param>
        public ContentTypeReaderAttribute (Type outputType)
		{
            OutputType = outputType;
		}

		/// <summary>
		/// Gets the type the content type reader is able to read.
		/// </summary>
        public Type OutputType{get;private set;}
	}
}

