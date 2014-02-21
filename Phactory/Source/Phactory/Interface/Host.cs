using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PhactoryHost
{
    public interface Host
    {
        string GetPluginStringValue(string key, string defaultValue);
        int GetPluginIntValue(string key, int defaultValue);
        bool GetPluginBoolValue(string key, bool defaultValue);
        void SetPluginStringValue(string key, string value);
        void SetPluginIntValue(string key, int value);
        void SetPluginBoolValue(string key, bool value);

        IntPtr GetAppViewHandle();
        void AppDoEvents();

        string MakeFullPath(string fullPath);
        string MakeRelativePath(string fullPath);
        string GetProjectPath();
        string GetPluginsPath();
            
        void Log(string statusText);
        
        bool StartProcess(string filename, string arguments, string workingDirectory, bool waitForExit);
        bool StartAndWaitAfterProcess(string filename, string arguments, string workingDirectory);
        String GetLastErrorOutput();
        String GetLastStandardOutput();

        bool IsVerboseOutput();

        void RequestSave(Database.Resource resource);

        void RefreshOutput(Database.Resource resource);
        void RefreshOutput(Database.Resource resource, List<string> outputFilenames);
        void RefreshDependencies(Database.Resource resource, List<Database.Resource> dependentResources);

        List<Database.Resource> IsOutputResourcesReferencedByOtherResources(Database.Resource resource, Database.Resource editedResource);
        List<Database.Resource> IsResourceReferencedByOtherResources(Database.Resource resource);
        List<Database.Resource> IsResourceReferencedByOtherResourcesCACHE(Database.Resource resource);
        bool IsResourceReferencedByOtherResourcesRecursive(Database.Resource resource);
        void ShowCantRemoveBecauseOtherResources(Database.Resource resourceToRemove, Database.Resource editedResource, List<Database.Resource> otherResources);
        
        FileInfo GetFileInfo(Database.Resource resource);
        Database.Resource GetFirstParent(Database.Resource resource);
        Database.Resource GuessResource(FileInfo fileInfo);
        Database.Resource GetResource(int resId);
        Database.Resource GetStartupResource();

        List<Database.Resource> GetOutputResources(Database.Resource resource);
        List<Database.Resource> GetDependencyResources(Database.Resource resource);
        bool IsOutputResourceSlow(Database.Resource resource);

        Database.Project GetEntities();

        void RefreshDB();

        void SetToolTipText(Panel panel, string toolTipText);

        T XMLRead<T>(string filename);
        bool XMLWrite(string filename, object instance);

        List<Database.Resource> ShowResourceSelector();
        List<Database.Resource> ShowResourceSelector(Database.Resource defaultResource);
        List<Database.Resource> ShowResourceSelector(Database.Resource defaultResource, bool multiSelect);
        List<Database.Resource> ShowResourceSelector(Database.Resource defaultResource, List<string> extensionFilter);
        List<Database.Resource> ShowResourceSelector(Database.Resource defaultResource, bool multiSelect, List<string> extensionFilter);
    }
}
