namespace engenious.Graphics
{
    /// <summary>
    /// Specifies primitive types.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// Points.
        /// </summary>
        Points,
        /// <summary>
        /// Lines.
        /// </summary>
        Lines,
        /// <summary>
        /// Line loop.
        /// </summary>
        LineLoop,
        /// <summary>
        /// Line strip.
        /// </summary>
        LineStrip,
        /// <summary>
        /// Triangles.
        /// </summary>
        Triangles,
        /// <summary>
        /// Triangle strip.
        /// </summary>
        TriangleStrip,
        /// <summary>
        /// Triangle fan
        /// </summary>
        TriangleFan,
        /// <summary>
        /// Quads.
        /// </summary>
        Quads,
        /// <summary>
        /// Quads(Extension).
        /// </summary>
        QuadsExt = 7,
        /// <summary>
        /// Lines adjacency.
        /// </summary>
        LinesAdjacency = 10,
        /// <summary>
        /// Lines adjacency(Arb).
        /// </summary>
        LinesAdjacencyArb = 10,
        /// <summary>
        /// Lines adjacency(Extension).
        /// </summary>
        LinesAdjacencyExt = 10,
        /// <summary>
        /// Line strip adjacency.
        /// </summary>
        LineStripAdjacency,
        /// <summary>
        /// Line strip adjacency(Arb).
        /// </summary>
        LineStripAdjacencyArb = 11,
        /// <summary>
        /// Line strip adjacency(Extension).
        /// </summary>
        LineStripAdjacencyExt = 11,
        /// <summary>
        /// Triangles adjacency.
        /// </summary>
        TrianglesAdjacency,
        /// <summary>
        /// Triangles adjacency(Arb).
        /// </summary>
        TrianglesAdjacencyArb = 12,
        /// <summary>
        /// Triangles adjacency(Extension).
        /// </summary>
        TrianglesAdjacencyExt = 12,
        /// <summary>
        /// Triangle strip adjacency.
        /// </summary>
        TriangleStripAdjacency,
        /// <summary>
        /// Triangle strip adjacency(Arb).
        /// </summary>
        TriangleStripAdjacencyArb = 13,
        /// <summary>
        /// Triangle strip adjacency(Extension).
        /// </summary>
        TriangleStripAdjacencyExt = 13,
        /// <summary>
        /// Patches.
        /// </summary>
        Patches,
        /// <summary>
        /// Patches(Extension).
        /// </summary>
        PatchesExt = 14
    }
}

