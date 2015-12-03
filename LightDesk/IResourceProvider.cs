using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStack.LightDesk
{
    public interface IResourceProvider
    {
        Stream GetResourceStream(string path);

        int GetResourceSize(string path);

        bool Exists(string path);
    }
}
