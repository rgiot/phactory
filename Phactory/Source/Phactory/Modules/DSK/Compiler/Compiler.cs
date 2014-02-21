using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Project;

namespace CPCDSK.Controller
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
            return "DSK Compiler";
        }

        public string GetDescription()
        {
            return "Create DSK files used by Amstrad CPC emulators";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpcdsk", "CPCDSK File (*.cpcdsk)", true));

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

            if (fileInfo.Extension.ToLower() == ".dsk")
            {
                return true;
            }

            return false;
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

            string DSKFilename = Host.GetFileInfo(resource).FullName;
            DSKFilename = DSKFilename.Replace(".cpcdsk", ".dsk");
            FileInfo DSKFileInfo = new FileInfo(DSKFilename);

            Host.Log(new FileInfo(DSKFilename).Name);

            Document.Document document = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (!(document is Document.Document))
            {
                return false;
            }

            // create empty DSK
            if (document.TrackLoaderDisc)
            {
                App.Controller.View.AppDoEvents();

                if (File.Exists(DSKFilename))
                {
                    try
                    {
                        File.Delete(DSKFilename);
                    }
                    catch
                    {
                        Host.Log("Can't delete " + DSKFilename + " - is the file locked by other process ?");
                    }
                }
                try
                {
                    string discsysExeFullPath = Host.GetPluginsPath() + "discsys.exe";
                    var arguments = "-f \"myd\" " + DSKFileInfo.FullName;

                    if (Host.IsVerboseOutput())
                    {
                        Host.Log(discsysExeFullPath + " " + arguments);
                    }

                    isOK = Host.StartProcess(discsysExeFullPath, arguments, fileInfo.DirectoryName, true);
                }
                catch
                {
                }
            }
            else
            {
                string dskTemplateFilename = Host.GetPluginsPath() + "Empty.dsk";
                if (File.Exists(DSKFilename))
                {
                    try
                    {
                        File.Delete(DSKFilename);
                    }
                    catch
                    {
                        Host.Log("Can't delete " + DSKFilename + " - is the file locked by other process ?");
                    }
                }
                try
                {
                    File.Copy(dskTemplateFilename, DSKFilename);
                }
                catch
                {
                }
            }

            // copy files into DSK
            if (document.TrackLoaderDisc)
            {
                foreach (Document.Item item in document.Items)
                {
                    PhactoryHost.Database.Resource depResource = Host.GetResource(item.ResourceID);
                    if (depResource == null)
                    {
                        Host.Log("Unknown resource identifier : " + item.ResourceID);
                        return false;
                    }

                    FileInfo depFileInfo = Host.GetFileInfo(depResource);

                    if (!item.TrackLoaderItem)
                    {
                        // addhead -a -t "binary" -s 8192 -x 8192 boot.bin boot.bin

                        string addheadExeFullPath = Host.GetPluginsPath() + "addhead.exe";

                        string arguments = String.Empty;

                        if (item.ItemType == Document.ItemType.Basic)
                        {
                            arguments = "-a -t \"basic\" ";
                            arguments += "-s " + 368 + " ";
                            arguments += "-x " + 368 + " ";
                            arguments += depFileInfo.FullName + " ";

                            arguments += "\"" + Path.GetTempPath() + item.AmsdosFilename + "\"";
                        }
                        else
                        {
                            arguments = "-a -t \"binary\" ";
                            arguments += "-s " + item.LoadAdress + " ";
                            arguments += "-x " + item.ExecAdress + " ";
                            arguments += depFileInfo.FullName + " ";
                            arguments += "\"" + Path.GetTempPath() + item.AmsdosFilename + "\"";
                        }


                        if (Host.IsVerboseOutput())
                        {
                            Host.Log(addheadExeFullPath + " " + arguments);
                        }

                        isOK = Host.StartProcess(addheadExeFullPath, arguments, fileInfo.DirectoryName, true);
                        if (isOK)
                        {
                            string cpcxfsExeFullPath = Host.GetPluginsPath() + "cpcxfs.exe";
                            var dstCpcXfsExe = Path.GetTempPath() + new FileInfo(cpcxfsExeFullPath).Name;
                            var dstCpmDisksDef = Path.GetTempPath() + "cpmdisks.def";
                            if (!File.Exists(dstCpcXfsExe))
                            {
                                File.Copy(cpcxfsExeFullPath, dstCpcXfsExe, true);
                            }
                            if (!File.Exists(dstCpmDisksDef))
                            {
                                File.Copy(Host.GetPluginsPath() + "cpmdisks.def", dstCpmDisksDef, true);
                            }

                            arguments = "-o MLOD " + DSKFilename + " -f -b -p ";
                            arguments += "\"" + Path.GetTempPath() + item.AmsdosFilename + "\"";
                            if (Host.IsVerboseOutput())
                            {
                                Host.Log(cpcxfsExeFullPath + " " + arguments);
                            }

                            isOK = Host.StartProcess(cpcxfsExeFullPath, arguments, Path.GetTempPath(), true);

                            var lastErrorOutput = Host.GetLastErrorOutput();
                            var lastStandardOutput = Host.GetLastStandardOutput();

                            App.Controller.View.AppDoEvents();

                            if (File.Exists(dstCpcXfsExe))
                            {
                                File.Delete(dstCpcXfsExe);
                            }
                            if (File.Exists(dstCpmDisksDef))
                            {
                                File.Delete(dstCpmDisksDef);
                            }
                        }

                        if (File.Exists(Path.GetTempPath() + item.AmsdosFilename))
                        {
                            File.Delete(Path.GetTempPath() + item.AmsdosFilename);
                        }
                    }
                }

                int itemCount = 0;
                var content = String.Empty;
                foreach (Document.Item item in document.Items)
                {
                    PhactoryHost.Database.Resource depResource = Host.GetResource(item.ResourceID);
                    if (depResource == null)
                    {
                        Host.Log("Unknown resource identifier : " + item.ResourceID);
                        return false;
                    }

                    if (item.TrackLoaderItem)
                    {
                        // discsys test.dsk 1 sabotr2.ayc

                        FileInfo depFileInfo = Host.GetFileInfo(depResource);

                        content += itemCount + " \"" + depFileInfo.FullName + "\"\n";

                        itemCount++;
                    }
                }

                string fileListFilename = Host.GetFileInfo(resource).FullName;
                fileListFilename = fileListFilename.Replace(".cpcdsk", ".filelist");
                File.WriteAllText(fileListFilename, content);

                // discsys test.dsk 1 sabotr2.ayc

                var discsysExeFullPath = Host.GetPluginsPath() + "discsys.exe";
                var disksysArguments = "-l \"" + fileListFilename + "\" \"" + DSKFilename + "\"";

                if (Host.IsVerboseOutput())
                {
                    Host.Log(discsysExeFullPath + " " + disksysArguments);
                }
                isOK = Host.StartProcess(discsysExeFullPath, disksysArguments, fileInfo.DirectoryName, true);
            }
            else
            {
                string exeFullPath = Host.GetPluginsPath() + "ManageDSK.EXE";

                foreach (Document.Item item in document.Items)
                {
                    App.Controller.View.AppDoEvents();

                    PhactoryHost.Database.Resource depResource = Host.GetResource(item.ResourceID);
                    if (depResource == null)
                    {
                        Host.Log("Unknown resource identifier : " + item.ResourceID);
                        return false;
                    }

                    FileInfo depFileInfo = Host.GetFileInfo(depResource);

                    string arguments = "-L\"" + DSKFileInfo.FullName + "\" ";

                    arguments += "-I\"" + depFileInfo.FullName + "\"";

                    arguments += "/" + item.AmsdosFilename;

                    if (item.ItemType == Document.ItemType.Basic)
                    {
                        arguments += "/BAS/368";
                    }
                    else
                    {
                        arguments += "/BIN";

                        arguments += "/" + item.LoadAdress;
                        arguments += "/" + item.ExecAdress;
                    }

                    arguments += " ";

                    arguments += "-S\"" + DSKFileInfo.FullName + "\"";

                    if (Host.IsVerboseOutput())
                    {
                        Host.Log(exeFullPath + " " + arguments);
                    }

                    isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);
                    if (isOK)
                    {
                        if (Host.GetLastErrorOutput().ToUpper().IndexOf("ERREUR") != -1)
                        {
                            isOK = false;
                        }
                    }

                    if (item.CopyToWinAPEROMFolder)
                    {
                        string romFilename = item.AmsdosFilename; // depFileInfo.Name.Replace(depFileInfo.Extension, ".ROM");
                        string destROMFilename = Host.GetPluginsPath() + "ROM\\" + romFilename;
                        if (File.Exists(destROMFilename))
                        {
                            File.Delete(destROMFilename);
                        }
                        File.Copy(depFileInfo.FullName, destROMFilename);

                        App.Controller.View.AppDoEvents();
                    }
                }
            }

            if (document.GenerateHFE)
            {
                Host.Log(new FileInfo(DSKFilename.Replace(".dsk", ".hfe")).Name);

                string tempDir = "\\HFE_CONVERT";

                App.Controller.View.AppDoEvents();

                Directory.CreateDirectory(tempDir);

                App.Controller.View.AppDoEvents();

                string src = tempDir + "\\" + DSKFileInfo.Name;
                if (File.Exists(src))
                {
                    File.Delete(src);
                }
                File.Copy(DSKFileInfo.FullName, src);

                App.Controller.View.AppDoEvents();

                string hfeConverterFullPath = Host.GetPluginsPath() + "flopimage_convert.exe";

                string arguments = tempDir + " " + tempDir + " -HFE";

                if (Host.IsVerboseOutput())
                {
                    Host.Log(hfeConverterFullPath + " " + arguments);
                }

                string dst = "";

                isOK = Host.StartProcess(hfeConverterFullPath, arguments, fileInfo.DirectoryName, true);
                if (isOK)
                {
                    string dskhfe = src.Replace(".dsk", "_dsk.hfe");
                    dst = DSKFileInfo.FullName.Replace(".dsk", ".hfe");

                    App.Controller.View.AppDoEvents();

                    if (File.Exists(dst))
                    {
                        File.Delete(dst);
                    }
                    if (File.Exists(dskhfe))
                    {
                        File.Copy(dskhfe, dst);
                        File.Delete(dskhfe);
                    }

                    File.Delete(src);
                }

                App.Controller.View.AppDoEvents();

                Directory.Delete(tempDir, true);

                if (File.Exists(dst) == false)
                {
                    isOK = false;
                }
            }

            return isOK;
        }
    }
}