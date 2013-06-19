using System;
using System.Collections.Specialized;
using OpenWaves.ImageTransformations;

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
        /// Created instance of <see cref="ImageTransformQuery"/> from URL query string values collection.
        /// </summary>
        /// <param name="queryString">The URL query string values collection.</param>
        /// <returns></returns>
        public static ImageTransformQuery FromQueryString(NameValueCollection queryString)
        {

            var instance = new ImageTransformQuery
                {
                    Width = (int)SmartConvert.ChangeType<uint>(queryString["width"], 0),
                    Height = (int)SmartConvert.ChangeType<uint>(queryString["height"], 0),
                    Transform = SmartConvert.ChangeType(queryString["transform"], ImageTransform.Fit)
                };

            return instance;
        }

        /// <summary>
        /// Gets the transformation <see cref="OpenWaves.ImageTransformations.ImageTransformation"/> based on this instance.
        /// </summary>
        /// <returns>
        /// The requested transformation instance.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Not supported image transformation type.</exception>
        public ImageTransformation GetTransformation()
        {
            if (!IsEmpty)
            {
                int width = Width > 0 ? Width : Height;
                int height = Height > 0 ? Height : Width;

                switch (Transform)
                {
                    case ImageTransform.Fit:
                        return new ScaleToFitTransformation(width, height);
                    case ImageTransform.Fill:
                        return new ScaleToFillTransformation(width, height);
                    case ImageTransform.DownFit:
                        return new ScaleDownToFitTransformation(width, height);
                    case ImageTransform.DownFill:
                        return new ScaleDownToFillTransformation(width, height);
                    case ImageTransform.Crop:
                        return new CentralCropTransformation(width, height);
                    case ImageTransform.Stretch:
                        return new StretchTransformation(width, height);
                    default:
                        throw new NotSupportedException("Not supported image transformation type.");
                }
            }

            return null;
        }
    }
}