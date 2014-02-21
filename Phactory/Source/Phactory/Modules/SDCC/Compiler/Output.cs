using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace SDCC.Controller
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

            if ( (fileInfo.Extension.ToLower() != ".c") && (fileInfo.Extension.ToLower() != ".asm"))
            {
                // only compiles .ASM and .C files (ignore .H)
                return;
            }

            List<string> outputFilenames = new List<string>();
            
            if (fileInfo.Extension.ToLower() == ".c") 
            {
                // check that .c file is not an include from an other .c file
                // (in that case, skip compile)
                foreach (PhactoryHost.Database.Resource resourceDependency in Host.GetDependencyResources(resource))
                {
                    FileInfo parentFileInfo = Host.GetFileInfo(resourceDependency);
                    if (parentFileInfo.Extension.ToLower() == ".c")
                    {
                        return;
                    }
                }

                string outputFilename = fileInfo.Name;
                outputFilename = outputFilename.Replace(".c", ".asm");
                outputFilename = outputFilename.Replace(".C", ".ASM");

                outputFilenames.Add(outputFilename);
            }

            if (fileInfo.Extension.ToLower() == ".asm")
            {
                string outputFilename = fileInfo.Name;
                outputFilename = outputFilename.Replace(".asm", ".bin");
                outputFilename = outputFilename.Replace(".ASM", ".BIN");

                outputFilenames.Add(outputFilename);
            }

            Host.RefreshOutput(resource, outputFilenames);
        }
    }
}