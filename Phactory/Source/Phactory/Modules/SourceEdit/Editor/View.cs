using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Lewis.SST.Controls;
using Lewis.SST.Gui;

namespace SourceEdit.View
{
    public partial class View : UserControl
    {
        public PhactoryHost.Database.Resource Resource;

        private bool modified = false;
        private bool isReadOnly = false;
        
        public bool IsReady = false;
        
        public View(PhactoryHost.Database.Resource resource)
        {
            InitializeComponent();
            
            Resource = resource;
            modified = false;

            this.textEditorControl.ActiveTextAreaControl.TextArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextEditor_KeyDown);
            this.textEditorControl.TextChanged += new EventHandler(TextEditor_TextChanged);
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

        public void SetReadOnly(bool isReadOnly)
        {
            this.isReadOnly = isReadOnly;

            this.textEditorControl.IsReadOnly = isReadOnly;
        }

        delegate void RefreshTitleDelegate();
        public void RefreshTitle()
        {
            if (InvokeRequired)
            {
                Invoke(new RefreshTitleDelegate(RefreshTitle));
                return;
            }

            Panel parentPanel = this.Parent as Panel;
            Form parentForm = parentPanel.Parent as Form;

            parentForm.Text = Resource.DisplayName;
            if (this.modified)
            {
                parentForm.Text += "*";
            }
            if (this.isReadOnly)
            {
                parentForm.Text += " (read-only)";
            }

            string toolTipText = Plugin.ControllerEditor.Host.GetFileInfo(Resource).FullName;
            if (this.modified)
            {
                toolTipText += "*";
            }
            if (this.isReadOnly)
            {
                toolTipText += " (read-only)";
            }
            Plugin.ControllerEditor.Host.SetToolTipText(parentPanel, toolTipText);
        }

        delegate void WriteContentToFileDelegate(string filename);
        public void WriteContentToFile(string filename)
        {
            if (InvokeRequired)
            {
                Invoke(new WriteContentToFileDelegate(WriteContentToFile), new object[] { filename });
                return;
            }

            File.WriteAllText(filename, textEditorControl.Text);
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                return;
            }

            if (IsReady)
            {
                SetModified(true);
            }
        }
        
        private void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Control || e.KeyCode == Keys.F3)
            {
                // fine method call
                if (e.KeyCode == Keys.F && e.Control)
                {
                    Find(true);
                }
                else
                {
                    Find(false);
                }
            }
            else if (e.KeyCode == Keys.H && e.Control)
            {
                if (!isReadOnly)
                {
                    Replace();
                }
            } 
            
            if (isReadOnly)
            {
                return;
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                Plugin.ControllerEditor.Host.RequestSave(Resource);
            }
        }

        public void SetLine(int line)
        {
            //textEditorControl.GoTo.Line(line - 1);
        }

        private FindValueDlg fdlg = new FindValueDlg();
        protected DialogResult dr = DialogResult.OK;
        protected string searchPattern = string.Empty; private Regex _findNextRegex;
        private int _findNextStartPos = 0;

        private int Find(Regex regex, int startPos)
        {
            if (this.textEditorControl != null)
            {
                string context = this.textEditorControl.Text.Substring(startPos);
                Match m = regex.Match(context);
                if (!m.Success)
                {
                    MessageBox.Show("The specified text was not found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                int wordStart = TextUtilities.FindNextWordStart(this.textEditorControl.Document, m.Index);
                int line = this.textEditorControl.Document.GetLineNumberForOffset(m.Index + startPos);
                this.textEditorControl.ActiveTextAreaControl.TextArea.ScrollTo(line);

                TextUtils.SelectText(this.textEditorControl, m.Index + startPos, m.Length);
                _findNextRegex = regex;
                _findNextStartPos = m.Index + startPos;

                TextUtils.SetPosition(this.textEditorControl, m.Index + m.Length + startPos);
                return m.Index + m.Length + startPos;
            }
            return 0;
        }

        private int Replace(Regex regex, int startPos, string replaceWith)
        {
            if (this.textEditorControl != null)
            {
                if (this.textEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length > 0)
                {
                    int start = this.textEditorControl.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
                    int length = this.textEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length;
                    this.textEditorControl.Document.Replace(start, length, replaceWith);

                    return Find(regex, length + start);
                }

                string context = textEditorControl.Text.Substring(startPos);

                Match m = regex.Match(context);

                if (!m.Success)
                {
                    MessageBox.Show("The specified text was not found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                this.textEditorControl.Document.Replace(m.Index + startPos, m.Length, replaceWith);
                this.textEditorControl.Refresh();
                TextUtils.SetPosition(this.textEditorControl, m.Index + replaceWith.Length + startPos);
                return m.Index + replaceWith.Length + startPos;
            }
            return 0;
        }

        public void Find(bool display)
        {
            if (this.textEditorControl != null)
            {
                if (this.textEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length > 0)
                {
                    fdlg.FindValue = this.textEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
                }
                else
                {
                    fdlg.FindValue = fdlg.FindValue == null || fdlg.FindValue.Length == 0 ? string.Empty : fdlg.FindValue;
                }
                fdlg.ReplaceFlag = false;
                fdlg.Text = "Find...";

                if (display)
                {
                    dr = fdlg.ShowDialog(this);
                }
                if (fdlg.FindValue.Length == 0) return;

                if (dr == DialogResult.OK)
                {
                    searchPattern = fdlg.FindValue;
                    searchPattern = searchPattern.Replace("[", @"\[").Replace("]", @"\]").Replace("(", @"\(").Replace(")", @"\)");

                    try
                    {
                        _findNextRegex = new Regex(searchPattern, RegexOptions.IgnoreCase);
                        _findNextStartPos = Find(_findNextRegex, _findNextStartPos);
                    }
                    catch
                    {
                        //_view.WriteLine("Error in regular expression for Find.");
                    }
                }
            }
        }

        public void Replace()
        {
            if (this.textEditorControl != null)
            {
                if (this.textEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length > 0)
                {
                    fdlg.FindValue = this.textEditorControl.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
                }
                else
                {
                    fdlg.FindValue = string.Empty;
                }

                fdlg.ReplaceFlag = true;
                dr = fdlg.ShowDialog(this);
                if (fdlg.FindValue.Length == 0) return;

                if (dr == DialogResult.OK)
                {
                    string replacement = fdlg.ReplaceValue;
                    string searchPattern = fdlg.FindValue;
                    searchPattern =
                        searchPattern.Replace("[", @"\[").Replace("]", @"\]").Replace("(", @"\(").Replace(")", @"\)");

                    try
                    {
                        _findNextRegex = new Regex(searchPattern, RegexOptions.IgnoreCase);
                        _findNextStartPos = Replace(_findNextRegex, _findNextStartPos, replacement);
                    }
                    catch
                    {
                        // _view.WriteLine("Error in regular expression for Find.");
                    }
                }
            }
        }
    }
}
