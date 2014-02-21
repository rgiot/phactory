using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Project.Controller
{
    [Serializable]
    public class UserConfig
    {
        public string ProjectFilename = "";
        public string ProjectPath = "";
        public string ResourcePath = "";

        public string AddExistingResourceFullPath = ""; 
        
        public string NewProjectPath = "";
        public string NewFilePath = "";

        public bool LoadLastLoadedProject = true;
        public bool ReopenResources = false;
        public bool MaximizeWindowAtStartup = false;
        public bool VerboseOutput = false;
        public bool FileMonitoring = false;
        public bool SaveAllBeforeBuilding = true;
        public bool HideUnusedResourcesInProjectExplorer = true;
        public bool DontBuildBeforeRun = false;
        public int ProcessTimeoutInSec = 200;
        
        public string DataSource = "template.s3db";
        public bool DBReadOnly = false;

        [XmlArrayAttribute]
        public List<int> OpenedResources = new List<int>();

        [XmlArrayAttribute]
        public List<string> RecentProjects = new List<string>();

        [XmlArrayAttribute]
        public List<string> RecentFiles = new List<string>();

        public SerializableDictionary<string, string> PluginSettings = new SerializableDictionary<string, string>();

        public UserConfig()
        {
        }
    }

    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
