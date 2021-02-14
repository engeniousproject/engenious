using System;
using System.Threading;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Content type reader to load <see cref="RasterizerState"/> instances.
    /// </summary>
    [ContentTypeReader(typeof(RasterizerState))]
    public class RasterizerStateTypeReader : ContentTypeReader<RasterizerState>
    {
        #region implemented abstract members of ContentTypeReader

        /// <inheritdoc />
        public override RasterizerState? Read(ContentManagerBase managerBase, ContentReader reader, Type? customType = null)
        {
            if (reader.ReadBoolean())
                return null;
            var state = new RasterizerState
            {
                CullMode = (CullMode) reader.ReadUInt16(),
                FillMode = (PolygonMode) reader.ReadUInt16(),
                MultiSampleAntiAlias = reader.ReadBoolean(),
                ScissorTestEnable = reader.ReadBoolean(),

                DepthBias = reader.ReadSingle(),
                SlopeScaleDepthBias = reader.ReadSingle()
            };
            return state;
        }

        #endregion
    }

    /// <summary>
    /// Content type reader to load <see cref="DepthStencilState"/> instances.
    /// </summary>
    [ContentTypeReader(typeof(DepthStencilState))]
    public class DepthStencilStateTypeReader : ContentTypeReader<DepthStencilState>
    {
        #region implemented abstract members of ContentTypeReader

        /// <inheritdoc />
        public override DepthStencilState? Read(ContentManagerBase managerBase, ContentReader reader,
            Type? customType = null)
        {
            if (reader.ReadBoolean())
                return null;
            var state = new DepthStencilState()
            {
                DepthBufferEnable = reader.ReadBoolean(),
                DepthBufferWriteEnable = reader.ReadBoolean(),
                StencilEnable = reader.ReadBoolean(),

                ReferenceStencil = reader.ReadInt32(),
                StencilMask = reader.ReadInt32(),

                DepthBufferFunction = (DepthFunction) reader.ReadUInt16(),
                StencilFunction = (StencilFunction) reader.ReadUInt16(),
                StencilDepthBufferFail = (StencilOp) reader.ReadUInt16(),
                StencilFail = (StencilOp) reader.ReadUInt16(),
                StencilPass = (StencilOp) reader.ReadUInt16(),

                TwoSidedStencilMode = reader.ReadBoolean(),
                CounterClockwiseStencilFunction = (StencilFunction) reader.ReadUInt16(),
                CounterClockwiseStencilDepthBufferFail = (StencilOp) reader.ReadUInt16(),
                CounterClockwiseStencilFail = (StencilOp) reader.ReadUInt16(),
                CounterClockwiseStencilPass = (StencilOp) reader.ReadUInt16()
            };
            return state;
        }

        #endregion
    }

    /// <summary>
    /// Content type reader to load <see cref="BlendState"/> instances.
    /// </summary>
    [ContentTypeReader(typeof(BlendState))]
    public class BlendStateTypeReader : ContentTypeReader<BlendState>
    {
        #region implemented abstract members of ContentTypeReader

        /// <inheritdoc />
        public override BlendState? Read(ContentManagerBase managerBase, ContentReader reader, Type? customType = null)
        {
            if (reader.ReadBoolean())
                return null;
            var state = new BlendState()
            {
                AlphaBlendFunction = (BlendEquationMode) reader.ReadUInt16(),
                AlphaDestinationBlend = (BlendingFactorDest)reader.ReadUInt16(),
                AlphaSourceBlend = (BlendingFactorSrc)reader.ReadUInt16(),

                ColorBlendFunction = (BlendEquationMode)reader.ReadUInt16(),
                ColorDestinationBlend = (BlendingFactorDest)reader.ReadUInt16(),
                ColorSourceBlend = (BlendingFactorSrc)reader.ReadUInt16(),

                //state.BlendFactor = reader.ReadColor(),
                ColorWriteChannels = (ColorWriteChannels) reader.ReadByte(),
                ColorWriteChannels1 = (ColorWriteChannels) reader.ReadByte(),
                ColorWriteChannels2 = (ColorWriteChannels) reader.ReadByte(),
                ColorWriteChannels3 = (ColorWriteChannels) reader.ReadByte(),
            };
            return state;
        }

        #endregion
    }
}

