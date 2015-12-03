using LightStack.LightDesk.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LightStack.LightDesk
{
    public static class Utils
    {
        public static string GetResourceString(string name)
        {
            var assembly = Assembly.GetCallingAssembly();
            var shortName = assembly.FullName.Split(',')[0];
            var resourceName = string.Format("{0}.{1}", shortName, name);
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                return reader.ReadToEnd();
            }
        }

        public static Icon GetResourceIcon(string name)
        {
            var assembly = Assembly.GetCallingAssembly();
            var shortName = assembly.FullName.Split(',')[0];
            var resourceName = string.Format("{0}.{1}", shortName, name);
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                return new Icon(stream);
            }
        }

        public static void ForeachAsyncWithProgress<T>(IEnumerable<T> items, Action<T> action, InteropService.Callback progress)
        {
            Task.Run(() =>
            {
                var total = items.Count();
                double counter = 0;
                int lastCounter = 0;
                double step = 100.0 / (double)total;
                foreach (var item in items)
                {
                    if (lastCounter != (int)counter)
                    {
                        progress(new object[] { (int)counter });
                    }
                    lastCounter = (int)counter;

                    action(item);

                    counter += step;
                }
                progress(new object[] { -1 });
            });
        }
    }
}
