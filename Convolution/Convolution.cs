using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.IntegralTransforms;
using System;
using System.Numerics;

namespace Convolution;

public static class Convolution
{
    // Function to Zero-Pad a matrix to the desired size (P x P)
    private static Matrix<Complex> ZeroPad(Matrix<Complex> inputBlock, int targetSize)
    {
        // Create a matrix of the target size initialized with zeros
        var paddedBlock = Matrix<Complex>.Build.Dense(targetSize, targetSize, Complex.Zero);

        // Use SetSubmatrix to copy the inputBlock into the top-left corner of the padded block
        paddedBlock.SetSubMatrix(0, 0, inputBlock);

        return paddedBlock;
    }

    // Function to perform 2D FFT on a padded block
    private static Matrix<Complex> FFT2D(Matrix<Complex> block)
    {
        // Perform the 2D FFT using MathNet
        var fftBlock = block.Clone(); // Clone to avoid modifying original matrix
        Fourier.Forward2D(fftBlock); 

        return fftBlock;
    }

    // Function to multiply in frequency domain (element-wise)
    private static Matrix<Complex> MultiplyFrequencyDomain(Matrix<Complex> inputFreq, Matrix<Complex> filterFreq)
    {
        return inputFreq.PointwiseMultiply(filterFreq);
    }

    // Function to perform 2D Inverse FFT on a block (Inverse FFT)
    private static Matrix<Complex> IFFT2D(Matrix<Complex> freqBlock)
    {
        // Perform the 2D Inverse FFT using MathNet
        var ifftBlock = freqBlock.Clone(); // Clone to avoid modifying original matrix
        Fourier.Inverse2D(ifftBlock); 

        return ifftBlock;
    }

    private static Matrix<double> CropConvoluted(Matrix<double> convoluted, int originalRows, int originalCols)
    {
        return convoluted.Transpose().SubMatrix(0, originalCols, 0, originalRows);
    }

    // Overlap-Add 2D Convolution Algorithm
    public static Matrix<double> OverlapAdd(Matrix<Complex> image, Matrix<Complex> kernel, int blockSize=0, int overlapSize=0)
    {
        var X = image.Clone();
        var H = kernel.Clone();

    	var P = blockSize;  // Target FFT size (should be power of 2 for efficiency)
    	var Q = overlapSize;

        var m = X.RowCount;
        var n = X.ColumnCount;

        if (blockSize == 0)
    	{
    		var f = Math.Max(m, n);
    		P = (int)Math.Pow(2, Math.Ceiling(Math.Log2(f)));
    	}
    	if (overlapSize == 0)
    	{
    		Q = (int) Math.Floor(P / 2d);
    	}

        // Zero-pad the filter to P x P
        var H_padded = FFT2D(ZeroPad(H, P));
        
        // Initialize the output matrix (padded size)
        var Y = Matrix<Complex>.Build.Dense(m + P - 1, n + P - 1, Complex.Zero);

        // Process blocks of the input matrix X
        for (int i = 0; i < m; i += Q)
        {
            for (int j = 0; j < n; j += Q)
            {
                // Extract current block from the input matrix X (of size overlapSize x overlapSize)
                var subR = Q; 
                var subC = Q;
                if (i >= m-Q)
                {
                    subR = X.RowCount-i;
                }
                if (j >= n-Q)
                {
                    subC = X.ColumnCount-j;
                }

                var X_block = X.SubMatrix(i, subR, j, subC);
                
                // Zero-pad the current block
                var X_padded = ZeroPad(X_block, P);

                // Perform FFT on the padded block (2D FFT)
                var X_freq = FFT2D(X_padded);

                // Multiply in the frequency domain
                var Y_freq = MultiplyFrequencyDomain(X_freq, H_padded);

                // Perform Inverse FFT (2D IFFT)
                var Y_block = IFFT2D(Y_freq);

                // Add the resulting block to the output matrix (Overlap-Add)
                Y.SetSubMatrix(i, P, j, P, Y.SubMatrix(i, P, j, P)+Y_block);
            }
        }

        // Optionally, convert the complex result back to real values (if desired)
        var realY = Matrix<Double>.Build.Dense(m + P - 1, n + P - 1, (i, j) => (double)Y[i, j].Real);

        return CropConvoluted(realY, image.RowCount, image.ColumnCount);
    }

    public static Matrix<double> OverlapAdd(Matrix<double> image, Matrix<double> kernel, int blockSize=0, int overlapSize=0)
    {
        return OverlapAdd(image.ToComplex(), kernel.ToComplex(), blockSize, overlapSize);
    }


    public static Matrix<double> OverlapAddNormalized(Matrix<Complex> image, Matrix<Complex> kernel, int peak, int blockSize=0, int overlapSize=0)
    {
        return OverlapAdd(image, kernel, blockSize, overlapSize).Normalize(peak);
    }

    public static Matrix<double> OverlapAddNormalized(Matrix<double> image, Matrix<double> kernel, int peak, int blockSize=0, int overlapSize=0)
    {
        return OverlapAddNormalized(image.ToComplex(), kernel.ToComplex(), peak, blockSize, overlapSize);
    }
}