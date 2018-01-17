using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace RestImageResize.EPiServer.Test.Models.Media
{
    [ContentType(DisplayName = "Image", GUID = "60b5ff20-96eb-4057-91c9-8bab83a75b96", Description = "")]
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