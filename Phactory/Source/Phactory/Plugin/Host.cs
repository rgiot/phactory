using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using PhactoryHost;

namespace Project.Plugin
{
    public class Host : PhactoryHost.Host
    {
        public string GetPluginStringValue(string key, string defaultValue)
        {
            if (App.Controller.UserConfig.PluginSettings.ContainsKey(key))
            {
                return App.Controller.UserConfig.PluginSettings[key];
            }
            return defaultValue;
        }

        public int GetPluginIntValue(string key, int defaultValue)
        {
            if (App.Controller.UserConfig.PluginSettings.ContainsKey(key))
            {
                return Int32.Parse(App.Controller.UserConfig.PluginSettings[key]);
            }
            return defaultValue;
        }

        public bool GetPluginBoolValue(string key, bool defaultValue)
        {
            if (App.Controller.UserConfig.PluginSettings.ContainsKey(key))
            {
                return Boolean.Parse(App.Controller.UserConfig.PluginSettings[key]);
            }
            return defaultValue;
        }

        public void SetPluginStringValue(string key, string value)
        {
            if (App.Controller.UserConfig.PluginSettings.ContainsKey(key))
            {
                App.Controller.UserConfig.PluginSettings[key] = value;
            }
            else
            {
                App.Controller.UserConfig.PluginSettings.Add(key, value);
            }
        }

        public void SetPluginIntValue(string key, int value)
        {
            if (App.Controller.UserConfig.PluginSettings.ContainsKey(key))
            {
                App.Controller.UserConfig.PluginSettings[key] = "" + value;
            }
            else
            {
                App.Controller.UserConfig.PluginSettings.Add(key, "" + value);
            }
        }

        public void SetPluginBoolValue(string key, bool value)
        {
            if (App.Controller.UserConfig.PluginSettings.ContainsKey(key))
            {
                App.Controller.UserConfig.PluginSettings[key] = value ? Boolean.TrueString : Boolean.FalseString;
            }
            else
            {
                App.Controller.UserConfig.PluginSettings.Add(key, value ? Boolean.TrueString : Boolean.FalseString);
            }
        }

        public IntPtr GetAppViewHandle()
        {
            return App.Controller.View.Handle;
        }

        public void AppDoEvents()
        {
            App.Controller.View.AppDoEvents();
        }

        public PhactoryHost.Database.Project GetEntities()
        {
            return App.Controller.Entities;
        }

        public void Log(String statusText)
        {
            App.Controller.Log.Append(statusText);
        }

        public bool IsVerboseOutput()
        {
            return App.Controller.UserConfig.VerboseOutput;
        }

        public bool StartProcess(string filename, string arguments, string workingDirectory, bool waitForExit)
        {
            return Helper.Shell.StartProcess( filename, arguments, workingDirectory, waitForExit, false);
        }

        public bool StartAndWaitAfterProcess(string filename, string arguments, string workingDirectory)
        {
            return Helper.Shell.StartProcess(filename, arguments, workingDirectory, false, true);
        }

        public String GetLastErrorOutput()
        {
            return Helper.Shell.LastErrorOutput;
        }

        public String GetLastStandardOutput()
        {
            return Helper.Shell.LastStandardOutput;
        }

        public void RequestSave(PhactoryHost.Database.Resource resource)
        {
            App.Controller.SaveResource(resource);
        }

        public PhactoryHost.Database.Resource GetFirstParent(PhactoryHost.Database.Resource resource)
        {
            if (resource.IdDependencies.Count > 0)
            {
                int parentId = resource.IdDependencies[0];
                return App.Controller.Entities.GetResource(parentId);
            }

            return null;
        }

        public PhactoryHost.Database.Resource GuessResource(FileInfo fileInfo)
        {
            return Helper.DBHelper.GuessResource(fileInfo);
        }

        public PhactoryHost.Database.Resource GetResource(int resId)
        {
            foreach (PhactoryHost.Database.Resource resource in App.Controller.Entities.Resources)
            {
                if (resource.Id == resId)
                {
                    return resource;
                }
            }

            return null;
        }

