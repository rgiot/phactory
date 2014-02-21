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
    public partial class NewProject : Form
    {
        public NewProject()
        {
            InitializeComponent();

            ActiveControl = ProjectName;

            this.OK.Enabled = false;
        }

        public bool Valid = false;

        public string GetName()
        {
            return ProjectName.Text;
        }

        public void SetProjectLocation(string projectLocation)
        {
            ProjectLocation.Text = projectLocation;
        }

        public string GetProjectLocation()
        {
            return ProjectLocation.Text;
        }

        public string GetFilename()
        {
            return ProjectLocation.Text + ProjectName.Text + ".cpcproj";
        }

        private void OnOK()
        {
            Valid = true;

            Close();

            if (this.createDirectoryForProject.Checked)
            {
                ProjectLocation.Text += ProjectName.Text + "\\";
            }
        }
        
        private void OK_Click(object sender, EventArgs e)
        {
            OnOK();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            openFolder.Description = "Select a folder where the project will be created";
            openFolder.ShowNewFolderButton = true;

            if (Directory.Exists(GetProjectLocation()))
            {
                openFolder.SelectedPath = GetProjectLocation();
            }
            else
            {
                openFolder.RootFolder = Environment.SpecialFolder.MyComputer;
            }

            if (openFolder.ShowDialog() == DialogResult.OK)
            {
                ProjectLocation.Text = openFolder.SelectedPath + "\\";
            }
        }

        private void ProjectName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                OnOK();
            }
        }

        private void ProjectName_TextChanged(object sender, EventArgs e)
        {
            if (this.ProjectName.Text.Length == 0)
            {
                this.OK.Enabled = false;
            }
            else
            {
                this.OK.Enabled = true;
            }
        }
    }
}
