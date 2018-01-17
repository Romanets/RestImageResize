using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace RestImageResize.EPiServer.PackageTest.Models.Pages
{
    [ContentType(DisplayName = "StartPage", GUID = "a8267682-bf9e-4ad7-a4c5-4763f1534073", Description = "")]
    public class StartPage : PageData
    {
        public virtual ContentReference Image { get; set; }
    }
}