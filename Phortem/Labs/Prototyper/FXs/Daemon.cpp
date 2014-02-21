
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Daemon.h"

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


Daemon::Daemon() :
	FXBase()
{
	MovePreca();

	FILE *file = fopen("DaemonCircle_384x205.raw", "rb" );
	raw = new unsigned char[384*205*3];
	fread(raw, 1, 384*205*3, file);
	fclose(file);

	file = fopen("Highlines.bin", "rb" );
	highLines = new unsigned char[205*2];
	fread(highLines, 1, 205*2, file);
	fclose(file);


	file = fopen("SineCurveZoomBounce.bin", "wb" );
	char tableSize = 64;
	unsigned char *raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 77.0f;
		float v = cos(rad)*radius;

		if ( v<0)
		{
			v = -v;
		}

		v = radius - v;

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
}

Daemon::~Daemon()
{
}

SDLKey Daemon::GetKey()
{
	return SDLK_2;
}

const char *Daemon::GetName()
{
	return "Daemon";
}

void Daemon::Init()
{
}

void Daemon::Draw()
{
	DrawImage();
	
	CreateCircleOutBounds();
	CreateCircleInBounds();
}

void Daemon::DrawImage()
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

static FILE *file;
static void WriteByte(unsigned char value)
{
	fwrite(&value, 1, 1, file);
}

int Daemon::GetLeftOutX(unsigned char *p)
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

		if ( color != 0x00FF00 )
		{
			return x;
		}
	}

	return -1;
}

int Daemon::GetRightOutX(unsigned char *p)
{
	p += 191*6;
			
	for( int x = 191; x >= 0; x-- )
	{
		unsigned int color = 0;

		color+=(*p<<16);
		p++;

		color+=(*p<<8);
		p++;

		color+=(*p);
		p++;

		p -= 3;
		
		p -= 6;

		if ( color != 0x00FF00 )
		{
			return x;
		}
	}

	return -1;
}

int Daemon::GetLeftInX(unsigned char *p)
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

		if ( color != 0x00FF00 )
		{
			x++;

			for( int x2 = x; x2 < 192; x2++ )
			{
				color = 0;
			
				color+=(*p<<16);
				p++;
				color+=(*p<<8);
				p++;
				color+=(*p);
				p++;
				p += 3;

				if ( color == 0x00FF00 )
				{
					return x2-1;
				}
			}

			return -1;
		}
	}

	return -1;
}

int Daemon::GetRightInX(unsigned char *p)
{
	p += 191*6;
			
	for( int x = 191; x >= 0; x-- )
	{
		unsigned int color = 0;

		color+=(*p<<16);
		p++;

		color+=(*p<<8);
		p++;

		color+=(*p);
		p++;

		p -= 3;
		
		p -= 6;

		if ( color != 0x00FF00 )
		{
			x--;
			for( int x2 = x; x2 >= 0; x2-- )
			{
				color = 0;

				color+=(*p<<16);
				p++;

				color+=(*p<<8);
				p++;

				color+=(*p);
				p++;

				p -= 3;
		
				p -= 6;

				if ( color == 0x00FF00 )
				{
					return x2+1;
				}
			}

			return -1;
		}
	}

	return -1;
}

void Daemon::CreateCircleOutBounds()
{
	unsigned char *p = raw;
	unsigned short saveRamLength = 0;

	file = fopen("CircleOut.bin", "wb" );
	
	for( int y = 0; y < 205; y++ )
	{
		int leftX = GetLeftOutX(p);
		int rightX = GetRightOutX(p);

		if ( (leftX!=-1)&&(rightX!=-1) )
		{
			unsigned char cpcLeftX = leftX/2;
			/*if ( (leftX&1==1) )
			{
				cpcLeftX++;
			}*/
			WriteByte(cpcLeftX);

			unsigned char cpcRightX = rightX/2;
			if ( (rightX&1==1) )
			{
				//cpcRightX++;
			}

			unsigned char cpcLength = cpcRightX-cpcLeftX;
			// WriteByte(cpcLength);

			unsigned char relativeJump = 64-cpcLength;

			relativeJump-=2; // padding

			WriteByte(relativeJump*2);

			saveRamLength += cpcLength;
		}

		p += 192*6;
	}

	WriteByte(255);

	fclose(file);
}

