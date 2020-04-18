using System.Runtime.InteropServices;

namespace engenious
{
    /// <summary>
    /// Defines a RGBA <see cref="byte"/> Color.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorByte
    {

        /// <summary>
        /// Initializes a new <see cref="ColorByte"/> struct using bytes.
        /// </summary>
        /// <param name="r">A byte representing the red component.</param>
        /// <param name="g">A byte representing the green component.</param>
        /// <param name="b">A byte representing the blue component.</param>
        /// <param name="a">A byte representing the alpha component.</param>
        public ColorByte(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Initializes a new <see cref="ColorByte"/> struct using floats.
        /// </summary>
        /// <param name="r">A float representing the red component.</param>
        /// <param name="g">A float representing the green component.</param>
        /// <param name="b">A float representing the blue component.</param>
        /// <param name="a">A float representing the alpha component.</param>
        public ColorByte(float r, float g, float b, float a = 1.0f)
            : this((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), (byte)(a * 255))
        {
            
        }

        /// <summary>
        /// Initializes a new <see cref="ColorByte"/> struct using an existing <see cref="Color"/> and a new alpha value.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to use the RGB-components from.</param>
        /// <param name="a">The new alpha component.</param>
        public ColorByte(Color color, float a)
            : this(color.R, color.G, color.B, a)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ColorByte"/> struct using an existing <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="System.Drawing.Color"/>.</param>
        public ColorByte(System.Drawing.Color color)
            : this(color.R, color.G, color.B, color.A)
        {
        }

        /// <summary>
        /// Gets the red component.
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// Gets the green component.
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Gets the blue component.
        /// </summary>
        public byte B { get; }

        /// <summary>
        /// Gets the alpha component.
        /// </summary>
        public byte A { get; }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return A << 24 | R << 16 | G << 8 | B;//TODO?
        }

        /// <summary>
        /// Implicitly converts the <see cref="ColorByte"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="col">The <see cref="ColorByte"/>.</param>
        /// <returns>The resulting <see cref="Color"/>.</returns>
        public static implicit operator Color(ColorByte col)
        {
            return new Color(col.R, col.G, col.B, col.A);
        }
    }
}

