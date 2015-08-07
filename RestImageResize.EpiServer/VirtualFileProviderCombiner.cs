using System.Collections.Generic;
using System.Linq;
using OpenWaves;
using OpenWaves.ImageTransformations.Web;

namespace RestImageResize.EPiServer
{
    public class VirtualFileProviderCombiner : IVirtualFileProvider
    {
        public VirtualFileProviderCombiner(params IVirtualFileProvider[] providers)
        {
            FileProviders = providers.ToList();
        }

        public IVirtualFile GetFile(Url fileUrl)
        {
            IVirtualFile file = null;

            foreach (var virtualFileProvider in FileProviders)
            {
                try
                {
                    file = virtualFileProvider.GetFile(fileUrl);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch { }
                // ReSharper restore EmptyGeneralCatchClause

                if (file != null)
                {
                    return file;
                }
            }

            return null;
        }

        public virtual List<IVirtualFileProvider> FileProviders { get; private set; }
    }
}