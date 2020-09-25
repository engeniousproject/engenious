using System;
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
            GraphicsDevice.ValidateGraphicsThread();
            Program = GL.CreateProgram();
        }

        internal void BindAttribute(VertexElementUsage usage, string name)
        {
            GraphicsDevice.ValidateGraphicsThread();
            GL.BindAttribLocation(Program, (int) usage, name);
        }

        /// <summary>
        /// Caches the <see cref="EffectPass"/> parameter locations.
        /// </summary>
        protected internal virtual void CacheParameters()
        {
            var total = -1;
            GraphicsDevice.ValidateGraphicsThread();
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out total);
            for (var i = 0; i < total; ++i)
            {
                var name = GL.GetActiveUniform(Program, i, out _, out var type);
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
            GraphicsDevice.ValidateGraphicsThread();
            foreach (var shader in shaders)
            {
                AttachShader(shader);
            }
        }

        internal void AttachShader(Shader shader)
        {
            GraphicsDevice.ValidateGraphicsThread();
            GL.AttachShader(Program, shader.BaseShader);
        }

        internal void Link()
        {
            if (Attached == null)
                throw new Exception("Already linked");
            GraphicsDevice.ValidateGraphicsThread();
            
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
            Attached = null;
            GraphicsDevice.ValidateGraphicsThread();
            Parameters = new EffectPassParameterCollection(this);
        }

        /// <summary>
        /// Gets the parameters associated with this <see cref="EffectPass"/>.
        /// </summary>
        protected internal EffectPassParameterCollection Parameters{ get; private set; }

        /// <summary>
        /// Gets the passes name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Applies this render pass.
        /// </summary>
        public void Apply()
        {
            GraphicsDevice.ValidateGraphicsThread();
            GL.UseProgram(Program);
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
            GraphicsDevice.ValidateGraphicsThread();
            GL.UseProgram(Program);
            GL.DispatchCompute(x, y, z);
        }

        /// <summary>
        /// Wait for compute shader execution completion.
        /// </summary>
        /// <remarks>Only works on compute shaders.</remarks>
        public void WaitForImageCompletion()
        {
            GraphicsDevice.ValidateGraphicsThread();
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GraphicsDevice.ValidateGraphicsThread();
            GL.DeleteProgram(Program);
        }

        /// <summary>
        /// Gets the <see cref="BlendState"/> associated with this pass.
        /// </summary>
        public BlendState BlendState{ get; internal set; }
        //TODO: apply states

        /// <summary>
        /// Gets the <see cref="DepthStencilState"/> associated with this pass.
        /// </summary>
        public DepthStencilState DepthStencilState{ get; internal set; }

        /// <summary>
        /// Gets the <see cref="RasterizerState"/> associated with this pass.
        /// </summary>
        public RasterizerState RasterizerState{ get; internal set; }
    }
}

