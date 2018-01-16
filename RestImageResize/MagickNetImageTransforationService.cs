using ImageMagick;
using OpenWaves.ImageTransformations;
using System.IO;

namespace RestImageResize
{
    public class MagickNetImageTransforationService : IImageTransformationService
    {
        private class MagickImageWrapper : IImage
        {
            private readonly MagickImage _magickImage;

            public MagickImageWrapper(MagickImage magickImage)
            {
                _magickImage = magickImage;
            }

            public int Width => _magickImage.Width;

            public int Height => _magickImage.Height;

            public void Scale(int width, int height)
            {
                _magickImage.Scale(width, height);
            }

            public void Crop(int left, int top, int width, int height)
            {
                _magickImage.Crop(left, top, width, height);
            }
        }

        public void TransformImage(Stream input, Stream output, ImageFormat outputFormat, IImageTransformation transformation)
        {
            using (MagickImage magickImage = new MagickImage(input))
            {
                var image = new MagickImageWrapper(magickImage);
                transformation.ApplyToImage(image);
                magickImage.Write(output);
            }
        }
    }
}
