using System;
using System.Web;
using RestImageResize.Contracts;

namespace RestImageResize
{
    /// <summary>
    /// Provides image-resizing functionality that can be requested with query string of image URL that is built on top of OpenWaves.ImageTransform.Web framework.
    /// </summary>
    /// <example>
    /// &lt;img src=&quot;~/Images/test_image.jpg?width=1000&amp;height=200&amp;transform=fill&quot; /&gt;
    /// </example>
    /// <remarks>
    /// Supported transforms: Fit(default), Fill, DownFit, DownFill, Crop, Stretch.
    /// </remarks>
    public class RestImageResizeModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Updates the RawUrl property of <see cref="HttpRequest"/> instance.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="url">The target URL.</param>
        private static void UpdateRequestRawUrl(HttpRequest request, string url)
        {
            var rawUrlProperty = request.GetType().GetProperty("RawUrl");
            rawUrlProperty.SetValue(request, url, null);
        }

        /// <summary>
        /// Handles the <c>BeginRequest</c> event the current of <see cref="HttpApplication"/> instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            if (application == null || application.Context == null)
            {
                return;
            }

            var httpContext = application.Context;

            var urlEncoder = OpenWaves.ServiceLocator.Resolve<IOpenWaveRestApiEncoder>();
            if (urlEncoder != null)
            {
                if (!string.IsNullOrEmpty(httpContext.Request.Url.PathAndQuery))
                {
                    var uri = new Uri(httpContext.Request.Url.PathAndQuery, UriKind.Relative);
                    if (urlEncoder.IsSupportedUri(uri))
                    {
                        string encodedUrl = urlEncoder.EncodeUri(uri).ToString();
                        httpContext.RewritePath(encodedUrl);
                        UpdateRequestRawUrl(httpContext.Request, encodedUrl);
                    }
                }
            }
        }
    }
}
