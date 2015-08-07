using EPiServer.Core;
using EPiServer.Web.Routing;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.EPiServer
{
    public class ImageDataFileProvider : IVirtualFileProvider
    {
        public ImageDataFileProvider(UrlResolver urlResolver)
        {
            UrlResolver = urlResolver;
        }

        protected virtual UrlResolver UrlResolver { get; private set; }

        public IVirtualFile GetFile(Url fileUrl)
        {
            var contentRef = UrlResolver.Route(new global::EPiServer.UrlBuilder(fileUrl));

            var image = contentRef as ImageData;

            if (image != null)
            {
                return new ImageDataVirtualFile(fileUrl, image);
            }

            return null;
        }
    }
}