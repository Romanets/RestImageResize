using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using OpenWaves;
using OpenWaves.ImageTransformations.EPiServer;

namespace $rootnamespace$.App_Start
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class OpenWavesImageTransformationsInitModule : IInitializableModule
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