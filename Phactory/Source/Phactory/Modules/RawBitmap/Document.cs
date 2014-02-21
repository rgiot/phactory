using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;

namespace CPCRawBitmap.Document
{
    public enum ItemType
    {
        Raw,
        VerticalRaw
    };

    public class IntermediateImage
    {
        public int Width;
        public int Height;

        public int IndiceCount;

        public int[] Data;
    }

    [Serializable]
    public class Item
    {
        public int ResourceID;
        
        [System.Xml.Serialization.XmlIgnore]
        public IntermediateImage IntermediateImage;

        public ItemType Type;

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
            if (!CreateIndices())
            {
                return false;
            }

            return true;
        }

        public bool CreateIndices()
        {
            Plugin.ControllerEditor.Host.Log("Resolving image data...");

            foreach (Item item in Items)
            {
                List<int> indices = new List<int>();

                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                FileInfo fileInfo = Plugin.ControllerEditor.Host.GetFileInfo(resource);

                Image image = Image.FromFile(fileInfo.FullName);
                Bitmap bitmap = new Bitmap(image);

                item.IntermediateImage = new IntermediateImage();
                int pixelIncrement = 1; // was 2... why? because of Ced scaling?
                
                bool tooManyIndices = false;

                int indexMax = 0;
                if (        (item.Type == ItemType.Raw)
                        ||  (item.Type == ItemType.VerticalRaw) )
                {
                    indexMax = 255;
                }
                
                item.IntermediateImage.Width = image.Width / pixelIncrement;
                item.IntermediateImage.Height = image.Height;
                item.IntermediateImage.Data = new int[item.IntermediateImage.Width * image.Height];
                
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x += pixelIncrement)
                    {
                        int imageOffset = (y * item.IntermediateImage.Width) + (x / pixelIncrement);

                        Color color = bitmap.GetPixel(x, y);
                        int argb = color.ToArgb();

                        if (!indices.Contains(argb))
                        {
                            indices.Add(argb);
                        }

                        if ( (indices.Count-1) > indexMax)
                        {
                            tooManyIndices = true;
                        }

                        item.IntermediateImage.Data[imageOffset] = indices.IndexOf(argb);
                    }
                }

                item.IntermediateImage.IndiceCount = indices.Count;

                if (tooManyIndices)
                {
                    Plugin.ControllerEditor.Host.Log("Too many different indices found with '" + resource.DisplayName + "' (expecting max. " + (indexMax+1) + " colors, but " + indices.Count + " were found)");
                    
                    return false;
                }
            }

            return true;
        }   
    }
}
