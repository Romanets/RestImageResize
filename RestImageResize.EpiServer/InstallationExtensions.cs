using System.Web.Routing;
using EPiServer.Web.Routing;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.EPiServer
{
    public static class InstallationExtensions
    {
        public static BasicResolver RegisterRestImageResize(this BasicResolver resolver, IFileStore imagesCacheStore = null)
        {
           // RouteTable.Routes.Add(new Route("*.resizedimageblob", new CustomHandler()));

            var imageDataFileProvider = new ImageDataFileProvider();

            if (imagesCacheStore == null)
            {
                imagesCacheStore = new VirtualFileStore("ImagesTransformVPP");
            }

            var webImageTransformService = new WebImageTransformationService(
                imageDataFileProvider,
                new ConcurrentFileStore(imagesCacheStore),
                new MagickNetImageTransformationService());

            resolver
                .Register<IFileStore>(imagesCacheStore)
                .Register<IVirtualFileProvider>(imageDataFileProvider)
                .Register<IWebImageTransformationService>(webImageTransformService)
                .Register<IWebImageTransformationModuleImplementation>(
                    new WebImageTransformationModuleImplementation(webImageTransformService));

            return resolver;
        }

        public static BasicResolver RegisterRestImageResizeWithCacheInBlobs(this BasicResolver resolver, string cacheContainerName = "_RestImageResizeCache")
        {
            return resolver.RegisterRestImageResize(new BlobFileStore(cacheContainerName));
        }
    }
}
