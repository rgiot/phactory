namespace CPCCloud.View
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
            this.lvFiles = new System.Windows.Forms.ListView();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.tabPreviews = new System.Windows.Forms.TabControl();
            this.tabCloudMaskPreview = new System.Windows.Forms.TabPage();
            this.CloudMaskPreview = new System.Windows.Forms.PictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2.SuspendLayout();
            this.tabPreviews.SuspendLayout();
            this.tabCloudMaskPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloudMaskPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvFiles);
            this.groupBox2.Controls.Add(this.Remove);
            this.groupBox2.Controls.Add(this.Add);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(854, 439);
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
            this.lvFiles.Size = new System.Drawing.Size(851, 391);
            this.lvFiles.TabIndex = 4;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(84, 410);
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
            this.Add.Location = new System.Drawing.Point(3, 410);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 2;
            this.Add.Text = "Add...";
            this.Add.UseVisualStyleBackColor = false;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // tabPreviews
            // 
            this.tabPreviews.Controls.Add(this.tabCloudMaskPreview);
            this.tabPreviews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreviews.Location = new System.Drawing.Point(0, 0);
            this.tabPreviews.Name = "tabPreviews";
            this.tabPreviews.SelectedIndex = 0;
            this.tabPreviews.Size = new System.Drawing.Size(854, 200);
            this.tabPreviews.TabIndex = 1;
            // 
            // tabCloudMaskPreview
            // 
            this.tabCloudMaskPreview.Controls.Add(this.CloudMaskPreview);
            this.tabCloudMaskPreview.Location = new System.Drawing.Point(4, 22);
            this.tabCloudMaskPreview.Name = "tabCloudMaskPreview";
            this.tabCloudMaskPreview.Size = new System.Drawing.Size(846, 174);
            this.tabCloudMaskPreview.TabIndex = 3;
            this.tabCloudMaskPreview.Text = "Cloud Mask Preview";
            this.tabCloudMaskPreview.UseVisualStyleBackColor = true;
            // 
            // CloudMaskPreview
            // 
            this.CloudMaskPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CloudMaskPreview.Location = new System.Drawing.Point(0, 0);
            this.CloudMaskPreview.Name = "CloudMaskPreview";
            this.CloudMaskPreview.Size = new System.Drawing.Size(846, 174);
            this.CloudMaskPreview.TabIndex = 1;
            this.CloudMaskPreview.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabPreviews);
            this.splitContainer2.Size = new System.Drawing.Size(854, 643);
            this.splitContainer2.SplitterDistance = 439;
            this.splitContainer2.TabIndex = 0;
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Name = "View";
            this.Size = new System.Drawing.Size(854, 643);
            this.groupBox2.ResumeLayout(false);
            this.tabPreviews.ResumeLayout(false);
            this.tabCloudMaskPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CloudMaskPreview)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.TabControl tabPreviews;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabPage tabCloudMaskPreview;
        private System.Windows.Forms.PictureBox CloudMaskPreview;
    }
}
