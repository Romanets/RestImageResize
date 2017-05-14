using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using OpenWaves;
using OpenWaves.ImageTransformations;
using RestImageResize.Transformations;

namespace RestImageResize
{
    [DefaultImplementation(typeof(TransformationRegistry))] //? singleton
    public interface ITransformationRegistry
    {
        ITransformationRegistry Add([NotNull] string name, [NotNull] Type transformationType);

        Type TryGetByTransformationName([NotNull] string name);
    }

    public class TransformationRegistry : ITransformationRegistry
    {
        private readonly IDictionary<string, Type> registry = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        public virtual ITransformationRegistry Add([NotNull] string name, [NotNull] Type transformationType) 
        {
            if (transformationType == null) throw new ArgumentNullException(nameof(transformationType));
            if (name == null) throw new ArgumentNullException(nameof(name));
            registry[name] = transformationType;
            return this;
        }

        public virtual Type TryGetByTransformationName(string name)
        {
            return registry.TryGetValue(name);
        }
    }

    public static class TransformationRegistryExtensions
    {
        public static ITransformationRegistry Add<TTransformation>(this ITransformationRegistry registry,
            [NotNull] string name) where TTransformation : IImageTransformation
        {
            return registry.Add(name, typeof(TTransformation));
        }
        public static ITransformationRegistry Add<TTransformation>(this ITransformationRegistry registry,
            ImageTransform transform)
            where TTransformation : IImageTransformation
        {
            return registry.Add<TTransformation>(transform.ToString());
        }
    }

    /// <summary>
    /// Extends the <see cref="ImageTransformationParser"/> to provide parsing of any type of <see cref="IImageTransformation"/> instead predefined only.
    /// </summary>
    public class UniversalImageTransformationParser : IImageTransformationParser
    {
        public UniversalImageTransformationParser(ITransformationRegistry transformationRegistry)
        {
            TransformationRegistry = transformationRegistry;
        }

        /// <summary>
        /// 
        /// </summary>
        protected ITransformationRegistry TransformationRegistry { get; }

        /// <summary>
        /// Tries to parse transformation instance by name and properties string. 
        /// </summary>
        /// <param name="serializedProperties">The serialized transformation properties string.</param>
        /// <param name="transformation">The transformation instance.</param>
        /// <returns>
        /// <c>true</c> if transformation is successfully parsed, otherwise - <c>False</c>
        /// </returns>
        public virtual bool TryParse(string serializedProperties, out IImageTransformation transformation)
        {
            transformation = null;
            try
            {
                var propertiesDictionary = ImageTransformation2.PropertiesSerializer.TryDeserialize(serializedProperties);
                if (propertiesDictionary == null)
                    return false;

                var type = TransformationRegistry.TryGetByTransformationName(propertiesDictionary["transform"]);
                if (type != null && typeof(IImageTransformation).IsAssignableFrom(type))
                {
                    var ctor = type.GetConstructor(new[] { typeof(IDictionary<string, string>) });
                    if (ctor != null)
                    {
                        object instance = ctor.Invoke(new object[] { propertiesDictionary });
                        transformation = (IImageTransformation)instance;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public virtual IImageTransformation Parse([NotNull] string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            IImageTransformation transformation;
            if (!this.TryParse(value, out transformation) || transformation == null)
                throw new FormatException("Failed to parse image transformation from string: " + value);

            return transformation;
        }
    }
}
