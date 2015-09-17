using System;
using System.Collections.Generic;
using System.Linq;

namespace RestImageResize.Security
{
    /// <summary>
    /// Authorizes image transformation queries.
    /// </summary>
    public class QueryAuthorizer
    {
        private readonly IPrivateKeyProvider _privateKeyProvider;
        private readonly HashGenerator _hashGenerator;

        public QueryAuthorizer(IPrivateKeyProvider privateKeyProvider, HashGenerator hashGenerator)
        {
            _privateKeyProvider = privateKeyProvider;
            _hashGenerator = hashGenerator;
        }

        /// <summary>
        /// Checks if image transformation query hash is valid (computed using a valid private key).
        /// </summary>
        /// <param name="query">Image transformation query.</param>
        public virtual bool IsAuthorized(ImageTransformQuery query)
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
