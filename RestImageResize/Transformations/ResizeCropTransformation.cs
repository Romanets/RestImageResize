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
            if ((double)image.Width / (double)image.Height < (double)context.TargetWidth / (double)context.TargetHeight)
            {
                width = context.TargetWidth;
                height = (int)Math.Round((double)context.TargetWidth / (double)image.Width * (double)image.Height);
            }
            else
            {
                width = (int)Math.Round((double)context.TargetHeight / (double)image.Height * (double)image.Width);
                height = context.TargetHeight;
            }
            image.Scale(width, height);
            if (width <= context.TargetWidth && height <= context.TargetHeight)
                return;

            var cropArea = CreateCropArea(context, image, width, height);

            image.Crop(cropArea.CropPoint.X, cropArea.CropPoint.Y, cropArea.Width, cropArea.Height);
        }

        private CropArea CreateCropArea(TransformationContext context, IImage image, int width, int height)
        {
            var cropArea = new CropArea()
            {
                CropPoint = new Coordinates()
                {
                    X = (int)((double)(width - context.TargetWidth) / 2.0),
                    Y = (int)((double)(height - context.TargetHeight) / 2.0)
                },
                Width = context.TargetWidth,
                Height = context.TargetHeight
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