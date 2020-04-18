using System;

namespace engenious.Graphics
{
    /// <summary>
    /// Specifies channel colors to write to.
    /// </summary>
    [Flags]
    public enum ColorWriteChannels
    {
        /// <summary>
        /// Write to no channel.
        /// </summary>
        None,
        /// <summary>
        /// Write to the alpha channel.
        /// </summary>
        Alpha=1,
        /// <summary>
        /// Write to the blue color channel.
        /// </summary>
        Blue=2,
        /// <summary>
        /// Write to the green color channel.
        /// </summary>
        Green=4,
        /// <summary>
        /// Write to the red color channel.
        /// </summary>
        Red=8,
        /// <summary>
        /// Write to all color channels.
        /// </summary>
        All=Alpha|Blue|Green|Red
    }
}