using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Web.Routing;
using OpenWaves;
using OpenWaves.ImageTransformations;
using RestImageResize.Utils;

namespace RestImageResize.Transformations
{
    public class TransformationContext
    {
        public int TargetWidth { get; set; }
        public int TargetHeight { get; set; }
    }

    public abstract class ImageTransformation2 : IImageTransformation, IResponsiveImageTransformation
    {
        public static readonly IImageTransformation Null = new NullTransformation();

        public static bool TryParse(string value, out IImageTransformation transformation)
        {
            return ServiceLocator.Resolve<IImageTransformationParser>().TryParse(value, out transformation);
        }

        public static IImageTransformation Parse(string value)
        {
            return ServiceLocator.Resolve<IImageTransformationParser>().Parse(value);
        }

        public static IImageTransformationPropertiesSerializer PropertiesSerializer =>
            ServiceLocator.Resolve<IImageTransformationPropertiesSerializer>();

        public static ITransformationRegistry TransformationRegistry =>
            ServiceLocator.Resolve<ITransformationRegistry>();

        private readonly int width;
        protected int Width
        {
            get { return this.width; }
        }

        private readonly int height;
        protected int Height
        {
            get { return this.height; }
        }

        public virtual string Name
        {
            get
            {
                var name = ImageTransform?.ToString().ToLowerInvariant() ?? GetLegacyTransformName(this.GetType());
                return name;
            }
        }

        public virtual ImageTransform? ImageTransform => TransformationRegistry.TryGetTransformByType(this.GetType());

        public static string GetLegacyTransformName(Type type)
        {
            Contract.Assert(type.Name.EndsWith("Transformation", StringComparison.Ordinal));
            var s = type.Name;
            var name = s.Substring(0, s.Length - "Transformation".Length);
            return name;
        }

        protected ImageTransformation2(int width, int height)
        {
            Contract.Requires<ArgumentNullException>(width > 0);
            Contract.Requires<ArgumentNullException>(height > 0);

            this.width = width;
            this.height = height;
        }

        protected ImageTransformation2(string serializedProperties)
            : this(PropertiesSerializer.Deserialize(serializedProperties))
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrEmpty(serializedProperties) == false);
        }

        protected ImageTransformation2(IDictionary<string, string> properties)
        {
            this.width = SmartConvert.TryChangeType<int>(properties.TryGetValue(nameof(Width)) ?? "0");
            this.height = SmartConvert.TryChangeType<int>(properties.TryGetValue(nameof(Height)) ?? "0");
        }

        public virtual void ApplyToImage(IImage image)
        {
            var context = CreateContext();
            Applying(context, image);
            ApplyToImage(context, image);
            Applied(context, image);
        }

        /// <summary>
        /// A connection point to create "Applied" filter
        /// </summary>
        /// <param name="context"></param>
        /// <param name="image"></param>
        protected virtual void Applied(TransformationContext context, IImage image)
        {
        }

        /// <summary>
        /// A connection point to create "Applying" filter
        /// </summary>
        /// <param name="context"></param>
        /// <param name="image"></param>
        protected virtual void Applying(TransformationContext context, IImage image)
        {
            if (context.TargetWidth == 0)
            {
                context.TargetWidth = image.Width;
            }

            if (context.TargetHeight == 0)
            {
                context.TargetHeight = image.Height;
            }
        }

        protected virtual TransformationContext CreateContext()
        {
            var result = new TransformationContext
            {
                TargetWidth = width,
                TargetHeight = height
            };

            return result;
        }

        public abstract void ApplyToImage(TransformationContext context, IImage image);

        public virtual string Serialize()
        {
            var result = PropertiesSerializer.Serialize(this.ToPropertiesDictionary());
            return result;
            //return String.Format(CultureInfo.InvariantCulture, "{0}|{1}x{2}", name, this.Width, this.Height);
        }

        protected virtual IDictionary<string, string> ToPropertiesDictionary()
        {
            var result = new RouteValueDictionary(new
            {
                Transform = Name,
                Width,
                Height
            }).ToPropertiesDictionary();
            return result;
        }

        public override string ToString()
        {
            return this.Serialize();
        }

        public virtual bool Equals(ImageTransformation2 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.width == this.width && other.height == this.height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ImageTransformation)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.width * 397) ^ this.height;
            }
        }

        public virtual IImageTransformation Scale(double pixelRatio)
        {
            return this.Scale((int)(this.Width * pixelRatio), (int)(this.Height * pixelRatio));
        }

        protected abstract IImageTransformation Scale(int width, int height);

        private class NullTransformation : IImageTransformation
        {
            public void ApplyToImage(IImage image)
            {
            }

            public string Serialize()
            {
                return "Null";
            }
        }
    }
}