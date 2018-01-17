using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace RestImageResize.EPiServer.PackageTest.Models.Media
{
    [ContentType(DisplayName = "Image", GUID = "2e920015-92be-4b67-b06e-0618975243b4", Description = "")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,tiff,svg")]
    public class Image : ImageData
    {
        /*
                [CultureSpecific]
                [Editable(true)]
                [Display(
                    Name = "Description",
                    Description = "Description field's description",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
                public virtual string Description { get; set; }
         */
    }
}