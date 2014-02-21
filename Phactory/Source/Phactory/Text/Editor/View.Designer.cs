namespace CPCText.View
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DocText = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DocCharset = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.AppendEndOfText = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.DocText);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(394, 318);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Text";
            // 
            // DocText
            // 
            this.DocText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DocText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocText.Location = new System.Drawing.Point(3, 16);
            this.DocText.Name = "DocText";
            this.DocText.Size = new System.Drawing.Size(388, 299);
            this.DocText.TabIndex = 0;
            this.DocText.Text = "";
            this.DocText.TextChanged += new System.EventHandler(this.Text_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DocCharset);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 47);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Charset";
            // 
            // DocCharset
            // 
            this.DocCharset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DocCharset.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocCharset.Location = new System.Drawing.Point(3, 16);
            this.DocCharset.Multiline = false;
            this.DocCharset.Name = "DocCharset";
            this.DocCharset.Size = new System.Drawing.Size(394, 28);
            this.DocCharset.TabIndex = 0;
            this.DocCharset.Text = "";
            this.DocCharset.TextChanged += new System.EventHandler(this.Charset_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.AppendEndOfText);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(400, 400);
            this.splitContainer1.SplitterDistance = 47;
            this.splitContainer1.TabIndex = 6;
            // 
            // AppendEndOfText
            // 
            this.AppendEndOfText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AppendEndOfText.AutoSize = true;
            this.AppendEndOfText.Checked = true;
            this.AppendEndOfText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AppendEndOfText.Location = new System.Drawing.Point(6, 327);
            this.AppendEndOfText.Name = "AppendEndOfText";
            this.AppendEndOfText.Size = new System.Drawing.Size(241, 17);
            this.AppendEndOfText.TabIndex = 5;
            this.AppendEndOfText.Text = "Automatically append 0xFF value (end of text)";
            this.AppendEndOfText.UseVisualStyleBackColor = true;
            this.AppendEndOfText.CheckedChanged += new System.EventHandler(this.AppendEndOfText_CheckedChanged);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "View";
            this.Size = new System.Drawing.Size(400, 400);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox DocText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox DocCharset;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox AppendEndOfText;


    }
}
