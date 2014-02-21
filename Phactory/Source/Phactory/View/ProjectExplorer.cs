using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using PhactoryHost.Database;

namespace Project.View
{
    public partial class ProjectExplorer : DockContent
    {
        private Font itemFont = SystemFonts.DefaultFont;
        private Font boldFont = new Font(SystemFonts.DefaultFont, FontStyle.Underline);
        private Font projectFont = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
        private Font italicFont = new Font(SystemFonts.DefaultFont, FontStyle.Italic);
        
        public ProjectExplorer()
        {
            InitializeComponent();

            treeView.TreeViewNodeSorter = new TreeNodeSorter();
        }

        private TreeNode startupNode = null;
        private void SetTreeViewResource(TreeNode uiNode, PhactoryHost.Database.Resource resource)
        {
            App.Controller.View.AppDoEvents();

            uiNode.ContextMenuStrip = contextMenuStripResource;

            string extension = resource.GetFileExtension();
            string key;
            switch (extension)
            {
                case ".s": key = "FileASM"; break;
                case ".asm": key = "FileASM"; break;
                case ".cxx": key = "FileC"; break;
                case ".c": key = "FileC"; break;
                case ".h": key = "FileH"; break;
                case ".cpp": key = "FileCPP"; break;
                case ".txt": key = "FileTXT"; break;
                case ".cpcbigfile":
                case ".cpcbin2c":
                case ".cpcbitmap":
                case ".cpccloud":
                case ".cpcdsk":
                case ".cpcpack":
                case ".cpctext": 
                    key = "FileProp"; break;
                case ".bmp": key = "VSProject_imagefile"; break;
                case ".bin":
                case ".dsk":
                case ".raw":
                case ".packed":
                case ".bmp.packed":
                case ".bin.packed": key = "VSObject_Type"; break;
                default:
                    key = "FileGeneric"; break;
            }

            uiNode.ImageKey = key;
            uiNode.SelectedImageKey = key;

            if (App.Controller.Entities.StartupResourceId == resource.Id)
            {
                uiNode.NodeFont = boldFont;

                startupNode = uiNode;
            }
            else
            {
                if (App.Controller.Entities.IsOutput(resource))
                {
                    uiNode.ImageKey = "VSProject_generatedfile";
                    uiNode.SelectedImageKey = "VSProject_generatedfile";
                    
                    uiNode.NodeFont = italicFont;
                }
                else
                {
                    uiNode.NodeFont = itemFont;
                }
            }
        }

        private void AddTreeViewOutputResourceRecurse(TreeNode uiNode, PhactoryHost.Database.Resource resource)
        {
            App.Controller.View.AppDoEvents();

            foreach (int outputResourceId in resource.IdOutputs)
            {
                PhactoryHost.Database.Resource outputResource = App.Controller.Entities.GetResource(outputResourceId);

                if (App.Controller.UserConfig.HideUnusedResourcesInProjectExplorer)
                {
                    if (outputResource.Id != App.Controller.Entities.StartupResourceId)
                    {
                        if (App.Controller.PluginManager.Host.IsResourceReferencedByOtherResources(outputResource).Count == 0)
                        {
                            continue;
                        }
                    }
                }

                if (outputResource == null)
                {
                    continue;
                }

                TreeNode uiOutputNode = uiNode.Nodes.Add(outputResource.DisplayName);
                uiOutputNode.Tag = outputResource; // WARNING : not a node, but Resource !
                SetTreeViewResource(uiOutputNode, outputResource);

                AddTreeViewOutputResourceRecurse(uiOutputNode, outputResource);
            }
        }

        private void CreateTreeView(TreeNode uiRootNode, Node docRootNode )
        {
            if ( docRootNode.ChildNodes.Count == 0 )
            {
                return;
            }

            App.Controller.View.AppDoEvents();

            foreach (Node treeNode in docRootNode.ChildNodes)
            {
                TreeNode uiNode = null;
                
                if (treeNode.IsFolder)
                {
                    uiNode = uiRootNode.Nodes.Add(treeNode.FolderName);
                    uiNode.Tag = treeNode;
                    uiNode.NodeFont = itemFont;
                    uiNode.ImageKey = "VSFolder_closed";
                    uiNode.SelectedImageKey = uiNode.ImageKey;
                    //uiNode.StateImageKey = "VSFolder_open";
                    uiNode.ContextMenuStrip = contextMenuStripFolder;
                    CreateTreeView(uiNode, treeNode);
                }
                else
                {
                    Resource resource = App.Controller.Entities.GetResource(treeNode.ResourceId);
                    if (resource != null)
                    {
                        uiNode = uiRootNode.Nodes.Add(resource.DisplayName);
                        uiNode.Tag = treeNode;
                        SetTreeViewResource(uiNode, resource);

                        AddTreeViewOutputResourceRecurse(uiNode, App.Controller.Entities.GetResource(treeNode.ResourceId));
                    }
                }

                if ((uiNode != null) && treeNode.Expanded)
                {
                    uiNode.Expand();
                }   
            }  
        }

