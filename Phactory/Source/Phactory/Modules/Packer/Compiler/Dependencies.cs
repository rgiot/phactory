using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCPacker.Controller
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
                foreach (Document.Item item in document.Items)
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

            List<PhactoryHost.Database.Resource> dependentResources = new List<PhactoryHost.Database.Resource>();

            Document.Document document = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (document is Document.Document)
            {
                foreach( Document.Item item in document.Items )
                {
                    PhactoryHost.Database.Resource depResource = Host.GetResource(item.ResourceID);
                    if ( (depResource!=null) && (resource != depResource) )
                    {
                        dependentResources.Add(depResource);
                    }
                }
            }

            Host.RefreshDependencies(resource, dependentResources);
        }
    }
}
