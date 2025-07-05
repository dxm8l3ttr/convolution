using System.Numerics;

namespace Convolution;

public sealed class OverlapSaveVectorConvolver : VectorConvolver
{
    public OverlapSaveVectorConvolver(MathNet.Numerics.LinearAlgebra.Vector<Complex> image, MathNet.Numerics.LinearAlgebra.Vector<Complex> kernel) : base(image, kernel)
    {
    }

    public override void Convolve()
    {
        throw new NotImplementedException();
    }
}