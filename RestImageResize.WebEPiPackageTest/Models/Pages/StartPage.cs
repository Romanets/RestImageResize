using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace RestImageResize.WebEPiTest.Models.Pages
{
    [ContentType(GUID = "04F455CD-0605-4FEF-85B6-9617AB074B83")]
    public class StartPage : PageData
    {
        [BackingType(typeof(PropertyImageUrl))]
        public virtual Url MainImage { get; set; }

        public virtual ContentArea MainContent { get; set; }
    }
}