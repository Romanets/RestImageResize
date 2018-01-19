using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Internal;
using EPiServer.Web.Routing;

namespace RestImageResize.EPiServer
{
    [TemplateDescriptor(Inherited = true, TemplateTypeCategory = TemplateTypeCategories.HttpHandler)]
    public class ImageDataHttpHandler : BlobHttpHandler, IRenderTemplate<ImageData>, IRenderTemplate
    {
        protected override Blob GetBlob(HttpContextBase httpContext)
        {
            Blob resizedImageBlob = httpContext.Items[Constants.ResizedImageBlobHttpContextItemKey] as Blob;
            if (resizedImageBlob != null)
            {
                return resizedImageBlob;
            }

            IBinaryStorable content = ServiceLocator.Current.GetInstance<IContentRouteHelper>().Content as IBinaryStorable;
            if (content == null)
                return (Blob)null;
            return content.BinaryData;
        }
    }
}
