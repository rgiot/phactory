using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;

namespace CPCCloud.Document
{
    [Serializable]
    public class Item
    {
        public int ResourceID;
        
        [System.Xml.Serialization.XmlIgnore]
        public byte[] CloudMaskData;

        [System.Xml.Serialization.XmlIgnore]
        public int CloudMaskWidth;
        [System.Xml.Serialization.XmlIgnore]
        public int CloudMaskHeight;

        public Item()
        {
            ResourceID = -1;
        }
    }

    [Serializable]
    public class Document
    {
        public string Version;
        public List<Item> Items = new List<Item>();
        
        public Document()
        {
            Version = Plugin.ControllerEditor.GetVersion();
        }

        public bool CompileInternal()
        {
            if (Plugin.ControllerEditor.Host.IsVerboseOutput())
            {
                Plugin.ControllerEditor.Host.Log("Resolving palettes and image data...");
            }

            foreach (Item item in Items)
            {
                var maskResource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                if (maskResource != null)
                {
                    var maskFileInfo = Plugin.ControllerEditor.Host.GetFileInfo(maskResource);
                    var maskImage = Image.FromFile(maskFileInfo.FullName);
                    var maskBitmap = new Bitmap(maskImage);

                    item.CloudMaskData = new byte[maskBitmap.Width*maskBitmap.Height];
                    item.CloudMaskWidth = maskBitmap.Width;
                    item.CloudMaskHeight = maskBitmap.Height;

                    for (int y = 0; y < maskBitmap.Height; y++)
                    {
                        for (int x = 0; x < maskBitmap.Width; x++)
                        {
                            var color = maskBitmap.GetPixel(x, y);

                            byte value = color.R;
                            item.CloudMaskData[(y*maskBitmap.Width) + x] = value;
                        }
                    }
                }
            }

            return true;
        }
    }
}
