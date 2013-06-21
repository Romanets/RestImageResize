using System;
using System.Web.Mvc;

namespace RestImageResize.WebEPiPackageTest
{
    public class Global : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}