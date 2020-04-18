namespace engenious.Graphics
{
    /// <summary>
    /// Specifies buffer usages.
    /// </summary>
    public enum BufferUsageHint
    {
        /// <summary>
        /// Stream, draw.
        /// </summary>
        StreamDraw = 35040,
        /// <summary>
        /// Stream, read.
        /// </summary>
        StreamRead,
        /// <summary>
        /// Stream, copy.
        /// </summary>
        StreamCopy,
        /// <summary>
        /// Static, draw.
        /// </summary>
        StaticDraw = 35044,
        /// <summary>
        /// Static, read.
        /// </summary>
        StaticRead,
        /// <summary>
        /// Static, copy.
        /// </summary>
        StaticCopy,
        /// <summary>
        /// Dynamic, draw.
        /// </summary>
        DynamicDraw = 35048,
        /// <summary>
        /// Dynamic, copy.
        /// </summary>
        DynamicRead,
        /// <summary>
        /// Dynamic, copy.
        /// </summary>
        DynamicCopy
    }
}

