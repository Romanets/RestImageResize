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
            var virtualFileProvider = new VirtualFileProviderCombiner(imageDataFileProvider, new EPiVirtualPathFileProvider(HostingEnvironment.VirtualPathProvider));
            var fileStore = new VirtualFileStore("ImagesTransformVPP");
            var webImageTransformService = new WebImageTransformationService(
                virtualFileProvider,
                new ConcurrentFileStore(fileStore),
                new MagickNetImageTransforationService());

            resolver
                .Register<IFileStore>(fileStore)
                .Register<IVirtualFileProvider>(virtualFileProvider)
                .Register<IWebImageTransformationService>(webImageTransformService);

            return resolver;
        }
    }
}
