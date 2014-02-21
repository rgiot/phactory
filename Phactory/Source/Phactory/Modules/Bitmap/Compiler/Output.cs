using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CPCBitmap.Controller
{
    public partial class Compiler : PhactoryHost.CompilerPlugin
    {
        public void RefreshOutput(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return;
            }

            if (!fileInfo.Exists)
            {
                return;
            }

            if (!IsResourceSupported(resource))
            {
                return;
            }

            Document.Document tempDocument = Host.XMLRead<Document.Document>(Host.GetFileInfo(resource).FullName);

            List<string> outputFilenames = new List<string>();

            string outputFilename = fileInfo.Name;
            var outputFilenamePalette = outputFilename + ".palette";
            var outputFilenameFirmwarePalette = outputFilename + ".firmwarePalette";
            outputFilenames.Add(outputFilenamePalette);
            outputFilenames.Add(outputFilenameFirmwarePalette);

            outputFilename = fileInfo.Name + ".info.h";
            outputFilenames.Add(outputFilename);

            foreach (Document.Item item in tempDocument.Items)
            {
                PhactoryHost.Database.Resource resItem = Host.GetResource(item.ResourceID);
                FileInfo resFileInfo = Host.GetFileInfo( resItem );
                outputFilename = resFileInfo.Name;

                if (item.IsFade)
                {
                    string outputFadeFilename = outputFilename + ".fadePalette";
                    outputFilenames.Add(outputFadeFilename);
                }
                else if (item.Type == Document.ItemType.FullScreenBitmap)
                {
                    string outputTopFilename = outputFilename + ".topBin";
                    string outputBottomFilename = outputFilename + ".bottomBin";
                    outputFilenames.Add(outputTopFilename);
                    outputFilenames.Add(outputBottomFilename);
                }
                else if (item.Type == Document.ItemType.Font)
                {
                    string outputFontFilename = outputFilename + ".font";
                    outputFilenames.Add(outputFontFilename);
                }
                else if (item.Type == Document.ItemType.SpriteRawData)
                {
                    if (item.IsCloudSprite)
                    {
                        string outputCloudSpriteBitplan1Filename = outputFilename + ".cloudSprite.sprRawData1";
                        outputFilenames.Add(outputCloudSpriteBitplan1Filename);
                    }
                    else
                    {
                        string outputSpriteBitplan1Filename = outputFilename + ".sprRawData1";
                        string outputSpriteBitplan2Filename = outputFilename + ".sprRawData2";

                        outputFilenames.Add(outputSpriteBitplan1Filename);
                        outputFilenames.Add(outputSpriteBitplan2Filename);
                    }
                }
                else if (item.Type == Document.ItemType.SpriteData)
                {
                    string outputSpriteBitplan1Filename = outputFilename + ".sprData1";
                    string outputSpriteBitplan2Filename = outputFilename + ".sprData2";

                    outputFilenames.Add(outputSpriteBitplan1Filename);
                    outputFilenames.Add(outputSpriteBitplan2Filename);
                }
                else if (item.Type == Document.ItemType.SpriteScreenData)
                {
                    string outputSpriteBitplan1Filename = outputFilename + ".sprScrData1";
                    string outputSpriteBitplan2Filename = outputFilename + ".sprScrData2";

                    outputFilenames.Add(outputSpriteBitplan1Filename);
                    outputFilenames.Add(outputSpriteBitplan2Filename);
                }
                else if (item.Type == Document.ItemType.SpriteOpcodes)
                {
                    string outputSpriteBitplan1Filename = outputFilename + ".sprZ801";
                    string outputSpriteBitplan2Filename = outputFilename + ".sprZ802";

                    outputFilenames.Add(outputSpriteBitplan1Filename);
                    outputFilenames.Add(outputSpriteBitplan2Filename);
                }
                else if (item.Type == Document.ItemType.SpriteFullScreen)
                {
                    string outputSpriteBitplan1Filename = outputFilename + ".sprFullScreen";

                    outputFilenames.Add(outputSpriteBitplan1Filename);
                }
            }       

            Host.RefreshOutput(resource, outputFilenames); 
        }
    }
}