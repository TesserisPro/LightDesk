using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightStack.LightDesk.Services
{
    public class DiagnosticService
    {
        public void Message(string message)
        {
            MessageBox.Show(message, "Diagnostic Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Warning(string message)
        {
            MessageBox.Show(message, "Diagnostic Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void Error(string message)
        {
            MessageBox.Show(message, "Diagnostic Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool Question(string message)
        {
            return MessageBox.Show(message, "Diagnostic Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
