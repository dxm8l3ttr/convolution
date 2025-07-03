using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using Convolution;

namespace Convolution;


public class ImageFile
{
	public Bitmap Map { get; private set; }
	private MatrixBuilder<double> _builder;

	public ImageFile(string path)
	{
		_builder = Matrix<double>.Build;
		Map = (Bitmap)Image.FromFile(path);
	}

	public ImageFile(string path, Matrix<double> matrix, Colours colour=Colours.Grayscale)
	{
		_builder = Matrix<double>.Build;
		Map = matrix.ToBitmap(colour);
		Map.Save(path);
	}

	public ImageFile(string path, Matrix<double> A, Matrix<double> R, Matrix<double> G, Matrix<double> B)
	{
		_builder = Matrix<double>.Build;
		Map = new Bitmap(A.ColumnCount, A.RowCount);
		for(int i = 0; i < A.RowCount; i++)
		{
			for(int j = 0; j < A.ColumnCount; j++)
			{
				var a = (int)A.At(i, j);
				var r = (int)R.At(i, j);
				var g = (int)G.At(i, j);
				var b = (int)B.At(i, j);
				Map.SetPixel(j, i, Color.FromArgb(a, r, g, b));
			}
		}
		Map.Save(path);
	}

	public ImageFile(string path, Matrix<double> R, Matrix<double> G, Matrix<double> B)
	{
		_builder = Matrix<double>.Build;
		Map = new Bitmap(R.ColumnCount, R.RowCount);
		for(int i = 0; i < R.RowCount; i++)
		{
			for(int j = 0; j < R.ColumnCount; j++)
			{
				var r = (int)R.At(i, j);
				var g = (int)G.At(i, j);
				var b = (int)B.At(i, j);
				Map.SetPixel(j, i, Color.FromArgb(255, r, g, b));
			}
		}
		Map.Save(path);
	}

	public Matrix<double> ToMatrix()
	{
		var matrix = _builder
			.Dense(Map.Width, Map.Height, (i, j) => Map
				.GetPixel(i, j)
				.ToGrayScale());
		return matrix;
	}

	public Matrix<double> ToMatrix(Colours colour)
	{
		var matrix = _builder.Dense(Map.Width, Map.Height);
		switch (colour)
		{
			case Colours.Blue:
				matrix = _builder
					.Dense(Map.Width, Map.Height, (i, j) => Map
						.GetPixel(i, j).B);
				break;
			case Colours.Green:
				matrix = _builder
					.Dense(Map.Width, Map.Height, (i, j) => Map
						.GetPixel(i, j).G);
				break;
			case Colours.Red:
				matrix = _builder
					.Dense(Map.Width, Map.Height, (i, j) => Map
						.GetPixel(i, j).R);
				break;
			case Colours.Grayscale:
				matrix = this.ToMatrix();
				break;
		}
		return matrix;
	}

	public (Matrix<double>, Matrix<double>, Matrix<double>, Matrix<double>) ToMatrices()
	{
		var A = _builder
					.Dense(Map.Width, Map.Height, (i, j) => Map
						.GetPixel(i, j).A);
		var R = this.ToMatrix(Colours.Red);
		var G = this.ToMatrix(Colours.Green);
		var B = this.ToMatrix(Colours.Blue);

		return (A, R, G, B);
	}

	public static ImageFile Save(string path, Matrix<double> matrix, Colours colour=Colours.Grayscale)
	{
		return new ImageFile(path, matrix, colour);
	}
}