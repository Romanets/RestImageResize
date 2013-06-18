using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize
{
    /// <summary>
    /// The default instance of <see cref="IOpenWaveRestApiEncoder"/>.
    /// </summary>
    public class OpenWaveRestApiEncoder : IOpenWaveRestApiEncoder
    {
        private static readonly IEnumerable<string> ValidImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        /// <summary>
        /// Gets the OpenWave image transformation service.
        /// </summary>
        // ReSharper disable RedundantNameQualifier
        protected IWebImageTransformationService ImageTransformationService { get { return OpenWaves.ServiceLocator.Resolve<IWebImageTransformationService>(); } }
        // ReSharper restore RedundantNameQualifier

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
                // if query string does not contain OpanWave image transformation parameters(not previously encoded).
                if (queryString["t"] == null || queryString["ts"] == null)
                {
                    // if some transformation requested.
                    if (!ImageTransformQuery.FromQueryString(queryString).IsEmpty)
                    {
                        return true;
                    }
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

            var transformQuery = ImageTransformQuery.FromQueryString(uri.GetQueryString());
            Url url = ImageTransformationService.GetTransformedImageUrl(Url.Parse(uri.ToString()), transformQuery.GetTransformation());
            return new Uri(url.ToString(), UriKind.RelativeOrAbsolute);
        }
    }
}
