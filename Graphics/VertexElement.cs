using System;
using OpenToolkit.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a vertex element for a <see cref="VertexDeclaration"/>.
    /// </summary>
    [Serializable]
    public struct VertexElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement"/> struct.
        /// </summary>
        /// <param name="offset">The offset of this vertex element inside a buffer.</param>
        /// <param name="elementFormat">The vertex format of this element.</param>
        /// <param name="elementUsage">The vertex usage of this element.</param>
        /// <param name="usageIndex">A custom usage index for this element</param>
        public VertexElement(
            int offset,
            VertexElementFormat elementFormat,
            VertexElementUsage elementUsage,
            int usageIndex
        )
        {
            Offset = offset;
            VertexElementFormat = elementFormat;
            VertexElementUsage = elementUsage;
            UsageIndex = usageIndex;
        }

        /// <summary>
        /// Gets or sets the offset this element is located at inside a buffer.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the format of this vertex element.
        /// </summary>
        public VertexElementFormat VertexElementFormat { get; set; }

        /// <summary>
        /// Gets or sets an index for custom vertex element usage.
        /// </summary>
        public int UsageIndex { get; set; }

        /// <summary>
        /// Gets or sets what this vertex element is used for.
        /// </summary>
        public VertexElementUsage VertexElementUsage { get; set; }

        internal bool IsNormalized
        {
            get
            {
                switch (VertexElementFormat)
                {
                    case VertexElementFormat.NormalizedShort2:
                    case VertexElementFormat.NormalizedShort4:
                    case VertexElementFormat.Normalized101010:
                        return true;
                    default:
                        return false;
                }
            }
        }

        internal VertexAttribPointerType GetGlVertexDataType()
        {
            switch (VertexElementFormat)
            {
                case VertexElementFormat.Single:
                case VertexElementFormat.Vector2:
                case VertexElementFormat.Vector3:
                case VertexElementFormat.Vector4:
                case VertexElementFormat.Color:
                case VertexElementFormat.Rgba32:
                case VertexElementFormat.Rg32:
                    return VertexAttribPointerType.Float;
                case VertexElementFormat.HalfVector2:
                case VertexElementFormat.HalfVector4:
                    return VertexAttribPointerType.HalfFloat;
                case VertexElementFormat.Rgba64:
                    return VertexAttribPointerType.Double;
                case VertexElementFormat.NormalizedShort2:
                case VertexElementFormat.NormalizedShort4:
                case VertexElementFormat.Short2:
                case VertexElementFormat.Short4:
                    return VertexAttribPointerType.Short;
                case VertexElementFormat.Byte4:
                    return VertexAttribPointerType.Byte;
                case VertexElementFormat.Normalized101010:
                    return VertexAttribPointerType.Int;
                case VertexElementFormat.UInt101010:
                    return VertexAttribPointerType.UnsignedInt;
            }
            throw new ArgumentOutOfRangeException();
        }

        internal int GetGlVertexDataTypeSize()
        {
            switch (GetGlVertexDataType())
            {
                case VertexAttribPointerType.Byte:
                case VertexAttribPointerType.UnsignedByte:
                    return 1;
                case VertexAttribPointerType.Short:
                case VertexAttribPointerType.UnsignedShort:
                case VertexAttribPointerType.HalfFloat:
                    return 2;
                case VertexAttribPointerType.Int:
                case VertexAttribPointerType.UnsignedInt:
                case VertexAttribPointerType.Float:
                case VertexAttribPointerType.UnsignedInt2101010Rev:
                case VertexAttribPointerType.Int2101010Rev:
                case VertexAttribPointerType.Fixed:
                    return 4;
                case VertexAttribPointerType.Double:
                    return 8;
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Gets the number of bytes this vertex element uses.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown on unknown <see cref="VertexElementFormat"/>.</exception>
        public int ByteCount
        {
            get
            {
                switch (VertexElementFormat)
                {
                    case VertexElementFormat.Single:
                        return 4;
                    case VertexElementFormat.Vector2:
                        return 8;
                    case VertexElementFormat.Vector3:
                        return 12;
                    case VertexElementFormat.Vector4:
                        return 16;
                    case VertexElementFormat.HalfVector2:
                        return 4;
                    case VertexElementFormat.HalfVector4:
                        return 8;
                    case VertexElementFormat.Rgba64:
                        return 32;
                    case VertexElementFormat.Color:
                        return 16;
                    case VertexElementFormat.Rgba32:
                        return 16;
                    case VertexElementFormat.Rg32:
                        return 8;
                    case VertexElementFormat.NormalizedShort2:
                        return 4;
                    case VertexElementFormat.NormalizedShort4:
                        return 8;
                    case VertexElementFormat.Normalized101010:
                        return 4;
                    case VertexElementFormat.Short2:
                        return 4;
                    case VertexElementFormat.Short4:
                        return 8;
                    case VertexElementFormat.Byte4:
                        return 4;
                    case VertexElementFormat.UInt101010:
                        return 4;
                }
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}