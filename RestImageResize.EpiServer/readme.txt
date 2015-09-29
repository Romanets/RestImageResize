RestImageResize for EPiServer
=============================

To enable RestImageResize add next code to your initialization module.

For EPiServer 7.5/8/9+:

    OpenWaves.ServiceLocator.SetResolver(new OpenWaves.BasicResolver().RegisterRestImageResize());

For older versions:

    OpenWaves.ServiceLocator.SetResolver(new OpenWaves.BasicResolver().RegisterWebImageTransformationModule());