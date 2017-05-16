using System.Collections.Generic;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    public class DownResizeCropTransformation : ResizeCropTransformation
    {
        public DownResizeCropTransformation(int width, int height, FocusPoint focusPoint) : base(width, height, focusPoint)
        {
        }

        public DownResizeCropTransformation(IDictionary<string, string> serializedProperties) : base(serializedProperties)
        {
        }

        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            if (image.Width <= context.Width && image.Height <= context.Height)
                return;
            base.ApplyToImage(context, image);
        }

        protected override IImageTransformation Scale(int width, int height)
        {
            return new DownResizeCropTransformation(width, height, FocusPoint);
        }
    }
}