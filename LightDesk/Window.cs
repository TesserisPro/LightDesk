using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace LightStack.LightDesk
{
    public partial class Window : Form
    {
        public CefWebBrowser browser;

        public Window(LightApplication application)
        {
            InitializeComponent();
            if (application.Icon != null)
            {
                this.Icon = application.Icon;
            }
            this.Text = application.Title;
            browser = new CefWebBrowser();
            browser.Dock = DockStyle.Fill;
            browser.StartUrl = string.Format("http://app/{0}", application.EntryPoint);
            this.Controls.Add(browser);
            
            FadeIn();
        }

        public void FadeIn()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            {
                Thread.Sleep(1000);
                for (double i = 0; i <= 1; i += 0.1)
                {
                    Invoke(new Action(() =>
                    {
                        this.Opacity = i;
                    }));
                    Thread.Sleep(100);
                }
            }));
        }

        internal void RefreshBrowser()
        {
            browser.Refresh();
        }
    }
}
