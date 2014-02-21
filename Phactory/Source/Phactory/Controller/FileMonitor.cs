using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Project.Controller
{
    public class FileMonitor
    {
        private FileSystemWatcher fileSystemWatcher;
        public bool IsStarted = false;
            
        public FileMonitor()
        {
        }

        public void Start()
        {
            Stop();

            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = App.Controller.UserConfig.ResourcePath;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fileSystemWatcher.InternalBufferSize = 16384;
            fileSystemWatcher.Filter = ""; // "" = all files

            fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
            // fileSystemWatcher.Created += new FileSystemEventHandler(OnCreated);
            // fileSystemWatcher.Deleted += new FileSystemEventHandler(OnDeleted);
            fileSystemWatcher.Renamed += new RenamedEventHandler(OnRenamed);

            fileSystemWatcher.EnableRaisingEvents = true;

            IsStarted = true;
            App.Controller.Log.Append("Project file monitoring started at location " + fileSystemWatcher.Path);
        }

        public void Stop()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.EnableRaisingEvents = false;

                IsStarted = false;
                App.Controller.Log.Append("Project file monitoring stopped");
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(e.FullPath);
            PhactoryHost.Database.Resource resource = Helper.DBHelper.GuessResource(fileInfo);
            if (resource != null)
            {
                App.Controller.View.OnResourceDeleted(resource);
            }
        }

        Dictionary<string, DateTime> lastChangedDictionnary = new Dictionary<string, DateTime>();
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            DateTime dateTime = File.GetLastWriteTime(e.FullPath);

            bool doChange = true;
            if (lastChangedDictionnary.ContainsKey(e.FullPath))
            {
                DateTime dateTimeDictionnary = lastChangedDictionnary[e.FullPath];

                if (dateTimeDictionnary.Second == dateTime.Second)
                {
                    doChange = false;
                }
                else
                {
                    lastChangedDictionnary.Remove(e.FullPath);
                }
            }
            
            if ( doChange )
            {
                lastChangedDictionnary.Add(e.FullPath, dateTime);

                FileInfo fileInfo = new FileInfo(e.FullPath);

                PhactoryHost.Database.Resource resource = Helper.DBHelper.GuessResource(fileInfo);
                if ((resource != null)&&(fileInfo.Exists==true))
                {
                    App.Controller.View.OnResourceChanged(resource);
                }
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // NOT YET IMPLEMENTED...
            
            /*
            Document.Resource resource = App.Controller.Document.GetResource(e.OldFullPath);
            if (resource != null)
            {
                FileInfo fileInfo = new FileInfo(e.FullPath);
                resource.Filename = fileInfo.FullName;
                resource.DisplayName = Helper.StringHelper.GetDisplayName(fileInfo.FullName);

                App.Controller.View.OnResourceRenamed(resource.Id, fileInfo.Name);
            }
            */

            FileInfo fileInfo = new FileInfo(e.FullPath);
            PhactoryHost.Database.Resource resource = Helper.DBHelper.GuessResource(fileInfo);
            if (resource != null)
            {
                App.Controller.View.OnResourceRenamed(resource);
            }
        }
    }
}
