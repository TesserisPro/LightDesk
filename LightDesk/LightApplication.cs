using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using Xilium.CefGlue;
using System.Drawing;

namespace LightStack.LightDesk
{
    public class LightApplication
    {
        protected DryIoc.Container Container { get; set; } = new DryIoc.Container();

        protected Dictionary<string, Type> services = new Dictionary<string, Type>();

        public LightApplication()
        {

        }

        public LightApplication(string title, string entryPoint = "index.html")
        {
            Title = title;
            EntryPoint = entryPoint;
        }

        public string Title { get; protected set; }

        public Icon Icon { get; set; }

        public string EntryPoint { get; protected set; }

        public IResourceProvider ResourceProvider { get; set; } = new ApplicationFolderResourceProvider();

        public void RegisterServiceInstance<T>(T instance)
        {
            this.services.Add(typeof(T).Name, typeof(T));
            Container.RegisterInstance<T>(instance);
        }

        public void RegisterService<T>()
        {
            this.services.Add(typeof(T).Name, typeof(T));
            Container.Register<T>(Reuse.Singleton);
        }

        public T ResolveService<T>()
        {
            return Container.Resolve<T>(IfUnresolved.ReturnNull);
        }

        public object ResolveService(string name)
        {
            var type = this.services[name];
            return Container.Resolve(type);
        }
    }
}
