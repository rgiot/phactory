using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCDSK.Controller
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

            string dskOutputFilename = fileInfo.Name;
            dskOutputFilename = dskOutputFilename.Replace(".cpcdsk", ".dsk");
            outputFilenames.Add(dskOutputFilename);

            if (tempDocument.GenerateHFE)
            {
                string hfeOutputFilename = fileInfo.Name;
                hfeOutputFilename = hfeOutputFilename.Replace(".cpcdsk", ".hfe");
                outputFilenames.Add(hfeOutputFilename);
            }

            string fileListFilename = fileInfo.Name;
            fileListFilename = fileListFilename.Replace(".cpcdsk", ".filelist");
            outputFilenames.Add(fileListFilename);

            Host.RefreshOutput(resource, outputFilenames); 
        }
    }
}