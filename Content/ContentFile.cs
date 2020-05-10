using System;
using System.IO;
using engenious.Content.Serialization;

namespace engenious.Content
{
    [Serializable]
    internal sealed class ContentFile
    {
        public const uint MAGIC = 0x45474f43;

        internal ContentFile(string type)
        {
            FileType = type;
        }

        public string FileType{ get; private set; }

        public object Load(ContentManager manager, Stream stream,Type type)
        {
            var reader = new ContentReader(stream);
            var readName = reader.ReadString();
            var tp = manager.GetReaderByOutput(type.FullName);
            if (tp == null)
                tp = manager.GetReader(readName);
            return tp.Read(manager, reader, type);
        }
    }
}

