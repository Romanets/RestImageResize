using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using OpenWaves;
using OpenWaves.ImageTransformations;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.WebTest
{


    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            OpenWaves.ServiceLocator.SetResolver(RegisterRestImageResize(new BasicResolver()));
        }


        public static BasicResolver RegisterRestImageResize(BasicResolver resolver)
        {

            var webImageTransformService = new WebImageTransformationService(
                new VirtualPathFileProvider(HostingEnvironment.VirtualPathProvider),
                new ConcurrentFileStore(new MapPathBasedFileStore(UrlPath.Parse("~/Images/Scaled"))),
                new MagickNetImageTransforationService());

            resolver.Register<IWebImageTransformationService>(webImageTransformService);

            return resolver;
        }
    }
}