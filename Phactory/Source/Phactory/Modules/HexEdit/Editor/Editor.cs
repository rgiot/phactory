using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace HexEdit.Controller
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
            Plugin.Controller = this;
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
            return "Hexadecimal Editor";
        }

        public string GetDescription()
        {
            return "Used to edit binary files";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public bool IsDefaultPluginForUnknownTypes()
        {
            return true;
        }

        public string GetDefaultExtensionForNewResource()
        {
            return String.Empty;
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".bin", "Binary file (*.bin)", false));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".raw", "Raw binary file (*.raw)", false));

            return extensions;
        }

        public bool CreateEmptyResource(PhactoryHost.Database.Resource resource)
        {
            File.CreateText(Host.GetFileInfo(resource).FullName).Close();

            return true;
        }

        public void OpenResource(Panel parentPanel, PhactoryHost.Database.Resource resource)
        {
            View.View view = new View.View(resource);
            view.Parent = parentPanel;
            view.Dock = System.Windows.Forms.DockStyle.Fill;

            viewBinding.Add(resource, new ResourceBinding(parentPanel, view));

            view.RefreshContent();
            view.RefreshTitle();

            if (Host.IsVerboseOutput())
            {
                Host.Log(resource.DisplayName + " loaded");
            }

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
            View.View view = viewBinding[resource].View;

            return view.IsModified();
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
            View.View view = viewBinding[resource].View;

            FileInfo fileInfo = Host.GetFileInfo(resource);
            string fileContent = File.ReadAllText(fileInfo.FullName);

            view.RefreshContent();

            Host.Log(resource.DisplayName + " refreshed");
        }

        public void OnResourceRenamed(PhactoryHost.Database.Resource resource)
        {
        }

        public void SaveResource(PhactoryHost.Database.Resource resource)
        {
            if (viewBinding.ContainsKey(resource))
            {
                /*string filename = Host.GetFilename(resourceId);

                View.View view = viewBinding[resourceId].View;

                StreamWriter writer = new StreamWriter(filename);
                writer.Write(view.TextEditor.Text);
                writer.Close();

                Host.RefreshOutput(resource);

                view.SetModified(false);

                if (App.Controller.UserConfig.VerboseOutput)
                {
                    Host.Log(new FileInfo(filename).Name + " written");
                 
                }*/
            }
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
    }
}
