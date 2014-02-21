using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Phactory.Modules.RawBitmap.Compiler
{
    public class RawCompiler
    {
        public bool WriteRawBinFile(string destTopFilename, int width, int height, int[] data, bool isVertical)
		{
            try
            {
                var srcImage = new List<byte>();

                if (isVertical)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            byte palIndex = (byte)(data[(y * width) + x]);
                            srcImage.Add(palIndex);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < width * height; i++)
                    {
                        byte palIndex = (byte)data[i];
                        srcImage.Add(palIndex);
                    }
                }

                File.WriteAllBytes(destTopFilename, srcImage.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
		}
    }
}
