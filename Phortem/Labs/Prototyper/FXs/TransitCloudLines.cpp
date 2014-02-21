
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "TransitCloudLines.h"

TransitCloudLines::TransitCloudLines() :
	FXBase()
{
	{
		FILE *file = fopen("Transit_1x205.raw", "rb" );
		unsigned char *raw = new unsigned char[205];
		fread(raw, 1, 205, file);
		fclose(file);

		std::vector<unsigned char> orderIndices;
		for( unsigned short i = 0; i < 256; i++ )
		{
			for ( unsigned char j = 0; j < 205; j++ )
			{
				if ( raw[ j ] <= i )
				{
					bool found = false;
					for ( int k = 0; k<orderIndices.size(); k++ )
					{
						if ( orderIndices[k] == j )
						{
							found = true;
						}
					}
					if ( !found )
					{
						orderIndices.push_back(j);
					}
				}
			}
		}
		FILE *transit = fopen("..\\..\\winape\\src\\horizTransit\\horizTransit_data.bin", "wb" );
		fwrite( &orderIndices[0], 205, 1, transit );
		fclose(transit);
	}

	{
		FILE *file = fopen("Transit_192x1.raw", "rb" );
		unsigned char *raw = new unsigned char[192];
		fread(raw, 1, 192, file);
		fclose(file);

		std::vector<unsigned char> orderIndices;
		for( unsigned short i = 0; i < 256; i++ )
		{
			for ( unsigned char j = 0; j < 192; j++ )
			{
				if ( (unsigned short)raw[ j ] <= i )
				{
					bool found = false;
					for ( int k = 0; k<orderIndices.size(); k++ )
					{
						if ( orderIndices[k] == j )
						{
							found = true;
						}
					}
					if ( !found )
					{
						orderIndices.push_back(j);
					}
				}
			}
		}
		FILE *vertTransit = fopen("..\\..\\winape\\src\\vertTransit\\vertTransit_data.bin", "wb" );
		fwrite( &orderIndices[0], 192, 1, vertTransit );
		fclose(vertTransit);
	}

	exit(-1);
}

TransitCloudLines::~TransitCloudLines()
{
}

SDLKey TransitCloudLines::GetKey()
{
	return SDLK_2;
}

const char *TransitCloudLines::GetName()
{
	return "TransitCloudLines";
}

void TransitCloudLines::Init()
{
}

void TransitCloudLines::Draw()
{
}

void TransitCloudLines::Destroy()
{
}