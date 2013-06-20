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
    }
}