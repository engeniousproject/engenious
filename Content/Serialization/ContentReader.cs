using System;
using System.IO;
using OpenTK;

namespace engenious.Content.Serialization
{
    public sealed class ContentReader :BinaryReader
    {
        public ContentReader(Stream input)
            : base(input)
        {
        }

        public T Read<T>(ContentManager manager)
        {
            string name = ReadString();
            IContentTypeReader typeReader = manager.GetReader(name);
            if (typeReader == null)
                return default(T);
            return Read<T>(manager, typeReader);
        }

        public T Read<T>(ContentManager manager, IContentTypeReader typeReader)
        {
            return (T)typeReader.Read(manager, this);

        }

        public engenious.Graphics.VertexPositionNormalTexture ReadVertexPositionNormalTexture()
        {
            return new engenious.Graphics.VertexPositionNormalTexture(ReadVector3(), ReadVector3(), ReadVector2());
        }

        public engenious.Graphics.VertexPositionColor ReadVertexPositionColor()
        {
            return new engenious.Graphics.VertexPositionColor(ReadVector3(), ReadColor());
        }

        public engenious.Graphics.VertexPositionColorTexture ReadVertexPositionColorTexture()
        {
            return new engenious.Graphics.VertexPositionColorTexture(ReadVector3(), ReadColor(), ReadVector2());
        }

        public engenious.Graphics.VertexPositionTexture ReadVertexPositionTexture()
        {
            return new engenious.Graphics.VertexPositionTexture(ReadVector3(), ReadVector2());
        }

        public Matrix ReadMatrix()
        {
            return new Matrix(ReadVector4(), ReadVector4(), ReadVector4(), ReadVector4());
        }

        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Color ReadColor()
        {
            return new Color(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }
    }
}

