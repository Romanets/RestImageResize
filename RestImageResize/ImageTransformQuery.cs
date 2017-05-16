using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using OpenWaves;
using RestImageResize.Transformations;
using RestImageResize.Utils;

namespace RestImageResize
{
    /// <summary>
    /// Represents the image transformation query.
    /// </summary>
    public class ImageTransformQuery
    {
        /// <summary>
        /// Gets or sets the desired width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the desired height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the transformation parameters
        /// </summary>
        public NameValueCollection Parameters { get; set; } = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets or sets the type of transformation that should be applied to an image.
        /// </summary>
        public ImageTransform Transform { get; set; }

        /// <summary>
        /// Gets or sets the hash of the query.
        /// </summary>
        public string Hash { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToPropertiesString();
        }

        public virtual IDictionary<string, string> ToPropertiesDictionary()
        {
            var result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            result.Add(nameof(Transform).ToLowerInvariant(), Transform.ToString().ToLowerInvariant());

            if (!Hash.IsNullOrWhiteSpace())
                result.Add("h", Hash);
            if (Width > 0)
                result.Add(nameof(Width).ToLowerInvariant(), Width.ToString(CultureInfo.InvariantCulture));
            if (Height > 0)
                result.Add(nameof(Height).ToLowerInvariant(), Height.ToString(CultureInfo.InvariantCulture));
            foreach (string key in Parameters)
            {
                result[key] = Parameters[key];
            }

            return result;
        }

        public static IImageTransformationPropertiesSerializer PropertiesSerializer =>
            ServiceLocator.Resolve<IImageTransformationPropertiesSerializer>();

        public virtual string ToPropertiesString()
        {
            var result = PropertiesSerializer.Serialize(this.ToPropertiesDictionary());
            return result;
        }
        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Width <= 0 && Height <= 0;
            }
        }

        /// <summary>
        /// Creates and fills instance of <see cref="ImageTransformQuery"/> from URL query string values collection.
        /// </summary>
        /// <param name="queryString">The URL query string values collection.</param>
        /// <param name="defaultTransform">The default transform.</param>
        /// <returns>
        /// The query instance.
        /// </returns>
        [NotNull]
        public static ImageTransformQuery FromQueryString([NotNull] NameValueCollection queryString, ImageTransform defaultTransform = ImageTransform.DownFit)
        {
            if (queryString == null) throw new ArgumentNullException(nameof(queryString));

            var queryKeys = new HashSet<string>(new[]
            {
                "width",
                "height",
                "transform",
                "h"
            }, StringComparer.InvariantCultureIgnoreCase);

            var result = new ImageTransformQuery
            {
                Width = (int)SmartConvert.ChangeType<uint>(queryString["width"]),
                Height = (int)SmartConvert.ChangeType<uint>(queryString["height"]),
                Transform = SmartConvert.ChangeType(queryString["transform"], defaultTransform),
                Hash = queryString["h"]
            };

            foreach (string key in queryString)
            {
                if (!queryKeys.Contains(key))
                    result.Parameters.Add(key, queryString[key]);
            }

            return result;
        }
    }
}