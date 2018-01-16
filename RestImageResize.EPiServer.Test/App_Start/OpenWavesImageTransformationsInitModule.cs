using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using OpenWaves;
using RestImageResize.EPiServer;
using ServiceLocator = OpenWaves.ServiceLocator;

namespace RestImageResize.WebEPiTest.App_Start
{
    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class OpenWavesImageTransformationsInitModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ServiceLocator.SetResolver(new BasicResolver().RegisterRestImageResize());
        }

        public void Uninitialize(InitializationEngine context)
        {

        }

        public void Preload(string[] parameters)
        {

        }
    }
}