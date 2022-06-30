using System;
using System.IO;
using System.Text;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// A extended binary reader able to read some basic engenious types.
    /// </summary>
    public sealed class ContentReader : BinaryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentReader"/> class.
        /// </summary>
        /// <param name="input">The stream to read from.</param>
        public ContentReader(Stream input)
            : base(input)
        {
        }

        /// <summary>
        /// Reads a generic content type.
        /// </summary>
        /// <param name="managerBase">The content manager to use for reading.</param>
        /// <typeparam name="T">The content type to read.</typeparam>
        /// <returns>The read content type.</returns>
        public T? Read<T>(ContentManagerBase managerBase)
        {
            var name = ReadString();
            var typeReader = managerBase.GetReader(name);
            return typeReader == null ? default : Read<T>(managerBase, typeReader);
        }

        /// <summary>
        /// Reads a generic content type using a specific <see cref="IContentTypeReader"/>.
        /// </summary>
        /// <param name="managerBase">The content manager to use for reading.</param>
        /// <param name="typeReader">The type reader to use for reading.</param>
        /// <typeparam name="T">The content type to read.</typeparam>
        /// <returns>The read content type.</returns>
        public T? Read<T>(ContentManagerBase managerBase, IContentTypeReader typeReader)
        {
            return (T?)typeReader.Read(managerBase, this, typeof(T));

        }

        /// <summary>
        /// Reads a <see cref="VertexPositionNormalTexture"/>.
        /// </summary>
        /// <returns>The read <see cref="VertexPositionNormalTexture"/>.</returns>
        public VertexPositionNormalTexture ReadVertexPositionNormalTexture()
        {
            return new VertexPositionNormalTexture(ReadVector3(), ReadVector3(), ReadVector2());
        }

        /// <summary>
        /// Reads a <see cref="VertexPositionColor"/>.
        /// </summary>
        /// <returns>The read <see cref="VertexPositionColor"/>.</returns>
        public VertexPositionColor ReadVertexPositionColor()
        {
            return new VertexPositionColor(ReadVector3(), ReadColor());
        }

        /// <summary>
        /// Reads a <see cref="VertexPositionColorTexture"/>.
        /// </summary>
        /// <returns>The read <see cref="VertexPositionColorTexture"/>.</returns>
        public VertexPositionColorTexture ReadVertexPositionColorTexture()
        {
            return new VertexPositionColorTexture(ReadVector3(), ReadColor(), ReadVector2());
        }

        /// <summary>
        /// Reads a <see cref="VertexPositionTexture"/>.
        /// </summary>
        /// <returns>The read <see cref="VertexPositionTexture"/>.</returns>
        public VertexPositionTexture ReadVertexPositionTexture()
        {
            return new VertexPositionTexture(ReadVector3(), ReadVector2());
        }

        /// <summary>
        /// Reads a <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The read <see cref="Matrix"/>.</returns>
        public Matrix ReadMatrix()
        {
            return new Matrix(ReadVector4(), ReadVector4(), ReadVector4(), ReadVector4());
        }

        /// <summary>
        /// Reads a <see cref="Quaternion"/>.
        /// </summary>
        /// <returns>The read <see cref="Quaternion"/>.</returns>
        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The read <see cref="Vector2"/>.</returns>
        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a <see cref="Vector3"/>.
        /// </summary>
        /// <returns>The read <see cref="Vector3"/>.</returns>
        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a <see cref="Vector4"/>.
        /// </summary>
        /// <returns>The read <see cref="Vector4"/>.</returns>
        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a <see cref="Color"/>.
        /// </summary>
        /// <returns>The read <see cref="Matrix"/>.</returns>
        public Color ReadColor()
        {
            return new Color(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a <see cref="Rune"/>.
        /// </summary>
        /// <returns>The read <see cref="Rune"/>.</returns>
        public Rune ReadRune()
        {
            return new Rune(ReadInt32());
        }
    }
}

