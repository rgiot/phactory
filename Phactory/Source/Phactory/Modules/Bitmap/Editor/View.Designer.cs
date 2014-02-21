namespace CPCBitmap.View
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
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.cbGroupDelimiter = new System.Windows.Forms.CheckBox();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.Add = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbMaskBitmap = new System.Windows.Forms.CheckBox();
            this.cbColorMask = new System.Windows.Forms.CheckBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.BrowseMask = new System.Windows.Forms.Button();
            this.isMerge = new System.Windows.Forms.CheckBox();
            this.pickColor = new System.Windows.Forms.Button();
            this.isCloudSprite = new System.Windows.Forms.CheckBox();
            this.MaskBitmap = new System.Windows.Forms.TextBox();
            this.BrowseCloud = new System.Windows.Forms.Button();
            this.CloudBitmap = new System.Windows.Forms.TextBox();
            this.tbStartIndex = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.MaskMergeBitmap = new System.Windows.Forms.TextBox();
            this.PanelColorMask = new System.Windows.Forms.Panel();
            this.BrowseMergeMask = new System.Windows.Forms.Button();
            this.MergePosX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MergePosY = new System.Windows.Forms.TextBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.isFullScreenTitle = new System.Windows.Forms.CheckBox();
            this.MaskPenIndex = new System.Windows.Forms.TextBox();
            this.BrowseFadeMask = new System.Windows.Forms.Button();
            this.MaskFadeBitmap = new System.Windows.Forms.TextBox();
            this.isFade = new System.Windows.Forms.CheckBox();
            this.ImportPlusButton = new System.Windows.Forms.Button();
            this.ExportPlusButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.processedBox = new System.Windows.Forms.PictureBox();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.PaletteLabel = new System.Windows.Forms.Label();
            this.lvPalette = new System.Windows.Forms.ListView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.FontAlignOnCharaterLine = new System.Windows.Forms.CheckBox();
            this.FontCharWidthInBytes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processedBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.splitContainer3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 648);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resource(s)";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 16);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.cbGroupDelimiter);
            this.splitContainer3.Panel1.Controls.Add(this.lvFiles);
            this.splitContainer3.Panel1.Controls.Add(this.Add);
            this.splitContainer3.Panel1.Controls.Add(this.Remove);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(362, 629);
            this.splitContainer3.SplitterDistance = 289;
            this.splitContainer3.TabIndex = 30;
            // 
            // cbGroupDelimiter
            // 
            this.cbGroupDelimiter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbGroupDelimiter.AutoSize = true;
            this.cbGroupDelimiter.Location = new System.Drawing.Point(235, 269);
            this.cbGroupDelimiter.Name = "cbGroupDelimiter";
            this.cbGroupDelimiter.Size = new System.Drawing.Size(98, 17);
            this.cbGroupDelimiter.TabIndex = 5;
            this.cbGroupDelimiter.Text = "Group Delimiter";
            this.cbGroupDelimiter.UseVisualStyleBackColor = true;
            this.cbGroupDelimiter.CheckedChanged += new System.EventHandler(this.cbGroupDelimiter_CheckedChanged);
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.GridLines = true;
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(3, 3);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(353, 254);
            this.lvFiles.TabIndex = 4;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Add.Location = new System.Drawing.Point(3, 265);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(103, 23);
            this.Add.TabIndex = 2;
            this.Add.Text = "Add...";
            this.Add.UseVisualStyleBackColor = false;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(114, 265);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(110, 23);
            this.Remove.TabIndex = 3;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.FontCharWidthInBytes);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.FontAlignOnCharaterLine);
            this.panel1.Controls.Add(this.cbMaskBitmap);
            this.panel1.Controls.Add(this.cbColorMask);
            this.panel1.Controls.Add(this.comboBoxType);
            this.panel1.Controls.Add(this.BrowseMask);
            this.panel1.Controls.Add(this.isMerge);
            this.panel1.Controls.Add(this.pickColor);
            this.panel1.Controls.Add(this.isCloudSprite);
            this.panel1.Controls.Add(this.MaskBitmap);
            this.panel1.Controls.Add(this.BrowseCloud);
            this.panel1.Controls.Add(this.CloudBitmap);
            this.panel1.Controls.Add(this.tbStartIndex);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.MaskMergeBitmap);
            this.panel1.Controls.Add(this.PanelColorMask);
            this.panel1.Controls.Add(this.BrowseMergeMask);
            this.panel1.Controls.Add(this.MergePosX);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.MergePosY);
            this.panel1.Controls.Add(this.RefreshButton);
            this.panel1.Controls.Add(this.isFullScreenTitle);
            this.panel1.Controls.Add(this.MaskPenIndex);
            this.panel1.Controls.Add(this.BrowseFadeMask);
            this.panel1.Controls.Add(this.MaskFadeBitmap);
            this.panel1.Controls.Add(this.isFade);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 336);
            this.panel1.TabIndex = 45;
            // 
            // cbMaskBitmap
            // 
            this.cbMaskBitmap.AutoSize = true;
            this.cbMaskBitmap.Location = new System.Drawing.Point(3, 192);
            this.cbMaskBitmap.Name = "cbMaskBitmap";
            this.cbMaskBitmap.Size = new System.Drawing.Size(87, 17);
            this.cbMaskBitmap.TabIndex = 46;
            this.cbMaskBitmap.Text = "Mask Bitmap";
            this.cbMaskBitmap.UseVisualStyleBackColor = true;
            this.cbMaskBitmap.CheckedChanged += new System.EventHandler(this.ItemType_CheckedChanged);
            // 
            // cbColorMask
            // 
            this.cbColorMask.AutoSize = true;
            this.cbColorMask.Location = new System.Drawing.Point(3, 167);
            this.cbColorMask.Name = "cbColorMask";
            this.cbColorMask.Size = new System.Drawing.Size(79, 17);
            this.cbColorMask.TabIndex = 45;
            this.cbColorMask.Text = "Color Mask";
            this.cbColorMask.UseVisualStyleBackColor = true;
            this.cbColorMask.CheckedChanged += new System.EventHandler(this.ItemType_CheckedChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Full-Screen",
            "Sprite (value, value, value..)",
            "Sprite (screen ptr, value)",
            "Sprite (posx, value, mask-type)",
            "Sprite (opcodes)",
            "Sprite Fullscreen (&800-aligned: value, value, value..)",
            "Font"});
            this.comboBoxType.Location = new System.Drawing.Point(3, 3);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(356, 21);
            this.comboBoxType.TabIndex = 32;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // BrowseMask
            // 
            this.BrowseMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseMask.Location = new System.Drawing.Point(333, 188);
            this.BrowseMask.Name = "BrowseMask";
            this.BrowseMask.Size = new System.Drawing.Size(26, 23);
            this.BrowseMask.TabIndex = 16;
            this.BrowseMask.Text = "...";
            this.BrowseMask.UseVisualStyleBackColor = false;
            this.BrowseMask.Click += new System.EventHandler(this.BrowseMask_Click);
            // 
            // isMerge
            // 
            this.isMerge.AutoSize = true;
            this.isMerge.Location = new System.Drawing.Point(3, 84);
            this.isMerge.Name = "isMerge";
            this.isMerge.Size = new System.Drawing.Size(87, 17);
            this.isMerge.TabIndex = 19;
            this.isMerge.Text = "Merge with...";
            this.isMerge.UseVisualStyleBackColor = true;
            this.isMerge.CheckedChanged += new System.EventHandler(this.isMerge_CheckedChanged);
            // 
            // pickColor
            // 
            this.pickColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pickColor.Location = new System.Drawing.Point(276, 163);
            this.pickColor.Name = "pickColor";
            this.pickColor.Size = new System.Drawing.Size(83, 23);
            this.pickColor.TabIndex = 22;
            this.pickColor.Text = "Pick Color...";
            this.pickColor.UseVisualStyleBackColor = true;
            this.pickColor.Click += new System.EventHandler(this.pickColor_Click);
            // 
            // isCloudSprite
            // 
            this.isCloudSprite.AutoSize = true;
            this.isCloudSprite.Location = new System.Drawing.Point(3, 56);
            this.isCloudSprite.Name = "isCloudSprite";
            this.isCloudSprite.Size = new System.Drawing.Size(83, 17);
            this.isCloudSprite.TabIndex = 28;
            this.isCloudSprite.Text = "Cloud Sprite";
            this.isCloudSprite.UseVisualStyleBackColor = true;
            this.isCloudSprite.CheckedChanged += new System.EventHandler(this.isCloudSprite_CheckedChanged);
            // 
            // MaskBitmap
            // 
            this.MaskBitmap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MaskBitmap.Location = new System.Drawing.Point(114, 190);
            this.MaskBitmap.Name = "MaskBitmap";
            this.MaskBitmap.Size = new System.Drawing.Size(213, 20);
            this.MaskBitmap.TabIndex = 15;
            // 
            // BrowseCloud
            // 
            this.BrowseCloud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseCloud.Location = new System.Drawing.Point(333, 54);
            this.BrowseCloud.Name = "BrowseCloud";
            this.BrowseCloud.Size = new System.Drawing.Size(26, 23);
            this.BrowseCloud.TabIndex = 30;
            this.BrowseCloud.Text = "...";
            this.BrowseCloud.UseVisualStyleBackColor = false;
            this.BrowseCloud.Click += new System.EventHandler(this.BrowseCloud_Click);
            // 
            // CloudBitmap
            // 
            this.CloudBitmap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CloudBitmap.Location = new System.Drawing.Point(114, 56);
            this.CloudBitmap.Name = "CloudBitmap";
            this.CloudBitmap.Size = new System.Drawing.Size(213, 20);
            this.CloudBitmap.TabIndex = 29;
            // 
            // tbStartIndex
            // 
            this.tbStartIndex.Location = new System.Drawing.Point(114, 30);
            this.tbStartIndex.Name = "tbStartIndex";
            this.tbStartIndex.Size = new System.Drawing.Size(39, 20);
            this.tbStartIndex.TabIndex = 2;
            this.tbStartIndex.Text = "0";
            this.tbStartIndex.TextChanged += new System.EventHandler(this.tbStartIndex_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Palette Start Index";
            // 
            // MaskMergeBitmap
            // 
            this.MaskMergeBitmap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MaskMergeBitmap.Location = new System.Drawing.Point(114, 82);
            this.MaskMergeBitmap.Name = "MaskMergeBitmap";
            this.MaskMergeBitmap.Size = new System.Drawing.Size(213, 20);
            this.MaskMergeBitmap.TabIndex = 24;
            // 
            // PanelColorMask
            // 
            this.PanelColorMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelColorMask.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelColorMask.Location = new System.Drawing.Point(114, 163);
            this.PanelColorMask.Name = "PanelColorMask";
            this.PanelColorMask.Size = new System.Drawing.Size(156, 21);
            this.PanelColorMask.TabIndex = 21;
            // 
            // BrowseMergeMask
            // 
            this.BrowseMergeMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseMergeMask.Location = new System.Drawing.Point(333, 80);
            this.BrowseMergeMask.Name = "BrowseMergeMask";
            this.BrowseMergeMask.Size = new System.Drawing.Size(26, 23);
            this.BrowseMergeMask.TabIndex = 25;
            this.BrowseMergeMask.Text = "...";
            this.BrowseMergeMask.UseVisualStyleBackColor = false;
            this.BrowseMergeMask.Click += new System.EventHandler(this.BrowseMergeMask_Click);
            // 
            // MergePosX
            // 
            this.MergePosX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MergePosX.Location = new System.Drawing.Point(166, 111);
            this.MergePosX.Name = "MergePosX";
            this.MergePosX.Size = new System.Drawing.Size(39, 20);
            this.MergePosX.TabIndex = 20;
            this.MergePosX.TextChanged += new System.EventHandler(this.MergePosX_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Y";
            // 
            // MergePosY
            // 
            this.MergePosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MergePosY.Location = new System.Drawing.Point(231, 111);
            this.MergePosY.Name = "MergePosY";
            this.MergePosY.Size = new System.Drawing.Size(39, 20);
            this.MergePosY.TabIndex = 21;
            this.MergePosY.TextChanged += new System.EventHandler(this.MergePosY_TextChanged);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshButton.Location = new System.Drawing.Point(276, 108);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(83, 23);
            this.RefreshButton.TabIndex = 27;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = false;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // isFullScreenTitle
            // 
            this.isFullScreenTitle.AutoSize = true;
            this.isFullScreenTitle.Location = new System.Drawing.Point(3, 218);
            this.isFullScreenTitle.Name = "isFullScreenTitle";
            this.isFullScreenTitle.Size = new System.Drawing.Size(103, 17);
            this.isFullScreenTitle.TabIndex = 32;
            this.isFullScreenTitle.Text = "Mask Pen Index";
            this.isFullScreenTitle.UseVisualStyleBackColor = true;
            this.isFullScreenTitle.CheckedChanged += new System.EventHandler(this.isFullScreenTitle_CheckedChanged);
            // 
            // MaskPenIndex
            // 
            this.MaskPenIndex.Location = new System.Drawing.Point(114, 216);
            this.MaskPenIndex.Name = "MaskPenIndex";
            this.MaskPenIndex.Size = new System.Drawing.Size(39, 20);
            this.MaskPenIndex.TabIndex = 33;
            this.MaskPenIndex.TextChanged += new System.EventHandler(this.MaskPenIndex_TextChanged);
            // 
            // BrowseFadeMask
            // 
            this.BrowseFadeMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseFadeMask.Location = new System.Drawing.Point(333, 135);
            this.BrowseFadeMask.Name = "BrowseFadeMask";
            this.BrowseFadeMask.Size = new System.Drawing.Size(26, 23);
            this.BrowseFadeMask.TabIndex = 37;
            this.BrowseFadeMask.Text = "...";
            this.BrowseFadeMask.UseVisualStyleBackColor = false;
            this.BrowseFadeMask.Click += new System.EventHandler(this.BrowseFadeMask_Click);
            // 
            // MaskFadeBitmap
            // 
            this.MaskFadeBitmap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MaskFadeBitmap.Location = new System.Drawing.Point(114, 137);
            this.MaskFadeBitmap.Name = "MaskFadeBitmap";
            this.MaskFadeBitmap.Size = new System.Drawing.Size(213, 20);
            this.MaskFadeBitmap.TabIndex = 36;
            // 
            // isFade
            // 
            this.isFade.AutoSize = true;
            this.isFade.Location = new System.Drawing.Point(3, 139);
            this.isFade.Name = "isFade";
            this.isFade.Size = new System.Drawing.Size(81, 17);
            this.isFade.TabIndex = 35;
            this.isFade.Text = "Fade with...";
            this.isFade.UseVisualStyleBackColor = true;
            this.isFade.CheckedChanged += new System.EventHandler(this.isFade_CheckedChanged);
            // 
            // ImportPlusButton
            // 
            this.ImportPlusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportPlusButton.Location = new System.Drawing.Point(251, 0);
            this.ImportPlusButton.Name = "ImportPlusButton";
            this.ImportPlusButton.Size = new System.Drawing.Size(82, 23);
            this.ImportPlusButton.TabIndex = 31;
            this.ImportPlusButton.Text = "Import Plus...";
            this.ImportPlusButton.UseVisualStyleBackColor = true;
            this.ImportPlusButton.Click += new System.EventHandler(this.ImportPlusButton_Click);
            // 
            // ExportPlusButton
            // 
            this.ExportPlusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportPlusButton.Location = new System.Drawing.Point(339, 0);
            this.ExportPlusButton.Name = "ExportPlusButton";
            this.ExportPlusButton.Size = new System.Drawing.Size(82, 23);
            this.ExportPlusButton.TabIndex = 30;
            this.ExportPlusButton.Text = "Export Plus...";
            this.ExportPlusButton.UseVisualStyleBackColor = true;
            this.ExportPlusButton.Click += new System.EventHandler(this.ExportPlusButton_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.processedBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbMode);
            this.splitContainer1.Panel2.Controls.Add(this.PaletteLabel);
            this.splitContainer1.Panel2.Controls.Add(this.lvPalette);
            this.splitContainer1.Panel2.Controls.Add(this.ImportPlusButton);
            this.splitContainer1.Panel2.Controls.Add(this.ExportPlusButton);
            this.splitContainer1.Size = new System.Drawing.Size(424, 648);
            this.splitContainer1.SplitterDistance = 329;
            this.splitContainer1.TabIndex = 3;
            // 
            // processedBox
            // 
            this.processedBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.processedBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.processedBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processedBox.Location = new System.Drawing.Point(0, 0);
            this.processedBox.Name = "processedBox";
            this.processedBox.Size = new System.Drawing.Size(424, 329);
            this.processedBox.TabIndex = 1;
            this.processedBox.TabStop = false;
            // 
            // cbMode
            // 
            this.cbMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "MODE0",
            "MODE1",
            "MODE2"});
            this.cbMode.Location = new System.Drawing.Point(180, 1);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(65, 21);
            this.cbMode.TabIndex = 32;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // PaletteLabel
            // 
            this.PaletteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PaletteLabel.BackColor = System.Drawing.Color.Transparent;
            this.PaletteLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PaletteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PaletteLabel.Location = new System.Drawing.Point(0, 0);
            this.PaletteLabel.Name = "PaletteLabel";
            this.PaletteLabel.Size = new System.Drawing.Size(174, 23);
            this.PaletteLabel.TabIndex = 3;
            this.PaletteLabel.Text = "Palette";
            this.PaletteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvPalette
            // 
            this.lvPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPalette.GridLines = true;
            this.lvPalette.Location = new System.Drawing.Point(0, 26);
            this.lvPalette.Name = "lvPalette";
            this.lvPalette.Size = new System.Drawing.Size(424, 286);
            this.lvPalette.TabIndex = 0;
            this.lvPalette.UseCompatibleStateImageBehavior = false;
            this.lvPalette.View = System.Windows.Forms.View.Details;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(796, 648);
            this.splitContainer2.SplitterDistance = 368;
            this.splitContainer2.TabIndex = 0;
            // 
            // FontAlignOnCharaterLine
            // 
            this.FontAlignOnCharaterLine.AutoSize = true;
            this.FontAlignOnCharaterLine.Location = new System.Drawing.Point(159, 244);
            this.FontAlignOnCharaterLine.Name = "FontAlignOnCharaterLine";
            this.FontAlignOnCharaterLine.Size = new System.Drawing.Size(136, 17);
            this.FontAlignOnCharaterLine.TabIndex = 47;
            this.FontAlignOnCharaterLine.Text = "Align on Character Line";
            this.FontAlignOnCharaterLine.UseVisualStyleBackColor = true;
            this.FontAlignOnCharaterLine.CheckedChanged += new System.EventHandler(this.FontAlignOnCharaterLine_CheckedChanged);
            // 
            // FontCharWidthInBytes
            // 
            this.FontCharWidthInBytes.Location = new System.Drawing.Point(114, 242);
            this.FontCharWidthInBytes.Name = "FontCharWidthInBytes";
            this.FontCharWidthInBytes.Size = new System.Drawing.Size(39, 20);
            this.FontCharWidthInBytes.TabIndex = 49;
            this.FontCharWidthInBytes.Text = "0";
            this.FontCharWidthInBytes.TextChanged += new System.EventHandler(this.FontCharWidthInBytes_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "CharWidth (bytes)";
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Name = "View";
            this.Size = new System.Drawing.Size(796, 648);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.processedBox)).EndInit();
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
        private System.Windows.Forms.Button BrowseMask;
        private System.Windows.Forms.TextBox MaskBitmap;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.ListView lvPalette;
        public System.Windows.Forms.PictureBox processedBox;
        private System.Windows.Forms.Button pickColor;
        private System.Windows.Forms.Panel PanelColorMask;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox tbStartIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox MaskMergeBitmap;
        private System.Windows.Forms.Button BrowseMergeMask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MergePosY;
        private System.Windows.Forms.TextBox MergePosX;
        private System.Windows.Forms.CheckBox isMerge;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.TextBox CloudBitmap;
        private System.Windows.Forms.Button BrowseCloud;
        private System.Windows.Forms.CheckBox isCloudSprite;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label PaletteLabel;
        private System.Windows.Forms.CheckBox cbGroupDelimiter;
        private System.Windows.Forms.CheckBox isFullScreenTitle;
        private System.Windows.Forms.Button ImportPlusButton;
        private System.Windows.Forms.Button ExportPlusButton;
        private System.Windows.Forms.TextBox MaskPenIndex;
        private System.Windows.Forms.Button BrowseFadeMask;
        private System.Windows.Forms.TextBox MaskFadeBitmap;
        private System.Windows.Forms.CheckBox isFade;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbMaskBitmap;
        private System.Windows.Forms.CheckBox cbColorMask;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.TextBox FontCharWidthInBytes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox FontAlignOnCharaterLine;
    }
}
