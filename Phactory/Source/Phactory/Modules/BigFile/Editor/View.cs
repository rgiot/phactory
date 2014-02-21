using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CPCBigFile.View
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
            lvFiles.Columns.Add("Address", 50);
            lvFiles.Columns.Add("Length", 50);
            lvFiles.Columns.Add("Length (Hex)", 80);
            lvFiles.Columns.Add("Is External From", 200);
        }

        public void RefreshUI()
        {
            List<Document.File> toRemove = new List<Document.File>();

            BaseAddress.Text = "0x" + String.Format("{0:X4}", Document.BaseAddress);
            this.TruncateFile.Checked = Document.TruncateFiles;
            this.FilesInBank.Checked = Document.FilesInBank;

            lvFiles.Items.Clear();            
            foreach (Document.File file in Document.Files)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(file.ResourceID);
                if (resource == null)
                {
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + file.ResourceID);
                    toRemove.Add(file);
                    continue;
                }

                FileInfo fileInfo = Plugin.ControllerEditor.Host.GetFileInfo(resource);

                ListViewItem item = new ListViewItem(resource.RelativePath, 0); // Relative Path

                item.Checked = file.Pad256;

                if (file.SetAddress)
                {
                    item.SubItems.Add("0x" + String.Format("{0:X4}", file.Address)); // Address (Base 16)
                    item.BackColor = Color.PaleTurquoise;
                }
                else
                {
                    item.SubItems.Add("--");
                }

                if (fileInfo.Exists)
                {
                    item.SubItems.Add("" + fileInfo.Length); // Length (Base 10)
                    item.SubItems.Add("0x" + String.Format("{0:X4}", fileInfo.Length)); // Length (Base 16)
                }
                else
                {
                    item.SubItems.Add("need compile"); // Length (Base 10)
                    item.SubItems.Add("need compile"); // Length (Base 16)
                }
                
                // Is External From
                if (file.ResourceCPCBigFileID != -1)
                {
                    item.SubItems.Add(Plugin.ControllerEditor.Host.GetResource(file.ResourceCPCBigFileID).DisplayName); // IsInclude

                    item.BackColor = Color.PaleTurquoise;
                }
                else
                {
                    item.SubItems.Add("");
                }

                lvFiles.Items.Add(item);
            }

            if (toRemove.Count > 0)
            {
                SetModified(true);
                MessageBox.Show("" + toRemove.Count + " resources were found as referenced but missing in the project !\n\nThese references have been automatically removed.", "Missing references", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            foreach (Document.File removeFile in toRemove)
            {
                Document.Files.Remove(removeFile);
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
                    var file = new CPCBigFile.Document.File(resource.Id);
                    Document.Files.Add(file);
                }

                Document.Expand();

                SetModified(true);
                RefreshUI();
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            foreach (int i in lvFiles.SelectedIndices)
            {
                CPCBigFile.Document.File item = Document.Files[i];
                if (item.ResourceCPCBigFileID != -1)
                {
                    MessageBox.Show("Can not modify external items", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            List<int> indices = new List<int>();
            
            foreach (int index in lvFiles.SelectedIndices)
            {
                Document.File file = Document.Files[index];

                List<PhactoryHost.Database.Resource> otherReferencingResources = Plugin.ControllerEditor.Host.IsOutputResourcesReferencedByOtherResources(Plugin.ControllerEditor.Host.GetResource(file.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id));
                if (otherReferencingResources.Count > 0)
                {
                    Plugin.ControllerEditor.Host.ShowCantRemoveBecauseOtherResources(Plugin.ControllerEditor.Host.GetResource(file.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id), otherReferencingResources);
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

                Document.Files.RemoveAt(index);
                lvFiles.Items.RemoveAt(index);
            }

            if (indices.Count > 0)
            {
                SetModified(true);
                RefreshUI();
            }
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                this.cbItemAddress.Enabled = false;
                this.ItemAddress.Enabled = false;
                return;
            }

            int index = lvFiles.SelectedIndices[ 0 ];
            if (index >= 0)
            {
                IsReady = false;

                CPCBigFile.Document.File item = Document.Files[ index ];

                this.cbItemAddress.Enabled = true;
                this.cbItemAddress.Checked = item.SetAddress;
                this.ItemAddress.Text = "0x" + String.Format("{0:X4}", item.Address);
                ItemAddress.Enabled = cbItemAddress.Checked;

                IsReady = true;
            }
        }

        private CPCBigFile.Document.File GetSelectedItem()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return null;
            }

            int index = lvFiles.SelectedIndices[0];

            CPCBigFile.Document.File item = Document.Files[index];
            return item;
        }

        private void MoveUp_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return;
            }

            foreach (int i in lvFiles.SelectedIndices)
            {
                CPCBigFile.Document.File item = Document.Files[i];
                if (item.ResourceCPCBigFileID != -1)
                {
                    MessageBox.Show("Can not modify external items", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            int index = lvFiles.SelectedIndices[0];
            if (index >= 1)
            {
                CPCBigFile.Document.File item = Document.Files[index];

                Document.Files.RemoveAt(index);
                Document.Files.Insert( index - 1, item );

                SetModified(true);
                RefreshUI();

                lvFiles.SelectedIndices.Add(index - 1);
            }
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return;
            }

            foreach (int i in lvFiles.SelectedIndices)
            {
                CPCBigFile.Document.File item = Document.Files[i];
                if (item.ResourceCPCBigFileID != -1)
                {
                    MessageBox.Show("Can not modify external items", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            int index = lvFiles.SelectedIndices[0];
            if (index < (lvFiles.Items.Count - 1) )
            {
                CPCBigFile.Document.File item = Document.Files[index];

                Document.Files.RemoveAt(index);
                Document.Files.Insert(index + 1, item);

                SetModified(true);
                RefreshUI();

                lvFiles.SelectedIndices.Add(index + 1);
            }
        }

        private void TruncateFile_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsReady)
            {
                return;
            }

            Document.TruncateFiles = this.TruncateFile.Checked;
            SetModified(true);
        }

        private int GetInt(string text)
        {
            try
            {
                return int.Parse(text.Replace("0x", ""), NumberStyles.AllowHexSpecifier);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private void BaseAddress_TextChanged(object sender, EventArgs e)
        {
            if (!IsReady)
            {
                return;
            }

            Document.BaseAddress = GetInt(BaseAddress.Text);

            SetModified(true);
        }

        private void filesInBank_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsReady)
            {
                return;
            }

            Document.FilesInBank = this.FilesInBank.Checked;
            SetModified(true);
        }

        private void lvFiles_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!IsReady)
            {
                return;
            }

            Document.Files[e.Item.Index].Pad256 = e.Item.Checked;

            SetModified(true);
        }

        private void cbItemAddress_CheckedChanged(object sender, EventArgs e)
        {
            this.ItemAddress.Enabled = this.cbItemAddress.Checked;

            if (!IsReady)
            {
                return;
            }

            if (GetSelectedItem() != null)
            {
                GetSelectedItem().SetAddress = this.cbItemAddress.Checked;
            }

            SetModified(true);
        }

        private void ItemAddress_TextChanged(object sender, EventArgs e)
        {
            if (!IsReady)
            {
                return;
            }

            if ( GetSelectedItem() != null )
            {
                GetSelectedItem().Address = GetInt(this.ItemAddress.Text);
            }

            SetModified(true);
        }
    }
}
