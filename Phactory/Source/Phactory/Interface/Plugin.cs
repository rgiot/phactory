using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhactoryHost
{
    public interface Plugin
    {
        string GetName();
        string GetDescription();
        string GetVersion();
        
        void Register(Host parent);

        bool ShowSettings(Panel parentPanel);
        bool SaveSettings();
    }
}
