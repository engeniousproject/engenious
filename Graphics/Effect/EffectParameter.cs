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
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(bool[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(int value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(int[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(uint value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(uint[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(float value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(float[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(string value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(Texture value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(Vector2 value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(Vector2[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(Vector3 value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(Vector3[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(Vector4 value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(Vector4[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(Matrix value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(Matrix[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(Quaternion value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

        public void SetValue(Quaternion[] values)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(values);
                    }
                });
        }

        public void SetValue(ConstantBuffer value)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    foreach (var param in parameters)
                    {
                        param.pass.Apply();
                        param.SetValue(value);
                    }
                });
        }

    }
}

