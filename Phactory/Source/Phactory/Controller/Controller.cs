using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data;

namespace Project.Controller
{
    public class Controller
    {
        public View.View View { get; set; }

        public PhactoryHost.Database.Project Entities { get; set; }
        
        private Config config = new Config();
        public Config Config
        {
            get { return config; }
        }

        private UserConfig userConfig = new UserConfig();
        public UserConfig UserConfig
        {
            get { return userConfig; }
            set { userConfig = value; }
        }

        private Log log;
        public Log Log
        {
            get { return log; }
        }

        private Build build = new Build();
        public Build Build
        {
            get { return build; }
        }

        private FileMonitor fileMonitor = new FileMonitor();
        public FileMonitor FileMonitor
        {
            get { return fileMonitor; }
        }

        private Plugin.PluginManager pluginManager;
        public Plugin.PluginManager PluginManager
        {
            get { return pluginManager; }
        }

        public bool IsAppClosing = false;

        public Controller(View.View view)
        {
            this.View = view;
        }

        public void StartApplication(string filename)
        {
            View.StartApplication(filename);
        }

        public void Init()
        {
            log = new Log(App.Controller.Config.ApplicationPath + "Log.txt");
            log.Init();

            config = Helper.ClassSerializer.Read<Config>(config.ConfigPath);

            UserConfig userConfig = Helper.ClassSerializer.Read<UserConfig>(config.UserConfigPath);
            if (userConfig != null)
            {
                App.Controller.UserConfig = userConfig;
            }

            pluginManager = new Plugin.PluginManager();
            pluginManager.Init();

            App.Controller.View.UpdateMostRecentUsed();
        }

        public void ShowProjectProperties()
        {
            App.Controller.View.ShowProjectProperties();
        }

        public void WriteUserConfig()
        {
            Helper.ClassSerializer.Write(config.UserConfigPath, userConfig);
        }

        private void InsertProjectMostRecentUsed( string filename )
        {
            if (userConfig.RecentProjects.Count >= 1)
            {
                if (userConfig.RecentProjects[0] == filename)
                {
                    return;
                }
            }

            for ( int iRecent = 0; iRecent < userConfig.RecentProjects.Count; iRecent++ )
            {
                if ( userConfig.RecentProjects[ iRecent ] == filename )
                {
                    userConfig.RecentProjects.RemoveAt(iRecent);
                    break;
                }
            }
            
            userConfig.RecentProjects.Insert(0, filename);            
            if ( userConfig.RecentProjects.Count > config.MRUCount )
            {
                userConfig.RecentProjects.RemoveAt(userConfig.RecentProjects.Count - 1);
            }

            WriteUserConfig();            
        }

        public void InsertFileMostRecentUsed(string filename)
        {
            for (int iRecent = 0; iRecent < userConfig.RecentFiles.Count; iRecent++)
            {
                if (userConfig.RecentFiles[iRecent] == filename)
                {
                    userConfig.RecentFiles.RemoveAt(iRecent);
                    break;
                }
            }

            userConfig.RecentFiles.Insert(0, filename);
            if (userConfig.RecentFiles.Count > config.MRUCount)
            {
                userConfig.RecentFiles.RemoveAt(userConfig.RecentFiles.Count - 1);
            }

            WriteUserConfig();
            View.UpdateMostRecentUsed();
        }

        public void CreateNewProject(string filename, string name)
        {
            Helper.DBHelper.CreateNewProject(filename, name);

            OpenProject(filename);
        }

        public void SaveProject()
        {
            Helper.ClassSerializer.Write(App.Controller.UserConfig.ProjectFilename, App.Controller.Entities);
        }

