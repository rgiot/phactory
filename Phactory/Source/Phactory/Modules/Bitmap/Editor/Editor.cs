using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCBitmap.Controller
{
    public class ResourceBinding
    {
        public Panel Panel;
        public View.View View;

        public ResourceBinding()
        {
        }

        public ResourceBinding(Panel panel, View.View view)
        {
            this.Panel = panel;
            this.View = view;
        }
    }

    public class Editor : PhactoryHost.EditorPlugin
    {
        private Dictionary<PhactoryHost.Database.Resource, ResourceBinding> viewBinding = new Dictionary<PhactoryHost.Database.Resource, ResourceBinding>();

        public PhactoryHost.Host Host;

        public Editor()
        {
            Plugin.ControllerEditor = this;
        }

        public bool IsDefaultPluginForUnknownTypes()
        {
            return false;
        }

        public string GetDefaultExtensionForNewResource()
        {
            return String.Empty;
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
            return "Bitmap Editor";
        }

        public string GetDescription()
        {
            return "Bitmap (or group of bitmap) converted to CPC constraints";
        }

        public string GetVersion()
        {
            return "2.1";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpcbitmap", "Bitmap(s) file (*.cpcbitmap)", true));

            return extensions;
        }

        public void SetLine(PhactoryHost.Database.Resource resource, int line)
        {
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

        public bool CreateEmptyResource(PhactoryHost.Database.Resource resource)
        {
            return Host.XMLWrite(Host.GetFileInfo(resource).FullName, new Document.Document() );
        }

        public void OpenResource(Panel parentPanel, PhactoryHost.Database.Resource resource)
        {
            View.View view = new View.View(resource);
            view.Parent = parentPanel;
            view.Dock = System.Windows.Forms.DockStyle.Fill;

            ResourceBinding resourceBinding = new ResourceBinding(parentPanel, view);
            viewBinding.Add(resource, resourceBinding);

            resourceBinding.View.Document = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (resourceBinding.View.Document is Document.Document)
            {
                if (Host.IsVerboseOutput())
                {
                    Host.Log(resource.DisplayName + " loaded");
                }
            }
            else
            {
                Host.Log("Problem while loading " + resource.DisplayName);
            }

            view.RefreshUI();
            view.RefreshTitle();

            view.IsReady = true;
        }

        public bool CloseResource(PhactoryHost.Database.Resource resource)
        {
            if (viewBinding.ContainsKey(resource))
            {
                if (IsResourceModified(resource))
                {
                    switch (MessageBox.Show("The file has been modified. Save changes ?", "Confirm file close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            SaveResource(resource);
                            break;
                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            return false;
                    }
                } 
                
                viewBinding.Remove(resource);
            }

            return true;
        }

        public bool IsResourceOpened(PhactoryHost.Database.Resource resource)
        {
            try
            {
                View.View view = viewBinding[resource].View;
                if (view != null)
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public bool IsResourceModified(PhactoryHost.Database.Resource resource)
        {
            try
            {
                View.View view = viewBinding[resource].View;
                if (view != null)
                {
                    if (view.IsModified())
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        public void SaveResource(PhactoryHost.Database.Resource resource)
        {
            bool writeOK = Host.XMLWrite(Host.GetFileInfo(resource).FullName, viewBinding[resource].View.Document);

            if (writeOK)
            {
                if (Host.IsVerboseOutput())
                {
                    Host.Log(resource.DisplayName + " written");
                }

                Host.RefreshOutput(resource);
                List<PhactoryHost.Database.Resource> dependentResources = new List<PhactoryHost.Database.Resource>();
                foreach (Document.Item item in viewBinding[resource].View.Document.Items)
                {
                    dependentResources.Add(Plugin.ControllerEditor.Host.GetResource(item.ResourceID));
                }
                Host.RefreshDependencies(resource, dependentResources);

                viewBinding[resource].View.SetModified(false);
            }
            else
            {
                Host.Log("Problem while writing " + resource.DisplayName);
            }
        }

        public void RefreshViewTitle(PhactoryHost.Database.Resource resource)
        {
            View.View view = viewBinding[resource].View;

            view.RefreshTitle();
        }

        public void OnResourceDeleted(PhactoryHost.Database.Resource resource)
        {
        }

        public void OnResourceChanged(PhactoryHost.Database.Resource resource)
        {
        }

        public void OnResourceRenamed(PhactoryHost.Database.Resource resource)
        {
        }
    }
}
