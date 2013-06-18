using System;
using OpenWaves;

namespace RestImageResize
{
    /// <summary>
    /// Defines an interface of encoder of REST image resizing URI to OpenWave web image transformation URI.
    /// </summary>
    [DefaultImplementation(typeof(OpenWaveRestApiEncoder))]
    public interface IOpenWaveRestApiEncoder
    {
        /// <summary>
        /// Determines whether URL is supported to encode (contains image resizing request).
        /// </summary>
        /// <param name="uri">The target URI.</param>
        /// <returns>
        ///   <c>true</c> if target URI is supported; otherwise, <c>false</c>
        /// </returns>
        bool IsSupportedUri(Uri uri);

        /// <summary>
        /// Encodes the URI to OpenWave image transformation URI.
        /// </summary>
        /// <param name="uri">The target URI.</param>
        /// <returns>
        /// Transformed URI instance.
        /// </returns>
        Uri EncodeUri(Uri uri);
    }
}
