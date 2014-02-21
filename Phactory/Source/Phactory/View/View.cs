using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;

namespace Project.View
{
    public partial class View : Form
    {
        public enum BuildStatus
        {
            Neutral,
            Success,
            Failure,
            Running
        };

        private Color defaultForeColor;
        private BuildStatus _buildStatus = BuildStatus.Neutral;

        private bool CheckBuildStatus()
        {
            if (_buildStatus == BuildStatus.Running)
            {
                App.Controller.Log.Append("Close running process first !");

                return false;
            }

            return true;
        }

        private void SetBuildStatus(BuildStatus status )
        {
            _buildStatus = status;

            if (status == BuildStatus.Running)
            {
                this.statusStrip1.BackColor = System.Drawing.Color.Chocolate;
                this.statusStrip1.ForeColor = System.Drawing.Color.White;
            }
            else if (status == BuildStatus.Success)
            {
                this.statusStrip1.BackColor = System.Drawing.Color.ForestGreen;
                this.statusStrip1.ForeColor = System.Drawing.Color.White;
            }
            else if ( status==BuildStatus.Failure )
            {
                this.statusStrip1.BackColor = System.Drawing.Color.Red;
                this.statusStrip1.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                this.statusStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.statusStrip1.ForeColor = defaultForeColor;
            }
        }

        private ProjectExplorer projectExplorer;
        public ProjectExplorer ProjectExplorer
        {
            get { return projectExplorer; }
        }

        private Output output;
        public Output Output
        {
            get { return output; }
        }

        private Dictionary<PhactoryHost.ToolPlugin, PluginView> pluginToolBinding = new Dictionary<PhactoryHost.ToolPlugin, PluginView>();
        private string StartProjectFilename = null;

        public View()
        {
            InitializeComponent();

            defaultForeColor = statusStrip1.ForeColor;

            foreach (ToolStripItem tsi in toolStrip1.Items)
            {
                if (tsi is ToolStripDropDownButton)
                {
                    ToolStripDropDownButton tsd = (ToolStripDropDownButton)tsi;
                    ((ToolStripDropDownMenu)tsd.DropDown).ShowImageMargin = false;
                    ((ToolStripDropDownMenu)tsd.DropDown).ShowCheckMargin = false;
                }
            }

            EnableCancelBuild(false, false);

            toolStripStatusLabel1.Text = "Ready";
        }

        protected bool IsFullScreen = false;
        protected Helper.FormState formState = new Helper.FormState();
        private void ToogleFullscreen()
        {
            if (IsFullScreen)
            {
                formState.Restore(this);
                IsFullScreen = false;
            }
            else
            {
                formState.Maximize(this);
                IsFullScreen = true;
            }
        }

        protected override bool ProcessCmdKey ( ref Message msg, Keys keyData)
        {
            bool handled = false;
            
            switch (keyData)
            {
                case Keys.F5:
                    handled = true;
                    //ShowOutput(true); 
                    RunProject();
                    break;

                case (Keys.Shift | Keys.Alt | Keys.Enter):
                    handled = true;
                    ToogleFullscreen();
                    break;

                case (Keys.Alt | Keys.Control | Keys.F7):
                    handled = true;
                    //ShowOutput(true);
                    RebuildProject();
                    break;

                case (Keys.Control | Keys.F7):
                    handled = true;
                    //ShowOutput(true);
                    CleanProject();
                    break;

                case Keys.F7:
                    handled = true;
                    //ShowOutput(true);
                    BuildProject(true);
                    break;

                case (Keys.Control | Keys.F4):
                    handled = true;
                    CloseResource();
                    break;

                case (Keys.Control | Keys.S):
                    handled = true;
                    OnSave();
                    break;

                case (Keys.Control | Keys.Shift | Keys.S):
                    handled = true;
                    OnSaveAll();
                    break;
            }

            return handled;
        }

        public void Init(string filename)
        {
            StartProjectFilename = filename;
            
            Text = App.Controller.GetApplicationName();

            projectExplorer = new ProjectExplorer();
            output = new Output();

            dockPanel.ShowDocumentIcon = false; 

            ShowProjectExplorer(true);
            ShowOutput(true);
        }

