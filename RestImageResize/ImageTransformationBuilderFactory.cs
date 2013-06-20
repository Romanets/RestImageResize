using RestImageResize.Contracts;

namespace RestImageResize
{
    /// <summary>
    /// The default implementation of <see cref="IImageTransformationBuilderFactory"/>.
    /// </summary>
    public class ImageTransformationBuilderFactory: IImageTransformationBuilderFactory
    {
        /// <summary>
        /// Creates the builder instance.
        /// </summary>
        /// <returns>
        /// The builder instance.
        /// </returns>
        public IImageTransformationBuilder CreateBuilder()
        {
            return new ImageTransformationBuilder();
        }
    }
}