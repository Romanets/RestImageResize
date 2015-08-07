RestImageResize
===============

### Provides an ASP.NET HttpModule that adds support of simple rest API to OpenWaves.ImageTransformations.Web package.
***
For quick start install [Nuget][1] Package [RestImageResize][2] (or [RestImageResize.EPiServer][3] for [EpiServer CMS][5] based site) with nuget [Package Manager Console][4] and add image url with query like in following example:
```
<img src="~/Content/Images/bigcat.JPG?width=1200&height=260&transform=fill" />
```
***

Query syntax:

Parameters "width" and "height" - numbers from 0 to 2147483647, if any of them is missed value will be automatically ajusted according to transformation type, for 'fit' transformations missed dimension is ignored for other is treat as unchangeable.

Parameter "transform" can be specified with one of the following values: "fit", "fill", "downFit", "downFill", "crop", "stretch". If parameter is missed - "downFit" value will be used as default. You can configure this behaviour with "RestImageResize.DefautTransform" appSettings configuration section's item.

## EPiServer Integration

After installing [RestImageResize.EPiServer][3] package, add next code to your initialization module.

For EPiServer 7.5/8+:

    OpenWaves.ServiceLocator.SetResolver(new OpenWaves.BasicResolver().RegisterRestImageResize());

For older versions:

    OpenWaves.ServiceLocator.SetResolver(new OpenWaves.BasicResolver().RegisterWebImageTransformationModule());

[1]: http://nuget.org/
[2]: http://nuget.org/packages/RestImageResize/
[3]: http://nuget.org/packages/RestImageResize.EpiServer/
[4]: http://docs.nuget.org/docs/start-here/using-the-package-manager-console
[5]: http://www.episerver.com/Products/EPiServer-7-CMS/
