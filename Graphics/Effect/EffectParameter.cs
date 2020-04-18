using System.Collections.Generic;
using engenious.Helper;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a parameter of an <see cref="Effect"/>.
    /// </summary>
    public sealed class EffectParameter
    {
        private readonly List<EffectPassParameter> _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParameter"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public EffectParameter(string name)
        {
            _parameters = new List<EffectPassParameter>();
            Name = name;
        }

        internal void Add(EffectPassParameter param)
        {
            _parameters.Add(param);
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name{ get; private set; }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(bool value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(bool[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(int value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(int[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(uint value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(uint[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(float value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(float[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(double value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(double[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(string value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Texture value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector2 value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector2d value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2d[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector3 value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector3d value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3d[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector4 value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector4[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector4d value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector4d[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Matrix value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Matrix[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Quaternion value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Quaternion[] values)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(values);
                }
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(ConstantBuffer value)
        {
            using (Execute.OnUiContext)
            {
                foreach (var param in _parameters)
                {
                    param.Pass.Apply();
                    param.SetValue(value);
                }
            }
        }
    }
}

