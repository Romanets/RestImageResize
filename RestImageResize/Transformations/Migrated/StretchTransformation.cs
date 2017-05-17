using System.Collections.Generic;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    public class StretchTransformation : ImageTransformation2
    {
        public StretchTransformation(int width, int height) : base(width, height)
        {
        }

        public StretchTransformation(IDictionary<string, string> properties) : base(properties)
        {
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            image.Scale(context.TargetWidth, context.TargetHeight);
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new StretchTransformation(width, height);
        }
    }
}