using System;

namespace engenious.Content.Serialization
{
	[AttributeUsageAttribute (AttributeTargets.Class)]
	public class ContentTypeReaderAttribute : Attribute
	{
		public ContentTypeReaderAttribute ()
		{
		}
	}
}

