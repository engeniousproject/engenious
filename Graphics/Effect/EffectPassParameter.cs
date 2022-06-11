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
        /// <summary>
        ///     Gets a value indicating the gpu driver location of the parameter.
        /// </summary>
        public int Location { get; }
        internal EffectPass Pass;
        internal EffectParameterType Type;

        private TextureCollection.TextureSlotReference? _currentTextureSlotReference;

        internal EffectPassParameter(EffectPass pass, string name, int location, int size,
            EffectParameterType type = (EffectParameterType) 0x7FFFFFFF)
        {
            Pass = pass;
            Location = location;
            Name = name;
            Type = type;
            Size = size;
            _currentTextureSlotReference = null;
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Gets the size of the parameter buffer/array.
        /// </summary>
        public int Size { get; }

        #region SetValues
        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(bool value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(bool[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(int value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(int[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(uint value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(uint[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(float value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(float[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(double value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(double[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(string value) => EffectPassParameter.SetValue(Pass, Location, value);
        
        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Texture? value) => EffectPassParameter.SetValue(Pass, Location, value, ref _currentTextureSlotReference);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector2 value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector2d value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2d[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector3 value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector3d value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3d[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4 value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4d value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe void SetValue(Vector4d[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Matrix value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe void SetValue(Matrix[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe void SetValue(Quaternion value) => EffectPassParameter.SetValue(Pass, Location, value);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Quaternion[] values) => EffectPassParameter.SetValue(Pass, Location, values);

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(ConstantBuffer value) => EffectPassParameter.SetValue(Pass, Location, value);
        #endregion
        #region SetValuesStatic
        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, bool value)
        {
            using (pass.Apply())
                GL.Uniform1(location, value ? 1 : 0);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, bool[] values)
        {
            using (pass.Apply())
                GL.Uniform1(location, values.Length, values.Cast<int>().ToArray());
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, int value)
        {
            using (pass.Apply())
                GL.Uniform1(location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, int[] values)
        {
            using (pass.Apply())
                GL.Uniform1(location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, uint value)
        {
            using (pass.Apply())
                GL.Uniform1(location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, uint[] values)
        {
            using (pass.Apply())
                GL.Uniform1(location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, float value)
        {
            using (pass.Apply())
                GL.Uniform1(location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, float[] values)
        {
            using (pass.Apply())
                GL.Uniform1(location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, double value)
        {
            using (pass.Apply())
                GL.Uniform1(location, value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, double[] values)
        {
            using (pass.Apply())
                GL.Uniform1(location, values.Length, values);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, string value)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        /// <param name="currentTextureSlotReference">The currently used slot reference.</param>
        public static void SetValue(EffectPass pass, int location, Texture? value, ref TextureCollection.TextureSlotReference? currentTextureSlotReference)
        {
            var dev = pass.GraphicsDevice;
            currentTextureSlotReference?.Release();
            if (value == null)
            {
                currentTextureSlotReference = null;
                using (pass.Apply())
                    GL.Uniform1(location, 0);
                return;
            }
            var val = dev.Textures.InsertFree(value);
            if (val == null)
                throw new Exception("Out of textures");
            
            val.Acquire();
            currentTextureSlotReference = val;
            using (pass.Apply())
                GL.Uniform1(location, val.Slot);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector2 value)
        {
            using (pass.Apply())
                GL.Uniform2(location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, Vector2[] values)
        {
            using (pass.Apply())
                unsafe
                {
                    fixed (Vector2* ptr = values)
                    {
                        GL.Uniform2(location, values.Length, (float*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector2d value)
        {
            using (pass.Apply())
                GL.Uniform2(location, 1, (double*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, Vector2d[] values)
        {
            using (pass.Apply())
                unsafe
                {
                    fixed (Vector2d* ptr = values)
                    {
                        GL.Uniform2(location, values.Length, (double*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector3 value)
        {
            using (pass.Apply())
                GL.Uniform3(location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, Vector3[] values)
        {
            using (pass.Apply())
                unsafe
                {
                    fixed (Vector3* ptr = values)
                    {
                        GL.Uniform3(location, values.Length, (float*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector3d value)
        {
            using (pass.Apply())
                GL.Uniform3(location, 1, (double*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, Vector3d[] values)
        {
            using (pass.Apply())
                unsafe
                {
                    fixed (Vector3d* ptr = values)
                    {
                        GL.Uniform3(location, values.Length, (double*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector4 value)
        {
            using (pass.Apply())
                GL.Uniform4(location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector4[] values)
        {
            using (pass.Apply())
                fixed (Vector4* ptr = values)
                {
                    GL.Uniform4(location, values.Length, (float*) ptr); //TODO: verify?
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector4d value)
        {
            using (pass.Apply())
                GL.Uniform4(location, 1, (double*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Vector4d[] values)
        {
            using (pass.Apply())
                fixed (Vector4d* ptr = values)
                {
                    GL.Uniform4(location, values.Length, (double*) ptr); //TODO: verify?
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Matrix value)
        {
            using (pass.Apply())
                GL.UniformMatrix4(location, 1, false, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Matrix[] values)
        {
            using (pass.Apply())
                fixed (Matrix* ptr = values)
                {
                    GL.UniformMatrix4(location, values.Length, false, (float*) ptr); //TODO: verify?
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public unsafe static void SetValue(EffectPass pass, int location, Quaternion value)
        {
            using (pass.Apply())
                GL.Uniform4(location, 1, (float*) &value);
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="values">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, Quaternion[] values)
        {
            using (pass.Apply())
                unsafe
                {
                    fixed (Quaternion* ptr = values)
                    {
                        GL.Uniform4(location, values.Length, (float*) ptr); //TODO: verify?
                    }
                }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="pass">The <see cref="EffectPass"/> to set the uniform of.</param>
        /// <param name="location">The uniform location to set the value of.</param>
        /// <param name="value">The value to set the parameter to.</param>
        public static void SetValue(EffectPass pass, int location, ConstantBuffer value)
        {
            GL.UniformBlockBinding(pass.Program, location, value.Ubo);
        }
        #endregion
        
        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}