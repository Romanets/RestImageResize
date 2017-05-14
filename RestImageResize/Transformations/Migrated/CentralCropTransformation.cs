using System;
using System.Collections.Generic;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    public class CentralCropTransformation : ImageTransformation2
    {
        public CentralCropTransformation(int width, int height) : base(width, height)
        {
        }

        public CentralCropTransformation(IDictionary<string, string> properties) : base(properties)
        {
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            if (image.Width <= context.Width && image.Height <= context.Height)
                return;

            image.Crop(
                Math.Max(0, image.Width - context.Width) / 2,
                Math.Max(0, image.Height - context.Height) / 2,
                Math.Min(image.Width, context.Width),
                Math.Min(image.Height, context.Height));
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new CentralCropTransformation(width, height);
        }
    }
}
