using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Linq;

namespace engenious
{
    internal static class WrappingHelper
    {
        public static bool ValidateStructs<T1,T2>()
        {
            try
            {
                
                if (Marshal.SizeOf(typeof(T1)) != Marshal.SizeOf(typeof(T2)))
                    return false;
                FieldInfo[] origFields = typeof(T1).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(x => Marshal.OffsetOf(typeof(T1), x.Name).ToInt64()).ToArray();
                FieldInfo[] fields = typeof(T2).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(x => Marshal.OffsetOf(typeof(T2), x.Name).ToInt64()).ToArray();
                if (origFields.Length != fields.Length)
                    return false;
                for (int i = 0; i < origFields.Length; i++)
                {
                    FieldInfo origField = origFields[i];
                    FieldInfo field = fields[i];

                    if (Marshal.SizeOf(origField.FieldType) != Marshal.SizeOf(field.FieldType) || Marshal.OffsetOf(typeof(T1), origField.Name) != Marshal.OffsetOf(typeof(T2), field.Name))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

