using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace CPCRawBitmap.View
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
            lvFiles.Columns.Add("Filename", 200);
            lvFiles.Columns.Add("Size", 100);
            lvFiles.Columns.Add("Width", 100);
            lvFiles.Columns.Add("Height", 100);
            lvFiles.Columns.Add("Created", 120);
            lvFiles.Columns.Add("Last Modified", 120);
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
            CPCRawBitmap.Document.Item selectedItem = GetSelectedItem();

            ListViewItem selLvItem = null;

            List<Document.Item> toRemove = new List<Document.Item>();
            lvFiles.Items.Clear();            
            foreach (Document.Item item in Document.Items)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                if (resource == null)
                {
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + item.ResourceID);
                    toRemove.Add(item);
                    continue;
                } 
                
                ListViewItem lvItem = AddListViewItem(resource);

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

            Panel parentPanel = this.Parent as Panel;
            Form parentForm = parentPanel.Parent as Form;

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
            List<PhactoryHost.Database.Resource> resources = Plugin.ControllerEditor.Host.ShowResourceSelector(null, true, GetFilterExtension());
            if (resources != null)
            {
                foreach (PhactoryHost.Database.Resource resource in resources)
                {
                    Document.Item item = new Document.Item();
                    item.ResourceID = resource.Id;

                    item.Type = CPCRawBitmap.Document.ItemType.Raw;

                    Document.Items.Add(item);
                }
            }

            SetModified(true);

            RefreshUI();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();

            foreach (int index in lvFiles.SelectedIndices)
            {
                Document.Item item = Document.Items[index];

                List<PhactoryHost.Database.Resource> otherReferencingResources = Plugin.ControllerEditor.Host.IsOutputResourcesReferencedByOtherResources(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id));
                if (otherReferencingResources.Count > 0)
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

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                curSelIndexLabel.Text = "";                
                Preview.Image = null;
                GroupItemProperties.Enabled = false;
                Information.Text = "";
                
                return;
            }

            int index = lvFiles.SelectedIndices[ 0 ];
            if (index < 0)
            {
                GroupItemProperties.Enabled = false;
            }
            else
            {
                curSelIndexLabel.Text = "Selection index: " + (index+1);

                GroupItemProperties.Enabled = true;
                
                CPCRawBitmap.Document.Item item = Document.Items[index];

                PhactoryHost.Database.Resource bmpResource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                
                string filename = Plugin.ControllerEditor.Host.GetFileInfo( bmpResource ).FullName;
                Preview.Image = Image.FromFile(filename);

                IsReady = false;

                CPCRawBitmap.Document.ItemType type = item.Type;
                
                switch( type )
                {
                    case CPCRawBitmap.Document.ItemType.Raw:
                        Raw.Checked = true;
                        break;

                    case CPCRawBitmap.Document.ItemType.VerticalRaw:
                        VerticalRaw.Checked = true;
                        break;
                }

                IsReady = true;
            }
        }

        private void UpdateData()
        {
            if (IsReady == false)
            {
                return;
            }

            CPCRawBitmap.Document.ItemType type = CPCRawBitmap.Document.ItemType.Raw;

            if (Raw.Checked)
            {
                type = CPCRawBitmap.Document.ItemType.Raw;
            }
            else if (VerticalRaw.Checked)
            {
                type = CPCRawBitmap.Document.ItemType.VerticalRaw;
            }
            
            CPCRawBitmap.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.Type = type;

            RefreshUI();

            SetModified(true);
        }
                    
        private void ItemType_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private CPCRawBitmap.Document.Item GetSelectedItem()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return null;
            }

            int index = lvFiles.SelectedIndices[0];

            CPCRawBitmap.Document.Item item = Document.Items[index];
            return item;
        }

        private void CompileInternally_Click(object sender, EventArgs e)
        {
            Document.CompileInternal();

            if (lvFiles.Items.Count > 0)
            {
                CPCRawBitmap.Document.Item item = Document.Items[0];
                
                Information.Text = "" + item.IntermediateImage.IndiceCount + " indices found";
            }
            else
            {
                Information.Text = "";
            }
        }        
    }
}
