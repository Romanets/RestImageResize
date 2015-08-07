using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using OpenWaves;
using OpenWaves.ImageTransformations.EPiServer;
using OpenWaves.ImageTransformations.Web;

namespace $rootnamespace$.App_Start
{
    public class ImageResizerConfig : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var webImageTransformService = new WebImageTransformationService(
                new VirtualFileProviderCombiner(context.Locate.Advanced.GetInstance<ImageDataFileProvider>(), new EPiVirtualPathFileProvider(HostingEnvironment.VirtualPathProvider)),
                new ConcurrentFileStore(new VirtualFileStore("ImagesTransformVPP")),
                new EPiImageTransformationService(new ImageService()));

            var validationRules = new IImageTransformationUrlValidationRule[]
            {
                new ForEditorsImageTransformationUrlValidationRule(),
                new HmacImageTransformationUrlValidationRule()
            };

            var moduleImplementation = new WebImageTransformationModuleImplementation(webImageTransformService, validationRules);

            ServiceLocator.SetResolver(new BasicResolver().Register<IWebImageTransformationService>(moduleImplementation).Register<IWebImageTransformationModuleImplementation>(moduleImplementation));
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}