using System;
using System.Collections.Generic;
using OpenTK;

namespace engenious.Graphics
{
    public sealed class EffectParameter
    {
        private List<EffectPassParameter> parameters;

        public EffectParameter(string name)
        {
            this.parameters = new List<EffectPassParameter>();
            this.Name = name;
        }

        internal void Add(EffectPassParameter param)
        {
            parameters.Add(param);
        }

        public string Name{ get; private set; }

        public void SetValue(bool value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(bool[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(int value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(int[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(uint value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(uint[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(float value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(float[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(string value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(Texture value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(Vector2 value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(Vector2[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(Vector3 value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(Vector3[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(Vector4 value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(Vector4[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(Matrix value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(Matrix[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

        public void SetValue(Quaternion value)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(value);
            }
        }

        public void SetValue(Quaternion[] values)
        {
            foreach (var param in parameters)
            {
                param.pass.Apply();
                param.SetValue(values);
            }
        }

    }
}

