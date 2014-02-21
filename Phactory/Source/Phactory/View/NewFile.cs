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
    public partial class NewFile : Form
    {
        public List<PhactoryHost.PluginExtension> pluginExtensions = new List<PhactoryHost.PluginExtension>();
        private string fileLocation;

        public NewFile()
        {
            InitializeComponent();

            ActiveControl = FileName;
            OK.Enabled = false;

            string defaultExtension = App.Controller.PluginManager.GetDefaultExtensionForNewResource();
            int selectedIndex = 0;

            List<PhactoryHost.EditorPlugin> pluginEditors = App.Controller.PluginManager.GetPluginEditors();
            foreach (PhactoryHost.EditorPlugin pluginEditor in pluginEditors)
            {
                foreach (PhactoryHost.PluginExtension pluginExtension in pluginEditor.GetSupportedExtensions())
                {
                    if (pluginExtension.CanBeCreatedFromScratch())
                    {
                        if (pluginExtension.GetName() == defaultExtension)
                        {
                            selectedIndex = lbFileType.Items.Count;
                        } 
                        
                        pluginExtensions.Add(pluginExtension);
                        lbFileType.Items.Add(pluginExtension.GetDescription());
                    }
                }
            }
            lbFileType.SelectedIndex = selectedIndex;
        }

        public bool Valid = false;

        public void SetFileLocation(string fileLocation)
        {
            this.fileLocation = fileLocation;

            UpdateTextInfo();
        }

        public string GetFileLocation()
        {
            return fileLocation;
        }

        public string GetFilename()
        {
            return fileLocation + FileName.Text + pluginExtensions[lbFileType.SelectedIndex].GetName();
        }

        public string GetDisplayName()
        {
            return FileName.Text + pluginExtensions[lbFileType.SelectedIndex].GetName();
        }

        public string GetExtension()
        {
            return pluginExtensions[lbFileType.SelectedIndex].GetName();
        }

        private void OnOK()
        {
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

        private void FileName_TextChanged(object sender, EventArgs e)
        {
            UpdateTextInfo();
        }

        private void UpdateTextInfo()
        {
            if (FileName.Text.Length != 0)
            {
                OK.Enabled = true;
            }
            else
            {
                OK.Enabled = false;
            }

            textInfo.Text = GetFilename();
        }

        private void cbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextInfo();
        }

        private void FileName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                OnOK();
            }
        }
    }
}
