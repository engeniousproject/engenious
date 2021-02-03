using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes the sampler state used for texture sampling.
    /// </summary>
    public class SamplerState : GraphicsResource
    {
        
        private const TextureParameterName TextureParameterNameTextureMaxAnisotropy = (TextureParameterName) ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt;

        private const TextureParameterName TextureParameterNameTextureMaxLevel = (TextureParameterName) All.TextureMaxLevel;
        /// <summary>
        /// A sampler state with linear filtering and clamp to edge.
        /// </summary>
        public static readonly SamplerState LinearClamp;
        /// <summary>
        /// A sampler state with linear filtering and texture wrap.
        /// </summary>
        public static readonly SamplerState LinearWrap;

        static SamplerState()
        {
            LinearClamp = new SamplerState();
            LinearClamp.InitPredefined();
            LinearWrap = new SamplerState();
            LinearWrap.AddressU = LinearWrap.AddressV = LinearWrap.AddressW = TextureWrapMode.Repeat;
            LinearWrap.InitPredefined();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState"/> class.
        /// </summary>
        public SamplerState()
        {
            //TODO: implement completly	
            AddressU = AddressV = AddressW = TextureWrapMode.ClampToEdge;
        }

        private bool _isPredefined;
        internal void InitPredefined()
        {
            _isPredefined = true;
        }

        private void ThrowIfReadOnly()
        {
            if (_isPredefined)
                throw new InvalidOperationException("you are not allowed to change a predefined sampler state");
        }

        /// <summary>
        /// Gets or sets the texture wrap mode in horizontal direction.
        /// </summary>
        public TextureWrapMode AddressU
        {
            get => _addressU;
            set
            {
                ThrowIfReadOnly();
                if (_addressU == value)
                    return;
                _addressU = value;
                if (_boundTexture != null)
                    ApplyAddressU();
            }
        }

        private void ApplyAddressU()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyAddressUGl();
            EndApply();
        }
        private void ApplyAddressUGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureWrapS,(int)AddressU);
        }

        /// <summary>
        /// Gets or sets the texture wrap mode in vertical direction.
        /// </summary>
        public TextureWrapMode AddressV
        {
            get => _addressV;
            set
            {
                ThrowIfReadOnly();
                if (_addressV == value)
                    return;
                _addressV = value; 
                if (_boundTexture != null)
                    ApplyAddressVGl();
            }
        }

        private void ApplyAddressV()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyAddressVGl();
            EndApply();
        }
        private void ApplyAddressVGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureWrapT,(int)AddressV);
        }

        /// <summary>
        /// Gets or sets the texture wrap mode in depth direction.
        /// </summary>
        public TextureWrapMode AddressW
        {
            get => _addressW;
            set
            {
                ThrowIfReadOnly();
                if (_addressW == value)
                    return;
                _addressW = value;
                if (_boundTexture != null)
                    ApplyAddressW();
            }
        }

        private void ApplyAddressW()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyAddressWGl();
            EndApply();
        }
        private void ApplyAddressWGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureWrapR,(int)AddressW);
        }

        /// <summary>
        /// Gets or sets the minification filter fot this sampler.
        /// </summary>
        public MinFilter MinFilter
        {
            get => _minFilter;
            set
            {
                ThrowIfReadOnly();
                if (_minFilter == value)
                    return;
                _minFilter = value;
                if (_boundTexture != null)
                    ApplyMinFilter();
            }
        }

        private void ApplyMinFilter()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyMinFilterGl();
            EndApply();
        }
        private void ApplyMinFilterGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureMinFilter,(int)MinFilter);
        }

        /// <summary>
        /// Gets or sets the magnification filter fot this sampler.
        /// </summary>
        public MagFilter MagFilter
        {
            get => _magFilter;
            set
            {
                ThrowIfReadOnly();
                if (_magFilter == value)
                    return;
                _magFilter = value;
                if (_boundTexture != null)
                    ApplyMagFilter();
            }
        }

        private void ApplyMagFilter()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyMagFilterGl();
            EndApply();
        }

        private void ApplyMagFilterGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureMagFilter,(int)MagFilter);
        }

        /// <summary>
        /// Gets or sets the compare mode used for depth texture sampling.
        /// </summary>
        public TextureCompareMode TextureCompareMode
        {
            get => _textureCompareMode;
            set
            {
                ThrowIfReadOnly();
                if (_textureCompareMode == value)
                    return;
                _textureCompareMode = value;
                if (_boundTexture != null)
                    ApplyTextureCompareMode();
            }
        }

        private void ApplyTextureCompareMode()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyTextureCompareModeGl();
            EndApply();
        }

        private void ApplyTextureCompareModeGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureCompareMode,(int)TextureCompareMode);
        }

        /// <summary>
        /// Gets or sets the compare mode used for depth texture sampling.
        /// </summary>
        public TextureCompareFunc TextureCompareFunction
        {
            get => _textureCompareFunction;
            set
            {
                ThrowIfReadOnly();
                if (_textureCompareFunction == value)
                    return;
                _textureCompareFunction = value;
                if (_boundTexture != null)
                    ApplyTextureCompareFunc();
            }
        }

        private void ApplyTextureCompareFunc()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyTextureCompareFuncGl();
            EndApply();
        }

        private void ApplyTextureCompareFuncGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureCompareFunc,(int)TextureCompareFunction);
        }

        /// <summary>
        /// Gets or sets the maximum anisotropy.
        /// </summary>
        public int MaxAnisotropy
        {
            get => _maxAnisotropy;
            set
            {
                ThrowIfReadOnly();
                if (_maxAnisotropy == value)
                    return;
                _maxAnisotropy = value;
                if (_boundTexture != null)
                    ApplyMaxAnisotropy();
            }
        }
        private void ApplyMaxAnisotropy()
        {
            if (!GraphicsDevice.Capabilities.SupportsTextureFilterAnisotropic)
                return;
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            ApplyMaxAnisotropyGl();
            EndApply();
        }

        private void ApplyMaxAnisotropyGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterNameTextureMaxAnisotropy, MathHelper.Clamp(MaxAnisotropy, 1.0f, GraphicsDevice.Capabilities.MaxTextureAnisotropy));
        }

        /// <summary>
        /// Gets or sets the maximum mip-map level.
        /// </summary>
        public int MaxMipLevel
        {
            get => _maxMipLevel;
            set
            {
                ThrowIfReadOnly();
                if (_maxMipLevel == value)
                    return;
                _maxMipLevel = value;
                if (_boundTexture != null)
                    ApplyMaxMipLevel();
            }
        }

        private void ApplyMaxMipLevel()
        {
            if (!GraphicsDevice.Capabilities.SupportsTextureMaxLevel)
                return;
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();

            ApplyMaxMipLevelGl();

            EndApply();
        }

        private void ApplyMaxMipLevelGl()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterNameTextureMaxLevel,MaxMipLevel > 0 ? MaxMipLevel : 1000);
        }

        /// <summary>
        /// Gets or sets the level of detail bias for mip-mapping.
        /// </summary>
        public float MipMapLevelOfDetailBias
        {
            get => _mipMapLevelOfDetailBias;
            set
            {
                ThrowIfReadOnly();
                if (_mipMapLevelOfDetailBias == value)
                    return;
                _mipMapLevelOfDetailBias = value;
                if (_boundTexture != null)
                    ApplyMipMapLevelOfDetailBias();
            }
        }

        
        
        private void ApplyMipMapLevelOfDetailBias()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();

            ApplyMipMapLevelOfDetailBiasGl();

            EndApply();
        }
        private void ApplyMipMapLevelOfDetailBiasGl()
        {
            GL.TexParameter(_boundTexture.Target, TextureParameterName.TextureLodBias, MipMapLevelOfDetailBias);
        }

        private int _maxAnisotropy = 4;
        private int _maxMipLevel = 0;
        private float _mipMapLevelOfDetailBias = 0.0f;

        private GetIndexedPName GetPNameFromTarget(TextureTarget target)
        {
            return PNames[GetIndexFromTarget(target)];
        }

        private int GetIndexFromTarget(TextureTarget target)
        {
            switch (target)
            {
                case TextureTarget.Texture1D:
                    return 0;
                case TextureTarget.Texture2D:
                    return 1;
                case TextureTarget.Texture3D:
                    return 2;
                case TextureTarget.Texture1DArray:
                    return 3;
                case TextureTarget.Texture2DArray:
                    return 4;
                case TextureTarget.TextureRectangle:
                    return 5;
                case TextureTarget.TextureCubeMap:
                    return 6;
                case TextureTarget.TextureCubeMapArray:
                    return 7;
            }

            throw new NotSupportedException();
        }

        private static readonly TextureTarget[] Targets = new[]
        {
            TextureTarget.Texture1D, TextureTarget.Texture2D, TextureTarget.Texture3D, TextureTarget.Texture1DArray,
            TextureTarget.Texture2DArray, TextureTarget.TextureRectangle, TextureTarget.TextureCubeMap,
            TextureTarget.TextureCubeMapArray
        };

        private static readonly GetIndexedPName[] PNames = new[]
        {
            GetIndexedPName.TextureBinding1D, GetIndexedPName.TextureBinding2D, GetIndexedPName.TextureBinding3D,
            GetIndexedPName.TextureBinding1DArray, GetIndexedPName.TextureBinding2DArray,
            GetIndexedPName.TextureBindingRectangle, GetIndexedPName.TextureBindingCubeMap,
            GetIndexedPName.TextureBindingCubeMapArray
        };



        private Texture _boundTexture = null;
        private GetIndexedPName _pName;
        internal void Bind(Texture texture)
        {
            texture.SamplerState?.Unbind();
            GraphicsDevice = texture.GraphicsDevice;
            _boundTexture = texture;
            _pName = GetPNameFromTarget(texture.Target);
            
            Apply();
        }

        private bool _bound;
        private int _previousText;
        private MinFilter _minFilter = MinFilter.Linear;
        private MagFilter _magFilter = MagFilter.Linear;
        private TextureWrapMode _addressU = TextureWrapMode.ClampToEdge;
        private TextureWrapMode _addressV = TextureWrapMode.ClampToEdge;
        private TextureWrapMode _addressW = TextureWrapMode.ClampToEdge;
        private TextureCompareMode _textureCompareMode = TextureCompareMode.None;
        private TextureCompareFunc _textureCompareFunction = TextureCompareFunc.Never;

        private void BeginApply()
        {
            if (_bound)
                return;
            var index = GetIndexFromTarget(_boundTexture.Target);
            GL.GetInteger(PNames[index], 0, out _previousText);
            _bound = true;
        }

        private void EndApply()
        {
            _bound = false;
            if (_previousText != 0)
            {
                GL.BindTexture(_boundTexture.Target, _previousText);
            }
        }
        private void Apply()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            BeginApply();
            var oldState = _boundTexture.SamplerState;

            if (oldState == null || oldState.MipMapLevelOfDetailBias != MipMapLevelOfDetailBias)
                ApplyMipMapLevelOfDetailBiasGl();

            if (oldState == null || oldState.AddressU != AddressU)
                ApplyAddressUGl();
            if (oldState == null || oldState.AddressV != AddressV)
                ApplyAddressVGl();
            if (oldState == null || oldState.AddressW != AddressW)
                ApplyAddressWGl();

            if (oldState == null || oldState.MagFilter != MagFilter)
                ApplyMagFilterGl();
            if (oldState == null || oldState.MinFilter != MinFilter)
                ApplyMagFilterGl();


            if (oldState == null || oldState.TextureCompareMode != TextureCompareMode)
                ApplyTextureCompareModeGl();
            if (oldState == null || oldState.TextureCompareFunction != TextureCompareFunction)
                ApplyTextureCompareFuncGl();

            if (GraphicsDevice.Capabilities.SupportsTextureMaxLevel && (oldState == null || oldState.MaxMipLevel != MaxMipLevel))
                ApplyMaxMipLevelGl();

            if (GraphicsDevice.Capabilities.SupportsTextureFilterAnisotropic && (oldState == null || oldState.MaxAnisotropy != MaxAnisotropy))
                ApplyMaxAnisotropyGl();
            
            
            EndApply();
        }

        

        internal void Unbind()
        {
            _boundTexture = null;
        }
    }
}