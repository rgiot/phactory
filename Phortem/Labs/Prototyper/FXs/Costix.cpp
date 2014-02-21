
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Costix.h"

Costix::Costix() :
	FXBase()
{
}

Costix::~Costix()
{
}

SDLKey Costix::GetKey()
{
	return SDLK_2;
}

const char *Costix::GetName()
{
	return "Costix";
}

void Costix::AddCostix(char *filename)
{
	FILE *file = fopen( filename, "rb" );
	fseek(file, 0L, SEEK_END);
	int fileSize = ftell(file);
	fseek(file, 0L, SEEK_SET);
	unsigned char *baseBuffer = new unsigned char[fileSize];
	fread(baseBuffer, 1, fileSize, file);
	fclose(file);

	int min = 565;
	int max = -1;

	for ( int i = 0; i < fileSize; i++ )
	{
		unsigned char a = baseBuffer[i];

		a = (unsigned char)(((float)a)/16.0f);

		baseBuffer[i] = a;

		if ( a > max )
		{
			max = a;
		}
		if ( a < min )
		{
			min = a;
		}
	}

	m_costixTable.push_back( baseBuffer );
}

void Costix::Init()
{
	FILE *file = fopen("Costix_384x205.raw", "rb" );
	raw = new unsigned char[384*205*3];
	fread(raw, 1, 384*205*3, file);
	fclose(file);
		
	file = fopen( "CostixMask_192x205.raw", "rb" );
	fseek(file, 0L, SEEK_END);
	int fileSize = ftell(file);
	fseek(file, 0L, SEEK_SET);
	m_costixMask = new unsigned char[fileSize];
	fread(m_costixMask, 1, fileSize, file);
	fclose(file);	

	AddCostix("costix\\caustics_001.raw");
	AddCostix("costix\\caustics_002.raw");
	AddCostix("costix\\caustics_003.raw");
	AddCostix("costix\\caustics_004.raw");
	AddCostix("costix\\caustics_005.raw");
	AddCostix("costix\\caustics_006.raw");
	AddCostix("costix\\caustics_007.raw");
	AddCostix("costix\\caustics_008.raw");
	AddCostix("costix\\caustics_009.raw");
	AddCostix("costix\\caustics_010.raw");
	AddCostix("costix\\caustics_011.raw");
	AddCostix("costix\\caustics_012.raw");
	AddCostix("costix\\caustics_013.raw");
	AddCostix("costix\\caustics_014.raw");
	AddCostix("costix\\caustics_015.raw");
	AddCostix("costix\\caustics_016.raw");
	AddCostix("costix\\caustics_017.raw");
	AddCostix("costix\\caustics_018.raw");
	AddCostix("costix\\caustics_019.raw");
	AddCostix("costix\\caustics_020.raw");
	AddCostix("costix\\caustics_021.raw");
	AddCostix("costix\\caustics_022.raw");
	AddCostix("costix\\caustics_023.raw");
	AddCostix("costix\\caustics_024.raw");
	AddCostix("costix\\caustics_025.raw");
	AddCostix("costix\\caustics_026.raw");
	AddCostix("costix\\caustics_027.raw");
	AddCostix("costix\\caustics_028.raw");
	AddCostix("costix\\caustics_029.raw");
	AddCostix("costix\\caustics_030.raw");
	AddCostix("costix\\caustics_031.raw");
	AddCostix("costix\\caustics_032.raw");
	
	CalcCostixEdges();
	for ( int i = 0; i < 32; i++ )
	{
		CalcCostixFrame(i);
	}

	//exit(-1);
}

void Costix::Draw()
{
	DrawImage();
	FilterBlocks();

	WriteResultBitmap();

	//exit(-1);
}

void Costix::Destroy()
{
}

