using System;
using System.ComponentModel;
using System.Globalization;

namespace engenious
{
    /// <summary>
    /// Provides a type converter to convert <see cref="Vector2"/> to and from various other representations.
    /// </summary>
    public class Vector2Converter : ExpandableObjectConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null)
                return base.ConvertFrom(context, culture, value);

            var val = s.Trim();
            if (val.StartsWith("[") && val.EndsWith("]"))
                val = val.Substring(1,val.Length-2);
            var splt = val.Split(new[]{','},2,StringSplitOptions.RemoveEmptyEntries);
            float x,y;
            if (float.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) && float.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y))
            {
                return new Vector2(x,y);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    /// <summary>
    /// Provides a type converter to convert <see cref="Vector2d"/> to and from various other representations.
    /// </summary>
    public class Vector2dConverter : ExpandableObjectConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null)
                return base.ConvertFrom(context, culture, value);

            var val = s.Trim();
            if (val.StartsWith("[") && val.EndsWith("]"))
                val = val.Substring(1,val.Length-2);
            var splt = val.Split(new[]{','},2,StringSplitOptions.RemoveEmptyEntries);
            double x,y;
            if (double.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) 
                && double.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y))
            {
                return new Vector2d(x,y);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    /// <summary>
    /// Provides a type converter to convert <see cref="Vector3"/> to and from various other representations.
    /// </summary>
    public class Vector3Converter : ExpandableObjectConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null)
                return base.ConvertFrom(context, culture, value);
            var val = s.Trim();
            if (val.StartsWith("[") && val.EndsWith("]"))
                val = val.Substring(1,val.Length-2);
            var splt = val.Split(new[]{','},3,StringSplitOptions.RemoveEmptyEntries);
            float x,y,z;
            if (float.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) && float.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y) && float.TryParse(splt[2].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out z))
            {
                return new Vector3(x,y,z);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    /// <summary>
    /// Provides a type converter to convert <see cref="Vector3d"/> to and from various other representations.
    /// </summary>
    public class Vector3dConverter : ExpandableObjectConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null)
                return base.ConvertFrom(context, culture, value);
            var val = s.Trim();
            if (val.StartsWith("[") && val.EndsWith("]"))
                val = val.Substring(1,val.Length-2);
            var splt = val.Split(new[]{','},3,StringSplitOptions.RemoveEmptyEntries);
            double x,y,z;
            if (double.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) 
                && double.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y) 
                && double.TryParse(splt[2].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out z))
            {
                return new Vector3d(x,y,z);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    /// <summary>
    /// Provides a type converter to convert <see cref="Vector4"/> to and from various other representations.
    /// </summary>
    public class Vector4Converter : ExpandableObjectConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null)
                return base.ConvertFrom(context, culture, value);
            var val = s.Trim();
            if (val.StartsWith("[") && val.EndsWith("]"))
                val = val.Substring(1,val.Length-2);
            var splt = val.Split(new[]{','},4,StringSplitOptions.RemoveEmptyEntries);
            float x,y,z,w;
            if (float.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) && float.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y) && float.TryParse(splt[2].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out z) && float.TryParse(splt[3].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out w))
            {
                return new Vector4(x,y,z,w);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    /// <summary>
    /// Provides a type converter to convert <see cref="Vector4d"/> to and from various other representations.
    /// </summary>
    public class Vector4dConverter : ExpandableObjectConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null)
                return base.ConvertFrom(context, culture, value);
            var val = s.Trim();
            if (val.StartsWith("[") && val.EndsWith("]"))
                val = val.Substring(1,val.Length-2);
            var splt = val.Split(new[]{','},4,StringSplitOptions.RemoveEmptyEntries);
            double x,y,z,w;
            if (double.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) && double.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y) && double.TryParse(splt[2].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out z) && double.TryParse(splt[3].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out w))
            {
                return new Vector4d(x,y,z,w);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    /// <summary>
    /// Provides a type converter to convert <see cref="Matrix"/> to and from various other representations.
    /// </summary>
    public class MatrixConverter : ExpandableObjectConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
    }
}

