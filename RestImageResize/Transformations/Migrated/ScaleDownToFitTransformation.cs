using System.Collections.Generic;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    public class ScaleDownToFitTransformation : ScaleToFitTransformation
    {
        public ScaleDownToFitTransformation(int width, int height) : base(width, height)
        {
        }

        public ScaleDownToFitTransformation(IDictionary<string, string> properties) : base(properties)
        {
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            if (image.Width <= context.TargetWidth && image.Height <= context.TargetHeight)
                return;

            base.ApplyToImage(context, image);
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new ScaleDownToFitTransformation(width, height);
        }
    }
}