using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using engenious.Content.Serialization;
using engenious.Graphics;
using engenious.Helper;

namespace engenious.Content
{
    /// <summary>
    /// A manager for content files.
    /// </summary>
    public abstract class ContentManagerBase
    {
        private readonly Dictionary<string ,IContentTypeReader> _typeReaders;
        private readonly Dictionary<string ,IContentTypeReader> _typeReadersOutput;
        private readonly Dictionary<string,object> _assets;
        internal GraphicsDevice GraphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManagerBase"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to use.</param>
        public ContentManagerBase(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            _typeReaders = new Dictionary<string, IContentTypeReader>();
            _typeReadersOutput = new Dictionary<string, IContentTypeReader>();
            _assets = new Dictionary<string, object>();
            AddAssembly(Assembly.GetExecutingAssembly());
        }

        internal void AddAssembly(Assembly assembly)
        {
            foreach (var t in assembly.GetTypes())
            {
                if (!t.GetInterfaces().Contains(typeof(IContentTypeReader)) || t.IsInterface || t.IsAbstract)
                    continue;
                var attr = (ContentTypeReaderAttribute?)t.GetCustomAttributes(typeof(ContentTypeReaderAttribute), true).FirstOrDefault();
                if (attr == null || t.FullName == null)
                    continue;
                var reader = Activator.CreateInstance(t) as IContentTypeReader;
                if (reader == null)
                    continue;
                _typeReaders.Add(t.FullName, reader);
                if (attr.OutputType != null && attr.OutputType.FullName != null)
                    _typeReadersOutput.Add(attr.OutputType.FullName, reader);
            }
        }
        internal IContentTypeReader? GetReaderByType<T>()
        {
            return GetReaderByOutput(typeof(T).FullName);
        }
        internal IContentTypeReader? GetReaderByOutput(string? outputType)
        {
            if (outputType == null)
                return null;
            return _typeReadersOutput.TryGetValue(outputType, out var res) ? res : null;
        }
        internal IContentTypeReader? GetReader(string reader)
        {
            return _typeReaders.TryGetValue(reader, out var res) ? res : null;
        }
        
        /// <summary>
        /// Test whether a stream contains a content file.
        /// </summary>
        /// <param name="stream">The stream to test.</param>
        /// <param name="tp">The content type reader to test with.</param>
        /// <returns>Whether the stream contains a matching content file.</returns>
        protected bool TestContentFile(Stream stream, IContentTypeReader tp)
        {
            var res = ReadContentFileHead(stream, false);
            if (res == null)
                return false;
            Console.WriteLine(res.FileType);
            return res.FileType == tp.GetType().FullName; // TODO: inheritance
        }
        
        /// <summary>
        /// Gets or sets the root directory.
        /// </summary>
        public abstract string RootDirectory{ get; set; }

        /// <summary>
        /// Unloads an asset by name.
        /// </summary>
        /// <param name="assetName">The asset to unload.</param>
        public void Unload(string assetName)
        {
            if (!_assets.TryGetValue(assetName, out var asset))
                return;
            var disposable = asset as IDisposable;
            disposable?.Dispose();
        }
        
        /// <summary>
        /// Unloads an asset by name and type.
        /// </summary>
        /// <param name="assetName">The asset to unload.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        public void Unload<T>(string assetName) where T : IDisposable
        {
            if (!_assets.TryGetValue(assetName, out var asset))
                return;
            if (asset is T stronglyTypedAsset)
            {
                stronglyTypedAsset.Dispose();
            }
        }

        /// <summary>
        /// Loads an asset by name and type.
        /// </summary>
        /// <param name="assetName">The asset to load.</param>
        /// <param name="useCache">Whether to try loading from cache or not.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The loaded asset.</returns>
        public T? Load<T>(string assetName, bool useCache = true) where T : class, IDisposable
        {
            if (useCache && _assets.TryGetValue(assetName, out var asset))
            {
                if (asset is T value)
                {
                    return value;
                }
            }
            var tmp = ReadAsset<T>(assetName);
            if (tmp != null)
            {
                _assets[assetName] = tmp;
            }
            return tmp;
        }
        
