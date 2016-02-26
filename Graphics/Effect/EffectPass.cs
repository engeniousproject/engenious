using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace engenious.Graphics
{
    public sealed class EffectPass :IDisposable
    {
        private int program;

        internal EffectPass(string name)//TODO: content loading
        {
            this.Name = name;
            program = GL.CreateProgram();

            BindAttribute(VertexElementUsage.Color, "color");//TODO: custom naming
            BindAttribute(VertexElementUsage.Position, "position");
            BindAttribute(VertexElementUsage.Normal, "normal");
            BindAttribute(VertexElementUsage.TextureCoordinate, "textureCoordinate");

        }

        internal void BindAttribute(VertexElementUsage usage, string name)
        {
            GL.BindAttribLocation(program, (int)usage, name);
        }

        internal void CacheParameters()
        {
            int total = -1;
            GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out total); 
            for (int i = 0; i < total; ++i)
            {
                int size;
                ActiveUniformType type;
                string name = GL.GetActiveUniform(program, i, out size, out type);
                int location = GetUniformLocation(name);

                Parameters.Add(new EffectPassParameter(this, name, location));
            }
        }

        internal int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(program, name);
        }

        internal List<Shader> attached = new List<Shader>();

        internal void AttachShaders(IEnumerable<Shader> shaders)
        {
            foreach (Shader shader in shaders)
            {
                AttachShader(shader);
            }
        }

        internal void AttachShader(Shader shader)
        {
            GL.AttachShader(program, shader.shader);
        }

        internal void Link()
        {
            if (attached == null)
                throw new Exception("Already linked");
            GL.LinkProgram(program);
            int linked;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out linked);
            if (linked != 1)
            {
                string error = GL.GetProgramInfoLog(program);
                throw new Exception(error);
            }
            foreach (Shader shader in attached)
            {
                GL.DetachShader(program, shader.shader);
            }
            attached.Clear();
            attached = null;
            Parameters = new EffectPassParameterCollection(this);
        }

        internal EffectPassParameterCollection Parameters{ get; private set; }

        public string Name
        {
            get;
            private set;
        }

        public void Apply()
        {
            GL.UseProgram(program);
        }

        public void Dispose()
        {
            GL.DeleteProgram(program);
        }

        public BlendState BlendState{ get; internal set; }
        //TODO: apply states

        public DepthStencilState DepthStencilState{ get; internal set; }

        public RasterizerState RasterizerState{ get; internal set; }
    }
}

