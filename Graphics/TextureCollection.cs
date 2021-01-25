using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace engenious.Graphics
{
    /// <summary>
    /// A collection of the <see cref="Texture"/> class.
    /// </summary>
    public sealed class TextureCollection : ICollection<Texture>
    {
        public class TextureSlotReference
        {
            private readonly TextureCollection _collection;
            private int _refCount;
            public int Slot { get; }

            public TextureSlotReference(TextureCollection collection, int slot)
            {
                _collection = collection;
                Slot = slot;
                _refCount = 0;
            }

            /// <summary>
            /// Aquires one reference.
            /// </summary>
            public void Acquire()
            {
                _refCount++;
            }

            /// <summary>
            /// Releases one reference.
            /// </summary>
            public void Release()
            {
                _refCount--;
                if (_refCount == 0)
                {
                    _collection[Slot] = null;
                }
            }
        }
        private readonly Texture[] _textures;
        private readonly TextureSlotReference[] _textureSlotReferences;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureCollection"/> class.
        /// </summary>
        public TextureCollection()
        {
            var maxTextures = GL.GetInteger(GetPName.MaxTextureImageUnits);
            _textures = new Texture[maxTextures];
            _textureSlotReferences = new TextureSlotReference[maxTextures];
            for (int i = 0; i < _textureSlotReferences.Length; i++)
            {
                _textureSlotReferences[i] = new TextureSlotReference(this, i);
            }
        }

        /// <summary>
        /// Gets a texture at a given index.
        /// </summary>
        /// <param name="index">The index of the texture to get.</param>
        public Texture this [int index]
        { 
            get
            {
                return _textures[index];
            }
            set
            {
                if (_textures[index] != value)
                {
                    _textures[index] = value;
                    GL.ActiveTexture(TextureUnit.Texture0 + index);
                    if (value == null)
                        GL.BindTexture(TextureTarget.Texture2D, 0);
                    else
                        value.Bind();

                }

            }

        }

        #region ICollection implementation

        /// <summary>
        /// Inserts a texture into a free texture slot and returns the index it was inserted at.
        /// </summary>
        /// <param name="item">The texture to insert.</param>
        /// <returns>The insert position, or -1 if no free slot was available.</returns>
        public TextureSlotReference? InsertFree(Texture item)
        {
            var ind = IndexOf(item);
            if (ind != -1)
                return _textureSlotReferences[ind];
            ind = IndexOf(null);
            if (ind == -1)
                return null;
            this[ind] = item;
            return _textureSlotReferences[ind];
        }

        /// <inheritdoc />
        public void Add(Texture item)
        {
            var free = IndexOf(null);
            if (free == -1)
                return;
            this[free] = item;
        }

        /// <inheritdoc />
        public void Clear()
        {
            for (var i = 0; i < _textures.Length; i++)
                this[i] = null;
        }

        /// <inheritdoc />
        public bool Contains(Texture item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Gets the index of a texture already inside the collection, or -1 if none is found.
        /// </summary>
        /// <param name="item">The element to search for.</param>
        /// <returns>The index of the item searched for if found; otherwise -1</returns>
        public int IndexOf(Texture item)
        {
            for (var i = 0; i < _textures.Length; i++)
                if (_textures[i] == item)
                    return i;
            return -1;
        }

        /// <inheritdoc />
        public void CopyTo(Texture[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Remove(Texture item)
        {
            var ind = IndexOf(item);
            if (ind == -1)
                return false;
            this[ind] = null;
            return true;
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return _textures.Length;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }


        #endregion

        #region IEnumerable implementation

        /// <inheritdoc />
        public IEnumerator<Texture> GetEnumerator()
        {
            return (IEnumerator<Texture>)_textures.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _textures.GetEnumerator();
        }

        #endregion
    }
}

