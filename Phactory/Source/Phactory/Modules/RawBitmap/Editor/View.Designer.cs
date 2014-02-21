namespace CPCRawBitmap.View
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
            this.Preview = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.curSelIndexLabel = new System.Windows.Forms.Label();
            this.CompileInternally = new System.Windows.Forms.Button();
            this.Information = new System.Windows.Forms.TextBox();
            this.tabPreviews = new System.Windows.Forms.TabControl();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.GroupItemProperties = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.VerticalRaw = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.Raw = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPreviews.SuspendLayout();
            this.tabPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.GroupItemProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // Preview
            // 
            this.Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Preview.Location = new System.Drawing.Point(3, 3);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(722, 146);
            this.Preview.TabIndex = 0;
            this.Preview.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvFiles);
            this.groupBox2.Controls.Add(this.Remove);
            this.groupBox2.Controls.Add(this.Add);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(459, 206);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resource(s)";
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(3, 16);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(453, 158);
            this.lvFiles.TabIndex = 4;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(84, 180);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 3;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Add.Location = new System.Drawing.Point(3, 180);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 2;
            this.Add.Text = "Add...";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.curSelIndexLabel);
            this.groupBox3.Controls.Add(this.CompileInternally);
            this.groupBox3.Controls.Add(this.Information);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(459, 86);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Information";
            // 
            // curSelIndexLabel
            // 
            this.curSelIndexLabel.AutoSize = true;
            this.curSelIndexLabel.Location = new System.Drawing.Point(166, 19);
            this.curSelIndexLabel.Name = "curSelIndexLabel";
            this.curSelIndexLabel.Size = new System.Drawing.Size(0, 13);
            this.curSelIndexLabel.TabIndex = 6;
            // 
            // CompileInternally
            // 
            this.CompileInternally.Location = new System.Drawing.Point(3, 16);
            this.CompileInternally.Name = "CompileInternally";
            this.CompileInternally.Size = new System.Drawing.Size(156, 23);
            this.CompileInternally.TabIndex = 5;
            this.CompileInternally.Text = "Build Internally...";
            this.CompileInternally.UseVisualStyleBackColor = true;
            this.CompileInternally.Click += new System.EventHandler(this.CompileInternally_Click);
            // 
            // Information
            // 
            this.Information.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Information.Location = new System.Drawing.Point(3, 42);
            this.Information.Multiline = true;
            this.Information.Name = "Information";
            this.Information.Size = new System.Drawing.Size(453, 41);
            this.Information.TabIndex = 0;
            // 
            // tabPreviews
            // 
            this.tabPreviews.Controls.Add(this.tabPreview);
            this.tabPreviews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreviews.Location = new System.Drawing.Point(0, 0);
            this.tabPreviews.Name = "tabPreviews";
            this.tabPreviews.SelectedIndex = 0;
            this.tabPreviews.Size = new System.Drawing.Size(736, 178);
            this.tabPreviews.TabIndex = 1;
            // 
            // tabPreview
            // 
            this.tabPreview.Controls.Add(this.Preview);
            this.tabPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPreview.Size = new System.Drawing.Size(728, 152);
            this.tabPreview.TabIndex = 0;
            this.tabPreview.Text = "Preview";
            this.tabPreview.UseVisualStyleBackColor = true;
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabPreviews);
            this.splitContainer1.Size = new System.Drawing.Size(736, 484);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.TabIndex = 7;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.Location = new System.Drawing.Point(0, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.GroupItemProperties);
            this.splitContainer3.Size = new System.Drawing.Size(736, 296);
            this.splitContainer3.SplitterDistance = 459;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer4.Size = new System.Drawing.Size(459, 296);
            this.splitContainer4.SplitterDistance = 206;
            this.splitContainer4.TabIndex = 3;
            // 
            // GroupItemProperties
            // 
            this.GroupItemProperties.Controls.Add(this.label6);
            this.GroupItemProperties.Controls.Add(this.VerticalRaw);
            this.GroupItemProperties.Controls.Add(this.label2);
            this.GroupItemProperties.Controls.Add(this.Raw);
            this.GroupItemProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupItemProperties.Enabled = false;
            this.GroupItemProperties.Location = new System.Drawing.Point(0, 0);
            this.GroupItemProperties.Name = "GroupItemProperties";
            this.GroupItemProperties.Size = new System.Drawing.Size(273, 296);
            this.GroupItemProperties.TabIndex = 6;
            this.GroupItemProperties.TabStop = false;
            this.GroupItemProperties.Text = "Item Properties";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(24, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Bitmap converted as a raw vertical buffer";
            // 
            // VerticalRaw
            // 
            this.VerticalRaw.AutoSize = true;
            this.VerticalRaw.Location = new System.Drawing.Point(6, 52);
            this.VerticalRaw.Name = "VerticalRaw";
            this.VerticalRaw.Size = new System.Drawing.Size(200, 17);
            this.VerticalRaw.TabIndex = 15;
            this.VerticalRaw.Text = "Vertical Raw DATA (max. 256 colors)";
            this.VerticalRaw.UseVisualStyleBackColor = true;
            this.VerticalRaw.CheckedChanged += new System.EventHandler(this.ItemType_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(24, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Bitmap converted as a raw linear buffer";
            // 
            // Raw
            // 
            this.Raw.AutoSize = true;
            this.Raw.Checked = true;
            this.Raw.Location = new System.Drawing.Point(6, 16);
            this.Raw.Name = "Raw";
            this.Raw.Size = new System.Drawing.Size(162, 17);
            this.Raw.TabIndex = 5;
            this.Raw.TabStop = true;
            this.Raw.Text = "Raw DATA (max. 256 colors)";
            this.Raw.UseVisualStyleBackColor = true;
            this.Raw.CheckedChanged += new System.EventHandler(this.ItemType_CheckedChanged);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "View";
            this.Size = new System.Drawing.Size(736, 484);
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPreviews.ResumeLayout(false);
            this.tabPreview.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.GroupItemProperties.ResumeLayout(false);
            this.GroupItemProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox Preview;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.TabControl tabPreviews;
        private System.Windows.Forms.TabPage tabPreview;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox Information;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button CompileInternally;
        private System.Windows.Forms.Label curSelIndexLabel;
        private System.Windows.Forms.GroupBox GroupItemProperties;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton VerticalRaw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton Raw;
    }
}
