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
        public void RefreshOutput(PhactoryHost.Database.Resource resource)
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

            List<string> outputFilenames = new List<string>();

            foreach (Document.Item item in tempDocument.Items)
            {
                PhactoryHost.Database.Resource depResource = Host.GetResource(item.ResourceID);
                if (resource != depResource)
                {
                    FileInfo depFileInfo = Host.GetFileInfo(depResource);

                    string outputFilename = depFileInfo.Name + ".pck";
                    outputFilename.ToUpper();

                    outputFilenames.Add(outputFilename);
                }
            }

            Host.RefreshOutput(resource, outputFilenames); 
        }
    }
}
