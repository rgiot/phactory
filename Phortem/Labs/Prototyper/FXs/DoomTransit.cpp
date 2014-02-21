
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "DoomTransit.h"

static FILE *fileWrite;

DoomTransit::DoomTransit() :
	FXBase()
{	
}

DoomTransit::~DoomTransit()
{
}

SDLKey DoomTransit::GetKey()
{
	return SDLK_2;
}

const char *DoomTransit::GetName()
{
	return "DoomTransit";
}

void DoomTransit::Init()
{
	FILE *file = fopen( "..\\..\\winape\\src\\oblicTransit\\OblicTransitSin.bin", "wb" );

	int tableSize = 205;
	unsigned char *raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 6.0f;
		float v = sin(rad*4)*radius;
				
		v = (radius*0.5f) + (v*0.5f);

		unsigned char finalV = (unsigned char) v;
		finalV += 0;
		
		raw[i] = finalV;
	}
	// 4,0,1,3,2
	for( int i = 0; i < 205; i+=5 )
	{
		unsigned char v0 = raw[i+4];
		unsigned char v1 = raw[i+0];
		unsigned char v2 = raw[i+1];
		unsigned char v3 = raw[i+3];
		unsigned char v4 = raw[i+2];

		raw[i] = v0;
		raw[i+1] = v1;
		raw[i+2] = v2;
		raw[i+3] = v3;
		raw[i+4] = v4;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;
	
	file = fopen( "..\\..\\winape\\src\\doomTransit\\DoomTransitSin.bin", "wb" );

	tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 127.0f;
		float v = sin(rad)*radius;
				
		v = (radius*0.5f) + (v*0.5f);

		unsigned char finalV = (unsigned char) v;
		finalV += 0;

		raw[i] = finalV;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;

	file = fopen( "..\\..\\winape\\src\\doomTransit2\\DoomTransitSin1.bin", "wb" );

	tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 127.0f;
		float v = sin(rad)*radius;
				
		v = (radius*0.5f) + (v*0.5f);

		unsigned char finalV = (unsigned char) v;
		finalV += 0;

		raw[i] = finalV;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;

	file = fopen( "..\\..\\winape\\src\\doomTransit2\\DoomTransitSin2.bin", "wb" );

	tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 127.0f;
		float v = cos(rad * 4)*radius;
				
		v = (radius*0.5f) + (v*0.5f);

		unsigned char finalV = (unsigned char) v;
		finalV += 0;

		raw[i] = finalV;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;
}

void DoomTransit::Draw()
{
}

void DoomTransit::Destroy()
{
}