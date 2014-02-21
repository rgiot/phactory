using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace CPCDSK.Controller
{
    public class Runner : PhactoryHost.RunnerPlugin
    {
        public PhactoryHost.Host Host;

        public Runner()
        {
            Plugin.ControllerRunner = this;
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
            return "DSK Runner";
        }

        public string GetDescription()
        {
            return "Allow to run DSK files";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpcdsk", "DSK container file (*.cpcdsk)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".dsk", "DSK file (*.dsk)", true));

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

        public bool Run(PhactoryHost.Database.Resource resource)
        {
            PhactoryHost.Database.Resource parentResource = Host.GetResource(resource.IdDependencies[0]);

            Document.Document document = Host.XMLRead<Document.Document>(Host.GetFileInfo(parentResource).FullName);

            string fileToExecute = "";
            foreach (Document.Item item in document.Items)
            {
                if (item.ExecAdress != 0)
                {
                    fileToExecute = item.AmsdosFilename;
                }
            }
            
            string DSKFilename = Host.GetFileInfo(resource).FullName;
            DSKFilename = DSKFilename.Replace(".cpcdsk", ".dsk");
            
            FileInfo DSKFileInfo = new FileInfo(DSKFilename);

            string WinAPEFullPath = Host.GetPluginsPath() + "WinAPE.exe";

            string arguments = "\"" + DSKFileInfo.FullName + "\" /A";

            if (Host.IsVerboseOutput())
            {
                Host.Log(WinAPEFullPath + " " + arguments);
            }

            return Host.StartAndWaitAfterProcess(WinAPEFullPath, arguments, DSKFileInfo.DirectoryName);
        }
    }
}
