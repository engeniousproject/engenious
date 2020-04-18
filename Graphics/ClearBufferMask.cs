using System;

namespace engenious.Graphics
{
    /// <summary>
    /// Specifies clear buffer masks.
    /// </summary>
    [Flags()]
    public enum ClearBufferMask
    {
        /// <summary>
        /// No clearing.
        /// </summary>
        None = 0,
        /// <summary>
        /// Clears depth buffer.
        /// </summary>
        DepthBufferBit = 256,
        /// <summary>
        /// Clears accumulation buffer.
        /// </summary>
        AccumBufferBit = 512,
        /// <summary>
        /// Clears stencil buffer.
        /// </summary>
        StencilBufferBit = 1024,
        /// <summary>
        /// Clears color buffer.
        /// </summary>
        ColorBufferBit = 16384,
        /// <summary>
        /// Clears coverage buffers.
        /// </summary>
        CoverageBufferBitNv = 32768
    }
}

