using MathNet.Numerics.LinearAlgebra;
using System.Numerics;

namespace Convolution;

public sealed class OverlapSaveMatrixConvolver : MatrixConvolver
{
    public OverlapSaveMatrixConvolver(Matrix<Complex> image, Matrix<Complex> kernel) : base (image, kernel) {}
    public override void Convolve()
    {
        throw new NotImplementedException();
    }
}