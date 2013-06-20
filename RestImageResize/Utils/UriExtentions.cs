using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace RestImageResize.Utils
{
    /// <summary>
    /// Contains utility methods that help to work with <see cref="Uri"/> instances.
    /// </summary>
    public static class UriExtentions
    {
        /// <summary>
        /// Gets the requested file name.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The file name, if some file is requested; otherwise empty string.
        /// </returns>
        public static string GetFileName(this Uri uri)
        {
            string uriPath;
            if(uri.IsAbsoluteUri)
            {
                uriPath = uri.LocalPath;
            }
            else
            {
                uriPath = uri.OriginalString;
                int queryStartMarkIndex = uriPath.IndexOf('?');
                if (queryStartMarkIndex > 0)
                {
                    uriPath = uriPath.Substring(0, queryStartMarkIndex);
                }
            }

            string filePath = (uriPath).Replace('/', '\\');
            string fileName = Path.GetFileName(filePath);
            return fileName;
        }

        /// <summary>
        /// Gets the query string values collection.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The query string values collection.
        /// </returns>
        public static NameValueCollection GetQueryString(this Uri uri)
        {
            string queryString = string.Empty;
            if (uri.IsAbsoluteUri)
            {
                queryString = uri.Query;
            }
            else
            {
                string url = uri.OriginalString;
                int queryStartMarkIndex = url.IndexOf('?');
                if (queryStartMarkIndex > 0)
                {
                    queryString = url.Substring(queryStartMarkIndex);
                }
            }

            return HttpUtility.ParseQueryString(queryString);
        }
    }
}