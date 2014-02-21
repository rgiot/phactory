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

            Document.Document document = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (document is Document.Document)
            {
                List<string> outputFilenames = new List<string>();

                string outputFilename = fileInfo.Name + ".bin";
                outputFilenames.Add( outputFilename );
                
                Host.RefreshOutput(resource, outputFilenames);
            }
        }
    }
}
