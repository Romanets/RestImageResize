using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using RestImageResize.Contracts;

namespace RestImageResize
{
    /// <summary>
    /// Default implementation of <see cref="IRestImageResizeService"/>
    /// </summary>
    public class RestImageResizeService : IRestImageResizeService
    {
        private static volatile bool _resizerInitialized = false;
        private static readonly object Lock = new object();

        private readonly Lazy<IOpenWaveRestApiEncoder> _encoderLazy = new Lazy<IOpenWaveRestApiEncoder>(OpenWaves.ServiceLocator.Resolve<IOpenWaveRestApiEncoder>);
        protected virtual IOpenWaveRestApiEncoder Encoder => _encoderLazy.Value;

        /// <summary>
        /// Inititializes this instance.
        /// </summary>
        protected virtual void Initialize()
        {
            if (!_resizerInitialized)
            {
                lock (Lock)
                {
                    if (!_resizerInitialized)
                    {
                        new Initializer().InitializeResizer();
                        _resizerInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Resizes image if input image Url is supported.
        /// </summary>
        /// <param name="url">Image url to resize.</param>
        /// <param name="resizedImageUrl">Local url of resized image or null if input image url is not supported.</param>
        /// <returns><c>true</c> if image was sucessfully resized, otherwise - <c>false</c>.</returns>
        public virtual bool TryResizeImage(Uri url, out Uri resizedImageUrl)
        {
            Initialize();
            url = GetRelativeUri(url);

            if (url != null && Encoder.IsSupportedUri(url))
            {
                resizedImageUrl = Encoder.EncodeUri(url);
                return true;
            }

            resizedImageUrl = null;
            return false;
        }

        /// <summary>
        /// Builds file name of transformed image(cached) according te image transformation URI.
        /// </summary>
        /// <param name="url">Image transformation URI</param>
        /// <returns>Path where transformed image should be placed after transformation.</returns>
        public virtual string BuildTransformedImageFileName(Uri url)
        {
            Initialize();
            url = GetRelativeUri(url);
            return url != null ? Encoder.BuldPathToTransformedImage(url) : null;
        }

        private Uri GetRelativeUri(Uri uri)
        {
            if (uri != null)
            {
                if (!uri.IsAbsoluteUri)
                {
                    return uri;
                }

                if (!string.IsNullOrEmpty(uri.PathAndQuery))
                {
                    return new Uri(uri.PathAndQuery, UriKind.Relative);
                }
            }

            return null;
        }

    }
}
