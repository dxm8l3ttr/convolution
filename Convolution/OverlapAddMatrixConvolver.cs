using MathNet.Numerics.LinearAlgebra;
using System.Numerics;

namespace Convolution;

public sealed class OverlapAddMatrixConvolver : MatrixConvolver
{
    public OverlapAddMatrixConvolver(Matrix<Complex> image, Matrix<Complex> kernel) : base (image, kernel) {}

    public override void Convolve()
    {

        var m = X.RowCount;
        var n = X.ColumnCount;

    	var f = Math.Max(m, n);
    	var P = (int)Math.Pow(2, Math.Ceiling(Math.Log2(f)));
    
    	var Q = (int) Math.Floor(P / 2d);

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

        var cropped = CropConvoluted(Y, X.RowCount, X.ColumnCount);
    
        X.SetSubMatrix(0, 0, cropped);
    }
}