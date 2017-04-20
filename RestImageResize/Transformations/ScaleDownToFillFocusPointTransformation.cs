using System;
using OpenWaves.ImageTransformations;
using RestImageResize.Utils;

namespace RestImageResize.Transformations
{
    public class ScaleDownToFillFocusPointTransformation : ScaleDownToFillTransformation
    {
        protected FocusPoint FocusPoint { get; set; }

        public ScaleDownToFillFocusPointTransformation(FocusPoint focusPoint, int width, int height) : base(width, height)
        {
            this.FocusPoint = focusPoint;
        }

        public ScaleDownToFillFocusPointTransformation(string serializedProperties) : base(serializedProperties)
        {
        }

        public override void ApplyToImage(IImage image)
        {
            if (FocusPoint.Left < 0 || FocusPoint.Top < 0)
            {
                base.ApplyToImage(image);
                return;
            }

            CropIfNeed(image);
            if (image.Width <= this.Width && image.Height <= this.Height)
                return;
            image.Scale(this.Width, this.Height);
        }

        private void CropIfNeed(IImage image)
        {
            double val1 = (double) image.Width / (double) this.Width;
            double val2 = (double) image.Height / (double) this.Height;
            if (Math.Abs(val1 - val2) > 0.0)
            {
                var cropArea = CreateCropArea(image, val1, val2);

                image.Crop(cropArea.CropPoint.X, cropArea.CropPoint.Y, cropArea.Width, cropArea.Height);
            }
        }

        private CropArea CreateCropArea(IImage image, double val1, double val2)
        {
            //Calculation from original ScaleDownToFillTransformation
            double num1 = Math.Min(val1, val2);
            double num2 = num1 * (double)this.Width;
            double num3 = num1 * (double)this.Height;
            double num4 = ((double)image.Width - num2) / 2.0;
            double num5 = ((double)image.Height - num3) / 2.0;

            var cropArea = new CropArea()
            {
                CropPoint = new Coordinates()
                {
                    X = Convert.ToInt32(num4),
                    Y = Convert.ToInt32(num5)
                },
                Width = Convert.ToInt32(num2),
                Height = Convert.ToInt32(num3)
            };
            
            Coordinates focusPoint = FocusPointUtil.GetFocusPointCoordinates(image, FocusPoint.Left, FocusPoint.Top);

            cropArea.CropPoint = FocusPointUtil.GetCropPoint(focusPoint, cropArea, image);
            return cropArea;
        }
    }
}