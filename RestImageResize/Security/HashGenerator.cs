using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestImageResize.Security
{
    /// <summary>
    /// Image transformation query hash generator.
    /// </summary>
    public class HashGenerator
    {
        /// <summary>
        /// Computes a hash for provided image transformation parameters.
        /// </summary>
        /// <param name="privateKey">Private key received from the image service administrator.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="transform">Image transformation mode.</param>
        public string ComputeHash(string privateKey, int width, int height, ImageTransform transform)
        {
            var values = new[]
            {
                privateKey.ToLower(),
                width.ToString(),
                height.ToString(),
                transform.ToString().ToLower()
            };
            var bytes = Encoding.ASCII.GetBytes(string.Join(":", values));

            var sha1 = SHA1.Create();
            sha1.ComputeHash(bytes);

            return BitConverter.ToString(sha1.Hash).Replace("-", "").ToLower();
        }
    }
}