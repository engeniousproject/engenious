﻿using System;
using System.Collections.Generic;
using System.Text;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// Render pass of an <see cref="Effect"/>.
    /// </summary>
    public class EffectPass : GraphicsResource, IDisposable
    {
        /// <summary>
        /// Class that restores the active <see cref="GraphicsDevice.EffectPass"/> on dispose.
        /// </summary>
        public readonly struct PassRestorer : IDisposable
        {
            private readonly GraphicsDevice _graphicsDevice;
            private readonly EffectPass? _oldValue;

            /// <summary>
            /// Restores the active <see cref="GraphicsDevice.EffectPass"/> to the given value on dispose.
            /// </summary>
            /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to restore on.</param>
            /// <param name="oldValue">The <see cref="EffectPass"/> to restore to.</param>
            public PassRestorer(GraphicsDevice graphicsDevice, EffectPass? oldValue)
            {
                _graphicsDevice = graphicsDevice;
                _oldValue = oldValue;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                _graphicsDevice.EffectPass = _oldValue;
            }
        }
        internal readonly int Program;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPass"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> the resource is allocated on.</param>
        /// <param name="name">The name of the pass.</param>
        protected internal EffectPass(GraphicsDevice graphicsDevice, string name)//TODO: content loading
            : base(graphicsDevice)
        {
            Name = name;
            GraphicsDevice.ValidateUiGraphicsThread();
            Program = GL.CreateProgram();
            Parameters = new EffectPassParameterCollection(this);
        }

        internal void BindAttribute(VertexElementUsage usage, string name)
        {
            BindAttribute(usage, 0, name);
        }
        internal void BindAttribute(VertexElementUsage usage, int usageIndex, string name)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            GL.BindAttribLocation(Program, (int) usage + usageIndex, name);
        }

        /// <summary>
        /// Caches the <see cref="EffectPass"/> parameter locations.
        /// </summary>
        protected internal virtual void CacheParameters()
        {
            var total = -1;
            GraphicsDevice.ValidateUiGraphicsThread();
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out total);
            Span<int> lengths = stackalloc int[total];
            Span<int> uniformIndices = stackalloc int[total];
            for (int i = 0; i < total; ++i)
            {
                uniformIndices[i] = i;
            }

            unsafe
            {
                fixed(int* uniformIndicesPtr = uniformIndices)
                fixed (int* lengthsPtr = lengths)
                    GL.GetActiveUniforms(Program, total, uniformIndicesPtr, ActiveUniformParameter.UniformNameLength,  lengthsPtr);
            }
            for (var i = 0; i < total; ++i)
            {
                GL.GetActiveUniform(Program, i, lengths[i],out _, out _,  out var type, out var name);
                var location = GetUniformLocation(name);
                Parameters.Add(new EffectPassParameter(this, name, location, (EffectParameterType)type));
            }
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniformBlocks, out total);
            for (var i = 0; i < total; ++i)
            {
                int size;
                var sb = new StringBuilder(512);
                string name;
                GL.GetActiveUniformBlockName(Program, i, 512, out size, out name);
                //var name = sb.ToString();
                var location = GL.GetUniformBlockIndex(Program, name);
                Parameters.Add(new EffectPassParameter(this, name, location));//TODO: 
            }
            //TODO: ssbos?

        }

        internal int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(Program, name);
        }

        internal List<Shader> Attached = new List<Shader>();

        internal void AttachShaders(IEnumerable<Shader> shaders)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var shader in shaders)
            {
                AttachShader(shader);
            }
        }

        internal void AttachShader(Shader shader)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            GL.AttachShader(Program, shader.BaseShader);
        }

        internal void Link()
        {
            if (Attached == null)
                throw new Exception("Already linked");
            GraphicsDevice.ValidateUiGraphicsThread();
            
            GL.LinkProgram(Program);
            int linked;
            GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out linked);
            if (linked != 1)
            {
                var error = GL.GetProgramInfoLog(Program);
                if (string.IsNullOrEmpty(error))
                    throw new Exception("Unknown error occured");
                throw new Exception(error);
            }
            foreach (var shader in Attached)
            {
                GL.DetachShader(Program, shader.BaseShader);
            }
            
            Attached.Clear();
            GraphicsDevice.ValidateUiGraphicsThread();
        }

        /// <summary>
        /// Gets the parameters associated with this <see cref="EffectPass"/>.
        /// </summary>
        protected internal EffectPassParameterCollection Parameters{ get; private set; }

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;
        
        /// <summary>
        /// Gets the passes name.
        /// </summary>
        public new string Name
        {
            get => base.Name!;
            private init => base.Name = value;
        }

        /// <summary>
        /// Applies this render pass.
        /// </summary>
        /// <returns>
        /// A <see cref="PassRestorer"/> which can be used to restore to the previous <see cref="EffectPass"/>.
        /// </returns>
        public PassRestorer Apply()
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            var passRestorer = new PassRestorer(GraphicsDevice, GraphicsDevice.EffectPass);
            GraphicsDevice.EffectPass = this;

            return passRestorer;
        }

        /// <summary>
        /// Execute this pass with specified number of groups.
        /// </summary>
        /// <remarks>Only works on compute shaders.</remarks>
        /// <param name="x">The x count of groups.</param>
        /// <param name="y">The y count of groups.</param>
        /// <param name="z">The z count of groups.</param>
        public void Compute(int x,int y=1,int z=1)
        {
            Apply();
            GL.DispatchCompute(x, y, z);
        }

        /// <summary>
        /// Wait for compute shader execution completion.
        /// </summary>
        /// <remarks>Only works on compute shaders.</remarks>
        public void WaitForImageCompletion()
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            GL.DeleteProgram(Program);
        }

        /// <summary>
        /// Gets the <see cref="BlendState"/> associated with this pass.
        /// </summary>
        public BlendState? BlendState{ get; internal set; }
        //TODO: apply states

        /// <summary>
        /// Gets the <see cref="DepthStencilState"/> associated with this pass.
        /// </summary>
        public DepthStencilState? DepthStencilState{ get; internal set; }

        /// <summary>
        /// Gets the <see cref="RasterizerState"/> associated with this pass.
        /// </summary>
        public RasterizerState? RasterizerState{ get; internal set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}

