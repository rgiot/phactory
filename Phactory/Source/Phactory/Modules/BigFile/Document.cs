using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CPCBigFile.Document
{
    [Serializable]
    public class File
    {
        public int ResourceID;
        public int ResourceCPCBigFileID;
        public bool Pad256;
        public bool SetAddress;
        public int Address;
        
        public File()
        {
            ResourceID = -1;
            ResourceCPCBigFileID = -1;
            Pad256 = false;
            SetAddress = false;
            Address = 0;
        }

        public File(int resourceID)
        {
            ResourceID = resourceID;
            ResourceCPCBigFileID = -1;
            Pad256 = false;
            SetAddress = false;
            Address = 0;
        }
    }

   [Serializable]
    public class Document
    {
        public string Version;

        public bool TruncateFiles = false;
        public bool FilesInBank = true;
        public int BaseAddress = 0x4000;

        public List<File> Files = new List<File>();
        
        public Document()
        {
            Version = Plugin.ControllerEditor.GetVersion();
        }

        public void Collapse()
        {
            for (int i = 0; i < Files.Count; i++)
            {
                File fileDoc = Files[i];
                if (fileDoc.ResourceCPCBigFileID != -1)
                {
                    int resId = fileDoc.ResourceCPCBigFileID;

                    while ( (Files.Count>0) 
                        &&  (i<Files.Count)
                        &&  (Files[i].ResourceCPCBigFileID == resId))
                    {
                        Files.RemoveAt(i);
                    }

                    File cpcBigFileItem = new File(resId);
                    Files.Insert(i, cpcBigFileItem);
                }
            }
        }
        
        public void Expand()
        {
            for (int i = 0; i < Files.Count; i++)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerCompiler.Host.GetResource(Files[i].ResourceID);
                if (resource == null)
                {
                    continue;
                }
                
                FileInfo fileInfo = Plugin.ControllerCompiler.Host.GetFileInfo(resource);

                if (fileInfo.Extension.ToLower() == ".cpcbigfile")
                {
                    Document tmpDocument = Plugin.ControllerCompiler.Host.XMLRead<Document>(fileInfo.FullName);

                    // remove .cpcbigfile reference
                    Files.RemoveAt(i);

                    int next = 0; // to respect order
                    foreach (File file in tmpDocument.Files)
                    {
                        Files.Insert(i + next, file);
                        next++;

                        file.ResourceCPCBigFileID = resource.Id;
                    }

                    // ensure i to be at the beginning of the new content, in case of recursive adds
                    i--;
                }
            }
        }
    }
}
