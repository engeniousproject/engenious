using System;
using System.Collections.Generic;
using engenious.Graphics;

namespace engenious.Content
{
    /// <inheritdoc />
    public class AggregateContentManager : ContentManagerBase
    {
        private readonly Dictionary<string, ContentManagerBase> _contentManagers;
        private readonly ContentManagerBase _fallbackContentManager;
        /// <inheritdoc />
        public AggregateContentManager(GraphicsDevice graphicsDevice, string rootDirectory) : base(graphicsDevice, rootDirectory)
        {
            _contentManagers = new Dictionary<string, ContentManagerBase>();
            _fallbackContentManager = new ResourceContentManager(graphicsDevice, rootDirectory);
            AddContentManager("file", new FileContentManager(graphicsDevice, rootDirectory));
        }

        /// <summary>
        /// Adds a content manager to use for a specific scheme.
        /// </summary>
        /// <param name="scheme">The scheme to use the content manager for.</param>
        /// <param name="contentManager">The content manager to use.</param>
        public void AddContentManager(string scheme, ContentManagerBase contentManager)
        {
            _contentManagers.Add(scheme, contentManager);
        }

        private static Uri PathToUri(string path)
        {
            return new(path.Contains(":") ? path : "file:///" + path);
        }

        private ContentManagerBase GetContentManager(string scheme)
        {
            return _contentManagers.TryGetValue(scheme, out var contentManager) ? contentManager : _fallbackContentManager;
        }

        private ContentManagerBase GetContentManager(Uri uri)
        {
            return GetContentManager(uri.Scheme);
        }

        /// <inheritdoc />
        public override IEnumerable<string> ListContent(Uri path, bool recursive = false)
        {
            return GetContentManager(path).ListContent(path, recursive);
        }

        /// <inheritdoc />
        public override IEnumerable<string> ListContent<T>(Uri path, bool recursive = false)
        {
            return GetContentManager(path).ListContent<T>(path, recursive);
        }
        
        /// <inheritdoc />
        internal override T ReadAsset<T>(Uri assetName)
        {
            return GetContentManager(assetName).ReadAsset<T>(assetName);
        }

        /// <inheritdoc />
        public override IEnumerable<string> ListContent(string path, bool recursive = false) =>
            ListContent(PathToUri(path), recursive);

        /// <inheritdoc />
        public override IEnumerable<string> ListContent<T>(string path, bool recursive = false) =>
            ListContent<T>(PathToUri(path), recursive);

        /// <inheritdoc />
        internal override T ReadAsset<T>(string assetName) => ReadAsset<T>(PathToUri(assetName));
    }
}