using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace engenious.Graphics
{
    public sealed class TextureCollection : ICollection<Texture>
    {
        private Texture[] textures;

        public TextureCollection()
        {
            int maxTextures = GL.GetInteger(GetPName.MaxTextureImageUnits);
            textures = new Texture[maxTextures];
        }

        public Texture this [int index]
        { 
            get
            {
                return textures[index];
            }
            set
            {
                if (textures[index] != value)
                {
                    textures[index] = value;
                    GL.ActiveTexture(TextureUnit.Texture0 + index);
                    if (value == null)
                        GL.BindTexture(TextureTarget.Texture2D, 0);
                    else
                        value.Bind();

                }

            }

        }

        #region ICollection implementation

        public int InsertFree(Texture item)
        {
            int ind = IndexOf(item);
            if (ind != -1)
                return ind;
            ind = IndexOf(null);
            if (ind == -1)
                return -1;
            this[ind] = item;
            return ind;
        }

        public void Add(Texture item)
        {
            int free = IndexOf(null);
            if (free == -1)
                return;
            this[free] = item;
        }

        public void Clear()
        {
            for (int i = 0; i < textures.Length; i++)
                this[i] = null;
        }

        public bool Contains(Texture item)
        {
            return IndexOf(item) != -1;
        }

        public int IndexOf(Texture item)
        {
            for (int i = 0; i < textures.Length; i++)
                if (textures[i] == item)
                    return i;
            return -1;
        }

        public void CopyTo(Texture[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Texture item)
        {
            int ind = IndexOf(item);
            if (ind == -1)
                return false;
            this[ind] = null;
            return true;
        }

        public int Count
        {
            get
            {
                return textures.Length;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }


        #endregion

        #region IEnumerable implementation

        public IEnumerator<Texture> GetEnumerator()
        {
            return (IEnumerator<Texture>)textures.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return textures.GetEnumerator();
        }

        #endregion
    }
}

