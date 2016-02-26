using System;

namespace engenious
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct ColorByte
    {

        public ColorByte(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public ColorByte(float r, float g, float b, float a = 1.0f)
            : this((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), (byte)(a * 255))
        {
            
        }

        public ColorByte(Color color, float a)
            : this(color.R, color.G, color.B, a)
        {
        }

        public ColorByte(System.Drawing.Color color)
            : this(color.R, color.G, color.B, color.A)
        {
        }

        private byte r, g, b, a;

        public byte R{ get { return r; } }

        public byte G{ get { return g; } }

        public byte B{ get { return b; } }

        public byte A{ get { return a; } }

        public override int GetHashCode()
        {
            return (int)(A << 24 | R << 16 | G << 8 | B);//TODO?
        }

        public static implicit operator Color(ColorByte col)
        {
            return new Color(col.R, col.G, col.B, col.A);
        }
    }
}

