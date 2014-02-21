using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Project;

namespace CPCBitmap.Controller
{
    public partial class Compiler : PhactoryHost.CompilerPlugin
    {
        public PhactoryHost.Host Host;

        public Compiler()
        {
            Plugin.ControllerCompiler = this;
        }

        public bool IsDefaultPluginForUnknownTypes()
        {
            return false;
        }

        public bool ShowSettings(Panel parentPanel)
        {
            return false;
        }

        public bool SaveSettings()
        {
            return false;
        }

        public void Register(PhactoryHost.Host parent)
        {
            this.Host = parent;
        }

        public string GetName()
        {
            return "Bitmap Compiler";
        }

        public string GetDescription()
        {
            return "Create Bitmap (or group of bitmap) converted to CPC constraints";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cpcbitmap", "Bitmap(s) file (*.cpcbitmap)", true));

            return extensions;
        }

        public bool IsResourceSupported(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            foreach (PhactoryHost.PluginExtension extension in GetSupportedExtensions())
            {
                if (String.Compare(extension.GetName(), fileInfo.Extension, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsIncludeResource(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            if (!IsResourceSupported(resource))
            {
                return false;
            }

            return false; 
        }

        public bool Compile(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            if (!IsResourceSupported(resource))
            {
                return false;
            }

            string CPCBitmapFilename = Host.GetFileInfo(resource).FullName;
            FileInfo CPCBitmapFileInfo = new FileInfo(CPCBitmapFilename);
            
            Document.Document tempDocument = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);
            if (!tempDocument.CompileInternal())
            {
                return false;
            }

            var CPCBitmapFilenamePalette = CPCBitmapFilename + ".palette";
            if (WritePalette(tempDocument, CPCBitmapFilenamePalette) == false)
            {
                return false;
            }

            var CPCBitmapFilenameFirmwarePalette = CPCBitmapFilename + ".firmwarePalette";
            if (WriteFirmwarePalette(tempDocument, CPCBitmapFilenameFirmwarePalette) == false)
            {
                return false;
            }

            string destFilenameH = Host.GetFileInfo(resource).FullName + ".info.h";
            var title = resource.DisplayName.ToUpper().Replace(" ", "").Replace(".", "") + "H";

            var fileContent = String.Empty;
            fileContent += "// ----------------------------------------------------------------------------\n";
            fileContent += "#ifndef _" + title + "_INFO_H_\n";
            fileContent += "#define _" + title + "_INFO_H_\n";
            fileContent += "\n";

            fileContent += "// ----------------------------------------------------------------------------\n";

            List<Document.Item> items = new List<Document.Item>();
            bool isGroup = false;
            foreach (Document.Item item in tempDocument.Items)
            {
                if (item.IsGroupDelimiter)
                {
                    if ( isGroup )
                    {
                        items.Add(item);
                        isGroup = false;
                    }
                    else
                    {
                        isGroup = true;
                    }
                }
                if ( isGroup )
                {
                    items.Add(item);
                }
            }
            isGroup = false;
            int startIndex = -1;
            List<Document.Item> resolvedItems = new List<Document.Item>();
            for ( int iItem = 0; iItem<tempDocument.Items.Count; iItem++)
            {
                Document.Item item = tempDocument.Items[iItem];
                Document.Item prevItem = null;

                if (item.IsGroupDelimiter)
                {
                    if ( isGroup )
                    {
                        prevItem = tempDocument.Items[iItem-2];
                        isGroup = false;
                    }
                    else
                    {
                        isGroup = true;
                        startIndex = iItem;
                    }
                }

                if ( isGroup )
                {
                    if ( iItem == startIndex )
                    {
                        prevItem = tempDocument.Items[startIndex + items.Count - 1 - 1];
                    }
                    else
                    if ( iItem == ( startIndex + 1 ) )
                    {
                        prevItem = tempDocument.Items[startIndex + items.Count - 1];
                    }
                    else
                    {
                        prevItem = tempDocument.Items[iItem-2];
                    }
                }
                
                resolvedItems.Add(prevItem);
            }

            for ( int iItem = 0; iItem<tempDocument.Items.Count; iItem++)
            {
                App.Controller.View.AppDoEvents(); 
                
                Document.Item item = tempDocument.Items[iItem];

                PhactoryHost.Database.Resource resItem = Host.GetResource(item.ResourceID);
                if (resItem == null)
                {
                    Host.Log("Unknown resource identifier : " + item.ResourceID);
                    return false;
                }

                FileInfo resFileInfo = Host.GetFileInfo(resItem);
                string outputFilename = resFileInfo.FullName;

                if (Host.IsVerboseOutput())
                {
                    Host.Log(outputFilename);
                }

                bool useMask = false;
                if (item.UseMaskType == Document.UseMaskType.NoMask)
                {
                }
                else
                if (item.UseMaskType == Document.UseMaskType.ColorMask)
                {
                    useMask = true;
                }
                else
                if (item.UseMaskType == Document.UseMaskType.BitmapMask)
                {
                    useMask = true;
                }

                var itemResource = Host.GetResource(item.ResourceID);
                var itemTitle = itemResource.DisplayName.ToUpper().Replace(" ", "").Replace(".", "");

                if (item.IsFade)
                {
                    for (int iTargetItem = 0; iTargetItem < tempDocument.Items.Count; iTargetItem++)
                    {
                        var targetItem = tempDocument.Items[iTargetItem];
                        
                        if (targetItem.ResourceID == item.MaskFadeResourceID)
                        {
                            var targetItemResource = Host.GetResource(targetItem.ResourceID);
                            
                            string outputFadeFilename = outputFilename + ".fadePalette";

                            if (!WriteFadePalette(tempDocument, outputFadeFilename, item, targetItem))
                            {
                                return false;
                            }

                            break;
                        }
                    }
                }
                else if (item.Type == Document.ItemType.FullScreenBitmap)
                {
                    string outputTopFilename = outputFilename + ".topBin";
                    string outputBottomFilename = outputFilename + ".bottomBin";

                    var bitmapCompiler = BitmapCompiler.CreateCompiler(tempDocument.VideoMode);
                    if (!bitmapCompiler.WriteFullscreenBitmap(outputTopFilename, 
                                                                outputBottomFilename, 
                                                                item.IsFullScreenTitle,
                                                                (byte)item.MaskPenIndex,
                                                                item.IntermediateImage.Width, 
                                                                item.IntermediateImage.Height, 
                                                                item.IntermediateImage.Data,
                                                                (byte)tempDocument.StartIndex))
                    {
                        return false;
                    }

                    fileContent += "#define " + itemTitle + "_FSBITMAP_HEIGHT " + item.IntermediateImage.Height + "\n";
                    fileContent += "#define " + itemTitle + "_FSBITMAP_BYTEWIDTH " + item.IntermediateImage.Width / 2 + "\n";
                    fileContent += "#define " + itemTitle + "_FSBITMAP_SIZE " + (item.IntermediateImage.Height / 2) * (item.IntermediateImage.Width / 2) + "\n";
                }
                else if (item.Type == Document.ItemType.Font)
                {
                    string outputFontFilename = outputFilename + ".font";

                    var bitmapCompiler = BitmapCompiler.CreateCompiler(tempDocument.VideoMode);
                    if (!bitmapCompiler.WriteFont(outputFontFilename, item.FontAlignOnCharaterLine, item.FontCharWidthInBytes, 
                                                                item.IntermediateImage.Width,
                                                                item.IntermediateImage.Height,
                                                                item.IntermediateImage.Data,
                                                                (byte)tempDocument.StartIndex))
                    {
                        return false;
                    }

                    fileContent += "#define " + itemTitle + "_FONT_HEIGHT " + item.IntermediateImage.Height + "\n";
                    fileContent += "#define " + itemTitle + "_FONT_BYTEWIDTH " + item.FontCharWidthInBytes + "\n";
                    fileContent += "#define " + itemTitle + "_FONT_CHARCOUNT " + ((item.IntermediateImage.Width/4) / item.FontCharWidthInBytes) + "\n";
                }
                else if (
                        (item.Type == Document.ItemType.SpriteFullScreen) 
                    || (item.Type == Document.ItemType.SpriteRawData) 
                    || (item.Type == Document.ItemType.SpriteScreenData) 
                    || (item.Type == Document.ItemType.SpriteData)
                    || (item.Type == Document.ItemType.SpriteOpcodes)
                    || (item.Type == Document.ItemType.Font))
                {
                    string outputSpriteBitplan1Filename = "";
                    string outputSpriteBitplan2Filename = "";

                    string outputCloudSpriteBitplan1Filename = "";

                    int spriteType = 0;

                    if (item.Type == Document.ItemType.SpriteRawData)
                    {
                        outputSpriteBitplan1Filename = outputFilename + ".sprRawData1";
                        outputSpriteBitplan2Filename = outputFilename + ".sprRawData2"; 
                        
                        if (item.IsCloudSprite)
                        {
                            outputCloudSpriteBitplan1Filename = outputFilename + ".cloudSprite.sprRawData1";
                        }

                        spriteType = 0;
                    }
                    else if (item.Type == Document.ItemType.SpriteData)
                    {
                        outputSpriteBitplan1Filename = outputFilename + ".sprData1";
                        outputSpriteBitplan2Filename = outputFilename + ".sprData2";

                        spriteType = 1;
                    }
                    else if (item.Type == Document.ItemType.SpriteScreenData)
                    {
                        outputSpriteBitplan1Filename = outputFilename + ".sprScrData1";
                        outputSpriteBitplan2Filename = outputFilename + ".sprScrData2";

                        spriteType = 3;
                    }
                    else if (item.Type == Document.ItemType.SpriteOpcodes)
                    {
                        outputSpriteBitplan1Filename = outputFilename + ".sprZ801";
                        outputSpriteBitplan2Filename = outputFilename + ".sprZ802";

                        spriteType = 2;
                    }
                    else if (item.Type == Document.ItemType.SpriteFullScreen)
                    {
                        outputSpriteBitplan1Filename = outputFilename + ".sprFullScreen";
                        outputSpriteBitplan2Filename = String.Empty;

                        spriteType = 4;
                    }

                    int[] diffData = null;
                    if (resolvedItems[iItem] != null)
                    {
                        diffData = resolvedItems[iItem].IntermediateImage.Data;
                    }

                    var bitmapCompiler = BitmapCompiler.CreateCompiler(tempDocument.VideoMode);
                    if (!bitmapCompiler.WriteSprite(outputSpriteBitplan1Filename,
                                                    outputSpriteBitplan2Filename,
                                                    item.IntermediateImage.Width,
                                                    item.IntermediateImage.Height,
                                                    item.IntermediateImage.Data,
                                                    item.IntermediateImage.BackgroundData,
                                                    spriteType,
                                                    useMask,
                                                    item.IntermediateImage.MaskData,
                                                    tempDocument.StartIndex,
                                                    item.MergePosX,
                                                    item.MergePosY,
                                                    item.IsCloudSprite,
                                                    outputCloudSpriteBitplan1Filename,
                                                    item.IntermediateImage.CloudData,
                                                    item.IntermediateImage.CloudWidth,
                                                    item.IntermediateImage.CloudHeight,
                                                    diffData
                                                    ))
                    {
                        return false;
                    }

                    fileContent += "#define " + itemTitle + "_SPR_HEIGHT " + bitmapCompiler.GetLastHeight() + "\n";

                    if (String.IsNullOrEmpty(outputSpriteBitplan2Filename))
                    {
                        fileContent += "#define " + itemTitle + "_SPR_BYTEWIDTH " + bitmapCompiler.GetLastWidthBytesBitplan1() + "\n";
                        fileContent += "#define " + itemTitle + "_SPR_SIZE 0x" + String.Format("{0:X4}", bitmapCompiler.GetLastWidthBytesBitplan1() * bitmapCompiler.GetLastHeight()) + "\n";
                    }
                    else
                    {
                        fileContent += "#define " + itemTitle + "_SPR_BYTEWIDTH1 " + bitmapCompiler.GetLastWidthBytesBitplan1() + "\n";
                        fileContent += "#define " + itemTitle + "_SPR_BYTEWIDTH2 " + bitmapCompiler.GetLastWidthBytesBitplan2() + "\n";
                        fileContent += "#define " + itemTitle + "_SPR_SIZE1 0x" + String.Format("{0:X4}", bitmapCompiler.GetLastWidthBytesBitplan1() * bitmapCompiler.GetLastHeight()) + "\n";
                        fileContent += "#define " + itemTitle + "_SPR_SIZE2 0x" + String.Format("{0:X4}", bitmapCompiler.GetLastWidthBytesBitplan2() * bitmapCompiler.GetLastHeight()) + "\n";
                    }

                    if (item.IsMerge)
                    {
                        fileContent += "#define " + itemTitle + "_MERGE_POSX " + item.MergePosX + "\n";
                        fileContent += "#define " + itemTitle + "_MERGE_POSY " + item.MergePosY + "\n";
                    }
                }
            }

            fileContent += "\n";

            fileContent += "// ----------------------------------------------------------------------------\n";
            fileContent += "#endif // _" + title + "_INFO_H_";

            File.WriteAllText(destFilenameH, fileContent);

            return true;
        }

        bool WritePalette(  Document.Document tempDocument, 
                            string CPCBitmapFilename )
        {
            int[] gateArrayPalette = null;

            gateArrayPalette = new int[16];
            for (int i = 0; i < 16; i++)
            {
                int palIndex = 0; // black as default color (unused)
                if (i < tempDocument.GetCPCPaletteIndices().Count)
                {
                    palIndex = tempDocument.GetCPCPaletteIndices()[i];
                }

                int gateArrayColor = tempDocument.GetCPCPaletteGateArray()[palIndex];

                int finalIndex = i + tempDocument.StartIndex;
                if (finalIndex < 16)
                {
                    gateArrayPalette[i + tempDocument.StartIndex] = gateArrayColor;
                }
            }

            var bitmapCompiler = BitmapCompiler.CreateCompiler(tempDocument.VideoMode);
            if (!bitmapCompiler.WritePalette(CPCBitmapFilename, gateArrayPalette, tempDocument.CPCAsicPalette))
            {
                return false;
            }

            return true;
        }

        bool WriteFirmwarePalette(Document.Document tempDocument,
                                    string CPCBitmapFirmwareFilename)
        {
            int[] firmwarePalette = null;

            firmwarePalette = new int[16];
            for (int i = 0; i < 16; i++)
            {
                int palIndex = 0; // black as default color (unused)
                if (i < tempDocument.GetCPCPaletteIndices().Count)
                {
                    palIndex = tempDocument.GetCPCPaletteIndices()[i];
                }

                int firmwareColor = palIndex;

                int finalIndex = i + tempDocument.StartIndex;
                if (finalIndex < 16)
                {
                    firmwarePalette[i + tempDocument.StartIndex] = firmwareColor;
                }
            }

            var bitmapCompiler = BitmapCompiler.CreateCompiler(tempDocument.VideoMode);
            if (!bitmapCompiler.WritePalette(CPCBitmapFirmwareFilename, firmwarePalette, null))
            {
                return false;
            }

            return true;
        }

        bool WriteFadePalette(Document.Document tempDocument,
                                string outputFadeFilename, 
                                CPCBitmap.Document.Item item, 
                                CPCBitmap.Document.Item targetItem)
        {
            int[] fadeGateArrayPalette = new int[16];
            UInt16[] fadeCPCAsicPalette = new UInt16[16] { 0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0 };

            for (int iClear = 0; iClear < 16; iClear++)
            {
                fadeGateArrayPalette[iClear] = tempDocument.GetCPCPaletteGateArray()[0];
            }
            
            for (int offset = 0; offset < item.IntermediateImage.Width * item.IntermediateImage.Height; offset++)
            {
                int palIndex = item.IntermediateImage.Data[offset];
                int targetPalIndex = targetItem.IntermediateImage.Data[offset];

                int finalPalIndex = tempDocument.GetCPCPaletteIndices()[palIndex];
                int gateArrayColor = tempDocument.GetCPCPaletteGateArray()[finalPalIndex];

                int finalIndex = targetPalIndex + tempDocument.StartIndex;
                if (finalIndex < 16)
                {
                    fadeGateArrayPalette[targetPalIndex + tempDocument.StartIndex] = gateArrayColor;
                }

                fadeCPCAsicPalette[targetPalIndex + tempDocument.StartIndex] = tempDocument.CPCAsicPalette[palIndex];
            }

            var bitmapCompiler = BitmapCompiler.CreateCompiler(tempDocument.VideoMode);
            if (!bitmapCompiler.WritePalette(outputFadeFilename, fadeGateArrayPalette, fadeCPCAsicPalette))
            {
                return false;
            }

            return true;
        }
    }
}
