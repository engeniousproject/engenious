using System;
using OpenTK.Graphics.OpenGL4;
using System.Linq;
using OpenTK;
using engenious.Graphics;

namespace engenious
{
    public class EffectPassParameter
    {
        internal int location;
        internal EffectPass pass;

        internal EffectPassParameter(EffectPass pass, string name, int location)
        {
            this.pass = pass;
            this.location = location;
            Name = name;
        }

        public string Name{ get; private set; }

        public void SetValue(bool value)
        {
            pass.Apply();
            GL.Uniform1(location, value ? 1 : 0);
        }

        public void SetValue(bool[] values)
        {
            pass.Apply();
            GL.Uniform1(location, values.Length, values.Cast<int>().ToArray());
        }

        public void SetValue(int value)
        {
            pass.Apply();
            GL.Uniform1(location, value);
        }

        public void SetValue(int[] values)
        {
            pass.Apply();
            GL.Uniform1(location, values.Length, values);
        }

        public void SetValue(uint value)
        {
            GL.Uniform1(location, value);
        }

        public void SetValue(uint[] values)
        {
            GL.Uniform1(location, values.Length, values);
        }

        public void SetValue(float value)
        {
            GL.Uniform1(location, value);
        }

        public void SetValue(float[] values)
        {
            GL.Uniform1(location, values.Length, values);
        }

        public void SetValue(string value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(Texture value)
        {
            GraphicsDevice dev = value.GraphicsDevice;
            int val = dev.Textures.InsertFree(value);
            if (val == -1)
                throw new Exception("Out of textures");
            GL.Uniform1(location, val);
        }

        public unsafe void SetValue(Vector2 value)
        {
            GL.Uniform2(location, 1, (float*)&value);
        }

        public void SetValue(Vector2[] values)
        {
            unsafe
            {
                fixed(Vector2* ptr = values)
                {
                    GL.Uniform2(location, values.Length, (float*)ptr);//TODO: verify?
                }
            }
        }

        public unsafe void SetValue(Vector3 value)
        {
            GL.Uniform3(location, 1, (float*)&value);
        }

        public void SetValue(Vector3[] values)
        {
            unsafe
            {
                fixed(Vector3* ptr = values)
                {
                    GL.Uniform3(location, values.Length, (float*)ptr);//TODO: verify?
                }
            }
        }

        public unsafe void SetValue(Vector4 value)
        {
            GL.Uniform4(location, 1, (float*)&value);
        }

        public unsafe void SetValue(Vector4[] values)
        {
            fixed(Vector4* ptr = values)
            {
                GL.Uniform4(location, values.Length, (float*)ptr);//TODO: verify?
            }
        }

        public unsafe void SetValue(Matrix value)
        {
            GL.UniformMatrix4(location, 1, false, (float*)&value);
        }

        public unsafe void SetValue(Matrix[] values)
        {
            fixed(Matrix* ptr = values)
            {
                GL.UniformMatrix4(location, values.Length, false, (float*)ptr);//TODO: verify?
            }
        }

        public unsafe void SetValue(Quaternion value)
        {
            GL.Uniform4(location, 1, (float*)&value);
        }

        public void SetValue(Quaternion[] values)
        {
            unsafe
            {
                fixed(Quaternion* ptr = values)
                {
                    GL.Uniform4(location, values.Length, (float*)ptr);//TODO: verify?
                }
            }
        }
			
    }
}

