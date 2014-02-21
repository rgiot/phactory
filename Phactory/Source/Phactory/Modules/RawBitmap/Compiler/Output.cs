using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCRawBitmap.Controller
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
                PhactoryHost.Database.Resource resItem = Host.GetResource(item.ResourceID);
                FileInfo resFileInfo = Host.GetFileInfo( resItem );
                string outputFilename = resFileInfo.Name;

                Document.ItemType type = item.Type;
                
                if (type == CPCRawBitmap.Document.ItemType.Raw)
                {                    
                    string outputTopFilename = outputFilename + ".rawBin";
                    outputFilenames.Add(outputTopFilename);
                }
                else if (type == CPCRawBitmap.Document.ItemType.VerticalRaw)
                {
                    string outputTopFilename = outputFilename + ".verticalRawBin";
                    outputFilenames.Add(outputTopFilename);
                }            
            }       

            Host.RefreshOutput(resource, outputFilenames); 
        }
    }
}