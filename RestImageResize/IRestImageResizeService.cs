using System;
using OpenWaves;

namespace RestImageResize
{
    /// <summary>
    /// Image resizing service.
    /// </summary>
    [DefaultImplementation(typeof(RestImageResizeService))]
    public interface IRestImageResizeService
    {
        /// <summary>
        /// Resizes image if input image Url is supported.
        /// </summary>
        /// <param name="url">Image url to resize.</param>
        /// <param name="resizedImageUrl">Local url of resized image or null if input image url is not supported.</param>
        /// <returns><c>true</c> if image was sucessfully resized, otherwise - <c>false</c>.</returns>
        bool TryResizeImage(Uri url, out Uri resizedImageUrl);
    }
}