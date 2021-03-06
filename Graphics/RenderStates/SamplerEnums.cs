﻿namespace engenious
{
    /// <summary>
    /// Specifies the texture wrapping modes.
    /// </summary>
    public enum TextureWrapMode
    {
        /// <summary>
        /// Repeats texture.
        /// </summary>
        Repeat = 10497,
        /// <summary>
        /// Clamps the texture to the border.
        /// </summary>
        ClampToBorder = 33069,
        /// <summary>
        /// Clamps the texture to the border(Arb).
        /// </summary>
        ClampToBorderArb = 33069,
        /// <summary>
        /// Clamps the texture to the border(Nv).
        /// </summary>
        ClampToBorderNv = 33069,
        /// <summary>
        /// Clamps the texture to the border(Sgis).
        /// </summary>
        ClampToBorderSgis = 33069,
        /// <summary>
        /// Clamps the texture to the edge.
        /// </summary>
        ClampToEdge = 33071,
        /// <summary>
        /// Clamps the texture to the edge(Sgis).
        /// </summary>
        ClampToEdgeSgis = 33071,
        /// <summary>
        /// Repeats the texture mirrored, to previous direction.
        /// </summary>
        MirroredRepeat = 33648
    }

    /// <summary>
    /// Specifies the magnification filters.
    /// </summary>
    public enum MagFilter
    {
        /// <summary>
        /// Nearest neighbour filtering.
        /// </summary>
        Nearest = 9728, // 0x00002600
        /// <summary>
        /// Linear filtering.
        /// </summary>
        Linear = 9729, // 0x00002601
        /// <summary>
        /// Linear filtering using higher level image for higher detail.(Sgis)
        /// </summary>
        LinearDetailSgis = 32919, // 0x00008097
        /// <summary>
        /// Linear filtering, for alpha channel using higher level image for higher detail.(Sgis)
        /// </summary>
        LinearDetailAlphaSgis = 32920, // 0x00008098
        /// <summary>
        /// Linear filtering, for color channel using higher level image for higher detail.(Sgis)
        /// </summary>
        LinearDetailColorSgis = 32921, // 0x00008099
        /// <summary>
        /// Linear filtering, sharpening by extrapolating from higher level image.(Sgis)
        /// </summary>
        LinearSharpenSgis = 32941, // 0x000080AD
        /// <summary>
        /// Linear filtering, sharpening only the alpha channel by extrapolating from higher level image.(Sgis)
        /// </summary>
        LinearSharpenAlphaSgis = 32942, // 0x000080AE
        /// <summary>
        /// Linear filtering, sharpening only the color channel by extrapolating from higher level image.(Sgis)
        /// </summary>
        LinearSharpenColorSgis = 32943, // 0x000080AF
        /// <summary>
        /// Filtering using specific interpolation coefficients.(Sgis)
        /// </summary>
        /// <remarks>
        /// E.g. according to Mitchell-Netravali.
        /// <a href="https://www.khronos.org/registry/OpenGL/extensions/SGI/GLU_SGI_filter4_parameters.txt" />
        /// </remarks>
        Filter4Sgis = 33094, // 0x00008146
    }

    /// <summary>
    /// Specifies the minification filters.
    /// </summary>
    public enum MinFilter
    {
        /// <summary>
        /// Nearest neighbour filtering.
        /// </summary>
        Nearest = 9728, // 0x00002600
        /// <summary>
        /// Linear filtering.
        /// </summary>
        Linear = 9729, // 0x00002601
        /// <summary>
        /// Uses the mip-map closest matching in size and then uses nearest neighbour filtering on it.
        /// </summary>
        NearestMipmapNearest = 9984, // 0x00002700
        /// <summary>
        /// Uses the mip-map closest matching in size and then uses linear filtering on it.
        /// </summary>
        LinearMipmapNearest = 9985, // 0x00002701
        /// <summary>
        /// Uses the two mip-maps closest matching in size, uses nearest neighbour filtering on them and does a weighted average.
        /// </summary>
        NearestMipmapLinear = 9986, // 0x00002702
        /// <summary>
        /// Uses the two mip-maps closest matching in size, uses linear filtering on them and does a weighted average.
        /// </summary>
        LinearMipmapLinear = 9987, // 0x00002703
        /// <summary>
        /// Filtering using specific interpolation coefficients.(Sgis)
        /// </summary>
        /// <remarks>
        /// E.g. according to Mitchell-Netravali.
        /// <a href="https://www.khronos.org/registry/OpenGL/extensions/SGI/GLU_SGI_filter4_parameters.txt" />
        /// </remarks>
        Filter4Sgis = 33094, // 0x00008146
        /// <summary>
        /// Uses the two clip-maps closest matching in size, uses linear filtering on them and does a weighted average.(Sgix)
        /// </summary>
        LinearClipmapLinearSgix = 33136, // 0x00008170
        /// <summary>
        /// Uses the clip-map closest matching in size and then uses nearest neighbour filtering on it.(Sgix)
        /// </summary>
        NearestClipmapNearestSgix = 33869, // 0x0000844D
        /// <summary>
        /// Uses the two clip-maps closest matching in size, uses nearest neighbour filtering on them and does a weighted average.(Sgix)
        /// </summary>
        NearestClipmapLinearSgix = 33870, // 0x0000844E
        /// <summary>
        /// Uses the clip-map closest matching in size and then uses linear filtering on it.(Sgix)
        /// </summary>
        LinearClipmapNearestSgix = 33871, // 0x0000844F
    }

    /// <summary>
    /// Specifies the comparison mode to use for a depth texture.
    /// </summary>
    public enum TextureCompareMode
    {
        /// <summary>
        /// The r-channel is the value read from the depth texture.
        /// </summary>
        None = 0,
        /// <summary>
        /// The interpolated and clamped r-texture coordinate should be compared according to the given <see cref="TextureCompareFunc"/>.
        /// </summary>
        CompareRefToTexture = 34894, // 0x0000884E
    }

    /// <summary>
    /// Specifies the comparison function used when <see cref="TextureCompareMode.CompareRefToTexture"/> is set.
    /// </summary>
    public enum TextureCompareFunc
    {
        /// <summary>
        /// The result is always <c>0.0</c>.
        /// </summary>
        Never = 512, // 0x00000200
        /// <summary>
        /// The result is <c>1.0</c> when the depth value is less than the given value; otherwise <c>0.0</c>.
        /// </summary>
        Less = 513, // 0x00000201
        /// <summary>
        /// The result is <c>1.0</c> when the depth value is equal to the given value; otherwise <c>0.0</c>.
        /// </summary>
        Equal = 514, // 0x00000202
        /// <summary>
        /// The result is <c>1.0</c> when the depth value is less than or equal to the given value; otherwise <c>0.0</c>.
        /// </summary>
        LessOrEequal = 515, // 0x00000203
        /// <summary>
        /// The result is <c>1.0</c> when the depth value is greater than the given value; otherwise <c>0.0</c>.
        /// </summary>
        Greater = 516, // 0x00000204
        /// <summary>
        /// The result is <c>1.0</c> when the depth value is not equal to the given value; otherwise <c>0.0</c>.
        /// </summary>
        NotEqual = 517, // 0x00000205
        /// <summary>
        /// The result is <c>1.0</c> when the depth value is greater than or equal to the given value; otherwise <c>0.0</c>.
        /// </summary>
        GreaterOrEqual = 518, // 0x00000206
        /// <summary>
        /// The result is always <c>1.0</c>.
        /// </summary>
        Always = 519, // 0x00000207
    }
}

