using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CPCDSK.Document
{
    public enum ItemType
    {
        Binary,
        Basic
    };

    [Serializable]
    public class Item
    {
        public int ResourceID;
        public string AmsdosFilename;
        public int LoadAdress;
        public int ExecAdress;
        public ItemType ItemType;
        public bool CopyToWinAPEROMFolder;
        public bool TrackLoaderItem;
        public bool IsDuplicate;
        public int DuplicatedIndex;

        public Item()
        {
            ResourceID = -1;

            LoadAdress = 0x0000;
            ExecAdress = 0x0000;
            ItemType = ItemType.Binary;
            CopyToWinAPEROMFolder = false;
            TrackLoaderItem = false;
            IsDuplicate = false;
            AmsdosFilename = String.Empty;
            DuplicatedIndex = -1;
        }
    }

    [Serializable]
    public class Document
    {
        public string Version;
        public List<Item> Items = new List<Item>();
        public bool GenerateHFE = false;
        public bool TrackLoaderDisc = false;

        public Document()
        {
            Version = Plugin.ControllerEditor.GetVersion();
        }
    }
}
