﻿using System;
using engenious.Helper;
using OpenToolkit.Graphics.OpenGL;


namespace engenious.Graphics
{
    /// <summary>
    /// Specifies shader types.
    /// </summary>
    public enum ShaderType
    {
        /// <summary>
        /// Fragment(pixel) shader.
        /// </summary>
        FragmentShader = 35632,
        /// <summary>
        /// Vertex shader.
        /// </summary>
        VertexShader,
        /// <summary>
        /// Geometry shader.
        /// </summary>
        GeometryShader = 36313,
        /// <summary>
        /// Tessellation evaluation shader.
        /// </summary>
        TessEvaluationShader = 36487,
        /// <summary>
        /// Tessellation control shader.
        /// </summary>
        TessControlShader,
        /// <summary>
        /// Compute shader.
        /// </summary>
        ComputeShader = 37305
    }

    internal class Shader : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        public int BaseShader;

        public Shader(GraphicsDevice graphicsDevice, ShaderType type, string source)
        {
            _graphicsDevice = graphicsDevice;
            _graphicsDevice.ValidateGraphicsThread();
            BaseShader = GL.CreateShader((OpenToolkit.Graphics.OpenGL.ShaderType) type);
            if (!source.Contains("#version"))
                source = $"#version {(graphicsDevice.GlslVersion.Major*100+graphicsDevice.GlslVersion.Minor).ToString()}\r\n#line 1\r\n"+source;
            GL.ShaderSource(BaseShader, source);
        }
        

        internal void Compile()
        {
            _graphicsDevice.ValidateGraphicsThread();
            GL.CompileShader(BaseShader);

            int compiled;
            GL.GetShader(BaseShader, ShaderParameter.CompileStatus, out compiled);
            if (compiled != 1)
            {
                var error = GL.GetShaderInfoLog(BaseShader);
                throw new Exception(error);
            }
        }

        public void Dispose()
        {
            _graphicsDevice.ValidateGraphicsThread();
            GL.DeleteProgram(BaseShader);
        }

    }
}

