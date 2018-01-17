using EPiServer.Core;
using EPiServer.Core.Routing.Pipeline.Internal;
using EPiServer.Web.Routing;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.EPiServer
{
    public class ImageDataFileProvider : IVirtualFileProvider
    {
        protected virtual IUrlResolver UrlResolver => global::EPiServer.Web.Routing.UrlResolver.Current;

        public IVirtualFile GetFile(Url fileUrl)
        {
            var content = UrlResolver.Route(new global::EPiServer.UrlBuilder(fileUrl));
            
            var image = content as ImageData;

            if (image != null)
            {
                return new ImageDataVirtualFile(fileUrl, image);
            }

            return null;
        }
    }
}