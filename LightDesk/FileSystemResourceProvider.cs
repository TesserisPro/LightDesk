using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStack.LightDesk
{
    public class FileSystemResourceProvider : IResourceProvider
    {
        private string applicationRoot;

        public FileSystemResourceProvider(string path)
        {
            this.applicationRoot = path;
        }

        public bool Exists(string path)
        {
            return File.Exists(MapPath(path));
        }

        public int GetResourceSize(string path)
        {
            var info = new FileInfo(MapPath(path));
            return (int)info.Length;
        }

        public Stream GetResourceStream(string path)
        {
            return File.OpenRead(MapPath(path));
        }

        private string MapPath(string path)
        {
            return Path.Combine(this.applicationRoot, path);
        }
    }
}
