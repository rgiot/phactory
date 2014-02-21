using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace PixelCalculator.Controller
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
            return "Pixel Calculator";
        }

        public string GetDescription()
        {
            return "Used to calculate CPC pixel value";
        }

        public string GetVersion()
        {
            return "1.1";
        }

        public void ShowDialog(IWin32Window owner)
        {
            PixelCalculator.Tool.View view = new PixelCalculator.Tool.View(Host);
            view.ShowDialog(owner);
        }
    }
}
