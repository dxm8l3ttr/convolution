using MathNet.Numerics.LinearAlgebra;
using Convolution;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Convolution;


public class ImageFile
{
	public Image<Rgba32> Image { get; private init; }
	private MatrixBuilder<double> _builder;

	public ImageFile(string path)
	{
		_builder = Matrix<double>.Build;
		Image =  SixLabors.ImageSharp.Image.Load<Rgba32>(path);
	}

	public ImageFile(string path, Matrix<double> matrix, Colours colour=Colours.Grayscale)
	{
		_builder = Matrix<double>.Build;
		Image = matrix.ToImage(colour);
		Image.Save(path);
	}

	public ImageFile(string path, Matrix<double> A, Matrix<double> R, Matrix<double> G, Matrix<double> B)
	{
		_builder = Matrix<double>.Build;
		Image = FromMatrices(A, R, G, B);
		Image.Save(path);
	}

	private Image<Rgba32> FromMatrices(Matrix<double> A, Matrix<double> R, Matrix<double> G, Matrix<double> B)
	{
		var image = new Image<Rgba32>(A.RowCount, A.ColumnCount);
		var (normA, normR, normG, normB) = 
		(
			A.Normalize(), 
			R.Normalize(), 
			G.Normalize(), 
			B.Normalize()
		);
		for(int i = 0; i < normA.RowCount; i++)
		{
			for(int j = 0; j < normA.ColumnCount; j++)
			{
				var a = (float)normA[i, j];
				var r = (float)normR[i, j];
				var g = (float)normG[i, j];
				var b = (float)normB[i, j];
				Image[i, j] = new Rgba32(r, g, b, a);
			}
		}
		return Image;
	}

	public ImageFile(string path, Matrix<double> R, Matrix<double> G, Matrix<double> B)
	{
		_builder = Matrix<double>.Build;
		Image = FromMatrices(_builder.Dense(R.RowCount, R.ColumnCount, 1), R, G, B);
		Image.Save(path);
	}

	public Matrix<double> ToMatrix()
	{
		var matrix = _builder
			.Dense(Image.Width, Image.Height, (i, j) => Image
				[i, j]
				.ToGrayScale());
		return matrix;
	}

	public Matrix<double> ToMatrix(Colours colour)
	{
		var matrix = _builder.Dense(Image.Width, Image.Height);
		switch (colour)
		{
			case Colours.Blue:
				matrix = _builder
					.Dense(Image.Width, Image.Height, (i, j) => Image
						[i, j].B);
				break;
			case Colours.Green:
				matrix = _builder
					.Dense(Image.Width, Image.Height, (i, j) => Image
						[i, j].G);
				break;
			case Colours.Red:
				matrix = _builder
					.Dense(Image.Width, Image.Height, (i, j) => Image
						[i, j].R);
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
					.Dense(Image.Width, Image.Height, (i, j) => Image[i, j].A);
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