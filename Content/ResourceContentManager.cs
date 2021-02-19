using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using engenious.Graphics;

namespace engenious.Content
{
    /// <inheritdoc />
    public class ResourceContentManager : ContentManagerBase
    {
        /// <inheritdoc />
        public ResourceContentManager(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            RootDirectory = string.Empty;
        }

        private Assembly? GetAssembly(Uri path)
        {
            if (!string.IsNullOrEmpty(path.Host))
                throw new ArgumentException("Host name not supported be sure to use the format [assemblyName]:///!");
            return Assembly.Load(new AssemblyName(path.Scheme));
        }

        private static bool ResourceNamePathStartsWith(string path, Uri uri, out int restIndex)
        {
            var uriAbs = uri.AbsolutePath;
            restIndex = -1;
            if (uriAbs.Length < 1)
                return false;
            uriAbs = uriAbs.Substring(1);
            if (path.Length < uriAbs.Length)
                return false;
            int i;
            for (i = 0; i < uriAbs.Length; i++)
            {
                var c1 = path[i];
                var c2 = uriAbs[i];
                if (c1 == c2 ||
                    (c1 == '.' && (c2 == Path.DirectorySeparatorChar || c2 == Path.AltDirectorySeparatorChar)))
                {
                    continue;
                }

                return false;
            }

            if (path.Length > i && path[i] == '.')
                i++;
            restIndex = i;
            return true;
        }

        private static IEnumerable<string> ListContent(Assembly? assembly, Uri path, bool recursive = false)
        {
            var res = assembly?.GetManifestResourceNames();
            var asmName = assembly?.GetName().Name;
            if (res == null || asmName == null)
                throw new TypeLoadException($"Could not find or load assembly with name '{path.Scheme}'");
            return res.Where(x => x.EndsWith(".ego")).
                    Select(x => x.Substring(asmName.Length + 1, x.Length - (asmName.Length + 5))).Where(
                            x =>
                            {
                                if (!ResourceNamePathStartsWith(x, path, out var restIndex))
                                    return false;
                                if (recursive)
                                    return true;
                                var foundDot = x.IndexOf('.', restIndex);
                                return foundDot == -1 || foundDot == x.Length - 4;

                            });
        }

        /// <inheritdoc />
        public sealed override string RootDirectory { get; set; }

        /// <inheritdoc />
        public override IEnumerable<string> ListContent(Uri path, bool recursive = false)
        {
            var asm = GetAssembly(path);
            var asmName = asm?.GetName().Name;
            return ListContent(asm, path, recursive).Select(x => asmName + ":///" + x.Replace('.', Path.DirectorySeparatorChar));
        }

        /// <inheritdoc />
        public override IEnumerable<string> ListContent<T>(Uri path, bool recursive = false)
            where T : class
        {
            var tp = GetReaderByType<T>();
            var asm = GetAssembly(path);
            var asmName = asm?.GetName().Name;
            if (tp == null)
                yield break;
            foreach (var r in ListContent(asm, path, recursive))
            {
                var resourceName = asmName + $".{r}.ego";
                using var resourceStream = asm!.GetManifestResourceStream(resourceName);
                if (resourceStream == null)
                    continue;
                if (TestContentFile(resourceStream, tp))
                    yield return asmName + ":///" + r.Replace('.', Path.DirectorySeparatorChar);
            }
        }

        /// <inheritdoc />
        internal override T? ReadAsset<T>(Uri assetName)
            where T : class
        {
            var asm = GetAssembly(assetName);
            var asmName = asm?.GetName();
            if (asm == null || asmName == null)
                throw new TypeLoadException($"Could not find or load assembly with name '{assetName.Scheme}'");
            var resourceName = assetName.AbsolutePath.Replace(Path.DirectorySeparatorChar, '.')
                .Replace(Path.AltDirectorySeparatorChar, '.');

            resourceName = $"{asmName.Name}{resourceName}.ego";
                
            using var resourceStream = asm!.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
                throw new MissingManifestResourceException($"Cannot find resource: {assetName}");
            var res = ReadContentFileHead(resourceStream);

            return (T?)res?.Load(this, resourceStream, typeof(T));
        }

        
        /// <inheritdoc />
        public override IEnumerable<string> ListContent(string path, bool recursive = false) => 
            ListContent(new Uri(path), recursive);

        /// <inheritdoc />
        public override IEnumerable<string> ListContent<T>(string path, bool recursive = false) where T : class =>
            ListContent<T>(new Uri(path), recursive);

        /// <inheritdoc />
        internal override T? ReadAsset<T>(string assetName) where T : class => ReadAsset<T>(new Uri(assetName));
    }
}