void Daemon::CreateCircleInBounds()
{
	unsigned char *p = raw;
	unsigned short saveRamLength = 0;

	file = fopen("CircleIn.bin", "wb" );
	
	for( int y = 0; y < 205; y++ )
	{
		int leftX = GetLeftInX(p);
		int rightX = GetRightInX(p);

		if ( (leftX!=-1)&&(rightX!=-1) )
		{
			if (leftX<rightX)
			{
				unsigned char cpcLeftX = leftX/2;
				WriteByte(cpcLeftX);

				unsigned char cpcRightX = rightX/2;
				if ( (rightX&1==1) )
				{
					cpcRightX+=1;
				}

				//putPixel(cpcLeftX*2, y, 0xFFFF00);
				//putPixel(cpcRightX*2, y, 0x0000FF);
				
				unsigned char cpcLength = cpcRightX-cpcLeftX;
				
				//putPixel((cpcLeftX+cpcLength)*2, y, 0xFF00FF);

				unsigned char relativeJump = 64-cpcLength;
				relativeJump -= 1; // padding
				WriteByte(relativeJump*2);

				saveRamLength += cpcLength;
			}			
		}

		p += 192*6;
	}

	WriteByte(255);

	fclose(file);
}

void Daemon::Destroy()
{
}

#include "..\Track.h"

Track *g_trackX;
Track *g_trackY;
double g_time;

void Daemon::AddKey(double time, unsigned short x, unsigned short y)
{
	TrackKey xKey;
	xKey.value = x;
	xKey.time = g_time;

	TrackKey yKey;
	yKey.value = y;
	yKey.time = g_time;

	g_trackX->setKey(xKey);
	g_trackY->setKey(yKey);

	g_time += time;
}

void Daemon::MovePreca()
{
	g_trackX = Track::createTrack( TRACK_TYPE_SPLINE );
	g_trackY = Track::createTrack( TRACK_TYPE_SPLINE );

	const int maxPosX = 120;
	const int maxPosY = 333; // 205+128;
	g_time = 0.0f;

	/*AddKey(1.0f, 20, 333);
	AddKey(1.0f, 110, 300);
	AddKey(1.0f, 30, 280);
	AddKey(1.0f, 80, 250);
	AddKey(1.0f, 40, 200);
	AddKey(1.0f, 70, 240);
	AddKey(1.0f, 12, 235);
	AddKey(1.0f, 53, 270);
	AddKey(1.0f, 120, 333);
	AddKey(1.0f, 76, 280);
	AddKey(1.0f, 40, 210);
	AddKey(1.0f, 10, 140);
	AddKey(1.0f, 30, 120);
	AddKey(1.0f, 32, 70);
	AddKey(1.0f, 67, 30);
	AddKey(1.0f, 103, 128);*/
	AddKey(5.0f, 10, 333);
	AddKey(5.0f, 110, 230);
	AddKey(5.0f, 68, 196);
	AddKey(5.0f, 8, 260);
	AddKey(5.0f, 64, 100);
	AddKey(5.0f, 90, 140);
	AddKey(5.0f, 3, 138);
	AddKey(5.0f, 37, 1);
	
	FILE *file = fopen("MoveCircle_XY.bin", "wb" );
	for(double t = 0.0f; t < g_time; t += 0.16f*1.25f )
	{
		unsigned short x = (unsigned short) g_trackX->getValue(t);
		unsigned short y = (unsigned short) g_trackY->getValue(t);

		fwrite(&x, 1, 2, file);
		fwrite(&y, 1, 2, file);
	}
	
	fclose(file);
}