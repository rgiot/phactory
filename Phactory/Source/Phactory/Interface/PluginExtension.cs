using System;
using System.Collections.Generic;
using System.Text;

namespace PhactoryHost
{
    public interface PluginExtension
    {
        string GetName();
        string GetDescription();
        bool CanBeCreatedFromScratch();
    }
}
