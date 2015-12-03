using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStack.LightDesk.Services
{
    public class ResourceService
    {
        private Dictionary<string, Func<string, Stream>> providers = new Dictionary<string, Func<string, Stream>>();

        public void RegisterProvider(string host, Func<string, Stream> provider)
        {
            if (providers.ContainsKey(host))
            {
                throw new InvalidOperationException("Provider with such host already added");
            }

            providers.Add(host, provider);
        }

        public Stream GetResource(string host, string path)
        {
            Func<string, Stream> provider;
            if(providers.TryGetValue(host, out provider))
            {
                return provider(path);
            }

            return null;
        }
    }
}
