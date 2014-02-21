using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CPCCloud.View
{
    public partial class View : UserControl
    {
        public PhactoryHost.Database.Resource Resource;
        public Document.Document Document = new Document.Document();

        private bool modified;
        public bool IsReady = false;

        public View(PhactoryHost.Database.Resource resource)
        {
            InitializeComponent();

            Resource = resource;
            modified = false;

            lvFiles.Clear();
            lvFiles.Columns.Add("Filename", 130);
            lvFiles.Columns.Add("Size", 70);
            lvFiles.Columns.Add("Width", 70);
            lvFiles.Columns.Add("Height", 70);
            lvFiles.Columns.Add("Created", 120);
            lvFiles.Columns.Add("Last Modified", 120);

            RefreshProperties();
        }

        private List<string> GetFilterExtension()
        {
            List<string> filterExtension = new List<string>();
            filterExtension.Add(".bmp");
            filterExtension.Add(".gif");
            filterExtension.Add(".jpg");
            filterExtension.Add(".png");
            filterExtension.Add(".tiff");

            return filterExtension;
        }

        private ListViewItem AddListViewItem(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Plugin.ControllerEditor.Host.GetFileInfo(resource);
            if ( fileInfo.Exists == false )
            {
                return null;
            }

            ListViewItem item = new ListViewItem(resource.RelativePath, 0); // Relative Path
            item.SubItems.Add(((fileInfo.Length / 1024) + 1).ToString() + " Kb"); // File Size

            Image image = Image.FromFile(fileInfo.FullName);
            item.SubItems.Add("" + image.Width); // Width
            item.SubItems.Add("" + image.Height); // Height

            item.SubItems.Add(fileInfo.CreationTime.ToString()); // Created
            item.SubItems.Add(fileInfo.LastWriteTime.ToString()); // Modified

            lvFiles.Items.Add(item);

            SetModified(true);

            return item;
        }

        public void RefreshUI()
        {
            var selectedItem = GetSelectedItem();

            ListViewItem selLvItem = null;

            var toRemove = new List<Document.Item>();

            lvFiles.Items.Clear();            
            foreach (Document.Item item in Document.Items)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                if (resource == null)
                {
                    toRemove.Add(item);
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + item.ResourceID);
                    continue;
                }

                ListViewItem lvItem = AddListViewItem(resource);
                if (lvItem == null)
                {
                    toRemove.Add(item);
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + item.ResourceID);
                    continue;
                } 

                if (item == selectedItem)
                {
                    selLvItem = lvItem;
                }
            }

            if (toRemove.Count > 0)
            {
                SetModified(true);
                MessageBox.Show("" + toRemove.Count + " resources were found as referenced but missing in the project !\n\nThese references have been automatically removed.", "Missing references", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            foreach (Document.Item removeItem in toRemove)
            {
                Document.Items.Remove(removeItem);
            }

            Document.CompileInternal();

            if (selLvItem != null)
            {
                selLvItem.Selected = true;
            }
        }

        public bool IsModified()
        {
            return modified;
        }

        public void SetModified(bool modified)
        {
            if (IsReady == false)
            {
                return;
            }

            if (this.modified != modified)
            {
                this.modified = modified;

                RefreshTitle();
            }
        }

        public delegate void RefreshTitleDelegate();
        public void RefreshTitle()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new RefreshTitleDelegate(RefreshTitle));
                return;
            }

            var parentPanel = this.Parent as Panel;
            var parentForm = parentPanel.Parent as Form;

            parentForm.Text = Resource.DisplayName;
            if (this.modified)
            {
                parentForm.Text += "*";
            }

            string toolTipText = Plugin.ControllerEditor.Host.GetFileInfo(Resource).FullName;
            if (this.modified)
            {
                toolTipText += "*";
            }
            Plugin.ControllerEditor.Host.SetToolTipText(parentPanel, toolTipText);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var resources = Plugin.ControllerEditor.Host.ShowResourceSelector(null, true, GetFilterExtension());
            if (resources != null)
            {
                foreach (PhactoryHost.Database.Resource resource in resources)
                {
                    Document.Item item = new Document.Item();
                    item.ResourceID = resource.Id;
                    
                    Document.Items.Add(item);
                }

                SetModified(true);

                RefreshUI();
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var indices = new List<int>();

            foreach (int index in lvFiles.SelectedIndices)
            {
                Document.Item item = Document.Items[index];

                List<PhactoryHost.Database.Resource> otherReferencingResources = Plugin.ControllerEditor.Host.IsOutputResourcesReferencedByOtherResources(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id));
                if ( otherReferencingResources.Count > 0 )
                {
                    Plugin.ControllerEditor.Host.ShowCantRemoveBecauseOtherResources(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id), otherReferencingResources);
                    return;
                }
                else
                {
                    indices.Add(index);
                }
            }

            // reverse delete
            for (int iIndex = indices.Count - 1; iIndex >= 0; iIndex--)
            {
                int index = indices[iIndex];

                Document.Items.RemoveAt(index);
                lvFiles.Items.RemoveAt(index);
            }

            if (indices.Count > 0)
            {
                SetModified(true);
            }
        }

        private void RefreshProperties()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                ShowCloudMaskPreview(-1);
                return;
            }

            int index = lvFiles.SelectedIndices[0];
            if (index >= 0)
            {
                var item = Document.Items[index];

                ShowCloudMaskPreview(item.ResourceID);

                IsReady = true;
            }
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshProperties();
        }

        private void ShowCloudMaskPreview(int cloudMaskResourceID)
        {
            if (cloudMaskResourceID == -1)
            {
                CloudMaskPreview.Image = null;
            }
            else
            {
                PhactoryHost.Database.Resource bmpMaskResource = Plugin.ControllerEditor.Host.GetResource(cloudMaskResourceID);

                string filename = Plugin.ControllerEditor.Host.GetFileInfo(bmpMaskResource).FullName;
                CloudMaskPreview.Image = Image.FromFile(filename);
            }
        }

        private void UpdateData()
        {
            if (IsReady == false)
            {
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            RefreshUI();

            SetModified(true);
        }
                    
        private CPCCloud.Document.Item GetSelectedItem()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return null;
            }

            int index = lvFiles.SelectedIndices[0];

            CPCCloud.Document.Item item = Document.Items[index];
            return item;
        }

        private void BrowseCloudMaskBitmap_Click(object sender, EventArgs e)
        {
            CPCCloud.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            List<PhactoryHost.Database.Resource> cloudMaskResource = Plugin.ControllerEditor.Host.ShowResourceSelector(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), false, GetFilterExtension());
            if (cloudMaskResource != null)
            {
                item.ResourceID = cloudMaskResource[0].Id;

                ShowCloudMaskPreview(item.ResourceID);

                SetModified(true);
            }     
        }
    }
}
