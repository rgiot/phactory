using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCCloud.Controller
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
            return "Cloud Compiler";
        }

        public string GetDescription()
        {
            return "Create Cloud (or group of cloud) converted to CPC constraints";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpccloud", "Cloud(s) file (*.cpccloud)", true));

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

            var compiler = new Phactory.Modules.Cloud.Compiler.CloudCompiler();
            
            Document.Document tempDocument = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (!tempDocument.CompileInternal())
            {
                return false;
            }

            foreach (Document.Item item in tempDocument.Items)
            {
                PhactoryHost.Database.Resource resItem = Host.GetResource(item.ResourceID);
                if (resItem == null)
                {
                    Host.Log("Unknown resource identifier : " + item.ResourceID);
                    return false;
                }

                FileInfo resFileInfo = Host.GetFileInfo(resItem);
                string outputFilename = resFileInfo.FullName;

                if (Host.IsVerboseOutput())
                {
                    Host.Log(outputFilename);
                }

                string outputFilenameBin = outputFilename + ".bin";

                if (!compiler.WriteCloudBitmap(outputFilenameBin, item.CloudMaskData, item.CloudMaskWidth, item.CloudMaskHeight ))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
