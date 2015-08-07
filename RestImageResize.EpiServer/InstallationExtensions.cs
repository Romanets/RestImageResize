using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using EPiServer.ImageLibrary;
using OpenWaves;
using OpenWaves.ImageTransformations.EPiServer;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.EPiServer
{
    public static class InstallationExtensions
    {
        public static BasicResolver RegisterRestImageResize(this BasicResolver resolver)
        {
            var imageDataFileProvider = global::EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ImageDataFileProvider>();
            var webImageTransformService = new WebImageTransformationService(
                new VirtualFileProviderCombiner(imageDataFileProvider, new EPiVirtualPathFileProvider(HostingEnvironment.VirtualPathProvider)),
                new ConcurrentFileStore(new VirtualFileStore("ImagesTransformVPP")),
                new EPiImageTransformationService(new ImageService()));

            var validationRules = new IImageTransformationUrlValidationRule[]
            {
                new ForEditorsImageTransformationUrlValidationRule(),
                new HmacImageTransformationUrlValidationRule()
            };

            var moduleImplementation = new WebImageTransformationModuleImplementation(webImageTransformService, validationRules);

            resolver
                .Register<IWebImageTransformationService>(moduleImplementation)
                .Register<IWebImageTransformationModuleImplementation>(moduleImplementation);

            return resolver;
        }
    }
}
