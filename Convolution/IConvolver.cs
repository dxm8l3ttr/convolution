using System.Numerics;

namespace Convolution;

public interface IConvolver<T> 
{
    public T X { get; init; }
    public T H { get; init; }
    public void Convolve();
}