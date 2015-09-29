using System.Collections.Generic;

namespace RestImageResize.Security
{
    /// <summary>
    /// Represents a provider for private keys required to transform images.
    /// </summary>
    public interface IPrivateKeyProvider
    {
        /// <summary>
        /// Get all image transformation private keys from the provider.
        /// </summary>
        IEnumerable<PrivateKey> GetAllPrivateKeys();
    }
}