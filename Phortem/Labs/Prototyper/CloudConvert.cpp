#include <Windows.h>
#include <stdio.h>

void CloudConvert(char *srcFilename, char *dstFilename)
{
	FILE *fileSrc = fopen(srcFilename, "rb");
	FILE *fileDst = fopen(dstFilename, "wb");

	for ( int y = 0; y < 52; y++ )
	{
		for ( int x = 0; x < 48; x++ )
		{
			unsigned char pixel1;
			fread( &pixel1, 1, 1, fileSrc );

			unsigned char pixel2;
			fread( &pixel2, 1, 1, fileSrc );

			unsigned char write = ( pixel1 << 4 ) + pixel2;
			fwrite( &write, 1, 1, fileDst );
		}
	}
	
	fclose(fileDst);
	fclose(fileSrc);
}

void CloudConvert()
{
	CloudConvert("MangaCloudTile.raw","MangaCloudTile.raw.cloud");
	CloudConvert("MangaCloudTitle.raw","MangaCloudTitle.raw.cloud");	
}