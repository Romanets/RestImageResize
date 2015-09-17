using System;
using System.Collections.Generic;
using System.Linq;

namespace RestImageResize.Security
{
    public class AppSettingsPrivateKeyProvider : IPrivateKeyProvider
    {
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
