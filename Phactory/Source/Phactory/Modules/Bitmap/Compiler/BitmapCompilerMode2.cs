using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Project;

namespace CPCBitmap.Controller
{
    public class BitmapCompilerMode2 : BitmapCompilerInterface
    {
        private byte ConvertPixelCPC_Mode2(byte p0, byte p1)
        {
            byte v;
            v = (byte)(
                    (((p0) & 1) << 7) + (((p1) & 1) << 6)
                + (((p0 >> 2) & 1) << 5) + (((p1 >> 2) & 1) << 4)
                + (((p0 >> 1) & 1) << 3) + (((p1 >> 1) & 1) << 2)
                + (((p0 >> 3) & 1) << 1) + (((p1 >> 3) & 1)));

            return v;
        }

        private byte ConvertPixelCPC_Mode2_TODO(byte p0, byte p1, byte p2, byte p3, byte p4, byte p5, byte p6, byte p7)
        {
            byte v;
            v = (byte)(
                    (((p0) & 1) << 7) + (((p1) & 1) << 6)
                + (((p2) & 1) << 5) + (((p3) & 1) << 4)
                + (((p4) & 1) << 3) + (((p5) & 1) << 2)
                + (((p6) & 1) << 1) + (((p7) & 1)));

            return v;
        }

        public bool WritePalette(string CPCBitmapFilename, int[] gateArrayData, ushort[] asicGata)
        {
            try
            {
                var bytes = new List<byte>();

                for(int iPen = 0; iPen<2; iPen++)
                {
                    var iGateArrayData = gateArrayData[ iPen ];
                    var index = (byte)iGateArrayData;
                    bytes.Add(index);
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
                        int r6_start, int r6_height)
        {
            var global = Project.App.Controller.Entities.GlobalSettings;
            var reg1 = global.CRTC_Register1;
            var reg6 = global.CRTC_Register6;
            var reg9 = global.CRTC_Register9;

            // use 2 first pixels as mask
            var maskPixel1 = (byte)srcImage[0];
            var maskPixel2 = (byte)srcImage[1];

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

                        int srcOffset = r6 * ((reg1 * 4) * (reg9 + 1));
                        srcOffset += r9 * (reg1 * 4);
                        srcOffset += r1 * 2;

                        if (srcOffset >= (width * height))
                        {
                            continue;
                        }

                        var pixel1 = (byte)(srcImage[srcOffset]);
                        var pixel2 = (byte)(srcImage[srcOffset + 1]);

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
                        }
                        else
                        {
                            pixel1 += startIndex;
                            pixel2 += startIndex;
                        }

                        var pixelCPC = ConvertPixelCPC_Mode2(pixel1, pixel2);

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

                if (!WriteFullScreenBitmapPart(destTopFilename, isTitle, maskPenIndex, width, height, srcImage, startIndex,
                        0,
                        Project.App.Controller.Entities.GlobalSettings.Screen1_TopHeightInChars))
                {
                    return false;
                }

                if (!WriteFullScreenBitmapPart(destBottomFilename, isTitle, maskPenIndex, width, height, srcImage, startIndex,
                        Project.App.Controller.Entities.GlobalSettings.Screen1_TopHeightInChars,
                        Project.App.Controller.Entities.GlobalSettings.Screen1_BottomHeightInChars))
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
            return false;
        }

        public bool WriteFont(string fontFilename,
                        bool fontAlignOnCharaterLine,
                        int fontCharWidthInBytes,
                        int width,
                        int height,
                        int[] srcImage,
                        int startIndex)
        {
            // not supported yet
            return false;
        }
    }
}
