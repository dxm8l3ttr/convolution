# Convolution
Version 2.0.0. 
# Instalation
Run ```dotnet add package Convolution --version 2.0.0```
# Documentation
## Contents
- [Types](#types)
    - [ImageFile](#imagefile)
    - [ConvolverFactory](#convolverfactory)
    - [IConvolver](#iconvolver)
    - [VectorConvolver](#vectorconvolver)
    - [OverlapAddVectorConvolver](#overlapaddvectorconvolver)
    - [MatrixConvolver](#matrixconvolver)
    - [OverlapAddMatrixConvolver](#overlapaddmatrixconvolver)
- [Extensions](#extensions)
    - [Colours](#colours)
    - [GrayscaleType](#grayscaletype)
    - [Extensions](#extensions-class)

## Types
### ImageFile
The ```ImageFile``` class is used to store and process image files as matrices.
#### Properties
- ```Image<Rgba32> Image```
#### Constructors
- ```ImageFile(string path)```
Creates an ```Image<Rgba32>``` from the image in the specified location.
- ```ImageFile(string path, Matrix<double> matrix, Colours colour=Colours.Grayscale)```
Creates an ```Image<Rgba32>``` from the specified matrix and saves it to the specified path.
- ```ImageFile(string path, Matrix<double> A, Matrix<double> R, Matrix<double> G, Matrix<double> B)```
Creates an ARGB bitmap using the specified colour matrices and saves it to the specified path.
- ```ImageFile(string path, Matrix<double> R, Matrix<double> G, Matrix<double> B)```
Creates an ```Image<Rgba32>``` using the specified colour matrices, with A being set to 255 for all pixels, and saves it to the specified path.
#### Static Methods
- ```ImageFile Save(string path, Matrix<double> matrix, Colours colour=Colours.Grayscale)```
Returns an monochrome ImageFile of the specified colour saved to the specified path.
#### Methods
- ```Matrix<double> ToMatrix()```
Returns a grayscale matrix
- ```Matrix<double> ToMatrix(Colours colour)```
Returns a monochrome matrix of the specified colour.
- ```(Matrix<double>, Matrix<double>, Matrix<double>, Matrix<double>) ToMatrices()```
Returns a matrix for all four ARGB channels.
### ConvolverFactory
#### Methods
- ```IConvolver<T> Create(T X, T, H)```
Returns an IConvolver with the image X and kernel H. T can be ```Vector<double>```, ```Vector<Complex>```, ```Matrix<double>``` and ```Matrix<Complex>```.
```double[]```, ```double[,]```, ```Complex[]``` and ```Complex[,]``` will also be accepted but the resulting IConvolver will be using one of the types mentioned above. 
### IConvolver
#### Properties
- ```T X```
The image.
- ```T H```
The kernel.
#### Methods
- ```void Convolve()```
Performs the convolution of X and H. The result is stored in X. 
### VectorConvolver
Implements the ```IConvolver<Vector<Complex>>``` interface. The ```Convolve()``` method is abstract.
#### Constructor
- ```VectorConvolver(Vector<Complex> image, Vector<Complex> kernel)```
#### Methods
- ```abstract void Convolve()```
### OverlapAddVectorConvolver
Implements the ```VectorConvolver``` class
#### Constructor
- ```OverlapAddVectorConvolver(Vector<Complex> image, Vector<Complex> kernel)```
#### Methods
- ```void Convolve()```
### MatrixConvolver
Implements the ```IConvolver<Matrix<Complex>>``` interface. The ```Convolve()``` method is abstract.
#### Constructor
- ```MatrixConvolver(Matrix<Complex> image, Matrix<Complex> kernel)```
#### Methods
- ```abstract void Convolve()```
### OverlapAddMatrixConvolver
Implements the ```MatrixConvolver``` class
#### Constructor
- ```OverlapAddMatrixConvolver(Matrix<Complex> image, Matrix<Complex> kernel)```
#### Methods
- ```void Convolve()```
## Extensions
### Colours
Enum for describing the colour channels of an image.
#### Values
- ```Green```
- ```Blue```
- ```Red```
- ```Grayscale```
### GrayscaleType
Enum describing the two grayscale algorithms.
#### Values
- ```Average```
- ```Weighted```
### Extensions Class
#### Methods
- ```int ToGrayScale(this Rgba32 color, GrayscaleType type = GrayscaleType.Weighted)```
Converts Rgba32 object to integer grayscale value following the specified grayscale algorithm.
- ```double Sum(this Matrix<double> matrix)```
Returns the sum of all the elements of the matrix.
- ```Image<Rgba32> ToImage(this Matrix<double> matrix, Colours colour = Colours.Grayscale)```
Returns a monochrome Image of the specified colour.
- ```Matrix<double> ToReal(this Matrix<Complex> m)```
Returns the real component matrix of the complex matrix.
- ```Vector<double> ToReal(Vector<Complex> v)```
Returns the real component vector of the complex vector.
- ```Matrix<double> Normalize(this Matrix<double> m, int peak=1)```
Returns the matrix normalized to fit in 0 to peak range. 
- ```Vector<double> Normalize(this Vector<double> v, int peak=1)```
Returns the vector normalize to fit in 0 to peak range.
