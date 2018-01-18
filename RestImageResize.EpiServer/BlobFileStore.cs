using System;
using System.IO;
using System.Web;
using EPiServer.Framework.Blobs;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;
using RestImageResize.EPiServer.Utils;
using ServiceLocator = EPiServer.ServiceLocation.ServiceLocator;

namespace RestImageResize.EPiServer
{
    public class BlobFileStore : IFileStore
    {
        private IBlobFactory BlobFactory => ServiceLocator.Current.GetInstance<IBlobFactory>();
        protected string BlobsContainerName { get; }

        public BlobFileStore(string blobsContainerName)
        {
            BlobsContainerName = blobsContainerName;
        }

        public virtual Url GetFileUrl(string fileName)
        {
            var imageBlob = BlobFactory.GetBlob(GetBlobUri(fileName));
            HttpContext.Current.Items[Constants.ResizedImageBlobHttpContextItemKey] = imageBlob;
            string originalImageUrl = "/" + HttpContext.Current.Request.Url.LocalPath;
            return Url.Parse(originalImageUrl);
        }

        public virtual bool FileExists(string fileName)
        {
            var blob = BlobFactory.GetBlob(GetBlobUri(fileName));
            
            try
            {
                using (blob.OpenRead())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public virtual Stream CreateFile(string fileName)
        {
            var blob = BlobFactory.GetBlob(GetBlobUri(fileName));
            return blob.OpenWrite();
        }

        public virtual void DeleteFile(string fileName)
        {
            BlobFactory.Delete(GetBlobUri(fileName));
        }

        protected virtual Uri GetBlobContainerUri()
        {
            var tempContainerId = Guid.NewGuid();
            var containerUri = Blob.GetContainerIdentifier(tempContainerId);
            var containerUrl = containerUri.ToString();
            containerUrl = containerUrl.Replace(tempContainerId.ToString("N"), BlobsContainerName);
            return new Uri(containerUrl, UriKind.RelativeOrAbsolute);
        }

        protected virtual Uri GetBlobUri(string fileName)
        {
            var fileUri = GetBlobContainerUri().Append(fileName);
            return fileUri;
        }
    }
}