using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PhactoryHost.Database;

namespace PhactoryHost
{
    public interface RunnerPlugin : Plugin
    {
        List<PluginExtension> GetSupportedExtensions();
        
        bool IsResourceSupported(Resource resource);
        
        bool Run(Resource resource);
    }
}
