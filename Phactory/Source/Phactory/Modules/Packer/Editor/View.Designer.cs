namespace CPCPacker.View
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
            this.label1 = new System.Windows.Forms.Label();
            this.rbBitBuster = new System.Windows.Forms.RadioButton();
            this.rbExomizer = new System.Windows.Forms.RadioButton();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rbBitBuster);
            this.groupBox2.Controls.Add(this.rbExomizer);
            this.groupBox2.Controls.Add(this.lvFiles);
            this.groupBox2.Controls.Add(this.Remove);
            this.groupBox2.Controls.Add(this.Add);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(500, 489);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CPC Files to be compressed";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Packer :";
            // 
            // rbBitBuster
            // 
            this.rbBitBuster.AutoSize = true;
            this.rbBitBuster.Location = new System.Drawing.Point(133, 17);
            this.rbBitBuster.Name = "rbBitBuster";
            this.rbBitBuster.Size = new System.Drawing.Size(67, 17);
            this.rbBitBuster.TabIndex = 6;
            this.rbBitBuster.Text = "BitBuster";
            this.rbBitBuster.UseVisualStyleBackColor = true;
            this.rbBitBuster.CheckedChanged += new System.EventHandler(this.rbBitBuster_CheckedChanged);
            // 
            // rbExomizer
            // 
            this.rbExomizer.AutoSize = true;
            this.rbExomizer.Checked = true;
            this.rbExomizer.Location = new System.Drawing.Point(60, 17);
            this.rbExomizer.Name = "rbExomizer";
            this.rbExomizer.Size = new System.Drawing.Size(67, 17);
            this.rbExomizer.TabIndex = 5;
            this.rbExomizer.TabStop = true;
            this.rbExomizer.Text = "Exomizer";
            this.rbExomizer.UseVisualStyleBackColor = true;
            this.rbExomizer.CheckedChanged += new System.EventHandler(this.rbExomizer_CheckedChanged);
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(3, 37);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(494, 420);
            this.lvFiles.TabIndex = 4;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(84, 463);
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
            this.Add.Location = new System.Drawing.Point(3, 463);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 2;
            this.Add.Text = "Add...";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "View";
            this.Size = new System.Drawing.Size(500, 489);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbBitBuster;
        private System.Windows.Forms.RadioButton rbExomizer;

    }
}
