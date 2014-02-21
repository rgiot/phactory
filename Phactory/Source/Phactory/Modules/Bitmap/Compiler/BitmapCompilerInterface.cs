using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPCBitmap.Controller
{
    public interface BitmapCompilerInterface
    {
        int GetLastWidthBytesBitplan1();
        int GetLastWidthBytesBitplan2();
        int GetLastHeight();

        bool WritePalette(string CPCBitmapFilename, int[] gateArrayData, ushort[] asicGata);

        bool WriteFullscreenBitmap(string destTopFilename,
                        string destBottomFilename,
                        bool isTitle,
                        byte maskPenIndex,
                        int width,
                        int height,
                        int[] srcImage,
                        byte startIndex);

        bool WriteSprite(string spriteBitplan1Filename,
                            string spriteBitplan2Filename,
                            int width,
                            int height,
                            int[] srcImage,
                            int[] srcBGImage,
                            int spriteType, // 0=SPRITE_RAWDATA, 1= SPRITE_DATA, 2=SPRITE_OPCODES, 3=SPRITE_SCREENDATA, 4=SPRITE_FULLSCREEN
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
                            );

        bool WriteFont( string fontFilename,
                        bool fontAlignOnCharaterLine,
                        int fontCharWidthInBytes,
                        int width,
                        int height,
                        int[] srcImage, 
                        int startIndex);
    }
}
