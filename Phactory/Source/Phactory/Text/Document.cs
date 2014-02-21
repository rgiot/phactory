using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CPCText.Document
{
    [Serializable]
    public class Document
    {
        public string Charset;
        public string Text;
        public bool AppendEndOfText = true;
        
        public Document()
        {
        }
    }
}