        public PhactoryHost.Database.Resource GetStartupResource()
        {
            return GetResource(App.Controller.Entities.StartupResourceId);
        }

        public List<PhactoryHost.Database.Resource> IsOutputResourcesReferencedByOtherResources(PhactoryHost.Database.Resource resource, PhactoryHost.Database.Resource editedResource)
        {
            List<PhactoryHost.Database.Resource> dependentResourceIDs = new List<PhactoryHost.Database.Resource>();

            foreach (int editedOutputResourceId in editedResource.IdOutputs)
            {
                PhactoryHost.Database.Resource editedOutputResource = App.Controller.Entities.GetResource(editedOutputResourceId);

                if (editedOutputResource.DisplayName.Contains(resource.DisplayName))
                {
                    List<PhactoryHost.Database.Resource> outputResourceDependentResources = IsResourceReferencedByOtherResources(editedOutputResource);
                    foreach (PhactoryHost.Database.Resource outputResourceDependentResource in outputResourceDependentResources)
                    {
                        if (!dependentResourceIDs.Contains(outputResourceDependentResource))
                        {
                            dependentResourceIDs.Add(outputResourceDependentResource);
                        }
                    }
                }
            }

            return dependentResourceIDs;
        }

        public List<PhactoryHost.Database.Resource> IsResourceReferencedByOtherResourcesCACHE(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> dependentResourceIDs = new List<PhactoryHost.Database.Resource>();

            if (resource != null)
            {
                if (resource.SkipIsResourceReferencedByOtherResources)
                {
                    return resource.IsResourceReferencedByOtherResources;
                }

                foreach (PhactoryHost.Database.Resource iResource in App.Controller.Entities.Resources)
                {
                    foreach (int dependentResourceId in iResource.IdDependencies)
                    {
                        if (resource.Id == dependentResourceId)
                        {
                            if (!dependentResourceIDs.Contains(iResource))
                            {
                                dependentResourceIDs.Add(iResource);
                            }
                        }
                    }
                }

                resource.SkipIsResourceReferencedByOtherResources = true;
                resource.IsResourceReferencedByOtherResources = dependentResourceIDs;
            }

            return dependentResourceIDs;
        }

        public List<PhactoryHost.Database.Resource> IsResourceReferencedByOtherResources(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> dependentResourceIDs = new List<PhactoryHost.Database.Resource>();

            foreach (PhactoryHost.Database.Resource iResource in App.Controller.Entities.Resources)
            {
                foreach (int dependentResourceId in iResource.IdDependencies)
                {
                    if (resource.Id == dependentResourceId)
                    {
                        if (!dependentResourceIDs.Contains(iResource))
                        {
                            dependentResourceIDs.Add(iResource);
                        }
                    }
                }
            }

            return dependentResourceIDs;
        }

        public bool IsResourceReferencedByOtherResourcesRecursive(PhactoryHost.Database.Resource resource)
        {
            if (App.Controller.Entities.StartupResourceId == resource.Id)
            {
                return true;
            }

            foreach (int outputResourceId in resource.IdOutputs)
            {
                if (outputResourceId == App.Controller.Entities.StartupResourceId)
                {
                    return true;
                }

                PhactoryHost.Database.Resource outputResource = GetResource(outputResourceId);
                if (IsResourceReferencedByOtherResourcesRecursive(outputResource))
                {
                    return true;
                }
            }

            List<PhactoryHost.Database.Resource> depResources = IsResourceReferencedByOtherResources(resource);
            foreach (PhactoryHost.Database.Resource depResource in depResources)
            {
                if (IsResourceReferencedByOtherResourcesRecursive(depResource))
                {
                    return true;
                }
            }

            return false;
        }

        public void ShowCantRemoveBecauseOtherResources(PhactoryHost.Database.Resource resourceToRemove, PhactoryHost.Database.Resource editedResource, List<PhactoryHost.Database.Resource> otherResources)
        {
            string title = "Can't remove '" + resourceToRemove.DisplayName + "'";
            string content = "Output of '" + resourceToRemove.DisplayName + "' is still referenced by :\n\n";

            for (int iResource = 0; iResource < otherResources.Count; iResource++)
            {
                content += otherResources[iResource].DisplayName + "\n";
            }

            content += "\nRemove first these references.";

            App.Controller.Log.Append(content);

            App.Controller.View.ShowErrorMessage(title, content);
        }

