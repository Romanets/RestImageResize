using System;
using OpenWaves.ImageTransformations;
using RestImageResize.Utils;

namespace RestImageResize.Transformations
{
    public class ScaleToFillFocusPointTransformation : ScaleToFillTransformation
    {
        protected FocusPoint FocusPoint { get; set; }

        public ScaleToFillFocusPointTransformation(FocusPoint focusPoint, int width, int height) : base(width, height)
        {
            this.FocusPoint = focusPoint;
        }

        public ScaleToFillFocusPointTransformation(string serializedProperties) : base(serializedProperties)
        {
        }

        public override void ApplyToImage(IImage image)
        {
            if (FocusPoint.Left < 0 || FocusPoint.Top < 0)
            {
                base.ApplyToImage(image);
                return;
            }

            //Calculation from original ScaleToFillTransformation
            int width;
            int height;
            if ((double)image.Width / (double)image.Height < (double)this.Width / (double)this.Height)
            {
                width = this.Width;
                height = (int)Math.Round((double)this.Width / (double)image.Width * (double)image.Height);
            }
            else
            {
                width = (int)Math.Round((double)this.Height / (double)image.Height * (double)image.Width);
                height = this.Height;
            }
            image.Scale(width, height);
            if (width <= this.Width && height <= this.Height)
                return;

            var cropArea = CreateCropArea(image, width, height);

            image.Crop(cropArea.CropPoint.X, cropArea.CropPoint.Y, cropArea.Width, cropArea.Height);
        }

        private CropArea CreateCropArea(IImage image, int width, int height)
        {
            var cropArea = new CropArea()
            {
                CropPoint = new Coordinates()
                {
                    X = (int)((double)(width - this.Width) / 2.0),
                    Y = (int)((double)(height - this.Height) / 2.0)
                },
                Width = this.Width,
                Height = this.Height
            };
            
            Coordinates focusPoint = FocusPointUtil.GetFocusPointCoordinates(image, FocusPoint.Left, FocusPoint.Top);
            cropArea.CropPoint = FocusPointUtil.GetCropPoint(focusPoint, cropArea, image);
            return cropArea;
        }
    }
}