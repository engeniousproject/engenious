using System;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    [ContentTypeReaderAttribute()]
    public class Texture2DTypeReader:ContentTypeReader<Texture2D>
    {
        public Texture2DTypeReader()
        {
        }

        public override Texture2D Read(ContentManager manager, ContentReader reader)
        {
			

            bool isPng = reader.ReadByte() == 1;
            if (isPng)
            {
                int size = reader.ReadInt32();
                byte[] buffer = reader.ReadBytes(size);
                using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(buffer))
                {
                    using (System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(memStream))
                        return Texture2D.FromBitmap(manager.graphicsDevice, bmp);
                }
            }
            else
            {
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();

                Texture2D text = new Texture2D(manager.graphicsDevice, width, height);
                int[] data = new int[width * height];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = reader.ReadInt32();
                }

                text.SetData<int>(data);

                return text;
            }

        }
    }
}

