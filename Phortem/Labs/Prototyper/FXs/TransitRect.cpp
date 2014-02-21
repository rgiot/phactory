
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "TransitRect.h"

TransitRect::TransitRect() :
	FXBase()
{
	FILE *file = fopen("Angel_384x205.raw", "rb" );
	raw1 = new unsigned char[384*205*3];
	fread(raw1, 1, 384*205*3, file);
	fclose(file);

	file = fopen("AngelAsDemon_384x205.raw", "rb" );
	raw2 = new unsigned char[384*205*3];
	fread(raw2, 1, 384*205*3, file);
	fclose(file);
}

TransitRect::~TransitRect()
{
}

SDLKey TransitRect::GetKey()
{
	return SDLK_2;
}

const char *TransitRect::GetName()
{
	return "TransitRect";
}

void TransitRect::Init()
{
}

void TransitRect::Draw()
{
	DrawImage();
}

void TransitRect::DrawImage()
{	
}

void TransitRect::Destroy()
{
}