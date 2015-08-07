using System.Globalization;
using System.IO;
using EPiServer.Core;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.EPiServer
{
    public class ImageDataVirtualFile : IVirtualFile
    {
        public ImageDataVirtualFile(Url url, ImageData image)
        {
            Image = image;
            Url = url;
        }

        public virtual Stream Open()
        {
            
            return Image.BinaryData.OpenRead();
        }

        public virtual Url Url { get; private set; }

        public string Hash { get { return Image.Changed.ToFileTime().ToString(CultureInfo.InvariantCulture); } }

        protected ImageData Image { get; private set; }
    }
}