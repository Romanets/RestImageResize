using System;
using System.Collections.Generic;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    public class ScaleToFillTransformation : ImageTransformation2
    {
        public ScaleToFillTransformation(int width, int height) : base(width, height)
        {
        }

        public ScaleToFillTransformation(IDictionary<string, string> properties) : base(properties)
        {
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            int width;
            int height;
            if ((double)image.Width / image.Height < (double)context.TargetWidth / context.TargetHeight)
            {
                width = context.TargetWidth;
                height = (int)(Math.Round((double)context.TargetWidth / image.Width * image.Height));
            }
            else
            {
                width = (int)(Math.Round((double)context.TargetHeight / image.Height * image.Width));
                height = context.TargetHeight;
            }

            image.Scale(width, height);

            if (width > context.TargetWidth || height > context.TargetHeight)
                image.Crop((int)((width - context.TargetWidth) / 2.0), (int)((height - context.TargetHeight) / 2.0), context.TargetWidth, context.TargetHeight);
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new ScaleToFillTransformation(width, height);
        }
    }
}