using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CPCPacker.View
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
        }

        private void AddListViewItem(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Plugin.ControllerEditor.Host.GetFileInfo(resource);

            ListViewItem item = new ListViewItem(resource.RelativePath, 0); // Relative Path
            if (fileInfo.Exists)
            {
                item.SubItems.Add(fileInfo.Length.ToString() + " bytes"); // File Size
            }
            else
            {
                item.SubItems.Add("Unknown size"); // File Size
            }

            lvFiles.Items.Add(item);
        }

        public void RefreshUI()
        {
            lvFiles.Items.Clear();

            switch(Document.PackerType)
            {
                default:
                case CPCPacker.Document.PackerType.Exomizer:
                    this.rbExomizer.Checked = true;
                    break;

                case CPCPacker.Document.PackerType.BitBuster:
                    this.rbBitBuster.Checked = true;
                    break;
            }

            List<Document.Item> toRemove = new List<Document.Item>();
            
            foreach (Document.Item item in Document.Items)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                if (resource == null)
                {
                    toRemove.Add(item);
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + item.ResourceID);
                }
                else
                {
                    AddListViewItem(resource);
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
        }

        public bool IsModified()
        {
            return modified;
        }

        public void SetModified(bool modified)
        {
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
            List<PhactoryHost.Database.Resource> resources = Plugin.ControllerEditor.Host.ShowResourceSelector(null, true);
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
                return;
            }

            int index = lvFiles.SelectedIndices[ 0 ];
            if (index >= 0)
            {
                CPCPacker.Document.Item item = Document.Items[ index ];

                PhactoryHost.Database.Resource bmpResource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                
                string filename = Plugin.ControllerEditor.Host.GetFileInfo( bmpResource ).FullName;
            }
        }

        private CPCPacker.Document.Item GetSelectedItem()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return null;
            }

            int index = lvFiles.SelectedIndices[0];

            CPCPacker.Document.Item item = Document.Items[index];
            return item;
        }

        private void rbExomizer_CheckedChanged(object sender, EventArgs e)
        {
            if ( IsReady==false )
            {
                return;

            }
            if ( rbExomizer.Checked )
            {
                SetModified(true);
                this.Document.PackerType = CPCPacker.Document.PackerType.Exomizer;
            }
        }

        private void rbBitBuster_CheckedChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;

            }
            if (rbBitBuster.Checked)
            {
                SetModified(true);
                this.Document.PackerType = CPCPacker.Document.PackerType.BitBuster;
            }
        }
    }
}
