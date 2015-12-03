using LightStack.LightDesk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace LightStack.LightDesk.CefProcessHandlers
{
    class LightDeskHandler : CefV8Handler
    {
        private LightApplication application;

        public LightDeskHandler(LightApplication application)
        {
            this.application = application;
        }

        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            exception = null;
            returnValue = CefV8Value.CreateNull();
            switch (name)
            {
                case "message":
                    returnValue = Message(arguments);
                    return true;

                case "resolve":
                    if (arguments.Length >= 1 && arguments[0].IsString)
                    {
                        returnValue = Resolve(arguments[0].GetStringValue());
                        if(returnValue == null)
                        {
                            exception = string.Format("resolve: Service '{0}' not found", arguments[0].GetStringValue());
                        }
                    }
                    else
                    {
                        exception = "resolve: Wrong type or number of arguments";
                    }
                    return true;               

                default:
                    returnValue = CefV8Value.CreateNull();
                    exception = null;
                    return false;
            }
        }

        private CefV8Value Resolve(string service)
        {
            var serviceInstance = application.ResolveService(service);
            if (serviceInstance != null)
            {
                return application.ResolveService<InteropService>().MapService(serviceInstance);
            }
            return CefV8Value.CreateNull();
        }

        private static CefV8Value Message(CefV8Value[] arguments)
        {
            MessageBox.Show(arguments[0].GetStringValue(), "Message form V8");
            return CefV8Value.CreateNull();
        }
    }
}
