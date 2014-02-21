namespace Project.View
{
    partial class Options
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.OK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.ProcessTimeout = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dontBuildBeforeRun = new System.Windows.Forms.CheckBox();
            this.hideUnusedResourcesInProjectExplorer = new System.Windows.Forms.CheckBox();
            this.reopenResources = new System.Windows.Forms.CheckBox();
            this.saveAllBeforeBuilding = new System.Windows.Forms.CheckBox();
            this.fileMonitoring = new System.Windows.Forms.CheckBox();
            this.verboseOutput = new System.Windows.Forms.CheckBox();
            this.maximizeWindowAtStartup = new System.Windows.Forms.CheckBox();
            this.loadLastLoadedProject = new System.Windows.Forms.CheckBox();
            this.tabPlugins = new System.Windows.Forms.TabPage();
            this.lvPlugins = new System.Windows.Forms.ListView();
            this.description = new System.Windows.Forms.TextBox();
            this.version = new System.Windows.Forms.TextBox();
            this.defaultPluginForUnknownExtensions = new System.Windows.Forms.CheckBox();
            this.type = new System.Windows.Forms.TextBox();
            this.extensions = new System.Windows.Forms.ListBox();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabPlugins.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(419, 321);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabPlugins);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(489, 303);
            this.tabControl1.TabIndex = 2;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.ProcessTimeout);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.Add(this.dontBuildBeforeRun);
            this.tabGeneral.Controls.Add(this.hideUnusedResourcesInProjectExplorer);
            this.tabGeneral.Controls.Add(this.reopenResources);
            this.tabGeneral.Controls.Add(this.saveAllBeforeBuilding);
            this.tabGeneral.Controls.Add(this.fileMonitoring);
            this.tabGeneral.Controls.Add(this.verboseOutput);
            this.tabGeneral.Controls.Add(this.maximizeWindowAtStartup);
            this.tabGeneral.Controls.Add(this.loadLastLoadedProject);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(481, 277);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "Global Settings";
            // 
            // ProcessTimeout
            // 
            this.ProcessTimeout.Location = new System.Drawing.Point(132, 196);
            this.ProcessTimeout.Name = "ProcessTimeout";
            this.ProcessTimeout.Size = new System.Drawing.Size(100, 20);
            this.ProcessTimeout.TabIndex = 11;
            this.ProcessTimeout.TextChanged += new System.EventHandler(this.ProcessTimeout_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Process timeout (in sec.)";
            // 
            // dontBuildBeforeRun
            // 
            this.dontBuildBeforeRun.AutoSize = true;
            this.dontBuildBeforeRun.Location = new System.Drawing.Point(9, 173);
            this.dontBuildBeforeRun.Name = "dontBuildBeforeRun";
            this.dontBuildBeforeRun.Size = new System.Drawing.Size(133, 17);
            this.dontBuildBeforeRun.TabIndex = 9;
            this.dontBuildBeforeRun.Text = "Don\'t Build before Run";
            this.dontBuildBeforeRun.UseVisualStyleBackColor = true;
            // 
            // hideUnusedResourcesInProjectExplorer
            // 
            this.hideUnusedResourcesInProjectExplorer.AutoSize = true;
            this.hideUnusedResourcesInProjectExplorer.Location = new System.Drawing.Point(9, 150);
            this.hideUnusedResourcesInProjectExplorer.Name = "hideUnusedResourcesInProjectExplorer";
            this.hideUnusedResourcesInProjectExplorer.Size = new System.Drawing.Size(223, 17);
            this.hideUnusedResourcesInProjectExplorer.TabIndex = 8;
            this.hideUnusedResourcesInProjectExplorer.Text = "Hide unused resources in Project Explorer";
            this.hideUnusedResourcesInProjectExplorer.UseVisualStyleBackColor = true;
            // 
            // reopenResources
            // 
            this.reopenResources.AutoSize = true;
            this.reopenResources.Location = new System.Drawing.Point(30, 35);
            this.reopenResources.Name = "reopenResources";
            this.reopenResources.Size = new System.Drawing.Size(166, 17);
            this.reopenResources.TabIndex = 7;
            this.reopenResources.Text = "Reopen resource(s) at startup";
            this.reopenResources.UseVisualStyleBackColor = true;
            // 
            // saveAllBeforeBuilding
            // 
            this.saveAllBeforeBuilding.AutoSize = true;
            this.saveAllBeforeBuilding.Location = new System.Drawing.Point(9, 104);
            this.saveAllBeforeBuilding.Name = "saveAllBeforeBuilding";
            this.saveAllBeforeBuilding.Size = new System.Drawing.Size(224, 17);
            this.saveAllBeforeBuilding.TabIndex = 5;
            this.saveAllBeforeBuilding.Text = "Save all opened resources before building";
            this.saveAllBeforeBuilding.UseVisualStyleBackColor = true;
            // 
            // fileMonitoring
            // 
            this.fileMonitoring.AutoSize = true;
            this.fileMonitoring.Enabled = false;
            this.fileMonitoring.Location = new System.Drawing.Point(9, 81);
            this.fileMonitoring.Name = "fileMonitoring";
            this.fileMonitoring.Size = new System.Drawing.Size(126, 17);
            this.fileMonitoring.TabIndex = 4;
            this.fileMonitoring.Text = "Enable file monitoring";
            this.fileMonitoring.UseVisualStyleBackColor = true;
            // 
            // verboseOutput
            // 
            this.verboseOutput.AutoSize = true;
            this.verboseOutput.Location = new System.Drawing.Point(9, 127);
            this.verboseOutput.Name = "verboseOutput";
            this.verboseOutput.Size = new System.Drawing.Size(119, 17);
            this.verboseOutput.TabIndex = 2;
            this.verboseOutput.Text = "Use verbose output";
            this.verboseOutput.UseVisualStyleBackColor = true;
            // 
            // maximizeWindowAtStartup
            // 
            this.maximizeWindowAtStartup.AutoSize = true;
            this.maximizeWindowAtStartup.Location = new System.Drawing.Point(9, 58);
            this.maximizeWindowAtStartup.Name = "maximizeWindowAtStartup";
            this.maximizeWindowAtStartup.Size = new System.Drawing.Size(209, 17);
            this.maximizeWindowAtStartup.TabIndex = 1;
            this.maximizeWindowAtStartup.Text = "Maximize application window at startup";
            this.maximizeWindowAtStartup.UseVisualStyleBackColor = true;
            // 
            // loadLastLoadedProject
            // 
            this.loadLastLoadedProject.AutoSize = true;
            this.loadLastLoadedProject.Location = new System.Drawing.Point(9, 12);
            this.loadLastLoadedProject.Name = "loadLastLoadedProject";
            this.loadLastLoadedProject.Size = new System.Drawing.Size(139, 17);
            this.loadLastLoadedProject.TabIndex = 0;
            this.loadLastLoadedProject.Text = "Load last loaded project";
            this.loadLastLoadedProject.UseVisualStyleBackColor = true;
            this.loadLastLoadedProject.CheckedChanged += new System.EventHandler(this.loadLastLoadedProject_CheckedChanged);
            // 
            // tabPlugins
            // 
            this.tabPlugins.Controls.Add(this.lvPlugins);
            this.tabPlugins.Controls.Add(this.description);
            this.tabPlugins.Controls.Add(this.version);
            this.tabPlugins.Controls.Add(this.defaultPluginForUnknownExtensions);
            this.tabPlugins.Controls.Add(this.type);
            this.tabPlugins.Controls.Add(this.extensions);
            this.tabPlugins.Location = new System.Drawing.Point(4, 22);
            this.tabPlugins.Name = "tabPlugins";
            this.tabPlugins.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlugins.Size = new System.Drawing.Size(481, 277);
            this.tabPlugins.TabIndex = 0;
            this.tabPlugins.Text = "Modules";
            // 
            // lvPlugins
            // 
            this.lvPlugins.FullRowSelect = true;
            this.lvPlugins.Location = new System.Drawing.Point(6, 11);
            this.lvPlugins.Name = "lvPlugins";
            this.lvPlugins.Size = new System.Drawing.Size(223, 260);
            this.lvPlugins.TabIndex = 32;
            this.lvPlugins.UseCompatibleStateImageBehavior = false;
            this.lvPlugins.View = System.Windows.Forms.View.Details;
            this.lvPlugins.SelectedIndexChanged += new System.EventHandler(this.plugins_SelectedIndexChanged);
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(235, 11);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Size = new System.Drawing.Size(240, 40);
            this.description.TabIndex = 30;
            // 
            // version
            // 
            this.version.Location = new System.Drawing.Point(235, 83);
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Size = new System.Drawing.Size(240, 20);
            this.version.TabIndex = 29;
            // 
            // defaultPluginForUnknownExtensions
            // 
            this.defaultPluginForUnknownExtensions.AutoSize = true;
            this.defaultPluginForUnknownExtensions.Enabled = false;
            this.defaultPluginForUnknownExtensions.Location = new System.Drawing.Point(235, 254);
            this.defaultPluginForUnknownExtensions.Name = "defaultPluginForUnknownExtensions";
            this.defaultPluginForUnknownExtensions.Size = new System.Drawing.Size(212, 17);
            this.defaultPluginForUnknownExtensions.TabIndex = 27;
            this.defaultPluginForUnknownExtensions.Text = "Default module for unknown extensions";
            this.defaultPluginForUnknownExtensions.UseVisualStyleBackColor = true;
            // 
            // type
            // 
            this.type.Location = new System.Drawing.Point(235, 57);
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.Size = new System.Drawing.Size(240, 20);
            this.type.TabIndex = 26;
            // 
            // extensions
            // 
            this.extensions.FormattingEnabled = true;
            this.extensions.Location = new System.Drawing.Point(235, 109);
            this.extensions.Name = "extensions";
            this.extensions.Size = new System.Drawing.Size(240, 134);
            this.extensions.TabIndex = 21;
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Magenta;
            this.imageListTree.Images.SetKeyName(0, "FileProject");
            this.imageListTree.Images.SetKeyName(1, "FolderClosed");
            this.imageListTree.Images.SetKeyName(2, "FolderOpen");
            this.imageListTree.Images.SetKeyName(3, "FileH");
            this.imageListTree.Images.SetKeyName(4, "FileASM");
            this.imageListTree.Images.SetKeyName(5, "FileC");
            this.imageListTree.Images.SetKeyName(6, "FileCPlusPlus");
            this.imageListTree.Images.SetKeyName(7, "FileGeneric");
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 356);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabPlugins.ResumeLayout(false);
            this.tabPlugins.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPlugins;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.CheckBox loadLastLoadedProject;
        private System.Windows.Forms.CheckBox maximizeWindowAtStartup;
        private System.Windows.Forms.ListBox extensions;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.TextBox type;
        private System.Windows.Forms.CheckBox defaultPluginForUnknownExtensions;
        private System.Windows.Forms.TextBox version;
        private System.Windows.Forms.CheckBox verboseOutput;
        private System.Windows.Forms.CheckBox fileMonitoring;
        private System.Windows.Forms.CheckBox saveAllBeforeBuilding;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.CheckBox reopenResources;
        private System.Windows.Forms.ListView lvPlugins;
        private System.Windows.Forms.CheckBox hideUnusedResourcesInProjectExplorer;
        private System.Windows.Forms.CheckBox dontBuildBeforeRun;
        private System.Windows.Forms.TextBox ProcessTimeout;
        private System.Windows.Forms.Label label1;
    }
}