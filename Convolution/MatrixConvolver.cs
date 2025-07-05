using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.LinearAlgebra;

namespace Convolution;

public abstract class MatrixConvolver : IConvolver<Matrix<Complex>>
{
    public Matrix<Complex> X { get; init; }
    public Matrix<Complex> H { get; init; }

    // Function to Zero-Pad a matrix to the desired size (P x P)
    protected Matrix<Complex> ZeroPad(Matrix<Complex> inputBlock, int targetSize)
    {
        // Create a matrix of the target size initialized with zeros
        var paddedBlock = Matrix<Complex>.Build.Dense(targetSize, targetSize, Complex.Zero);

        // Use SetSubmatrix to copy the inputBlock into the top-left corner of the padded block
        paddedBlock.SetSubMatrix(0, 0, inputBlock);

        return paddedBlock;
    }

    // Function to perform 2D FFT on a padded block
    protected Matrix<Complex> FFT2D(Matrix<Complex> block)
    {
        // Perform the 2D FFT using MathNet
        var fftBlock = block.Clone(); // Clone to avoid modifying original matrix
        Fourier.Forward2D(fftBlock); 

        return fftBlock;
    }

    // Function to multiply in frequency domain (element-wise)
    protected Matrix<Complex> MultiplyFrequencyDomain(Matrix<Complex> inputFreq, Matrix<Complex> filterFreq)
    {
        return inputFreq.PointwiseMultiply(filterFreq);
    }

    // Function to perform 2D Inverse FFT on a block (Inverse FFT)
    protected Matrix<Complex> IFFT2D(Matrix<Complex> freqBlock)
    {
        // Perform the 2D Inverse FFT using MathNet
        var ifftBlock = freqBlock.Clone(); // Clone to avoid modifying original matrix
        Fourier.Inverse2D(ifftBlock); 

        return ifftBlock;
    }

    protected Matrix<Complex> CropConvoluted(Matrix<Complex> convoluted, int originalRows, int originalCols)
    {
        return convoluted.SubMatrix(0, originalRows, 0, originalCols);
    }

    public MatrixConvolver(Matrix<Complex> image, Matrix<Complex> kernel)
    {
        X = image;
        H = kernel;
    }

    public abstract void Convolve();
}
