RestImageResize for EPiServer
=============================

To enable RestImageResize add following initialization module.

	[InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class OpenWavesImageTransformationsInitModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ServiceLocator.SetResolver(new BasicResolver().RegisterRestImageResize()); // With cache in ImagesTransformVPP
			// ServiceLocator.SetResolver(new BasicResolver().RegisterRestImageResizeWithCacheInBlobs()); // With cache in blobs
        }

        public void Uninitialize(InitializationEngine context)
        {

        }

        public void Preload(string[] parameters)
        {

        }
    }