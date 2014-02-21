using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCText.Controller
{
    public partial class Compiler : PhactoryHost.CompilerPlugin
    {
        public List<int> GetBrokenResourceIDs(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return null;
            }

            if (!fileInfo.Exists)
            {
                return null;
            }

            List<int> brokenResourceIDs = new List<int>();

            return brokenResourceIDs;
        }

        public void UpdateDependencies(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);            
            if (fileInfo == null)
            {
                return;
            }

            if (!fileInfo.Exists)
            {
                return;
            } 
            
            if (!IsResourceSupported(resource))
            {
                return;
            }

            Document.Document tempDocument = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);

            List<PhactoryHost.Database.Resource> dependentResources = new List<PhactoryHost.Database.Resource>();

            Host.RefreshDependencies(resource, dependentResources);
        }
    }
}
