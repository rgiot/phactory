using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCBigFile.Controller
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

            Document.Document document = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (document is Document.Document)
            {
                foreach (Document.File item in document.Files)
                {
                    if (Host.GetResource(item.ResourceID) == null)
                    {
                        brokenResourceIDs.Add(item.ResourceID);
                    }
                }
            }

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

            foreach (Document.File file in tempDocument.Files)
            {
                PhactoryHost.Database.Resource depResource = Host.GetResource(file.ResourceID);
                if ( (depResource!=null) && (resource != depResource) )
                {
                    dependentResources.Add(depResource);
                }
            }

            Host.RefreshDependencies(resource, dependentResources);
        }
    }
}
