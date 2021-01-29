using System;

namespace engenious.Graphics
{
    /// <summary>
    /// Specifies the cull modes.
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// No culling.
        /// </summary>
        None,
        /// <summary>
        /// Culls clockwise polygons.
        /// </summary>
        Clockwise = 2304,
        /// <summary>
        /// Culls counter clockwise polygons.
        /// </summary>
        CounterClockwise = 2305
    }

    /// <summary>
    /// Specifies the polygon mode.
    /// </summary>
    public enum PolygonMode
    {
        /// <summary>
        /// Renders only points of the polygons.
        /// </summary>
        Point = 6912,
        /// <summary>
        /// Renders only edges of the polygons.
        /// </summary>
        Line = 6913,
        /// <summary>
        /// Renders the filled polygons.
        /// </summary>
        Fill = 6914
    }

    /// <summary>
    /// Specifies modes to apply <see cref="RasterizerState.DepthBias"/> and
    /// <see cref="RasterizerState.SlopeScaleDepthBias"/> to.
    /// </summary>
    [Flags]
    public enum DepthBiasMode
    {
        /// <summary>
        /// Do not apply any depth bias.
        /// </summary>
        None = 0,
        /// <summary>
        /// Apply depth bias to all polygons rendered in <see cref="PolygonMode.Fill"/>.
        /// </summary>
        BiasFillMode = 1,
        /// <summary>
        /// Apply depth bias to all polygons rendered in <see cref="PolygonMode.Line"/>.
        /// </summary>
        BiasLineMode = 2,
        /// <summary>
        /// Apply depth bias to all polygons rendered in <see cref="PolygonMode.Point"/>.
        /// </summary>
        BiasPointMode = 4
    }
}

