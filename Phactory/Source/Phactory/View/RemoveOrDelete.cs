using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project.View
{
    public partial class RemoveOrDelete : Form
    {
        private string name;
        private bool isFolder;

        public bool Valid = false;
        public bool IsRemove = false;
        public bool IsDelete = false;

        public RemoveOrDelete()
        {
            InitializeComponent();
        }

        public RemoveOrDelete(bool isFolder, string name) : this()
        {
            this.isFolder = isFolder;
            this.name = name;

            if (isFolder)
            {
                this.Label1.Text = "Choose Remove to remove all content of '" + name + "' from project.";
                this.Label2.Text = "Choose Delete to permanently delete all content of '" + name + "'.";
            }
            else
            {
                this.Label1.Text = "Choose Remove to remove '" + name + "' from project.";
                this.Label2.Text = "Choose Delete to permanently delete '" + name + "'.";
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            Valid = true;
            IsRemove = true;

            Close();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Valid = true;
            IsDelete = true;

            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Valid = false;

            Close();
        }
    }
}
