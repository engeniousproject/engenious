using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using engenious.Content.Serialization;
using engenious.Graphics;
using engenious.Helper;

namespace engenious.Content
{
    /// <summary>
    /// A manager for content files.
    /// </summary>
    public class ContentManager
    {
        private readonly Dictionary<string ,IContentTypeReader> _typeReaders;
        private readonly Dictionary<string ,IContentTypeReader> _typeReadersOutput;
        private readonly Dictionary<string,object> _assets;
        private readonly IFormatter _formatter;
        internal GraphicsDevice GraphicsDevice;

        private static string GetDefaultDirectory()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var dirName = entryAssembly == null ? null : Path.GetDirectoryName(entryAssembly.Location);
            return dirName == null ? Path.GetFullPath("Content") : Path.Combine(dirName, "Content");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to use.</param>
        public ContentManager(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, GetDefaultDirectory())
        {
			
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to use.</param>
        /// <param name="rootDirectory">The root directory to load content files from.</param>
        public ContentManager(GraphicsDevice graphicsDevice, string rootDirectory)
        {
            RootDirectory = rootDirectory;
            GraphicsDevice = graphicsDevice;
            _typeReaders = new Dictionary<string, IContentTypeReader>();
            _typeReadersOutput = new Dictionary<string, IContentTypeReader>();
            _assets = new Dictionary<string, object>();
            _formatter = new BinaryFormatter();
            AddAssembly(Assembly.GetExecutingAssembly());
        }

        internal void AddAssembly(Assembly assembly)
        {
            foreach (var t in assembly.GetTypes())
            {

                if (t.GetInterfaces().Contains(typeof(IContentTypeReader)) && !(t.IsInterface || t.IsAbstract))
                {
                    var attr = (ContentTypeReaderAttribute)t.GetCustomAttributes(typeof(ContentTypeReaderAttribute), true).FirstOrDefault();
                    if (attr != null){
                        var reader = Activator.CreateInstance(t) as IContentTypeReader;
                        _typeReaders.Add(t.FullName, reader);
                        if (attr.OutputType != null)
                            _typeReadersOutput.Add(attr.OutputType.FullName,reader);
                    }
                }
            }
        }
        internal IContentTypeReader GetReaderByOutput(string outputType)
        {
            IContentTypeReader res;
            if (!_typeReadersOutput.TryGetValue(outputType, out res))
                return null;
            return res;
        }
        internal IContentTypeReader GetReader(string reader)
        {
            IContentTypeReader res;
            if (!_typeReaders.TryGetValue(reader, out res))
                return null;
            return res;
        }

        /// <summary>
        /// Gets or sets the root directory.
        /// </summary>
        public string RootDirectory{ get; set; }

        /// <summary>
        /// Unloads an asset by name.
        /// </summary>
        /// <param name="assetName">The asset to unload.</param>
        public void Unload(string assetName)
        {
            object asset;
            if (_assets.TryGetValue(assetName, out asset))
            {
                var disp = asset as IDisposable;
                disp?.Dispose();
            }
        }
        
        /// <summary>
        /// Unloads an asset by name and type.
        /// </summary>
        /// <param name="assetName">The asset to unload.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        public void Unload<T>(string assetName) where T : IDisposable
        {
            object asset;
            if (_assets.TryGetValue(assetName, out asset))
            {
                if (asset is T)
                {
                    var disp = (T) asset;
                    disp.Dispose();
                }
            }
        }

        /// <summary>
        /// Loads an asset by name and type.
        /// </summary>
        /// <param name="assetName">The asset to load.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The loaded asset.</returns>
        public T Load<T>(string assetName) where T : IDisposable
        {
            object asset;
            var containsName =false;
            if (_assets.TryGetValue(assetName, out asset))
            {
                containsName = true;
                if (asset is T)
                {
                    return (T)asset;
                }
            }
            var tmp = ReadAsset<T>(assetName);
            if (tmp != null)
            {
                if (!containsName)
                    _assets.Add(assetName, tmp);
            }
            return tmp;
        }

        /// <summary>
        /// List the content files in <see cref="RootDirectory"/>.
        /// </summary>
        /// <param name="path">The subpath to list the contents of.</param>
        /// <param name="recursive">Whether to recursively search for content files.</param>
        /// <returns>Enumerable of content files in given path.</returns>
        public IEnumerable<string> ListContent(string path,bool recursive=false)
        {
            path = Path.Combine(RootDirectory, path);
            return Directory.GetFiles(path,"*.ego",recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// List the content files in <see cref="RootDirectory"/> with given type.
        /// </summary>
        /// <param name="path">The subpath to list the contents of.</param>
        /// <param name="recursive">Whether to recursively search for content files.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>Enumerable of matching content files in given path.</returns>
        public IEnumerable<string> ListContent<T>(string path,bool recursive=false)//rather slow(needs to load part of files)
        {
            var tp = GetReaderByOutput(typeof(T).FullName);
            foreach(var file in ListContent(path)){
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    var res = ReadContentFileHead(fs, false);
                    if (res == null)
                        continue;
                    Console.WriteLine(res.FileType);
                    if (res.FileType == tp.GetType().FullName)//TODO inheritance
                        yield return file;
                }
            }
        }
        /// <summary>
        /// Reads an asset by given name and type.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The read asset.</returns>
        /// <exception cref="Exception">Not an actual content file, or corrupt content file.</exception>
        protected T ReadAsset<T>(string assetName)
        {
            using (var fs = new FileStream(Path.Combine(RootDirectory, assetName + ".ego"), FileMode.Open, FileAccess.Read))
            {
                var res = ReadContentFileHead(fs);

                return (T)res.Load(this, fs,typeof(T));
            }
            //return default(T);
        }
        private ContentFile ReadContentFileHead(FileStream fs, bool throwsOnError = true)
        {
            ContentFile res;
            var magic = ReadMagic(fs);
            if (magic == ContentFile.MAGIC)
            {
                res = ReadNewContentFile(fs, throwsOnError);
                if (res == null)
                {
                    if (!throwsOnError)
                        return null;
                    throw new Exception("Could not load content file");
                }
            }
            else
            {
                fs.Position -= 4;
                res = ReadDeprecatedContentFile(fs, throwsOnError);
                if (res == null)
                {
                    if (!throwsOnError)
                        return null;
                    throw new Exception("Could not load legacy content file");
                }
            }
            return res;
        }
        private ContentFile ReadContentFileV1(Stream stream, bool throwsOnError = true)
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
            return new ContentFile(contentType);
        }

        public const byte ReaderVersion = 1;
        private ContentFile ReadNewContentFile(Stream stream, bool throwsOnError = true)
        {
            var version = stream.ReadByte();

            switch(version)
            {
                case ReaderVersion:
                    return ReadContentFileV1(stream, throwsOnError);
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
        private ContentFile ReadDeprecatedContentFile(Stream stream, bool throwsOnError = true)
        {
            try
            {
                return _formatter.Deserialize(stream) as ContentFile;
            }
            catch
            {
                if (throwsOnError)
                    throw;
                return null;
            }
        }
    }
}

