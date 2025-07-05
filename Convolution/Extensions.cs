using MathNet.Numerics.LinearAlgebra;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Numerics;

namespace Convolution;

public enum Colours
{
	Green,
	Blue,
	Red,
	Grayscale,
}

public enum GrayscaleType
{
	Average,
	Weighted,
}

public static class Extensions
{
	public static int ToGrayScale(this Rgba32 color, GrayscaleType type = GrayscaleType.Weighted)
	{
		int o = 0;
		switch (type)
		{
			case GrayscaleType.Weighted:
				o = (int) Math.Floor(0.299 * color.G + 0.587 * color.B + 0.114 * color.R);
				break;
			case GrayscaleType.Average:
				o = (int) Math.Floor((color.G + color.B + color.R)/3f);
				break;
		}
		
		return o;
	}

	public static double Sum(this Matrix<double> matrix)
	{
		var sum = 0d;
		for(int i = 0; i < matrix.RowCount; i++)
		{
			for(int j = 0; j < matrix.ColumnCount; j++)
			{
				sum += matrix.At(i, j);
			}
		}
		return sum;
	}

	public static Image<Rgba32> ToImage(this Matrix<double> matrix, Colours colour = Colours.Grayscale)
	{
		var image = new Image<Rgba32>(matrix.ColumnCount, matrix.RowCount);
		var norm = matrix.Normalize().Transpose();
		switch (colour)
		{
			case Colours.Grayscale:
				for(int i = 0; i < norm.RowCount; i++)
				{
					for(int j = 0; j < norm.ColumnCount; j++)
					{
						var x = (float)norm[i, j]/3f;
						image[i, j] = new Rgba32(x, x, x, 1);
					}
				}
				break;
			case Colours.Red:
				for(int i = 0; i < norm.RowCount; i++)
				{
					for(int j = 0; j < norm.ColumnCount; j++)
					{
						var x = (float)matrix[i, j];
						image[i, j] = new Rgba32(x, 0, 0);
					}
				}
				break;
			case Colours.Green:
				for(int i = 0; i < matrix.RowCount; i++)
				{
					for(int j = 0; j < matrix.ColumnCount; j++)
					{
						var x = (float)matrix[i, j];
						image[i, j] = new Rgba32(0, x, 0);
					}
				}
				break;
			case Colours.Blue:
				for(int i = 0; i < matrix.RowCount; i++)
				{
					for(int j = 0; j < matrix.ColumnCount; j++)
					{
						var x = (float)matrix[i, j];
						image[i, j] = new Rgba32(0, 0, x);
					}
				}
				break;
		} 
		return image;
	}

	public static Matrix<double> ToReal(this Matrix<Complex> m) 
		=> m.Map(x => x.Real);
	public static MathNet.Numerics.LinearAlgebra.Vector<double> ToReal(this MathNet.Numerics.LinearAlgebra.Vector<Complex> v) 
		=> v.Map(x => x.Real);

	public static MathNet.Numerics.LinearAlgebra.Vector<double> Normalize(this MathNet.Numerics.LinearAlgebra.Vector<double> v, int peak=1)
	{
		var min = v.Min();
		var max = v.Max();

		return v.Map(x => (x - min) / (max - min))*peak;
	}
	public static Matrix<double> Normalize(this Matrix<double> m, int peak=1)
	{
		var min = m.Enumerate().Min();
		var max = m.Enumerate().Max();

		return m.Map(x => (x - min) / (max - min))*peak;
	}
}