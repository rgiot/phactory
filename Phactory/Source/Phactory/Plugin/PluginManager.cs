using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;

namespace Project.Plugin
{
    public class PluginManager
    {
        private PhactoryHost.Host host;
        public PhactoryHost.Host Host
        {
            get { return host; }
        }

        private List<PhactoryHost.Plugin> plugins = new List<PhactoryHost.Plugin>();
        
        public PluginManager()
        {
            host = new Plugin.Host();
        }

        private void LogPluginInfo(PhactoryHost.Plugin plugin)
        {
            PhactoryHost.ToolPlugin tool = plugin as PhactoryHost.ToolPlugin;
            PhactoryHost.EditorPlugin editor = plugin as PhactoryHost.EditorPlugin;
            PhactoryHost.CompilerPlugin compiler = plugin as PhactoryHost.CompilerPlugin;
            PhactoryHost.RunnerPlugin runner = plugin as PhactoryHost.RunnerPlugin;

            if (tool != null)
            {
                App.Controller.Log.Append(plugin.GetName() + " " + plugin.GetVersion() + " (tool)");
            }
            else
            if (editor != null)
            {
                string extensions = "";

                foreach (PhactoryHost.PluginExtension supportedExtension in editor.GetSupportedExtensions())
                {
                    if (extensions.Length > 0)
                    {
                        extensions += ", ";
                    }
                    extensions += "*" + supportedExtension.GetName();
                }

                string text = plugin.GetName() + " " + plugin.GetVersion() + " (editor for " + extensions + ")";

                if (editor.IsDefaultPluginForUnknownTypes())
                {
                    text += " (also used as default editor for unknown types)";
                }

                App.Controller.Log.Append(text);
            }
            else if (compiler != null)
            {
                string extensions = "";

                foreach (PhactoryHost.PluginExtension supportedExtension in compiler.GetSupportedExtensions())
                {
                    if (extensions.Length > 0)
                    {
                        extensions += ", ";
                    }
                    extensions += "*" + supportedExtension.GetName();
                }

                App.Controller.Log.Append(plugin.GetName() + " " + plugin.GetVersion() + " (compiler for " + extensions + ")");
            }
            else if (runner != null)
            {
                string extensions = "";

                foreach (PhactoryHost.PluginExtension supportedExtension in runner.GetSupportedExtensions())
                {
                    if (extensions.Length > 0)
                    {
                        extensions += ", ";
                    }
                    extensions += "*" + supportedExtension.GetName();
                }

                App.Controller.Log.Append(plugin.GetName() + " " + plugin.GetVersion() + " (runner for " + extensions + ")");
            }
        }

