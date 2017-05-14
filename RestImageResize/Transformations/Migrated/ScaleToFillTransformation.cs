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
            if ((double)image.Width / image.Height < (double)context.Width / context.Height)
            {
                width = context.Width;
                height = (int)(Math.Round((double)context.Width / image.Width * image.Height));
            }
            else
            {
                width = (int)(Math.Round((double)context.Height / image.Height * image.Width));
                height = context.Height;
            }

            image.Scale(width, height);

            if (width > context.Width || height > context.Height)
                image.Crop((int)((width - context.Width) / 2.0), (int)((height - context.Height) / 2.0), context.Width, context.Height);
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new ScaleToFillTransformation(width, height);
        }
    }
}