using System;
using System.ComponentModel;
using engenious.Helper;
using OpenToolkit.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a rasterizer state used for rasterization.
    /// </summary>
    public class RasterizerState : GraphicsResource
    {
        /// <summary>
        /// A rasterizer state which does no culling.
        /// </summary>
        public static readonly RasterizerState CullNone;

        /// <summary>
        /// A rasterizer state which culls clockwise polygons.
        /// </summary>
        public static readonly RasterizerState CullClockwise;

        /// <summary>
        /// A rasterizer state which culls counter-clockwise polygons.
        /// </summary>
        public static readonly RasterizerState CullCounterClockwise;
        private CullMode _cullMode = CullMode.CounterClockwise;
        private PolygonMode _fillMode = PolygonMode.Fill;
        private bool _scissorTestEnable = false;
        private bool _multiSampleAntiAlias = false;
        private float _slopeScaleDepthBias = 0.0f;
        private float _depthBias = 0.0f;

        static RasterizerState()
        {
            CullNone = new RasterizerState();
            CullNone.CullMode = CullMode.None;
            CullNone.FillMode = PolygonMode.Fill;
            CullNone.InitPredefined();

            CullClockwise = new RasterizerState();
            CullClockwise.CullMode = CullMode.Clockwise;
            CullClockwise.FillMode = PolygonMode.Fill;
            CullClockwise.InitPredefined();

            CullCounterClockwise = new RasterizerState();
            CullCounterClockwise.CullMode = CullMode.CounterClockwise;
            CullCounterClockwise.FillMode = PolygonMode.Fill;
            CullCounterClockwise.InitPredefined();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState"/> class.
        /// </summary>
        public RasterizerState()
        {
            CullMode = CullMode.None;
            FillMode = PolygonMode.Fill;
        }
        private bool _isPredefined;
        internal void InitPredefined()
        {
            _isPredefined = true;
        }

        private void ThrowIfReadOnly()
        {
            if (_isPredefined)
                throw new InvalidOperationException("you are not allowed to change a predefined rasterizer state");
        }

        /// <summary>
        /// Gets or sets the cull mode for rendering primitives.
        /// </summary>
        public CullMode CullMode
        {
            get => _cullMode;
            set
            {
                ThrowIfReadOnly();
                if (_cullMode == value)
                    return;
                _cullMode = value;
                if (GraphicsDevice != null)
                    ApplyCullAndFill();
            }
        }

        /// <summary>
        /// Gets or sets the fill mode for rendering primitives.
        /// </summary>
        public PolygonMode FillMode
        {
            get => _fillMode;
            set
            {
                ThrowIfReadOnly();
                if (_fillMode == value)
                    return;
                _fillMode = value; 
                if (GraphicsDevice != null)
                    ApplyCullAndFill();
            }
        }

        private void ApplyCullAndFill()
        {
            GraphicsDevice.ValidateGraphicsThread();

            ApplyCullAndFillGl();
        }

        private void ApplyCullAndFillGl()
        {
            if (CullMode == CullMode.None)
            {
                GL.Disable(EnableCap.CullFace);
                GL.PolygonMode(MaterialFace.FrontAndBack, (OpenToolkit.Graphics.OpenGL.PolygonMode) FillMode);
            }
            else
            {
                GL.Enable(EnableCap.CullFace);
                GL.FrontFace((FrontFaceDirection) CullMode);
                GL.PolygonMode(MaterialFace.FrontAndBack,
                    (OpenToolkit.Graphics.OpenGL.PolygonMode) FillMode);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether multi sample anti aliasing is enabled.
        /// </summary>
        public bool MultiSampleAntiAlias
        {
            get => _multiSampleAntiAlias;
            set
            {
                ThrowIfReadOnly();
                if (_multiSampleAntiAlias == value)
                    return;
                _multiSampleAntiAlias = value;
                if (GraphicsDevice != null)
                    ApplyMultiSampleAntiAlias();
            }
        }

        private void ApplyMultiSampleAntiAlias()
        {
            GraphicsDevice.ValidateGraphicsThread();

            if (MultiSampleAntiAlias)
                GL.Enable(EnableCap.Multisample);
            else
                GL.Disable(EnableCap.Multisample);
        }

        /// <summary>
        /// Gets or sets a value indicating whether scissor testing is enabled.
        /// </summary>
        public bool ScissorTestEnable
        {
            get => _scissorTestEnable;
            set
            {
                ThrowIfReadOnly();
                if (_scissorTestEnable == value)
                    return;
                _scissorTestEnable = value;
                if (GraphicsDevice != null)
                    ApplyScissorTest();
            }
        }

        private void ApplyScissorTest()
        {
            GraphicsDevice.ValidateGraphicsThread();

            if (ScissorTestEnable)
                GL.Enable(EnableCap.ScissorTest);
            else
                GL.Disable(EnableCap.ScissorTest);
        }

        /// <summary>
        /// Gets or sets a bias value that takes into account the slope of a polygon.
        /// </summary>
        public float SlopeScaleDepthBias
        {
            get => _slopeScaleDepthBias;
            set
            {
                ThrowIfReadOnly();
                if (_slopeScaleDepthBias == value)
                    return;
                _slopeScaleDepthBias = value;
                if (GraphicsDevice != null)
                    ApplyPolygonOffset();
            }
        }

        /// <summary>
        /// Gets or sets the depth bias for polygons,
        /// which is the amount of bias to apply to the depth of a primitive to alleviate depth testing problems for primitives of similar depth.
        /// </summary>
        public float DepthBias
        {
            get => _depthBias;
            set
            {
                ThrowIfReadOnly();
                if (_depthBias == value)
                    return;
                _depthBias = value; 
                if (GraphicsDevice != null)
                    ApplyPolygonOffset();
            }
        }

        private void ApplyPolygonOffset()
        {
            GraphicsDevice.ValidateGraphicsThread();

            GL.PolygonOffset(_slopeScaleDepthBias, _depthBias);
        }

        internal void Bind(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            Apply();
        }

        private void Apply()
        {
            GraphicsDevice.ValidateGraphicsThread();

            var oldState = GraphicsDevice.RasterizerState;
            if (oldState == null || oldState.CullMode != CullMode || oldState.FillMode != FillMode)
                ApplyCullAndFillGl();

            if (oldState == null || oldState.MultiSampleAntiAlias != MultiSampleAntiAlias)
            {
                if (MultiSampleAntiAlias)
                    GL.Enable(EnableCap.Multisample);
                else
                    GL.Disable(EnableCap.Multisample);
            }

            if (oldState == null || oldState.ScissorTestEnable != ScissorTestEnable)
            {
                if (ScissorTestEnable)
                    GL.Enable(EnableCap.ScissorTest);
                else
                    GL.Disable(EnableCap.ScissorTest);
            }
            if (oldState == null || oldState.SlopeScaleDepthBias != SlopeScaleDepthBias || oldState.DepthBias != DepthBias)
                GL.PolygonOffset(_slopeScaleDepthBias, _depthBias);
        }

        internal void Unbind()
        {
            GraphicsDevice = null;
        }
    }
}

