using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace Project.Controller
{
    public class Build
    {
        public Build()
        {
        }

        public bool CancelCompile = false;

        private List<PhactoryHost.Database.Resource> GetOutput(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> outputs = new List<PhactoryHost.Database.Resource>();

            if (resource != null)
            {
                foreach (int resourceOutputId in resource.IdOutputs)
                {
                    outputs.Add(App.Controller.Entities.GetResource(resourceOutputId));
                }
            }
            return outputs;
        }

        private void CleanResource(PhactoryHost.Database.Resource resource)
        {
            if (resource == null)
            {
                return;
            }

            if (resource.IsOutputResource == false)
            {
                return;
            }

            string filename = resource.RelativePath;
            string extension = Helper.StringHelper.GetFileExtension(filename);

            string location = Helper.StringHelper.MakeFullPath(resource.RelativePath);
            if (File.Exists(location))
            {
                // App.Controller.Log.Append(resource.DisplayName);
            
                File.SetAttributes(location, FileAttributes.Normal);
                File.Delete(location);
            }
        }

        private void CleanResourceOutputRecursive(PhactoryHost.Database.Resource resource)
        {
            App.Controller.View.AppDoEvents();

            CleanResource(resource);

            int iCount = 0;

            List<PhactoryHost.Database.Resource> outputResources = GetOutput(resource);
            foreach (PhactoryHost.Database.Resource outputResource in outputResources)
            {
                CleanResourceOutputRecursive(outputResource);

                iCount++;
                if (iCount > 50)
                {
                    iCount = 0;
                    App.Controller.View.AppDoEvents();
                }
            }
        }

        private bool CompileResource(PhactoryHost.Database.Resource resource)
        {
            if (App.Controller.UserConfig.VerboseOutput == false)
            {
                if (resource.IdOutputs.Count == 1)
                {
                    PhactoryHost.Database.Resource outputResource = App.Controller.Entities.GetResource(resource.IdOutputs[0]);

                    if (outputResource.Id != App.Controller.Entities.StartupResourceId)
                    {
                        if (App.Controller.PluginManager.Host.IsResourceReferencedByOtherResources(outputResource).Count == 0)
                        {
                            // simply skip compilation
                            return true;
                        }
                    }
                }
            }

            string filename = resource.RelativePath;
            string extension = Helper.StringHelper.GetFileExtension(filename);

            PhactoryHost.CompilerPlugin compiler = App.Controller.PluginManager.GetCompiler(extension);
            if (compiler == null)
            {
                if (App.Controller.UserConfig.VerboseOutput)
                {
                    App.Controller.Log.Append(resource.DisplayName + " (skipped)");
                }
                return true;
            }

            bool showInLog = true;
            string typeInfo = "";

            if (resource.IsIncludeResource)
            {
                typeInfo += " (include resource)";

                showInLog = App.Controller.UserConfig.VerboseOutput;
            }

            if (resource.IsOutputResource)
            {
                typeInfo += " (output resource)";

                showInLog = App.Controller.UserConfig.VerboseOutput;
            }

            if (showInLog)
            {
                App.Controller.Log.Append(resource.DisplayName + typeInfo);
            }

            App.Controller.View.AppDoEvents();

            return compiler.Compile(resource);
        }

        private bool CompileResourceOutputRecursive(PhactoryHost.Database.Resource resource)
        {
            App.Controller.View.AppDoEvents();

            bool compiledOK = CompileResource(resource);

            if (compiledOK)
            {
                List<PhactoryHost.Database.Resource> outputResources = GetOutput(resource);
                foreach (PhactoryHost.Database.Resource outputResource in outputResources)
                {
                    if (CompileResourceOutputRecursive(outputResource) == false)
                    {
                        compiledOK = false;
                        break;
                    }
                }
            }

            return compiledOK;
        }

        public void Clean(PhactoryHost.Database.Resource resource, bool clearLog)
        {
            if (clearLog)
            {
                App.Controller.View.ClearLog();
            }

            App.Controller.Build.CancelCompile = false;
            App.Controller.View.EnableCancelBuild(true, true);
    
            App.Controller.Log.Append("------ Clean started: " + App.Controller.Entities.ProjectName + " ------");

            App.Controller.Log.Append("Determinig resource(s) to clean...");
            List<PhactoryHost.Database.Resource> resourceDependencies = new List<PhactoryHost.Database.Resource>();
            UpdateDependenciesRecursive(resource, resourceDependencies);

            if (resourceDependencies.Count == 0)
            {
                App.Controller.Log.Append("No resource(s) to clean.");
            }
            else
            {
                App.Controller.Log.Append("Cleaning...");

                foreach (PhactoryHost.Database.Resource resourceDependency in resourceDependencies)
                {
                    CleanResourceOutputRecursive(resourceDependency);
                }

                CleanResourceOutputRecursive(resource);

                App.Controller.Log.Append("Clean completed !");
            }

            App.Controller.View.EnableCancelBuild(false, false);
        }

        public bool Compile(PhactoryHost.Database.Resource resource, bool clearLog)
        {
            if (clearLog)
            {
                App.Controller.View.ClearLog();
            }

            App.Controller.Build.CancelCompile = false;
            App.Controller.View.EnableCancelBuild(true, false);

            App.Controller.Log.Append("------ Build started: " + App.Controller.Entities.ProjectName + " ------");

            App.Controller.Log.Append("Resolving dependencies...");
            List<PhactoryHost.Database.Resource> orderedResources = DependencyResolve(resource);
            
            bool isOK = true;
            int compiled = 0;

            // remove resources from list that does not need to be compiled
            if ( orderedResources.Count > 0 )
            {
                List<PhactoryHost.Database.Resource> noNeedToCompileResources = new List<PhactoryHost.Database.Resource>(App.Controller.Entities.Resources.Count);

                // refresh all file infos
                foreach (PhactoryHost.Database.Resource iResource in App.Controller.Entities.Resources)
                {
                    var fullPath = App.Controller.UserConfig.ResourcePath + iResource.RelativePath;

                    iResource.LastWriteTime = File.GetLastWriteTime(fullPath);
                    iResource.SkipNeedCompile = false;
                    iResource.SkipIsResourceReferencedByOtherResources = false;
                    iResource.NeedCompile = false;
                    iResource.FileExists = File.Exists(fullPath);
                }

                App.Controller.View.AppDoEvents();

                App.Controller.Log.Append("Determinig resource(s) to compile...");

                int indexOrderedResource = (orderedResources.Count - 1);
                foreach( PhactoryHost.Database.Resource iOrderedResource in orderedResources )
                {
                    App.Controller.View.AppDoEvents(); 
                    
                    if (DoesResourceNeedCompile(iOrderedResource, true))
                    {
                        if (App.Controller.UserConfig.VerboseOutput)
                        {
                            App.Controller.Log.Append(iOrderedResource.DisplayName + " needs compile");
                        }
                    }
                    else
                    {
                        noNeedToCompileResources.Add(iOrderedResource);
                    }
                }

                // remove from list
                foreach (PhactoryHost.Database.Resource noNeedToCompileResource in noNeedToCompileResources)
                {
                    orderedResources.Remove(noNeedToCompileResource);

                    App.Controller.View.AppDoEvents();
                }
            }

            if (orderedResources.Count == 0)
            {
                App.Controller.Log.Append("Build is up-to-date.");
            }
            else
            {
                App.Controller.Log.Append("Compiling " + orderedResources.Count + " resource(s)...");
            }

            float iProgressResource = 0.0f;

            foreach (PhactoryHost.Database.Resource orderedResource in orderedResources)
            {
                if (CompileResource(orderedResource) == false)
                {
                    isOK = false;
                    break;
                }

                if (App.Controller.Build.CancelCompile == true)
                {
                    App.Controller.View.EnableCancelBuild(false, false); 
                    App.Controller.Build.CancelCompile = false;
                    App.Controller.Log.Append("Build cancelled !");
                    return false;
                }

                compiled++;

                float percent = (iProgressResource / orderedResources.Count) * 100.0f;
                App.Controller.View.SetProgressBar((int)percent);
                iProgressResource += 1.0f;

                App.Controller.View.AppDoEvents();
            }

            if (isOK)
            {
                List<PhactoryHost.Database.Resource> outputResources = GetOutput(resource);
                foreach (PhactoryHost.Database.Resource outputResource in outputResources)
                {
                    if (CompileResourceOutputRecursive(outputResource) == false)
                    {
                        isOK = false;
                        break;
                    }

                    if (App.Controller.Build.CancelCompile == true)
                    {
                        App.Controller.View.EnableCancelBuild(false, false); 
                        App.Controller.Build.CancelCompile = false; 
                        App.Controller.Log.Append("Build cancelled !");
                        return false;
                    }

                    compiled++;
                }
            }

            App.Controller.View.EnableCancelBuild(false, false);

            if (orderedResources.Count > 0)
            {
                if (isOK)
                {
                    App.Controller.Log.Append("Build completed !");
                }
                else
                {
                    App.Controller.Log.Append("Build failed !");
                }
            }

            return isOK;
        }

        public bool CleanCompile(PhactoryHost.Database.Resource resource)
        {
            Clean(resource, true);
            return Compile(resource, false);
        }

        public void Run(PhactoryHost.Database.Resource resource)
        {
            string filename = resource.RelativePath;
            string extension = Helper.StringHelper.GetFileExtension(filename);

            PhactoryHost.RunnerPlugin runner = App.Controller.PluginManager.GetRunner(extension);
            if (runner == null)
            {
                App.Controller.Log.Append("Don't know how to run " + resource.DisplayName + " ...");
                App.Controller.View.ShowErrorMessage("Can not run", "This is no runner available for that kind of resource.");
                return;
            }

            App.Controller.Log.Append("Running " + resource.DisplayName + " ...");
            runner.Run(resource);

            App.Controller.View.Focus();
        }

        public void UpdateDependenciesRecursive(PhactoryHost.Database.Resource resource)
        {
            App.Controller.View.AppDoEvents();

            List<PhactoryHost.Database.Resource> resourceDependencies = new List<PhactoryHost.Database.Resource>();
            UpdateDependenciesRecursive(resource, resourceDependencies);
        }

        public void UpdateDependenciesRecursive(PhactoryHost.Database.Resource resource, List<PhactoryHost.Database.Resource> resourceDependencies)
        {
            App.Controller.View.AppDoEvents();

            if (resource == null)
            {
                return;
            }

            string filename = resource.RelativePath;
            string extension = Helper.StringHelper.GetFileExtension(filename);

            FileInfo resourceFileInfo = Helper.StringHelper.GetFileInfo(resource);
            if (resourceFileInfo.LastWriteTime > resource.LastWriteTime)
            {
                resource.LastWriteTime = resourceFileInfo.LastWriteTime;

                PhactoryHost.CompilerPlugin compiler = App.Controller.PluginManager.GetCompiler(extension);
                if (compiler != null)
                {
                    compiler.UpdateDependencies(resource);
                }
            }

            foreach (int resourceDependencyId in resource.IdDependencies)
            {
                PhactoryHost.Database.Resource resourceDependency = App.Controller.Entities.GetResource(resourceDependencyId);
                if ( !resourceDependencies.Contains(resourceDependency) )
                {
                    resourceDependencies.Add(resourceDependency);
                }
                UpdateDependenciesRecursive(resourceDependency, resourceDependencies);
            }
        }

        public void CreatePairValueRecursive(PhactoryHost.Database.Resource resource, List<Helper.TopologicalSort.PairValue> pairValues)
        {
            if (resource == null)
            {
                return;
            }
            foreach (int dependentResourceId in resource.IdDependencies)
            {
                bool exist = false;
                foreach( Helper.TopologicalSort.PairValue pairValue in pairValues )
                {
                    if ( ( pairValue.A == resource.Id ) && ( pairValue.B == dependentResourceId ) )
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    pairValues.Add(new Helper.TopologicalSort.PairValue(resource.Id, dependentResourceId));
                    CreatePairValueRecursive(App.Controller.Entities.GetResource(dependentResourceId), pairValues);
                }
            }
        }

        public List<PhactoryHost.Database.Resource> DependencyResolve(PhactoryHost.Database.Resource resource)
        {
            List<PhactoryHost.Database.Resource> orderedResources = new List<PhactoryHost.Database.Resource>();

            Helper.TopologicalSort topologicalSort = new Helper.TopologicalSort();
            List<Helper.TopologicalSort.PairValue> pairValues = new List<Helper.TopologicalSort.PairValue>();
            
            CreatePairValueRecursive(resource, pairValues);
            
            List<int> sortedIds = topologicalSort.Sort(pairValues);
            
            foreach( int resourceId in sortedIds )
            {
                orderedResources.Add(App.Controller.Entities.GetResource(resourceId));
            }
            orderedResources.Add(resource);

            return orderedResources;
        }

        public bool DoesIncludeResourceNeedCompile(PhactoryHost.Database.Resource resourceDependency, PhactoryHost.Database.Resource resource)
        {
            if (resourceDependency.SkipNeedCompile)
            {
                return resourceDependency.NeedCompile;
            }
            
            string filename = resourceDependency.RelativePath;
            string extension = Helper.StringHelper.GetFileExtension(filename);

            PhactoryHost.CompilerPlugin compiler = App.Controller.PluginManager.GetCompiler(extension);
            if (compiler != null)
            {
                if (resourceDependency.IsIncludeResource)
                {
                    if (resourceDependency.FileExists == false)
                    {
                        resourceDependency.SkipNeedCompile = true;
                        resourceDependency.NeedCompile = true;
                        return true;
                    }
                    else
                    {
                        foreach (int resourceOutputId in resource.IdOutputs)
                        {
                            PhactoryHost.Database.Resource resourceOutput = App.Controller.Entities.GetResource(resourceOutputId);

                            if (resourceOutput.FileExists == false)
                            {
                                resourceDependency.SkipNeedCompile = true;
                                resourceDependency.NeedCompile = true;
                                return true;
                            }
                            else
                            {
                                if (resourceDependency.LastWriteTime > resourceOutput.LastWriteTime)
                                {
                                    resourceDependency.SkipNeedCompile = true;
                                    resourceDependency.NeedCompile = true;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            foreach (int dependencyId in resourceDependency.IdDependencies)
            {
                PhactoryHost.Database.Resource iResourceDependency = App.Controller.Entities.GetResource(dependencyId);

                if (DoesIncludeResourceNeedCompile(iResourceDependency, resource))
                {
                    resourceDependency.SkipNeedCompile = true;
                    resourceDependency.NeedCompile = true;
                    return true;
                }
            }

            resourceDependency.SkipNeedCompile = true;
            resourceDependency.NeedCompile = false;
            return false;
        }

        public bool DoesResourceNeedCompile(PhactoryHost.Database.Resource resource, bool useCache)
        {
            if (resource == null)
            {
                return false;
            }

            if ( resource.SkipNeedCompile )
            {
                return resource.NeedCompile;
            }

            List<PhactoryHost.Database.Resource> resources;
            if (useCache)
            {
                resources = App.Controller.PluginManager.Host.IsResourceReferencedByOtherResourcesCACHE(resource);
            }
            else
            {
                resources = App.Controller.PluginManager.Host.IsResourceReferencedByOtherResources(resource);
            }

            foreach( PhactoryHost.Database.Resource referencingResource in resources)
            {
                if (!referencingResource.IsOutputResource)
                {
                    foreach (int outputId in referencingResource.IdOutputs)
                    {
                        PhactoryHost.Database.Resource resourceOutput = App.Controller.Entities.GetResource(outputId);

                        if (resourceOutput == null)
                        {
                            continue;
                        }

                        List<PhactoryHost.Database.Resource> resourceOutputs;
                        if (useCache)
                        {
                            resourceOutputs = App.Controller.PluginManager.Host.IsResourceReferencedByOtherResourcesCACHE(resourceOutput);
                        }
                        else
                        {
                            resourceOutputs = App.Controller.PluginManager.Host.IsResourceReferencedByOtherResources(resourceOutput);
                        }

                        if (resourceOutputs.Count > 0)
                        {
                            if (resourceOutput.FileExists == false)
                            {
                                resource.SkipNeedCompile = true;
                                resource.NeedCompile = true;
                                return true;
                            }
                            else if (resource.LastWriteTime > resourceOutput.LastWriteTime)
                            {
                                resource.SkipNeedCompile = true;
                                resource.NeedCompile = true;
                                return true;
                            }
                        }
                    }
                }
            }

            // check against REFERENCED outputs
            foreach (int outputId in resource.IdOutputs)
            {
                PhactoryHost.Database.Resource resourceOutput = App.Controller.Entities.GetResource(outputId);

                bool doCheck = false;
                if (App.Controller.Entities.StartupResourceId == outputId)
                {
                    doCheck = true;
                }
                else
                {
                    if (useCache)
                    {
                        resources = App.Controller.PluginManager.Host.IsResourceReferencedByOtherResourcesCACHE(resourceOutput);
                    }
                    else
                    {
                        resources = App.Controller.PluginManager.Host.IsResourceReferencedByOtherResources(resourceOutput);
                    }
                    if (resources.Count != 0)
                    {
                        doCheck = true;
                    }
                }

                if ( doCheck )
                {
                    if (resourceOutput.FileExists == false)
                    {
                        resource.SkipNeedCompile = true;
                        resource.NeedCompile = true;
                        return true;
                    }
                    else if (resource.LastWriteTime > resourceOutput.LastWriteTime)
                    {
                        resource.SkipNeedCompile = true;
                        resource.NeedCompile = true;
                        return true;
                    }
                }
            }

            // now check against dependencies
            foreach (int dependencyId in resource.IdDependencies)
            {
                PhactoryHost.Database.Resource resourceDependency = App.Controller.Entities.GetResource(dependencyId);
                if (DoesResourceNeedCompile(resourceDependency, useCache))
                {
                    resource.SkipNeedCompile = true;
                    resource.NeedCompile = true;
                    return true;
                }
                else if ( DoesIncludeResourceNeedCompile( resourceDependency, resource ) )
                {
                    resource.SkipNeedCompile = true;
                    resource.NeedCompile = true;
                    return true;
                }
            }

            if (resource.IdOutputs.Count > 0)
            {
                resource.SkipNeedCompile = true;
            }
            resource.NeedCompile = false;
            return false;
        }
    }
}
