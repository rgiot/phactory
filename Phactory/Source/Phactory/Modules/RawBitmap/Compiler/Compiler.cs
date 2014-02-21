using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Project;

namespace CPCRawBitmap.Controller
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
            return "Raw Bitmap Compiler";
        }

        public string GetDescription()
        {
            return "Create Raw Bitmap (or group of bitmap) converted to linear raw data";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".rawbmp", "Raw Bitmap(s) file (*.rawbmp)", true));

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

            var compiler = new Phactory.Modules.RawBitmap.Compiler.RawCompiler();
            
            Document.Document tempDocument = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (!tempDocument.CompileInternal())
            {
                return false;
            }

            List<string> outputFilenames = new List<string>();
            
            foreach (Document.Item item in tempDocument.Items)
            {
                App.Controller.View.AppDoEvents(); 
                
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

                Document.ItemType type = item.Type;
                
                if (type == CPCRawBitmap.Document.ItemType.Raw)
                {
                    string outputTopFilename = outputFilename + ".rawBin";

                    if (!compiler.WriteRawBinFile(outputTopFilename,
                                                        item.IntermediateImage.Width,
                                                        item.IntermediateImage.Height,
                                                        item.IntermediateImage.Data,
                                                        false))
                    {
                        return false;
                    }
                }
                else if (type == CPCRawBitmap.Document.ItemType.VerticalRaw)
                {
                    string outputTopFilename = outputFilename + ".verticalRawBin";

                    if (!compiler.WriteRawBinFile(outputTopFilename,
                                                        item.IntermediateImage.Width,
                                                        item.IntermediateImage.Height,
                                                        item.IntermediateImage.Data,
                                                        true))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}
