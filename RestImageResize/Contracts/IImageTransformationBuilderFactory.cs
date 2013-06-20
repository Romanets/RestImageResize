using OpenWaves;

namespace RestImageResize.Contracts
{
    /// <summary>
    /// Defines an interface of factory that is used to create instances of <see cref="IImageTransformationBuilder"/> component.
    /// </summary>
    [DefaultImplementation(typeof(ImageTransformationBuilderFactory))]
    public interface IImageTransformationBuilderFactory
    {
        /// <summary>
        /// Creates the builder instance.
        /// </summary>
        /// <returns>
        /// The builder instance.
        /// </returns>
        IImageTransformationBuilder CreateBuilder();
    }
}
