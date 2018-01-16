using System;
using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using RestImageResize.EPiServer.Test.Models.Media;

namespace RestImageResize.EPiServer.Test.Models.Pages
{
    [ContentType(DisplayName = "StartPage", GUID = "3233a939-56a2-4b90-92ea-e22d9d4e7415", Description = "")]
    public class StartPage : PageData
    {
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

    }
}