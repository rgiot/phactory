namespace HxCConverter.Tool
{
    partial class View
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Convert = new System.Windows.Forms.Button();
            this.Files = new System.Windows.Forms.ListBox();
            this.AddFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // Convert
            // 
            this.Convert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Convert.Location = new System.Drawing.Point(361, 264);
            this.Convert.Name = "Convert";
            this.Convert.Size = new System.Drawing.Size(75, 23);
            this.Convert.TabIndex = 0;
            this.Convert.Text = "Convert";
            this.Convert.UseVisualStyleBackColor = true;
            this.Convert.Click += new System.EventHandler(this.Convert_Click);
            // 
            // Files
            // 
            this.Files.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Files.FormattingEnabled = true;
            this.Files.IntegralHeight = false;
            this.Files.Location = new System.Drawing.Point(12, 12);
            this.Files.Name = "Files";
            this.Files.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.Files.Size = new System.Drawing.Size(424, 246);
            this.Files.TabIndex = 1;
            // 
            // AddFile
            // 
            this.AddFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddFile.Location = new System.Drawing.Point(12, 264);
            this.AddFile.Name = "AddFile";
            this.AddFile.Size = new System.Drawing.Size(75, 23);
            this.AddFile.TabIndex = 2;
            this.AddFile.Text = "Add File(s)...";
            this.AddFile.UseVisualStyleBackColor = true;
            this.AddFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 299);
            this.Controls.Add(this.Files);
            this.Controls.Add(this.AddFile);
            this.Controls.Add(this.Convert);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "View";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HxC Floppy Emulator Converter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Convert;
        private System.Windows.Forms.ListBox Files;
        private System.Windows.Forms.Button AddFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}