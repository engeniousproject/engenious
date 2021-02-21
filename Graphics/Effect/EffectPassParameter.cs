using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a parameter of an <see cref="EffectPass"/>.
    /// </summary>
    public class EffectPassParameter
    {
        internal int Location;
        internal EffectPass Pass;
        internal EffectParameterType Type;

        private TextureCollection.TextureSlotReference? _currentTextureSlotReferene;

        internal EffectPassParameter(EffectPass pass, string name, int location,
            EffectParameterType type = (EffectParameterType) 0x7FFFFFFF)
        {
            Pass = pass;
            Location = location;
            Name = name;
            Type = type;
            _currentTextureSlotReferene = null;
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(bool value)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, value ? 1 : 0);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(bool[] values)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, values.Length, values.Cast<int>().ToArray());
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(int value)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(int[] values)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(uint value)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(uint[] values)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(float value)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(float[] values)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(double value)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(double[] values)
        {
            using (Pass.Apply())
                GL.Uniform1(Location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(string value)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Texture? value)
        {
            var dev = Pass.GraphicsDevice;
            _currentTextureSlotReferene?.Release();
            if (value == null)
            {
                _currentTextureSlotReferene = null;
                using (Pass.Apply())
                    GL.Uniform1(Location, 0);
                return;
            }
            var val = dev.Textures.InsertFree(value);
            if (val == null)
                throw new Exception("Out of textures");
            
            val.Acquire();
            _currentTextureSlotReferene = val;
            using (Pass.Apply())
                GL.Uniform1(Location, val.Slot);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector2 value)
        {
            using (Pass.Apply())
                GL.Uniform2(Location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2[] values)
        {
            using (Pass.Apply())
                unsafe
                {
                    fixed (Vector2* ptr = values)
                    {
                        GL.Uniform2(Location, values.Length, (float*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector2d value)
        {
            using (Pass.Apply())
                GL.Uniform2(Location, 1, (double*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2d[] values)
        {
            using (Pass.Apply())
                unsafe
                {
                    fixed (Vector2d* ptr = values)
                    {
                        GL.Uniform2(Location, values.Length, (double*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector3 value)
        {
            using (Pass.Apply())
                GL.Uniform3(Location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3[] values)
        {
            using (Pass.Apply())
                unsafe
                {
                    fixed (Vector3* ptr = values)
                    {
                        GL.Uniform3(Location, values.Length, (float*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector3d value)
        {
            using (Pass.Apply())
                GL.Uniform3(Location, 1, (double*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3d[] values)
        {
            using (Pass.Apply())
                unsafe
                {
                    fixed (Vector3d* ptr = values)
                    {
                        GL.Uniform3(Location, values.Length, (double*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4 value)
        {
            using (Pass.Apply())
                GL.Uniform4(Location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4[] values)
        {
            using (Pass.Apply())
                fixed (Vector4* ptr = values)
                {
                    GL.Uniform4(Location, values.Length, (float*) ptr); //TODO: verify?
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4d value)
        {
            using (Pass.Apply())
                GL.Uniform4(Location, 1, (double*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4d[] values)
        {
            using (Pass.Apply())
                fixed (Vector4d* ptr = values)
                {
                    GL.Uniform4(Location, values.Length, (double*) ptr); //TODO: verify?
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Matrix value)
        {
            using (Pass.Apply())
                GL.UniformMatrix4(Location, 1, false, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe void SetValue(Matrix[] values)
        {
            using (Pass.Apply())
                fixed (Matrix* ptr = values)
                {
                    GL.UniformMatrix4(Location, values.Length, false, (float*) ptr); //TODO: verify?
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Quaternion value)
        {
            using (Pass.Apply())
                GL.Uniform4(Location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Quaternion[] values)
        {
            using (Pass.Apply())
                unsafe
                {
                    fixed (Quaternion* ptr = values)
                    {
                        GL.Uniform4(Location, values.Length, (float*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(ConstantBuffer value)
        {
            GL.UniformBlockBinding(Pass.Program, Location, value.Ubo);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}