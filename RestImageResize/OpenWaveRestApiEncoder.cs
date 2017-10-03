using OpenWaves;
using OpenWaves.ImageTransformations;
using OpenWaves.ImageTransformations.Web;
using RestImageResize.Contracts;
using RestImageResize.Security;
using RestImageResize.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

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
            : this(null, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenWaveRestApiEncoder" /> class.
        /// </summary>
        /// <param name="imageTransformService">The image transform service.</param>
        /// <param name="imageTransformationBuilderFactory">The image transformation builder factory.</param>
        /// <param name="defaultImageTransform">The image transform type that should be used if not specified in query.</param>
        /// <param name="queryAuthorizer">The image transformation query authorizer.</param>
        public OpenWaveRestApiEncoder(
            IWebImageTransformationService imageTransformService = null,
            IImageTransformationFactory imageTransformationBuilderFactory = null,
            ImageTransform? defaultImageTransform = null,
            IQueryAuthorizer queryAuthorizer = null,
            IVirtualFileProvider virtualFileProvider = null)
        {
            ImageTransformationService = imageTransformService ?? OpenWaves.ServiceLocator.Resolve<IWebImageTransformationService>();
            ImageTransformationFactory = imageTransformationBuilderFactory ?? OpenWaves.ServiceLocator.Resolve<IImageTransformationFactory>();
            DefaultImageTransform = defaultImageTransform ?? Config.DefaultTransform;
            QueryAuthorizer = queryAuthorizer ?? OpenWaves.ServiceLocator.Resolve<IQueryAuthorizer>();
            ImageFileProvider = virtualFileProvider ?? OpenWaves.ServiceLocator.Resolve<IVirtualFileProvider>();
        }

        /// <summary>
        /// Gets the OpenWave image transformation service.
        /// </summary>
        protected virtual IWebImageTransformationService ImageTransformationService { get; private set; }

        /// <summary>
        /// Gets the image transformation builder factory.
        /// </summary>
        protected virtual IImageTransformationFactory ImageTransformationFactory { get; private set; }

        /// <summary>
        /// Gets the default image transform.
        /// </summary>
        protected virtual ImageTransform DefaultImageTransform { get; private set; }

        /// <summary>
        /// Gets the query authorizer.
        /// </summary>
        protected virtual IQueryAuthorizer QueryAuthorizer { get; private set; }


        /// <summary>
        /// Gets Image file provider.
        /// </summary>
        protected virtual IVirtualFileProvider ImageFileProvider { get; private set; }

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

            var transformation = ImageTransformationFactory.TryCreate(transformQuery);

            //var transformBuilder = ImageTransformationBuilderFactory.CreateBuilder();
            //transformBuilder.Width = transformQuery.Width;
            //transformBuilder.Height = transformQuery.Height;
            //transformBuilder.TransformType = transformQuery.Transform;
            //transformBuilder.FocusPoint = transformQuery.FocusPoint;

            if (transformation == null)
                return uri;

            var url = ImageTransformationService.GetTransformedImageUrl(Url.Parse(uri.ToString()), transformation);
            return new Uri(url.ToString(), UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Builds path to transformed image(cached) according te image transformation URI.
        /// </summary>
        /// <param name="uri">Image transformation URI</param>
        /// <returns>Path where transformed image should be placed after transformation.</returns>
        public virtual string BuldPathToTransformedImage(Uri uri)
        {
            if (IsSupportedUri(uri))
            {
                var transformQuery = ImageTransformQuery.FromQueryString(uri.GetQueryString(), DefaultImageTransform);
                var transformation = ImageTransformationFactory.TryCreate(transformQuery);
                if (transformation != null)
                {
                    var imageFile = ImageFileProvider.GetFile(Url.Parse(uri.ToString()));

                    if (imageFile != null)
                    {
                        string transformedImagePath = GetTransformedImagePath(imageFile, transformation, imageFile.Url.Path.GetFileExtension());
                        return transformedImagePath;
                    }
                }
            }

            return null;
        }

        private void EnsureAuthorizedQuery(ImageTransformQuery query)
        {
            // note: we can use IImageTransformationUrlValidationRule concept for this. 
            var authorized = QueryAuthorizer.IsAuthorized(query);

            if (!authorized)
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, $"Forbidden query '{query}'");
            }
        }

        private static string GetTransformedImagePath(IVirtualFile imageFile, IImageTransformation transformation, string extension) // Copied from OpenWaves.ImageTransformations.Web.WebImageTransformationService.GetTransformedImagePath
        {
            var sb = new StringBuilder();
            var name = sb
                .Append(imageFile.Url)
                .Append(imageFile.Hash ?? String.Empty)
                .Append(transformation)
                .ToString();

            return MD5Hash.Compute(name) + extension;
        }
    }
}
