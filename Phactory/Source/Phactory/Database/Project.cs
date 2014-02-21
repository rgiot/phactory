using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace PhactoryHost.Database
{
    [Serializable]
    public class GlobalSettings
    {
        [Category("FullScreen Bitmaps")]
        [Description("CPC Address screen pointer to Top Screen")]
        public ushort Screen1_TopScreenPtr { get; set; }

        [Category("FullScreen Bitmaps")]
        [Description("CPC Address screen pointer to Bottom Screen")]
        public ushort Screen1_BottomScreenPtr { get; set; }

        [CategoryAttribute("FullScreen Bitmaps")]
        [Description("Height in Characters of Top part")]
        public int Screen1_TopHeightInChars { get; set; }

        [CategoryAttribute("FullScreen Bitmaps")]
        [Description("Height in Characters of Bottom part")]
        public int Screen1_BottomHeightInChars { get; set; }

        [CategoryAttribute("CRTC")]
        [Description("Register 1")]
        public int CRTC_Register1 { get; set; }

        [CategoryAttribute("CRTC")]
        [Description("Register 6")]
        public int CRTC_Register6 { get; set; }

        [CategoryAttribute("CRTC")]
        [Description("Register 9")]
        public int CRTC_Register9 { get; set; }

        public GlobalSettings()
        {
            Screen1_TopScreenPtr = 0x0080;
            Screen1_BottomScreenPtr= 0x4000;
            Screen1_TopHeightInChars = 20;
            Screen1_BottomHeightInChars = 21;
            CRTC_Register1 = 48;
            CRTC_Register6 = 41;
            CRTC_Register9 = 4;
        }
    }

    [Serializable]
    public class Project
    {
        public string Version;
        public string ProjectName = "";
        public int StartupResourceId = 0;
        public List<Resource> Resources = new List<Resource>();
        public Node TreeNode = new Node();
        public int ResourceId = 0;

        public GlobalSettings GlobalSettings = new GlobalSettings();
        
        public Project()
        {
            Version = GetVersion();
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public Node GetTreeNode(Node rootNode, Resource resource)
        {
            if ( !rootNode.IsFolder )
            {
                if (rootNode.ResourceId == resource.Id)
                {
                    return rootNode;
                }
            }

            foreach (Node childNode in rootNode.ChildNodes)
            {
                Node outNode = GetTreeNode(childNode, resource);
                if (outNode != null)
                {
                    return outNode;
                }
            }

            return null;
        }

        public Node GetParentFolderTreeNode(Node rootNode, ref Node folderNode, Resource resource)
        {
            if (rootNode.IsFolder)
            {
                folderNode = rootNode;
            }
            else if (rootNode.ResourceId == resource.Id)
            {
                return rootNode;
            }

            foreach (Node childNode in rootNode.ChildNodes)
            {
                Node outNode = GetParentFolderTreeNode(childNode, ref folderNode, resource);
                if (outNode != null)
                {
                    return outNode;
                }
            }

            return null;
        }

        public Resource GetResource(int id)
        {
            foreach (Resource iResource in Resources)
            {
                if (iResource.Id == id)
                {
                    return iResource;
                }
            }
            return null;
        }

        public bool DeleteResource(Resource resource)
        {
            foreach (Resource iResource in Resources)
            {
                iResource.IdDependencies.Remove(resource.Id);
                iResource.IdOutputs.Remove(resource.Id);
            }

            Resources.Remove(resource);

            return false;
        }

        public int CreateNewResourceId()
        {
            ResourceId++;
            return ResourceId;
        }

        public bool AddResource(Resource resource)
        {
            Resources.Add(resource);
            return true;
        }

        public bool IsOutput(Resource resource)
        {
            foreach (Resource iResource in Resources)
            {
                if (iResource.IdOutputs.IndexOf(resource.Id) != -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
