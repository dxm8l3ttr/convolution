using MathNet.Numerics.LinearAlgebra;

namespace Convolution;

public class RealVectorConvolver : IConvolver<Vector<double>>
{
    private readonly VectorConvolver _convolver;

    public Vector<double> X { get; init; }
    public Vector<double> H { get; init; }

    public RealVectorConvolver
    (
        Vector<double> image,
        Vector<double> kernel,
        ConvolutionMethod method=ConvolutionMethod.OverlapAdd
    )
    {
        X = image;
        H = kernel;
        _convolver = (VectorConvolver) ConvolverFactory.Create(image.ToComplex(), kernel.ToComplex(), method);
    }

    public void Convolve()
    {
        _convolver.Convolve();
        X.SetValues(_convolver.X.ToReal().ToArray());
    }
}

public class RealMatrixConvolver : IConvolver<Matrix<double>>
{
    private readonly MatrixConvolver _convolver;
    public Matrix<double> X { get; init; }
    public Matrix<double> H { get; init; }
    public RealMatrixConvolver(Matrix<double> image, Matrix<double> kernel, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        X = image;
        H = kernel;
        _convolver = (MatrixConvolver) ConvolverFactory.Create(image.ToComplex(), kernel.ToComplex(), method);
    }

    public void Convolve()
    {
        _convolver.Convolve();
        X.SetSubMatrix(0, 0, _convolver.X.ToReal());
    }
}