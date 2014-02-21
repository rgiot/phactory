using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PhactoryHost.Database;

namespace PhactoryHost
{
    public interface CompilerPlugin : Plugin
    {
        List<PluginExtension> GetSupportedExtensions();
        
        bool IsResourceSupported(Resource resource);
        List<int> GetBrokenResourceIDs(Resource resource);
        void UpdateDependencies(Resource resource);

        void RefreshOutput(Resource resource);

        bool IsIncludeResource(Resource resource);
            
        bool Compile(Resource resource);
    }
}
