namespace CPCBigFile.View
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FilesInBank = new System.Windows.Forms.CheckBox();
            this.BaseAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TruncateFile = new System.Windows.Forms.CheckBox();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.ItemAddress = new System.Windows.Forms.TextBox();
            this.cbItemAddress = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MoveDown
            // 
            this.MoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MoveDown.Location = new System.Drawing.Point(191, 476);
            this.MoveDown.Name = "MoveDown";
            this.MoveDown.Size = new System.Drawing.Size(73, 23);
            this.MoveDown.TabIndex = 6;
            this.MoveDown.Text = "Move Down";
            this.MoveDown.UseVisualStyleBackColor = true;
            this.MoveDown.Click += new System.EventHandler(this.MoveDown_Click);
            // 
            // MoveUp
            // 
            this.MoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MoveUp.Location = new System.Drawing.Point(123, 476);
            this.MoveUp.Name = "MoveUp";
            this.MoveUp.Size = new System.Drawing.Size(62, 23);
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
            this.lvFiles.CheckBoxes = true;
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(3, 85);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(585, 385);
            this.lvFiles.TabIndex = 4;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvFiles_ItemChecked);
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.cbItemAddress);
            this.groupBox2.Controls.Add(this.ItemAddress);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.FilesInBank);
            this.groupBox2.Controls.Add(this.BaseAddress);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.TruncateFile);
            this.groupBox2.Controls.Add(this.MoveDown);
            this.groupBox2.Controls.Add(this.MoveUp);
            this.groupBox2.Controls.Add(this.lvFiles);
            this.groupBox2.Controls.Add(this.Remove);
            this.groupBox2.Controls.Add(this.Add);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(591, 505);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resource(s)";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(372, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Item-check in list= aligned to next 256 bytes";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FilesInBank
            // 
            this.FilesInBank.AutoSize = true;
            this.FilesInBank.Location = new System.Drawing.Point(6, 62);
            this.FilesInBank.Name = "FilesInBank";
            this.FilesInBank.Size = new System.Drawing.Size(86, 17);
            this.FilesInBank.TabIndex = 17;
            this.FilesInBank.Text = "Files in Bank";
            this.FilesInBank.UseVisualStyleBackColor = true;
            this.FilesInBank.CheckedChanged += new System.EventHandler(this.filesInBank_CheckedChanged);
            // 
            // BaseAddress
            // 
            this.BaseAddress.Location = new System.Drawing.Point(191, 13);
            this.BaseAddress.Name = "BaseAddress";
            this.BaseAddress.Size = new System.Drawing.Size(167, 20);
            this.BaseAddress.TabIndex = 16;
            this.BaseAddress.Text = "0x0000";
            this.BaseAddress.TextChanged += new System.EventHandler(this.BaseAddress_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Base Adress (Hexadecimal)";
            // 
            // TruncateFile
            // 
            this.TruncateFile.AutoSize = true;
            this.TruncateFile.Location = new System.Drawing.Point(6, 39);
            this.TruncateFile.Name = "TruncateFile";
            this.TruncateFile.Size = new System.Drawing.Size(179, 17);
            this.TruncateFile.TabIndex = 7;
            this.TruncateFile.Text = "Truncate files on bank boundary";
            this.TruncateFile.UseVisualStyleBackColor = true;
            this.TruncateFile.CheckedChanged += new System.EventHandler(this.TruncateFile_CheckedChanged);
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(61, 476);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(56, 23);
            this.Remove.TabIndex = 3;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Add.Location = new System.Drawing.Point(3, 476);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(52, 23);
            this.Add.TabIndex = 2;
            this.Add.Text = "Add...";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // ItemAddress
            // 
            this.ItemAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemAddress.Location = new System.Drawing.Point(463, 478);
            this.ItemAddress.Name = "ItemAddress";
            this.ItemAddress.Size = new System.Drawing.Size(125, 20);
            this.ItemAddress.TabIndex = 20;
            this.ItemAddress.Text = "0x0000";
            this.ItemAddress.TextChanged += new System.EventHandler(this.ItemAddress_TextChanged);
            // 
            // cbItemAddress
            // 
            this.cbItemAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbItemAddress.AutoSize = true;
            this.cbItemAddress.Location = new System.Drawing.Point(362, 480);
            this.cbItemAddress.Name = "cbItemAddress";
            this.cbItemAddress.Size = new System.Drawing.Size(95, 17);
            this.cbItemAddress.TabIndex = 21;
            this.cbItemAddress.Text = "Address (Hex.)";
            this.cbItemAddress.UseVisualStyleBackColor = true;
            this.cbItemAddress.CheckedChanged += new System.EventHandler(this.cbItemAddress_CheckedChanged);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "View";
            this.Size = new System.Drawing.Size(591, 505);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MoveDown;
        private System.Windows.Forms.Button MoveUp;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.CheckBox TruncateFile;
        private System.Windows.Forms.TextBox BaseAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox FilesInBank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbItemAddress;
        private System.Windows.Forms.TextBox ItemAddress;


    }
}
