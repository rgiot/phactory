using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PhactoryHost.Database;

namespace Phactory.View
{
    public partial class ProjectProperties : Form
    {
        public ProjectProperties()
        {
            InitializeComponent();

            this.propertyGrid1.SelectedObject = Project.App.Controller.Entities.GlobalSettings;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Project.App.Controller.SaveProject();

            this.Close();
        }
    }
}
