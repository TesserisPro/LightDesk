namespace LightStack.LightDesk.CefProcessHandlers
{
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using Xilium.CefGlue;
    using Xilium.CefGlue.Wrapper;

    class RenderProcessHandler : CefRenderProcessHandler
    {
        private LightApplication application;

        internal static bool DumpProcessMessages { get; private set; }

        public RenderProcessHandler(LightApplication application)
        {
            this.application = application;
            MessageRouter = new CefMessageRouterRendererSide(new CefMessageRouterConfig());
        }

        internal CefMessageRouterRendererSide MessageRouter { get; private set; }

        protected override void OnWebKitInitialized()
        {
            base.OnWebKitInitialized();
            CefRuntime.RegisterExtension("LightDesk", Utils.GetResourceString("Scripts.LightDesk.js"), new LightDeskHandler(this.application));
        }

        protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            MessageRouter.OnContextCreated(browser, frame, context);
        }

        protected override void OnBrowserCreated(CefBrowser browser)
        {
            base.OnBrowserCreated(browser);
        }

        protected override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            MessageRouter.OnContextReleased(browser, frame, context);
        }

        
        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            //if (DumpProcessMessages)
            //{
            //    var arguments = message.Arguments;
            //    for (var i = 0; i < arguments.Count; i++)
            //    {
            //        var type = arguments.GetValueType(i);
            //        object value;
            //        switch (type)
            //        {
            //            case CefValueType.Null:
            //                value = null;
            //                break;
            //            case CefValueType.String:
            //                value = arguments.GetString(i);
            //                break;
            //            case CefValueType.Int:
            //                value = arguments.GetInt(i);
            //                break;
            //            case CefValueType.Double:
            //                value = arguments.GetDouble(i);
            //                break;
            //            case CefValueType.Bool:
            //                value = arguments.GetBool(i);
            //                break;
            //            default:
            //                value = null;
            //                break;
            //        }

            //        Console.WriteLine("  [{0}] ({1}) = {2}", i, type, value);
            //    }
            //}

            var handled = MessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
            if (handled) return true;

            //if (message.Name == "myMessage2") return true;

            //var message2 = CefProcessMessage.Create("myMessage2");
            //var success = browser.SendProcessMessage(CefProcessId.Renderer, message2);


            //var message3 = CefProcessMessage.Create("myMessage3");
            //var success2 = browser.SendProcessMessage(CefProcessId.Browser, message3);


            //return false;
            return true;
        }
    }
}
