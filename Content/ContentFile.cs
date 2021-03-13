using System;
using System.IO;
using engenious.Content.Serialization;

namespace engenious.Content
{
    /// <summary>
    /// A class for describing a content file.
    /// </summary>
    [Serializable]
    public sealed class ContentFile
    {
        /// <summary>
        /// The magic header for content files.
        /// </summary>
        public const uint Magic = 0x45474f43;

        internal ContentFile(string type, uint contentVersion)
        {
            FileType = type;
            ContentVersion = contentVersion;
        }

        /// <summary>
        /// Gets the type of the file.
        /// </summary>
        public string FileType{ get; private set; }

        /// <summary>
        /// Gets or sets a version number for the content.
        /// </summary>
        public uint ContentVersion { get; private set; }

        /// <summary>
        /// Tries to load the content file as a specified type from a stream.
        /// </summary>
        /// <param name="managerBase">The content manager to read the file with.</param>
        /// <param name="stream">The stream to read the file from.</param>
        /// <param name="type">The type to try to read the file as.</param>
        /// <returns>The read content file.</returns>
        public object? Load(ContentManagerBase managerBase, Stream stream, Type type)
        {
            var reader = new ContentReader(stream);
            var readName = reader.ReadString();
            var tp = managerBase.GetReaderByOutput(type.FullName) ?? managerBase.GetReader(readName);

            if (tp == null)
                return null;
            if (tp.ContentVersion != ContentVersion)
                throw new NotSupportedException(
                    $"EGO content version mismatch: File - {ContentVersion} != Reader - {tp.ContentVersion}");
            
            return tp.Read(managerBase, reader, type);
        }
    }
}

