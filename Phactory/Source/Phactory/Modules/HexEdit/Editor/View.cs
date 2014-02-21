using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Be.Windows.Forms;

namespace HexEdit.View
{
    public partial class View : UserControl
    {
        public PhactoryHost.Database.Resource Resource;

        private bool modified;
        public bool IsReady = false;

        public View(PhactoryHost.Database.Resource resource)
        {
            InitializeComponent();

            Resource = resource;
            modified = false;
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

        public void RefreshContent()
        {
            try
            {
                FileInfo fileInfo = Plugin.Controller.Host.GetFileInfo(Resource);
                hexBox.ByteProvider = new DynamicByteProvider(File.ReadAllBytes(fileInfo.FullName));

                hexBox.ResetText();
                hexBox.Refresh();

                FileSize.Text = "Length = " + fileInfo.Length + " (0x" + String.Format("{0:X4}", fileInfo.Length) + ") bytes";
            }
            catch (Exception)
            {   
            }
            finally
            {
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
            parentForm.Text += " (read-only)";

            string toolTipText = Plugin.Controller.Host.GetFileInfo(Resource).FullName;
            if (this.modified)
            {
                toolTipText += "*";
            }
            toolTipText += " (read-only)";

            Plugin.Controller.Host.SetToolTipText(parentPanel, toolTipText);
        }
    }
}
