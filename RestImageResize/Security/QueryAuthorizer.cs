using System;
using System.Collections.Generic;
using System.Linq;

namespace RestImageResize.Security
{
    public class QueryAuthorizer
    {
        private readonly HashGenerator _hashGenerator = new HashGenerator();

        public virtual bool IsAuthorized(ImageTransformQuery query)
        {
            var privateKeys = Config.PrivateKeys;

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
