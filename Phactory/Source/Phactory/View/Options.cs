using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project.View
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            this.loadLastLoadedProject.Checked = App.Controller.UserConfig.LoadLastLoadedProject;
            this.reopenResources.Checked = App.Controller.UserConfig.ReopenResources;
            this.reopenResources.Enabled = this.loadLastLoadedProject.Checked;
            
            this.maximizeWindowAtStartup.Checked = App.Controller.UserConfig.MaximizeWindowAtStartup;
            this.verboseOutput.Checked = App.Controller.UserConfig.VerboseOutput;
            this.fileMonitoring.Checked = App.Controller.UserConfig.FileMonitoring;
            this.saveAllBeforeBuilding.Checked = App.Controller.UserConfig.SaveAllBeforeBuilding;
            this.hideUnusedResourcesInProjectExplorer.Checked = App.Controller.UserConfig.HideUnusedResourcesInProjectExplorer;
            this.dontBuildBeforeRun.Checked = App.Controller.UserConfig.DontBuildBeforeRun;
            this.ProcessTimeout.Text = App.Controller.UserConfig.ProcessTimeoutInSec.ToString();

            lvPlugins.Clear();
            lvPlugins.Columns.Add("Name", 140);
            
            foreach (PhactoryHost.Plugin plugin in App.Controller.PluginManager.GetPlugins())
            {
                ListViewItem item = new ListViewItem(plugin.GetName()); // Name

                lvPlugins.Items.Add(item);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (LastPlugin != null)
            {
                LastPlugin.SaveSettings();
                
                LastPlugin = null;
            }

            Close();

            App.Controller.UserConfig.LoadLastLoadedProject = this.loadLastLoadedProject.Checked;
            App.Controller.UserConfig.ReopenResources = this.reopenResources.Checked;
            App.Controller.UserConfig.MaximizeWindowAtStartup = this.maximizeWindowAtStartup.Checked;
            App.Controller.UserConfig.VerboseOutput = this.verboseOutput.Checked;
            App.Controller.UserConfig.SaveAllBeforeBuilding = this.saveAllBeforeBuilding.Checked;
            App.Controller.UserConfig.HideUnusedResourcesInProjectExplorer = this.hideUnusedResourcesInProjectExplorer.Checked;
            App.Controller.UserConfig.DontBuildBeforeRun = this.dontBuildBeforeRun.Checked;
            App.Controller.UserConfig.ProcessTimeoutInSec = int.Parse(this.ProcessTimeout.Text);

            if (this.fileMonitoring.Checked && (!App.Controller.UserConfig.FileMonitoring))
            {
                App.Controller.UserConfig.FileMonitoring = true;
                App.Controller.FileMonitor.Start();
            }
            else
            if ((!this.fileMonitoring.Checked) && App.Controller.UserConfig.FileMonitoring)
            {
                App.Controller.UserConfig.FileMonitoring = false;
                App.Controller.FileMonitor.Stop();
            }

            App.Controller.WriteUserConfig();

            App.Controller.View.RefreshDB();
        }

        private PhactoryHost.Plugin LastPlugin = null;
        private void plugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPlugins.SelectedItems.Count == 0)
            {
                return;
            }

            if (LastPlugin != null)
            {
                LastPlugin.SaveSettings();
            }

            PhactoryHost.Plugin plugin = null;
            if (lvPlugins.SelectedItems.Count>0)
            {
                plugin = App.Controller.PluginManager.GetPluginByName(lvPlugins.SelectedItems[0].Text);
            }
            LastPlugin = plugin;

            description.Text = plugin.GetDescription();

            PhactoryHost.ToolPlugin toolPlugin = plugin as PhactoryHost.ToolPlugin;
            PhactoryHost.EditorPlugin editorPlugin = plugin as PhactoryHost.EditorPlugin;
            PhactoryHost.CompilerPlugin compilerPlugin = plugin as PhactoryHost.CompilerPlugin;
            PhactoryHost.RunnerPlugin runnerPlugin = plugin as PhactoryHost.RunnerPlugin;

            if (toolPlugin != null) type.Text = "Tool Module";
            if (editorPlugin != null) type.Text = "Editor Module";
            if (compilerPlugin != null) type.Text = "Build Module";
            if (runnerPlugin != null) type.Text = "Runner Module";

            version.Text = "" + plugin.GetVersion();

            extensions.Items.Clear();

            defaultPluginForUnknownExtensions.Checked = false;

            List<PhactoryHost.PluginExtension> supportedExtensions = null;

            if (editorPlugin != null)
            {
                supportedExtensions = editorPlugin.GetSupportedExtensions();

                defaultPluginForUnknownExtensions.Checked = editorPlugin.IsDefaultPluginForUnknownTypes();
            }

            if (compilerPlugin != null)
            {
                supportedExtensions = compilerPlugin.GetSupportedExtensions();
            }

            if (runnerPlugin != null)
            {
                supportedExtensions = runnerPlugin.GetSupportedExtensions();
            }

            if (supportedExtensions != null)
            {
                foreach (PhactoryHost.PluginExtension supportedExtension in supportedExtensions)
                {
                    extensions.Items.Add(supportedExtension.GetName());
                }
            }
        }

        private void loadLastLoadedProject_CheckedChanged(object sender, EventArgs e)
        {
            this.reopenResources.Enabled = this.loadLastLoadedProject.Checked;
        }

        private void ProcessTimeout_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            if (!int.TryParse(ProcessTimeout.Text, out value))
            {
                this.ProcessTimeout.Text = App.Controller.UserConfig.ProcessTimeoutInSec.ToString();
            }
        }
    }
}
