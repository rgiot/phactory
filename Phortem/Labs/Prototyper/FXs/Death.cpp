
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Death.h"

static unsigned int CPCPalette[] = {
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
			0xfdf5f0 };

static unsigned char ImgPalette[] = { 10, 1, 0, 13, 17, 19, 25, 14, 26, 23, 16, 4, 7, 3, 11, 6 };


Death::Death() :
	FXBase()
{
	FILE *file = fopen("Death_384x205.raw", "rb" );
	raw = new unsigned char[384*205*3];
	fread(raw, 1, 384*205*3, file);
	fclose(file);

	file = fopen("Highlines.bin", "rb" );
	highLines = new unsigned char[205*2];
	fread(highLines, 1, 205*2, file);
	fclose(file);
}

Death::~Death()
{
}

SDLKey Death::GetKey()
{
	return SDLK_2;
}

const char *Death::GetName()
{
	return "Death";
}

void Death::Init()
{
}

void Death::Draw()
{
	DrawImage();
	
	//DrawScreenTicks();

	CreateBands();

	WriteResultBitmap();
}

void Death::DrawImage()
{
	unsigned char *p = raw;

	for( int y = 0; y < 205; y++ )
	{
		for( int x = 0; x < 192; x++ )
		{
			unsigned int color = 0;
			
			color+=(*p<<16);
			p++;

			color+=(*p<<8);
			p++;

			color+=(*p);
			p++;

			p += 3;
			
			/*if ( color == 0x00FF00 )
			{
				color = CPCPalette[ImgPalette[4]];
			}*/

			putPixel(x, y, color);
		}
	}
}

void Death::WriteResultBitmap()
{
	FILE *file = fopen("BitmapMask.raw", "wb" );

	for( int y = 0; y < 205; y++ )
	{
		for( int x = 0; x < 192; x++ )
		{
			unsigned int pixel = getPixel(x, y);

			unsigned char b = pixel & 255;
			unsigned char g = (pixel>>8) & 255;
			unsigned char r = (pixel>>16) & 255;
			unsigned char a = (pixel>>24) & 255;

			fwrite(&r, 1, 1, file);
			fwrite(&g, 1, 1, file);
			fwrite(&b, 1, 1, file);

			// irfan view  color ordre 192x205 RGBA 24 bits
		}
	}

	fclose(file);
}

void Death::DrawScreenTicks()
{
	unsigned short *p = (unsigned short*) highLines;

	for( int y = 0; y < 205; y++ )
	{
		unsigned short screenPtr = *p;
		p++;

		for( int x = 0; x < 192; x++ )
		{
			screenPtr++;
			if (( screenPtr & 255 ) == 0 )
			{
				putPixel(x, y, CPCPalette[6]);
				putPixel(x+1, y, CPCPalette[6]);
			}

			if (x==64)
			{
				putPixel(x, y, CPCPalette[14]);
				putPixel(x+1, y, CPCPalette[14]);				
			}
		}
	}
}

void Death::CreateBands()
{
	CreateBand( "BandLeft.bin", 0 );
	CreateBand( "BandCenter.bin", 1 );
	CreateBand( "BandRight.bin", 2 );	
}

void Death::CreateBand(char *filename, int type)
{
	FILE *band = fopen(filename, "wb" );

	unsigned short *p = (unsigned short*) highLines;

	unsigned short *ptrs = new unsigned short[32];
	unsigned short *ptrs2 = new unsigned short[32];
	unsigned char count = 0;
	unsigned char count2 = -1;

	int offset = 0;
	if ( type == 1 )
	{
		offset = 64;
	}
	else if ( type == 2 )	
	{
		offset = 128;
	}

	for( int y = 0; y < 205; y++ )
	{
		count = 0;

		unsigned char lastRightTextureSkip = 0;
		unsigned char lastRightScreenSkip = 0;

		unsigned char lastLeftScreenSkip = 16*4;

		bool rightFirst = true;
		
		for( int x = offset + 64-4; x >=offset ; x-=4 )
		{
			unsigned int pixel1 = getPixel(x, y);
			unsigned int pixel2 = getPixel(x+1, y);
			unsigned int pixel3 = getPixel(x+2, y);
			unsigned int pixel4 = getPixel(x+3, y);

			bool isFX = false;
			if ( pixel1 == 0x00FF00 )
			{
				isFX = true;
			}
			if ( pixel2 == 0x00FF00 )
			{
				isFX = true;
			}
			if ( pixel3 == 0x00FF00 )
			{
				isFX = true;
			}
			if ( pixel4 == 0x00FF00 )
			{
				isFX = true;
			}

			unsigned short ptr = (x-offset) / 4;

			if ( isFX )
			{
				if ( rightFirst )
				{
					rightFirst = false;

					lastRightTextureSkip = -((15-ptr)*2);
					lastRightScreenSkip = (15-ptr)*4;
				}

				if ( type == 2 )
				{
					lastLeftScreenSkip = 16*4;
				}
			}
			else
			{
				if ( type == 2 )
				{
					lastLeftScreenSkip = (15-ptr)*4;
				}

				ptr = 15-ptr; // reversed because stack usage
				ptr *= 4; // 4 bytes for the following code:
				
				/*
				PIXEL ON:
					ld c, (hl)
					dec l
					ld b, (hl)
					dec l	
					push bc

				PIXEL OFF:
					ld c, (hl)
					dec l
					dec sp **
					dec l	
					dec sp **
				*/
				//ptr += 2; // locate to ld b,(hl) opcode
				//ptr += CODESEGMENT8000;

				ptrs[count] = ptr;
				count++;

				/*putPixel(x, y, CPCPalette[26-type*3]);
				putPixel(x+1, y, CPCPalette[26-type*3]);
				putPixel(x+2, y, CPCPalette[26-type*3]);
				putPixel(x+3, y, CPCPalette[26-type*3]);*/

				unsigned int color = 0;
				color+=(*raw<<16);
				color+=(*(raw+1)<<8);
				color+=(*(raw+2));
				putPixel(x, y, color);
				putPixel(x+1, y, color);
				putPixel(x+2, y, color);
				putPixel(x+3, y, color);
			}
		}

		bool isDuplicate = false;
		if ( count2 == count )
		{
			isDuplicate = true;
			for(int i = 0; i < count; i++)
			{
				if ( ptrs[i]!=ptrs2[i] )
				{
					isDuplicate = false;
				}
			}
		}

		if ( count == 16 )
		{
			unsigned char skipLine = 254;
			fwrite(&skipLine, 1, 1, band);		
		}
		else if ( isDuplicate )
		{
			unsigned char duplicate = 255;
			fwrite(&duplicate, 1, 1, band);		
		}
		else
		{
			fwrite(&count, 1, 1, band);
			for(unsigned char i = 0; i < count; i++)
			{
				//unsigned short value = ptrs[i];
				unsigned char value = (unsigned char) ptrs[i];
				fwrite(&value, 1, 1, band);
			}

			if ( type == 2 )
			{
				fwrite(&lastLeftScreenSkip, 1, 1, band);
			}
			else
			{
				fwrite(&lastRightTextureSkip, 1, 1, band);
				fwrite(&lastRightScreenSkip, 1, 1, band);
			}
		}

		memcpy(ptrs2, ptrs, 32*2);
		count2 = count;
	}

	fclose(band);

	delete [] ptrs;
	delete [] ptrs2;
}

void Death::Destroy()
{
}