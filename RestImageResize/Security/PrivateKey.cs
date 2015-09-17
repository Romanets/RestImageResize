using System;
using System.Collections.Generic;
using System.Linq;

namespace RestImageResize.Security
{
    /// <summary>
    /// Represents a private key for transforming images.
    /// </summary>
    public class PrivateKey
    {
        /// <summary>
        /// Key name (just for user-friendly identification).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Private key.
        /// </summary>
        public string Key { get; set; }
    }
}