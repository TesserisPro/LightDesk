namespace LightStack.LightDesk
{
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Text;
    using System.Threading;
    using Xilium.CefGlue;

    internal sealed class ResourceHandler : CefResourceHandler
    {
        private LightApplication application;

        private Stream resourceStream;

        private int size;

        public ResourceHandler(LightApplication application)
        {
            this.application = application;
        }

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var path = new Uri(request.Url).PathAndQuery.TrimStart('/');
            var host = new Uri(request.Url).Host;
            if (host == "app")
            {
                if (application.ResourceProvider.Exists(path))
                {
                    this.resourceStream = application.ResourceProvider.GetResourceStream(path);
                    this.size = application.ResourceProvider.GetResourceSize(path);
                    if (this.size == -1)
                    {
                        this.size = (int)resourceStream.Length;
                    }

                    callback.Continue();
                    return true;
                }
            }
            else
            {
                this.resourceStream = this.application.ResolveService<ResourceService>().GetResource(host, path);
                if(this.resourceStream != null)
                {
                    this.size = (int)resourceStream.Length;
                    callback.Continue();
                    return true;
                }
            }
            callback.Cancel();
            return false;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            if (this.resourceStream != null)
            {
                //response.MimeType = "text/html";
                response.Status = 200;
                response.StatusText = "OK";

                var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
                headers.Add("Cache-Control", "no-store");
                response.SetHeaderMap(headers);

                responseLength = this.size;
                redirectUrl = null;
            }
            else
            {
                responseLength = -1;
                redirectUrl = null;
            }
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            if (this.resourceStream != null)
            {
                var buffer = new byte[bytesToRead];
                bytesRead = resourceStream.Read(buffer, 0, buffer.Length);
                response.Write(buffer, 0, bytesRead);

                if(bytesRead < bytesToRead)
                {
                    DisposeStream();
                }
                else if(bytesRead == bytesToRead)
                {
                    callback.Continue();
                }
                
                return true;
            }

            bytesRead = 0;
            return false;
        }

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override void Cancel()
        {
            DisposeStream();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DisposeStream();
        }

        private void DisposeStream()
        {
            if (this.resourceStream != null)
            {
                this.resourceStream.Dispose();
                this.resourceStream = null;
            }
        }
    }
}
