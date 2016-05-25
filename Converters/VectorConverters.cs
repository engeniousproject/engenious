using System;
using System.ComponentModel;
using System.Globalization;

namespace engenious
{
    public class Vector2Converter : ExpandableObjectConverter
    {
        public Vector2Converter()
        {
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string val = ((string)value).Trim();
                if (val.StartsWith("[") && val.EndsWith("]"))
                    val = val.Substring(1,val.Length-2);
                string[] splt = val.Split(new char[]{','},2,StringSplitOptions.RemoveEmptyEntries);
                float x,y;
                if (float.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) && float.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y))
                {
                    return new Vector2(x,y);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    public class Vector3Converter : ExpandableObjectConverter
    {
        public Vector3Converter()
        {
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string val = ((string)value).Trim();
                if (val.StartsWith("[") && val.EndsWith("]"))
                    val = val.Substring(1,val.Length-2);
                string[] splt = val.Split(new char[]{','},3,StringSplitOptions.RemoveEmptyEntries);
                float x,y,z;
                if (float.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) && float.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y) && float.TryParse(splt[2].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out z))
                {
                    return new Vector3(x,y,z);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    public class Vector4Converter : ExpandableObjectConverter
    {
        public Vector4Converter()
        {
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string val = ((string)value).Trim();
                if (val.StartsWith("[") && val.EndsWith("]"))
                    val = val.Substring(1,val.Length-2);
                string[] splt = val.Split(new char[]{','},4,StringSplitOptions.RemoveEmptyEntries);
                float x,y,z,w;
                if (float.TryParse(splt[0].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out x) && float.TryParse(splt[1].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out y) && float.TryParse(splt[2].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out z) && float.TryParse(splt[3].Trim(),NumberStyles.Float,CultureInfo.InvariantCulture.NumberFormat,out w))
                {
                    return new Vector4(x,y,z,w);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    public class MatrixConverter : ExpandableObjectConverter
    {
        public MatrixConverter()
        {
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
    }
}

