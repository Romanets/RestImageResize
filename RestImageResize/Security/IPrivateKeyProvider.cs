using System.Collections.Generic;

namespace RestImageResize.Security
{
    public interface IPrivateKeyProvider
    {
        IEnumerable<PrivateKey> GetAllPrivateKeys();
    }
}