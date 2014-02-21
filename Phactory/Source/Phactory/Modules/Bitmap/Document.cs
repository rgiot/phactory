using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;

namespace CPCBitmap.Document
{
    public enum ItemType
    {
        FullScreenBitmap,
        SpriteRawData, 
        SpriteData,
        SpriteOpcodes,
        SpriteScreenData,
        SpriteFullScreen,
        Font
    };

    public enum UseMaskType
    {
        NoMask,
        ColorMask,
        BitmapMask
    };

    public class IntermediateImage
    {
        public const int MaskIndex = 127;

        public int Width;
        public int Height;
        public int[] Data;

        public int[] MaskData;

        public int[] BackgroundData;

        public byte[] CloudData;
        public int CloudWidth;
        public int CloudHeight;
    }

    [Serializable]
    public class Item
    {
        public int ResourceID;
        
        [System.Xml.Serialization.XmlIgnore]
        public IntermediateImage IntermediateImage;
        public bool IsGroupDelimiter;
        public bool IsFullScreenTitle;

        public ItemType Type;
        public UseMaskType UseMaskType;
        public int ColorMask;
        public int MaskResourceID;
        public int MaskMergeResourceID;
        public int MaskFadeResourceID;
        public int MaskPenIndex;
        public int MergePosX;
        public int MergePosY;
        public bool IsMerge;
        public bool IsFade;

        public int CloudResourceID;
        public bool IsCloudSprite;

        public bool FontAlignOnCharaterLine;
        public int FontCharWidthInBytes;
        
        public Item()
        {
            ResourceID = -1;

            IsGroupDelimiter = false;
            
            UseMaskType = UseMaskType.NoMask;
            ColorMask = Color.Transparent.ToArgb();
            MaskResourceID = -1;
            CloudResourceID = -1;
            IsMerge = false;
            IsFade = false;
            MergePosX = 0;
            MergePosY = 0;
            MaskMergeResourceID = -1;
            MaskFadeResourceID = -1;
            IsCloudSprite = false;
            MaskPenIndex = 15;
            FontAlignOnCharaterLine = false;
            FontCharWidthInBytes = 2;
        }
    }

    [Serializable]
    public class Document
    {
        public string Version;
        public List<Item> Items = new List<Item>();

        public bool ExportPalette = true;
        public int StartIndex = 0;
        public bool FadeFromWhite = false;
        public int VideoMode = 0;

        private int[] CPCPaletteGateArray = new int[]
        {
            0x54,
            0x44,
            0x55,
            0x5C,
            0x58,
            0x5D,
            0x4C,
            0x45,
            0x4D,
            0x56,	
            0x46,
            0x57,
            0x5E,
            0x40,
            0x5F,
            0x4E,
            0x47,
            0x4F,
            0x52,
            0x42,
            0x53,
            0x5A,
            0x59,
            0x5B,
            0x4A,
            0x43,
            0x4B
        };

        // ASIC
        private int[] CPCPalette = new int[]
        { 
            0x020702, 
            0x050663,
            0x0507f1,
            0x670600,
            0x680764,
            0x6807F1,
            0xFD0704,
            0xFF0764,
            0xFD07F2,
            0x046703,
            0x046764,
            0x0567f1,
            0x686704,
            0x686764,
            0x6867f1,
            0xfd6704,
            0xfd6763,
            0xfd67f1,
            0x04f502,
            0x04f562,
            0x04f5f1,
            0x68f500,
            0x68f564,
            0x68f5f1,
            0xfef504,
            0xfdf563,
            0xfdf5f0
        };

        // Gate Array
        /*private int[] CPCPalette = new int[]
        { 
            0x000201, 
            0x00026B,
            0x0C02F4,
            0x6C0201,
            0x690268,
            0x6C02F2,
            0xF30506,
            0xF00268,
            0xF302F4,
            0x027801,
            0x007868,
            0x0C7BF4,
            0x6E7B01,
            0x6E7D6B,
            0x6E7BF6,
            0xF37D0D,
            0xF37D6B,
            0xFA80F9,
            0x02F001,
            0x00F36B,
            0x0FF3F2,
            0x71F504,
            0x71F36B,
            0x71F3F4,
            0xF3F30D,
            0xF3F36D,
            0xFFF3F9
        };*/

        private List<int> CPCPaletteIndices;
        public UInt16[] CPCAsicPalette = new UInt16[16] { 0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0 };

        public Document()
        {
            Version = Plugin.ControllerEditor.GetVersion();
        }

        public int[] GetCPCPaletteGateArray()
        {
            return CPCPaletteGateArray;
        }

        public int[] GetCPCPalette()
        {
            return CPCPalette;
        }

        public List<int> GetCPCPaletteIndices()
        {
            return CPCPaletteIndices;
        }

        public bool CompileInternal()
        {
            if (!CreateIndices())
            {
                return false;
            }

            return true;
        }

