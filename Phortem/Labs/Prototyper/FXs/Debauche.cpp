
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"

#include "Debauche.h"

static  unsigned int CPCPalette[] = {
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

static  unsigned char ImgPalette[] = { 18, 26, 25, 1, 3, 15, 12, 0, 6, 13, 23, 4, 14, 17, 5, 10 };

Debauche::Debauche() :
	FXBase()
{
	FILE *file = fopen("PlasmaBackground_384x205.raw", "rb" );
	raw = new unsigned char[384*205*3];
	fread(raw, 1, 384*205*3, file);
	fclose(file);

	file = fopen("Highlines.bin", "rb" );
	highLines = new unsigned char[205*2];
	fread(highLines, 1, 205*2, file);
	fclose(file);
}

Debauche::~Debauche()
{
}

SDLKey Debauche::GetKey()
{
	return SDLK_1;
}

const char *Debauche::GetName()
{
	return "Debauche";
}

void Debauche::Init()
{
	FILE *file = fopen("PlasmaSineCurve1.bin", "wb" );
	unsigned char *raw = new unsigned char[256];
	for(int i = 0; i<256; i++)
	{
		float deg = ((((float)i)/256.0f)*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 24.0f;
		float v = (sin(rad)*radius)+radius;

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, 256, file);
	fclose(file);
	delete [] raw;

	file = fopen("PlasmaSineCurve2.bin", "wb" );
	int tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 127.0f;
		float v = sin(rad)*radius;
				
		v = (radius*0.5f) + (v*0.5f);

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;

	file = fopen("PlasmaSineCurve3.bin", "wb" );
	tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 195.0f;
		float v = cos(rad)*radius;

		/*if ( v<0)
		{
			v = -v;
		}*/
		//v = radius - v;

		v = (radius/2.0f) + (v*0.5f);

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
}

void Debauche::Draw()
{
	DrawImage();

	CreateZones();
	WriteResultBitmap();
}

void Debauche::DrawImage()
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

			putPixel(x, y, color);
		}
	}
}

static FILE *file;
static void WriteByte(unsigned char v)
{
	fwrite(&v, 1, 1, file);
}

void Debauche::CreateZones()
{
	file = fopen("PlasmaZones.bin", "wb");

	unsigned char *p = raw;

	unsigned char leftX;
	int x, y;

	unsigned int mask = 0x00FF00;

	for( y = 0; y < 205; y++ )
	{
		leftX = 255;

		for( x = 0; x < 192; x+=2 )
		{
			unsigned int pixel1 = getPixel(x, y);			
			unsigned int pixel2 = getPixel(x+1, y);

			if ( leftX == 255 )
			{
				if ( pixel1 == 0x00FF00 )
				{
					leftX = x;
					WriteByte(x/2);

					putPixel(x, y, mask);
					putPixel(x+1, y, mask);
				}
				else if ( pixel2 == 0x00FF00 )
				{
					leftX = x;
					WriteByte(x/2);

					putPixel(x, y, pixel1);
					putPixel(x+1, y, mask);
				}
				else
				{
					putPixel(x, y, mask);
					putPixel(x+1, y, mask);
				}
			}
			else
			{
				if ( pixel1 != 0x00FF00 )
				{
					WriteByte(192-(x-leftX));
					leftX = 255;

					putPixel(x, y, mask);
					putPixel(x+1, y, mask);
				}
				else if ( pixel2 != 0x00FF00 )
				{
					WriteByte(192-((x+2)-leftX));
					leftX = 255;

					putPixel(x, y, mask);
					putPixel(x+1, y, pixel2);
				}
				else
				{
					putPixel(x, y, mask);
					putPixel(x+1, y, mask);
				}
			}
		}

		if ( leftX != 255 )
		{
			//WriteByte(x);
			WriteByte(192-(x-leftX));
		}

		WriteByte(-1);
	}

	fclose(file);
}

void Debauche::WriteResultBitmap()
{
	FILE *file = fopen("PlasmaBitmapMask.raw", "wb" );

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

void Debauche::Destroy()
{
}