void Costix::CalcCostixEdges()
{
	FILE *fileLeftX = fopen( "costix\\out\\CostixLeftX.bin", "wb" );
	FILE *fileLength = fopen( "costix\\out\\CostixLength.bin", "wb" );
	
	for ( int y = 0; y < 205; y += 5 )
	{
		int leftX = -1;
		int rightX = -1;

		for ( int x = 0; x < 192; x += 2 )
		{
			bool isBlockOn = false;

			for ( int blockY = 0; blockY < 5; blockY++ )
			{
				for ( int blockX = 0; blockX < 2; blockX++ )
				{
					if ( m_costixMask[ (y+blockY) * 192 + x+blockX ] != 0 )
					{
						isBlockOn = true;
					}
				}
			}

			if ( isBlockOn )
			{
				if ( leftX == -1 )
				{
					leftX = x;
				}
			}
			else
			{
				if ( leftX != -1 ) // is leftX already treated?
				{
					if ( rightX == -1 )
					{
						rightX = x;
					}
				}
			}
		}

		unsigned char ucLeftX = leftX/2;
		unsigned char ucRightX = rightX/2;
		unsigned char ucLength = ucRightX-ucLeftX;

		fwrite( &ucLeftX, 1, 1, fileLeftX );
		fwrite( &ucLength, 1, 1, fileLength );
	}

	fclose(fileLeftX);
	fclose(fileLength);
}

void Costix::CalcCostixFrame(int frame)
{
	unsigned char *costixFrameData = m_costixTable[frame];

	char szNum[10];
	itoa(frame, szNum, 10);
	std::string filename = "costix\\out\\CostixFrame";
	if ( frame < 10 )
	{
		filename += "0";
	}
	filename += szNum;
	filename += ".bin";

	FILE *file = fopen( filename.c_str(), "wb" );

	int minDataX = 4324;
	int maxDataX = -4324;
	
	for ( int y = 0; y < 205; y += 5 )
	{
		for ( int x = 0; x < 192; x += 2 )
		{
			bool isBlockOn = false;

			for ( int blockY = 0; blockY < 5; blockY++ )
			{
				for ( int blockX = 0; blockX < 2; blockX++ )
				{
					if ( m_costixMask[ (y+blockY) * 192 + x+blockX ] != 0 )
					{
						isBlockOn = true;
					}
				}
			}

			if ( isBlockOn )
			{
				int dataX = x/2;
				int dataY = y/5;

				dataX -= 14;

				if ( dataX <= 0 )
				{
					dataX = 0;
				}
				if ( dataX >= 64 )
				{
					dataX -= 64;
				}
				if ( dataY >= 41 )
				{
					dataY = 40;
				}

				if ( dataX < minDataX )
				{
					minDataX = dataX;
				}
				if ( dataX > maxDataX )
				{
					maxDataX = dataX;
				}

				unsigned char tile = costixFrameData[(dataY*64)+dataX];

				if ( tile >= 13 )
				{
					tile = 13;
				}
				tile -= 3;
				unsigned char finalTile = (unsigned char) tile;
				
				/*float fTile = (float)tile;
				fTile *= (13.0f/16.0f);
				unsigned char finalTile = (unsigned char) fTile;*/

				unsigned short shortTileOffset = finalTile*0x15; // opcodes size

				if ( shortTileOffset > 255 )
				{
					shortTileOffset = 255; // must not happen, crash
				}

				unsigned char finalTileWithOffset = (unsigned char) shortTileOffset;

				fwrite(&finalTileWithOffset, 1, 1, file);
			}
		}
	}

	fclose(file);
}

void Costix::DrawImage()
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

void Costix::FilterBlocks()
{
	int minDataX = 4324;

	unsigned int colorReplace = getPixel(96, 0);
	
	for ( int y = 0; y < 205; y += 5 )
	{
		for ( int x = 0; x < 192; x += 2 )
		{
			bool isBlockOn = false;

			for ( int blockY = 0; blockY < 5; blockY++ )
			{
				for ( int blockX = 0; blockX < 2; blockX++ )
				{
					if ( m_costixMask[ (y+blockY) * 192 + x+blockX ] != 0 )
					{
						isBlockOn = true;
					}
				}
			}

			if ( !isBlockOn )
			{
				for ( int blockY = 0; blockY < 5; blockY++ )
				{
					for ( int blockX = 0; blockX < 2; blockX++ )
					{
						putPixel(x+blockX, y+blockY, colorReplace);
						//putPixel(x+blockX+1, y+blockY, colorReplace);
					}
				}
			}
		}
	}
}

void Costix::WriteResultBitmap()
{
	FILE *file = fopen("CostixBitmapMask.raw", "wb" );

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