        /// <summary>
        /// List the content files in <see cref="RootDirectory"/>.
        /// </summary>
        /// <param name="path">The subpath to list the contents of.</param>
        /// <param name="recursive">Whether to recursively search for content files.</param>
        /// <returns>Enumerable of content files in given path.</returns>
        public abstract IEnumerable<string> ListContent(Uri path, bool recursive = false);

        /// <summary>
        /// List the content files in <see cref="RootDirectory"/> with given type.
        /// </summary>
        /// <param name="path">The subpath to list the contents of.</param>
        /// <param name="recursive">Whether to recursively search for content files.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>Enumerable of matching content files in given path.</returns>
        public abstract IEnumerable<string> ListContent<T>(Uri path, bool recursive = false) where T : class;

        /// <summary>
        /// List the content files in <see cref="RootDirectory"/>.
        /// </summary>
        /// <param name="path">The subpath to list the contents of.</param>
        /// <param name="recursive">Whether to recursively search for content files.</param>
        /// <returns>Enumerable of content files in given path.</returns>
        public abstract IEnumerable<string> ListContent(string path, bool recursive = false);

        /// <summary>
        /// List the content files in <see cref="RootDirectory"/> with given type.
        /// </summary>
        /// <param name="path">The subpath to list the contents of.</param>
        /// <param name="recursive">Whether to recursively search for content files.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>Enumerable of matching content files in given path.</returns>
        public abstract IEnumerable<string> ListContent<T>(string path, bool recursive = false) where T : class;

        /// <summary>
        /// Reads an asset by given name and type.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The read asset.</returns>
        /// <exception cref="Exception">Not an actual content file, or corrupt content file.</exception>
        internal abstract T? ReadAsset<T>(Uri assetName) where T : class;

        /// <summary>
        /// Reads an asset by given name and type.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The read asset.</returns>
        /// <exception cref="Exception">Not an actual content file, or corrupt content file.</exception>
        internal abstract T? ReadAsset<T>(string assetName) where T : class;

        /// <summary>
        /// Reads the header of a content file
        /// </summary>
        /// <param name="stream">The stream to read the header from.</param>
        /// <param name="throwsOnError">Whether the method should throw an exception on failure.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown when the file could not be loaded.</exception>
        internal ContentFile? ReadContentFileHead(Stream stream, bool throwsOnError = true)
        {
            ContentFile? res;
            var magic = ReadMagic(stream);
            if (magic == ContentFile.Magic)
            {
                res = ReadNewContentFile(stream, throwsOnError);
                if (res != null)
                    return res;
                if (!throwsOnError)
                    return null;
                throw new Exception("Could not load content file");
            }
            // Legacy loading
            if (!throwsOnError)
                return null;
            throw new NotSupportedException("Legacy content file no longer supported!");
        }
        private ContentFile? ReadContentFileV2(Stream stream, bool throwsOnError = true)
        {
            var contentTypeLen = ReadUIntLE(stream);
            var buffer = new byte[contentTypeLen];
            if (contentTypeLen < stream.Read(buffer, 0, buffer.Length))
            {
                if (!throwsOnError)
                    return null;
                throw new Exception("Could not load content file: Out of data");
            }
            string contentType = System.Text.Encoding.UTF8.GetString(buffer);
            return new ContentFile(contentType, ReadUIntLE(stream));
        }

        /// <summary>
        /// The currently implemented content file reader version.
        /// </summary>
        public const byte ReaderVersion = 2;
        private ContentFile? ReadNewContentFile(Stream stream, bool throwsOnError = true)
        {
            var version = stream.ReadByte();

            switch(version)
            {
                case ReaderVersion:
                    return ReadContentFileV2(stream, throwsOnError);
                default:
                    if (throwsOnError)
                        throw new Exception($"Content file version {version} not supported");
                    return null;
            }
        }

        private unsafe uint ReadUInt(Stream str)
        {
            var uintBytes = new byte[4];
            str.Read(uintBytes, 0, uintBytes.Length);
            return BitConverter.ToUInt32(uintBytes, 0);
        }
        private unsafe uint ReadUIntLE(Stream str)
        {
            var val = ReadUInt(str);
            return BitHelper.BitConverterToLittleEndian(val);
        }
        private unsafe uint ReadMagic(Stream str) // Big Endian
        {
            var val = ReadUInt(str);
            return BitHelper.BitConverterToBigEndian(val);
        }
    }
}

