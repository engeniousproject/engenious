using System;

namespace engenious
{
    public interface IEffectMatrices
    {
        Matrix Projection{ get; set; }

        Matrix View{ get; set; }

        Matrix World{ get; set; }
    }
}

