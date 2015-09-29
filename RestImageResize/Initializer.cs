using System;
using System.Collections.Generic;
using System.Linq;
using OpenWaves.ImageTransformations;
using RestImageResize.Security;
using RestImageResize.Utils;

namespace RestImageResize
{
    internal class Initializer
    {
        public void InitializeResizer()
        {
            var resolver = new WrapResolver(ServiceLocatorUtils.GetCurrentResolver());

            RegisterDefault<IImageTransformationParser>(new UniversalImageTransformationParser(), resolver);

            // hash authorization

            var appSettingsPrivateKeyProvider = new AppSettingsPrivateKeyProvider();
            var sha1HashGenerator = new Sha1HashGenerator();
            var privateKeyQueryAuthorizer = new PrivateKeyQueryAuthorizer(appSettingsPrivateKeyProvider, sha1HashGenerator);

            RegisterDefault<IPrivateKeyProvider>(appSettingsPrivateKeyProvider, resolver);
            RegisterDefault<IHashGenerator>(sha1HashGenerator, resolver);
            RegisterDefault<IQueryAuthorizer>(privateKeyQueryAuthorizer, resolver);

            OpenWaves.ServiceLocator.SetResolver(resolver);
        }

        private void RegisterDefault<T>(T implementation, WrapResolver resolver) where T : class
        {
            T privateKeyProvider;
            if (!resolver.TryResolve(out privateKeyProvider))
            {
                resolver.Register(implementation);
            }
        }
    }
}
