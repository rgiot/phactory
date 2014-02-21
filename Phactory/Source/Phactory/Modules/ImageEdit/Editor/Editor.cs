using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace ImageEdit.Controller
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
            return "Image Editor";
        }

        public string GetDescription()
        {
            return "Used to edit image";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".bmp", "Bitmap image file (*.bmp)", false));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".gif", "Bitmap image file (*.gif)", false));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".jpg", "Jpeg image file (*.jpg)", false));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".png", "Bitmap image file (*.png)", false));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".tiff", "Bitmap image file (*.tiff)", false));

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
            return false;
        }

        public void OpenResource(Panel parentPanel, PhactoryHost.Database.Resource resource)
        {
            View.View view = new View.View(resource);
            view.Parent = parentPanel;
            view.Dock = System.Windows.Forms.DockStyle.Fill;

            viewBinding.Add(resource, new ResourceBinding(parentPanel, view));

            try
            {
                view.PictureBox.Image = Image.FromFile(Host.GetFileInfo(resource).FullName, true);
                view.PictureWidth.Text = "" + view.PictureBox.Image.Width + " Pixel(s)";
                view.PictureHeight.Text = "" + view.PictureBox.Image.Height + " Pixel(s)";
                view.PictureDepth.Text = "" + view.PictureBox.Image.PixelFormat.ToString();

                if (Host.IsVerboseOutput())
                {
                    Host.Log(resource.DisplayName + " loaded");
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Host.Log("Problem while loading " + resource.DisplayName);
            }

            view.RefreshTitle();

            view.IsReady = true;
        }

        public bool CloseResource(PhactoryHost.Database.Resource resource)
        {
            if (viewBinding.ContainsKey(resource))
            {
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
            return false;
        }

        public void SaveResource(PhactoryHost.Database.Resource resource)
        {            
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
