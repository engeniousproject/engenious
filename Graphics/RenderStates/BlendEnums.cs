namespace engenious.Graphics
{
    /// <summary>
    /// Specifies the source blending factors.
    /// </summary>
    public enum BlendingFactorSrc
    {
        /// <summary>
        /// RGBA source multiplied by 0, resulting in (0,0,0,0).
        /// </summary>
        Zero,
        /// <summary>
        /// RGBA source multiplied by (R_s0,G_s0,B_s0,A_s0) where the <c>s0</c> stands for the first source color.
        /// </summary>
        SrcColor = 768,
        /// <summary>
        /// RGBA source multiplied by (1-R_s0,1-G_s0,1-B_s0,1-A_s0) where the <c>s0</c> stands for the first source color.
        /// </summary>
        OneMinusSrcColor,
        /// <summary>
        /// RGBA source multiplied by (A_s0,A_s0,A_s0,A_s0) where the <c>A_s0</c> stands for the first source alpha.
        /// </summary>
        SrcAlpha,
        /// <summary>
        /// RGBA source multiplied by (1-A_s0,1-A_s0,1-A_s0,1-A_s0) where the <c>A_s0</c> stands for the first source alpha.
        /// </summary>
        OneMinusSrcAlpha,
        /// <summary>
        /// RGBA source multiplied by (A_d,A_d,A_d,A_d) where the <c>A_d</c> stands for destination alpha.
        /// </summary>
        DstAlpha,
        /// <summary>
        /// RGBA source multiplied by (1-A_d,1-A_d,1-A_d,1-A_d) where the <c>A_d</c> stands for destination alpha.
        /// </summary>
        OneMinusDstAlpha,
        /// <summary>
        /// RGBA source multiplied by (R_d,G_d,B_d,A_d) where the <c>d</c> stands for destination color.
        /// </summary>
        DstColor,
        /// <summary>
        /// RGBA source multiplied by (1-R_d,1-G_d,1-B_d,1-A_d) where the <c>d</c> stands for destination color.
        /// </summary>
        OneMinusDstColor,
        /// <summary>
        /// RGBA source multiplied by (i,i,i,i1) where <c>i=min(A_s, 1-A_d)</c>;<c>A_s</c> stands for source alpha;<c>A_d</c> stands for source alpha.
        /// </summary>
        SrcAlphaSaturate,
        /// <summary>
        /// RGBA source multiplied by (R_c,G_c,B_c,A_c) where <c>c</c> stands for constant color.
        /// </summary>
        ConstantColor = 32769,
        /// <summary>
        /// RGBA source multiplied by (R_c,G_c,B_c,A_c) where <c>c</c> stands for constant color.(Extension)
        /// </summary>
        ConstantColorExt = 32769,
        /// <summary>
        /// RGBA source multiplied by (1-R_c,1-G_c,1-B_c,1-A_c) where <c>c</c> stands for constant color.
        /// </summary>
        OneMinusConstantColor,
        /// <summary>
        /// RGBA source multiplied by (1-R_c,1-G_c,1-B_c,1-A_c) where <c>c</c> stands for constant color.(Extension)
        /// </summary>
        OneMinusConstantColorExt = 32770,
        /// <summary>
        /// RGBA source multiplied by (A_c,A_c,A_c,A_c) where the <c>A_c</c> stands for constant alpha.
        /// </summary>
        ConstantAlpha,
        /// <summary>
        /// RGBA source multiplied by (A_c,A_c,A_c,A_c) where the <c>A_c</c> stands for constant alpha.(Extension)
        /// </summary>
        ConstantAlphaExt = 32771,
        /// <summary>
        /// RGBA source multiplied by (1-A_c,1-A_c,1-A_c,1-A_c) where the <c>A_c</c> stands for constant alpha.
        /// </summary>
        OneMinusConstantAlpha,
        /// <summary>
        /// RGBA source multiplied by (1-A_c,1-A_c,1-A_c,1-A_c) where the <c>A_c</c> stands for constant alpha.(Extension)
        /// </summary>
        OneMinusConstantAlphaExt = 32772,
        /// <summary>
        /// RGBA source multiplied by (A_s1,A_s1,A_s1,A_s1) where the <c>A_s1</c> stands for the second source alpha.
        /// </summary>
        Src1Alpha = 34185,
        /// <summary>
        /// RGBA source multiplied by (R_s1,G_s1,B_s1,A_s1) where the <c>s1</c> stands for the second source color.
        /// </summary>
        Src1Color = 35065,
        /// <summary>
        /// RGBA source multiplied by (1-R_s1,1-G_s1,1-B_s1,1-A_s1) where the <c>s1</c> stands for the second source color.
        /// </summary>
        OneMinusSrc1Color,
        /// <summary>
        /// RGBA source multiplied by (1-A_s1,1-A_s1,1-A_s1,1-A_s1) where the <c>A_s1</c> stands for the second source alpha.
        /// </summary>
        OneMinusSrc1Alpha,
        /// <summary>
        /// RGBA source multiplied by (1,1,1,1), resulting in (R_s,G_s,B_s,A_s) where <c>s</c> stands for source color.
        /// </summary>
        One = 1
    }

    /// <summary>
    /// Specifies the destination blending factors.
    /// </summary>
    public enum BlendingFactorDest
    {
        /// <summary>
        /// RGBA destination multiplied by 0, resulting in (0,0,0,0).
        /// </summary>
        Zero,
        /// <summary>
        /// RGBA destination multiplied by (R_s0,G_s0,B_s0,A_s0) where the <c>s0</c> stands for the first source color.
        /// </summary>
        SrcColor = 768,
        /// <summary>
        /// RGBA destination multiplied by (1-R_s0,1-G_s0,1-B_s0,1-A_s0) where the <c>s0</c> stands for the first source color.
        /// </summary>
        OneMinusSrcColor,
        /// <summary>
        /// RGBA destination multiplied by (A_s0,A_s0,A_s0,A_s0) where the <c>A_s0</c> stands for the first source alpha.
        /// </summary>
        SrcAlpha,
        /// <summary>
        /// RGBA destination multiplied by (1-A_s0,1-A_s0,1-A_s0,1-A_s0) where the <c>A_s0</c> stands for the first source alpha.
        /// </summary>
        OneMinusSrcAlpha,
        /// <summary>
        /// RGBA destination multiplied by (A_d,A_d,A_d,A_d) where the <c>A_d</c> stands for destination alpha.
        /// </summary>
        DstAlpha,
        /// <summary>
        /// RGBA destination multiplied by (1-A_d,1-A_d,1-A_d,1-A_d) where the <c>A_d</c> stands for destination alpha.
        /// </summary>
        OneMinusDstAlpha,
        /// <summary>
        /// RGBA destination multiplied by (R_d,G_d,B_d,A_d) where the <c>d</c> stands for destination color.
        /// </summary>
        DstColor,
        /// <summary>
        /// RGBA destination multiplied by (1-R_d,1-G_d,1-B_d,1-A_d) where the <c>d</c> stands for destination color.
        /// </summary>
        OneMinusDstColor,
        /// <summary>
        /// RGBA destination multiplied by (i,i,i,i1) where <c>i=min(A_s, 1-A_d)</c>;<c>A_s</c> stands for source alpha;<c>A_d</c> stands for source alpha.
        /// </summary>
        SrcAlphaSaturate,
        /// <summary>
        /// RGBA destination multiplied by (R_c,G_c,B_c,A_c) where <c>c</c> stands for constant color.
        /// </summary>
        ConstantColor = 32769,
        /// <summary>
        /// RGBA destination multiplied by (R_c,G_c,B_c,A_c) where <c>c</c> stands for constant color.(Extension)
        /// </summary>
        ConstantColorExt = 32769,
        /// <summary>
        /// RGBA destination multiplied by (1-R_c,1-G_c,1-B_c,1-A_c) where <c>c</c> stands for constant color.
        /// </summary>
        OneMinusConstantColor,
        /// <summary>
        /// RGBA destination multiplied by (1-R_c,1-G_c,1-B_c,1-A_c) where <c>c</c> stands for constant color.(Extension)
        /// </summary>
        OneMinusConstantColorExt = 32770,
        /// <summary>
        /// RGBA destination multiplied by (A_c,A_c,A_c,A_c) where the <c>A_c</c> stands for constant alpha.
        /// </summary>
        ConstantAlpha,
        /// <summary>
        /// RGBA destination multiplied by (A_c,A_c,A_c,A_c) where the <c>A_c</c> stands for constant alpha.(Extension)
        /// </summary>
        ConstantAlphaExt = 32771,
        /// <summary>
        /// RGBA destination multiplied by (1-A_c,1-A_c,1-A_c,1-A_c) where the <c>A_c</c> stands for constant alpha.
        /// </summary>
        OneMinusConstantAlpha,
        /// <summary>
        /// RGBA destination multiplied by (1-A_c,1-A_c,1-A_c,1-A_c) where the <c>A_c</c> stands for constant alpha.(Extension)
        /// </summary>
        OneMinusConstantAlphaExt = 32772,
        /// <summary>
        /// RGBA destination multiplied by (A_s1,A_s1,A_s1,A_s1) where the <c>A_s1</c> stands for the second source alpha.
        /// </summary>
        Src1Alpha = 34185,
        /// <summary>
        /// RGBA destination multiplied by (R_s1,G_s1,B_s1,A_s1) where the <c>s1</c> stands for the second source color.
        /// </summary>
        Src1Color = 35065,
        /// <summary>
        /// RGBA destination multiplied by (1-R_s1,1-G_s1,1-B_s1,1-A_s1) where the <c>s1</c> stands for the second source color.
        /// </summary>
        OneMinusSrc1Color,
        /// <summary>
        /// RGBA destination multiplied by (1-A_s1,1-A_s1,1-A_s1,1-A_s1) where the <c>A_s1</c> stands for the second source alpha.
        /// </summary>
        OneMinusSrc1Alpha,
        /// <summary>
        /// RGBA destination multiplied by (1,1,1,1), resulting in (R_s,G_s,B_s,A_s) where <c>s</c> stands for source color.
        /// </summary>
        One = 1
    }

    /// <summary>
    /// Specifies the blending functions.
    /// </summary>
    public enum BlendEquationMode
    {
        /// <summary>
        /// Adds for blending.
        /// </summary>
        FuncAdd = 32774,
        /// <summary>
        /// Uses the min value.
        /// </summary>
        Min,
        /// <summary>
        /// Uses the max value.
        /// </summary>
        Max,
        /// <summary>
        /// Subtracts for blending.
        /// </summary>
        FuncSubtract = 32778,
        /// <summary>
        /// Reverses operands and then subtracts.
        /// </summary>
        FuncReverseSubtract
    }
}

