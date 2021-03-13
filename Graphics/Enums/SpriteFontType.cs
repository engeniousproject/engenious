namespace engenious.Graphics
{
    /// <summary>
    /// Specifies the available sprite font types.
    /// </summary>
    public enum SpriteFontType
    {
        /// <summary>
        /// The default font type.
        /// </summary>
        Default = BitmapFont,
        /// <summary>
        /// A bitmap font.
        /// </summary>
        BitmapFont = 0,
        /// <summary>
        /// A font rendered with pseudo signed distance fields.
        /// </summary>
        PseudoSignedDistanceField = 1,
        /// <summary>
        /// A font rendered with signed distance fields.
        /// </summary>
        SignedDistanceField = 2,
        /// <summary>
        /// A font rendered with multi signed distance fields.
        /// </summary>
        MultiSignedDistanceField = 3,
        /// <summary>
        /// A font rendered with multi and true signed distance fields.
        /// </summary>
        MultiSignedAndTrueDistanceField = 4,
        /// <inheritdoc cref="SpriteFontType.PseudoSignedDistanceField"/>
        PSDF = PseudoSignedDistanceField,
        /// <inheritdoc cref="SpriteFontType.SignedDistanceField"/>
        SDF = SignedDistanceField,
        /// <inheritdoc cref="SpriteFontType.MultiSignedDistanceField"/>
        MSDF = MultiSignedDistanceField,
        /// <inheritdoc cref="SpriteFontType.MultiSignedAndTrueDistanceField"/>
        MTSDF = MultiSignedAndTrueDistanceField,
    }
}