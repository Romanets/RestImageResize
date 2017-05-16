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

        string TryGetTransformationName([NotNull] Type type);
    }

    public class TransformationRegistry : ITransformationRegistry
    {
        private readonly IDictionary<string, Type> registry = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        private readonly IDictionary<string, string> lookup = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public virtual ITransformationRegistry Add([NotNull] string name, [NotNull] Type transformationType) 
        {
            if (transformationType == null) throw new ArgumentNullException(nameof(transformationType));
            if (name == null) throw new ArgumentNullException(nameof(name));
            registry[name] = transformationType;
            lookup[transformationType.AssemblyQualifiedName] = name;
            return this;
        }

        public virtual Type TryGetByTransformationName(string name)
        {
            return registry.TryGetValue(name);
        }

        public virtual string TryGetTransformationName([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return lookup.TryGetValue(type.AssemblyQualifiedName);
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

        public static ImageTransform? TryGetTransformByType(this ITransformationRegistry registry, Type type)
        {
            var name = registry.TryGetTransformationName(type);
            if (name == null)
                return null;
            ImageTransform result;
            return ImageTransform.TryParse(name, true, out result) ? (ImageTransform?) result : null;
        }
    }

    /// <summary>
    /// Extends the <see cref="ImageTransformationParser"/> to provide parsing of any type of <see cref="IImageTransformation"/> instead predefined only.
    /// </summary>
    public class UniversalImageTransformationParser : IImageTransformationParser
    {
        public UniversalImageTransformationParser(ITransformationRegistry transformationRegistry, ILoggingService logger)
        {
            TransformationRegistry = transformationRegistry;
            Logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        protected ITransformationRegistry TransformationRegistry { get; }
        protected ILoggingService Logger { get; }

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
                {
                    Logger.Log(LogEntrySeverity.Warning, $"Nothing to parse: '{serializedProperties}'");
                    return false;
                }

                var type = TransformationRegistry.TryGetByTransformationName(propertiesDictionary["transform"]);
                if (type != null && typeof (IImageTransformation).IsAssignableFrom(type))
                {
                    var ctor = type.GetConstructor(new[] {typeof (IDictionary<string, string>)});
                    if (ctor != null)
                    {
                        object instance = ctor.Invoke(new object[] {propertiesDictionary});
                        transformation = (IImageTransformation) instance;
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(LogEntrySeverity.Warning, $"Error parsing string '{serializedProperties}': {ex}");
                return false;
            }

            Logger.Log(LogEntrySeverity.Warning, $"Properties parsed, but transform is not created: '{serializedProperties}'");
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
