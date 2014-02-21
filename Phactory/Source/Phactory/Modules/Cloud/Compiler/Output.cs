using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCCloud.Controller
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

            var tempDocument = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            var outputFilenames = new List<string>();

            foreach (Document.Item item in tempDocument.Items)
            {
                var resItem = Host.GetResource(item.ResourceID);
                var resFileInfo = Host.GetFileInfo( resItem );
                var outputFilename = resFileInfo.Name;
                
                string outputFilenameBin = outputFilename + ".bin";
                outputFilenames.Add(outputFilenameBin);
            }       

            Host.RefreshOutput(resource, outputFilenames); 
        }
    }
}