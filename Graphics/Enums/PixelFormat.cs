using System.Diagnostics.CodeAnalysis;

namespace engenious
{
    /// <summary>
    /// Specifies pixel formats.
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// Unsigned short.
        /// </summary>
        UnsignedShort = 5123,
        /// <summary>
        /// Unsigned int.
        /// </summary>
        UnsignedInt = 5125,
        /// <summary>
        /// Color index.
        /// </summary>
        ColorIndex = 6400,
        /// <summary>
        /// Stencil index.
        /// </summary>
        StencilIndex,
        /// <summary>
        /// Depth component.
        /// </summary>
        DepthComponent,
        /// <summary>
        /// Red.
        /// </summary>
        Red,
        /// <summary>
        /// RedExt.
        /// </summary>
        RedExt = 6403,
        /// <summary>
        /// Green.
        /// </summary>
        Green,
        /// <summary>
        /// Blue.
        /// </summary>
        Blue,
        /// <summary>
        /// Alpha.
        /// </summary>
        Alpha,
        /// <summary>
        /// Rgb.
        /// </summary>
        Rgb,
        /// <summary>
        /// Rgba.
        /// </summary>
        Rgba,
        /// <summary>
        /// Luminance.
        /// </summary>
        Luminance,
        /// <summary>
        /// Luminance with alpha.
        /// </summary>
        LuminanceAlpha,
        /// <summary>
        /// AbgrExt.
        /// </summary>
        AbgrExt = 32768,
        /// <summary>
        /// CmykExt.
        /// </summary>
        CmykExt = 32780,
        /// <summary>
        /// CmykaExt.
        /// </summary>
        CmykaExt,
        /// <summary>
        /// Bgr.
        /// </summary>
        Bgr = 32992,
        /// <summary>
        /// Bgra.
        /// </summary>
        Bgra,
        /// <summary>
        /// Ycrcb422Sgix.
        /// </summary>
        Ycrcb422Sgix = 33211,
        /// <summary>
        /// Ycrcb444Sgix.
        /// </summary>
        Ycrcb444Sgix,
        /// <summary>
        /// Rg.
        /// </summary>
        Rg = 33319,
        /// <summary>
        /// RgInteger.
        /// </summary>
        RgInteger,
        /// <summary>
        /// R5G6B5IccSgix.
        /// </summary>
        R5G6B5IccSgix = 33894,
        /// <summary>
        /// R5G6B5A8IccSgix.
        /// </summary>
        R5G6B5A8IccSgix,
        /// <summary>
        /// Alpha16IccSgix.
        /// </summary>
        Alpha16IccSgix,
        /// <summary>
        /// Luminance16IccSgix.
        /// </summary>
        Luminance16IccSgix,
        /// <summary>
        /// Luminance16Alpha8IccSgix.
        /// </summary>
        Luminance16Alpha8IccSgix = 33899,
        /// <summary>
        /// Depth stencil.
        /// </summary>
        DepthStencil = 34041,
        /// <summary>
        /// Red integer.
        /// </summary>
        RedInteger = 36244,
        /// <summary>
        /// Green integer.
        /// </summary>
        GreenInteger,
        /// <summary>
        /// Blue integer.
        /// </summary>
        BlueInteger,
        /// <summary>
        /// Alpha integer.
        /// </summary>
        AlphaInteger,
        /// <summary>
        /// Rgb integer.
        /// </summary>
        RgbInteger,
        /// <summary>
        /// Rgba integer.
        /// </summary>
        RgbaInteger,
        /// <summary>
        /// Bgr integer.
        /// </summary>
        BgrInteger,
        /// <summary>
        /// Bgra integer.
        /// </summary>
        BgraInteger
    }

    /// <summary>
    /// Specifies GPU sided pixel formats.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum PixelInternalFormat
    {
        /// <summary>
        /// Depth component.
        /// </summary>
        DepthComponent = 6402,
        /// <summary>
        /// Alpha.
        /// </summary>
        Alpha = 6406,
        /// <summary>
        /// Rgb.
        /// </summary>
        Rgb,
        /// <summary>
        /// Rgba.
        /// </summary>
        Rgba,
        /// <summary>
        /// Luminance.
        /// </summary>
        Luminance,
        /// <summary>
        /// Luminance Alpha.
        /// </summary>
        LuminanceAlpha,
        /// <summary>
        /// R3G3B2.
        /// </summary>
        R3G3B2 = 10768,
        /// <summary>
        /// Rgb2Ext.
        /// </summary>
        Rgb2Ext = 32846,
        /// <summary>
        /// Rgb4.
        /// </summary>
        Rgb4,
        /// <summary>
        /// Rgb5.
        /// </summary>
        Rgb5,
        /// <summary>
        /// Rgb8.
        /// </summary>
        Rgb8,
        /// <summary>
        /// Rgb10.
        /// </summary>
        Rgb10,
        /// <summary>
        /// Rgb12.
        /// </summary>
        Rgb12,
        /// <summary>
        /// Rgb16.
        /// </summary>
        Rgb16,
        /// <summary>
        /// Rgba2.
        /// </summary>
        Rgba2,
        /// <summary>
        /// Rgba4.
        /// </summary>
        Rgba4,
        /// <summary>
        /// Rgb5A1.
        /// </summary>
        Rgb5A1,
        /// <summary>
        /// Rgba8.
        /// </summary>
        Rgba8,
        /// <summary>
        /// Rgb10A2.
        /// </summary>
        Rgb10A2,
        /// <summary>
        /// Rgba12.
        /// </summary>
        Rgba12,
        /// <summary>
        /// Rgba16.
        /// </summary>
        Rgba16,
        /// <summary>
        /// DualAlpha4Sgis.
        /// </summary>
        DualAlpha4Sgis = 33040,
        /// <summary>
        /// DualAlpha8Sgis.
        /// </summary>
        DualAlpha8Sgis,
        /// <summary>
        /// DualAlpha12Sgis.
        /// </summary>
        DualAlpha12Sgis,
        /// <summary>
        /// DualAlpha16Sgis.
        /// </summary>
        DualAlpha16Sgis,
        /// <summary>
        /// DualLuminance4Sgis.
        /// </summary>
        DualLuminance4Sgis,
        /// <summary>
        /// DualLuminance8Sgis.
        /// </summary>
        DualLuminance8Sgis,
        /// <summary>
        /// DualLuminance12Sgis.
        /// </summary>
        DualLuminance12Sgis,
        /// <summary>
        /// DualLuminance16Sgis.
        /// </summary>
        DualLuminance16Sgis,
        /// <summary>
        /// DualIntensity4Sgis.
        /// </summary>
        DualIntensity4Sgis,
        /// <summary>
        /// DualIntensity8Sgis.
        /// </summary>
        DualIntensity8Sgis,
        /// <summary>
        /// DualIntensity12Sgis.
        /// </summary>
        DualIntensity12Sgis,
        /// <summary>
        /// DualIntensity16Sgis.
        /// </summary>
        DualIntensity16Sgis,
        /// <summary>
        /// DualLuminanceAlpha4Sgis.
        /// </summary>
        DualLuminanceAlpha4Sgis,
        /// <summary>
        /// DualLuminanceAlpha8Sgis.
        /// </summary>
        DualLuminanceAlpha8Sgis,
        /// <summary>
        /// QuadAlpha4Sgis.
        /// </summary>
        QuadAlpha4Sgis,
        /// <summary>
        /// QuadAlpha8Sgis.
        /// </summary>
        QuadAlpha8Sgis,
        /// <summary>
        /// QuadLuminance4Sgis.
        /// </summary>
        QuadLuminance4Sgis,
        /// <summary>
        /// QuadLuminance8Sgis.
        /// </summary>
        QuadLuminance8Sgis,
        /// <summary>
        /// QuadIntensity4Sgis.
        /// </summary>
        QuadIntensity4Sgis,
        /// <summary>
        /// QuadIntensity8Sgis.
        /// </summary>
        QuadIntensity8Sgis,
        /// <summary>
        /// DepthComponent16.
        /// </summary>
        DepthComponent16 = 33189,
        /// <summary>
        /// DepthComponent16Sgix.
        /// </summary>
        DepthComponent16Sgix = 33189,
        /// <summary>
        /// DepthComponent24.
        /// </summary>
        DepthComponent24,
        /// <summary>
        /// DepthComponent24Sgix.
        /// </summary>
        DepthComponent24Sgix = 33190,
        /// <summary>
        /// DepthComponent32.
        /// </summary>
        DepthComponent32,
        /// <summary>
        /// DepthComponent32Sgix.
        /// </summary>
        DepthComponent32Sgix = 33191,
        /// <summary>
        /// CompressedRed.
        /// </summary>
        CompressedRed = 33317,
        /// <summary>
        /// CompressedRg.
        /// </summary>
        CompressedRg,
        /// <summary>
        /// R8.
        /// </summary>
        R8 = 33321,
        /// <summary>
        /// R16.
        /// </summary>
        R16,
        /// <summary>
        /// Rg8.
        /// </summary>
        Rg8,
        /// <summary>
        /// Rg16.
        /// </summary>
        Rg16,
        /// <summary>
        /// R16f.
        /// </summary>
        R16f,
        /// <summary>
        /// R32f.
        /// </summary>
        R32f,
        /// <summary>
        /// Rg16f.
        /// </summary>
        Rg16f,
        /// <summary>
        /// Rg32f.
        /// </summary>
        Rg32f,
        /// <summary>
        /// R8i.
        /// </summary>
        R8i,
        /// <summary>
        /// R8ui.
        /// </summary>
        R8ui,
        /// <summary>
        /// R16i.
        /// </summary>
        R16i,
        /// <summary>
        /// R16ui.
        /// </summary>
        R16ui,
        /// <summary>
        /// R32i.
        /// </summary>
        R32i,
        /// <summary>
        /// R32ui.
        /// </summary>
        R32ui,
        /// <summary>
        /// Rg8i.
        /// </summary>
        Rg8i,
        /// <summary>
        /// Rg8ui.
        /// </summary>
        Rg8ui,
        /// <summary>
        /// Rg16i.
        /// </summary>
        Rg16i,
        /// <summary>
        /// Rg16ui.
        /// </summary>
        Rg16ui,
        /// <summary>
        /// Rg32i.
        /// </summary>
        Rg32i,
        /// <summary>
        /// Rg32ui.
        /// </summary>
        Rg32ui,
        /// <summary>
        /// CompressedRgbS3tcDxt1Ext.
        /// </summary>
        CompressedRgbS3tcDxt1Ext = 33776,
        /// <summary>
        /// CompressedRgbaS3tcDxt1Ext.
        /// </summary>
        CompressedRgbaS3tcDxt1Ext,
        /// <summary>
        /// CompressedRgbaS3tcDxt3Ext.
        /// </summary>
        CompressedRgbaS3tcDxt3Ext,
        /// <summary>
        /// CompressedRgbaS3tcDxt5Ext.
        /// </summary>
        CompressedRgbaS3tcDxt5Ext,
        /// <summary>
        /// RgbIccSgix.
        /// </summary>
        RgbIccSgix = 33888,
        /// <summary>
        /// RgbaIccSgix.
        /// </summary>
        RgbaIccSgix,
        /// <summary>
        /// AlphaIccSgix.
        /// </summary>
        AlphaIccSgix,
        /// <summary>
        /// LuminanceIccSgix.
        /// </summary>
        LuminanceIccSgix,
        /// <summary>
        /// IntensityIccSgix.
        /// </summary>
        IntensityIccSgix,
        /// <summary>
        /// LuminanceAlphaIccSgix.
        /// </summary>
        LuminanceAlphaIccSgix,
        /// <summary>
        /// R5G6B5IccSgix.
        /// </summary>
        R5G6B5IccSgix,
        /// <summary>
        /// R5G6B5A8IccSgix.
        /// </summary>
        R5G6B5A8IccSgix,
        /// <summary>
        /// Alpha16IccSgix.
        /// </summary>
        Alpha16IccSgix,
        /// <summary>
        /// Luminance16IccSgix.
        /// </summary>
        Luminance16IccSgix,
        /// <summary>
        /// Intensity16IccSgix.
        /// </summary>
        Intensity16IccSgix,
        /// <summary>
        /// Luminance16Alpha8IccSgix.
        /// </summary>
        Luminance16Alpha8IccSgix,
        /// <summary>
        /// CompressedAlpha.
        /// </summary>
        CompressedAlpha = 34025,
        /// <summary>
        /// CompressedLuminance.
        /// </summary>
        CompressedLuminance,
        /// <summary>
        /// CompressedLuminanceAlpha.
        /// </summary>
        CompressedLuminanceAlpha,
        /// <summary>
        /// CompressedIntensity.
        /// </summary>
        CompressedIntensity,
        /// <summary>
        /// CompressedRgb.
        /// </summary>
        CompressedRgb,
        /// <summary>
        /// CompressedRgba.
        /// </summary>
        CompressedRgba,
        /// <summary>
        /// DepthStencil.
        /// </summary>
        DepthStencil = 34041,
        /// <summary>
        /// Rgba32f.
        /// </summary>
        Rgba32f = 34836,
        /// <summary>
        /// Rgb32f.
        /// </summary>
        Rgb32f,
        /// <summary>
        /// Rgba16f.
        /// </summary>
        Rgba16f = 34842,
        /// <summary>
        /// Rgb16f.
        /// </summary>
        Rgb16f,
        /// <summary>
        /// Depth24Stencil8.
        /// </summary>
        Depth24Stencil8 = 35056,
        /// <summary>
        /// R11fG11fB10f.
        /// </summary>
        R11fG11fB10f = 35898,
        /// <summary>
        /// Rgb9E5.
        /// </summary>
        Rgb9E5 = 35901,
        /// <summary>
        /// Srgb.
        /// </summary>
        Srgb = 35904,
        /// <summary>
        /// Srgb8.
        /// </summary>
        Srgb8,
        /// <summary>
        /// SrgbAlpha.
        /// </summary>
        SrgbAlpha,
        /// <summary>
        /// Srgb8Alpha8.
        /// </summary>
        Srgb8Alpha8,
        /// <summary>
        /// SluminanceAlpha.
        /// </summary>
        SluminanceAlpha,
        /// <summary>
        /// Sluminance8Alpha8.
        /// </summary>
        Sluminance8Alpha8,
        /// <summary>
        /// Sluminance.
        /// </summary>
        Sluminance,
        /// <summary>
        /// Sluminance8.
        /// </summary>
        Sluminance8,
        /// <summary>
        /// CompressedSrgb.
        /// </summary>
        CompressedSrgb,
        /// <summary>
        /// CompressedSrgbAlpha.
        /// </summary>
        CompressedSrgbAlpha,
        /// <summary>
        /// CompressedSluminance.
        /// </summary>
        CompressedSluminance,
        /// <summary>
        /// CompressedSluminanceAlpha.
        /// </summary>
        CompressedSluminanceAlpha,
        /// <summary>
        /// CompressedSrgbS3tcDxt1Ext.
        /// </summary>
        CompressedSrgbS3tcDxt1Ext,
        /// <summary>
        /// CompressedSrgbAlphaS3tcDxt1Ext.
        /// </summary>
        CompressedSrgbAlphaS3tcDxt1Ext,
        /// <summary>
        /// CompressedSrgbAlphaS3tcDxt3Ext.
        /// </summary>
        CompressedSrgbAlphaS3tcDxt3Ext,
        /// <summary>
        /// CompressedSrgbAlphaS3tcDxt5Ext.
        /// </summary>
        CompressedSrgbAlphaS3tcDxt5Ext,
        /// <summary>
        /// DepthComponent32f.
        /// </summary>
        DepthComponent32f = 36012,
        /// <summary>
        /// Depth32fStencil8.
        /// </summary>
        Depth32fStencil8,
        /// <summary>
        /// Rgba32ui.
        /// </summary>
        Rgba32ui = 36208,
        /// <summary>
        /// Rgb32ui.
        /// </summary>
        Rgb32ui,
        /// <summary>
        /// Rgba16ui.
        /// </summary>
        Rgba16ui = 36214,
        /// <summary>
        /// Rgb16ui.
        /// </summary>
        Rgb16ui,
        /// <summary>
        /// Rgba8ui.
        /// </summary>
        Rgba8ui = 36220,
        /// <summary>
        /// Rgb8ui.
        /// </summary>
        Rgb8ui,
        /// <summary>
        /// Rgba32i.
        /// </summary>
        Rgba32i = 36226,
        /// <summary>
        /// Rgb32i.
        /// </summary>
        Rgb32i,
        /// <summary>
        /// Rgba16i.
        /// </summary>
        Rgba16i = 36232,
        /// <summary>
        /// Rgb16i.
        /// </summary>
        Rgb16i,
        /// <summary>
        /// Rgba8i.
        /// </summary>
        Rgba8i = 36238,
        /// <summary>
        /// Rgb8i.
        /// </summary>
        Rgb8i,
        /// <summary>
        /// Float32UnsignedInt248Rev.
        /// </summary>
        Float32UnsignedInt248Rev = 36269,
        /// <summary>
        /// CompressedRedRgtc1.
        /// </summary>
        CompressedRedRgtc1 = 36283,
        /// <summary>
        /// CompressedSignedRedRgtc1.
        /// </summary>
        CompressedSignedRedRgtc1,
        /// <summary>
        /// CompressedRgRgtc2.
        /// </summary>
        CompressedRgRgtc2,
        /// <summary>
        /// CompressedSignedRgRgtc2.
        /// </summary>
        CompressedSignedRgRgtc2,
        /// <summary>
        /// CompressedRgbaBptcUnorm.
        /// </summary>
        CompressedRgbaBptcUnorm = 36492,
        /// <summary>
        /// CompressedRgbBptcSignedFloat.
        /// </summary>
        CompressedRgbBptcSignedFloat = 36494,
        /// <summary>
        /// CompressedRgbBptcUnsignedFloat.
        /// </summary>
        CompressedRgbBptcUnsignedFloat,
        /// <summary>
        /// R8Snorm.
        /// </summary>
        R8Snorm = 36756,
        /// <summary>
        /// Rg8Snorm.
        /// </summary>
        Rg8Snorm,
        /// <summary>
        /// Rgb8Snorm.
        /// </summary>
        Rgb8Snorm,
        /// <summary>
        /// Rgba8Snorm.
        /// </summary>
        Rgba8Snorm,
        /// <summary>
        /// R16Snorm.
        /// </summary>
        R16Snorm,
        /// <summary>
        /// Rg16Snorm.
        /// </summary>
        Rg16Snorm,
        /// <summary>
        /// Rgb16Snorm.
        /// </summary>
        Rgb16Snorm,
        /// <summary>
        /// Rgba16Snorm.
        /// </summary>
        Rgba16Snorm,
        /// <summary>
        /// Rgb10A2ui.
        /// </summary>
        Rgb10A2ui = 36975,
        /// <summary>
        /// One.
        /// </summary>
        One = 1,
        /// <summary>
        /// Two.
        /// </summary>
        Two,
        /// <summary>
        /// Three.
        /// </summary>
        Three,
        /// <summary>
        /// Four.
        /// </summary>
        Four,
    }
}

