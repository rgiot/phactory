using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Project;

namespace CPCPacker.Controller
{
    public partial class Compiler : PhactoryHost.CompilerPlugin
    {
        public PhactoryHost.Host Host;

        public Compiler()
        {
            Plugin.ControllerCompiler = this;
        }

        public bool IsDefaultPluginForUnknownTypes()
        {
            return false;
        }

        public bool ShowSettings(Panel parentPanel)
        {
            return false;
        }

        public bool SaveSettings()
        {
            return false;
        }

        public void Register(PhactoryHost.Host parent)
        {
            this.Host = parent;
        }

        public string GetName()
        {
            return "Packer Compiler";
        }

        public string GetDescription()
        {
            return "Compress many files";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpcpack", "Packer file (*.cpcpack)", true));

            return extensions;
        }

        public bool IsResourceSupported(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            foreach (PhactoryHost.PluginExtension extension in GetSupportedExtensions())
            {
                if (String.Compare(extension.GetName(), fileInfo.Extension, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsIncludeResource(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            if (!IsResourceSupported(resource))
            {
                return false;
            }

            if (fileInfo.Extension.ToLower().IndexOf(".pck") != -1 )
            {
                return true;
            }

            return false; 
        }

        private void InsertPackerType(string filename, int packerType, uint originalSize)
        {
            if (originalSize == 0)
            {
                byte[] header = new byte[1];
                header[0] = (byte)packerType;

                byte[] allBytes = File.ReadAllBytes(filename);
                byte[] outBytes = new byte[1 + allBytes.GetLength(0)];
                header.CopyTo(outBytes, 0);
                allBytes.CopyTo(outBytes, 1);

                File.WriteAllBytes(filename, outBytes);
            }
            else
            {
                byte[] header = new byte[3];
                header[0] = (byte)packerType;

                header[2] = (byte)(originalSize>>8);
                header[1] = (byte)(originalSize & 255);

                byte[] allBytes = File.ReadAllBytes(filename);
                byte[] outBytes = new byte[3 + allBytes.GetLength(0)];
                header.CopyTo(outBytes, 0);
                allBytes.CopyTo(outBytes, 3);

                File.WriteAllBytes(filename, outBytes);
            }
        }

        public bool Compile(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            if (!IsResourceSupported(resource))
            {
                return false;
            }

            bool isOK = true;

            int totalOriginalSize = 0;
            int totalPackedSize = 0;

            Document.Document document = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (document is Document.Document)
            {
                switch (document.PackerType)
                {
                    default:
                    case CPCPacker.Document.PackerType.Exomizer:
                        Host.Log("Using Exomizer...");
                        break;

                    case CPCPacker.Document.PackerType.BitBuster:
                        Host.Log("Using BitBuster...");
                        break;
                }

                var allTasks = new List<Task>();

                foreach (Document.Item item in document.Items)
                {
                    App.Controller.View.AppDoEvents(); 
                    
                    PhactoryHost.Database.Resource depResource = Host.GetResource(item.ResourceID);
                    if (depResource == null)
                    {
                        Host.Log("Unknown resource identifier : " + item.ResourceID);
                        return false;
                    }

                    var task = new Task(() =>
                        {
                            FileInfo depFileInfo = Host.GetFileInfo(depResource);

                            //Host.Log("  +" + depResource.DisplayName + "...");

                            int originalSize = (int)depFileInfo.Length;
                            bool switchedToExomizer = false;
                            bool switchedToRaw = false;

                            string exeFullPath = Host.GetPluginsPath();
                            string arguments = "";

                            var packerType = document.PackerType;

                            switch (packerType)
                            {
                                default:
                                case CPCPacker.Document.PackerType.Exomizer:
                                    exeFullPath += "exomizer.exe";
                                    arguments = "raw \"" + depFileInfo.Name + "\" -o \"" + depFileInfo.Name + ".temp.pck\"";

                                    isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);
                                    break;

                                case CPCPacker.Document.PackerType.BitBuster:
                                    exeFullPath += "BitBuster.exe";
                                    arguments = "\"" + depFileInfo.Name + "\"";

                                    if (File.Exists(depFileInfo.FullName + ".pck"))
                                    {
                                        File.Delete(depFileInfo.FullName + ".pck");
                                    }

                                    isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);

                                    isOK = File.Exists(depFileInfo.FullName + ".pck");

                                    if (isOK)
                                    {
                                        // UNCOMMENT THIS BLOCK IF YOU WANT EXOMIZER FALLBACK IN CASE OF PACK FAIL WITH BITBUSTER
                                        /*
                                        int packedSize = (int)new FileInfo(depFileInfo.FullName + ".pck").Length;
                                        if (originalSize<packedSize)
                                        {
                                            switchedToExomizer = true;
                                            isOK = false;
                                        }*/
                                    }

                                    if (!isOK)
                                    {
                                        isOK = true;
                                        switchedToExomizer = true;
                                        packerType = Document.PackerType.Exomizer;

                                        exeFullPath = Host.GetPluginsPath() + "exomizer.exe";
                                        arguments = "raw \"" + depFileInfo.Name + "\" -o \"" + depFileInfo.Name + ".temp.pck\"";

                                        isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);
                                    }
                                    break;
                            }

                            if (isOK)
                            {
                                int packedSize = 0;
                                string destFullName = depFileInfo.FullName + ".pck";

                                switch (packerType)
                                {
                                    default:
                                    case CPCPacker.Document.PackerType.Exomizer:
                                        exeFullPath = Host.GetPluginsPath() + "exoopt.exe";
                                        arguments = depFileInfo.Name + ".temp.pck " + depFileInfo.Name + ".pck";

                                        isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);

                                        File.Delete(depFileInfo.FullName + ".temp.pck");

                                        if (isOK)
                                        {
                                            FileInfo outFileInfo = new FileInfo(destFullName);

                                            if (outFileInfo.Exists)
                                            {
                                                packedSize = (int)outFileInfo.Length;

                                                InsertPackerType(destFullName, 0, 0);
                                            }
                                        }
                                        break;

                                    case CPCPacker.Document.PackerType.BitBuster:
                                        {
                                            string result = Host.GetLastErrorOutput();

                                            packedSize = (int)new FileInfo(destFullName).Length;

                                            if (packedSize < originalSize)
                                            {
                                                InsertPackerType(destFullName, 1, 0);
                                            }
                                            else
                                            {
                                                switchedToRaw = true;
                                                try
                                                {
                                                    File.Copy(depFileInfo.FullName, destFullName, true);
                                                }
                                                catch
                                                {
                                                    Host.Log(depFileInfo.FullName + " : failed to copy source file !");
                                                }

                                                InsertPackerType(destFullName, 2, (uint)originalSize);
                                            }
                                        }
                                        break;
                                }

                                if (isOK)
                                {
                                    string logLine = depResource.DisplayName + ".pck (" + originalSize + " => " + packedSize;
                                    if (packedSize >= originalSize)
                                    {
                                        logLine += " bytes, pack failed)";
                                        // isOK = false;
                                        isOK = true;
                                    }
                                    else
                                    {
                                        logLine += " bytes)";
                                    }
                                    if (switchedToExomizer)
                                    {
                                        logLine += " (switched to Exomizer)";
                                    }
                                    else if (switchedToRaw)
                                    {
                                        logLine += " (switched to raw)";
                                    }

                                    Host.Log("  +" + logLine);

                                    totalOriginalSize += originalSize;
                                    totalPackedSize += packedSize;
                                }
                            }
                        });

                    allTasks.Add(task);
                }

                foreach (var task in allTasks)
                {
                    task.Start();

                    Thread.Sleep(20);
                    Application.DoEvents();
                }

                int aliveCount = 0;
                do
                {
                    aliveCount = allTasks.Count;
                    
                    foreach (var task in allTasks)
                    {
                        if (task.IsCompleted)
                        {
                            aliveCount--;
                        }
                        else if (task.IsFaulted)
                        {
                            aliveCount--;
                        }
                    }

                    Thread.Sleep(100);
                    Application.DoEvents();
                } while (aliveCount != 0);
            }

            if ( isOK )
            {
                //Host.Log("  = " + totalOriginalSize + " => " + totalPackedSize + " bytes");
            }

            return isOK;
        }
    }
}
