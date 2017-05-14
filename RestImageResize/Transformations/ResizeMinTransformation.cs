using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using OpenWaves.ImageTransformations;

namespace RestImageResize.Transformations
{
    /// <summary>
    /// Does the minimal resize needed to meet either target width or height. 
    /// It doesn't change image aspect ratio and doesn't crop the original image. 
    /// (Similar to ResizeMin from http://imageprocessor.org/imageprocessor-web/imageprocessingmodule/resize/)
    /// Example: 
    /// original image 600px x 300px => ResizeMin to 100px x 150px
    ///     returns image 300px x 150px
    /// original image 300px x 600px => ResizeMin to 100px x 150px
    ///     returns image 100px x 200px
    /// </summary>
    public class ResizeMinTransformation : ImageTransformation2
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ResizeMinTransformation(int width, int height)
            : base(width, height)
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedProperties"></param>
        public ResizeMinTransformation(IDictionary<string, string> serializedProperties)
            : base(serializedProperties)
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        public override void ApplyToImage(TransformationContext context, [NotNull] IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            if (image.Width == 0 || image.Height == 0)
                return;
            
            var scaleX = (double) image.Width / (double) context.Width;
            var scaleY = (double) image.Height / (double) context.Height;

            if (scaleX <= scaleY)
            {
                var newHeight = (int) Math.Round((double) image.Height / scaleX);
                image.Scale(context.Width, newHeight);
            }
            else
            {
                var newWidth = (int) Math.Round((double) image.Width / scaleY);
                image.Scale(newWidth, context.Height);
            }
        }

        protected override void Applying(TransformationContext context, IImage image)
        {
            base.Applying(context, image); 

            if (context.Width == 0)
            {
                context.Width = Height;
            }

            if (context.Height == 0)
            {
                context.Height = Width;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected override IImageTransformation Scale(int width, int height)
        {
            return new ResizeMinTransformation(width, height);
        }
    }

    /// <summary>
    /// Same as ResizeMinTransformation, but does nothing if the target image is bigger than the original
    /// </summary>
    public class DownResizeMinTransformation : ResizeMinTransformation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public DownResizeMinTransformation(int width, int height) : base(width, height)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedProperties"></param>
        public DownResizeMinTransformation(IDictionary<string, string> serializedProperties)
            : base(serializedProperties)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        public override void ApplyToImage(TransformationContext context, IImage image)
        {
            if (image.Width <= context.Width || image.Height <= context.Height)
                return;

            base.ApplyToImage(image);
        }
    }
}