        public void StartApplication(string filename)
        {
            try
            {
                Init(filename);
                App.Controller.Init();

                Application.Run(this);
            }
            catch (Exception e)
            {
                App.Controller.Log.Append(e.ToString());
            }
        }
        
        private void View_Shown(object sender, EventArgs e)
        {
            if (App.Controller.UserConfig.VerboseOutput)
            {
                App.Controller.Log.Append("Verbose output mode activated");

                if (App.Controller.UserConfig.FileMonitoring)
                {
                    App.Controller.Log.Append("File monitoring activated");
                }
                else
                {
                    App.Controller.Log.Append("File monitoring disabled");
                }
            }

            string filename = null;
            bool doOpenProject = false;
            if (StartProjectFilename != "")
            {
                filename = StartProjectFilename;
                doOpenProject = true;
            }
            else
            {
                if ((App.Controller.UserConfig.RecentProjects.Count > 0) && (App.Controller.UserConfig.LoadLastLoadedProject))
                {
                    if (App.Controller.UserConfig.VerboseOutput)
                    {
                        App.Controller.Log.Append("Loading last loaded project");
                    }

                    filename = App.Controller.UserConfig.RecentProjects[0];
                    doOpenProject = true;
                }
            }

            App.Controller.View.AppDoEvents();

            if (App.Controller.UserConfig.MaximizeWindowAtStartup)
            {
                this.WindowState = FormWindowState.Maximized;
                App.Controller.Log.Append("Application window maximized");
            }

            if (doOpenProject)
            {
                if (App.Controller.OpenProject(filename))
                {
                    if (App.Controller.UserConfig.ReopenResources)
                    {
                        if (App.Controller.UserConfig.VerboseOutput)
                        {
                            if (App.Controller.UserConfig.OpenedResources.Count > 0)
                            {
                                App.Controller.Log.Append("Reopening resource(s)...");
                            }
                        }

                        List<int> invalidIDs = new List<int>();
                        foreach (int resourceId in App.Controller.UserConfig.OpenedResources)
                        {
                            App.Controller.View.AppDoEvents();

                            PhactoryHost.Database.Resource resource = App.Controller.Entities.GetResource(resourceId);
                            if (resource == null)
                            {
                                invalidIDs.Add(resourceId);
                            }
                            else
                            {
                                App.Controller.OpenResource(resource);
                                App.Controller.Log.Append(resource.DisplayName + " opened");
                            }
                        }
                        foreach (int invalidResourceId in invalidIDs)
                        {
                            App.Controller.UserConfig.OpenedResources.Remove(invalidResourceId);
                        }
                        if (invalidIDs.Count > 0)
                        {
                            if (App.Controller.UserConfig.VerboseOutput)
                            {
                                App.Controller.Log.Append(
                                    "Found invalid resource IDs in previously opened list... removing them.");
                            }
                            App.Controller.WriteUserConfig();
                        }
                    }
                }
            }

            if (App.Controller.UserConfig.VerboseOutput)
            {
                App.Controller.Log.Append("Initialization done");
            }

            App.Controller.View.AppDoEvents();
        }

        private void OnClickRecentFile(object sender, EventArgs e)
        {
        }

        private void OnClickRecentProject(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            if ( toolStripMenuItem != null )
            {
                App.Controller.OpenProject(toolStripMenuItem.Text);
            }
        }

        public void ShowInformationMessage(string title, string content)
        {
            MessageBox.Show(content, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowWarningMessage(string title, string content)
        {
            MessageBox.Show(content, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ShowErrorMessage(string title, string content)
        {
            MessageBox.Show(content, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void UpdateMostRecentUsed()
        {
            /*recentFilesToolStripMenuItem.DropDownItems.Clear();
            recentFilesToolStripMenuItem.Enabled = (App.Controller.UserConfig.RecentFiles.Count != 0);
            foreach (string recentFile in App.Controller.UserConfig.RecentFiles)
            {
                recentFilesToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(recentFile, null, new System.EventHandler(this.OnClickRecentFile)));
            }           

            recentProjectsToolStripMenuItem.DropDownItems.Clear();
            recentProjectsToolStripMenuItem.Enabled = (App.Controller.UserConfig.RecentProjects.Count != 0);
            foreach (string recentProject in App.Controller.UserConfig.RecentProjects)
            {
                recentProjectsToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(recentProject, null, new System.EventHandler(this.OnClickRecentProject)));
            }*/
        }

        private void ShowProjectExplorer(bool show)
        {
            if(show) 
            {
                projectExplorer.Show(dockPanel, DockState.DockLeft);
            }
            else
            {
                projectExplorer.Hide();
            }
        }

        private void ShowOutput(bool show)
        {
            if(show) 
            {
                output.Show(dockPanel, DockState.DockBottom);
            }
            else
            {
                output.Hide();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);

        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new Phactory.View.About();
            dialog.ShowDialog();
        }

        delegate void CancelBuildDelegate();
        public void CancelBuild()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new CancelBuildDelegate(CancelBuild));
                return;
            }

            App.Controller.Build.CancelCompile = true;
        }

        delegate void ClearLogDelegate();
        public void ClearLog()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ClearLogDelegate(ClearLog));
                return;
            }
            
            Output.LogListBox.Items.Clear();
        }

