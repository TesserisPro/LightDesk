using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace LightStack.LightDesk
{

    internal sealed class LightCefApp : CefApp
    {
        private CefBrowserProcessHandler _browserProcessHandler;
        private CefRenderProcessHandler _renderProcessHandler;

        public LightCefApp(LightApplication application)
        {
            _renderProcessHandler = new CefProcessHandlers.RenderProcessHandler(application);
            _browserProcessHandler = new CefProcessHandlers.BrowserProcessHandler();
        }

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            // TODO: currently on linux platform location of locales and pack files are determined
            // incorrectly (relative to main module instead of libcef.so module).
            // Once issue http://code.google.com/p/chromiumembedded/issues/detail?id=668 will be resolved
            // this code can be removed.
            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                var path = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;
                path = Path.GetDirectoryName(path);

                commandLine.AppendSwitch("resources-dir-path", path);
                commandLine.AppendSwitch("locales-dir-path", Path.Combine(path, "locales"));
            }
        }

        protected override CefBrowserProcessHandler GetBrowserProcessHandler()
        {
            return _browserProcessHandler;
        }

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return _renderProcessHandler;
        }
    }
}
