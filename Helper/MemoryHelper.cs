using System;
using System.Reflection.Emit;

namespace engenious.Helper
{
    /// <summary>
    /// A helper class containing otherwise unavailable IL functionality.
    /// </summary>
    public class MemoryHelper
    {
        static MemoryHelper()
        {
            CopyBulk = CompileBulkCopy();
        }
        private static CopyBulkDelegate CompileBulkCopy() {
            var dynamicMethod = new DynamicMethod(
                "copybulk",
                null,
                new[] { typeof(IntPtr),typeof( IntPtr),typeof(uint) });

            var gen = dynamicMethod.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Cpblk);
            gen.Emit(OpCodes.Ret);
            return (CopyBulkDelegate)dynamicMethod.CreateDelegate(typeof(CopyBulkDelegate));
        }
        /// <summary>
        /// Delegate for the IL Cpblk instruction wrapper.
        /// </summary>
        /// <param name="src">The source to copy from.</param>
        /// <param name="dst">The destination to copy to.</param>
        /// <param name="size">The byte count to copy.</param>
        public delegate void CopyBulkDelegate(IntPtr src, IntPtr dst, uint size);

        /// <summary>
        /// Gets a wrapper for the Cpblk instruction.
        /// </summary>
        public static CopyBulkDelegate CopyBulk;
    }
}

