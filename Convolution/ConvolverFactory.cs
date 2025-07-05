using MathNet.Numerics.LinearAlgebra;
using System.Numerics;

namespace Convolution;

public enum ConvolutionMethod
{
    OverlapAdd,
    OverlapSave,
}

public static class ConvolverFactory
{
    public static IConvolver<Matrix<Complex>> Create(Matrix<Complex> X, Matrix<Complex> H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        return CreateMatrix(X, H, method);
    }
    public static IConvolver<Matrix<Complex>> Create(Complex[,] X, Complex[,] H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        var xMatrix = Matrix<Complex>.Build.DenseOfArray(X);
        var hMatrix = Matrix<Complex>.Build.DenseOfArray(H);
        return Create(xMatrix, hMatrix, method);
    }
    public static IConvolver<MathNet.Numerics.LinearAlgebra.Vector<Complex>> Create(Complex[] X, Complex[] H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        var xVector = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.DenseOfArray(X);
        var hVector = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.DenseOfArray(H);
        return Create(xVector, hVector, method);
    }
    public static IConvolver<MathNet.Numerics.LinearAlgebra.Vector<Complex>> Create
        (MathNet.Numerics.LinearAlgebra.Vector<Complex> X, 
        MathNet.Numerics.LinearAlgebra.Vector<Complex> H, 
        ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        return CreateVector(X, H, method);
    }
    public static IConvolver<MathNet.Numerics.LinearAlgebra.Vector<double>> Create(double[] X,
        double[] H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        var xVector = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray(X);
        var hVector = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray(H);

        return new RealVectorConvolver(xVector, hVector, method);
    }
    public static IConvolver<MathNet.Numerics.LinearAlgebra.Vector<double>> Create(
        MathNet.Numerics.LinearAlgebra.Vector<double> X, MathNet.Numerics.LinearAlgebra.Vector<double> H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        return new RealVectorConvolver(X, H, method);
    }
    public static IConvolver<Matrix<double>> Create(double[,] X, double[,] H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        var xMatrix = Matrix<double>.Build.DenseOfArray(X);
        var hMatrix = Matrix<double>.Build.DenseOfArray(H);
        return new RealMatrixConvolver(xMatrix, hMatrix, method);
    }
    public static IConvolver<Matrix<double>> Create(Matrix<double> X, Matrix<double> H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        return new RealMatrixConvolver(X, H, method);
    }
    private static VectorConvolver CreateVector(MathNet.Numerics.LinearAlgebra.Vector<Complex> X, 
        MathNet.Numerics.LinearAlgebra.Vector<Complex> H, 
        ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        switch (method)
        {
            case ConvolutionMethod.OverlapAdd:
                return new OverlapAddVectorConvolver(X, H);
            case ConvolutionMethod.OverlapSave:
                return new OverlapSaveVectorConvolver(X, H);
            default:
                return new OverlapAddVectorConvolver(X, H);
        }
    }
    private static MatrixConvolver CreateMatrix(Matrix<Complex> X, Matrix<Complex> H, ConvolutionMethod method=ConvolutionMethod.OverlapAdd)
    {
        switch (method)
        {
            case ConvolutionMethod.OverlapAdd:
                return new OverlapAddMatrixConvolver(X, H);
            case ConvolutionMethod.OverlapSave:
                return new OverlapSaveMatrixConvolver(X, H);
            default:
                return new OverlapAddMatrixConvolver(X, H);
        }
    }
}