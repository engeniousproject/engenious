using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using engenious.Content.Serialization;
using engenious.Graphics;

namespace engenious.Content
{
    /// <inheritdoc />
    public class FileContentManager : ContentManagerBase
    {
        private static string GetDefaultDirectory()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var dirName = entryAssembly == null ? null : Path.GetDirectoryName(entryAssembly.Location);
            return dirName == null ? Path.GetFullPath("Content") : Path.Combine(dirName, "Content");
        }
        /// <inheritdoc />
        public FileContentManager(GraphicsDevice graphicsDevice) : base(graphicsDevice, GetDefaultDirectory())
        {
        }

        /// <inheritdoc />
        public FileContentManager(GraphicsDevice graphicsDevice, string rootDirectory) : base(graphicsDevice, rootDirectory)
        {
        }

        private static string UriToPath(Uri uri)
        {
            if (uri.Scheme != "file")
                throw new ArgumentException("Invalid uri for this content manager type.");
            return uri.AbsolutePath.Substring(1);
        }

        /// <inheritdoc />
        public override IEnumerable<string> ListContent(Uri path, bool recursive = false) =>
            ListContent(UriToPath(path), recursive);

        /// <inheritdoc />
        public override IEnumerable<string> ListContent<T>(Uri path, bool recursive = false) =>
            ListContent<T>(UriToPath(path), recursive);

        /// <inheritdoc />
        public override IEnumerable<string> ListContent(string path, bool recursive = false)
        {
            path = Path.Combine(RootDirectory, path);
            return Directory.GetFiles(path,"*.ego",recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        /// <inheritdoc />
        public override IEnumerable<string> ListContent<T>(string path,bool recursive=false)//rather slow(needs to load part of files)
        {
            var tp = GetReaderByType<T>();
            foreach(var file in ListContent(path))
            {
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                if (TestContentFile(fs, tp))
                    yield return file;
            }
        }

        /// <inheritdoc />
        internal override T ReadAsset<T>(Uri assetName) => ReadAsset<T>(UriToPath(assetName));

        /// <inheritdoc />
        internal override T ReadAsset<T>(string assetName)
        {
            using (var fs = new FileStream(Path.Combine(RootDirectory, assetName + ".ego"), FileMode.Open, FileAccess.Read))
            {
                var res = ReadContentFileHead(fs);

                return (T)res.Load(this, fs,typeof(T));
            }
        }
    }
}