        public FileInfo GetFileInfo(PhactoryHost.Database.Resource resource)
        {
            return Helper.StringHelper.GetFileInfo(resource);
        }

        public string MakeRelativePath(string fullPath)
        {
            return Helper.StringHelper.MakeRelativePath(fullPath);
        }

        public string MakeFullPath(string fullPath)
        {
            return Helper.StringHelper.MakeFullPath(fullPath);
        }

        public string GetProjectPath()
        {
            return App.Controller.UserConfig.ProjectPath;
        }

        public string GetResourcePath()
        {
            return App.Controller.UserConfig.ResourcePath;
        }

        public string GetPluginsPath()
        {
            return App.Controller.Config.ApplicationPath + App.Controller.Config.PluginPath;
        }

        public void SetToolTipText(Panel panel, string toolTipText)
        {
            App.Controller.View.SetToolTipText(panel, toolTipText);
        }

        public T XMLRead<T>(string filename)
        {
            return Helper.ClassSerializer.Read<T>(filename);
        }

        public bool XMLWrite(string filename, object instance)
        {
            return Helper.ClassSerializer.Write(filename, instance);
        }

        public List<PhactoryHost.Database.Resource> ShowResourceSelector()
        {
            return ShowResourceSelector(null);
        }

        public List<PhactoryHost.Database.Resource> ShowResourceSelector(PhactoryHost.Database.Resource defaultResource)
        {
            return ShowResourceSelector(defaultResource, false);
        }

        public List<PhactoryHost.Database.Resource> ShowResourceSelector(PhactoryHost.Database.Resource defaultResource, bool multiSelect)
        {
            return ShowResourceSelector(defaultResource, multiSelect, null);
        }

        public List<PhactoryHost.Database.Resource> ShowResourceSelector(PhactoryHost.Database.Resource defaultResource, bool multiSelect, List<string> extensionFilter)
        {
            Project.View.ResourceSelector resourceSelectorDialog = new Project.View.ResourceSelector(defaultResource, multiSelect, extensionFilter);
            resourceSelectorDialog.ShowDialog(App.Controller.View);

            if (resourceSelectorDialog.Valid == false)
            {
                return null;
            }

            return resourceSelectorDialog.Resources;
        }

        public List<PhactoryHost.Database.Resource> ShowResourceSelector(PhactoryHost.Database.Resource defaultResource, List<string> extensionFilter)
        {
            Project.View.ResourceSelector resourceSelectorDialog = new Project.View.ResourceSelector(defaultResource, false, extensionFilter);
            resourceSelectorDialog.ShowDialog(App.Controller.View);

            if (resourceSelectorDialog.Valid == false)
            {
                return null;
            }

            return resourceSelectorDialog.Resources;
        }

        public bool IsOutputResourceSlow(PhactoryHost.Database.Resource resource)
        {
            return Helper.DBHelper.IsOutputResourceSlow(resource);
        }
    
        public List<PhactoryHost.Database.Resource> GetOutputResources(PhactoryHost.Database.Resource resource)
        {
            return Helper.DBHelper.GetOutputResources(resource);
        }

        public List<PhactoryHost.Database.Resource> GetDependencyResources(PhactoryHost.Database.Resource resource)
        {
            return Helper.DBHelper.GetDependentResources(resource);
        }

        public void RefreshOutput(PhactoryHost.Database.Resource resource)
        {
            App.Controller.View.AppDoEvents();

            string extension = Helper.StringHelper.GetFileInfo(resource).Extension;

            PhactoryHost.CompilerPlugin compiler = App.Controller.PluginManager.GetCompiler(extension); 
            if (compiler != null)
            {
                compiler.RefreshOutput(resource);
            }
        }

