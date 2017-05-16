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

For EPiServer 7.5/8/9+:

    OpenWaves.ServiceLocator.SetResolver(new OpenWaves.BasicResolver().RegisterRestImageResize());

For older versions:

    OpenWaves.ServiceLocator.SetResolver(new OpenWaves.BasicResolver().RegisterWebImageTransformationModule());

## Request protection

Image transformation queries can be protected from DOS attacs from a third-party by hashing a query with a private key.

By default all valid private keys should be added to `RestImageResize.PrivateKeys` app setting in format `{pk1:abc123|pk2:456edf}` (keys are pipe-delimited, each containing a name and a private key itself).
It is also possible to use another storage (eg EPiServer) for private keys: implement your own `IPrivateKeyProvider` and register it in OpenWaves service resolver:

```
public class HardcodedPrivateKeyProvider : IPrivateKeyProvider
{
    public IEnumerable<PrivateKey> GetAllPrivateKeys()
    {
        return new[]
        {
            new PrivateKey
            {
                Name = "MySecretKey",
                Key = "123456789abcdef"
            }
        };
    }
}

// In app configuration code:
OpenWaves.ServiceLocator.SetResolver(new OpenWaves.BasicResolver()
    .RegisterRestImageResize()
    .Register<IPrivateKeyProvider>(new HardcodedPrivateKeyProvider())
);
```

When at least one private key is configured, all image transformation requests should contain additional `h` query string parameter with SHA1 hash of the query in format (strings are lowercased):

    private-key:width:height:transform

For example, for the URL `http://myserver.com/images/logo.png?width=200&height=100&transform=DownFit` and a private key `123456789ABCDEF`, SHA1 hash of the query string `123456789abcdef:200:100:downfit`, which is `2649a43c840fb3398e61672b988917e9c4109cd3` should be added in `h` parameter.
So the resulting URL would be: `http://myserver.com/images/logo.png?width=200&height=100&transform=downfit&h=2649a43c840fb3398e61672b988917e9c4109cd3`.

If provided hash is wrong, then transformation parameters were changed or private key is not valid. In this case `403 Forbidden` error will be returned from the server.

C# clients could use `RestImageResize.Security.Sha1HashGenerator` from `RestImageResize` assembly to generate hashes for image queries:

    var hashGenerator = new RestImageResize.Security.Sha1HashGenerator();
    var hash = hashGenerator.ComputeHash("123456789ABCDEF", 200, 100, ImageTransform.DownFit);

## Log service configuration
The feature added in v1.1.5 allows to configure log file for RestImageResize by adding the following configuration section:

```
  <configSections>
    <section name="restImageResize.logging" type="RestImageResize.Utils.LoggingConfigurationSection, RestImageResize" />
  </configSections>
  
  <restImageResize.logging 
    logSeverityThreshold="Debug" 
    logServiceType="OpenWaves.FileLoggingService, OpenWaves"
    logDirectory="~/App_Data/logs/" />

```
Default configuration is:
```
  <restImageResize.logging logSeverityThreshold="Info"  logServiceType="RestImageResize.Utils.DebugOutputLoggingService, RestImageResize"/>
```
[1]: http://nuget.org/
[2]: http://nuget.org/packages/RestImageResize/
[3]: http://nuget.org/packages/RestImageResize.EpiServer/
[4]: http://docs.nuget.org/docs/start-here/using-the-package-manager-console
[5]: http://www.episerver.com/Products/EPiServer-7-CMS/
