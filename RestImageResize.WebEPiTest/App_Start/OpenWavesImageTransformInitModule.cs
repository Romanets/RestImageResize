using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using OpenWaves;
using OpenWaves.ImageTransformations.EPiServer;

namespace RestImageResize.WebEPiTest.App_Start
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class OpenWavesImageTransformInitModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ServiceLocator.SetResolver(new BasicResolver().RegisterWebImageTransformationModule());
        }

        public void Uninitialize(InitializationEngine context)
        {
            
        }

        public void Preload(string[] parameters)
        {
            
        }
    }
}