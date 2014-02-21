using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace CPCDSK.View
{
    public partial class View : UserControl
    {
        public PhactoryHost.Database.Resource Resource;
        public Document.Document Document = new Document.Document();

        private bool modified;
        public bool IsReady = false;
        private bool isControlUpdating = false;

        public View(PhactoryHost.Database.Resource resource)
        {
            InitializeComponent();

            Resource = resource;
            modified = false;

            lvFiles.Clear();
            lvFiles.Columns.Add("Filename", 100);
            lvFiles.Columns.Add("Filename (AMSDOS 8.3)", 140);
            lvFiles.Columns.Add("Type", 80);
            lvFiles.Columns.Add("Length in Kb", 80);
            lvFiles.Columns.Add("Length (Base 10)", 100);
            lvFiles.Columns.Add("Length (Base 16)", 100);
            lvFiles.Columns.Add("Load Address", 100);
            lvFiles.Columns.Add("Execution Address", 100);

            RefreshMoveButtonStates();
        }

        private void RefreshListViewItem( ListViewItem listViewItem, Document.Item item )
        {
            listViewItem.SubItems[1].Text = item.AmsdosFilename;
            
            if (item.ItemType == CPCDSK.Document.ItemType.Binary)
            {
                listViewItem.SubItems[2].Text = "BINARY";
            }
            else
            {
                listViewItem.SubItems[2].Text = "BASIC";
            }

            listViewItem.SubItems[6].Text = "0x" + String.Format("{0:X4}", item.LoadAdress); // Load Adress
            listViewItem.SubItems[7].Text = "0x" + String.Format("{0:X4}", item.ExecAdress); // Execution Adress
        }

        public void RefreshUI()
        {
            ResetItemPropertiesUI();
            gbFileProperties.Enabled = false;

            cbHFE.Checked = Document.GenerateHFE;
            cbCopyToWinAPERomFolder.Checked = false;
            cbTrackLoaderDisc.Checked = Document.TrackLoaderDisc;
            cbTrackLoaderItem.Checked = false;
            
            lvFiles.Items.Clear();

            List<Document.Item> toRemove = new List<Document.Item>();
            
            foreach (Document.Item item in Document.Items)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                if (resource == null)
                {
                    toRemove.Add(item);
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + item.ResourceID);
                    continue;
                }

                FileInfo fileInfo = Plugin.ControllerEditor.Host.GetFileInfo(resource);

                ListViewItem listViewItem = new ListViewItem(fileInfo.Name, 0); // Filename

                listViewItem.SubItems.Add(""); // Amsdos Filename

                if (item.TrackLoaderItem)
                {
                    listViewItem.BackColor = Color.PaleTurquoise;
                }
                
                listViewItem.SubItems.Add(""); // Type

                long length = 0;
                if (fileInfo.Exists)
                {
                    length = fileInfo.Length;
                }

                long lengthKb = 1 + (length / 1024);
                listViewItem.SubItems.Add("" + lengthKb + " Kb"); // Lenght in Kb

                listViewItem.SubItems.Add(length.ToString()); // Length Base 10
                listViewItem.SubItems.Add("0x" + String.Format("{0:X4}", length)); // Length Base 16

                listViewItem.SubItems.Add(""); // Load Adress
                listViewItem.SubItems.Add(""); // Execution Adress

                RefreshListViewItem(listViewItem, item);

                listViewItem.Tag = item;

                lvFiles.Items.Add(listViewItem);
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

            if ( lvFiles.SelectedItems.Count==0)
            {
                if (lvFiles.Items.Count > 0)
                {
                    lvFiles.Items[0].Selected = true;
                }
            }
            lvFiles.Focus();

            this.NbFiles.Text = "" + lvFiles.Items.Count + " File(s)";
        }

        public bool IsModified()
        {
            return modified;
        }

        public void SetModified(bool modified)
        {
            if (isControlUpdating)
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
            List<PhactoryHost.Database.Resource> resources = Plugin.ControllerEditor.Host.ShowResourceSelector(null, true);
            if (resources != null)
            {
                foreach (PhactoryHost.Database.Resource resource in resources)
                {
                    Document.Item item = new Document.Item();

                    string amsdosFilename = resource.DisplayName.ToUpper();
                    if (amsdosFilename.Length > 11)
                    {
                        amsdosFilename.Substring(0, 11);
                    }
                    item.AmsdosFilename = amsdosFilename;

                    item.ResourceID = resource.Id;
                    item.LoadAdress = 0x4000;
                    item.ExecAdress = 0x4000;

                    if (Document.TrackLoaderDisc)
                    {
                        item.TrackLoaderItem = true;
                    }
                    else
                    {
                        item.TrackLoaderItem = false;
                    }
                    
                    Document.Items.Add(item);
                }

                SetModified(true);

                RefreshUI();
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            bool isModified = false;

            isControlUpdating = true;

            while ( lvFiles.SelectedIndices.Count != 0 )
            {
                int index = lvFiles.SelectedIndices[0];

                Document.Item item = Document.Items[index];

                List<PhactoryHost.Database.Resource> otherReferencingResources = Plugin.ControllerEditor.Host.IsOutputResourcesReferencedByOtherResources(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id));
                if (otherReferencingResources.Count > 0)
                {
                    Plugin.ControllerEditor.Host.ShowCantRemoveBecauseOtherResources(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id), otherReferencingResources);
                    isControlUpdating = false;
                    return;
                }
                else
                {
                    Document.Items.RemoveAt(index);
                    lvFiles.Items.RemoveAt(index);

                    isModified = true;
                }
            }

            isControlUpdating = false;

            if (isModified)
            {
                SetModified(true);
            }
        }

        private void ResetItemPropertiesUI()
        {
            lvFiles.SelectedItems.Clear();

            cbType.SelectedIndex = -1;

            LoadAdress.Text = "";
            ExecAdress.Text = "";
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isControlUpdating)
            {
                return;
            }

            RefreshMoveButtonStates();

            if (lvFiles.SelectedIndices.Count == 0)
            {
                ResetItemPropertiesUI();
                
                gbFileProperties.Enabled = false;

                return;
            }

            isControlUpdating = true;

            gbFileProperties.Enabled = true;

            int index = lvFiles.SelectedIndices[ 0 ];
            if (index >= 0)
            {
                CPCDSK.Document.Item item = Document.Items[ index ];

                AmsdosFilename.Text = item.AmsdosFilename;

                if (item.ItemType == CPCDSK.Document.ItemType.Binary)
                {
                    cbType.SelectedIndex = 0;
                }
                else
                {
                    cbType.SelectedIndex = 1;
                }

                LoadAdress.Text = "0x" + String.Format("{0:X4}", item.LoadAdress); // Load Adress
                ExecAdress.Text = "0x" + String.Format("{0:X4}", item.ExecAdress); // Execution Adress

                cbCopyToWinAPERomFolder.Checked = item.CopyToWinAPEROMFolder;
                cbTrackLoaderItem.Checked = item.TrackLoaderItem;
            }

            isControlUpdating = false;
        }

        private CPCDSK.Document.Item GetSelectedItem()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return null;
            }

            int index = lvFiles.SelectedIndices[0];

            CPCDSK.Document.Item item = Document.Items[index];
            return item;
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

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CPCDSK.Document.Item item = GetSelectedItem();

            if (item == null)
            {
                return;
            }

            if (cbType.SelectedIndex == 0)
            {
                item.ItemType = CPCDSK.Document.ItemType.Binary;
            }
            else
            {
                item.ItemType = CPCDSK.Document.ItemType.Basic;
            }

            RefreshListViewItem(lvFiles.SelectedItems[0], item);
            
            SetModified(true);
        }

        private void AmsdosFilename_TextChanged(object sender, EventArgs e)
        {
            CPCDSK.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.AmsdosFilename = AmsdosFilename.Text.ToUpper();

            RefreshListViewItem(lvFiles.SelectedItems[0], item);

            SetModified(true);
        }

        private void LoadAdress_TextChanged(object sender, EventArgs e)
        {
            CPCDSK.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.LoadAdress = GetInt(LoadAdress.Text);

            RefreshListViewItem(lvFiles.SelectedItems[0], item);

            SetModified(true);
        }

        private void ExecAdress_TextChanged(object sender, EventArgs e)
        {
            CPCDSK.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.ExecAdress = GetInt(ExecAdress.Text);

            RefreshListViewItem(lvFiles.SelectedItems[0], item);

            SetModified(true);
        }

        private void cbHFE_CheckedChanged(object sender, EventArgs e)
        {
            if (lvFiles.Items.Count == 0)
            {
                return;
            }

            Document.GenerateHFE = cbHFE.Checked;

            SetModified(true);
        }

        private void cbCopyToWinAPERomFolder_CheckedChanged(object sender, EventArgs e)
        {
            CPCDSK.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.CopyToWinAPEROMFolder = cbCopyToWinAPERomFolder.Checked;

            RefreshListViewItem(lvFiles.SelectedItems[0], item);

            SetModified(true);
        }

        private void cbTrackLoaderDisc_CheckedChanged(object sender, EventArgs e)
        {
            if (lvFiles.Items.Count == 0)
            {
                return;
            }

            Document.TrackLoaderDisc = cbTrackLoaderDisc.Checked;

            SetModified(true);
        }

        private void cbTrackLoaderItem_CheckedChanged(object sender, EventArgs e)
        {
            CPCDSK.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.TrackLoaderItem = cbTrackLoaderItem.Checked;
            RefreshListViewItem(lvFiles.SelectedItems[0], item);

            SetModified(true);
        }

        private void MoveUp_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return;
            }

            int index = lvFiles.SelectedIndices[0];
            if (index >= 1)
            {
                CPCDSK.Document.Item item = Document.Items[index];

                Document.Items.RemoveAt(index);
                Document.Items.Insert(index - 1, item);

                SetModified(true);
                RefreshUI();

                lvFiles.SelectedIndices.Clear();
                lvFiles.Items[index-1].Selected = true;
                lvFiles.Items[index - 1].EnsureVisible();
            }
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return;
            }

            int index = lvFiles.SelectedIndices[0];
            if (index < (lvFiles.Items.Count - 1))
            {
                CPCDSK.Document.Item item = Document.Items[index];

                Document.Items.RemoveAt(index);
                Document.Items.Insert(index + 1, item);

                SetModified(true);
                RefreshUI();

                lvFiles.SelectedIndices.Clear();
                lvFiles.Items[index + 1].Selected = true;
                lvFiles.Items[index + 1].EnsureVisible();
            }
        }

        private void RefreshMoveButtonStates()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                MoveUp.Enabled = false;
                MoveDown.Enabled = false;
                return;
            }

            MoveUp.Enabled = true;
            MoveDown.Enabled = true;
        }

        private void Reorder_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter =
                "All Order files (*.*)|*.*";

            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select Order file(s)...";
            openFileDialog.FileName = String.Empty;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Plugin.ControllerEditor.Host.Log("Re-ordering...");

                var filename = openFileDialog.FileName;

                var orderContent = File.ReadAllBytes(filename);

                var newItems = new List<CPCDSK.Document.Item>(Document.Items.Count);

                foreach (var oldAmsdosItem in Document.Items)
                {
                    if ( oldAmsdosItem.TrackLoaderItem == false )
                    {
                        newItems.Add(oldAmsdosItem);
                    }
                }

                foreach( var fileIndex in orderContent )
                {
                    var oldItem = Document.Items[(int)(fileIndex)];
                    
                    int foundCount = 0;
                    foreach( var newItem in newItems )
                    {
                        if ( newItem.ResourceID == oldItem.ResourceID )
                        {
                            foundCount++;
                        }
                    }

                    if (foundCount!=0)
                    {
                        PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(oldItem.ResourceID);

                        var duplicateItem = new CPCDSK.Document.Item();
                        duplicateItem.ResourceID = oldItem.ResourceID;
                        duplicateItem.LoadAdress = oldItem.LoadAdress;
                        duplicateItem.ExecAdress = oldItem.ExecAdress;
                        duplicateItem.ItemType = oldItem.ItemType;
                        duplicateItem.CopyToWinAPEROMFolder = oldItem.CopyToWinAPEROMFolder;
                        duplicateItem.TrackLoaderItem = oldItem.TrackLoaderItem;
                        duplicateItem.AmsdosFilename = oldItem.AmsdosFilename;
                        duplicateItem.IsDuplicate = true;
                        duplicateItem.DuplicatedIndex = foundCount + 1;

                        Plugin.ControllerEditor.Host.Log("Duplicate created : " + resource.DisplayName + " (was File Index=" + ((int)fileIndex) + ")");

                        newItems.Add(duplicateItem);
                    }
                    else
                    {
                        newItems.Add(oldItem);
                    }
                }

                Document.Items = newItems;

                Plugin.ControllerEditor.Host.Log("Reorder completed");

                SetModified(true);

                RefreshUI();
            }
        }
    }
}
