using LightStack.LightDesk;
using LightStack.LightDesk.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var engine = new Engine(new LightApplication("Light Desk")
            {
                ResourceProvider = new ApplicationResourcesResourceProvider()
            });
            engine.Application.ResolveService<ResourceService>().RegisterProvider("pic", x => File.OpenRead(x));
            engine.Run();
        }
    }
}
