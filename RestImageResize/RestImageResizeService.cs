using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Creates <see cref="RestImageResizeService"/> instance.
        /// </summary>
        public RestImageResizeService()
        {
            InitializeResizer();
            Encoder = OpenWaves.ServiceLocator.Resolve<IOpenWaveRestApiEncoder>();
        }

        private IOpenWaveRestApiEncoder Encoder { get; }

        private void InitializeResizer()
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
            InitializeResizer();

            if (!string.IsNullOrEmpty(url.PathAndQuery))
            {
                var uri = new Uri(url.PathAndQuery, UriKind.Relative);
                if (Encoder.IsSupportedUri(uri))
                {
                    resizedImageUrl = Encoder.EncodeUri(uri);
                    return true;
                }
            }

            resizedImageUrl = null;
            return false;
        }
    }
}
