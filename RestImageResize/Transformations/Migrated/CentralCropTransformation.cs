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
            if (image.Width <= context.TargetWidth && image.Height <= context.TargetHeight)
                return;

            image.Crop(
                Math.Max(0, image.Width - context.TargetWidth) / 2,
                Math.Max(0, image.Height - context.TargetHeight) / 2,
                Math.Min(image.Width, context.TargetWidth),
                Math.Min(image.Height, context.TargetHeight));
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new CentralCropTransformation(width, height);
        }
    }
}
