using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Project;
    
namespace PhactoryHost.Database
{
    [Serializable]
    public class Resource
    {
        public int Id = -1;
        public string DisplayName = "";
        public string RelativePath = "";

        public List<int> IdDependencies = new List<int>();
        public List<int> IdOutputs = new List<int>();

        public bool IsOutputResource = false;
        public bool IsIncludeResource = false;
        
        [XmlIgnoreAttribute]
        public DateTime LastWriteTime;

        [XmlIgnoreAttribute]
        public bool SkipNeedCompile = false;
        [XmlIgnoreAttribute]
        public bool NeedCompile = false;
        [XmlIgnoreAttribute]
        public bool FileExists = false;
        [XmlIgnoreAttribute]
        public List<Resource> IsResourceReferencedByOtherResources = null;
        [XmlIgnoreAttribute]
        public bool SkipIsResourceReferencedByOtherResources = false;

        [XmlIgnoreAttribute]
        public FileInfo FileInfo
        {
            get
            {
                return new FileInfo(App.Controller.UserConfig.ResourcePath + RelativePath);
            }
        }

        public Resource()
        {
        }

        public string GetFileExtension()
        {
            int lastDot = RelativePath.LastIndexOf(".");

            var extension = RelativePath.Substring(lastDot).ToLower();

            return extension;
        }

        public bool IsBitmapResource()
        {
            List<string> filterExtension = new List<string>();
            filterExtension.Add(".bmp");
            filterExtension.Add(".gif");
            filterExtension.Add(".jpg");
            filterExtension.Add(".png");
            filterExtension.Add(".tiff");

            string resourceExtension = this.FileInfo.Extension.ToLower();

            foreach (string extension in filterExtension)
            {
                if (resourceExtension == extension.ToLower())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
