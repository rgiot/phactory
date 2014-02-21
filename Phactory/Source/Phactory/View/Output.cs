using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace Project.View
{
    public partial class Output : DockContent
    {
        public Output()
        {
            InitializeComponent();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogListBox.Items.Clear();
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string outputContent = "";
            foreach (string line in LogListBox.Items)
            {
                outputContent += line + "\n";
            }

            Clipboard.SetDataObject(outputContent, true);
        }

        private void LogListBox_DoubleClick(object sender, EventArgs e)
        {
            string selectedContent = LogListBox.Text;

            int line = -1;
            string filename = "";

            // Source1.s (7): error: Syntax Error '$d020xcf'.
            int warningIndex = selectedContent.IndexOf("warning");
            if (warningIndex == -1)
            {
                warningIndex = selectedContent.IndexOf("error");
            }
            if (warningIndex > 0)
            {
                while ((warningIndex > 0) && (selectedContent[warningIndex] != ':'))
                {
                    warningIndex--;
                }

                int numberIndex = selectedContent.IndexOf(":", 4, warningIndex - 1) + 1;
                string szLine = selectedContent.Substring(numberIndex, warningIndex - numberIndex);

                if (szLine.Contains(":"))
                {
                    szLine = szLine.Substring(0, szLine.IndexOf(":"));
                }
                line = (int) Convert.ToInt16(szLine);

                filename = selectedContent.Substring(0, numberIndex - 1);
                filename = filename.Replace("\\/", "\\");
            }
            else
            { // cc65
                warningIndex = selectedContent.IndexOf("Warning:");
                if (warningIndex == -1)
                {
                    warningIndex = selectedContent.IndexOf("Error:");
                }
                if (warningIndex > 0)
                {
                    int parenthesisOpen = selectedContent.IndexOf("(");
                    int parenthesisClose = selectedContent.IndexOf(")");

                    string szLine = selectedContent.Substring(parenthesisOpen+1, parenthesisClose - parenthesisOpen - 1);

                    line = (int)Convert.ToInt16(szLine);

                    filename = selectedContent.Substring(0, parenthesisOpen);
                    filename = filename.Replace("\\/", "\\");
                }
            }

            if ( (line!=-1) && (filename.Length>0))
            {
                FileInfo fileInfo = new FileInfo(filename);

                if (fileInfo.Exists == false)
                {
                    fileInfo = new FileInfo(App.Controller.UserConfig.ResourcePath + filename);
                }

                PhactoryHost.Database.Resource resource = Helper.DBHelper.GuessResource(fileInfo);
                if (resource != null)
                {
                    App.Controller.OpenTextResourceAtLine(resource, line);
                }
            }
        }
    }
}
