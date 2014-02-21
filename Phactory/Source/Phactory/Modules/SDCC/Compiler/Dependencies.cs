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
        private void GetLocalSourceDependencies(PhactoryHost.Database.Resource resource, List<PhactoryHost.Database.Resource> dependentResources)
        {
            GetLocalSourceDependencies(resource, dependentResources, "include ");
            GetLocalSourceDependencies(resource, dependentResources, "incbin ");
        }

        private void GetLocalSourceDependencies(PhactoryHost.Database.Resource resource, List<PhactoryHost.Database.Resource> dependentResources, string patternSearch)
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

            string fileContent = File.ReadAllText(fileInfo.FullName);

            int startIndex = 0;
            while (startIndex != -1)
            {
                int newStartIndex = fileContent.IndexOf(patternSearch, startIndex, StringComparison.OrdinalIgnoreCase);
                
                startIndex = newStartIndex;

                if (startIndex != -1)
                {
                    int indexEndLine = fileContent.IndexOf("\n", startIndex);
                    int indexFirstQuote = fileContent.IndexOf("\"", startIndex + patternSearch.Length);
                    int indexSecondQuote = fileContent.IndexOf("\"", indexFirstQuote + 1);

                    if ((indexEndLine == -1) || (indexFirstQuote == -1) || (indexSecondQuote == -1))
                    {
                        startIndex = -1;
                    }
                    else
                    {
                        string path = fileContent.Substring(indexFirstQuote + 1, indexSecondQuote - indexFirstQuote - 1);

                        FileInfo fileInfoDependency = new FileInfo(path);
                        if (!fileInfoDependency.Exists)
                        {
                            fileInfoDependency = new FileInfo(fileInfo.DirectoryName + "\\" + path);
                        }

                        PhactoryHost.Database.Resource depResource = Host.GuessResource(fileInfoDependency);
                        if ((depResource != null) && (resource != depResource))
                        {
                            dependentResources.Add(depResource);
                        }

                        startIndex = indexEndLine;
                    }
                }
            }
        }

        private void GetDependenciesRecursive(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> dependentResources = new List<PhactoryHost.Database.Resource>();

            FileInfo fileInfo = Host.GetFileInfo(resource);
            
            if ((fileInfo.Extension.ToLower() == ".asm"))
            {
                GetLocalSourceDependencies(resource, dependentResources);
            }
            else
            if ((fileInfo.Extension.ToLower() == ".c") || (fileInfo.Extension.ToLower() == ".h") || (fileInfo.Extension.ToLower() == ".cxx"))
            {
                GetLocalSourceDependencies(resource, dependentResources);
            }

            Host.RefreshDependencies(resource, dependentResources);
        }

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

            if (!IsResourceSupported(resource))
            {
                return;
            }

            GetDependenciesRecursive(resource);
        }
    }
}
