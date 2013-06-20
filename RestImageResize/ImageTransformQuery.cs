using System;
using System.Collections.Specialized;
using OpenWaves.ImageTransformations;
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
        /// Gets or sets the type of transformation that should be applied to an image.
        /// </summary>
        public ImageTransform Transform { get; set; }


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
        public static ImageTransformQuery FromQueryString(NameValueCollection queryString, ImageTransform defaultTransform)
        {

            var instance = new ImageTransformQuery
                {
                    Width = (int)SmartConvert.ChangeType<uint>(queryString["width"]),
                    Height = (int)SmartConvert.ChangeType<uint>(queryString["height"]),
                    Transform = SmartConvert.ChangeType(queryString["transform"], defaultTransform)
                };

            return instance;
        }
    }
}