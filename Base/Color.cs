// ReSharper disable CompareOfFloatsByEqualityOperator
namespace engenious
{
    /// <summary>
    /// Defines a RGBA <see cref="float"/> Color.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct Color
    {
        private readonly Vector4 _color;

        /// <summary>
        /// Initializes a new <see cref="Color"/> struct using bytes.
        /// </summary>
        /// <param name="r">A byte representing the red component.</param>
        /// <param name="g">A byte representing the green component.</param>
        /// <param name="b">A byte representing the blue component.</param>
        /// <param name="a">A byte representing the alpha component.</param>
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            _color = new Vector4(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        /// <summary>
        /// Initializes a new <see cref="Color"/> struct using floats.
        /// </summary>
        /// <param name="r">A float representing the red component.</param>
        /// <param name="g">A float representing the green component.</param>
        /// <param name="b">A float representing the blue component.</param>
        /// <param name="a">A float representing the alpha component.</param>
        public Color(float r, float g, float b, float a = 1.0f)
        {
            _color = new Vector4(r, g, b, a);
        }

        /// <summary>
        /// Initializes a new <see cref="Color"/> struct using an existing <see cref="Color"/> and a new alpha value.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to use the RGB-components from.</param>
        /// <param name="a">The new alpha component.</param>
        public Color(Color color, float a)
            : this(color.R, color.G, color.B, a)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Color"/> struct using an existing <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="System.Drawing.Color"/>.</param>
        public Color(System.Drawing.Color color)
            : this(color.R, color.G, color.B, color.A)
        {
        }

        /// <summary>
        /// Gets the red component.
        /// </summary>
        public float R => _color.X;

        /// <summary>
        /// Gets the green component.
        /// </summary>
        public float G => _color.Y;

        /// <summary>
        /// Gets the blue component.
        /// </summary>
        public float B => _color.Z;

        /// <summary>
        /// Gets the alpha component.
        /// </summary>
        public float A => _color.W;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _color.GetHashCode();
        }

        /// <summary>
        /// Implicitly converts the <see cref="Color"/> to a <see cref="ColorByte"/>.
        /// </summary>
        /// <param name="col">The <see cref="Color"/>.</param>
        /// <returns>The resulting <see cref="ColorByte"/>.</returns>
        public static implicit operator ColorByte(Color col)
        {
            return new ColorByte(col.R, col.G, col.B, col.A);
        }

        /// <summary>
        /// Implicitly converts the <see cref="Color"/> to a <see cref="Vector4"/>.
        /// </summary>
        /// <param name="col">The <see cref="Color"/>.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static implicit operator Vector4(Color col)
        {
            return col.ToVector4();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Color)) return false;
            var sec = (Color)obj;
            return A == sec.A && R == sec.R && G == sec.G && B == sec.B;
        }

        /// <summary>
        /// Compares two <see cref="Color"/> structs for equality using its RGBA components.
        /// </summary>
        /// <param name="a">The first <see cref="Color"/> to compare.</param>
        /// <param name="b">The second <see cref="Color"/> to compare.</param>
        /// <returns><c>true</c> if the colors are the same; otherwise <c>false</c>.</returns>
        public static bool operator ==(Color a, Color b)
        {
            return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
        }

        /// <summary>
        /// Compares two <see cref="Color"/> structs for inequality using its RGBA components.
        /// </summary>
        /// <param name="a">The first <see cref="Color"/> to compare.</param>
        /// <param name="b">The second <see cref="Color"/> to compare.</param>
        /// <returns><c>true</c> if the colors aren't the same; otherwise <c>false</c>.</returns>
        public static bool operator !=(Color a, Color b)
        {
            return a.A != b.A || a.R != b.R || a.G != b.G || a.B != b.B;
        }

        /// <summary>
        /// Multiplies the <see cref="Color"/> struct componentwise.
        /// </summary>
        /// <param name="value">The <see cref="Color"/> to scale.</param>
        /// <param name="scale">The scale to multiply the RGBA components with.</param>
        /// <returns>The scaled <see cref="Color"/>.</returns>
        public static Color operator *(Color value, float scale)
        {
            return new Color(value.R * scale, value.G * scale, value.B * scale, value.A * scale);
        }

        /// <summary>
        /// Adds up two <see cref="Color"/> structs componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Color"/> summand.</param>
        /// <param name="value2">The second <see cref="Color"/> summand.</param>
        /// <returns>The summed up <see cref="Color"/>.</returns>
        /// <remarks>No Clamping to [0;1] is applied.</remarks>
        public static Color operator +(Color value1, Color value2)
        {
            return new Color(value1.R + value2.R, value1.G + value2.G, value1.B + value2.B, value1.A + value2.A);
        }

        /// <summary>
        /// Converts the <see cref="Color"/> to a <see cref="Vector4"/>.
        /// </summary>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public Vector4 ToVector4()
        {
            return _color;
        }

        #region Colors

        static Color()
        {
            AliceBlue = new Color(System.Drawing.Color.AliceBlue);
            AntiqueWhite = new Color(System.Drawing.Color.AntiqueWhite);
            Aqua = new Color(System.Drawing.Color.Aqua);
            Aquamarine = new Color(System.Drawing.Color.Aquamarine);
            Azure = new Color(System.Drawing.Color.Azure);
            Beige = new Color(System.Drawing.Color.Beige);
            Bisque = new Color(System.Drawing.Color.Bisque);
            Black = new Color(0.0f, 0.0f, 0.0f);
            BlanchedAlmond = new Color(System.Drawing.Color.BlanchedAlmond);
            Blue = new Color(0.0f, 0.0f, 1.0f);
            BlueViolet = new Color(System.Drawing.Color.BlueViolet);
            Brown = new Color(System.Drawing.Color.Brown);
            BurlyWood = new Color(System.Drawing.Color.BurlyWood);
            CadetBlue = new Color(System.Drawing.Color.CadetBlue);
            Chartreuse = new Color(System.Drawing.Color.Chartreuse);
            Chocolate = new Color(System.Drawing.Color.Chocolate);
            Coral = new Color(System.Drawing.Color.Coral);
            CornflowerBlue = new Color(System.Drawing.Color.CornflowerBlue);
            Cornsilk = new Color(System.Drawing.Color.Cornsilk);
            Crimson = new Color(System.Drawing.Color.Crimson);
            Cyan = new Color(System.Drawing.Color.Cyan);
            DarkBlue = new Color(System.Drawing.Color.DarkBlue);
            DarkCyan = new Color(System.Drawing.Color.DarkCyan);
            DarkGoldenrod = new Color(System.Drawing.Color.DarkGoldenrod);
            DarkGray = new Color(System.Drawing.Color.DarkGray);
            DarkGreen = new Color(System.Drawing.Color.DarkGreen);
            DarkKhaki = new Color(System.Drawing.Color.DarkKhaki);
            DarkMagenta = new Color(System.Drawing.Color.DarkMagenta);
            DarkOliveGreen = new Color(System.Drawing.Color.DarkOliveGreen);
            DarkOrange = new Color(System.Drawing.Color.DarkOrange);
            DarkOrchid = new Color(System.Drawing.Color.DarkOrchid);
            DarkRed = new Color(System.Drawing.Color.DarkRed);
            DarkSalmon = new Color(System.Drawing.Color.DarkSalmon);
            DarkSeaGreen = new Color(System.Drawing.Color.DarkSeaGreen);
            DarkSlateBlue = new Color(System.Drawing.Color.DarkSlateBlue);
            DarkSlateGray = new Color(System.Drawing.Color.DarkSlateGray);
            DarkTurquoise = new Color(System.Drawing.Color.DarkTurquoise);
            DarkViolet = new Color(System.Drawing.Color.DarkViolet);
            DeepPink = new Color(System.Drawing.Color.DeepPink);
            DeepSkyBlue = new Color(System.Drawing.Color.DeepSkyBlue);
            DimGray = new Color(System.Drawing.Color.DimGray);
            DodgerBlue = new Color(System.Drawing.Color.DodgerBlue);
            Firebrick = new Color(System.Drawing.Color.Firebrick);
            FloralWhite = new Color(System.Drawing.Color.FloralWhite);
            ForestGreen = new Color(System.Drawing.Color.ForestGreen);
            Fuchsia = new Color(System.Drawing.Color.Fuchsia);
            Gainsboro = new Color(System.Drawing.Color.Gainsboro);
            GhostWhite = new Color(System.Drawing.Color.GhostWhite);
            Gold = new Color(System.Drawing.Color.Gold);
            Goldenrod = new Color(System.Drawing.Color.Goldenrod);
            Gray = new Color(System.Drawing.Color.Gray);
            Green = new Color(0.0f, 1.0f, 0.0f);
            GreenYellow = new Color(System.Drawing.Color.GreenYellow);
            Honeydew = new Color(System.Drawing.Color.Honeydew);
            HotPink = new Color(System.Drawing.Color.HotPink);
            IndianRed = new Color(System.Drawing.Color.IndianRed);
            Indigo = new Color(System.Drawing.Color.Indigo);
            Ivory = new Color(System.Drawing.Color.Ivory);
            Khaki = new Color(System.Drawing.Color.Khaki);
            Lavender = new Color(System.Drawing.Color.Lavender);
            LavenderBlush = new Color(System.Drawing.Color.LavenderBlush);
            LawnGreen = new Color(System.Drawing.Color.LawnGreen);
            LemonChiffon = new Color(System.Drawing.Color.LemonChiffon);
            LightBlue = new Color(System.Drawing.Color.LightBlue);
            LightCoral = new Color(System.Drawing.Color.LightCoral);
            LightCyan = new Color(System.Drawing.Color.LightCyan);
            LightGoldenrodYellow = new Color(System.Drawing.Color.LightGoldenrodYellow);
            LightGray = new Color(System.Drawing.Color.LightGray);
            LightGreen = new Color(System.Drawing.Color.LightGreen);
            LightPink = new Color(System.Drawing.Color.LightPink);
            LightSalmon = new Color(System.Drawing.Color.LightSalmon);
            LightSeaGreen = new Color(System.Drawing.Color.LightSeaGreen);
            LightSkyBlue = new Color(System.Drawing.Color.LightSkyBlue);
            LightSlateGray = new Color(System.Drawing.Color.LightSlateGray);
            LightSteelBlue = new Color(System.Drawing.Color.LightSteelBlue);
            LightYellow = new Color(System.Drawing.Color.LightYellow);
            Lime = new Color(System.Drawing.Color.Lime);
            LimeGreen = new Color(System.Drawing.Color.LimeGreen);
            Linen = new Color(System.Drawing.Color.Linen);
            Magenta = new Color(System.Drawing.Color.Magenta);
            Maroon = new Color(System.Drawing.Color.Maroon);
            MediumAquamarine = new Color(System.Drawing.Color.MediumAquamarine);
            MediumBlue = new Color(System.Drawing.Color.MediumBlue);
            MediumOrchid = new Color(System.Drawing.Color.MediumOrchid);
            MediumPurple = new Color(System.Drawing.Color.MediumPurple);
            MediumSeaGreen = new Color(System.Drawing.Color.MediumSeaGreen);
            MediumSlateBlue = new Color(System.Drawing.Color.MediumSlateBlue);
            MediumSpringGreen = new Color(System.Drawing.Color.MediumSpringGreen);
            MediumTurquoise = new Color(System.Drawing.Color.MediumTurquoise);
            MediumVioletRed = new Color(System.Drawing.Color.MediumVioletRed);
            MidnightBlue = new Color(System.Drawing.Color.MidnightBlue);
            MintCream = new Color(System.Drawing.Color.MintCream);
            MistyRose = new Color(System.Drawing.Color.MistyRose);
            Moccasin = new Color(System.Drawing.Color.Moccasin);
            NavajoWhite = new Color(System.Drawing.Color.NavajoWhite);
            Navy = new Color(System.Drawing.Color.Navy);
            OldLace = new Color(System.Drawing.Color.OldLace);
            Olive = new Color(System.Drawing.Color.Olive);
            OliveDrab = new Color(System.Drawing.Color.OliveDrab);
            Orange = new Color(System.Drawing.Color.Orange);
            OrangeRed = new Color(System.Drawing.Color.OrangeRed);
            Orchid = new Color(System.Drawing.Color.Orchid);
            PaleGoldenrod = new Color(System.Drawing.Color.PaleGoldenrod);
            PaleGreen = new Color(System.Drawing.Color.PaleGreen);
            PaleTurquoise = new Color(System.Drawing.Color.PaleTurquoise);
            PaleVioletRed = new Color(System.Drawing.Color.PaleVioletRed);
            PapayaWhip = new Color(System.Drawing.Color.PapayaWhip);
            PeachPuff = new Color(System.Drawing.Color.PeachPuff);
            Peru = new Color(System.Drawing.Color.Peru);
            Pink = new Color(System.Drawing.Color.Pink);
            Plum = new Color(System.Drawing.Color.Plum);
            PowderBlue = new Color(System.Drawing.Color.PowderBlue);
            Purple = new Color(System.Drawing.Color.Purple);
            Red = new Color(1.0f, 0.0f, 0.0f);
            RosyBrown = new Color(System.Drawing.Color.RosyBrown);
            RoyalBlue = new Color(System.Drawing.Color.RoyalBlue);
            SaddleBrown = new Color(System.Drawing.Color.SaddleBrown);
            Salmon = new Color(System.Drawing.Color.Salmon);
            SandyBrown = new Color(System.Drawing.Color.SandyBrown);
            SeaGreen = new Color(System.Drawing.Color.SeaGreen);
            SeaShell = new Color(System.Drawing.Color.SeaShell);
            Sienna = new Color(System.Drawing.Color.Sienna);
            Silver = new Color(System.Drawing.Color.Silver);
            SkyBlue = new Color(System.Drawing.Color.SkyBlue);
            SlateBlue = new Color(System.Drawing.Color.AliceBlue);
            SlateGray = new Color(System.Drawing.Color.SlateGray);
            Snow = new Color(System.Drawing.Color.Snow);
            SpringGreen = new Color(System.Drawing.Color.SpringGreen);
            SteelBlue = new Color(System.Drawing.Color.SteelBlue);
            Tan = new Color(System.Drawing.Color.Tan);
            Teal = new Color(System.Drawing.Color.Teal);
            Thistle = new Color(System.Drawing.Color.Thistle);
            Tomato = new Color(System.Drawing.Color.Tomato);
            Transparent = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Turquoise = new Color(System.Drawing.Color.Turquoise);
            Violet = new Color(System.Drawing.Color.Violet);
            Wheat = new Color(System.Drawing.Color.Wheat);
            White = new Color(1.0f, 1.0f, 1.0f);
            WhiteSmoke = new Color(System.Drawing.Color.WhiteSmoke);
            Yellow = new Color(System.Drawing.Color.Yellow);
            YellowGreen = new Color(System.Drawing.Color.YellowGreen);
        }

        /// <summary>
        /// Gets the <see cref="Color"/> AliceBlue.
        /// </summary>
        public static Color AliceBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> AntiqueWhite.
        /// </summary>
        public static Color AntiqueWhite
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Aqua.
        /// </summary>
        public static Color Aqua
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Aquamarine.
        /// </summary>
        public static Color Aquamarine
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Azure.
        /// </summary>
        public static Color Azure
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Beige.
        /// </summary>
        public static Color Beige
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Bisque.
        /// </summary>
        public static Color Bisque
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Black.
        /// </summary>
        public static Color Black
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> BlanchedAlmond.
        /// </summary>
        public static Color BlanchedAlmond
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Blue.
        /// </summary>
        public static Color Blue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> BlueViolet.
        /// </summary>
        public static Color BlueViolet
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Brown.
        /// </summary>
        public static Color Brown
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> BurlyWood.
        /// </summary>
        public static Color BurlyWood
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> CadetBlue.
        /// </summary>
        public static Color CadetBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Chartreuse.
        /// </summary>
        public static Color Chartreuse
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Chocolate.
        /// </summary>
        public static Color Chocolate
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Coral.
        /// </summary>
        public static Color Coral
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> CornflowerBlue.
        /// </summary>
        public static Color CornflowerBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Cornsilk.
        /// </summary>
        public static Color Cornsilk
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Crimson.
        /// </summary>
        public static Color Crimson
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Cyan.
        /// </summary>
        public static Color Cyan
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkBlue.
        /// </summary>
        public static Color DarkBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkCyan.
        /// </summary>
        public static Color DarkCyan
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkGoldenrod.
        /// </summary>
        public static Color DarkGoldenrod
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkGray.
        /// </summary>
        public static Color DarkGray
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkGreen.
        /// </summary>
        public static Color DarkGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkKhaki.
        /// </summary>
        public static Color DarkKhaki
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkMagenta.
        /// </summary>
        public static Color DarkMagenta
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkOliveGreen.
        /// </summary>
        public static Color DarkOliveGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkOrange.
        /// </summary>
        public static Color DarkOrange
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkOrchid.
        /// </summary>
        public static Color DarkOrchid
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkRed.
        /// </summary>
        public static Color DarkRed
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkSalmon.
        /// </summary>
        public static Color DarkSalmon
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkSeaGreen.
        /// </summary>
        public static Color DarkSeaGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkSlateBlue.
        /// </summary>
        public static Color DarkSlateBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkSlateGray.
        /// </summary>
        public static Color DarkSlateGray
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkTurquoise.
        /// </summary>
        public static Color DarkTurquoise
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DarkViolet.
        /// </summary>
        public static Color DarkViolet
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DeepPink.
        /// </summary>
        public static Color DeepPink
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DeepSkyBlue.
        /// </summary>
        public static Color DeepSkyBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DimGray.
        /// </summary>
        public static Color DimGray
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> DodgerBlue.
        /// </summary>
        public static Color DodgerBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Firebrick.
        /// </summary>
        public static Color Firebrick
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> FloralWhite.
        /// </summary>
        public static Color FloralWhite
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> ForestGreen.
        /// </summary>
        public static Color ForestGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Fuchsia.
        /// </summary>
        public static Color Fuchsia
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Gainsboro.
        /// </summary>
        public static Color Gainsboro
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> GhostWhite.
        /// </summary>
        public static Color GhostWhite
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Gold.
        /// </summary>
        public static Color Gold
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Goldenrod.
        /// </summary>
        public static Color Goldenrod
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Gray.
        /// </summary>
        public static Color Gray
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Green.
        /// </summary>
        public static Color Green
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> GreenYellow.
        /// </summary>
        public static Color GreenYellow
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Honeydew.
        /// </summary>
        public static Color Honeydew
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> HotPink.
        /// </summary>
        public static Color HotPink
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> IndianRed.
        /// </summary>
        public static Color IndianRed
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Indigo.
        /// </summary>
        public static Color Indigo
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Ivory.
        /// </summary>
        public static Color Ivory
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Khaki.
        /// </summary>
        public static Color Khaki
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Lavender.
        /// </summary>
        public static Color Lavender
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LavenderBlush.
        /// </summary>
        public static Color LavenderBlush
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LawnGreen.
        /// </summary>
        public static Color LawnGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LemonChiffon.
        /// </summary>
        public static Color LemonChiffon
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightBlue.
        /// </summary>
        public static Color LightBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightCoral.
        /// </summary>
        public static Color LightCoral
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightCyan.
        /// </summary>
        public static Color LightCyan
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightGoldenrodYellow.
        /// </summary>
        public static Color LightGoldenrodYellow
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightGray.
        /// </summary>
        public static Color LightGray
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightGreen.
        /// </summary>
        public static Color LightGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightPink.
        /// </summary>
        public static Color LightPink
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightSalmon.
        /// </summary>
        public static Color LightSalmon
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightSeaGreen.
        /// </summary>
        public static Color LightSeaGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightSkyBlue.
        /// </summary>
        public static Color LightSkyBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightSlateGray.
        /// </summary>
        public static Color LightSlateGray
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightSteelBlue.
        /// </summary>
        public static Color LightSteelBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LightYellow.
        /// </summary>
        public static Color LightYellow
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Lime.
        /// </summary>
        public static Color Lime
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> LimeGreen.
        /// </summary>
        public static Color LimeGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Linen.
        /// </summary>
        public static Color Linen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Magenta.
        /// </summary>
        public static Color Magenta
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Maroon.
        /// </summary>
        public static Color Maroon
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumAquamarine.
        /// </summary>
        public static Color MediumAquamarine
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumBlue.
        /// </summary>
        public static Color MediumBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumOrchid.
        /// </summary>
        public static Color MediumOrchid
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumPurple.
        /// </summary>
        public static Color MediumPurple
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumSeaGreen.
        /// </summary>
        public static Color MediumSeaGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumSlateBlue.
        /// </summary>
        public static Color MediumSlateBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumSpringGreen.
        /// </summary>
        public static Color MediumSpringGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumTurquoise.
        /// </summary>
        public static Color MediumTurquoise
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MediumVioletRed.
        /// </summary>
        public static Color MediumVioletRed
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MidnightBlue.
        /// </summary>
        public static Color MidnightBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MintCream.
        /// </summary>
        public static Color MintCream
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> MistyRose.
        /// </summary>
        public static Color MistyRose
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Moccasin.
        /// </summary>
        public static Color Moccasin
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> NavajoWhite.
        /// </summary>
        public static Color NavajoWhite
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Navy.
        /// </summary>
        public static Color Navy
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> OldLace.
        /// </summary>
        public static Color OldLace
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Olive.
        /// </summary>
        public static Color Olive
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> OliveDrab.
        /// </summary>
        public static Color OliveDrab
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Orange.
        /// </summary>
        public static Color Orange
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> OrangeRed.
        /// </summary>
        public static Color OrangeRed
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Orchid.
        /// </summary>
        public static Color Orchid
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> PaleGoldenrod.
        /// </summary>
        public static Color PaleGoldenrod
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> PaleGreen.
        /// </summary>
        public static Color PaleGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> PaleTurquoise.
        /// </summary>
        public static Color PaleTurquoise
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> PaleVioletRed.
        /// </summary>
        public static Color PaleVioletRed
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> PapayaWhip.
        /// </summary>
        public static Color PapayaWhip
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> PeachPuff.
        /// </summary>
        public static Color PeachPuff
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Peru.
        /// </summary>
        public static Color Peru
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Pink.
        /// </summary>
        public static Color Pink
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Plum.
        /// </summary>
        public static Color Plum
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> PowderBlue.
        /// </summary>
        public static Color PowderBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Purple.
        /// </summary>
        public static Color Purple
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Red.
        /// </summary>
        public static Color Red
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> RosyBrown.
        /// </summary>
        public static Color RosyBrown
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> RoyalBlue.
        /// </summary>
        public static Color RoyalBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SaddleBrown.
        /// </summary>
        public static Color SaddleBrown
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Salmon.
        /// </summary>
        public static Color Salmon
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SandyBrown.
        /// </summary>
        public static Color SandyBrown
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SeaGreen.
        /// </summary>
        public static Color SeaGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SeaShell.
        /// </summary>
        public static Color SeaShell
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Sienna.
        /// </summary>
        public static Color Sienna
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Silver.
        /// </summary>
        public static Color Silver
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SkyBlue.
        /// </summary>
        public static Color SkyBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SlateBlue.
        /// </summary>
        public static Color SlateBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SlateGray.
        /// </summary>
        public static Color SlateGray
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Snow.
        /// </summary>
        public static Color Snow
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SpringGreen.
        /// </summary>
        public static Color SpringGreen
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> SteelBlue.
        /// </summary>
        public static Color SteelBlue
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Tan.
        /// </summary>
        public static Color Tan
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Teal.
        /// </summary>
        public static Color Teal
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Thistle.
        /// </summary>
        public static Color Thistle
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Tomato.
        /// </summary>
        public static Color Tomato
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Transparent.
        /// </summary>
        public static Color Transparent
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Turquoise.
        /// </summary>
        public static Color Turquoise
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Violet.
        /// </summary>
        public static Color Violet
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Wheat.
        /// </summary>
        public static Color Wheat
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> White.
        /// </summary>
        public static Color White
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> WhiteSmoke.
        /// </summary>
        public static Color WhiteSmoke
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> Yellow.
        /// </summary>
        public static Color Yellow
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Color"/> YellowGreen.
        /// </summary>
        public static Color YellowGreen
        {
            get;
        }

        #endregion
    }
}