        delegate void LogLineDelegate(string line);
        public void LogLine(string line)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new LogLineDelegate(LogLine), new object[] { line });
                return;
            }

            Output.LogListBox.BeginUpdate();
            
            Output.LogListBox.Items.Add(line);
            Output.LogListBox.SelectedIndex = Output.LogListBox.Items.Count - 1;
            Output.LogListBox.SelectedIndex = -1;

            Output.LogListBox.EndUpdate();
        }

        delegate void EnableCancelBuildDelegate(bool enable, bool forceCancelButtonDisabled);
        public void EnableCancelBuild(bool enable, bool forceCancelButtonDisabled)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new EnableCancelBuildDelegate(EnableCancelBuild), new object[] { enable, forceCancelButtonDisabled });
                return;
            }

            if (forceCancelButtonDisabled)
            {
                this.cancelToolStripButton.Enabled = false;
                this.cancelToolStripMenuItem.Enabled = false;
            }
            else
            {
                this.cancelToolStripButton.Enabled = enable;
                this.cancelToolStripMenuItem.Enabled = enable;
            }

            this.buildToolStripMenuItem.Enabled = !enable;
            this.rebuildToolStripMenuItem.Enabled = !enable;
            this.runToolStripMenuItem.Enabled = !enable;
            toolStripCleanButton.Enabled = !enable;
            toolStripBuildButton.Enabled = !enable;
            toolStripRunButton.Enabled = !enable;
            cleanToolStripMenuItem1.Enabled = !enable;

            if (!enable)
            {
                this.toolStripProgressBar1.Visible = enable;
            }
        }

        delegate void SetProgressBarDelegate(int percent);
        public void SetProgressBar(int percent)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new SetProgressBarDelegate(SetProgressBar), new object[] { percent });
                return;
            }

            this.toolStripProgressBar1.Visible = true;
            this.toolStripProgressBar1.Value = percent;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            options.ShowDialog(this);
        }

        public PluginView CreatePluginView(PhactoryHost.Database.Resource resource, bool isDependency)
        {
            PluginView pluginView = new PluginView(isDependency);

            string filename = resource.RelativePath;
            pluginView.Text = new FileInfo(filename).Name;
            pluginView.Resource = resource;
            pluginView.Show(dockPanel);

            return pluginView;
        }

        public PluginView CreateToolPluginView(PhactoryHost.ToolPlugin pluginTool)
        {
            PluginView pluginView = new PluginView(false);

            pluginView.Text = pluginTool.GetName();
            pluginView.Show(dockPanel);

            return pluginView;
        }

        public List<PhactoryHost.Database.Resource> GetAllEditedResources()
        {
            List<PhactoryHost.Database.Resource> allEditedResources = new List<PhactoryHost.Database.Resource>();
            
            foreach (IDockContent content in dockPanel.Documents)
            {
                PluginView pluginView = content as PluginView;
                if (pluginView != null)
                {
                    if (pluginView.IsDependency == false)
                    {
                        allEditedResources.Add(pluginView.Resource);
                    }
                }
            }

            return allEditedResources;
        }

        public PluginView FindPluginView(PhactoryHost.Database.Resource resource)
        {
            if (resource == null)
            {
                return null;
            }

            foreach (IDockContent content in dockPanel.Documents)
            {
                PluginView pluginView = content as PluginView;
                if (pluginView != null)
                {
                    if ((pluginView.IsDependency == false) && (pluginView.Resource.Id == resource.Id))
                    {
                        return pluginView;
                    }
                }
            }

            return null;
        }

        public PluginView FindPluginDependencyView(PhactoryHost.Database.Resource resource)
        {
            foreach (IDockContent content in dockPanel.Documents)
            {
                PluginView pluginView = content as PluginView;
                if (pluginView != null)
                {
                    if ((pluginView.IsDependency == true) && (pluginView.Resource.Id == resource.Id))
                    {
                        return pluginView;
                    }
                }
            }

            return null;
        }

        public void SetFocusedView(PluginView view)
        {
            view.DockHandler.Activate();
        }

        public PluginView GetActivePluginView()
        {
            if (dockPanel.ActiveDocument == null)
            {
                return null;
            }

            PluginView pluginView = dockPanel.ActiveDocument as PluginView;
            return pluginView;
        }

        public PhactoryHost.Database.Resource GetActivePluginResource()
        {
            PhactoryHost.Database.Resource resource = null;
            
            PluginView pluginView = GetActivePluginView();
            if (pluginView != null)
            {
                resource = pluginView.Resource;
            }

            return resource;
        }

        private void OnNewProject()
        {
            NewProject newProject = new NewProject();
            newProject.SetProjectLocation(App.Controller.Config.GetDesktopPath());
            newProject.ShowDialog(this);
            if (newProject.Valid)
            {
                Directory.CreateDirectory(newProject.GetProjectLocation());

                App.Controller.UserConfig.NewProjectPath = newProject.GetProjectLocation();
                App.Controller.CreateNewProject(newProject.GetFilename(), newProject.GetName());
                App.Controller.SaveProject();
            }
        }

        private void OnNewFile()
        {
            NewFile newFile = new NewFile();
            newFile.ShowDialog(this);
            if (newFile.Valid)
            {
                App.Controller.UserConfig.NewFilePath = newFile.GetFilename();
            }
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnNewProject();
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OnNewFile();
        }

        private void toolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (App.Controller.UserConfig.RecentProjects.Count == 0)
            {
                openFile.InitialDirectory = Application.ExecutablePath;            
            }
            else
            {
                openFile.InitialDirectory = new FileInfo(App.Controller.UserConfig.RecentProjects[0]).Directory.FullName;
            }

			openFile.Filter = "Phactory project files (*.cpcproj)|*.cpcproj|All files (*.*)|*.*";
			openFile.FilterIndex = 1;
			openFile.RestoreDirectory = true;
            openFile.Title = "Open Project";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string fullName = openFile.FileName;

                App.Controller.OpenProject(fullName);
            }	
        }

        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.Controller.IsAppClosing = true;
            
            if (App.Controller.UserConfig.FileMonitoring)
            {
                App.Controller.FileMonitor.Stop();
            }

            CloseAllResources();
            
            dockPanel.Visible = false;

            App.Controller.WriteUserConfig();
        }

        private void OnSaveAll()
        {
            App.Controller.SaveAllResources();       
        }

        private void OnSave()
        {
            App.Controller.SaveResource();
            App.Controller.SaveProject();            
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnSaveAll();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnSave();
        }

        private void OnClickPluginTool(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            if (toolStripMenuItem != null)
            {
                PhactoryHost.ToolPlugin pluginTool = toolStripMenuItem.Tag as PhactoryHost.ToolPlugin;
                pluginTool.ShowDialog(App.Controller.View);
            }
        }

        public void RefreshPluginTools()
        {
            List<PhactoryHost.ToolPlugin> pluginTools = App.Controller.PluginManager.GetPluginTools();

            toolStripPlugins.DropDownItems.Clear();

            foreach (PhactoryHost.ToolPlugin pluginTool in pluginTools)
            {
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(pluginTool.GetName() + "...", null, new System.EventHandler(this.OnClickPluginTool));
                toolStripMenuItem.Tag = pluginTool;

                toolStripPlugins.DropDownItems.Add(toolStripMenuItem);                
            }
        }

        private void startDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.Controller.Build.Run(App.Controller.Entities.GetResource(App.Controller.Entities.StartupResourceId));
        }

        private void toolStripButtonNewFile_Click(object sender, EventArgs e)
        {
            OnNewFile();
        }

        private void toolStripSave_Click(object sender, EventArgs e)
        {
            OnSave();
        }

        private void toolStripSaveAll_Click(object sender, EventArgs e)
        {
            OnSaveAll();
        }

        public delegate void RefreshApplicationTitleDelegate();
        public void RefreshApplicationTitle()
        {
            if ( this.InvokeRequired )
            {
                BeginInvoke(new RefreshApplicationTitleDelegate(RefreshApplicationTitle));
                return;
            }

            Text = App.Controller.Entities.ProjectName + " - " + App.Controller.GetApplicationName();
        }

        public void RefreshDB()
        {
            ProjectExplorer.RefreshDB();
        }

        delegate void OnResourceDeletedDelegate(PhactoryHost.Database.Resource resource);
        public void OnResourceDeleted(PhactoryHost.Database.Resource resource)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new OnResourceDeletedDelegate(OnResourceDeleted), new object[] { resource });
                return;
            }

            PluginView pluginView = FindPluginView(resource);
            if (pluginView != null)
            {
                App.Controller.PluginManager.OnResourceDeleted(pluginView, resource);
            }
        }

        delegate void OnResourceChangedDelegate(PhactoryHost.Database.Resource resource);
        public void OnResourceChanged(PhactoryHost.Database.Resource resource)
        {
            if (this.InvokeRequired)
            {
                Delegate method = new OnResourceChangedDelegate(OnResourceChanged);
                object[] args = new object[] { resource };
                this.Invoke(method, args);
                return;
            }

            PluginView pluginView = FindPluginView(resource);
            if (pluginView != null)
            {
                App.Controller.PluginManager.OnResourceChanged(pluginView, resource);
            }
        }

        delegate void OnResourceRenamedDelegate(PhactoryHost.Database.Resource resource);
        public void OnResourceRenamed(PhactoryHost.Database.Resource resource)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new OnResourceRenamedDelegate(OnResourceRenamed), new object[] { resource });
                return;
            }
        }

        public void RefreshEditorTitles(PhactoryHost.Database.Resource resource)
        {
            PluginView pluginView = FindPluginView(resource);
            if (pluginView != null)
            {
                App.Controller.PluginManager.RefreshViewTitle(pluginView, resource);
            }

            PluginView pluginDependencyView = FindPluginDependencyView(resource);
            if (pluginDependencyView != null)
            {
                pluginDependencyView.Text = resource.DisplayName + " (Dependency View)";
            }
        }

        public void ProjectExplorerRefreshNodeOutput(PhactoryHost.Database.Resource resource)
        {
            ProjectExplorer.RefreshNodeOutput(resource);
        }

        public void SetToolTipText(Panel panel, string toolTipText)
        {
            PluginView pluginView = panel.Parent as PluginView;
            pluginView.ToolTipText = toolTipText;
        }

        public void SetResource(PhactoryHost.Database.Resource resource)
        {
            toolStripStatusLabel1.Text = "Ready";
            
            if (resource != null )
            {
                string filename = App.Controller.UserConfig.ResourcePath + resource.RelativePath;
                FileInfo fileInfo = new FileInfo(filename);
                if ( fileInfo.Exists )
                {
                    toolStripStatusLabel1.Text = resource.DisplayName + " : " + ((fileInfo.Length / 1024) + 1) + "Kb, " + fileInfo.Length + " bytes, 0x" + String.Format("{0:X4}", fileInfo.Length) + " bytes";
                }
            }
        }

        public class BuildProjectWorker
        {
            private PhactoryHost.Database.Resource resource;
            private bool clearLog;
            public bool IsOk = false;

            public BuildProjectWorker(PhactoryHost.Database.Resource resource, bool clearLog)
            {
                this.resource = resource;
                this.clearLog = clearLog;
            }

            public void DoWork()
            {
                IsOk = App.Controller.Build.Compile(resource, clearLog);
            }
        }

        public void AppDoEvents()
        {
            Application.DoEvents();
        }
        
        private void BuildProject(bool clearLog)
        {
            if (!CheckBuildStatus())
            {
                return;
            }

            SetBuildStatus(BuildStatus.Neutral); 
            
            PhactoryHost.Database.Resource resource = App.Controller.Entities.GetResource(App.Controller.Entities.StartupResourceId);

            if (resource == null)
            {
                this.ShowWarningMessage("Operation failed !", "You need to define a resource as startup item first.");
            }
            else
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                if (App.Controller.UserConfig.SaveAllBeforeBuilding)
                {
                    if (App.Controller.SaveAllResources() != 0)
                    {
                        this.projectExplorer.RefreshDB();
                    }
                }

                var buildProjectWorker = new BuildProjectWorker(resource, clearLog);
                Thread t = new Thread(buildProjectWorker.DoWork);
                t.Start();
                while (t.IsAlive)
                {
                    Application.DoEvents();
                }

                stopwatch.Stop();
                App.Controller.Log.Append(String.Format("Elapsed time: {0}", FormatElapsedTime(stopwatch.Elapsed)));

                if (buildProjectWorker.IsOk)
                {
                    SetBuildStatus(BuildStatus.Success);
                }
                else
                {
                    SetBuildStatus(BuildStatus.Failure);
                }
            }
        }

        private void buidProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildProject(true);
        }

        public class RebuildProjectWorker
        {
            private PhactoryHost.Database.Resource resource;

            public bool IsOk = false;

            public RebuildProjectWorker(PhactoryHost.Database.Resource resource)
            {
                this.resource = resource;
            }

            public void DoWork()
            {
                try
                {
                    IsOk = App.Controller.Build.CleanCompile(resource);
                }
                catch (Exception e)
                {
                    App.Controller.Log.Append(e.ToString());
                }
            }
        }

        private void rebuildProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckBuildStatus())
            {
                return;
            }

            SetBuildStatus(BuildStatus.Neutral);
            
            PhactoryHost.Database.Resource resource = App.Controller.Entities.GetResource(App.Controller.Entities.StartupResourceId);

            if (resource == null)
            {
                this.ShowWarningMessage("Operation failed !", "You need to define a resource as startup item first.");
            }
            else
            {
                var w = new RebuildProjectWorker(resource);
                Thread t = new Thread(w.DoWork);
                t.Start();
                while (t.IsAlive)
                {
                    Application.DoEvents();
                }

                if (w.IsOk)
                {
                    SetBuildStatus(BuildStatus.Success);
                }
                else
                {
                    SetBuildStatus(BuildStatus.Failure);
                }
            }
        }

        private void RebuildProject()
        {
            if (!CheckBuildStatus())
            {
                return;
            }

            CleanProject();
            BuildProject(false);
        }

        public class CleanProjectWorker
        {
            private PhactoryHost.Database.Resource resource;

            public CleanProjectWorker(PhactoryHost.Database.Resource resource)
            {
                this.resource = resource;
            }

            public void DoWork()
            {
                try
                {
                    App.Controller.Build.Clean(resource, true);
                }
                catch (Exception e)
                {
                    App.Controller.Log.Append(e.ToString());
                }
            }
        }

        private void CleanProject()
        {
            if (!CheckBuildStatus())
            {
                return;
            }

            SetBuildStatus(BuildStatus.Neutral);

            PhactoryHost.Database.Resource resource = App.Controller.Entities.GetResource(App.Controller.Entities.StartupResourceId);

            if (resource == null)
            {
                this.ShowWarningMessage("Operation failed !", "You need to define a resource as startup item first.");
            }
            else
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                Thread t = new Thread(new CleanProjectWorker(resource).DoWork);
                t.Start();
                while (t.IsAlive)
                {
                    Application.DoEvents();
                }

                stopwatch.Stop();
                App.Controller.Log.Append(String.Format("Elapsed time: {0}", FormatElapsedTime(stopwatch.Elapsed)));
            }
        }

        private string FormatElapsedTime(TimeSpan elapsedTime)
        {
            int minutes = (int)elapsedTime.TotalMinutes;
            double fsec = 60 * (elapsedTime.TotalMinutes - minutes);
            int sec = (int)fsec;
            //int ms = (int)(1000 * (fsec - sec));
            string tsOut = String.Format("{0:D2}:{1:D2}", minutes, sec);

            return tsOut;
        }

        private void cleanProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CleanProject();
        }

        public class RunProjectWorker
        {
            private PhactoryHost.Database.Resource resource;
            public bool IsOK = false;

            public RunProjectWorker(PhactoryHost.Database.Resource resource)
            {
                this.resource = resource;
            }

            public void DoWork()
            {
                try
                {
                    if (App.Controller.UserConfig.SaveAllBeforeBuilding)
                    {
                        App.Controller.SaveAllResources();
                    }

                    IsOK = true;

                    if (!App.Controller.UserConfig.DontBuildBeforeRun)
                    {
                        IsOK = App.Controller.Build.Compile(resource, true);
                    }
                }
                catch (Exception e)
                {
                    App.Controller.Log.Append(e.ToString());
                }
            }
        }

        private void RunProject()
        {
            if (!CheckBuildStatus())
            {
                return;
            }

            SetBuildStatus(BuildStatus.Neutral);

            PhactoryHost.Database.Resource resource = App.Controller.Entities.GetResource(App.Controller.Entities.StartupResourceId);

            if (resource == null)
            {
                this.ShowWarningMessage("Operation failed !", "You need to define a resource as startup item first.");
            }
            else
            {
                RunProjectWorker w = new RunProjectWorker(resource);
                Thread t = new Thread(w.DoWork);
                t.Start();
                while (t.IsAlive)
                {
                    Application.DoEvents();
                }

                if (w.IsOK)
                {
                    SetBuildStatus(BuildStatus.Running); 
                    
                    App.Controller.Build.Run(resource);

                    SetBuildStatus(BuildStatus.Neutral);
                }
                else
                {
                    App.Controller.Log.Append("Compilation failed, skipping run..");

                    SetBuildStatus(BuildStatus.Failure);
                }
            }
        }
        
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunProject();
        }

        private void CloseResource()
        {
            PhactoryHost.Database.Resource resource = App.Controller.View.GetActivePluginResource();
            if (resource != null)
            {
                PluginView pluginView = FindPluginView(resource);
                if (pluginView != null)
                {
                    pluginView.Close();
                }
            }

            this.RefreshButtons();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseResource();
        }

        public void CloseAllResources()
        {
            List<PhactoryHost.Database.Resource> allEditedResources = App.Controller.View.GetAllEditedResources();

            foreach (PhactoryHost.Database.Resource resource in allEditedResources)
            {
                PluginView pluginView = FindPluginView(resource);
                if (pluginView != null)
                {
                    pluginView.Close();
                }
            }

            this.RefreshButtons();
        }

        private void closeAllResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllResources();
        }

        private void addExistingFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void cleanProjectToolStrip_Click(object sender, EventArgs e)
        {
            CleanProject();
        }

        private void rebuildProjectToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            RebuildProject();
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToogleFullscreen();
        }

        private void findResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindResource();
        }

        private void setAsProjectStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceSelector resourceSelectorDialog = new ResourceSelector(null);
            resourceSelectorDialog.ShowDialog(App.Controller.View);

            if (resourceSelectorDialog.Valid)
            {
                PhactoryHost.Database.Resource resource = resourceSelectorDialog.Resources[0];
                App.Controller.Entities.StartupResourceId = resource.Id;
                App.Controller.SaveProject();

                this.ProjectExplorer.SelectResource(resource);
            }
        }

        private void findResourceToolStripButton_Click(object sender, EventArgs e)
        {
            FindResource();
        }

        private void FindResource()
        {
            ResourceSelector resourceSelectorDialog = new ResourceSelector(null);
            resourceSelectorDialog.ShowDialog(App.Controller.View);

            if (resourceSelectorDialog.Valid)
            {
                this.ProjectExplorer.SelectResource(resourceSelectorDialog.Resources[0]);
            }
        }

        private void closeAllToolStripButton_Click(object sender, EventArgs e)
        {
            CloseAllResources();
        }

        public void RefreshButtons()
        {
            var allEditedResources = App.Controller.View.GetAllEditedResources();

            bool enabled = allEditedResources.Count != 0;

            this.saveAllToolStripMenuItem1.Enabled = enabled;
            this.closeAllToolStripMenuItem.Enabled = enabled;

            projectPropertiesToolStripMenuItem.Enabled = (App.Controller.Entities != null);
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            this.CancelBuild();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OnSaveAll();
        }

        public void ShowProjectProperties()
        {
            var dialog = new Phactory.View.ProjectProperties();
            dialog.ShowDialog(this);
        }

        private void projectPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowProjectProperties();
        }
    }
}
