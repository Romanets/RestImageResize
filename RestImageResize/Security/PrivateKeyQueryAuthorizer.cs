using System;
using System.Collections.Generic;
using System.Linq;

namespace RestImageResize.Security
{
    /// <summary>
    /// Authorizes image transformation queries by verifying query hash with configured private keys.
    /// </summary>
    public class PrivateKeyQueryAuthorizer : IQueryAuthorizer
    {
        private readonly IPrivateKeyProvider _privateKeyProvider;
        private readonly IHashGenerator _hashGenerator;

        /// <summary>
        /// Creates an authorizer.
        /// </summary>
        /// <param name="privateKeyProvider">Private key provider.</param>
        /// <param name="hashGenerator">Hash generator.</param>
        public PrivateKeyQueryAuthorizer(IPrivateKeyProvider privateKeyProvider, IHashGenerator hashGenerator)
        {
            _privateKeyProvider = privateKeyProvider;
            _hashGenerator = hashGenerator;
        }

        /// <summary>
        /// Checks if image transformation query hash is valid (computed using a valid private key).
        /// </summary>
        /// <param name="query">Image transformation query.</param>
        public bool IsAuthorized(ImageTransformQuery query)
        {
            var privateKeys = _privateKeyProvider.GetAllPrivateKeys().ToList();

            if (!privateKeys.Any())
            {
                return true;
            }

            foreach (var privateKey in privateKeys)
            {
                var hash = _hashGenerator.ComputeHash(privateKey.Key, query.Width, query.Height, query.Transform);

                if (hash == query.Hash)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
