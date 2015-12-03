using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LightStack.LightDesk
{
    public class ApplicationResourcesResourceProvider : IResourceProvider
    {
        private Assembly assembly;

        public ApplicationResourcesResourceProvider(Assembly assembly = null)
        {
            this.assembly = assembly;
            if (this.assembly == null)
            {
                this.assembly = Assembly.GetEntryAssembly();
            }
        }

        public bool Exists(string path)
        {
            string resourceName = GetResourceName(path);
            return assembly.GetManifestResourceInfo(resourceName) != null;
        }

        public int GetResourceSize(string path)
        {
            string resourceName = GetResourceName(path);
            return -1;
        }

        public Stream GetResourceStream(string path)
        {
            string resourceName = GetResourceName(path);
            return assembly.GetManifestResourceStream(resourceName);
        }

        private string GetResourceName(string path)
        {
            var shortName = assembly.FullName.Split(',')[0];
            var resourceName = string.Format("{0}.{1}", shortName, path.Replace('/', '.'));
            return resourceName;
        }
    }
}
