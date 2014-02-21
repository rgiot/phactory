using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Project.Helper
{
    static public class DBHelper
    {
        static public bool CreateNewProject(string filename, string projectName)
        {
            PhactoryHost.Database.Project project = new PhactoryHost.Database.Project();
            project.ProjectName = projectName;
            
            PhactoryHost.Database.Node treeNode;
            PhactoryHost.Database.Node sourceTreeNode;

            sourceTreeNode = new PhactoryHost.Database.Node();
            sourceTreeNode.IsFolder = true;
            sourceTreeNode.FolderName = "Source";
            project.TreeNode.ChildNodes.Add(sourceTreeNode);            
            sourceTreeNode.Expanded = true;

            treeNode = new PhactoryHost.Database.Node();
            treeNode.IsFolder = true;
            treeNode.FolderName = "Output";
            project.TreeNode.ChildNodes.Add(treeNode);
            project.TreeNode.Expanded = true;

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            Helper.ClassSerializer.Write(filename, project);

            FileInfo fileInfo = new FileInfo(filename);
            App.Controller.UserConfig.ProjectFilename = filename;
            App.Controller.UserConfig.ProjectPath = fileInfo.DirectoryName + "\\";
            App.Controller.UserConfig.ResourcePath = fileInfo.DirectoryName + "\\Resources\\";
            App.Controller.UserConfig.AddExistingResourceFullPath = fileInfo.DirectoryName + "\\";

            Directory.CreateDirectory(App.Controller.UserConfig.ResourcePath);

            App.Controller.Entities = project;

            return true;
        }

        static public bool OpenProject(string filename)
        {
            App.Controller.Entities = Helper.ClassSerializer.Read<PhactoryHost.Database.Project>(filename);
            if (App.Controller.Entities != null)
            {
                FileInfo fileInfo = new FileInfo(filename);
                App.Controller.UserConfig.ProjectFilename = filename;
                App.Controller.UserConfig.ProjectPath = fileInfo.DirectoryName + "\\";
                App.Controller.UserConfig.ResourcePath = fileInfo.DirectoryName + "\\Resources\\";

                foreach (PhactoryHost.Database.Resource iResource in App.Controller.Entities.Resources)
                {
                    iResource.LastWriteTime = iResource.FileInfo.LastWriteTime;
                }

                string projectName = App.Controller.Entities.ProjectName;
                App.Controller.Log.Append(projectName + " opened (" + fileInfo.FullName + ")");

                return true;
            }

            return false;
        }

        static public PhactoryHost.Database.Resource GuessResource(FileInfo fileInfo)
        {
            foreach (PhactoryHost.Database.Resource resource in App.Controller.Entities.Resources)
            {
                FileInfo iFileInfo = new FileInfo(App.Controller.UserConfig.ResourcePath + resource.RelativePath);

                if (iFileInfo.FullName.ToUpper() == fileInfo.FullName.ToUpper())
                {
                    return resource;
                }
            }

            return null;
        }

        static public bool IsOutputResourceSlow(PhactoryHost.Database.Resource resource)
        {
            foreach (PhactoryHost.Database.Resource iResource in App.Controller.Entities.Resources)
            {
                foreach (int resourceOutputId in iResource.IdOutputs)
                {
                    PhactoryHost.Database.Resource resourceOutput = App.Controller.Entities.GetResource(resourceOutputId);
                    if (resourceOutput == resource)
                    {
                        return true;
                    }
                }
            }
                   
            return false;
        }

        static private void GetOutputResourcesRecursiveExecute(PhactoryHost.Database.Resource resource, List<PhactoryHost.Database.Resource> outputResources)
        {
            foreach (PhactoryHost.Database.Resource resourceOutput in GetOutputResources(resource))
            {
                outputResources.Add(resourceOutput);

                GetOutputResourcesRecursiveExecute(resourceOutput, outputResources);
            }
        }

        static public List<PhactoryHost.Database.Resource> GetOutputResourcesRecursive(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> outputResources = new List<PhactoryHost.Database.Resource>();

            GetOutputResourcesRecursiveExecute(resource, outputResources);

            return outputResources;
        }

        static private void RenameResourceRecursive(PhactoryHost.Database.Resource resource, string previousName, string newName)
        {
            List<PhactoryHost.Database.Resource> outputResources = GetOutputResources(resource);
            foreach (PhactoryHost.Database.Resource outputResource in outputResources)
            {
                outputResource.DisplayName = outputResource.DisplayName.Replace(previousName, newName);
                outputResource.RelativePath = outputResource.RelativePath.Replace(previousName, newName);

                FileInfo fileInfo = new FileInfo(newName);
                PhactoryHost.EditorPlugin editor = App.Controller.PluginManager.GetEditor(fileInfo.Extension);
                if (editor != null)
                {
                    editor.CreateEmptyResource(outputResource);
                }

                RenameResourceRecursive(outputResource, previousName, newName);
            }
        }

        static public void RenameResource(PhactoryHost.Database.Resource resource, string previousName, string newName)
        {
            // Clean Output
            App.Controller.Build.Clean(resource, false);

            string[] previouses = previousName.Split('.');
            string[] newNames = newName.Split('.');

            RenameResourceRecursive(resource, previouses[0], newNames[0]);

            App.Controller.SaveProject();
        }
        
        static public List<PhactoryHost.Database.Resource> GetOutputResources(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> outputResources = new List<PhactoryHost.Database.Resource>();
            foreach( int iOutputResourceId in resource.IdOutputs)
            {
                outputResources.Add(App.Controller.Entities.GetResource(iOutputResourceId));
            }
            return outputResources;
        }

        static private void RemoveResourceWithOutput(bool deleteLocalFile, PhactoryHost.Database.Resource resource, List<PhactoryHost.Database.Resource> objectToDelete)
        {
            List<int> deleteIds = new List<int>();
            foreach ( int resourceOutputId in resource.IdOutputs)
            {
                var resourceOutput = App.Controller.Entities.GetResource(resourceOutputId);

                objectToDelete.Add(resourceOutput);
                deleteIds.Add(resourceOutputId);
            }

            foreach (int deleteResourceOutputId in deleteIds)
            {
                resource.IdOutputs.Remove(deleteResourceOutputId);
            }

            if (deleteLocalFile)
            {
                string location = Helper.StringHelper.MakeFullPath(resource.RelativePath);

                if (File.Exists(location))
                {
                    File.SetAttributes(location, FileAttributes.Normal);
                    File.Delete(location);
                }
            }

            App.Controller.CloseResource(resource);

            foreach ( int resourceDependencyId in resource.IdDependencies)
            {
                PhactoryHost.Database.Resource resourceDependency = App.Controller.Entities.GetResource(resourceDependencyId);
                objectToDelete.Add(resourceDependency);
            } 
            
            objectToDelete.Add(resource);
        }

        static public bool CheckIfResourceCanBeRemoved(PhactoryHost.Database.Resource resource)
        {
            if (resource.IsOutputResource)
            {
                string errorMessage = "Can not remove '" + resource.DisplayName + "' because it's an output resource";

                App.Controller.Log.Append(errorMessage);
                App.Controller.View.ShowErrorMessage("Can't remove resource", errorMessage);

                return false;
            }

            List<PhactoryHost.Database.Resource> resourceDependencies = GetDependentResources(resource);
            if (resourceDependencies.Count != 0)
            {
                string errorMessage = "Can not remove '" + resource.DisplayName + "' because it's referenced by :\n";
                errorMessage += "\n";
                foreach (PhactoryHost.Database.Resource res in resourceDependencies)
                {
                    errorMessage += "'" + res.DisplayName + "'\n";
                }

                App.Controller.Log.Append(errorMessage);
                App.Controller.View.ShowErrorMessage("Can't remove resource", errorMessage);

                return false;
            }

            return true;
        }
        
        static public bool RemoveResourceWithOutput(bool deleteLocalFile, PhactoryHost.Database.Resource resource)
        {
            var entitiesToDelete = new List<PhactoryHost.Database.Resource>();

            RemoveResourceWithOutput(deleteLocalFile, resource, entitiesToDelete);

            foreach (PhactoryHost.Database.Resource delResource in entitiesToDelete)
            {
                App.Controller.Entities.DeleteResource(delResource);
            }
            App.Controller.SaveProject();

            return true;
        }

        static public bool IsResourceUsedByOtherResourceBeingNotOutput(PhactoryHost.Database.Resource resource)
        {
            return ( GetDependentResources( resource ).Count != 0);
        }

        // return the list of resources using a specified resource (or its output)
        static private void GetDependentResources(List<PhactoryHost.Database.Resource> outDependentResources, PhactoryHost.Database.Resource resource)
        {
            foreach( PhactoryHost.Database.Resource iResource in App.Controller.Entities.Resources )
            {
                if (App.Controller.Entities.IsOutput(iResource) == false)
                {
                    foreach (int dependentResourceId in iResource.IdDependencies)
                    {
                        PhactoryHost.Database.Resource iDependentResource = App.Controller.Entities.GetResource(dependentResourceId);

                        if ((iDependentResource!=null)&&(iDependentResource.Id == resource.Id))
                        {
                            outDependentResources.Add(iResource);
                        }
                    }
                }
            }
        }

        // return the list of resources using a specified resource (or its output)
        static public List<PhactoryHost.Database.Resource> GetDependentResources(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> outDependentResources = new List<PhactoryHost.Database.Resource>();

            if (resource != null)
            {
                foreach (int outputResourceId in resource.IdOutputs)
                {
                    PhactoryHost.Database.Resource iOutputResource = App.Controller.Entities.GetResource(outputResourceId);

                    GetDependentResources(outDependentResources, iOutputResource);
                }
                GetDependentResources(outDependentResources, resource);
            }
            
            return outDependentResources;
        }
    }
}
