using System.Collections.Generic;
using engenious.Helper;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a parameter of an <see cref="Effect"/>.
    /// </summary>
    public sealed class EffectParameter : GraphicsResource
    {
        private readonly List<EffectPassParameter> _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParameter"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> the resource is allocated on.</param>
        /// <param name="name">The name of the parameter.</param>
        public EffectParameter(GraphicsDevice graphicsDevice, string name)
            : base(graphicsDevice)
        {
            _parameters = new List<EffectPassParameter>();
            Name = name;
        }

        internal void Add(EffectPassParameter param)
        {
            _parameters.Add(param);
        }

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;
        
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public new string Name
        {
            get => base.Name!;
            private init => base.Name = value;
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(bool value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(bool[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(int value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(int[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(uint value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(uint[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(float value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(float[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(double value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(double[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(string value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Texture? value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector2 value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector2d value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector2d[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector3 value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector3d value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector3d[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector4 value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector4[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Vector4d value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Vector4d[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Matrix value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Matrix[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(Quaternion value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="values">The value to set the parameter to.</param>
        public void SetValue(Quaternion[] values)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(values);
            }
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="value">The value to set the parameter to.</param>
        public void SetValue(ConstantBuffer value)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            foreach (var param in _parameters)
            {
                param.SetValue(value);
            }
        }
    }
}

