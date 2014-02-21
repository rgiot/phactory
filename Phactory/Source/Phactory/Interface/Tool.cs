using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhactoryHost
{
    public interface ToolPlugin : Plugin
    {
        void ShowDialog(IWin32Window owner);
    }
}
