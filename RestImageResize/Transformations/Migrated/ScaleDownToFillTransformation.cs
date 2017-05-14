using System;
using System.Collections.Generic;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    public class ScaleDownToFillTransformation : ImageTransformation2
    {
        public ScaleDownToFillTransformation(int width, int height) : base(width, height)
        {
        }

        public ScaleDownToFillTransformation(IDictionary<string, string> properties) : base(properties)
        {
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            var widthRatio = (double)image.Width / context.Width;
            var heightRatio = (double)image.Height / context.Height;

            if (Math.Abs(widthRatio - heightRatio) > 0)
            {
                var ratio = Math.Min(widthRatio, heightRatio);

                var desiredWidth = ratio * context.Width;
                var desiredHeight = ratio * context.Height;

                var left = (image.Width - desiredWidth) / 2;
                var top = (image.Height - desiredHeight) / 2;

                image.Crop(Convert.ToInt32(left), Convert.ToInt32(top), Convert.ToInt32(desiredWidth),
                    Convert.ToInt32(desiredHeight));
            }

            if (image.Width > context.Width || image.Height > context.Height)
            {
                image.Scale(context.Width, context.Height);
            }
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new ScaleDownToFillTransformation(width, height);
        }
    }
}