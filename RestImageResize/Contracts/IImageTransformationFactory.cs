using OpenWaves;
using OpenWaves.ImageTransformations;
using RestImageResize.Transformations;

namespace RestImageResize.Contracts
{
    /// <summary>
    /// Defines an interface of factory that is used to create instances of the <see cref="IImageTransformation"/>.
    /// </summary>
    [DefaultImplementation(typeof(ImageTransformationFactory))]
    public interface IImageTransformationFactory
    {
        IImageTransformation TryCreate(ImageTransformQuery transformQuery);
    }

    public class ImageTransformationFactory : IImageTransformationFactory
    {
        public virtual IImageTransformation TryCreate(ImageTransformQuery transformQuery)
        {
            var serialized = transformQuery.ToPropertiesString();
            IImageTransformation transformation;
            var result = ImageTransformation2.TryParse(serialized, out transformation) ? transformation : null;
            return result;
        }
    }
}
