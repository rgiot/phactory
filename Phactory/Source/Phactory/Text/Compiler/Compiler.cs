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
            return "Text Compiler";
        }

        public string GetDescription()
        {
            return "Text converted to binary depending on a given charset";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpctext", "Text file (*.cpctext)", true));

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

            List<String> resourceFilenames = new List<String>();
            List<int> execPtrs = new List<int>();

            Document.Document document = Host.XMLRead<Document.Document>(fileInfo.FullName);

            string outputFilename = fileInfo.FullName + ".bin";

            if (File.Exists(outputFilename))
            {
                File.Delete(outputFilename);
            }

            var compiler = new Phactory.Text.Compiler.TextCompiler();

            int charsNotFound = 0;

            if (!compiler.Compile(outputFilename, document.Charset, document.Text, document.AppendEndOfText, out charsNotFound))
            {
                return false;
            }

            if (charsNotFound>0)
            {
                Plugin.ControllerCompiler.Host.Log("Warning : " + charsNotFound + " characters in text was/were not found in charset !");
            }

            return true;
        }
    }
}
