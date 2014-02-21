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
                string resourceRelativePath = resource.RelativePath;
                resourceRelativePath = resourceRelativePath.Replace(".cpcbigfile", "");

                List<string> outputFilenames = new List<string>();
                
                outputFilenames.Add( resourceRelativePath + ".h" );

                if (document.FilesInBank)
                {
                    outputFilenames.Add(resourceRelativePath + ".c4");
                    outputFilenames.Add(resourceRelativePath + ".c5");
                    outputFilenames.Add(resourceRelativePath + ".c6");
                    outputFilenames.Add(resourceRelativePath + ".c7");
                }
                else
                {
                    outputFilenames.Add(resourceRelativePath + ".bigfile");                    
                }

                Host.RefreshOutput(resource, outputFilenames);
            }
        }
    }
}
