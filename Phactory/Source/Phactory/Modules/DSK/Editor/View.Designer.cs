namespace CPCDSK.View
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
            this.MoveDown = new System.Windows.Forms.Button();
            this.MoveUp = new System.Windows.Forms.Button();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.NbFiles = new System.Windows.Forms.Label();
            this.cbTrackLoaderDisc = new System.Windows.Forms.CheckBox();
            this.cbHFE = new System.Windows.Forms.CheckBox();
            this.gbFileProperties = new System.Windows.Forms.GroupBox();
            this.cbTrackLoaderItem = new System.Windows.Forms.CheckBox();
            this.cbCopyToWinAPERomFolder = new System.Windows.Forms.CheckBox();
            this.AmsdosFilename = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.ExecAdress = new System.Windows.Forms.TextBox();
            this.LoadAdress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Reorder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbFileProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // MoveDown
            // 
            this.MoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MoveDown.Enabled = false;
            this.MoveDown.Location = new System.Drawing.Point(276, 298);
            this.MoveDown.Name = "MoveDown";
            this.MoveDown.Size = new System.Drawing.Size(75, 23);
            this.MoveDown.TabIndex = 6;
            this.MoveDown.Text = "Move Down";
            this.MoveDown.UseVisualStyleBackColor = true;
            this.MoveDown.Click += new System.EventHandler(this.MoveDown_Click);
            // 
            // MoveUp
            // 
            this.MoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MoveUp.Enabled = false;
            this.MoveUp.Location = new System.Drawing.Point(195, 298);
            this.MoveUp.Name = "MoveUp";
            this.MoveUp.Size = new System.Drawing.Size(75, 23);
            this.MoveUp.TabIndex = 5;
            this.MoveUp.Text = "Move Up";
            this.MoveUp.UseVisualStyleBackColor = true;
            this.MoveUp.Click += new System.EventHandler(this.MoveUp_Click);
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(6, 67);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(893, 220);
            this.lvFiles.TabIndex = 4;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(87, 298);
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
            this.Add.Location = new System.Drawing.Point(6, 298);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 2;
            this.Add.Text = "Add...";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
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
            this.splitContainer1.Panel2.Controls.Add(this.gbFileProperties);
            this.splitContainer1.Size = new System.Drawing.Size(905, 545);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Reorder);
            this.groupBox1.Controls.Add(this.NbFiles);
            this.groupBox1.Controls.Add(this.cbTrackLoaderDisc);
            this.groupBox1.Controls.Add(this.cbHFE);
            this.groupBox1.Controls.Add(this.MoveDown);
            this.groupBox1.Controls.Add(this.lvFiles);
            this.groupBox1.Controls.Add(this.MoveUp);
            this.groupBox1.Controls.Add(this.Add);
            this.groupBox1.Controls.Add(this.Remove);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(905, 327);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CPC Files";
            // 
            // NbFiles
            // 
            this.NbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NbFiles.AutoSize = true;
            this.NbFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NbFiles.Location = new System.Drawing.Point(377, 303);
            this.NbFiles.Name = "NbFiles";
            this.NbFiles.Size = new System.Drawing.Size(43, 13);
            this.NbFiles.TabIndex = 9;
            this.NbFiles.Text = "0 File(s)";
            // 
            // cbTrackLoaderDisc
            // 
            this.cbTrackLoaderDisc.AutoSize = true;
            this.cbTrackLoaderDisc.Location = new System.Drawing.Point(7, 42);
            this.cbTrackLoaderDisc.Name = "cbTrackLoaderDisc";
            this.cbTrackLoaderDisc.Size = new System.Drawing.Size(114, 17);
            this.cbTrackLoaderDisc.TabIndex = 8;
            this.cbTrackLoaderDisc.Text = "Track-Loader Disc";
            this.cbTrackLoaderDisc.UseVisualStyleBackColor = true;
            this.cbTrackLoaderDisc.CheckedChanged += new System.EventHandler(this.cbTrackLoaderDisc_CheckedChanged);
            // 
            // cbHFE
            // 
            this.cbHFE.AutoSize = true;
            this.cbHFE.Location = new System.Drawing.Point(7, 19);
            this.cbHFE.Name = "cbHFE";
            this.cbHFE.Size = new System.Drawing.Size(278, 17);
            this.cbHFE.TabIndex = 7;
            this.cbHFE.Text = "Also generate HxC Floppy Emulator disc-image (.HFE)";
            this.cbHFE.UseVisualStyleBackColor = true;
            this.cbHFE.CheckedChanged += new System.EventHandler(this.cbHFE_CheckedChanged);
            // 
            // gbFileProperties
            // 
            this.gbFileProperties.Controls.Add(this.cbTrackLoaderItem);
            this.gbFileProperties.Controls.Add(this.cbCopyToWinAPERomFolder);
            this.gbFileProperties.Controls.Add(this.AmsdosFilename);
            this.gbFileProperties.Controls.Add(this.label4);
            this.gbFileProperties.Controls.Add(this.cbType);
            this.gbFileProperties.Controls.Add(this.ExecAdress);
            this.gbFileProperties.Controls.Add(this.LoadAdress);
            this.gbFileProperties.Controls.Add(this.label3);
            this.gbFileProperties.Controls.Add(this.label2);
            this.gbFileProperties.Controls.Add(this.label1);
            this.gbFileProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFileProperties.Location = new System.Drawing.Point(0, 0);
            this.gbFileProperties.Name = "gbFileProperties";
            this.gbFileProperties.Size = new System.Drawing.Size(905, 214);
            this.gbFileProperties.TabIndex = 0;
            this.gbFileProperties.TabStop = false;
            this.gbFileProperties.Text = "File Properties";
            // 
            // cbTrackLoaderItem
            // 
            this.cbTrackLoaderItem.AutoSize = true;
            this.cbTrackLoaderItem.Location = new System.Drawing.Point(9, 139);
            this.cbTrackLoaderItem.Name = "cbTrackLoaderItem";
            this.cbTrackLoaderItem.Size = new System.Drawing.Size(113, 17);
            this.cbTrackLoaderItem.TabIndex = 20;
            this.cbTrackLoaderItem.Text = "Track-Loader Item";
            this.cbTrackLoaderItem.UseVisualStyleBackColor = true;
            this.cbTrackLoaderItem.CheckedChanged += new System.EventHandler(this.cbTrackLoaderItem_CheckedChanged);
            // 
            // cbCopyToWinAPERomFolder
            // 
            this.cbCopyToWinAPERomFolder.AutoSize = true;
            this.cbCopyToWinAPERomFolder.Location = new System.Drawing.Point(9, 162);
            this.cbCopyToWinAPERomFolder.Name = "cbCopyToWinAPERomFolder";
            this.cbCopyToWinAPERomFolder.Size = new System.Drawing.Size(185, 17);
            this.cbCopyToWinAPERomFolder.TabIndex = 19;
            this.cbCopyToWinAPERomFolder.Text = "Copy file to WinAPE\'s ROM folder";
            this.cbCopyToWinAPERomFolder.UseVisualStyleBackColor = true;
            this.cbCopyToWinAPERomFolder.CheckedChanged += new System.EventHandler(this.cbCopyToWinAPERomFolder_CheckedChanged);
            // 
            // AmsdosFilename
            // 
            this.AmsdosFilename.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.AmsdosFilename.Location = new System.Drawing.Point(181, 21);
            this.AmsdosFilename.Name = "AmsdosFilename";
            this.AmsdosFilename.Size = new System.Drawing.Size(167, 20);
            this.AmsdosFilename.TabIndex = 18;
            this.AmsdosFilename.TextChanged += new System.EventHandler(this.AmsdosFilename_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "AMSDOS Filename";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "BINARY",
            "BASIC"});
            this.cbType.Location = new System.Drawing.Point(181, 47);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(167, 21);
            this.cbType.TabIndex = 16;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // ExecAdress
            // 
            this.ExecAdress.Location = new System.Drawing.Point(181, 101);
            this.ExecAdress.Name = "ExecAdress";
            this.ExecAdress.Size = new System.Drawing.Size(167, 20);
            this.ExecAdress.TabIndex = 15;
            this.ExecAdress.Text = "0x0000";
            this.ExecAdress.TextChanged += new System.EventHandler(this.ExecAdress_TextChanged);
            // 
            // LoadAdress
            // 
            this.LoadAdress.Location = new System.Drawing.Point(181, 75);
            this.LoadAdress.Name = "LoadAdress";
            this.LoadAdress.Size = new System.Drawing.Size(167, 20);
            this.LoadAdress.TabIndex = 14;
            this.LoadAdress.Text = "0x0000";
            this.LoadAdress.TextChanged += new System.EventHandler(this.LoadAdress_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Execution Adress (Hexadecimal)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Load Adress (Hexadecimal)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Type";
            // 
            // Reorder
            // 
            this.Reorder.Location = new System.Drawing.Point(127, 38);
            this.Reorder.Name = "Reorder";
            this.Reorder.Size = new System.Drawing.Size(75, 23);
            this.Reorder.TabIndex = 10;
            this.Reorder.Text = "Order...";
            this.Reorder.UseVisualStyleBackColor = true;
            this.Reorder.Click += new System.EventHandler(this.Reorder_Click);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "View";
            this.Size = new System.Drawing.Size(905, 545);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbFileProperties.ResumeLayout(false);
            this.gbFileProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button MoveDown;
        private System.Windows.Forms.Button MoveUp;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbFileProperties;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox ExecAdress;
        private System.Windows.Forms.TextBox LoadAdress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AmsdosFilename;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbHFE;
        private System.Windows.Forms.CheckBox cbCopyToWinAPERomFolder;
        private System.Windows.Forms.CheckBox cbTrackLoaderDisc;
        private System.Windows.Forms.CheckBox cbTrackLoaderItem;
        private System.Windows.Forms.Label NbFiles;
        private System.Windows.Forms.Button Reorder;

    }
}
