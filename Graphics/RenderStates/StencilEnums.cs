namespace engenious.Graphics
{
    /// <summary>
    /// Specifies the depth functions.
    /// </summary>
    public enum DepthFunction
    {
        /// <summary>
        /// Never passes.
        /// </summary>
        Never = 512,
        /// <summary>
        /// Passes if incoming value is lower than stored depth value.
        /// </summary>
        Less,
        /// <summary>
        /// Passes if incoming value is equal to stored depth value.
        /// </summary>
        Equal,
        /// <summary>
        /// Passes if incoming value is less or equal to stored depth value.
        /// </summary>
        Lequal,
        /// <summary>
        /// Passes if incoming value is greater than stored depth value.
        /// </summary>
        Greater,
        /// <summary>
        /// Passes if incoming value is not equal to stored depth value.
        /// </summary>
        Notequal,
        /// <summary>
        /// Passes if incoming value is greater or equal to stored depth value.
        /// </summary>
        Gequal,
        /// <summary>
        /// Always passes.
        /// </summary>
        Always
    }

    /// <summary>
    /// Specifies the stencil functions.
    /// </summary>
    public enum StencilFunction
    {
        /// <summary>
        /// Never passes.
        /// </summary>
        Never = 512,
        /// <summary>
        /// Passes if incoming value is lower than stored stencil value.
        /// </summary>
        Less,
        /// <summary>
        /// Passes if incoming value is equal to stored stencil value.
        /// </summary>
        Equal,
        /// <summary>
        /// Passes if incoming value is less or equal to stored stencil value.
        /// </summary>
        Lequal,
        /// <summary>
        /// Passes if incoming value is greater than stored stencil value.
        /// </summary>
        Greater,
        /// <summary>
        /// Passes if incoming value is not equal to stored stencil value.
        /// </summary>
        Notequal,
        /// <summary>
        /// Passes if incoming value is greater or equal to stored stencil value.
        /// </summary>
        Gequal,
        /// <summary>
        /// Always passes.
        /// </summary>
        Always
    }

    /// <summary>
    /// Specifies the stencil operations.
    /// </summary>
    public enum StencilOp
    {
        /// <summary>
        /// Zeroes the stencil buffer value.
        /// </summary>
        Zero,
        /// <summary>
        /// Inverts the stencil buffer value bitwise.
        /// </summary>
        Invert = 5386,
        /// <summary>
        /// Keeps the current stencil value.
        /// </summary>
        Keep = 7680,
        /// <summary>
        /// The stencil buffer value is replaced with the <see cref="DepthStencilState.ReferenceStencil"/> value of the stencil test function.
        /// </summary>
        Replace,
        /// <summary>
        /// Increments the current stencil value. This value is clamped to the maximum unsigned value.
        /// </summary>
        Incr,
        /// <summary>
        /// Decrements the current stencil value. This value is clamped to zero.
        /// </summary>
        Decr,
        /// <summary>
        /// Increments the current stencil value, wraps back to zero if maximum unsigned value was exceeded.
        /// </summary>
        IncrWrap = 34055,
        /// <summary>
        /// Decrements the current stencil value, wraps back to  maximum unsigned value if zero was preceded.
        /// </summary>
        DecrWrap
    }
}

