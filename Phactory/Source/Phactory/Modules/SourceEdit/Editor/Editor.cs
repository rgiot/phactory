using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace SourceEdit.Editor
{
    public class ResourceBinding
    {
        public Panel Panel;
        public View.View View;

        public ResourceBinding()
        {
        }

        public ResourceBinding(Panel panel, View.View view)
        {
            this.Panel = panel;
            this.View = view;
        }
    }
    
    public class Editor : PhactoryHost.EditorPlugin
    {
        private Dictionary<PhactoryHost.Database.Resource, ResourceBinding> viewBinding = new Dictionary<PhactoryHost.Database.Resource, ResourceBinding>();
        private PhactoryHost.Database.Resource IgnoreNextResource = null;

        public PhactoryHost.Host Host;
        
        public Editor()
        {
            Plugin.ControllerEditor = this;
        }

        public bool IsDefaultPluginForUnknownTypes()
        {
            return false;
        }

        public string GetDefaultExtensionForNewResource()
        {
            return ".asm";
        }

        public bool ShowSettings(Panel parentPanel)
        {
            return false;
        }

        public bool SaveSettings()
        {
            return false;
        }

        public void Register(PhactoryHost.Host parent)
        {
            this.Host = parent;
        }

        public string GetName()
        {
            return "Source Editor";
        }

        public string GetDescription()
        {
            return "Used to edit source code";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".asm", "Assembly source file (*.asm)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".c", "C source file (*.c)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cxx", "C include-source file (*.cxx)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".h", "Include source file (*.h)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".txt", "Text file (*.txt)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".xml", "XML file (*.xml)", true));

            return extensions;
        }

        public bool CreateEmptyResource(PhactoryHost.Database.Resource resource)
        {
            File.CreateText(Host.GetFileInfo(resource).FullName).Close();

            return true;
        }

        public void OpenResource(Panel parentPanel, PhactoryHost.Database.Resource resource)
        {
            View.View view = new View.View(resource);
            view.Parent = parentPanel;
            view.Dock = System.Windows.Forms.DockStyle.Fill;

            viewBinding.Add(resource, new ResourceBinding(parentPanel, view));

            bool highLight = false;

            string language = "";
            switch (Host.GetFileInfo(resource).Extension.ToLower())
            {
                case ".s":
                case ".asm":
                case ".c":
                case ".h":
                case ".cxx":
                case ".cpp":
                    highLight = true;
                    break;                
                default:
                    highLight = false;
                    break;
            }
            if (language.Length != 0)
            {
                //view.textEditorControl.ConfigurationManager.Language = language;
            }

            if (highLight)
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(Application.StartupPath));
                view.textEditorControl.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("c_z80");
            }
            
            FileInfo fileInfo = Host.GetFileInfo(resource);
            string fileContent = File.ReadAllText(fileInfo.FullName); ;
            view.textEditorControl.Text = fileContent;

            if (Host.IsVerboseOutput())
            {
                Host.Log(resource.DisplayName + " loaded");
            }

            view.IsReady = true;

            if (resource.IsOutputResource)
            {
                view.SetReadOnly(true);
            }

            view.RefreshTitle();
        }

        public bool CloseResource(PhactoryHost.Database.Resource resource)
        {
            if (viewBinding.ContainsKey(resource))
            {
                if (IsResourceModified(resource))
                {
                    switch( MessageBox.Show("The file has been modified. Save changes ?", "Confirm file close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) )
                    {
                        case DialogResult.Yes:
                            SaveResource(resource);
                            break;
                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            return false;
                    }
                }

                viewBinding.Remove(resource);
            }

            return true;
        }

        public bool IsResourceOpened(PhactoryHost.Database.Resource resource)
        {
            try
            {
                View.View view = viewBinding[resource].View;
                if (view != null)
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }
        
        public bool IsResourceModified(PhactoryHost.Database.Resource resource)
        {
            View.View view = null;
            
            try
            {
                 view = viewBinding[resource].View;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }

            return view.IsModified();
        }

        public void RefreshViewTitle(PhactoryHost.Database.Resource resource)
        {
            View.View view = viewBinding[resource].View;

            view.RefreshTitle();
        }

        public void OnResourceDeleted(PhactoryHost.Database.Resource resource)
        {
        }

        public void OnResourceChanged(PhactoryHost.Database.Resource resource)
        {
        }

        public void OnResourceRenamed(PhactoryHost.Database.Resource resource)
        {
        }

        public void SaveResource(PhactoryHost.Database.Resource resource)
        {
            if (viewBinding.ContainsKey(resource))
            {
                View.View view = viewBinding[resource].View;

                IgnoreNextResource = resource;

                FileInfo fileInfo = Host.GetFileInfo(resource);
                view.WriteContentToFile(fileInfo.FullName);
            
                if (Host.IsVerboseOutput())
                {
                    Host.Log(resource.DisplayName + " written");
                }
                
                Host.RefreshOutput(resource);

                view.SetModified(false);
            }
        }

        public void SetLine(PhactoryHost.Database.Resource resource, int line)
        {
            if (viewBinding.ContainsKey(resource))
            {
                View.View view = viewBinding[resource].View;
                view.SetLine(line);
            }
        }
    
        public bool IsResourceSupported(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            foreach (PhactoryHost.PluginExtension extension in GetSupportedExtensions())
            {
                if (String.Compare(extension.GetName(), fileInfo.Extension, true) == 0)
                {
                    return true;
                }
            }

            return false;            
        }
    }
}
