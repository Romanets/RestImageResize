using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using OpenWaves;
using OpenWaves.ImageTransformations;
using OpenWaves.ImageTransformations.Web;
using RestImageResize.Contracts;
using RestImageResize.Security;
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
            : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenWaveRestApiEncoder" /> class.
        /// </summary>
        /// <param name="imageTransformService">The image transform service.</param>
        /// <param name="imageTransformationBuilderFactory">The image transformation builder factory.</param>
        /// <param name="defaultImageTransform">The image transform type that should be used if not specified in query.</param>
        /// <param name="imageTransformationParser">The image transformation parser.</param>
        public OpenWaveRestApiEncoder(
            IWebImageTransformationService imageTransformService = null,
            IImageTransformationBuilderFactory imageTransformationBuilderFactory = null,
            ImageTransform? defaultImageTransform = null,
            IImageTransformationParser imageTransformationParser = null,
            IQueryAuthorizer queryAuthorizer = null)
        {
            ImageTransformationService = imageTransformService ?? OpenWaves.ServiceLocator.Resolve<IWebImageTransformationService>();
            ImageTransformationBuilderFactory = imageTransformationBuilderFactory ?? OpenWaves.ServiceLocator.Resolve<IImageTransformationBuilderFactory>();
            DefaultImageTransform = defaultImageTransform ?? Config.DefaultTransform;
            QueryAuthorizer = queryAuthorizer ?? OpenWaves.ServiceLocator.Resolve<IQueryAuthorizer>();
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
        /// Gets the query authorizer.
        /// </summary>
        protected virtual IQueryAuthorizer QueryAuthorizer { get; private set; }

        /// <summary>
        /// Determines whether URL is supported to encode (contains image resizing request).
        /// </summary>
        /// <param name="uri">The target URI.</param>
        /// <returns>
        ///   <c>true</c> if target URI is supported; otherwise, <c>false</c>
        /// </returns>
        public bool IsSupportedUri(Uri uri)
        {
            string fileExtension = (Path.GetExtension(uri.GetFileName()) ?? string.Empty).ToLower();
            if (ValidImageExtensions.Contains(fileExtension))
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

            EnsureAuthorizedQuery(transformQuery);

            var transformBuilder = ImageTransformationBuilderFactory.CreateBuilder();
            transformBuilder.Width = transformQuery.Width;
            transformBuilder.Height = transformQuery.Height;
            transformBuilder.TransformType = transformQuery.Transform;

            Url url = ImageTransformationService.GetTransformedImageUrl(Url.Parse(uri.ToString()), transformBuilder);
            return new Uri(url.ToString(), UriKind.RelativeOrAbsolute);
        }

        private void EnsureAuthorizedQuery(ImageTransformQuery query)
        {
            var authorized = QueryAuthorizer.IsAuthorized(query);

            if (!authorized)
            {
                throw new HttpException((int) HttpStatusCode.Forbidden,
                    string.Format("Forbidden query 'w={0}&h={1}&t={2}&h={3}'",
                        query.Width, query.Height, Enum.GetName(typeof(ImageTransform), query.Transform).ToLower(), query.Hash));
            }
        }
    }
}
