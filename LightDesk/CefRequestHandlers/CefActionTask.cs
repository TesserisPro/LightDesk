using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace LightStack.LightDesk.CefRequestHandlers
{
    public class CefActionTask : CefTask
    {
        public Action action;

        public CefActionTask(Action action)
        {
            this.action = action;
        }

        protected override void Execute()
        {
            this.action();
            this.action = null;
        }
    }

}
