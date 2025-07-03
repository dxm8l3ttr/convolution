# Convolution
## Contents
- [Types](#types)
   - [ImageFile](#imagefile)
   - [Convolution](#convolution-class)
- [Extensions](#extensions)
   - [Colours](#colours)
   - [GrayscaleType](#grayscaletype)
   - [Extensions](#extensions-class)

## Types
### ImageFile
The ImageFile class is used to store and process image files as matrices.
#### Properties
- Bitmap Map
Bitmap of the image
#### Constructors
-```ImageFile(string path)```
Creates a bitmap from the image in the specified location.
- ```ImageFile(string path, Matrix<double> matrix, Colours colour=Colours.Grayscale)```
Creates a monochrome bitmap from the specified matrix and saves it to the specified path.
- ```ImageFile(string path, Matrix<double> A, Matrix<double> R, Matrix<double> G, Matrix<double> B)```
Creates an ARGB bitmap using the specified colour matrices and saves it to the specified path.
- ```ImageFile(string path, Matrix<double> R, Matrix<double> G, Matrix<double> B)```
Creates an ARGB bitmap using the specified colour matrices, with A being set to 255 for all pixels, and saves it to the specified path.
#### Static Methods
- ```ImageFile Save(string path, Matrix<double> matrix, Colours colour=Colours.Grayscale)```
Returns a monochrome ImageFile of the specified colour saved to the specified path.
#### Methods
- ```Matrix<double> ToMatrix()```
Returns a grayscale matrix
- ```Matrix<double> ToMatrix(Colours colour)```
Returns a monochrome matrix of the specified colour.
- ```(Matrix<double>, Matrix<double>, Matrix<double>, Matrix<double>) ToMatrices()```
Returns a matrix for all four ARGB channels.
### Convolution Class
Static class used for performing convolution.
#### Methods
- ```Matrix<double> OverlapAdd(Matrix<Complex> image, Matrix<Complex> kernel, int blockSize=0, int overlapSize=0)```
Returns the convolution of the complex matrices image and kernel using the overlap-add algorithm.
- ```Matrix<double> OverlapAdd(Matrix<double> image, Matrix<double> kernel, int blockSize=0, int overlapSize=0)```
Returns the convolution of the real matrices image and kernel using the overlap-add algorithm.
- ```OverlapAddNormalized(Matrix<Complex> image, Matrix<Complex> kernel, int peak, int blockSize=0, int overlapSize=0)```
Returns the convolution of the complex matrices image and kernel using the overlap-add algorithm normalized to the peak.
- ```Matrix<double> OverlapAddNormalized(Matrix<double> image, Matrix<double> kernel, int peak, int blockSize=0, int overlapSize=0)```
Returns the convolution of the real matrices image and kernel using the overlap-add algorithm normalized to the peak.
## Extensions
### Colours
Enum for describing the colour channels of an image.
### GrayscaleType
Enum describing the two grayscale algorithms.
### Extensions Class
#### Methods
- ```int ToGrayScale(this Color color, GrayscaleType type = GrayscaleType.Weighted)```
Converts Color object to integer grayscale following the specified grayscale algorithm.
- ```double Sum(this Matrix<double> matrix)```
Returns the sum of all the elements of the matrix.
- ```Bitmap ToBitmap(this Matrix<double> matrix, Colours colour = Colours.Grayscale)```
Returns a monochrome Bitmap of the specified colour.
- ```Matrix<double> ToReal(this Matrix<Complex> m)```
Returns the real component matrix of the complex matrix.
- ```Matrix<double> Normalize(this Matrix<double> m, int peak=1)```
Returns the matrix normalized to the 0 to peak range. 
