
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"

#include "Default.h"

Default::Default() :
	FXBase()
{
}

Default::~Default()
{
}

SDLKey Default::GetKey()
{
	return SDLK_1;
}

const char *Default::GetName()
{
	return "Default";
}

void Default::Init()
{
}

void Default::Draw()
{
	drawRect( 0, 0, SCR_WIDTH, SCR_HEIGHT, PIXEL_GRAY );
}

void Default::Destroy()
{
}