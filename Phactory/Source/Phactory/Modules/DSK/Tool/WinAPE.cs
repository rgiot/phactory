using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCDSK.Controller
{
    public class Tool : PhactoryHost.ToolPlugin
    {
        public PhactoryHost.Host Host;

        public Tool()
        {
            Plugin.ControllerTool = this;
        }

        public bool ShowSettings(Panel parentPanel)
        {
            return false;
        }

        public bool SaveSettings()
        {
            return false;
        }

        public void Register(PhactoryHost.Host parent)
        {
            this.Host = parent;
        }

        public string GetName()
        {
            return "WinAPE Emulator";
        }

        public string GetDescription()
        {
            return "Launch WinAPE Emulator";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public void ShowDialog(IWin32Window owner)
        {
            string WinAPEFullPath = Host.GetPluginsPath() + "WinAPE.exe";
            string arguments = String.Empty;
            string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Host.StartProcess(WinAPEFullPath, arguments, initialDirectory, false);
        }
    }
}
