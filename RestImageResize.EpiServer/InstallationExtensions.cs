using OpenWaves;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.EPiServer
{
    public static class InstallationExtensions
    {
        public static BasicResolver RegisterRestImageResize(this BasicResolver resolver)
        {
            var imageDataFileProvider = new ImageDataFileProvider();
            
            var fileStore = new VirtualFileStore("ImagesTransformVPP");
            var webImageTransformService = new WebImageTransformationService(
                imageDataFileProvider,
                new ConcurrentFileStore(fileStore),
                new MagickNetImageTransformationService());

            resolver
                .Register<IFileStore>(fileStore)
                .Register<IVirtualFileProvider>(imageDataFileProvider)
                .Register<IWebImageTransformationService>(webImageTransformService)
                .Register<IWebImageTransformationModuleImplementation>(
                    new WebImageTransformationModuleImplementation(webImageTransformService));

            return resolver;
        }
    }
}
