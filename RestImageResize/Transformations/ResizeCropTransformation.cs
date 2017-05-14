using System;
using System.Collections.Generic;
using System.Web.Routing;
using OpenWaves.ImageTransformations;
using RestImageResize.Utils;

namespace RestImageResize.Transformations
{
    /// <summary>
    /// Does the similar to resizecrop in http://imageprocessor.org/imageprocessor-web/imageprocessingmodule/resize/ 
    /// Resizes the image to the given dimensions. 
    /// If the set dimensions do not match the aspect ratio of the original image then the output is cropped to match the new aspect ratio.
    /// Ensures that the focus point is visible after the crop. 
    /// </summary>
    public class ResizeCropTransformation : ImageTransformation2
    {
        public ResizeCropTransformation(int width, int height, FocusPoint focusPoint) : base(width, height)
        {
            this.FocusPoint = focusPoint;
        }

        public ResizeCropTransformation(IDictionary<string, string> serializedProperties) 
            : base(serializedProperties)
        {
            FocusPoint = FocusPoint.TryParse(serializedProperties["center"]);
        }

        protected FocusPoint FocusPoint { get; }

        protected override IDictionary<string, string> ToPropertiesDictionary()
        {
            var result = new RouteValueDictionary(new
            {
                Transform = Name,
                Center = FocusPoint.ToString(),
                Width,
                Height
            }).ToPropertiesDictionary();
            return result;
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            int width;
            int height;
            if ((double)image.Width / (double)image.Height < (double)context.Width / (double)context.Height)
            {
                width = context.Width;
                height = (int)Math.Round((double)context.Width / (double)image.Width * (double)image.Height);
            }
            else
            {
                width = (int)Math.Round((double)context.Height / (double)image.Height * (double)image.Width);
                height = context.Height;
            }
            image.Scale(width, height);
            if (width <= context.Width && height <= context.Height)
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

            if (FocusPoint != null)
            {
                Coordinates focusPoint = FocusPointUtil.GetFocusPointCoordinates(image, FocusPoint.Left, FocusPoint.Top);
                cropArea.CropPoint = FocusPointUtil.GetCropPoint(focusPoint, cropArea, image);
            }

            return cropArea;
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new ResizeCropTransformation(width, height, FocusPoint);
        }
    }
}