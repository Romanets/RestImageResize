﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using OpenWaves;
using OpenWaves.ImageTransformations;
using OpenWaves.ImageTransformations.Web;
using RestImageResize.Security;
using RestImageResize.Transformations;
using RestImageResize.Utils;
using CentralCropTransformation = RestImageResize.Transformations.CentralCropTransformation;
using ScaleDownToFillTransformation = RestImageResize.Transformations.ScaleDownToFillTransformation;
using ScaleDownToFitTransformation = RestImageResize.Transformations.ScaleDownToFitTransformation;
using ScaleToFillTransformation = RestImageResize.Transformations.ScaleToFillTransformation;
using ScaleToFitTransformation = RestImageResize.Transformations.ScaleToFitTransformation;
using StretchTransformation = RestImageResize.Transformations.StretchTransformation;

namespace RestImageResize
{
    internal class Initializer
    {
        public void InitializeResizer()
        {
            var resolver = new WrapResolver(ServiceLocatorUtils.GetCurrentResolver());

            var transformationRegistry = GetDefaultTransformationRegistry();

            RegisterDefault<ITransformationRegistry>(transformationRegistry, resolver);
            
            // hash authorization

            var appSettingsPrivateKeyProvider = new AppSettingsPrivateKeyProvider();
            var sha1HashGenerator = new Sha1HashGenerator();
            var privateKeyQueryAuthorizer = new PrivateKeyQueryAuthorizer(appSettingsPrivateKeyProvider, sha1HashGenerator);

            RegisterDefault<IPrivateKeyProvider>(appSettingsPrivateKeyProvider, resolver);
            RegisterDefault<IHashGenerator>(sha1HashGenerator, resolver);
            RegisterDefault<IQueryAuthorizer>(privateKeyQueryAuthorizer, resolver);

            var logger = new LogServiceFactory().CreateLogger();
            resolver.Register(logger);
            RegisterDefault<IImageTransformationParser>(new UniversalImageTransformationParser(transformationRegistry, logger), resolver);

            // register default OpenWaveComponents which is used in parameterless constructor of OpenWaves.ImageTransformations.Web
            RegisterDefault<IVirtualFileProvider>(new VirtualPathFileProvider(HostingEnvironment.VirtualPathProvider), resolver);
            RegisterDefault<IFileStore>(new MapPathBasedFileStore(UrlPath.Parse("~/Images/Scaled")), resolver);

            OpenWaves.ServiceLocator.SetResolver(resolver);
        }

        private ITransformationRegistry GetDefaultTransformationRegistry()
        {
            return new TransformationRegistry()
                .Add<ScaleToFitTransformation>(ImageTransform.Fit)
                .Add<ScaleToFillTransformation>(ImageTransform.Fill)
                .Add<ScaleDownToFitTransformation>(ImageTransform.DownFit)
                .Add<ScaleDownToFillTransformation>(ImageTransform.DownFill)
                .Add<CentralCropTransformation>(ImageTransform.Crop)
                .Add<StretchTransformation>(ImageTransform.Stretch)
                .Add<ResizeMinTransformation>(ImageTransform.ResizeMin)
                .Add<DownResizeMinTransformation>(ImageTransform.DownResizeMin)
                .Add<ResizeCropTransformation>(ImageTransform.ResizeCrop)
                .Add<DownResizeCropTransformation>(ImageTransform.DownResizeCrop)
                ;
        }

        private void RegisterDefault<T>(T implementation, WrapResolver resolver) where T : class
        {
            T privateKeyProvider;
            if (!resolver.TryResolve(out privateKeyProvider))
            {
                resolver.Register(implementation);
            }
        }
    }
}
