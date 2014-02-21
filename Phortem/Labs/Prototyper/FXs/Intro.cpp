
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Intro.h"

static unsigned char *bozo;
static unsigned int bozoSize = 0;
static unsigned int nbFrames = 0;
static unsigned int iOffset = 0;
static unsigned int iFrame = 0;
static FILE *outFile;

Intro::Intro() :
	FXBase()
{
	//FILE *file = fopen("Bozo_Encule.tmo", "rb" );
	FILE *file = fopen("Bozo_Bonjour.tmo", "rb" );
	fseek(file, 0L, SEEK_END);
	bozoSize = ftell(file);
	fseek(file, 0L, SEEK_SET);
	bozo = new unsigned char[bozoSize];
	fread(bozo, 1, bozoSize, file);
	fclose(file);

	nbFrames = bozo[0];
	nbFrames += bozo[1]<<8;
	iOffset = 2;

	iOffset = 3;
	iFrame = 0;

	outFile = fopen("BozoBonjour.bin", "wb" );
}

Intro::~Intro()
{
	fclose( outFile );
}

SDLKey Intro::GetKey()
{
	return SDLK_2;
}

const char *Intro::GetName()
{
	return "Intro";
}


void Intro::Init()
{
}


int iCharacter[] = {6,1, 1,9, 9,2, 1,8, 8,4, 4,5, 8,10, 10,0, 1,3, 3,7};
bool looped = false;

void Intro::Draw()
{
	drawRect(0, 0, SCR_WIDTH, SCR_HEIGHT, 0 );

	//drawCircle( SCR_WIDTH/2, SCR_HEIGHT/2, SCR_HEIGHT/4, false, 0xFF00FFFF );

	for ( int i = 0; i < 11; i++ )
	{
		float x1 = bozo[iOffset+iCharacter[i*2]*2];
		float y1 = bozo[iOffset+1+iCharacter[i*2]*2];
		
		float x2 = bozo[iOffset+iCharacter[(i*2)+1]*2];
		float y2 = bozo[iOffset+1+iCharacter[(i*2)+1]*2];

		const float xZoom = 2.1;
		const float yZoom = 2.9;

		x1 *= xZoom;
		y1 *= yZoom;
		x2 *= xZoom;
		y2 *= yZoom;

		drawLine(x1, y1, x2, y2, 0xFFFFFFFF );
	}

	if ( !looped )
	{
		fwrite( &bozo[iOffset], 1, 11*2, outFile );
	}

	iOffset += 11*2;
	iOffset += 11*2;
	
	iFrame++;
	if ( iFrame >= (nbFrames-1)/2 )
	{
		iFrame = 0;
		iOffset = 3;
		looped = true;
	}
}

void Intro::Destroy()
{
}

