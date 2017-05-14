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
            if (context.Width == 0)
            {
                context.Width = int.MaxValue;
            }

            if (context.Height == 0)
            {
                context.Height = int.MaxValue;
            }
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            int width;
            int height;
            if ((double)image.Width / image.Height < (double)context.Width / context.Height)
            {
                width = (int)(Math.Round((double)context.Height / image.Height * image.Width));
                height = context.Height;
            }
            else
            {
                width = context.Width;
                height = (int)(Math.Round((double)context.Width / image.Width * image.Height));
            }

            image.Scale(width, height);
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new ScaleToFitTransformation(width, height);
        }
    }
}