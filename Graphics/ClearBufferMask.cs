using System;

namespace engenious
{
    [Flags()]
    public enum ClearBufferMask
    {
        None = 0,
        DepthBufferBit = 256,
        AccumBufferBit = 512,
        StencilBufferBit = 1024,
        ColorBufferBit = 16384,
        CoverageBufferBitNv = 32768
    }
}

