using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue;
using DryIoc;
using LightStack.LightDesk.Services;
using System.Diagnostics;

namespace LightStack.LightDesk
{
    public class Engine
    {
        private LightApplication application;
        
        public Engine(LightApplication application)
        {
            this.application = application;
            this.Application.RegisterServiceInstance(new DiagnosticService());
            this.Application.RegisterServiceInstance(new ResourceService());
            this.Application.RegisterServiceInstance(application);
        }

        public LightApplication Application
        {
            get
            {
                return application;
            }
        }

        public void Run()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            CefRuntime.Load();

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows;
            settings.SingleProcess = true;
            settings.LogSeverity = CefLogSeverity.Info;
            settings.LogFile = "cef.log";
            //settings.ResourcesDirPath = System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath);
            settings.RemoteDebuggingPort = 20480;
            settings.NoSandbox = true;

            var app = new LightCefApp(this.Application);
            
            CefRuntime.Initialize(new CefMainArgs(new string[0]), settings, app, IntPtr.Zero);
            CefRuntime.RegisterSchemeHandlerFactory("http", string.Empty, new SchemeHandlerFactory(this.Application));

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            var window = new Window(this.Application);
            var windowsService = new WindowService(window);
            this.Application.RegisterServiceInstance(windowsService);
            this.Application.RegisterServiceInstance(new InteropService(windowsService));

            System.Windows.Forms.Application.Run(window);
            
            CefRuntime.Shutdown();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            Process.GetCurrentProcess().Kill();
        }
    }
}
