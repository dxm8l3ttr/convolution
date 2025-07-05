using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.LinearAlgebra;

namespace Convolution;

public abstract class VectorConvolver : IConvolver<MathNet.Numerics.LinearAlgebra.Vector<Complex>>
{
    public MathNet.Numerics.LinearAlgebra.Vector<Complex> X { get; init; }
    public MathNet.Numerics.LinearAlgebra.Vector<Complex> H { get; init; }

    public VectorConvolver(MathNet.Numerics.LinearAlgebra.Vector<Complex> image, MathNet.Numerics.LinearAlgebra.Vector<Complex> kernel)
    {
        X = image;
        H = kernel;
    }

    protected MathNet.Numerics.LinearAlgebra.Vector<Complex> ZeroPad(MathNet.Numerics.LinearAlgebra.Vector<Complex> input, int targetSize)
    {
        var result = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.Dense(targetSize, Complex.Zero);
        result.SetSubVector(0, input.Count, input);
        return result;
    }
    
    protected MathNet.Numerics.LinearAlgebra.Vector<Complex> CropConvoluted(MathNet.Numerics.LinearAlgebra.Vector<Complex> convoluted, int originalSize)
    {
        return convoluted.SubVector(0, originalSize);
    }
    
    protected MathNet.Numerics.LinearAlgebra.Vector<Complex> FFT(MathNet.Numerics.LinearAlgebra.Vector<Complex> input)
    {
        var array = input.ToArray();
        Fourier.Forward(array);
        return MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.DenseOfArray(array);
    }
    protected MathNet.Numerics.LinearAlgebra.Vector<Complex> IFFT(MathNet.Numerics.LinearAlgebra.Vector<Complex> input)
    {
        var array = input.ToArray();
        Fourier.Inverse(array);
        return MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.DenseOfArray(array);
    }

    public abstract void Convolve();
}
