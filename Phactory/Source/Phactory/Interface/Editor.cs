using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhactoryHost
{
    public interface EditorPlugin : Plugin
    {
        List<PluginExtension> GetSupportedExtensions();

        bool IsDefaultPluginForUnknownTypes();
        string GetDefaultExtensionForNewResource();

        bool IsResourceSupported(Database.Resource resource); 
        
        void SetLine(Database.Resource resource, int line);

        bool CreateEmptyResource(Database.Resource resource);

        void OpenResource(Panel parentPanel, Database.Resource resource);
        bool CloseResource(Database.Resource resource);
        void SaveResource(Database.Resource resource);
        bool IsResourceModified(Database.Resource resource);
        bool IsResourceOpened(Database.Resource resource);

        void RefreshViewTitle(Database.Resource resource);

        void OnResourceDeleted(Database.Resource resource);
        void OnResourceChanged(Database.Resource resource);
        void OnResourceRenamed(Database.Resource resource); 
    }
}
