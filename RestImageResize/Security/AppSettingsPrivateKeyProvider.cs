using System;
using System.Collections.Generic;
using System.Linq;

namespace RestImageResize.Security
{
    /// <summary>
    /// Image transformation private key provider from app settings.
    /// </summary>
    public class AppSettingsPrivateKeyProvider : IPrivateKeyProvider
    {
        /// <summary>
        /// Get all configured private keys from app settings. The format of the app setting is "{pk1:abc123|pk2:456edf}" (keys are pipe-delimited, each containing a name and a private key itself).
        /// </summary>
        public IEnumerable<PrivateKey> GetAllPrivateKeys()
        {
            var privateKeysString = Config.PrivateKeys;

            if (string.IsNullOrEmpty(privateKeysString))
            {
                return new List<PrivateKey>();
            }

            var privateKeys = privateKeysString.Split('|')
                .Select(val => new PrivateKey
                {
                    Name = val.Split(':').First(),
                    Key = val.Split(':').Last()
                })
                .ToList();
            return privateKeys;
        }
    }
}
