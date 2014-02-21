using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Project.View
{
    public partial class PluginView : DockContent
    {
        public PhactoryHost.Database.Resource Resource = null;
        public ToolStripMenuItem ToolStripMenuItem = null;

        public bool IsDependency { get; set; }

        public PluginView(bool isDependency)
        {
            InitializeComponent();

            IsDependency = isDependency;
        }

        private void PluginView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                App.Controller.IsAppClosing = true;
            } 
            
            if ((Resource != null) && (!App.Controller.CloseResource(Resource)))
            {
                e.Cancel = true;
            }

            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                App.Controller.IsAppClosing = false;
            }

            if (ToolStripMenuItem != null)
            {
                ToolStripMenuItem.Checked = false;
                e.Cancel = true;
                Hide();
            }
        }
    }
}
