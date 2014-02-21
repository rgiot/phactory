using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Project;

namespace CPCBitmap.Controller
{
    public class BitmapCompilerMode1 : BitmapCompilerInterface
    {
        private byte ConvertPixelCPC_Mode1(byte p0, byte p1, byte p2, byte p3)
        {
            byte v;
            v = (byte)(
                    (((p0) & 1) << 7) + (((p1) & 1) << 6)
                + (((p2) & 1) << 5) + (((p3) & 1) << 4)
                + (((p0 >> 1) & 1) << 3) + (((p1 >> 1) & 1) << 2)
                + (((p2 >> 1) & 1) << 1) + (((p3 >> 1) & 1)));

            return v;
        }

        public bool WritePalette(string CPCBitmapFilename, int[] gateArrayData, ushort[] asicGata)
        {
            try
            {
                var bytes = new List<byte>();

                for (int iPen = 0; iPen < 4; iPen++)
                {
                    var iGateArrayData = gateArrayData[iPen]; 
                    var index = (byte)iGateArrayData;
                    bytes.Add(index);
                }

                // Padding
                for (int iPen = 0; iPen < 16-4; iPen++)
                {
                    bytes.Add(0);
                }

                if (asicGata != null)
                {
                    foreach (var iAsicGata in asicGata)
                    {
                        var palColor = (ushort)iAsicGata;

                        bytes.Add((byte)(palColor & 255));
                        bytes.Add((byte)(palColor >> 8));
                    }
                }

                File.WriteAllBytes(CPCBitmapFilename, bytes.ToArray());
            }
            catch (Exception e)
            {
                Project.App.Controller.Log.Append(e.ToString());
                return false;
            }

            return true;
        }

        private bool WriteFullScreenBitmapPart(string destFilename,
                        bool isTitle,
                        byte maskPenIndex,
                        int width,
                        int height,
                        int[] srcImage,
                        byte startIndex,
                        int r6_start, int r6_height,
                        int reg1, int reg6, int reg9)
        {
            // use 2 first pixels as mask
            var maskPixel1 = (byte)srcImage[0];
            var maskPixel2 = (byte)srcImage[1];
            var maskPixel3 = (byte)srcImage[3];
            var maskPixel4 = (byte)srcImage[4];

            var cpcBuffer = new byte[0x16000];

            ushort cpcOffset = (ushort)0;

            for (int r6 = r6_start; r6 < r6_start + r6_height; r6++)
            {
                for (int r9 = 0; r9 <= reg9; r9++)
                {
                    for (int r1 = 0; r1 < reg1 * 2; r1++)
                    {
                        cpcOffset = (ushort)((r6 - r6_start) * (reg1 * 2));
                        cpcOffset += (ushort)(0x800 * r9);
                        cpcOffset += (ushort)r1;

                        int srcOffset = r6 * ((reg1 * 4 * 2) * (reg9 + 1));
                        srcOffset += r9 * (reg1 * 4 * 2);
                        srcOffset += r1 * 4;

                        if (srcOffset >= (width * height))
                        {
                            continue;
                        }

                        var pixel1 = (byte)(srcImage[srcOffset]);
                        var pixel2 = (byte)(srcImage[srcOffset + 1]);
                        var pixel3 = (byte)(srcImage[srcOffset + 2]);
                        var pixel4 = (byte)(srcImage[srcOffset + 3]);

                        if (isTitle)
                        {
                            if (pixel1 == maskPixel1)
                            {
                                pixel1 = maskPenIndex;
                            }
                            else
                            {
                                pixel1 += startIndex;
                            }
                            if (pixel2 == maskPixel2)
                            {
                                pixel2 = maskPenIndex;
                            }
                            else
                            {
                                pixel2 += startIndex;
                            }
                            if (pixel3 == maskPixel3)
                            {
                                pixel3 = maskPenIndex;
                            }
                            else
                            {
                                pixel3 += startIndex;
                            }
                            if (pixel4 == maskPixel4)
                            {
                                pixel4 = maskPenIndex;
                            }
                            else
                            {
                                pixel4 += startIndex;
                            }
                        }
                        else
                        {
                            pixel1 += startIndex;
                            pixel2 += startIndex;
                            pixel3 += startIndex;
                            pixel4 += startIndex;
                        }

                        var pixelCPC = ConvertPixelCPC_Mode1(pixel1, pixel2, pixel3, pixel4);

                        cpcBuffer[cpcOffset] = pixelCPC;
                    }
                }
            }

            var allBytes = cpcBuffer.Take(cpcOffset + 1).ToArray();
            File.WriteAllBytes(destFilename, allBytes);

            return true;
        }

        public bool WriteFullscreenBitmap(string destTopFilename,
                        string destBottomFilename,
                        bool isTitle,
                        byte maskPenIndex,
                        int width,
                        int height,
                        int[] srcImage,
                        byte startIndex)
        {
            try
            {
                var global = Project.App.Controller.Entities.GlobalSettings;
                var reg1 = global.CRTC_Register1;
                var reg6 = global.CRTC_Register6;
                var reg9 = global.CRTC_Register9;

                if (!WriteFullScreenBitmapPart(destTopFilename, isTitle, maskPenIndex, width, height, srcImage, startIndex,
                        0,
                        Project.App.Controller.Entities.GlobalSettings.Screen1_TopHeightInChars,
                        reg1, reg6, reg9))
                {
                    return false;
                }

                if (!WriteFullScreenBitmapPart(destBottomFilename, isTitle, maskPenIndex, width, height, srcImage, startIndex,
                        Project.App.Controller.Entities.GlobalSettings.Screen1_TopHeightInChars,
                        Project.App.Controller.Entities.GlobalSettings.Screen1_BottomHeightInChars,
                        reg1, reg6, reg9))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Project.App.Controller.Log.Append(e.ToString());
                return false;
            }

            return true;
        }

