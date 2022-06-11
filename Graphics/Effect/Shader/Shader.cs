using System;
using System.Buffers;
using System.Runtime.InteropServices;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;


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

        private Shader(GraphicsDevice graphicsDevice, ShaderType type)
        {
            _graphicsDevice = graphicsDevice;
            _graphicsDevice.ValidateUiGraphicsThread();
            BaseShader = GL.CreateShader((OpenTK.Graphics.OpenGL.ShaderType) type);
        }
        public Shader(GraphicsDevice graphicsDevice, ShaderType type, string source)
            : this(graphicsDevice, type)
        {
            int versionPos = source.IndexOf("#version", StringComparison.Ordinal);
            if (versionPos == -1)
            {
                source = graphicsDevice.GlslVersion == null
                    ? "#version 130\r\n#line 1\r\n" + source
                    : $"#version {(graphicsDevice.GlslVersion.Major*100+graphicsDevice.GlslVersion.Minor).ToString()}\r\n#line 1\r\n"+source;
            }
            else
            {
                var newLinePos = source.IndexOf('\n', versionPos);
                if (newLinePos == -1)
                    newLinePos = source.Length;
                //source = source.Insert(newLinePos,);
            }
            GL.ShaderSource(BaseShader, source);
        }

        public unsafe Shader(GraphicsDevice graphicsDevice, ShaderType type, int headLineCount, string head, string additional, string source)
            : this(graphicsDevice, type)
        {
            const int numberOfSources = 4;

            var sources = ArrayPool<string>.Shared.Rent(numberOfSources);

            var lineSource = $"#line {headLineCount} 2\n";
            sources[0] = string.Format(head, graphicsDevice.GlslVersion == null ? 130 : graphicsDevice.GlslVersion.Major*100+graphicsDevice.GlslVersion.Minor);
            sources[1] = string.IsNullOrEmpty(additional) ? "\n" : additional;
            sources[2] = lineSource;
            sources[3] = source;

            GL.ShaderSource(BaseShader, numberOfSources, sources, (int[]?)null);

            ArrayPool<string>.Shared.Return(sources);
        }

        internal void Compile()
        {
            _graphicsDevice.ValidateUiGraphicsThread();
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
            _graphicsDevice.ValidateUiGraphicsThread();
            GL.DeleteProgram(BaseShader);
        }

    }
}

