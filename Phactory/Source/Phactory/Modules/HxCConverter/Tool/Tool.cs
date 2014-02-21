using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace HxCConverter.Controller
{
    public class Tool : PhactoryHost.ToolPlugin
    {
        public PhactoryHost.Host Host;

        public Tool()
        {
            Plugin.Controller = this;
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
            return "HxC Floppy Emulator Converter";
        }

        public string GetDescription()
        {
            return "Used to convert DSK disc images to HFE format";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public void ShowDialog(IWin32Window owner)
        {
            HxCConverter.Tool.View view = new HxCConverter.Tool.View(Host);
            view.ShowDialog(owner);
        }
    }
}
