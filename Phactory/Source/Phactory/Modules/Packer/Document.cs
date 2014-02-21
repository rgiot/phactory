using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CPCPacker.Document
{
    public enum PackerType
    {
        Exomizer = 1,
        BitBuster = 2
    };

    [Serializable]
    public class Item
    {
        public int ResourceID;

        public Item()
        {
            ResourceID = -1;
        }
    }

    [Serializable]
    public class Document
    {
        public string Version;
        public CPCPacker.Document.PackerType PackerType = PackerType.BitBuster;
        
        public List<Item> Items = new List<Item>();

        public Document()
        {
            Version = Plugin.ControllerEditor.GetVersion();
            PackerType = PackerType.BitBuster;
        }
    }
}
