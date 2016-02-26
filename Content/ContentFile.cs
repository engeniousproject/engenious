using System;
using System.IO;
using engenious.Content.Serialization;
using engenious.Content;

namespace engenious.Content
{
    [Serializable]
    internal class ContentFile<T> : ContentFile
    {
        public ContentFile()
            : base(typeof(T).Namespace + "." + typeof(T).Name)
        {
        }
			
    }

    [Serializable]
    internal class ContentFile
    {
        internal ContentFile(string type)
        {
            this.FileType = type;
        }

        public string FileType{ get; private set; }

        public object Load(ContentManager manager, Stream stream)
        {
            ContentReader reader = new ContentReader(stream);
            string readName = reader.ReadString();
            IContentTypeReader tp = manager.GetReader(readName);
            return tp.Read(manager, reader);
        }
    }
}

