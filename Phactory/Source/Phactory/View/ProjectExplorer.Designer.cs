namespace Project.View
{
    partial class ProjectExplorer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorer));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addExistingResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addNewFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAllBitmapResourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripResource = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.cleanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.setAsProjectStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.copyPathInClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyContainingFolderIntoClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.openContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithDefaultApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewFilterToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.projectPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFolder.SuspendLayout();
            this.contextMenuStripResource.SuspendLayout();
            this.contextMenuStripProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.LabelEdit = true;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.ShowLines = false;
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(330, 331);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_BeforeLabelEdit);
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
            this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCollapse);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.treeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDoubleClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "VSFolder_closed");
            this.imageList.Images.SetKeyName(1, "VSFolder_open");
            this.imageList.Images.SetKeyName(2, "FileProject");
            this.imageList.Images.SetKeyName(3, "VSProject_generatedfile");
            this.imageList.Images.SetKeyName(4, "FileGeneric");
            this.imageList.Images.SetKeyName(5, "FileASM");
            this.imageList.Images.SetKeyName(6, "FileC");
            this.imageList.Images.SetKeyName(7, "FileH");
            this.imageList.Images.SetKeyName(8, "VSProject_imagefile");
            this.imageList.Images.SetKeyName(9, "VSObject_Type");
            this.imageList.Images.SetKeyName(10, "FileCPP");
            this.imageList.Images.SetKeyName(11, "FileTXT");
            this.imageList.Images.SetKeyName(12, "FileProp");
            // 
            // contextMenuStripFolder
            // 
            this.contextMenuStripFolder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.contextMenuStripFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewResourceToolStripMenuItem,
            this.addExistingResourceToolStripMenuItem,
            this.toolStripSeparator1,
            this.addNewFilterToolStripMenuItem,
            this.toolStripSeparator2,
            this.removeToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripSeparator9,
            this.expandAllToolStripMenuItem,
            this.toolStripSeparator4,
            this.exportAllBitmapResourcesToolStripMenuItem});
            this.contextMenuStripFolder.Name = "contextMenuStripFolder";
            this.contextMenuStripFolder.ShowImageMargin = false;
            this.contextMenuStripFolder.Size = new System.Drawing.Size(214, 182);
            // 
            // addNewResourceToolStripMenuItem
            // 
            this.addNewResourceToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addNewResourceToolStripMenuItem.Image = global::Phactory.Properties.Resources.NewDocument;
            this.addNewResourceToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.addNewResourceToolStripMenuItem.Name = "addNewResourceToolStripMenuItem";
            this.addNewResourceToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.addNewResourceToolStripMenuItem.Text = "Add New Resource...";
            this.addNewResourceToolStripMenuItem.Click += new System.EventHandler(this.addNewResourceToolStripMenuItem_Click);
            // 
            // addExistingResourceToolStripMenuItem
            // 
            this.addExistingResourceToolStripMenuItem.Name = "addExistingResourceToolStripMenuItem";
            this.addExistingResourceToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.addExistingResourceToolStripMenuItem.Text = "Add Existing Resource...";
            this.addExistingResourceToolStripMenuItem.Click += new System.EventHandler(this.addExistingResourceToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // addNewFilterToolStripMenuItem
            // 
            this.addNewFilterToolStripMenuItem.Name = "addNewFilterToolStripMenuItem";
            this.addNewFilterToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.addNewFilterToolStripMenuItem.Text = "Add New Filter";
            this.addNewFilterToolStripMenuItem.Click += new System.EventHandler(this.addNewFilterToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeFolderToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(210, 6);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(210, 6);
            // 
            // exportAllBitmapResourcesToolStripMenuItem
            // 
            this.exportAllBitmapResourcesToolStripMenuItem.Name = "exportAllBitmapResourcesToolStripMenuItem";
            this.exportAllBitmapResourcesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.exportAllBitmapResourcesToolStripMenuItem.Text = "Export all Bitmap Resource(s)...";
            this.exportAllBitmapResourcesToolStripMenuItem.Click += new System.EventHandler(this.exportAllBitmapResourcesToolStripMenuItem_Click);
            // 
            // contextMenuStripResource
            // 
            this.contextMenuStripResource.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.contextMenuStripResource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.toolStripSeparator7,
            this.cleanToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.runToolStripMenuItem,
            this.toolStripSeparator6,
            this.removeToolStripMenuItem1,
            this.renameToolStripMenuItem1,
            this.toolStripSeparator3,
            this.setAsProjectStartupToolStripMenuItem,
            this.toolStripSeparator10,
            this.copyPathInClipboardToolStripMenuItem,
            this.copyContainingFolderIntoClipboardToolStripMenuItem,
            this.toolStripSeparator12,
            this.openContainingFolderToolStripMenuItem,
            this.openWithDefaultApplicationToolStripMenuItem,
            this.toolStripSeparator5,
            this.expandAllToolStripMenuItem1});
            this.contextMenuStripResource.Name = "contextMenuStripResource";
            this.contextMenuStripResource.ShowImageMargin = false;
            this.contextMenuStripResource.Size = new System.Drawing.Size(242, 304);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(238, 6);
            // 
            // cleanToolStripMenuItem
            // 
            this.cleanToolStripMenuItem.Name = "cleanToolStripMenuItem";
            this.cleanToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.cleanToolStripMenuItem.Text = "Clean";
            this.cleanToolStripMenuItem.Click += new System.EventHandler(this.cleanToolStripMenuItem_Click);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.compileToolStripMenuItem.Image = global::Phactory.Properties.Resources.build3;
            this.compileToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.compileToolStripMenuItem.Text = "Build";
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.runToolStripMenuItem.Image = global::Phactory.Properties.Resources.Run;
            this.runToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(238, 6);
            // 
            // removeToolStripMenuItem1
            // 
            this.removeToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.removeToolStripMenuItem1.Image = global::Phactory.Properties.Resources.Delete;
            this.removeToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            this.removeToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
            this.removeToolStripMenuItem1.Text = "Remove";
            this.removeToolStripMenuItem1.Click += new System.EventHandler(this.removeToolStripMenuItem1_Click);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
            this.renameToolStripMenuItem1.Text = "Rename";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameToolStripMenuItem1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(238, 6);
            // 
            // setAsProjectStartupToolStripMenuItem
            // 
            this.setAsProjectStartupToolStripMenuItem.Name = "setAsProjectStartupToolStripMenuItem";
            this.setAsProjectStartupToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.setAsProjectStartupToolStripMenuItem.Text = "Set as Project Startup";
            this.setAsProjectStartupToolStripMenuItem.Click += new System.EventHandler(this.setAsProjectStartupToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(238, 6);
            // 
            // copyPathInClipboardToolStripMenuItem
            // 
            this.copyPathInClipboardToolStripMenuItem.Name = "copyPathInClipboardToolStripMenuItem";
            this.copyPathInClipboardToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.copyPathInClipboardToolStripMenuItem.Text = "Copy Path into Clipboard";
            this.copyPathInClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyFullPathToClipboard_Click);
            // 
            // copyContainingFolderIntoClipboardToolStripMenuItem
            // 
            this.copyContainingFolderIntoClipboardToolStripMenuItem.Name = "copyContainingFolderIntoClipboardToolStripMenuItem";
            this.copyContainingFolderIntoClipboardToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.copyContainingFolderIntoClipboardToolStripMenuItem.Text = "Copy Containing Folder into Clipboard";
            this.copyContainingFolderIntoClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyContainingFolderToClipboard_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(238, 6);
            // 
            // openContainingFolderToolStripMenuItem
            // 
            this.openContainingFolderToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.openContainingFolderToolStripMenuItem.Image = global::Phactory.Properties.Resources.openfolder_24;
            this.openContainingFolderToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
            this.openContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.openContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            this.openContainingFolderToolStripMenuItem.Click += new System.EventHandler(this.openContainingFolderToolStripMenuItem_Click);
            // 
            // openWithDefaultApplicationToolStripMenuItem
            // 
            this.openWithDefaultApplicationToolStripMenuItem.Name = "openWithDefaultApplicationToolStripMenuItem";
            this.openWithDefaultApplicationToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.openWithDefaultApplicationToolStripMenuItem.Text = "Open with Default Application";
            this.openWithDefaultApplicationToolStripMenuItem.Click += new System.EventHandler(this.openWithDefaultApplicationToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(238, 6);
            // 
            // expandAllToolStripMenuItem1
            // 
            this.expandAllToolStripMenuItem1.Name = "expandAllToolStripMenuItem1";
            this.expandAllToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
            this.expandAllToolStripMenuItem1.Text = "Expand All";
            this.expandAllToolStripMenuItem1.Click += new System.EventHandler(this.expandAllToolStripMenuItem1_Click);
            // 
            // contextMenuStripProject
            // 
            this.contextMenuStripProject.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.contextMenuStripProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewFilterToolStripMenuItem1,
            this.toolStripSeparator8,
            this.renameToolStripMenuItem2,
            this.toolStripSeparator13,
            this.projectPropertiesToolStripMenuItem});
            this.contextMenuStripProject.Name = "contextMenuStripProject";
            this.contextMenuStripProject.ShowImageMargin = false;
            this.contextMenuStripProject.Size = new System.Drawing.Size(159, 82);
            // 
            // addNewFilterToolStripMenuItem1
            // 
            this.addNewFilterToolStripMenuItem1.Name = "addNewFilterToolStripMenuItem1";
            this.addNewFilterToolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.addNewFilterToolStripMenuItem1.Text = "Add New Filter";
            this.addNewFilterToolStripMenuItem1.Click += new System.EventHandler(this.addNewFilterToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(155, 6);
            // 
            // renameToolStripMenuItem2
            // 
            this.renameToolStripMenuItem2.Name = "renameToolStripMenuItem2";
            this.renameToolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
            this.renameToolStripMenuItem2.Text = "Rename";
            this.renameToolStripMenuItem2.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(155, 6);
            // 
            // projectPropertiesToolStripMenuItem
            // 
            this.projectPropertiesToolStripMenuItem.Name = "projectPropertiesToolStripMenuItem";
            this.projectPropertiesToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.projectPropertiesToolStripMenuItem.Text = "Project Properties...";
            this.projectPropertiesToolStripMenuItem.Click += new System.EventHandler(this.projectPropertiesToolStripMenuItem_Click);
            // 
            // ProjectExplorer
            // 
            this.AutoHidePortion = 0.3D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 331);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.treeView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectExplorer";
            this.Text = "Project Explorer";
            this.contextMenuStripFolder.ResumeLayout(false);
            this.contextMenuStripResource.ResumeLayout(false);
            this.contextMenuStripProject.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFolder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripResource;
        private System.Windows.Forms.ToolStripMenuItem addNewResourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addExistingResourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setAsProjectStartupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openContainingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWithDefaultApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripProject;
        private System.Windows.Forms.ToolStripMenuItem addNewFilterToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exportAllBitmapResourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPathInClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyContainingFolderIntoClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem projectPropertiesToolStripMenuItem;
    }
}
