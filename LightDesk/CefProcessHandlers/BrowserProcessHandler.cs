namespace LightStack.LightDesk.CefProcessHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using Xilium.CefGlue;

    internal sealed class BrowserProcessHandler : CefBrowserProcessHandler
    {
        protected override void OnBeforeChildProcessLaunch(CefCommandLine commandLine)
        {
            // .NET in Windows treat assemblies as native images, so no any magic required.
            // Mono on any platform usually located far away from entry assembly, so we want prepare command line to call it correctly.
            if (Type.GetType("Mono.Runtime") != null)
            {
                if (!commandLine.HasSwitch("cefglue"))
                {
                    var path = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;
                    commandLine.SetProgram(path);

                    var mono = CefRuntime.Platform == CefRuntimePlatform.Linux ? "/usr/bin/mono" : @"C:\Program Files\Mono-2.10.8\bin\monow.exe";
                    commandLine.PrependArgument(mono);

                    commandLine.AppendSwitch("cefglue", "w");
                }
            }
        }
    }
}
