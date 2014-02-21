using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Project;

namespace CPCBigFile.Controller
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
            return "BigFile Compiler";
        }

        public string GetDescription()
        {
            return "Compile many files merged into a single big one";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpcbigfile", "BigFile file (*.cpcbigfile)", true));

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
            List<String> paddingFilenames = new List<String>();
            List<int> address = new List<int>();
            
            Document.Document document = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if ( document == null )
            {
                return false;
            }

            document.Expand();
            
            foreach (Document.File file in document.Files)
            {
                PhactoryHost.Database.Resource iResource = Host.GetResource( file.ResourceID );
                if (iResource == null)
                {
                    Host.Log("Unknown resource identifier : " + file.ResourceID);
                    return false;
                }

                FileInfo iFileInfo = Host.GetFileInfo( iResource );
                
                resourceFilenames.Add(iFileInfo.FullName);
                paddingFilenames.Add(file.Pad256 ? "true" : "false");
                address.Add((file.SetAddress==false)?0:file.Address);
            }

            var compiler = new Phactory.Modules.BigFile.Compiler.BigFileCompiler();
            
            string resourceRelativePathNoExt = resource.RelativePath;
            resourceRelativePathNoExt = resourceRelativePathNoExt.Replace(".cpcbigfile", "");

            App.Controller.View.AppDoEvents(); 
            
            string baseFilename = Host.MakeFullPath(resourceRelativePathNoExt);
            string headerFilename = Host.MakeFullPath(resourceRelativePathNoExt + ".H");
            if (!compiler.Compile(baseFilename, headerFilename, resourceFilenames, paddingFilenames, address, document.TruncateFiles, document.FilesInBank, document.BaseAddress))
            {
                return false;
            }

            return true;
        }
    }
}
