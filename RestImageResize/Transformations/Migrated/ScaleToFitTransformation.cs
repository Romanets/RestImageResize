using System;
using System.Collections.Generic;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    public class ScaleToFitTransformation : ImageTransformation2
    {
        public ScaleToFitTransformation(int width, int height) : base(width, height)
        {
        }

        public ScaleToFitTransformation(IDictionary<string, string> properties) : base(properties)
        {
        }

        protected override void Applying(TransformationContext context, IImage image)
        {
            if (context.TargetWidth == 0)
            {
                context.TargetWidth = int.MaxValue;
            }

            if (context.TargetHeight == 0)
            {
                context.TargetHeight = int.MaxValue;
            }
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            int width;
            int height;
            if ((double)image.Width / image.Height < (double)context.TargetWidth / context.TargetHeight)
            {
                width = (int)(Math.Round((double)context.TargetHeight / image.Height * image.Width));
                height = context.TargetHeight;
            }
            else
            {
                width = context.TargetWidth;
                height = (int)(Math.Round((double)context.TargetWidth / image.Width * image.Height));
            }

            image.Scale(width, height);
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new ScaleToFitTransformation(width, height);
        }
    }
}