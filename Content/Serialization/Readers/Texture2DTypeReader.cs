using System;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    [ContentTypeReaderAttribute(typeof(Texture2D))]
    public class Texture2DTypeReader:ContentTypeReader<Texture2D>
    {
        public Texture2DTypeReader()
        {
        }

        public override Texture2D Read(ContentManager manager, ContentReader reader)
        {
            bool genMipMaps = reader.ReadBoolean();
            int mipCount = reader.ReadInt32();

            int width=reader.ReadInt32(),height=reader.ReadInt32();
            TextureContentFormat format = (TextureContentFormat)reader.ReadInt32();
            bool hwCompressed = format == TextureContentFormat.DXT1 || format == TextureContentFormat.DXT3 || format == TextureContentFormat.DXT5;
            Texture2D text;
            int size = reader.ReadInt32();
            byte[] buffer = reader.ReadBytes(size);
            if (hwCompressed)
            {
                text = new Texture2D(manager.graphicsDevice,width,height,mipCount,(PixelInternalFormat)format);

                text.SetData<byte>(buffer,0,(OpenTK.Graphics.OpenGL4.PixelFormat)format);
                //TODO:...
            }
            else
            {
                text = new Texture2D(manager.graphicsDevice,width,height,mipCount);
                using (var stream = new System.IO.MemoryStream(buffer))
                    text = Texture2D.FromBitmap(manager.graphicsDevice,new System.Drawing.Bitmap(stream),mipCount);
            }

            if (genMipMaps)
                return text;
            for (int i=1;i<mipCount;i++)
            {
                size = reader.ReadInt32();
                buffer = reader.ReadBytes(size);
                hwCompressed = format == TextureContentFormat.DXT1 || format == TextureContentFormat.DXT3 || format == TextureContentFormat.DXT5;
                if (hwCompressed)
                    text.SetData<byte>(buffer,i,(OpenTK.Graphics.OpenGL4.PixelFormat)format);
                else{
                    using (var stream = new System.IO.MemoryStream(buffer))
                        text.SetData<byte>(buffer,i);
                }
            }
            return text;
        }
    }
}