        private void RegisterPlugins()
        {
            PhactoryHost.Plugin plugin = null; 
            
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && (type.GetInterface("PhactoryHost.Plugin") != null))
                {
                    plugin = Activator.CreateInstance(type) as PhactoryHost.Plugin;

                    if (App.Controller.UserConfig.VerboseOutput)
                    {
                        LogPluginInfo(plugin);
                    }

                    plugins.Add(plugin);
                    plugin.Register(host);
                }
            }
        }

        private void RegisterPlugin(FileInfo assemblyFileInfo)
        {
            PhactoryHost.Plugin plugin = null;

            System.Reflection.Assembly assembly;
            try
            {
                assembly = System.Reflection.Assembly.LoadFrom(assemblyFileInfo.FullName);
            }
            catch
            {
                return;
            }

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && (type.GetInterface("PhactoryHost.Plugin") != null))
                {
                    plugin = Activator.CreateInstance(type) as PhactoryHost.Plugin;

                    if (App.Controller.UserConfig.VerboseOutput)
                    {
                        LogPluginInfo(plugin);
                    }

                    plugins.Add(plugin);
                    plugin.Register(host);
                }
            }
        }

        public class InitWorker
        {
            public InitWorker()
            {            
            }

            public void DoWork()
            {
                /*string pluginPath = App.Controller.Config.ApplicationPath + App.Controller.Config.PluginPath;
                List<FileInfo> fileInfos = Helper.FileDB.GetFiles(pluginPath, "*.dll", true);

                foreach (FileInfo fileInfo in fileInfos)
                {
                    App.Controller.PluginManager.RegisterPlugin(fileInfo);
                }*/

                App.Controller.PluginManager.RegisterPlugins();
            }
        }

        public void Init()
        {
            Thread t = new Thread(new InitWorker().DoWork);
            t.Start();
            while (t.IsAlive)
            {
                App.Controller.View.AppDoEvents();
            }

            App.Controller.View.RefreshPluginTools();

            App.Controller.Log.Append(""+App.Controller.PluginManager.GetPlugins().Count + " modules registered");
        }

        public List<PhactoryHost.Plugin> GetPlugins()
        {
            return plugins;
        }

        public List<PhactoryHost.ToolPlugin> GetPluginTools()
        {
            List<PhactoryHost.ToolPlugin> pluginTools = new List<PhactoryHost.ToolPlugin>();

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                PhactoryHost.ToolPlugin pluginTool = plugin as PhactoryHost.ToolPlugin;
                if ( pluginTool != null )
                {
                    pluginTools.Add(pluginTool);
                }
            }

            return pluginTools;
        }

        public List<PhactoryHost.EditorPlugin> GetPluginEditors()
        {
            List<PhactoryHost.EditorPlugin> pluginEditors = new List<PhactoryHost.EditorPlugin>();

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                PhactoryHost.EditorPlugin pluginEditor = plugin as PhactoryHost.EditorPlugin;
                if (pluginEditor != null)
                {
                    pluginEditors.Add(pluginEditor);
                }
            }

            return pluginEditors;
        }

        public List<PhactoryHost.CompilerPlugin> GetPluginCompilers()
        {
            List<PhactoryHost.CompilerPlugin> pluginCompilers = new List<PhactoryHost.CompilerPlugin>();

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                PhactoryHost.CompilerPlugin pluginCompiler = plugin as PhactoryHost.CompilerPlugin;
                if (pluginCompiler != null)
                {
                    pluginCompilers.Add(pluginCompiler);
                }
            }

            return pluginCompilers;
        }

        public List<PhactoryHost.RunnerPlugin> GetPluginRunners()
        {
            List<PhactoryHost.RunnerPlugin> runnerCompilers = new List<PhactoryHost.RunnerPlugin>();

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                PhactoryHost.RunnerPlugin pluginRunner = plugin as PhactoryHost.RunnerPlugin;
                if (pluginRunner != null)
                {
                    runnerCompilers.Add(pluginRunner);
                }
            }

            return runnerCompilers;
        }

        public string GetDefaultExtensionForNewResource()
        {
            PhactoryHost.EditorPlugin editor = null;

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                editor = plugin as PhactoryHost.EditorPlugin;
                if (editor != null)
                {
                    string extension = editor.GetDefaultExtensionForNewResource();

                    if (extension != String.Empty)
                    {
                        return extension;
                    }
                }
            }

            return String.Empty;
        }
        
        public PhactoryHost.EditorPlugin GetDefaultEditorForUnknownTypes()
        {
            PhactoryHost.EditorPlugin editor = null;

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                editor = plugin as PhactoryHost.EditorPlugin;
                if (editor != null)
                {
                    if (editor.IsDefaultPluginForUnknownTypes())
                    {
                        return editor;
                    }
                }
            }

            return null;
        }

        public PhactoryHost.EditorPlugin GetEditor(string extension)
        {
            PhactoryHost.EditorPlugin editor = null;
            
            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                editor = plugin as PhactoryHost.EditorPlugin;
                if (editor != null)
                {
                    foreach (PhactoryHost.PluginExtension supportedExtension in editor.GetSupportedExtensions())
                    {
                        if (supportedExtension.GetName() == extension)
                        {
                            return editor;
                        }
                    }
                }
            }

            return null;
        }

        public PhactoryHost.CompilerPlugin GetCompiler(string extension)
        {
            PhactoryHost.CompilerPlugin compiler = null;

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                compiler = plugin as PhactoryHost.CompilerPlugin;
                if (compiler != null)
                {
                    foreach (PhactoryHost.PluginExtension supportedExtension in compiler.GetSupportedExtensions())
                    {
                        if (supportedExtension.GetName() == extension)
                        {
                            return compiler;
                        }
                    }
                }
            }

            return null;
        }

        public PhactoryHost.RunnerPlugin GetRunner(string extension)
        {
            PhactoryHost.RunnerPlugin runner = null;

            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                runner = plugin as PhactoryHost.RunnerPlugin;
                if (runner != null)
                {
                    foreach (PhactoryHost.PluginExtension supportedExtension in runner.GetSupportedExtensions())
                    {
                        if (supportedExtension.GetName() == extension)
                        {
                            return runner;
                        }
                    }
                }
            }

            return null;
        }

        public PhactoryHost.Plugin GetPluginByName(string pluginName)
        {
            foreach (PhactoryHost.Plugin plugin in plugins)
            {
                if (pluginName == plugin.GetName())
                {
                    return plugin;
                }
            }

            return null;
        }

        public void OnResourceDeleted(View.PluginView pluginView, PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            PhactoryHost.EditorPlugin editor = GetEditor(fileInfo.Extension);
            
            if ( (editor != null) && (pluginView != null) )
            {
                editor.OnResourceDeleted(resource);
            }
        }

        public void OnResourceChanged(View.PluginView pluginView, PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            PhactoryHost.EditorPlugin editor = GetEditor(fileInfo.Extension);

            if ((editor != null) && (pluginView != null))
            {
                editor.OnResourceChanged(resource);
            }
        }

        public void OnResourceRenamed(View.PluginView pluginView, PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            PhactoryHost.EditorPlugin editor = GetEditor(fileInfo.Extension);

            if ((editor != null) && (pluginView != null))
            {
                editor.OnResourceRenamed(resource);
            }
        }

        public void RefreshViewTitle(View.PluginView pluginView, PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            PhactoryHost.EditorPlugin editor = GetEditor(fileInfo.Extension);
            
            if ( (editor != null) && (pluginView != null) )
            {
                editor.RefreshViewTitle(resource);
            }
        }

        public void OpenResource(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            PhactoryHost.EditorPlugin editor = GetEditor(fileInfo.Extension);

            if (!App.Controller.UserConfig.OpenedResources.Contains(resource.Id))
            {
                App.Controller.UserConfig.OpenedResources.Add(resource.Id);
                App.Controller.WriteUserConfig();
            } 
            
            if (editor != null)
            {
                View.PluginView pluginView = App.Controller.View.CreatePluginView(resource, false);
                editor.OpenResource(pluginView.PluginContainer, resource);
            }
            else
            {
                PhactoryHost.EditorPlugin defaultEditor = GetDefaultEditorForUnknownTypes();
                if (defaultEditor != null)
                {
                    View.PluginView pluginView = App.Controller.View.CreatePluginView(resource, false);
                    defaultEditor.OpenResource(pluginView.PluginContainer, resource);
                }
            }
        }

        public bool CloseResource(PhactoryHost.Database.Resource resource)
        {
            string extension = Helper.StringHelper.GetFileExtension(resource.RelativePath);

            PhactoryHost.EditorPlugin editor = GetEditor(extension);

            bool closed = false;
            
            if (editor != null)
            {
                closed = editor.CloseResource(resource);
            }
            else
            {
                PhactoryHost.EditorPlugin defaultEditor = GetDefaultEditorForUnknownTypes();
                if (defaultEditor != null)
                {
                    if (defaultEditor.IsResourceOpened(resource))
                    {
                        closed = defaultEditor.CloseResource(resource);
                    }
                }
            }

            if (closed)
            {
                if (!App.Controller.IsAppClosing)
                {
                    if (App.Controller.UserConfig.OpenedResources.Contains(resource.Id))
                    {
                        App.Controller.UserConfig.OpenedResources.Remove(resource.Id);
                        App.Controller.WriteUserConfig();
                    }
                }
            }

            return closed;
        }

        public bool IsResourceOpened(PhactoryHost.Database.Resource resource)
        {
            string extension = Helper.StringHelper.GetFileExtension(resource.RelativePath);

            PhactoryHost.EditorPlugin editor = GetEditor(extension);
            if (editor != null)
            {
                if (editor.IsResourceOpened(resource))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsResourceModified(PhactoryHost.Database.Resource resource)
        {
            string extension = Helper.StringHelper.GetFileExtension(resource.RelativePath);

            PhactoryHost.EditorPlugin editor = GetEditor(extension);
            if (editor != null)
            {
                if (editor.IsResourceModified(resource))
                {
                    return true;
                }
            }

            return false;
        }

        public void SaveResource(PhactoryHost.Database.Resource resource)
        {
            string extension = Helper.StringHelper.GetFileExtension(resource.RelativePath);

            PhactoryHost.EditorPlugin editor = GetEditor(extension);
            editor.SaveResource(resource);

            App.Controller.Build.UpdateDependenciesRecursive(resource);
            App.Controller.View.RefreshDB();
        }
    }
}
