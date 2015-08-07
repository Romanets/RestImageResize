using System;
using System.Configuration;
using System.IO;
using System.Linq;
using EPiServer.Web;
using EPiServer.Web.Hosting;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;
using ServiceLocator = EPiServer.ServiceLocation.ServiceLocator;

namespace RestImageResize.EPiServer
{
    public class VirtualFileStore : IFileStore
    {
        private readonly string _storePath;
        private readonly Url _storeUrl;

        public VirtualFileStore(string virtualPathProviderName)
        {
            if (string.IsNullOrEmpty(virtualPathProviderName))
            {
                throw new ArgumentNullException("virtualPathProviderName");
            }

            var vppRegistrationHandler = ServiceLocator.Current.GetInstance<VirtualPathRegistrationHandler>();
            var storeVirtualPathProviderSettings = vppRegistrationHandler.RegisteredVirtualPathProviders.Values.FirstOrDefault(ps => ps.Name == virtualPathProviderName);

            if (storeVirtualPathProviderSettings == null)
            {
                throw new ArgumentException(string.Format("Virtual path provider with name \"{0}\" has not been registered, please check EPiServerFramwork.config file.", virtualPathProviderName), "virtualPathProviderName");
            }

            string storePath = storeVirtualPathProviderSettings.Parameters["physicalPath"];
            string storeUrl = storeVirtualPathProviderSettings.Parameters["virtualPath"];

            if (string.IsNullOrEmpty(storePath) || string.IsNullOrEmpty(storeUrl))
            {
                throw new ConfigurationErrorsException(@"Target virtual path provider is not well configured, ""physicalPath"" and ""virtualPath"" parameters are required.");
            }

            _storePath = VirtualPathUtilityEx.RebasePhysicalPath(storePath);
            _storeUrl = Url.Parse(storeUrl);

            if (!Directory.Exists(_storePath))
            {
                Directory.CreateDirectory(storePath);
            }
        }

        protected string StorePath
        {
            get { return _storePath; }
        }

        protected Url StoreUrl
        {
            get { return _storeUrl; }
        }

        public virtual Url GetFileUrl(string fileName)
        {
            Url url = StoreUrl.Combine(fileName);
            return url;
        }

        public virtual bool FileExists(string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        public virtual Stream CreateFile(string fileName)
        {
            string filePath = GetFilePath(fileName);
            // ReSharper disable AssignNullToNotNullAttribute
            DirectoryInfo targetDir = new DirectoryInfo(Path.GetDirectoryName(filePath));
            // ReSharper restore AssignNullToNotNullAttribute

            if (!targetDir.Exists)
            {
                targetDir.Create();
            }

            return File.Create(filePath);
        }

        public virtual void DeleteFile(string fileName)
        {
            string filePath = GetFilePath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        protected string GetFilePath(string fileName)
        {
            return Path.Combine(StorePath, fileName);
        }
    }
}
