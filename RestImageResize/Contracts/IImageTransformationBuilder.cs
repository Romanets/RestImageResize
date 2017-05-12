using OpenWaves.ImageTransformations;

namespace RestImageResize.Contracts
{
    /// <summary>
    /// Represents the image transformation builder component.
    /// </summary>
    public interface IImageTransformationBuilder : IImageTransformation
    {
        /// <summary>
        /// Gets or sets the desired image width.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the desired image height.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the type of the transformation that should be applied in image.
        /// </summary>
        ImageTransform TransformType { get; set; }

        /// <summary>
        /// Gets or sets the image focus point.
        /// This point will be as close to the center of your crop as possible while keeping the crop within the image
        /// </summary>
        FocusPoint FocusPoint { get; set; }
    }
}