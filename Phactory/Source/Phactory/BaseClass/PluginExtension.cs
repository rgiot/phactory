using System;
using System.Collections.Generic;
using System.Text;

namespace PhactoryHost.BaseClass
{
    public class PluginExtension : PhactoryHost.PluginExtension
    {
        private string name;
        private string description;
        private bool canBeCreatedFromScratch;

        public PluginExtension(string name, string description, bool canBeCreatedFromScratch)
        {
            this.name = name;
            this.description = description;
            this.canBeCreatedFromScratch = canBeCreatedFromScratch;
        }

        public string GetName()
        { 
            return name;
        }

        public string GetDescription()
        { 
            return description;
        }

        public bool CanBeCreatedFromScratch()
        {
            return canBeCreatedFromScratch;
        }
    }
}
