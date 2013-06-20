using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;
using RestImageResize.Contracts;
using RestImageResize.Utils;

namespace RestImageResize
{
    /// <summary>
    /// The default instance of <see cref="IOpenWaveRestApiEncoder"/>.
    /// </summary>
    public class OpenWaveRestApiEncoder : IOpenWaveRestApiEncoder
    {
        private static readonly IEnumerable<string> ValidImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenWaveRestApiEncoder"/> class.
        /// </summary>
        public OpenWaveRestApiEncoder() // needed to default implementation if interface resolve.
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenWaveRestApiEncoder"/> class.
        /// </summary>
        /// <param name="imageTransformService">The image transform service.</param>
        /// <param name="imageTransformationBuilderFactory">The image transformation builder factory.</param>
        /// <param name="defaultImageTransform">The image transform type that should be used if not specified in query.</param>
        public OpenWaveRestApiEncoder(
            IWebImageTransformationService imageTransformService = null,
            IImageTransformationBuilderFactory imageTransformationBuilderFactory = null,
            ImageTransform? defaultImageTransform = null)
        {
            ImageTransformationService = imageTransformService ?? OpenWaves.ServiceLocator.Resolve<IWebImageTransformationService>();
            ImageTransformationBuilderFactory = imageTransformationBuilderFactory ?? OpenWaves.ServiceLocator.Resolve<IImageTransformationBuilderFactory>();
            DefaultImageTransform = defaultImageTransform ?? Config.DefaultTransform;
        }

        /// <summary>
        /// Gets the OpenWave image transformation service.
        /// </summary>
        protected virtual IWebImageTransformationService ImageTransformationService { get; private set; }

        /// <summary>
        /// Gets the image transformation builder factory.
        /// </summary>
        protected virtual IImageTransformationBuilderFactory ImageTransformationBuilderFactory { get; private set; }

        /// <summary>
        /// Gets the default image transform.
        /// </summary>
        protected virtual ImageTransform DefaultImageTransform { get; private set; }

        /// <summary>
        /// Determines whether URL is supported to encode (contains image resizing request).
        /// </summary>
        /// <param name="uri">The target URI.</param>
        /// <returns>
        ///   <c>true</c> if target URI is supported; otherwise, <c>false</c>
        /// </returns>
        public bool IsSupportedUri(Uri uri)
        {
            string fileExtyention = (Path.GetExtension(uri.GetFileName()) ?? string.Empty).ToLower();
            if (ValidImageExtensions.Contains(fileExtyention))
            {
                var queryString = uri.GetQueryString();

                // if some transformation requested.
                if (!ImageTransformQuery.FromQueryString(queryString, DefaultImageTransform).IsEmpty)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Encodes the URI to OpenWave image transformation URI.
        /// </summary>
        /// <param name="uri">The target URI.</param>
        /// <returns>
        /// Transformed URI instance.
        /// </returns>
        public Uri EncodeUri(Uri uri)
        {
            if (!IsSupportedUri(uri))
            {
                return uri;
            }

            var transformQuery = ImageTransformQuery.FromQueryString(uri.GetQueryString(), DefaultImageTransform);

            var transformBuilder = ImageTransformationBuilderFactory.CreateBuilder();
            transformBuilder.Width = transformQuery.Width;
            transformBuilder.Height = transformQuery.Height;
            transformBuilder.TransformType = transformQuery.Transform;

            Url url = ImageTransformationService.GetTransformedImageUrl(Url.Parse(uri.ToString()), transformBuilder);
            return new Uri(url.ToString(), UriKind.RelativeOrAbsolute);
        }
    }
}
