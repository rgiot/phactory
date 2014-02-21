using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CPCText.View
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
        }

        public void RefreshUI()
        {
            DocCharset.Text = Document.Charset;
            DocText.Text = Document.Text;
            AppendEndOfText.Checked = Document.AppendEndOfText;
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

        private void Charset_TextChanged(object sender, EventArgs e)
        {
            Document.Charset = DocCharset.Text;

            if (IsReady)
            {
                SetModified(true);
            }
        }

        private void Text_TextChanged(object sender, EventArgs e)
        {
            Document.Text = DocText.Text;

            if (IsReady)
            {
                SetModified(true);
            }
        }

        private void AppendEndOfText_CheckedChanged(object sender, EventArgs e)
        {
            Document.AppendEndOfText = AppendEndOfText.Checked;

            if (IsReady)
            {
                SetModified(true);
            }
        }
    }
}