        private byte GetVideoMode(Bitmap bitmap)
        {
            bool isMode0 = true;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x+=2)
                {
                    if (bitmap.GetPixel(x, y) != bitmap.GetPixel(x + 1, y))
                    {
                        isMode0 = false;
                    }
                }
            }

            if (isMode0)
            {
                return 0;
            }

            return 1;
        }

        public bool CreateIndices()
        {
            bool success = true;

            CPCPaletteIndices = new List<int>();

            if (Plugin.ControllerEditor.Host.IsVerboseOutput())
            {
                Plugin.ControllerEditor.Host.Log("Resolving palettes and image data...");
            }

            foreach (Item item in Items)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                FileInfo fileInfo = Plugin.ControllerEditor.Host.GetFileInfo(resource);

                Image image = Image.FromFile(fileInfo.FullName);
                Bitmap bitmap = new Bitmap(image);

                item.IntermediateImage = new IntermediateImage();
                
                int xInc = 0;
                if (VideoMode == 0)
                {
                    xInc = 2;
                }
                else
                {
                    xInc = 1;
                }

                item.IntermediateImage.Width = image.Width / xInc;
                item.IntermediateImage.Height = image.Height;
                item.IntermediateImage.Data = new int[(image.Width / xInc) * image.Height];
                item.IntermediateImage.MaskData = new int[(image.Width / xInc) * image.Height];

                if ((item.IsCloudSprite) && (item.CloudResourceID != -1))
                {
                    Bitmap cloudBitmap = null;

                    PhactoryHost.Database.Resource cloudResource = Plugin.ControllerEditor.Host.GetResource(item.CloudResourceID);
                    FileInfo cloudFileInfo = Plugin.ControllerEditor.Host.GetFileInfo(cloudResource);

                    Image cloudImage = Image.FromFile(cloudFileInfo.FullName);
                    cloudBitmap = new Bitmap(cloudImage);

                    item.IntermediateImage.CloudWidth = cloudBitmap.Width;
                    item.IntermediateImage.CloudHeight = cloudBitmap.Height;
                    item.IntermediateImage.CloudData = new byte[cloudBitmap.Width * cloudBitmap.Height];

                    for (int y = 0; y < cloudBitmap.Height; y++)
                    {
                        for (int x = 0; x < cloudBitmap.Width; x++)
                        {
                            Color color = cloudBitmap.GetPixel(x, y);

                            item.IntermediateImage.CloudData[(y * item.IntermediateImage.CloudWidth) + x] = color.R;
                        }
                    }
                }

                Bitmap mergeBitmap = null;

                if ((item.IsMerge) && (item.UseMaskType == UseMaskType.ColorMask) && (item.MaskMergeResourceID != -1))
                {
                    PhactoryHost.Database.Resource mergeResource = Plugin.ControllerEditor.Host.GetResource(item.MaskMergeResourceID);
                    FileInfo mergeFileInfo = Plugin.ControllerEditor.Host.GetFileInfo(mergeResource);

                    Image mergeImage = Image.FromFile(mergeFileInfo.FullName);
                    mergeBitmap = new Bitmap(mergeImage);
                }

                if (item.IsMerge && (mergeBitmap != null))
                {
                    item.IntermediateImage.BackgroundData = new int[(image.Width / xInc) * image.Height];
                }

                if (item.UseMaskType == UseMaskType.ColorMask)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x += xInc)
                        {
                            Color color = bitmap.GetPixel(x, y);

                            int value = 1;
                            if (color == Color.FromArgb(item.ColorMask))
                            {
                                value = 0;
                            }

                            item.IntermediateImage.MaskData[(y * item.IntermediateImage.Width) + (x / xInc)] = value;
                        }
                    }
                }
                else if (item.UseMaskType == UseMaskType.BitmapMask)
                {
                    PhactoryHost.Database.Resource maskResource = Plugin.ControllerEditor.Host.GetResource(item.MaskResourceID);
                    if (maskResource == null)
                    {
                        continue;
                    }

                    FileInfo maskFileInfo = Plugin.ControllerEditor.Host.GetFileInfo(maskResource);

                    Image maskImage = Image.FromFile(maskFileInfo.FullName);
                    Bitmap maskBitmap = new Bitmap(maskImage);

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x += xInc)
                        {
                            Color color = maskBitmap.GetPixel(x, y);

                            int value = 1;
                            // let's be tolerant: black pixels can be not-so-exact black pixels
                            if ((color.R > 8) && (color.G > 8) && (color.B > 8))
                            {
                                value = 0;
                            }

                            item.IntermediateImage.MaskData[(y * item.IntermediateImage.Width) + (x / xInc)] = value;
                        }
                    }
                }
                else
                {
                    // empty mask
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x += xInc)
                        {
                            item.IntermediateImage.MaskData[(y * item.IntermediateImage.Width) + (x / xInc)] = 1;
                        }
                    }
                }

                invalidPixel = 0;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x += xInc)
                    {
                        int imageOffset = (y * item.IntermediateImage.Width) + (x / xInc);

                        Color color = bitmap.GetPixel(x, y);
                        Color backgroundColor = Color.Empty;
                        int u = 0;
                        int v = 0;

                        if (item.IsMerge && (mergeBitmap != null))
                        {
                            u = x + item.MergePosX*4;
                            v = y + item.MergePosY;

                            try
                            {
                                backgroundColor = mergeBitmap.GetPixel(u, v);
                            }
                            catch
                            {
                                backgroundColor = Color.Red;
                            }

                            SetPixel(backgroundColor, ref item.IntermediateImage.BackgroundData, imageOffset);    
                        }
                        
                        if ((item.UseMaskType == UseMaskType.ColorMask)
                            || (item.UseMaskType == UseMaskType.BitmapMask))
                        {
                            if (item.IntermediateImage.MaskData[imageOffset] == 0)
                            {
                                if (item.IsMerge && (mergeBitmap != null))
                                {
                                    if ((u < mergeBitmap.Width) && (v < mergeBitmap.Height))
                                    {
                                        color = backgroundColor;
                                    }
                                    else
                                    {
                                        item.IntermediateImage.Data[imageOffset] = IntermediateImage.MaskIndex;
                                        continue;
                                    }
                                }
                                else
                                {
                                    item.IntermediateImage.Data[imageOffset] = IntermediateImage.MaskIndex;
                                    continue;
                                }
                            }
                        }

                        SetPixel(color, ref item.IntermediateImage.Data, imageOffset);
                    }
                }

                if (Plugin.ControllerEditor.Host.IsVerboseOutput())
                {
                    if (invalidPixel == 0)
                    {
                        if (CPCPaletteIndices.Count > 16)
                        {
                            Plugin.ControllerEditor.Host.Log("Palette failed with '" + resource.DisplayName + "' (too many colors found, expecting max. 16 colors, but " + CPCPaletteIndices.Count + " were found)");

                            success = false;
                        }
                        else
                        {
                            Plugin.ControllerEditor.Host.Log("Palette resolved with " + resource.DisplayName);
                        }
                    }
                    else
                    {
                        Plugin.ControllerEditor.Host.Log("Palette failed with '" + resource.DisplayName + "' (" + invalidPixel + " invalid pixels were found)");
                        success = false;
                    }
                }
            }

            return success;
        }

        private int invalidPixel = 0;
        
        private void SetPixel(Color color, ref int[] data, int offset)
        {
            Color CPCcolor = new Color();
            int tolerance = 95;

            int colorR = color.R;
            int colorG = color.G;
            int colorB = color.B;

            int bestScore = 10000;
            int index = -1;
            for (int iCPCPal = 0; iCPCPal <= 26; iCPCPal++)
            {
                CPCcolor = Color.FromArgb(CPCPalette[iCPCPal]);
                int CPCcolorR = CPCcolor.R;
                int CPCcolorG = CPCcolor.G;
                int CPCcolorB = CPCcolor.B;

                if ((colorR > (CPCcolorR - tolerance))
                    && (colorR <= (CPCcolorR + tolerance)))
                {
                    if ((colorG > (CPCcolorG - tolerance))
                        && (colorG <= (CPCcolorG + tolerance)))
                    {
                        if ((colorB > (CPCcolorB - tolerance))
                        && (colorB <= (CPCcolorB + tolerance)))
                        {
                            int diffR = CPCcolorR - colorR;
                            if (diffR < 0)
                            {
                                diffR = -colorR;
                            }

                            int diffG = CPCcolorG - colorG;
                            if (diffG < 0)
                            {
                                diffG = -colorG;
                            }

                            int diffB = CPCcolorB - colorB;
                            if (diffB < 0)
                            {
                                diffB = -colorB;
                            }

                            int score = diffR + diffG + diffB;
                            if (score <= bestScore)
                            {
                                bestScore = score;

                                index = iCPCPal;
                            }

                            break;
                        }
                    }
                }
            }

            if (index == -1)
            {
                invalidPixel++;

                data[offset] = 0;
            }
            else
            {
                int palIndex = -1;

                bool indexFound = false;
                for (int iCPCIndex = 0; iCPCIndex < CPCPaletteIndices.Count; iCPCIndex++)
                {
                    if (CPCPaletteIndices[iCPCIndex] == index)
                    {
                        palIndex = iCPCIndex;
                        indexFound = true;
                        break;
                    }
                }

                if (indexFound == false)
                {
                    palIndex = CPCPaletteIndices.Count;
                    CPCPaletteIndices.Add(index);
                }

                data[offset] = palIndex;
            }
        }
    }
}
