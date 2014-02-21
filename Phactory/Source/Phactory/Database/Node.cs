using System;
using System.Collections.Generic;
using System.Text;

namespace PhactoryHost.Database
{
    [Serializable]
    public class Node
    {
        public bool IsFolder = false;
        public string FolderName = "";

        public int ResourceId = 0;

        public List<Node> ChildNodes = new List<Node>();
        public bool Expanded = false;
    }
}
