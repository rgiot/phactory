using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCDSK.Controller
{
    public class ManageDSK : PhactoryHost.ToolPlugin
    {
        public PhactoryHost.Host Host;

        public ManageDSK()
        {
            Plugin.ControllerManageDSK = this;
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
            return "ManageDSK";
        }

        public string GetDescription()
        {
            return "Launch ManageDSK";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public void ShowDialog(IWin32Window owner)
        {
            string ManageDSKFullPath = Host.GetPluginsPath() + "ManageDSK.exe";
            string arguments = String.Empty;
            string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Host.StartProcess(ManageDSKFullPath, arguments, initialDirectory, false);
        }
    }
}