        // NOT IMPLEMENTED FROM HERE ------------------------------------------------------

        public int GetLastWidthBytesBitplan1()
        {
            return 0;
        }
        public int GetLastWidthBytesBitplan2()
        {
            return 0;
        }
        public int GetLastHeight()
        {
            return 0;
        }

        public bool WriteSprite(string spriteBitplan1Filename,
                            string spriteBitplan2Filename,
                            int width,
                            int height,
                            int[] srcImage,
                            int[] srcBGImage,
                            int spriteType,
                            bool useMask,
                            int[] srcMaskImage,
                            int startIndex,
                            int posX,
                            int posY,
                            bool isCloudSprite,
                            string cloudSpriteBitplan1Filename,
                            byte[] srcCloudImage,
                            int cloudWidth,
                            int cloudHeight,
                            int[] srcDiffImage
                            )
        {
            if (spriteType == 0) // SPRITE_RAWDATA
            {
                // partial support for MODE 1
                bool ok = WriteSpriteRawData(spriteBitplan1Filename, width, height, srcImage, startIndex);
                WriteSpriteRawData(spriteBitplan2Filename, width, height, srcImage, startIndex);
                return ok;
            }
            else if (spriteType == 4) // SPRITE_FULLSCREEN
            {
                var global = Project.App.Controller.Entities.GlobalSettings;
                var reg1 = global.CRTC_Register1;
                var reg6 = global.CRTC_Register6;
                var reg9 = global.CRTC_Register9;

                if (!WriteFullScreenBitmapPart(spriteBitplan1Filename, false, 0, width, height, srcImage, 0,
                        0,
                        Project.App.Controller.Entities.GlobalSettings.Screen1_TopHeightInChars,
                        reg1, reg6, reg9))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        private bool WriteSpriteRawData(    string filename,
                                            int width,
                                            int height,
                                            int[] srcImage,
                                            int startIndex)
        {
            try
            {
                var cpcBuffer = new byte[width * height / 4];
                int cpcOffset = 0;

                for (int y = 0; y < height; y++) 
                {
                    for (int x = 0; x < width; x += 4)
                    {
                        var srcOffset = (y * width) + x;

                        var pixel1 = (byte)(srcImage[srcOffset]);
                        var pixel2 = (byte)(srcImage[srcOffset + 1]);
                        var pixel3 = (byte)(srcImage[srcOffset + 2]);
                        var pixel4 = (byte)(srcImage[srcOffset + 3]);

                        var pixelCPC = ConvertPixelCPC_Mode1(pixel1, pixel2, pixel3, pixel4);

                        cpcBuffer[cpcOffset] = pixelCPC;
                        cpcOffset++;
                    }
                }

                var allBytes = cpcBuffer.Take(cpcOffset + 1).ToArray();
                File.WriteAllBytes(filename, allBytes);
            }
            catch (Exception e)
            {
                Project.App.Controller.Log.Append(e.ToString());
                return false;
            }

            return true;
        }

        public bool WriteFont(string fontFilename,
                        bool fontAlignOnCharaterLine,
                        int fontCharWidthInBytes,
                        int width,
                        int height,
                        int[] srcImage,
                        int startIndex)
        {
            try
            {
                var cpcBuffer = new byte[width*height/4];
                int cpcOffset = 0;

                var orderedY = new List<int>(8);
                if (!fontAlignOnCharaterLine)
                {
                    orderedY.Add(0);
                    orderedY.Add(1);
                    orderedY.Add(2);
                    orderedY.Add(3);
                    orderedY.Add(4);
                    orderedY.Add(5);
                    orderedY.Add(6);
                    orderedY.Add(7);
                }
                else
                {
// 000 line 0
// 001 line 1 / set 3,h
// 011 line 3 / set 4,h
// 010 line 2 / res 3,h
// 110 line 6 / set 5,h
// 100 line 4 / res 4,h
// 101 line 5 / set 3,h
// 111 line 7 / set 4,h
                    orderedY.Add(0);
                    orderedY.Add(1);
                    orderedY.Add(3);
                    orderedY.Add(2);
                    orderedY.Add(6);
                    orderedY.Add(4);
                    orderedY.Add(5);
                    orderedY.Add(7);
                }

                for (int x = 0; x < width; x += 4 * fontCharWidthInBytes)
                {
                    foreach(var y in orderedY)
                    {
                        for (int iPixelInChar = 0; iPixelInChar < fontCharWidthInBytes; iPixelInChar++)
                        {
                            var srcOffset = (y * width) + x + (iPixelInChar*4);

                            var pixel1 = (byte)(srcImage[srcOffset]);
                            var pixel2 = (byte)(srcImage[srcOffset + 1]);
                            var pixel3 = (byte)(srcImage[srcOffset + 2]);
                            var pixel4 = (byte)(srcImage[srcOffset + 3]);

                            var pixelCPC = ConvertPixelCPC_Mode1(pixel1, pixel2, pixel3, pixel4);

                            cpcBuffer[cpcOffset] = pixelCPC;
                            cpcOffset++;
                        }
                    }
                }

                var allBytes = cpcBuffer.Take(cpcOffset + 1).ToArray();
                File.WriteAllBytes(fontFilename, allBytes);
            }
            catch (Exception e)
            {
                Project.App.Controller.Log.Append(e.ToString());
                return false;
            }

            return true;
        }
    }
}