        public delegate void RefreshDBDelegate();
        public void RefreshDB()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new RefreshDBDelegate(RefreshDB));
                return;
            }

            this.treeView.AfterExpand -= new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand); 
            
            App.Controller.View.AppDoEvents();

            treeView.BeginUpdate();
            
            treeView.Nodes.Clear();

            TreeNode rootNode = treeView.Nodes.Add(App.Controller.Entities.ProjectName);
            rootNode.Tag = App.Controller.Entities.TreeNode;
            rootNode.NodeFont = projectFont;
            rootNode.ImageKey = "FileProject";
            rootNode.SelectedImageKey = "FileProject";
            rootNode.ContextMenuStrip = contextMenuStripProject;

            CreateTreeView(rootNode, App.Controller.Entities.TreeNode);
            
            if (App.Controller.Entities.TreeNode.Expanded)
            {
                rootNode.Expand();
            }

            App.Controller.View.AppDoEvents();

            treeView.Sort();
            treeView.EndUpdate();

            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
        }

        private TreeNode GetTreeNode(TreeNode rootNode, PhactoryHost.Database.Resource resource)
        {
            Node docNode = rootNode.Tag as Node;
            if (docNode != null)
            {
                if (docNode.ResourceId == resource.Id)
                {
                    return rootNode;
                }
            }

            foreach (TreeNode childNode in rootNode.Nodes)
            {
                TreeNode outNode = GetTreeNode(childNode, resource);
                if (outNode != null)
                {
                    return outNode;
                }
            }

            return null;
        }

        private TreeNode GetDocTreeNode(TreeNode rootNode, Node docNode)
        {
            if ((rootNode.Tag as Node )==docNode)
            {
                return rootNode;
            }
            
            foreach (TreeNode childNode in rootNode.Nodes)
            {
                TreeNode outNode = GetDocTreeNode(childNode, docNode);
                if (outNode != null)
                {
                    return outNode;
                }
            }

            return null;
        }

        public void RefreshNodeOutput(PhactoryHost.Database.Resource resource)
        {
            App.Controller.View.AppDoEvents();

            TreeNode treeNode = GetTreeNode(treeView.Nodes[0], resource);
            if (treeNode != null)
            {
                treeNode.Nodes.Clear();
                AddTreeViewOutputResourceRecurse(treeNode, resource);

                treeView.BeginUpdate();
                treeView.Sort();
                treeView.EndUpdate();
            }
        }

        private void SelectResourceRecursive(TreeNode uiNode, PhactoryHost.Database.Resource resource)
        {
            foreach (TreeNode node in uiNode.Nodes)
            {
                if (node.Text == resource.DisplayName)
                {
                    treeView.SelectedNode = node;
                }
                else
                {
                    SelectResourceRecursive(node, resource);
                }
            }
        }
        public void SelectResource(Resource resource)
        {
            RefreshDB();

            TreeNodeCollection nodes = this.treeView.Nodes;
            foreach (TreeNode node in nodes)
            {
                SelectResourceRecursive(node, resource);
            }
        }
        
        private void addNewResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileLocation = App.Controller.UserConfig.ResourcePath;

            NewFile newFile = new NewFile();
            newFile.SetFileLocation(fileLocation);
            newFile.ShowDialog(this);
            if (newFile.Valid)
            {
                if (File.Exists(newFile.GetFilename()))
                {
                    App.Controller.View.ShowErrorMessage("Error", "'" + newFile.GetDisplayName() + "' already exists locally, choose another name.");
                    return;
                }

                App.Controller.UserConfig.NewFilePath = newFile.GetFilename();

                string relativePath = Helper.StringHelper.MakeRelativePath(App.Controller.UserConfig.NewFilePath);

                PhactoryHost.Database.Resource resource = new PhactoryHost.Database.Resource();
                resource.Id = App.Controller.Entities.CreateNewResourceId();
                resource.DisplayName = newFile.GetDisplayName();
                resource.RelativePath = relativePath;
                App.Controller.Entities.AddResource(resource);

                App.Controller.CreateNewResource(resource);

                Node childTreeNode = new Node();
                childTreeNode.IsFolder = false;
                childTreeNode.ResourceId = resource.Id;

                Node treeNode = TreeViewSelectedNode.Tag as Node;
                treeNode.ChildNodes.Add(childTreeNode);

                App.Controller.SaveProject();

                TreeViewSelectedNode.Nodes.Clear();
                CreateTreeView(TreeViewSelectedNode, treeNode);

                treeView.BeginUpdate();
                treeView.Sort();
                treeView.EndUpdate();

                TreeNode createdNode = GetDocTreeNode(TreeViewSelectedNode, childTreeNode);
                treeView.SelectedNode = createdNode;
                TreeViewSelectedNode = createdNode;
            }
        }

        private void addNewFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int filterNumber = 0;
            foreach (TreeNode iTreeNode in TreeViewSelectedNode.Nodes)
            {
                Node docNode = iTreeNode.Tag as Node;
                if ((docNode.IsFolder) && (docNode.FolderName.Contains("New Filter")))
                {
                    filterNumber++;
                }
            } 
            
            Node treeNode = new Node();
            treeNode.IsFolder = true;
            treeNode.FolderName = "New Filter" + (filterNumber + 1);

            Node selectedTreeNode = TreeViewSelectedNode.Tag as Node;
            selectedTreeNode.ChildNodes.Add(treeNode);

            TreeViewSelectedNode.Nodes.Clear();
            CreateTreeView(TreeViewSelectedNode, selectedTreeNode);

            TreeNode createdNode = GetDocTreeNode(TreeViewSelectedNode, treeNode);
            treeView.SelectedNode = createdNode;
            TreeViewSelectedNode = createdNode;

            createdNode.BeginEdit();
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
            {
                return;
            }
            
            if (e.Label.Length == 0)
            {
                e.CancelEdit = true;
                return;
            }

            if (e.Node == treeView.Nodes[0])
            {
                App.Controller.Entities.ProjectName = e.Label;
                App.Controller.View.RefreshApplicationTitle();
            }
            else
            {
                Node node = e.Node.Tag as Node;
                if (node.IsFolder)
                {
                    node.FolderName = e.Label;
                }
                else
                {
                    App.Controller.Build.Clean(App.Controller.Entities.GetResource(node.ResourceId), false);

                    App.Controller.Entities.GetResource(node.ResourceId).DisplayName = e.Label;

                    string location = Helper.StringHelper.MakeFullPath(App.Controller.Entities.GetResource(node.ResourceId).RelativePath);
                    FileInfo fileInfo = new FileInfo(location);
                    string source = fileInfo.DirectoryName + "\\" + e.Node.Text;
                    string destination = fileInfo.DirectoryName + "\\" + e.Label;

                    if (source.ToLower() == destination.ToLower())
                    {
                        File.Copy(source, destination + ".TMP");
                        File.SetAttributes(source, FileAttributes.Normal);
                        File.Delete(source);
                        File.Copy(destination + ".TMP", destination);
                        File.Delete(destination + ".TMP");
                    }
                    else
                    {
                        File.Copy(source, destination);
                        File.SetAttributes(source, FileAttributes.Normal);
                        File.Delete(source);
                    }

                    App.Controller.Entities.GetResource(node.ResourceId).RelativePath = Helper.StringHelper.MakeRelativePath(destination);

                    Helper.DBHelper.RenameResource(App.Controller.Entities.GetResource(node.ResourceId), e.Node.Text, e.Label);
                    this.RefreshNodeOutput(App.Controller.Entities.GetResource(node.ResourceId));
                }
            }

            App.Controller.SaveProject();

            e.Node.Text = e.Label;

            this.treeView.SelectedNode = e.Node;
        }

        private void Rename()
        {
            TreeViewSelectedNode.BeginEdit();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rename();
        }
        private TreeNode TreeViewSelectedNode = null;
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewSelectedNode = e.Node;

            if (IsFolderNode(e.Node))
            {
                App.Controller.View.SetResource(null);
            }
            else
            {
                App.Controller.View.SetResource(GetNodeResource(e.Node));
            }
        }

        PhactoryHost.Database.Resource GetNodeResource( TreeNode node )
        {
            if ( node.Tag is PhactoryHost.Database.Resource )
            {
                return node.Tag as PhactoryHost.Database.Resource;
            }

            Node docNode = node.Tag as Node;
            return App.Controller.Entities.GetResource(docNode.ResourceId);
        }

        bool IsFolderNode(TreeNode node)
        {
            if (node.Tag is PhactoryHost.Database.Resource)
            {
                return false;
            }

            Node docNode = node.Tag as Node;
            return docNode.IsFolder;
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            App.Controller.Build.Compile(GetNodeResource(TreeViewSelectedNode), false);
        }

        private void cleanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            App.Controller.Build.Clean(GetNodeResource(TreeViewSelectedNode), false);
        }

        private void cleanCompileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            App.Controller.Build.CleanCompile(GetNodeResource(TreeViewSelectedNode));
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            App.Controller.Build.Run(GetNodeResource(TreeViewSelectedNode));
        }

        private void setAsProjectStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            App.Controller.Entities.StartupResourceId = GetNodeResource(TreeViewSelectedNode).Id;
            App.Controller.SaveProject();

            if (startupNode != null)
            {
                SetTreeViewResource(startupNode, GetNodeResource(startupNode));
            }
            
            SetTreeViewResource(TreeViewSelectedNode, App.Controller.Entities.GetResource(App.Controller.Entities.StartupResourceId));
        }

        private void OpenSelectedResource()
        {
            if (TreeViewSelectedNode == treeView.Nodes[0])
            {
                return;
            }

            if (IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            App.Controller.OpenResource(GetNodeResource(TreeViewSelectedNode));
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSelectedResource();
        }

        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeViewSelectedNode.Expand();

            OpenSelectedResource();
        }

        private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Rename();
        }

        private void copyFullPathToClipboard_Click(object sender, EventArgs e)
        {
            if (this.IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            Resource resource = GetNodeResource(TreeViewSelectedNode);

            FileInfo resourceFileInfo = Helper.StringHelper.GetFileInfo(resource);
            if (resourceFileInfo.Exists)
            {
                Clipboard.SetText(resourceFileInfo.FullName);
            }
            else
            {
                Clipboard.SetText(resourceFileInfo.DirectoryName);
            }
        }

        private void copyContainingFolderToClipboard_Click(object sender, EventArgs e)
        {
            if (this.IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            Resource resource = GetNodeResource(TreeViewSelectedNode);

            FileInfo resourceFileInfo = Helper.StringHelper.GetFileInfo(resource);
            if (resourceFileInfo.Exists)
            {
                Clipboard.SetText(resourceFileInfo.DirectoryName);
            }
        }

        private void openContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            Resource resource = GetNodeResource(TreeViewSelectedNode);

            FileInfo resourceFileInfo = Helper.StringHelper.GetFileInfo(resource);
            if (resourceFileInfo.Exists)
            {
                App.Controller.Explore(resourceFileInfo.FullName);
            }
            else
            {
                App.Controller.Explore(resourceFileInfo.DirectoryName);
            }
        }

        private void openWithDefaultApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            Resource resource = GetNodeResource(TreeViewSelectedNode);

            string filename = Helper.StringHelper.MakeFullPath(resource.RelativePath);
            System.Diagnostics.Process.Start(filename);
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            Resource resource = GetNodeResource(TreeViewSelectedNode);
            if ( resource == null )
            {
                return;
            }

            if (!Helper.DBHelper.CheckIfResourceCanBeRemoved(resource))
            {
                return;
            }

            RemoveOrDelete removeOrDeleteDialog = new RemoveOrDelete(false, resource.DisplayName);
            removeOrDeleteDialog.ShowDialog(this);
            if (removeOrDeleteDialog.Valid == false)
            {
                return;
            }

            Helper.DBHelper.RemoveResourceWithOutput(removeOrDeleteDialog.IsDelete, resource);

            Node currentTreeNode = TreeViewSelectedNode.Tag as Node; 
            Node treeNode = TreeViewSelectedNode.Parent.Tag as Node;
            treeNode.ChildNodes.Remove(currentTreeNode);

            TreeViewSelectedNode.Parent.Nodes.Remove(TreeViewSelectedNode);

            App.Controller.SaveProject();
        }

        private void addExistingResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            OpenFileDialog openFile = new OpenFileDialog();

            string fileLocation = App.Controller.UserConfig.AddExistingResourceFullPath;
            
            string filterString = "";

            filterString = "All resources (*.*)|*.*";

            List<PhactoryHost.EditorPlugin> pluginEditors = App.Controller.PluginManager.GetPluginEditors();
            foreach (PhactoryHost.EditorPlugin pluginEditor in pluginEditors)
            {
                foreach (PhactoryHost.PluginExtension pluginExtension in pluginEditor.GetSupportedExtensions())
                {
                    filterString += "|" + pluginExtension.GetDescription() + "|" + "*" + pluginExtension.GetName();
                }
            }

            openFile.InitialDirectory = fileLocation;
            openFile.Filter = filterString;
            openFile.FilterIndex = 1;
            openFile.Multiselect = true;
            openFile.RestoreDirectory = true;
            openFile.Title = "Add existing resource";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                TreeNode createdNode = null;
                int createdNodeCount = 0;

                List<Resource> addedResources = new List<Resource>();

                App.Controller.Log.Append("Adding existing resources..");
                
                foreach (String srcFilename in openFile.FileNames)
                {
                    FileInfo srcFileInfo = new FileInfo(srcFilename);

                    string destFilename = App.Controller.UserConfig.ResourcePath + srcFileInfo.Name;

                    App.Controller.UserConfig.AddExistingResourceFullPath = srcFileInfo.Directory.FullName;

                    if (Path.GetFullPath(srcFileInfo.FullName) != Path.GetFullPath(destFilename))
                    {
                        bool addResource = true;
                        
                        if (File.Exists(destFilename))
                        {
                            if (MessageBox.Show(destFilename + " already exists !\n\nReplace content?", "Resource exists !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                File.Delete(destFilename);
                            }
                            else
                            {
                                addResource = false;
                            }
                        }
                        
                        if (addResource)
                        {
                            string relativePath = Helper.StringHelper.MakeRelativePath(destFilename);

                            PhactoryHost.Database.Resource resource = new PhactoryHost.Database.Resource();
                            resource.Id = App.Controller.Entities.CreateNewResourceId();
                            resource.DisplayName = new FileInfo(destFilename).Name;
                            resource.RelativePath = relativePath;
                            App.Controller.Entities.AddResource(resource);

                            App.Controller.CreateNewResource(resource);

                            File.Delete(destFilename);
                            File.Copy(srcFileInfo.FullName, destFilename);

                            Node childTreeNode = new Node();
                            childTreeNode.IsFolder = false;
                            childTreeNode.ResourceId = resource.Id;

                            Node treeNode = TreeViewSelectedNode.Tag as Node;
                            treeNode.ChildNodes.Add(childTreeNode);

                            App.Controller.SaveProject();

                            TreeViewSelectedNode.Nodes.Clear();
                            CreateTreeView(TreeViewSelectedNode, treeNode);

                            createdNode = GetDocTreeNode(TreeViewSelectedNode, childTreeNode);
                            createdNodeCount++;
                            
                            addedResources.Add(resource);
                            App.Controller.InsertFileMostRecentUsed(destFilename);
                        }
                    }
                }

                if (createdNodeCount == 1)
                {
                    treeView.SelectedNode = createdNode;
                    TreeViewSelectedNode = createdNode;
                }
                else
                {
                    treeView.SelectedNode.Expand();
                }

                App.Controller.Log.Append("Refreshing dependencies.."); 
                foreach (Resource addedResource in addedResources)
                {
                    App.Controller.Build.UpdateDependenciesRecursive(addedResource);
                }

                App.Controller.Log.Append("" + addedResources.Count + " resources imported");
            }
        }

        private void removeFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFolderNode(TreeViewSelectedNode))
            {
                return;
            }

            if (TreeViewSelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show("Can't delete folder because it's not empty !", "Can't delete folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Node currentTreeNode = TreeViewSelectedNode.Tag as Node;
            Node treeNode = TreeViewSelectedNode.Parent.Tag as Node;
            treeNode.ChildNodes.Remove(currentTreeNode);

            TreeViewSelectedNode.Parent.Nodes.Remove(TreeViewSelectedNode);

            App.Controller.SaveProject();
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true))
            {
                Point pt = this.treeView.PointToClient(new Point(e.X, e.Y));

                TreeNode node = this.treeView.GetNodeAt(pt);
                Node docNode = node.Tag as Node;

                if (docNode == null)
                {
                    return;
                }

                if (!docNode.IsFolder)
                {
                    PhactoryHost.Database.Resource resource = App.Controller.Entities.GetResource(docNode.ResourceId);
                    if (App.Controller.Entities.IsOutput(resource))
                    {
                        return;
                    }
                }

                e.Effect = DragDropEffects.Move;
            }
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true))
            {
                Point pt = this.treeView.PointToClient(new Point(e.X, e.Y));
                TreeNode destNode = this.treeView.GetNodeAt(pt);

                if (destNode != treeView.TopNode)
                {
                    if (!IsFolderNode(destNode))
                    {
                        return;
                    }            
                }

                TreeNode sourceNode = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode");
                
                if ( !(destNode == sourceNode) )
                {
                    Node docDestNode = destNode.Tag as Node;                    
                    Node docSourceNode = sourceNode.Tag as Node;
                    Node docSourceParentNode = sourceNode.Parent.Tag as Node;

                    docDestNode.ChildNodes.Add(docSourceNode);
                    docSourceParentNode.ChildNodes.Remove( docSourceNode );
                    
                    destNode.Nodes.Add( (TreeNode) sourceNode.Clone() );
                    sourceNode.Remove();

                    destNode.Expand();
                    
                    App.Controller.SaveProject();
                }
            }
        }

        private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == TreeViewSelectedNode)
            {
                if (IsFolderNode(e.Node) == false)
                {
                    Resource resource = GetNodeResource(e.Node);
                    if (resource != null)
                    {
                        if (resource.IsOutputResource)
                        {
                            App.Controller.View.ShowWarningMessage("Can't rename " + resource.DisplayName, "Output resources can not be renamed !");
                        }
                        else if (App.Controller.PluginManager.IsResourceOpened(resource))
                        {
                            App.Controller.View.ShowWarningMessage("Can't rename " + resource.DisplayName, "The resource is currently being edited.\n\nPlease close the opened editor before renaming.");
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            e.CancelEdit = true;
        }

        private void SetExpandRecurse(Node node, bool expandedState)
        {
            foreach (var childNode in node.ChildNodes)
            {
                SetExpandRecurse(childNode, expandedState);
            }

            node.Expanded = expandedState;
        }

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            Node treeNode = e.Node.Tag as Node;
            if ( treeNode != null )
            {
                SetExpandRecurse(treeNode, false);
            }

            App.Controller.SaveProject();
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            Node treeNode = e.Node.Tag as Node;
            if (treeNode != null)
            {
                treeNode.Expanded = true;
            }

            App.Controller.SaveProject();
        }

        private void ExpandAllSelectedNodes()
        {
            TreeViewSelectedNode.ExpandAll();
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandAllSelectedNodes();
        }

        private void expandAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExpandAllSelectedNodes();
        }

        private void expandAllToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ExpandAllSelectedNodes();
        }

        private void exportRecuseAllResourceBitmaps(TreeNode treeNode, string path)
        {
            var node = treeNode.Tag as Node;

            if (node.IsFolder)
            {
                var newFolder = path + node.FolderName + "\\";
                Directory.CreateDirectory(newFolder);

                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    exportRecuseAllResourceBitmaps(childNode, newFolder);
                }
            }
            else
            {
                var resource = App.Controller.Entities.GetResource(node.ResourceId);
                if ( resource.IsBitmapResource() )
                {
                    if (!File.Exists(resource.FileInfo.FullName))
                    {
                        App.Controller.Log.Append(resource.DisplayName + " does not exist !"); 
                    }
                    else
                    {
                        File.Copy(resource.FileInfo.FullName, path + resource.FileInfo.Name);
                    }
                }
            }
        }

        private void DeleteEmptyDirectories(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                DeleteEmptyDirectories(directory);
                if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private void exportAllBitmapResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = TreeViewSelectedNode.Tag as Node;
            if (!node.IsFolder)
            {
                MessageBox.Show("Selected resource is not a folder !", "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            folderBrowserDialog.Description = "Select a folder to export all Bitmap Resource(s) in...";
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var rootPath = folderBrowserDialog.SelectedPath;

                exportRecuseAllResourceBitmaps(TreeViewSelectedNode, rootPath + "\\");

                DeleteEmptyDirectories(rootPath);

                MessageBox.Show("All Bitmap Resource(s) have been exported to\n\n" + rootPath, "Export done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void projectPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.Controller.ShowProjectProperties();
        }
    }

    public class TreeNodeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            Node docNode1 = tx.Tag as Node;
            Node docNode2 = ty.Tag as Node;
            if (((docNode1 != null) && docNode1.IsFolder) && ((docNode2 != null) && !docNode2.IsFolder))
            {
                return -1;
            }
            else if (((docNode1 != null) && !docNode1.IsFolder) && ((docNode2 != null) && docNode2.IsFolder))
            {
                return 1;
            }

            return string.Compare( tx.Text.ToLower(), ty.Text.ToLower() );
        }
    }
}
