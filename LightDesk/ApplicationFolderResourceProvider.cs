using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LightStack.LightDesk
{
    public class ApplicationFolderResourceProvider : FileSystemResourceProvider
    {
        public ApplicationFolderResourceProvider() : base(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
        {
        }
    }
}