        public void CreateNewResource(PhactoryHost.Database.Resource resource)
        {
            string extension = Helper.StringHelper.GetFileInfo(resource).Extension;

            PhactoryHost.EditorPlugin editor = pluginManager.GetEditor(extension);
            if (editor != null)
            {
                editor.CreateEmptyResource(resource);
            }

            PhactoryHost.CompilerPlugin compiler = pluginManager.GetCompiler(extension);
            if (compiler != null)
            {
                resource.IsIncludeResource = compiler.IsIncludeResource(resource);

                compiler.RefreshOutput(resource);
            }

            InsertFileMostRecentUsed(Helper.StringHelper.GetFileInfo(resource).FullName);
        }

        public bool OpenProject(string filename)
        {
            if ( !File.Exists(filename) )
            {
                Log.Append(filename + " does not exist !");                
                return false;
            }

            View.CloseAllResources();

            App.Controller.View.AppDoEvents();

            Helper.DBHelper.OpenProject(filename);

            App.Controller.View.AppDoEvents();

            InsertProjectMostRecentUsed(filename);
            View.UpdateMostRecentUsed();

            App.Controller.View.AppDoEvents();

            View.RefreshApplicationTitle();
            
            App.Controller.View.AppDoEvents();

            View.RefreshDB();

            App.Controller.View.AppDoEvents();

            View.RefreshButtons();

            if (App.Controller.UserConfig.FileMonitoring)
            {
                FileMonitor.Start();
            }

            return ( Entities != null );
        }

        public void OpenTextResourceAtLine(PhactoryHost.Database.Resource resource, int line)
        {
            OpenResource(resource);

            View.PluginView pluginView = View.FindPluginView(resource);
            if (pluginView != null)
            {
                FileInfo fileInfo = Helper.StringHelper.GetFileInfo(resource);
                PhactoryHost.EditorPlugin editor = App.Controller.PluginManager.GetEditor(fileInfo.Extension);

                if ((editor != null) && (pluginView != null))
                {
                    editor.SetLine(resource, line);
                }
            }
        }
        
        public void OpenResource(PhactoryHost.Database.Resource resource)
        {
            View.PluginView pluginView = View.FindPluginView(resource);

            if (pluginView == null)
            {
                FileInfo fileInfo = Helper.StringHelper.GetFileInfo(resource);
                if (!fileInfo.Exists)
                {
                    Log.Append(resource.RelativePath + " does not exist !");
                    //View.ShowErrorMessage("Can't open file", resource.RelativePath + " does not exist !");

                    return;
                }

                pluginManager.OpenResource(resource);
            }
            else
            {
                View.SetFocusedView(pluginView);
            }

            View.RefreshButtons();
        }

        public bool CloseResource(PhactoryHost.Database.Resource resource)
        {
            return pluginManager.CloseResource(resource);
        }

        public string GetBuildDate()
        {
            string strpath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            System.IO.FileInfo fi = new System.IO.FileInfo(strpath);
            return fi.LastWriteTime.ToString();
        }

        public string GetApplicationName()
        {
            return "Phactory";
        }

        public void RenameProject(string newName)
        {
            App.Controller.Entities.ProjectName = newName;
            SaveProject();

            App.Controller.View.RefreshApplicationTitle();
        }
        
        public void Explore(string path)
        {
            Process.Start("explorer.exe", "/select," + path);
        }

        public void SaveResource()
        {
            PhactoryHost.Database.Resource resource = View.GetActivePluginResource();
            if (resource != null)
            {
                SaveResource(resource);
            }
        }

        public void SaveResource(PhactoryHost.Database.Resource resource)
        {
            pluginManager.SaveResource(resource);
        }

        public int SaveAllResources()
        {
            List<PhactoryHost.Database.Resource> allEditedResources = App.Controller.View.GetAllEditedResources();

            int fileSaved = 0;

            foreach (PhactoryHost.Database.Resource resource in allEditedResources)
            {
                if (pluginManager.IsResourceModified(resource))
                {
                    pluginManager.SaveResource(resource);

                    fileSaved++;
                }
            }

            return fileSaved;
        }
    }
}
