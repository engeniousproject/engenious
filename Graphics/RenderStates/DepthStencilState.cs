using System;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a depth stencil state, for depth testing.
    /// </summary>
    public class DepthStencilState : GraphicsResource
    {
        /// <summary>
        /// A default <see cref="DepthStencilState"/> where depth buffer and depth buffer writing is enabled.
        /// </summary>
        public static readonly DepthStencilState Default;

        /// <summary>
        /// A <see cref="DepthStencilState"/> where depth buffer is enabled but depth buffer writing is disabled.
        /// </summary>
        public static readonly DepthStencilState DepthRead;

        /// <summary>
        /// A <see cref="DepthStencilState"/> where depth buffer is disabled.
        /// </summary>
        public static readonly DepthStencilState None;
        private bool _depthBufferEnable = true;
        private bool _depthBufferWriteEnable = true;
        private DepthFunction _depthBufferFunction = DepthFunction.Lequal;
        private bool _stencilEnable = false;
        private StencilFunction _stencilFunction = StencilFunction.Always;
        private bool _twoSidedStencilMode = false;
        private StencilFunction _counterClockwiseStencilFunction = StencilFunction.Always;
        private int _stencilMask = int.MaxValue;
        private StencilOp _stencilDepthBufferFail = StencilOp.Keep;
        private StencilOp _stencilFail = StencilOp.Keep;
        private StencilOp _stencilPass = StencilOp.Keep;
        private int _referenceStencil = 0;
        private StencilOp _counterClockwiseStencilFail = StencilOp.Keep;
        private StencilOp _counterClockwiseStencilDepthBufferFail = StencilOp.Keep;
        private StencilOp _counterClockwiseStencilPass = StencilOp.Keep;

        static DepthStencilState()
        {
            Default = new DepthStencilState();
            Default.DepthBufferEnable = true;
            Default.DepthBufferWriteEnable = true;
            Default.InitPredefined();

            DepthRead = new DepthStencilState();
            DepthRead.DepthBufferEnable = true;
            DepthRead.DepthBufferWriteEnable = false;
            DepthRead.InitPredefined();

            None = new DepthStencilState();
            None.DepthBufferEnable = false;
            None.DepthBufferWriteEnable = false;
            None.InitPredefined();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState"/> class.
        /// </summary>
        public DepthStencilState()
        {
            DepthBufferEnable = true;
            DepthBufferWriteEnable = true;
        }
        private bool _isPredefined;
        internal void InitPredefined()
        {
            _isPredefined = true;
        }

        private void ThrowIfReadOnly()
        {
            if (_isPredefined)
                throw new InvalidOperationException("you are not allowed to change a predefined depth stencil state");
        }

        /// <summary>
        /// Gets or sets a value indicating whether the depth buffer is enabled.
        /// </summary>
        public bool DepthBufferEnable
        {
            get => _depthBufferEnable;
            set
            {
                ThrowIfReadOnly();
                if (_depthBufferEnable == value)
                    return;
                _depthBufferEnable = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyDepthBufferEnable();
            }
        }

        private void ApplyDepthBufferEnable()
        {
            GraphicsDevice!.ValidateUiGraphicsThread();

            if (DepthBufferEnable)
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether writing to the depth buffer is enabled.
        /// </summary>
        public bool DepthBufferWriteEnable
        {
            get => _depthBufferWriteEnable;
            set
            {
                ThrowIfReadOnly();
                if (_depthBufferWriteEnable == value)
                    return;
                _depthBufferWriteEnable = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyDepthBufferWriteEnable();
            }
        }

        private void ApplyDepthBufferWriteEnable()
        {
            GraphicsDevice!.ValidateUiGraphicsThread();

            GL.DepthMask(_depthBufferWriteEnable);
        }

        /// <summary>
        /// Gets or sets the function used for depth testing.
        /// </summary>
        public DepthFunction DepthBufferFunction
        {
            get => _depthBufferFunction;
            set
            {
                ThrowIfReadOnly();
                if (_depthBufferFunction == value)
                    return;
                _depthBufferFunction = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyDepthBufferFunction();
            }
        }

        private void ApplyDepthBufferFunction()
        {
            GraphicsDevice!.ValidateUiGraphicsThread();

            GL.DepthFunc((OpenTK.Graphics.OpenGL.DepthFunction) _depthBufferFunction);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the stencil buffer is enable.
        /// </summary>
        public bool StencilEnable
        {
            get => _stencilEnable;
            set
            {
                ThrowIfReadOnly();
                if (_stencilEnable == value)
                    return;
                _stencilEnable = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyStencilEnable();
            }
        }

        private void ApplyStencilEnable()
        {
            GraphicsDevice!.ValidateUiGraphicsThread();

            if (_stencilEnable)
                GL.Enable(EnableCap.StencilTest);
            else
                GL.Disable(EnableCap.StencilTest);
        }

        /// <summary>
        /// Gets or sets the function used for stencil testing.
        /// </summary>
        public StencilFunction StencilFunction
        {
            get => _stencilFunction;
            set
            {
                ThrowIfReadOnly();
                if (_stencilFunction == value)
                    return;
                _stencilFunction = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether two sided stencil mode is enabled.
        /// </summary>
        public bool TwoSidedStencilMode
        {
            get => _twoSidedStencilMode;
            set
            {
                ThrowIfReadOnly();
                if (_twoSidedStencilMode == value)
                    return;
                _twoSidedStencilMode = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        private void ApplyTwoSidedStencilMode()
        {
            GraphicsDevice!.ValidateUiGraphicsThread();

            ApplyTwoSidedStencilModeGl();
        }

        private void ApplyTwoSidedStencilModeGl()
        {
            if (TwoSidedStencilMode)
            {
                GL.StencilFuncSeparate(StencilFace.Front, (OpenTK.Graphics.OpenGL.StencilFunction) StencilFunction,
                    ReferenceStencil, StencilMask);

                GL.StencilFuncSeparate(StencilFace.Back,
                    (OpenTK.Graphics.OpenGL.StencilFunction) CounterClockwiseStencilFunction, ReferenceStencil,
                    StencilMask);

                GL.StencilOpSeparate(StencilFace.Front, (OpenTK.Graphics.OpenGL.StencilOp) StencilFail,
                    (OpenTK.Graphics.OpenGL.StencilOp) StencilDepthBufferFail,
                    (OpenTK.Graphics.OpenGL.StencilOp) StencilPass);

                GL.StencilOpSeparate(StencilFace.Back,
                    (OpenTK.Graphics.OpenGL.StencilOp) CounterClockwiseStencilFail,
                    (OpenTK.Graphics.OpenGL.StencilOp) CounterClockwiseStencilDepthBufferFail,
                    (OpenTK.Graphics.OpenGL.StencilOp) CounterClockwiseStencilPass);
            }
            else
            {
                GL.StencilFunc((OpenTK.Graphics.OpenGL.StencilFunction) StencilFunction, ReferenceStencil,
                    StencilMask);

                GL.StencilOp((OpenTK.Graphics.OpenGL.StencilOp) StencilFail,
                    (OpenTK.Graphics.OpenGL.StencilOp) StencilDepthBufferFail,
                    (OpenTK.Graphics.OpenGL.StencilOp) StencilPass);
            }
        }

        private bool TwoSidedStencilChanged(DepthStencilState? oldState)
        {
            if (oldState == null || oldState.TwoSidedStencilMode != TwoSidedStencilMode)
                return true;
            return oldState.StencilFunction != StencilFunction || oldState.ReferenceStencil != ReferenceStencil ||
                   oldState.StencilMask != StencilMask ||
                   oldState.StencilFail != StencilFail ||
                   oldState.StencilDepthBufferFail != StencilDepthBufferFail ||
                   oldState.StencilPass != StencilPass || (TwoSidedStencilMode
                                                           && (oldState.CounterClockwiseStencilFunction !=
                                                               CounterClockwiseStencilFunction ||
                                                               oldState.CounterClockwiseStencilDepthBufferFail !=
                                                               CounterClockwiseStencilDepthBufferFail ||
                                                               oldState.CounterClockwiseStencilFail !=
                                                               CounterClockwiseStencilFail ||
                                                               oldState.CounterClockwiseStencilPass !=
                                                               CounterClockwiseStencilPass));
        }

        /// <summary>
        /// Gets or sets the value the stencil test uses for comparison.
        /// </summary>
        public int ReferenceStencil
        {
            get => _referenceStencil;
            set
            {
                ThrowIfReadOnly();
                if (_referenceStencil == value)
                    return;
                _referenceStencil = value;

                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation for the counter clockwise stencil pass.
        /// </summary>
        public StencilOp CounterClockwiseStencilPass
        {
            get => _counterClockwiseStencilPass;
            set
            {
                ThrowIfReadOnly();
                if (_counterClockwiseStencilPass == value)
                    return;
                _counterClockwiseStencilPass = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }


        /// <summary>
        /// Gets or sets the stencil operation for the counter clockwise stencil depth buffer fail.
        /// </summary>
        public StencilOp CounterClockwiseStencilDepthBufferFail
        {
            get => _counterClockwiseStencilDepthBufferFail;
            set
            {
                ThrowIfReadOnly();
                if (_counterClockwiseStencilDepthBufferFail == value)
                    return;
                _counterClockwiseStencilDepthBufferFail = value;

                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation for the counter clockwise stencil fail.
        /// </summary>
        public StencilOp CounterClockwiseStencilFail
        {
            get => _counterClockwiseStencilFail;
            set
            {
                ThrowIfReadOnly();
                if (_counterClockwiseStencilFail == value)
                    return;
                _counterClockwiseStencilFail = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets the counter clockwise stencil function.
        /// </summary>
        public StencilFunction CounterClockwiseStencilFunction
        {
            get => _counterClockwiseStencilFunction;
            set
            {
                ThrowIfReadOnly();
                if (_counterClockwiseStencilFunction == value)
                    return;
                _counterClockwiseStencilFunction = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets the stencil mask.
        /// </summary>
        public int StencilMask
        {
            get => _stencilMask;
            set
            {
                ThrowIfReadOnly();
                if (_stencilMask == value)
                    return;
                _stencilMask = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation for the depth buffer fail.
        /// </summary>
        public StencilOp StencilDepthBufferFail
        {
            get => _stencilDepthBufferFail;
            set
            {
                ThrowIfReadOnly();
                if (_stencilDepthBufferFail == value)
                    return;
                _stencilDepthBufferFail = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation for the stencil buffer fail.
        /// </summary>
        public StencilOp StencilFail
        {
            get => _stencilFail;
            set
            {
                ThrowIfReadOnly();
                if (_stencilFail == value)
                    return;
                _stencilFail = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation for the stencil buffer pass.
        /// </summary>
        public StencilOp StencilPass
        {
            get => _stencilPass;
            set
            {
                ThrowIfReadOnly();
                if (_stencilPass == value)
                    return;
                _stencilPass = value;
                if (GraphicsDevice != null && GraphicsDevice.DepthStencilState == this)
                    ApplyTwoSidedStencilMode();
            }
        }

        internal void Bind(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            Apply();
        }

        private void Apply()
        {
            GraphicsDevice!.ValidateUiGraphicsThread();

            var oldState = GraphicsDevice.DepthStencilState;

            if (oldState == null || oldState.DepthBufferEnable != DepthBufferEnable)
            {
                if (DepthBufferEnable)
                    GL.Enable(EnableCap.DepthTest);
                else
                    GL.Disable(EnableCap.DepthTest);
            }
            if (oldState == null || oldState.DepthBufferFunction != DepthBufferFunction)
                GL.DepthFunc((OpenTK.Graphics.OpenGL.DepthFunction) _depthBufferFunction);
            
            if (oldState == null || oldState.DepthBufferWriteEnable != DepthBufferWriteEnable)
                GL.DepthMask(_depthBufferWriteEnable);

            if (oldState == null || oldState.StencilEnable != StencilEnable)
            {
                if (_stencilEnable)
                    GL.Enable(EnableCap.StencilTest);
                else
                    GL.Enable(EnableCap.StencilTest);
            }

            if (TwoSidedStencilChanged(oldState))
                ApplyTwoSidedStencilModeGl();
        }

        internal void Unbind()
        {
            GraphicsDevice = null;
        }
    }
}
