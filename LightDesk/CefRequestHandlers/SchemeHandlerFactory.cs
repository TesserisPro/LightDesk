namespace LightStack.LightDesk
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue;

    internal sealed class SchemeHandlerFactory : CefSchemeHandlerFactory
    {
        private LightApplication application;

        public SchemeHandlerFactory(LightApplication application)
        {
            this.application = application;
        }

        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new ResourceHandler(this.application);
        }
    }
}
