using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Phactory.Modules.Cloud.Compiler
{
    public class CloudCompiler
    {
        private List<byte> CreateCloudImage( byte[] srcCloudImage, int width, int height )
		{
            var cloudImage = new List<byte>();

			int i = -1;
			do
			{
				i++;
				for ( int y = 0; y < height; y++ )
				{
					for ( int x = 0; x < width; x++ )
					{
						int texelY = y;
						int texelX = x;
                        byte texel = srcCloudImage[(texelY * width) + texelX];

						if ( texel == (byte)i )
						{
                            cloudImage.Add((byte)x);
						}
					}
				}
			} while(i!=255);

			i = -1;
			do
			{
				i++;
				for ( int y = 0; y < height; y++ )
				{
					for ( int x = 0; x < width; x++ )
					{
						int texelY = y;
						int texelX = x;
                        byte texel = srcCloudImage[(texelY * width) + texelX];

						if ( texel == i )
						{
                            cloudImage.Add((byte)(y * 2));
						}
					}
				}
			} while(i!=255);

            return cloudImage;
		}

        public bool WriteCloudBitmap(string destFilename0080_4000, byte[] cloudMaskData, int width, int height)
		{
            try
            {
                var cloudImage = CreateCloudImage(cloudMaskData, width, height);

                File.WriteAllBytes(destFilename0080_4000, cloudImage.ToArray());
            }
            catch
            {
                return false;
            }
			
			return true;
		}
    }
}
