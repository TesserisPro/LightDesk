using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightStack.LightDesk.Services
{
    public class WindowService
    {
        public Window window;

        public WindowService(Window window)
        {
            this.window = window;
        }

        public void SetTitle(string title)
        {
            window.Text = title;
        }

        public void ToggleFullScreen()
        {
            var isFullScreen = window.WindowState == FormWindowState.Maximized && window.FormBorderStyle == FormBorderStyle.None;

            if (isFullScreen)
            {
                this.window.FormBorderStyle = FormBorderStyle.Sizable;
                this.window.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.window.FormBorderStyle = FormBorderStyle.None;
                this.window.WindowState = FormWindowState.Normal;
                this.window.Invoke(new Action(() => this.window.WindowState = FormWindowState.Maximized));
            }
        }

        public void Close()
        {
            window.Close();
        }

        internal void ShowBrowser()
        {
            window.FadeIn();
        }

        public void Refresh()
        {
            window.RefreshBrowser();
        }

        public string SelectFolder()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return null;
        }

        public void Invoke(Action action)
        {
            window.Invoke(action);
        }
    }
}
