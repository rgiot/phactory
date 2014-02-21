using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Project.Controller
{
    [Serializable]
    public class Config
    {
        [XmlIgnore]
        public string ApplicationPath = "";

        [XmlIgnore]
        public string ConfigPath = "";

        [XmlIgnore]
        public string UserConfigPath = "";

        public string PluginPath = "";

        public int MRUCount = 8;
        
        public Config()
        {
            FileInfo fileInfoAssembly = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ApplicationPath = fileInfoAssembly.DirectoryName + "\\";

            ConfigPath = ApplicationPath + "Config.xml";
            UserConfigPath = ApplicationPath + "UserConfig.xml";
        }

        public string GetDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
        }
    }
}
