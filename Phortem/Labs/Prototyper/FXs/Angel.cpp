
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Angel.h"

static FILE *fileWrite;
extern void GenerateScroll();

Angel::Angel() :
	FXBase()
{
	GenerateScroll();

	FILE *file = fopen("Angel_384x205.raw", "rb" );
	raw1 = new unsigned char[384*205*3];
	fread(raw1, 1, 384*205*3, file);
	fclose(file);

	file = fopen("AngelAsDemon_384x205.raw", "rb" );
	raw2 = new unsigned char[384*205*3];
	fread(raw2, 1, 384*205*3, file);
	fclose(file);

	file = fopen("AngelWobblerMask.raw", "rb" );
	wobblerRaw = new unsigned char[384*205];
	fread(wobblerRaw, 1, 384*205, file);
	fclose(file);

	fileWrite = fopen("AngelPosX.raw", "wb" );

	frameSinPos = 1.66;
}

Angel::~Angel()
{
}

SDLKey Angel::GetKey()
{
	return SDLK_2;
}

const char *Angel::GetName()
{
	return "Angel";
}

void Angel::Init()
{
}

void Angel::Draw()
{
	DrawImage();
}

static int screenshotCounter = 1;

extern SDL_Surface *s_sdlSurface;


static void DoScreenshot()
{
	char screenshotName[64];
    sprintf(screenshotName, "AngelAnim%04d.bmp", screenshotCounter);
	screenshotCounter++;

	/*Uint32 rmask, gmask, bmask, amask;
	rmask = 0x000000ff;
    gmask = 0x0000ff00;
    bmask = 0x00ff0000;
    amask = 0xff000000;
	SDL_Surface *surf = SDL_CreateRGBSurface(SDL_SWSURFACE, 384, 205, 32,
                                   rmask, gmask, bmask, amask);

	SDL_LockSurface( s_sdlSurface );
	SDL_LockSurface( surf );

	unsigned int *dstPixels = (unsigned int *)surf->pixels;
	unsigned int *srcPixels = (unsigned int *)s_sdlSurface->pixels;

	for ( int y = 0; y < 205; y++ )
	{
		for ( int x = 0; x < 384; x++ )
		{
			dstPixels[ (y*384) + x ] = srcPixels[ ( (y*2)*384 ) + ( x * 2 ) ];
		}
	}

	SDL_UnlockSurface( surf );

	SDL_UnlockSurface( s_sdlSurface );*/

    //SDL_SaveBMP(s_sdlSurface, screenshotName);

	//SDL_FreeSurface(surf);
}

void Angel::CreateWobblerScanline(int y)
{
	float sinPos = frameSinPos + ((float)y)*0.001f;

	if ( y == 0 )
	{
		unsigned short posX = (int)(sin(sinPos)*192.0f);
		fwrite(&posX, 2, 1, fileWrite);
	}

	for ( int x = 0; x < 192; x++ )
	{
		wobblerScanline[x*2] = 0;
		wobblerScanline[x*2+1] = 0;
	}

	for ( int x = 0; x < 192; x++ )
	{
		unsigned char mask = wobblerRaw[x*2];
		if ( mask == 0x0f ) 
		{
			mask = 255;
		}
		else
		{
			mask = 0;
		}

		int calcX = x + (int)(sin(sinPos)*192.0f);

		if ( (calcX>=0) && (calcX<192) )
		{
			wobblerScanline[calcX*2] = mask;
			wobblerScanline[(calcX*2)+1] = mask;
		}
	}
}

void Angel::DrawImage()
{
	unsigned char *pRaw1 = raw1;
	unsigned char *pRaw2 = raw2;

	frameSinPos += 0.01f;

	if ( frameSinPos >= 4.6 )
	{
		frameSinPos = 0;
		
		fclose(fileWrite);
		exit(0);
	}
	
	for( int y = 0; y < 205; y++ )
	{
		CreateWobblerScanline(y);
		unsigned char *pWobblerRaw = wobblerScanline;

		for( int x = 0; x < 192; x++ )
		{
			unsigned int colorRaw1 = 0;			
			colorRaw1+=(*pRaw1<<16);
			pRaw1++;
			colorRaw1+=(*pRaw1<<8);
			pRaw1++;
			colorRaw1+=(*pRaw1);
			pRaw1++;			
			pRaw1 += 3;

			unsigned int colorRaw2 = 0;			
			colorRaw2+=(*pRaw2<<16);
			pRaw2++;
			colorRaw2+=(*pRaw2<<8);
			pRaw2++;
			colorRaw2+=(*pRaw2);
			pRaw2++;
			pRaw2 += 3;

			unsigned char mask = *pWobblerRaw;
			pWobblerRaw += 2;
			
			if ( mask != 0x0 )
			{
				putPixel(x, y, colorRaw1);
			}
			else
			{
				putPixel(x, y, colorRaw2);
			}
		}
	}

	DoScreenshot();
}

void Angel::Destroy()
{
}