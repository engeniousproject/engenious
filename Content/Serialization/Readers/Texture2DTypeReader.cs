using System;
using System.Drawing;
using System.IO;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Content type reader to load <see cref="Texture2D"/> instances.
    /// </summary>
    [ContentTypeReader(typeof(Texture2D))]
    public class Texture2DTypeReader:ContentTypeReader<Texture2D>
    {
        /// <inheritdoc />
        public override Texture2D Read(ContentManagerBase managerBase, ContentReader reader, Type? customType = null)
        {
            var genMipMaps = reader.ReadBoolean();
            var mipCount = reader.ReadInt32();

            int width=reader.ReadInt32(),height=reader.ReadInt32();
            var format = (TextureContentFormat)reader.ReadInt32();
            var hwCompressed = format == TextureContentFormat.DXT1 || format == TextureContentFormat.DXT3 || format == TextureContentFormat.DXT5;
            Texture2D text;
            var size = reader.ReadInt32();
            var buffer = reader.ReadBytes(size);
            if (hwCompressed)
            {
                text = new Texture2D(managerBase.GraphicsDevice,width,height,mipCount,(PixelInternalFormat)format);

                text.SetData(buffer,0,(OpenTK.Graphics.OpenGL.PixelFormat)format);
                //TODO:...
            }
            else
            {
                //text = new Texture2D(manager.GraphicsDevice,width,height,mipCount);
                using var stream = new MemoryStream(buffer);
                using var bmp = new Bitmap(stream);
                text = Texture2D.FromBitmap(managerBase.GraphicsDevice, bmp, mipCount);
            }

            if (genMipMaps)
                return text;
            for (var i=1;i<mipCount;i++)
            {
                size = reader.ReadInt32();
                buffer = reader.ReadBytes(size);
                hwCompressed = format == TextureContentFormat.DXT1 || format == TextureContentFormat.DXT3 || format == TextureContentFormat.DXT5;
                if (hwCompressed)
                    text.SetData(buffer,i,(OpenTK.Graphics.OpenGL.PixelFormat)format);
                else
                {
                    text.SetData(buffer,i);
                }
            }
            return text;
        }

        /// <inheritdoc />
        public Texture2DTypeReader() : base(0)
        {
        }
    }
}

