using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Project;

namespace CPCBitmap.Controller
{
    public class BitmapCompilerMode0 : BitmapCompilerInterface
    {
        private byte ConvertPixelCPC_Mode0(byte p0, byte p1)
        {
            byte v;
            v = (byte)(
                    (((p0) & 1) << 7) + (((p1) & 1) << 6)
                + (((p0 >> 2) & 1) << 5) + (((p1 >> 2) & 1) << 4)
                + (((p0 >> 1) & 1) << 3) + (((p1 >> 1) & 1) << 2)
                + (((p0 >> 3) & 1) << 1) + (((p1 >> 3) & 1)));

            return v;
        }

        private byte ConvertPixelCPC_Mode1(byte p0, byte p1, byte p2, byte p3)
        {
            byte v;
            v = (byte)(
                    (((p0 >> 1) & 1) << 7) + (((p1 >> 1) & 1) << 6)
                + (((p2 >> 1) & 1) << 5) + (((p3 >> 1) & 1) << 4)
                + (((p0) & 1) << 3) + (((p1) & 1) << 2)
                + (((p2) & 1) << 1) + (((p3) & 1)));

            return v;
        }

        private byte ConvertPixelCPC_Mode2(byte p0, byte p1, byte p2, byte p3, byte p4, byte p5, byte p6, byte p7)
        {
            byte v;
            v = (byte)(
                    (((p0) & 1) << 7) + (((p1) & 1) << 6)
                + (((p2) & 1) << 5) + (((p3) & 1) << 4)
                + (((p4) & 1) << 3) + (((p5) & 1) << 2)
                + (((p6) & 1) << 1) + (((p7) & 1)));

            return v;
        }

        public int GetLastWidthBytesBitplan1()
        {
            return lastWidthInBytesBitplan1;
        }
        public int GetLastWidthBytesBitplan2()
        {
            return lastWidthInBytesBitplan2;
        }
        public int GetLastHeight()
        {
            return lastHeight;
        }

