using System.Numerics;

namespace Convolution;

public sealed class OverlapAddVectorConvolver : VectorConvolver
{
    public OverlapAddVectorConvolver(MathNet.Numerics.LinearAlgebra.Vector<Complex> image, MathNet.Numerics.LinearAlgebra.Vector<Complex> kernel) : base(image, kernel)
    {
    }

    public override void Convolve()
    {
        var L = CalculateOptimalBlockSize(H.Count);
        int M = H.Count;
        int N = L + M - 1;

        // Output length
        int outputLength = X.Count + M - 1;
        var y = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.Dense(outputLength);

        // FFT of zero-padded h
        var hFreq = FFT(ZeroPad(H, N));

        for (int i = 0; i < X.Count; i += L)
        {
            // Get current block of x
            int blockLength = Math.Min(L, X.Count-i);
            var xBlock = X.SubVector(i, blockLength);

            // Zero pad block to length N
            var xBlockPadded = ZeroPad(xBlock, N);

            // FFT of x block
            var xFreq = FFT(xBlockPadded);

            // Multiply in frequency domain
            var Y = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.Dense(N);
            for (int k = 0; k < N; k++)
            {
                Y[k] = xFreq[k] * hFreq[k];
            }

            // IFFT to get time domain result
            var yBlockComplex = IFFT(Y);
            var yBlock = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.Dense(N);
            for (int k = 0; k < N; k++)
            {
                yBlock[k] = yBlockComplex[k].Real;
            }

            // Overlap-add to output
            for (int k = 0; k < N; k++)
            {
                if (i + k < outputLength)
                    y[i + k] += yBlock[k];
            }
        }

        X.SetValues(CropConvoluted(y, X.Count).ToArray());
    }

    private int CalculateOptimalBlockSize(int filterLength)
    {
        var M = filterLength;

        // Next power of 2 greater than or equal to 2 * M
        var N = (int)Math.Pow(2, Math.Ceiling(Math.Log(2 * M, 2)));

        // Compute L
        var L = N - M + 1;

        return L;
    }
}