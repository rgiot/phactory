using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Project.View
{
    public partial class ResourceSelector : Form
    {
        public bool Valid = false;

        public List<PhactoryHost.Database.Resource> Resources = null;
        
        public PhactoryHost.Database.Resource DefaultResource = null;
        public List<string> ExtensionFilter = new List<string>();
        public bool MultiSelect = false;
        
        public ResourceSelector()
        {
            InitializeComponent();

            lvResources.Clear();
            lvResources.Columns.Add("Identifier", 65);
            lvResources.Columns.Add("Name", 65*5);
            lvResources.Columns.Add("Is Output", 65);
            lvResources.Columns.Add("File Exists", 65); 
            // lvResources.Columns.Add("Relative Path", 250);
        }

        public ResourceSelector(PhactoryHost.Database.Resource defaultResource) : 
            this()
        {
            DefaultResource = defaultResource;
        }

        public ResourceSelector(PhactoryHost.Database.Resource defaultResource, bool multiSelect) :
            this()
        {
            DefaultResource = defaultResource;
            MultiSelect = multiSelect;
        }

        public ResourceSelector(PhactoryHost.Database.Resource defaultResource, List<string> extensionFilter) :
            this()
        {
            DefaultResource = defaultResource;

            if (extensionFilter != null)
            {
                ExtensionFilter = extensionFilter;

                string title = this.Text;
                title += " (filtered by ";                
                for( int iExtension = 0; iExtension < extensionFilter.Count; iExtension++ )
                {
                    title += extensionFilter[ iExtension ];
                    if ( iExtension != ( extensionFilter.Count - 1 ) )
                    {
                        title += ", ";
                    }
                }
                title += ")";

                this.Text = title;
            }
        }

        public ResourceSelector(PhactoryHost.Database.Resource defaultResource, bool multiSelect, List<string> extensionFilter) :
            this(defaultResource, extensionFilter)
        {
            MultiSelect = multiSelect;
        }

        private void RefreshUI()
        {
            if (!this.Visible)
            {
                return;
            }

            lvResources.BeginUpdate();

            lvResources.Items.Clear();

            if (MultiSelect)
            {
                lvResources.MultiSelect = true;
            }

            foreach (PhactoryHost.Database.Resource resource in App.Controller.Entities.Resources)
            {
                Application.DoEvents();
                
                if (ExtensionFilter.Count > 0)
                {
                    string resourceExtension = Helper.StringHelper.GetFileInfo(resource).Extension.ToLower();

                    bool extensionFound = false;
                    foreach (string extension in ExtensionFilter)
                    {
                        if (resourceExtension == extension.ToLower())
                        {
                            extensionFound = true;
                            break;
                        }
                    }

                    if (!extensionFound)
                    {
                        continue;
                    }
                }

                if ( Filter.Text != "" )
                {
                    if (!resource.DisplayName.ToUpper().Contains(Filter.Text.ToUpper()))
                    {
                        continue;
                    }
                }

                ListViewItem item = new ListViewItem(""+resource.Id); // Id

                item.SubItems.Add(resource.DisplayName); // Name

                // Is Output
                if (resource.IsOutputResource)
                {
                    item.SubItems.Add("YES");

                    item.BackColor = Color.PaleTurquoise;
                }
                else
                {
                    item.SubItems.Add("");
                }
                
                // File Exists
                if (File.Exists(App.Controller.UserConfig.ResourcePath + resource.RelativePath))
                {
                    item.SubItems.Add("YES");
                }
                else
                {
                    item.SubItems.Add("MISSING");
                }

                // item.SubItems.Add(resource.RelativePath); // Relative Path

                lvResources.Items.Add(item);

                item.Tag = resource;

                if (DefaultResource != null)
                {
                    if (resource.Id == DefaultResource.Id)
                    {
                        item.Selected = true;
                    }
                }
            }

            lvResources.EndUpdate();

            Filter.Focus();
        }

        private void OnOK()
        {
            if (lvResources.SelectedItems.Count == 0 )
            {
                App.Controller.View.ShowWarningMessage("Empty selection !", "You need to select an item in list");                        
                return;
            }

            Resources = new List<PhactoryHost.Database.Resource>(lvResources.SelectedItems.Count);
            foreach( ListViewItem lvItem in lvResources.SelectedItems )
            {
                Resources.Add(lvItem.Tag as PhactoryHost.Database.Resource);
            }

            Valid = true;

            Close();
        }
        
        private void OK_Click(object sender, EventArgs e)
        {
            OnOK();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lvResources_DoubleClick(object sender, EventArgs e)
        {
            OnOK();
        }

        private void Filter_TextChanged(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private void cbShowOutputFiles_CheckedChanged(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private void ResourceSelector_Shown(object sender, EventArgs e)
        {
            RefreshUI();
        }
    }
}