        public bool WritePalette(string CPCBitmapFilename, int[] gateArrayData, ushort[] asicGata)
        {
            try
            {
                var bytes = new List<byte>();

                for (int iPen = 0; iPen < 16; iPen++)
                {
                    var iGateArrayData = gateArrayData[iPen]; 
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

                        var pixelCPC = ConvertPixelCPC_Mode0(pixel1, pixel2);

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

        private const byte LDEHL = (byte)0x5E;
        private const byte INCL = (byte)0x2C;
        private const byte INCHL = (byte)0x23;
        private const byte LDDHL = (byte)0x56;
        private const byte EXDEHL = (byte)0xEB;
        private const byte ADDHLBC = (byte)0x09;
        private const byte LDHLVAL = (byte)0x36;
        private const byte LDAHL = (byte)0x7E;
        private const byte LDHHL = (byte)0x66;
        private const byte LDLA = (byte)0x6F;
        private const byte LDHLA = (byte)0x77;
        private const byte ANDVAL = (byte)0xE6;
        private const byte XORVAL = (byte)0xEE;
        private const byte ORVAL = (byte)0xF6;
        private const byte ANDD = (byte)0xA2;
        private const byte ANDE = (byte)0xA3;
        private const byte RET = (byte)0xC9;
        private const byte PUSHHL = (byte)0xE5;
        private const byte POPHL = (byte)0xE1;
        private const byte POPBC = (byte)0xC1;
        private const byte PUSHAF = (byte)0xF5;
        private const byte PUSHBC = (byte)0xC5;
        private const byte LDBCNN = (byte)0x01;
        private const byte LDBN = (byte)0x06;
        private const byte LDBA = (byte)0x47;
        private const byte LDCA = (byte)0x4F;
        private const byte LDCVAL = (byte)0x0E;
        private const byte EXAFAF = (byte)0x08;
        private const byte JPIX_1 = (byte)0xDD;
        private const byte JPIX_2 = (byte)0xE9;
        private const byte DI = (byte)0xF3;
        private const byte EI = (byte)0xFB;
        private const byte LDHIX_1 = (byte)0xDD;
        private const byte LDHIX_2 = (byte)0x66;
        private const byte LDHIX_3 = (byte)0x00;
        private const byte LDLIX_1 = (byte)0xDD;
        private const byte LDLIX_2 = (byte)0x6E;
        private const byte LDLIX_3 = (byte)0x00;
        private const byte INCIX_1 = (byte)0xDD;
        private const byte INCIX_2 = (byte)0x23;

        private const byte PIXEL_MASK = (byte)127;

        private const byte SPRITE_RAWDATA = 0;
        private const byte SPRITE_DATA = 1;
        private const byte SPRITE_OPCODES = 2;
        private const byte SPRITE_SCR_DATA = 3;

        private int lastWidthInBytesBitplan1;
        private int lastWidthInBytesBitplan2;
        private int lastHeight;

        private List<byte> TempBuffer;
        private void WriteTempByte(byte b)
        {
            TempBuffer.Add(b);
        }

        private List<byte> BGTempBuffer;
        private void WriteBGTempByte(byte b)
        {
            BGTempBuffer.Add(b);
        }

        private List<byte> DiffTempBuffer;
        private void WriteDiffTempByte(byte b)
        {
            DiffTempBuffer.Add(b);
        }

        private List<byte> Buffer;
        private void WriteByte(byte b)
        {
            Buffer.Add(b);
        }

        string WriteSpriteOutFilename = string.Empty;

        void FlushBuffer(bool dontWriteFile)
        {
            if (!dontWriteFile)
            {
                File.WriteAllBytes(WriteSpriteOutFilename, Buffer.ToArray());
            }

            Buffer = new List<byte>();
        }

        List<ushort> CPCAddresses = new List<ushort>();
        private void InitLinesAddresses()
        {
            int y;

            var global = Project.App.Controller.Entities.GlobalSettings;

            ushort adr = global.Screen1_TopScreenPtr;
            for (y = 0;
                    y < ((global.CRTC_Register9 + 1) * global.Screen1_TopHeightInChars);
                    y += (global.CRTC_Register9 + 1))
            {
                ushort blockAdr = adr;

                for (int y2 = 0; y2 < (global.CRTC_Register9 + 1); y2++)
                {
                    CPCAddresses.Add(blockAdr);

                    blockAdr += 0x800;
                }

                adr += (ushort)(global.CRTC_Register1 * 2);
            }

            adr = global.Screen1_BottomScreenPtr;
            for (y = 0;
                y < ((global.CRTC_Register9 + 1) * global.Screen1_BottomHeightInChars);
                y += (global.CRTC_Register9 + 1))
            {
                ushort blockAdr = adr;

                for (int y2 = 0; y2 < (global.CRTC_Register9 + 1); y2++)
                {
                    CPCAddresses.Add(blockAdr);

                    blockAdr += 0x800;
                }

                adr += (ushort)(global.CRTC_Register1 * 2);
            }
        }

        private ushort GetCPCAddress(int byteX, int y)
        {
            return (ushort)(CPCAddresses[y] + byteX);
        }

        private byte[] CreateCloudImage(byte[] srcCloudImage, int width, int height)
        {
            var cloudBuffer = new byte[width * height * 2];
            int bufferSize = width * height * 2;
            for (int iClear = 0; iClear < bufferSize; iClear++)
            {
                cloudBuffer[iClear] = 0;
            }

            int count = 0;

            byte i = 0;
            unchecked
            {
                i = (byte)-1;
            }
            do
            {
                i++;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int texelY = y;
                        int texelX = x;
                        byte texel = srcCloudImage[(texelY * width) + texelX];

                        if (texel == i)
                        {
                            cloudBuffer[count] = (byte)x;
                            count++;
                            cloudBuffer[count] = (byte)y;
                            count++;
                        }
                    }
                }
            } while (i != 255);

            return cloudBuffer;
        }

        private bool WriteSprite(string spriteBitplanFilename,
                            byte[] srcImage,
                            byte[] srcMaskImage,
                            byte[] srcBGImage,
                            int width,
                            int height,
                            bool secondBitplan,
                            int spriteType, // 0=SPRITE_RAWDATA, 1= SPRITE_DATA, 2=SPRITE_OPCODES, 3=SPRITE_SCR_DATA
                            int startIndex,
                            int posX,
                            int posY,
                            bool isCloudSprite,
                            string cloudSpriteFilename,
                            byte[] srcCloudImage,
                            int cloudWidth,
                            int cloudHeight,
                            byte[] srcDiffImage)
        {
            if (secondBitplan && isCloudSprite)
            {
                // no sprData2 for cloudsprites
                return true;
            }

            try
            {
                Buffer = new List<byte>();
                TempBuffer = new List<byte>();
                BGTempBuffer = new List<byte>();
                DiffTempBuffer = new List<byte>();
                int offset = 0;

                InitLinesAddresses();

                byte[] cloudBuffer = null;
                if (isCloudSprite)
                {
                    cloudBuffer = CreateCloudImage(srcCloudImage, cloudWidth, cloudHeight);
                }

                WriteSpriteOutFilename = spriteBitplanFilename;

                int scrX = 0;
                if (secondBitplan == true)
                {
                    scrX++;
                }

                {
                    byte w = (byte)(width / 2); // /2 = mode 2, 2 pixels per byte

                    if (secondBitplan)
                    {
                        w++;
                        lastWidthInBytesBitplan2 = w;
                    }
                    else
                    {
                        lastWidthInBytesBitplan1 = w;
                    }
                    lastHeight = height;
                }

                int y;
                for (y = 0; y < height; y++)
                {
                    offset = Buffer.Count;

                    if (spriteType == SPRITE_OPCODES)
                    {
                        WriteByte(LDAHL);
                        WriteByte(INCHL);
                        WriteByte(PUSHHL);
                        WriteByte(LDHHL);
                        WriteByte(LDLA);
                        WriteByte(ADDHLBC);
                        WriteByte(PUSHBC);
                    }

                    int endX = width;

                    if (endX == 1)
                    {
                        endX = 2;
                    }

                    int inc = 2;

                    int x;
                    if (secondBitplan == false)
                    {
                        x = 0;
                    }
                    else
                    {
                        x = -1;
                    }

                    for (; x < endX; x += inc)
                    {
                        //unsigned char *ptr = &srcImage[ ( y * width ) + x ];
                        var index = (y * width) + x;

                        byte pixel1 = 0;
                        byte pixel2 = 0;

                        if (x == -1)
                        {
                            pixel1 = PIXEL_MASK;

                            if ((index + 1) >= (width * height))
                            {
                                pixel2 = PIXEL_MASK;
                            }
                            else
                            {
                                pixel2 = srcImage[index + 1];
                            }
                        }
                        else
                            if ((x + 1) >= endX)
                            {
                                if (index >= (width * height))
                                {
                                    pixel1 = PIXEL_MASK;
                                }
                                else
                                {
                                    pixel1 = srcImage[index];
                                }

                                pixel2 = PIXEL_MASK;
                            }
                            else
                            {
                                if (index >= (width * height))
                                {
                                    pixel1 = PIXEL_MASK;
                                }
                                else
                                {
                                    pixel1 = srcImage[index];
                                }

                                if ((index + 1) >= (width * height))
                                {
                                    pixel2 = PIXEL_MASK;
                                }
                                else
                                {
                                    pixel2 = srcImage[index + 1];
                                }
                            }

                        if (spriteType == SPRITE_SCR_DATA)
                        {
                            byte convertedPixel = ConvertPixelCPC_Mode0((byte)(pixel1 + startIndex), (byte)(pixel2 + startIndex));
                            WriteTempByte(convertedPixel);
                        }

                        if (srcDiffImage != null)
                        {
                            int diffIndex = (y * width) + x;

                            byte diffpixel1 = 0;
                            if (diffIndex == -1)
                            {
                                diffpixel1 = srcDiffImage[0];
                            }
                            else if (diffIndex >= (width * height))
                            {
                                diffpixel1 = srcDiffImage[(width * height) - 1];
                            }
                            else
                            {
                                diffpixel1 = srcDiffImage[diffIndex];
                            }

                            byte diffpixel2 = 0;
                            if ((diffIndex + 1) == width * height)
                            {
                                diffpixel2 = srcDiffImage[diffIndex];
                            }
                            else if ((diffIndex + 1) >= (width * height))
                            {
                                diffpixel2 = srcDiffImage[(width * height) - 1];
                            }
                            else
                            {
                                diffpixel2 = srcDiffImage[diffIndex + 1];
                            }

                            byte diffConvertedPixel = ConvertPixelCPC_Mode0((byte)(diffpixel1 + startIndex), (byte)(diffpixel2 + startIndex));
                            WriteDiffTempByte(diffConvertedPixel);
                        }

                        if (srcBGImage != null)
                        {
                            int bgptrIndex = (y * width) + x;

                            byte bgpixel1 = 0;
                            if (bgptrIndex == -1)
                            {
                                bgpixel1 = srcBGImage[0];
                            }
                            else if (bgptrIndex >= (width * height))
                            {
                                bgpixel1 = srcBGImage[(width * height) - 1];
                            }
                            else
                            {
                                bgpixel1 = srcBGImage[bgptrIndex];
                            }

                            byte bgpixel2 = 0;
                            if ((bgptrIndex + 1) == width * height)
                            {
                                bgpixel2 = srcBGImage[bgptrIndex];
                            }
                            else if ((bgptrIndex + 1) >= (width * height))
                            {
                                bgpixel2 = srcBGImage[(width * height) - 1];
                            }
                            else
                            {
                                bgpixel2 = srcBGImage[bgptrIndex + 1];
                            }
                            byte bgConvertedPixel = ConvertPixelCPC_Mode0((byte)(bgpixel1 + startIndex), (byte)(bgpixel2 + startIndex));
                            WriteBGTempByte(bgConvertedPixel);
                        }

                        if ((pixel1 == PIXEL_MASK) && (pixel2 == PIXEL_MASK))
                        {
                            if ((spriteType == SPRITE_RAWDATA))
                            {
                                WriteByte(ConvertPixelCPC_Mode0(0, 0));
                            }
                        }
                        else if ((pixel1 != PIXEL_MASK) && (pixel2 == PIXEL_MASK))
                        {
                            if (spriteType == SPRITE_OPCODES)
                            {
                                WriteByte(LDAHL);
                                WriteByte(ANDD);
                                WriteByte(ORVAL);
                                WriteByte(ConvertPixelCPC_Mode0((byte)(pixel1 + startIndex), 0));
                                WriteByte(LDHLA);
                            }
                            else if ((spriteType == SPRITE_RAWDATA))
                            {
                                if (isCloudSprite)
                                {
                                    WriteTempByte(ConvertPixelCPC_Mode0((byte)(pixel1 + startIndex), 0));
                                }
                                WriteByte(ConvertPixelCPC_Mode0((byte)(pixel1 + startIndex), 0));
                            }
                            else if (spriteType == SPRITE_DATA)
                            {
                                WriteByte((byte)(x / inc));
                                WriteByte(1);
                                WriteByte(ConvertPixelCPC_Mode0((byte)(pixel1 + startIndex), 0));
                            }
                        }
                        else if ((pixel1 == PIXEL_MASK) && (pixel2 != PIXEL_MASK))
                        {
                            if (spriteType == SPRITE_OPCODES)
                            {
                                WriteByte(LDAHL);
                                WriteByte(ANDE);
                                WriteByte(ORVAL);
                                WriteByte(ConvertPixelCPC_Mode0(0, (byte)(pixel2 + startIndex)));
                                WriteByte(LDHLA);
                            }
                            else if ((spriteType == SPRITE_RAWDATA))
                            {
                                if (isCloudSprite)
                                {
                                    WriteTempByte(ConvertPixelCPC_Mode0(0, (byte)(pixel2 + startIndex)));
                                }

                                WriteByte(ConvertPixelCPC_Mode0(0, (byte)(pixel2 + startIndex)));
                            }
                            else if (spriteType == SPRITE_DATA)
                            {
                                WriteByte((byte)(x / inc));
                                WriteByte(2);
                                WriteByte(ConvertPixelCPC_Mode0(0, (byte)(pixel2 + startIndex)));
                            }
                        }
                        else
                        {
                            byte convertedPixel = ConvertPixelCPC_Mode0((byte)(pixel1 + startIndex), (byte)(pixel2 + startIndex));

                            if (spriteType == SPRITE_OPCODES)
                            {
                                WriteByte(LDHLVAL);
                                WriteByte(convertedPixel);
                            }
                            else if ((spriteType == SPRITE_RAWDATA))
                            {
                                if (isCloudSprite)
                                {
                                    WriteTempByte(convertedPixel);
                                }

                                WriteByte(convertedPixel);
                            }
                            else if (spriteType == SPRITE_DATA)
                            {
                                WriteByte((byte)(128 + (byte)((x / inc))));
                                WriteByte(convertedPixel);
                            }
                        }

                        if (spriteType == SPRITE_OPCODES)
                        {
                            int tempX = x + inc;
                            int tempXEnd = endX;

                            int IncHLCount = 0;
                            bool HasOtherPixels = false;

                            while (tempX < tempXEnd)
                            {
                                int ptrIndex = (y * width) + tempX;

                                if (tempX == -1)
                                {
                                    pixel1 = PIXEL_MASK;
                                    pixel2 = srcImage[ptrIndex + 1];
                                }
                                else
                                    if ((tempX + 1) >= endX)
                                    {
                                        pixel1 = srcImage[ptrIndex];
                                        pixel2 = PIXEL_MASK;
                                    }
                                    else
                                    {
                                        pixel1 = srcImage[ptrIndex];
                                        pixel2 = srcImage[ptrIndex + 1];
                                    }

                                if ((pixel1 == PIXEL_MASK) && (pixel2 == PIXEL_MASK))
                                {
                                    if (!HasOtherPixels)
                                    {
                                        IncHLCount++;
                                    }
                                }
                                else
                                {
                                    HasOtherPixels = true;
                                }

                                tempX += inc;
                            }

                            if (HasOtherPixels)
                            {
                                bool doWriteIncHLCount = false;
                                if (IncHLCount >= 4)
                                {
                                    doWriteIncHLCount = true;
                                }

                                if (doWriteIncHLCount)
                                {
                                    WriteByte(LDCVAL);
                                    WriteByte((byte)(IncHLCount));
                                    WriteByte(ADDHLBC);

                                    x += inc * (IncHLCount - 1);
                                }
                                else
                                {
                                    WriteByte(INCHL);
                                }
                            }
                        }
                    }

                    if (spriteType == SPRITE_OPCODES)
                    {
                        WriteByte(POPBC);
                        WriteByte(POPHL);
                        WriteByte(INCHL);
                    }

                    if (spriteType == SPRITE_DATA)
                    {
                        unchecked
                        {
                            WriteByte((byte)(-1));
                        }
                    }
                }

                offset = Buffer.Count;

                if (spriteType == SPRITE_OPCODES)
                {
                    WriteByte(RET);
                    WriteByte(RET);

                    WriteByte(RET);
                    WriteByte(RET);
                }

                var values = new List<ushort>();
                var pixels = new List<byte>();

                if ((spriteType == SPRITE_SCR_DATA) && (!secondBitplan))
                {
                    int w2 = width / 2;

                    for (ushort iSH = 0; iSH < 256; iSH++)
                    {
                        byte iH = (byte)iSH;

                        for (int iY = 0; iY < height; iY++)
                        {
                            for (int x = 0; x < w2; x++)
                            {
                                byte srcPixel = TempBuffer[(iY * w2) + x];
                                byte diffPixel = DiffTempBuffer[(iY * w2) + x];
                                if (srcPixel != diffPixel)
                                {
                                    ushort screenPtr = GetCPCAddress(x + posX, iY + posY);
                                    if ((screenPtr >> 8) == iH)
                                    {
                                        ushort v = (byte)(screenPtr & 255);
                                        v <<= 8;
                                        v += srcPixel;
                                        values.Add(v);
                                    }
                                }
                            }
                        }

                        if (values.Count != 0)
                        {
                            byte lastX = 0;
                            unchecked
                            {
                                lastX = (byte)-10;
                            }
                            byte firstX = (byte)(values[0] >> 8);
                            bool isFirst = true;

                            pixels.Clear();

                            WriteByte(0x00);
                            WriteByte(iH);
                            if (iH >= 0x40)
                            {
                                isFirst = true;
                            }
                            for (int i = 0; i < values.Count; i++)
                            {
                                ushort v = values[i];
                                byte x = (byte)(v >> 8);
                                byte srcPixel = (byte)(v & 255);

                                if (isFirst)
                                {
                                    isFirst = false;
                                }
                                else
                                {
                                    if (x != (lastX + 1))
                                    {
                                        WriteByte((byte)(pixels.Count));
                                        WriteByte(firstX);
                                        for (int j = 0; j < pixels.Count; j++)
                                        {
                                            WriteByte(pixels[j]);
                                        }

                                        pixels.Clear();
                                        firstX = x;
                                    }
                                }

                                pixels.Add(srcPixel);

                                lastX = x;
                            }

                            if (pixels.Count != 0)
                            {
                                WriteByte((byte)(pixels.Count));
                                WriteByte(firstX);
                                for (int j = 0; j < pixels.Count; j++)
                                {
                                    WriteByte(pixels[j]);
                                }
                            }

                            values.Clear();
                        }
                    }

                    WriteByte(0xFF);
                }

                if (isCloudSprite && (!secondBitplan))
                {
                    // write .sprdata
                    FlushBuffer(true);

                    int lastWidth = lastWidthInBytesBitplan1;

                    int cloudSize = cloudWidth * cloudHeight * 2;

                    WriteSpriteOutFilename = cloudSpriteFilename;

                    byte texel1 = 0;
                    byte bgtexel1 = 0;
                    ushort screenPtr1 = 0;
                    byte texel2 = 0;
                    byte bgtexel2 = 0;
                    ushort screenPtr2 = 0;

                    bool second = false;

                    for (int i = 0; i < cloudSize; i += 2)
                    {
                        byte cloudX = cloudBuffer[i];
                        byte cloudY = cloudBuffer[i + 1];

                        if (!second)
                        {
                            bool isMaskWrite1 = (srcMaskImage[(cloudY * width) + (cloudX * 2)] != 0)
                                                || (srcMaskImage[(cloudY * width) + ((cloudX * 2) + 1)] != 0);
                            if (isMaskWrite1)
                            {
                                texel1 = TempBuffer[(cloudY * lastWidth) + cloudX];
                                bgtexel1 = BGTempBuffer[(cloudY * lastWidth) + cloudX];
                                screenPtr1 = GetCPCAddress(cloudX + posX, cloudY + posY);
                                second = true;
                            }
                        }
                        else
                        {
                            bool isMaskWrite2 = (srcMaskImage[(cloudY * width) + (cloudX * 2)] != 0)
                                                || (srcMaskImage[(cloudY * width) + ((cloudX * 2) + 1)] != 0);
                            if (isMaskWrite2)
                            {
                                texel2 = TempBuffer[(cloudY * lastWidth) + cloudX];
                                bgtexel2 = BGTempBuffer[(cloudY * lastWidth) + cloudX];
                                screenPtr2 = GetCPCAddress(cloudX + posX, cloudY + posY);

                                WriteByte(texel1);
                                WriteByte(texel2);
                                WriteByte(bgtexel1);
                                WriteByte(bgtexel2);

                                WriteByte((byte)(screenPtr1 & 255));
                                WriteByte((byte)(screenPtr1 >> 8));
                                WriteByte((byte)(screenPtr2 & 255));
                                WriteByte((byte)(screenPtr2 >> 8));

                                second = false;
                            }
                        }
                    }
                }

                FlushBuffer(false);
            }
            catch (Exception e)
            {
                Project.App.Controller.Log.Append(e.ToString());
                return false;
            }

            return true;
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
            var bSrcImage = new byte[width * height];
            for (int i = 0; i < width * height; i++)
            {
                bSrcImage[i] = (byte)srcImage[i];
            }

            var bSrcBGImage = new byte[width * height];
            if (srcBGImage != null)
            {
                for (int i = 0; i < width * height; i++)
                {
                    bSrcBGImage[i] = (byte)srcBGImage[i];
                }
            }

            var bSrcMaskImage = new byte[width * height];
            for (int i = 0; i < width * height; i++)
            {
                bSrcMaskImage[i] = (byte)srcMaskImage[i];
            }

            var bSrcDiffImage = new byte[width * height];
            if (srcDiffImage != null)
            {
                for (int i = 0; i < width * height; i++)
                {
                    bSrcDiffImage[i] = (byte)srcDiffImage[i];
                }
            }

            if (!WriteSprite(spriteBitplan1Filename, bSrcImage, bSrcMaskImage, bSrcBGImage, width, height, false, spriteType, startIndex, posX, posY, isCloudSprite, cloudSpriteBitplan1Filename, srcCloudImage, cloudWidth, cloudHeight, bSrcDiffImage))
            {
                return false;
            }

            if (!WriteSprite(spriteBitplan2Filename, bSrcImage, bSrcMaskImage, bSrcBGImage, width, height, true, spriteType, startIndex, posX, posY, isCloudSprite, String.Empty, srcCloudImage, cloudWidth, cloudHeight, bSrcDiffImage))
            {
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
            // not supported yet
            return false;
        }
    }
}