        public void RefreshDependencies(PhactoryHost.Database.Resource resource, List<PhactoryHost.Database.Resource> dependentResources)
        {
            App.Controller.View.AppDoEvents();

            // output resources do not have to be treated here
            if (resource.IsOutputResource)
            {
                return;
            }

            bool saveProject = false;
                    
            // remove pass (test resourceDependencies against given dependentResources)
            for ( int iIdDependency = 0; iIdDependency < resource.IdDependencies.Count; iIdDependency++)
            {
                int resourceDependencyId = resource.IdDependencies[iIdDependency];

                PhactoryHost.Database.Resource resourceDependency = App.Controller.Entities.GetResource(resourceDependencyId);
                
                bool found = false;
                foreach (PhactoryHost.Database.Resource dependentResource in dependentResources)
                {
                    if ((resourceDependency != null) && (dependentResource.Id == resourceDependency.Id))
                    {
                        found = true;
                    }
                }

                if (found == false)
                {
                    resource.IdDependencies.Remove(resourceDependencyId);

                    saveProject = true;
                }
            }

            // add pass (test given dependentResources against resourceDependencies)
            foreach (PhactoryHost.Database.Resource dependentResource in dependentResources)
            {
                bool found = false;
                foreach (int resourceDependencyId in resource.IdDependencies)
                {
                    PhactoryHost.Database.Resource iDependentResource = App.Controller.Entities.GetResource(resourceDependencyId);

                    if ((iDependentResource!=null)&&(iDependentResource.Id == dependentResource.Id))
                    {
                        found = true;
                    }
                }

                if (found == false)
                {
                    resource.IdDependencies.Add(dependentResource.Id);

                    saveProject = true;
                }
            }

            if (saveProject)
            {
                App.Controller.SaveProject();
            }
        }

        public void RefreshOutput(PhactoryHost.Database.Resource resource, List<string> outputFilenames)
        {
            bool saveProject = false;
            
            Dictionary<PhactoryHost.Database.Resource, bool> resourceOutputUsage = new Dictionary<PhactoryHost.Database.Resource, bool>();
            foreach (int resourceOutputId in resource.IdOutputs)
            {
                PhactoryHost.Database.Resource iResourceOutput = App.Controller.Entities.GetResource(resourceOutputId);
                if (iResourceOutput != null)
                {
                    resourceOutputUsage.Add(iResourceOutput, false);
                }
            }

            foreach (string outputFilename in outputFilenames)
            {
                FileInfo iFileInfo = new FileInfo(Helper.StringHelper.MakeFullPath(outputFilename));
                string destFilename = iFileInfo.FullName;

                string relativePath = Helper.StringHelper.MakeRelativePath(destFilename);

                PhactoryHost.Database.Resource resourceOutput = null;
                foreach (PhactoryHost.Database.Resource iResourceOutput in resourceOutputUsage.Keys)
                {
                    if (iResourceOutput.RelativePath == relativePath)
                    {
                        resourceOutput = iResourceOutput;
                        resourceOutputUsage[iResourceOutput] = true;
                        break;
                    }
                }

                if (resourceOutput == null)
                {
                    PhactoryHost.Database.Resource newResource = new PhactoryHost.Database.Resource();
                    newResource.Id = App.Controller.Entities.CreateNewResourceId();
                    newResource.DisplayName = new FileInfo(destFilename).Name;
                    newResource.RelativePath = relativePath;
                    newResource.IsOutputResource = true;
                    App.Controller.Entities.AddResource(newResource);

                    // create resource output link
                    resource.IdOutputs.Add(newResource.Id);
                    
                    App.Controller.CreateNewResource(newResource);
                    
                    // also create dependency link
                    newResource.IdDependencies.Add(resource.Id);

                    saveProject = true;
                }
            }

            // remove unused resources
            foreach (KeyValuePair<PhactoryHost.Database.Resource, bool> kvp in resourceOutputUsage)
            {
                if (kvp.Value == false)
                {
                    resource.IdOutputs.Remove(kvp.Key.Id);
                    App.Controller.Entities.DeleteResource(kvp.Key);

                    saveProject = true;
                }
            }

            if (saveProject)
            {
                App.Controller.SaveProject();

                App.Controller.View.ProjectExplorerRefreshNodeOutput(resource);
            }
        }

        public void RefreshDB()
        {
            App.Controller.View.RefreshDB();
        }
    }
}
