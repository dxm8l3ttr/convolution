using System.Drawing;
using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
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
	public static int ToGrayScale(this Color color, GrayscaleType type = GrayscaleType.Weighted)
	{
		int o = 0;
		switch (type)
		{
			case GrayscaleType.Weighted:
				o = (int) Math.Floor((0.299 * color.G + 0.587 * color.B + 0.114 * color.R));
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

	public static Bitmap ToBitmap(this Matrix<double> matrix, Colours colour = Colours.Grayscale)
	{
		var map = new Bitmap(matrix.ColumnCount, matrix.RowCount);
		switch (colour)
		{
			case Colours.Grayscale:
				for(int i = 0; i < matrix.RowCount; i++)
				{
					for(int j = 0; j < matrix.ColumnCount; j++)
					{
						var x = (int)matrix.At(i, j)/3;
						map.SetPixel(j, i, Color.FromArgb(255, x, x, x));
					}
				}
				break;
			case Colours.Red:
				for(int i = 0; i < matrix.RowCount; i++)
				{
					for(int j = 0; j < matrix.ColumnCount; j++)
					{
						var x = (int)matrix.At(i, j);
						map.SetPixel(j, i, Color.FromArgb(255, x, 0, 0));
					}
				}
				break;
			case Colours.Green:
				for(int i = 0; i < matrix.RowCount; i++)
				{
					for(int j = 0; j < matrix.ColumnCount; j++)
					{
						var x = (int)matrix.At(i, j);
						map.SetPixel(j, i, Color.FromArgb(255, 0, x, 0));
					}
				}
				break;
			case Colours.Blue:
				for(int i = 0; i < matrix.RowCount; i++)
				{
					for(int j = 0; j < matrix.ColumnCount; j++)
					{
						var x = (int)matrix.At(i, j);
						map.SetPixel(j, i, Color.FromArgb(255, 0, 0, x));
					}
				}
				break;
		} 
		return map;
	}

	public static Matrix<double> ToReal(this Matrix<Complex> m)
	{
		return Matrix<double>.Build.Dense(m.RowCount, m.ColumnCount, (i, j) => (double)m.At(i, j).Real);
	}

	public static Matrix<double> Normalize(this Matrix<double> m, int peak=1)
	{
		var min = m.Enumerate().Min();
		var max = m.Enumerate().Max();

		return m.Map(x => (x - min) / (max - min))*peak;
	}
}