
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "TransitLines.h"

TransitLines::TransitLines() :
	FXBase()
{	
	file = 0;
}

TransitLines::~TransitLines()
{
	unsigned char closeAnimTag = 198;
	fwrite( &closeAnimTag, sizeof( unsigned char), 1, file );

	fclose(file);
}

SDLKey TransitLines::GetKey()
{
	return SDLK_2;
}

const char *TransitLines::GetName()
{
	return "TransitLines";
}

void TransitLines::Init()
{	
}

void TransitLines::Draw()
{
	//TransitLinesFX1();
	//TransitLinesFX2();
	TransitLinesFX3();
}

void TransitLines::Destroy()
{
}
