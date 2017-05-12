using OpenWaves.ImageTransformations;

namespace RestImageResize.Utils
{
    public class FocusPointUtil
    {
        public static Coordinates GetCropPoint(Coordinates focusPoint, CropArea cropArea, IImage image)
        {
            var cropCenter = GetCropAreaCenter(cropArea);
            int deltaX = focusPoint.X - cropCenter.X;
            int deltaY = focusPoint.Y - cropCenter.Y;
            int newX = cropArea.CropPoint.X + deltaX;
            int newY = cropArea.CropPoint.Y + deltaY;

            if (newX < 0) newX = 0;
            if (newX + cropArea.Width > image.Width) newX = image.Width - cropArea.Width;

            if (newY < 0) newY = 0;
            if (newY + cropArea.Height > image.Height) newY = image.Height - cropArea.Height;

            return new Coordinates()
            {
                X = newX,
                Y = newY
            };
        }

        public static Coordinates GetCropAreaCenter(CropArea cropArea)
        {
            return new Coordinates()
            {
                X = cropArea.CropPoint.X + (cropArea.Width / 2),
                Y = cropArea.CropPoint.Y + (cropArea.Height / 2)
            };
        }

        public static Coordinates GetFocusPointCoordinates(IImage image, double left, double top)
        {
            return new Coordinates()
            {
                X = (int)(image.Width * left),
                Y = (int)(image.Height * top)
            };
        }
    }
}