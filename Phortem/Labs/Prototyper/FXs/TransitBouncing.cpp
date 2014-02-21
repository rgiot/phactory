
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "TransitBouncing.h"

static FILE *fileTransitBouncing;

TransitBouncing::TransitBouncing() :
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

	fileTransitBouncing = fopen("..\\..\\winape\\src\\bouncing\\BouncingDeltaY_1.bin", "wb" );
	//fileTransitBouncing = fopen("..\\..\\winape\\src\\bouncing2\\BouncingDeltaY_1.bin", "wb" );

	frameSinPos = 0;
}

TransitBouncing::~TransitBouncing()
{
	fclose(fileTransitBouncing);
}

SDLKey TransitBouncing::GetKey()
{
	return SDLK_2;
}

const char *TransitBouncing::GetName()
{
	return "TransitBouncing";
}

void TransitBouncing::Init()
{
	for( int y = 0; y < 205 + TUBE_HEIGHT + TUBE_HEIGHT; y++ )
	{
		LineInfo lineInfo = GetRaw2Ptr(y);

		RawLines[y] = lineInfo;
		RawLinesPrev1[y] = lineInfo;
		RawLinesPrev2[y] = lineInfo;
	}
}

void TransitBouncing::Draw()
{
	DrawImage();
}

void TransitBouncing::DrawImage()
{
	CreateRawLines1();
	//CreateRawLines2();
	
	// bouncing 1 : uncomment the following
	CreateDiffLines();
	RenderRawLines();
}

LineInfo TransitBouncing::GetRaw1Ptr( int y )
{
	y -= TUBE_HEIGHT;

	if ( y < 0 )
	{
		y = 0;
	}

	if ( y >= 205 )
	{
		y = 204;
	}

	LineInfo lineInfo;
	lineInfo.TextureY = y;
	lineInfo.Type = RAW1;
	return lineInfo;
}

LineInfo TransitBouncing::GetRaw2Ptr( int y )
{
	y -= TUBE_HEIGHT;

	if ( y < 0 )
	{
		y = 0;
	}

	if ( y >= 205 )
	{
		y = 204;
	}
	
	LineInfo lineInfo;
	lineInfo.TextureY = y;
	lineInfo.Type = RAW2;
	return lineInfo;
}

static bool canExit = false;
static bool newCurve = false;
static int heightDecrease = 0;
static bool doHeightDecrease = false;
static int HeightDecreaseCount = 1;
static int frameCount = 0;

void TransitBouncing::CreateRawLines1()
{
	frameCount++;

	memcpy( RawLinesPrev2, RawLinesPrev1, (205+TUBE_HEIGHT+TUBE_HEIGHT)*sizeof(LineInfo));
	memcpy( RawLinesPrev1, RawLines, (205+TUBE_HEIGHT+TUBE_HEIGHT)*sizeof(LineInfo));
	
	int yBounce = 205+10 - 1 - (int) ( abs( cos( frameSinPos ) ) *(205+10) );

	if ( newCurve )
	{
		yBounce = (205+TUBE_HEIGHT) - 1 - (int) ( abs( cos( frameSinPos ) ) *((205)) );
	}

	int fileSize = ftell(fileTransitBouncing);
	if ( fileSize > 0x1780 )
	{
		unsigned char nbFrames = 0xFE;
		fwrite( &nbFrames, 1, 1, fileTransitBouncing );

		fclose( fileTransitBouncing );

		fileTransitBouncing = fopen("..\\..\\winape\\src\\bouncing\\BouncingDeltaY_2.bin", "wb" );
	}

	if ( !newCurve && (yBounce >= 205 ) )
	{
		newCurve = true;
		frameSinPos += 0.25f;

		yBounce = (205+TUBE_HEIGHT) - 1 - (int) ( abs( cos( frameSinPos ) ) *((205)) );
	}

	if ( (frameSinPos>3.5f) && (!canExit))
	{
		canExit = true;
	}

	if ( newCurve && yBounce == (0+TUBE_HEIGHT) )
	{
		doHeightDecrease = true;
	}

	if ( doHeightDecrease )
	{
		HeightDecreaseCount--;
		if ( HeightDecreaseCount == 0 )
		{
			HeightDecreaseCount = 4;
			heightDecrease++;
		}
	}

	if (newCurve && canExit && ( yBounce >= 205+10+20 ))
	{
		unsigned char nbFrames = 0xFF;
		fwrite( &nbFrames, 1, 1, fileTransitBouncing );

		exit(-1);
	}

	frameSinPos += 0.018f * (1.0f +  ((float)heightDecrease)*0.10f);

	LineInfo lineInfo;

	for( int y = 0; y < 205 + TUBE_HEIGHT + TUBE_HEIGHT; y++ )
	{
		if ( y == yBounce )
		{
			int baseY = y;
			
			for ( int localY = 0; localY < TUBE_HEIGHT - heightDecrease; localY++ )
			{
				// VERSION FLAT 2D
				int yTube = TUBE_HEIGHT + TUBE_HEIGHT + baseY - localY;
				
				if ( yTube >= 205 + TUBE_HEIGHT )
				{
					lineInfo = GetRaw1Ptr(y);
				}
				else
				{
					lineInfo = GetRaw1Ptr(yTube);
				}

				RawLines[y++] = lineInfo;
			}

			y--;
		}
		else
		{
			if ( y < yBounce )
			{
				lineInfo = GetRaw1Ptr(y);
			}
			else
			{
				lineInfo = GetRaw2Ptr(y);
			}

			RawLines[y] = lineInfo;
		}
	}
}

void TransitBouncing::CreateRawLines2()
{
	frameCount++;

	memcpy( RawLinesPrev2, RawLinesPrev1, (205+TUBE_HEIGHT+TUBE_HEIGHT)*sizeof(LineInfo));
	memcpy( RawLinesPrev1, RawLines, (205+TUBE_HEIGHT+TUBE_HEIGHT)*sizeof(LineInfo));

	frameSinPos += 0.1f; // 0.07f;

	int yBounce = (int) (( ( (0.5f+(cos( frameSinPos ) /2)))*(205)) );

	unsigned char posY = yBounce;
	fwrite( &posY, 1, 1, fileTransitBouncing );

	if ( yBounce <= 0 )
	{
		unsigned char nbFrames = 0xFF;
		fwrite( &nbFrames, 1, 1, fileTransitBouncing );

		exit(-1);
	}

	LineInfo lineInfo;

	for( int y = 0; y < 205 + TUBE_HEIGHT + TUBE_HEIGHT; y++ )
	{
		if ( y < TUBE_HEIGHT + yBounce )
		{
			lineInfo = GetRaw1Ptr( y );
			//lineInfo = GetRaw1Ptr(  205 - ( yBounce - y ));
		}
		else
		{
			lineInfo = GetRaw2Ptr(( y - yBounce ));
		}

		RawLines[y] = lineInfo;
	}
}

void TransitBouncing::CreateDiffLines()
{
	FrameLines.clear();

	for( int y = 0; y < 205; y++ )
	{
		LineInfo lineInfo = RawLines[y + TUBE_HEIGHT];
		LineInfo lineInfoPrev = RawLinesPrev2[y + TUBE_HEIGHT];
		
		if ( (lineInfo.Type != lineInfoPrev.Type)|| (lineInfo.TextureY != lineInfoPrev.TextureY))
		{
			lineInfo.ScreenY = y;
			FrameLines.push_back(lineInfo);
		}
	}
}

void TransitBouncing::RenderRawLines()
{
	unsigned char nbFrames = FrameLines.size();
	fwrite( &nbFrames, 1, 1, fileTransitBouncing );

	unsigned short cpcPtr = 0;
	unsigned short prevCpcPtr = 0;

	for( int iLineInfo = 0; iLineInfo < FrameLines.size(); iLineInfo++ )
	{
		LineInfo lineInfo = FrameLines[iLineInfo];

		if ( iLineInfo == 0 )
		{
			unsigned char screenY = lineInfo.ScreenY;
			fwrite( &screenY, 1, 1, fileTransitBouncing );
		}

		unsigned char *rawLine;
		switch(lineInfo.Type)
		{
		case RAW1:
			rawLine = &raw1[lineInfo.TextureY*192*3*2]; 
			cpcPtr = 0xF900 + ( lineInfo.TextureY * 3 );
			break;
		case RAW2:
		default:
			rawLine = &raw2[lineInfo.TextureY*192*3*2];  
			cpcPtr = 0xFB70 + ( lineInfo.TextureY * 3 );
			break;
		}

		if ( (iLineInfo!=0) && ((prevCpcPtr>>8)==(cpcPtr>>8)) && ((cpcPtr & 255)<0xF0))
		{
			unsigned char cpcPtr256 = cpcPtr & 255;

			if ( cpcPtr256 >= 0xF0 )
			{
				cpcPtr256 = 0;
			}

			fwrite( &cpcPtr256, 1, 1, fileTransitBouncing );
		}
		else
		{
			unsigned char cpcPtrL = cpcPtr & 255;
			unsigned char cpcPtrH = cpcPtr >> 8;

			fwrite( &cpcPtrH, 1, 1, fileTransitBouncing );
			fwrite( &cpcPtrL, 1, 1, fileTransitBouncing );
		}

		prevCpcPtr = cpcPtr;

		for( int x = 0; x < 192; x++ )
		{
			unsigned int colorRaw = 0;			
			colorRaw+=(*rawLine<<16);
			rawLine++;
			colorRaw+=(*rawLine<<8);
			rawLine++;
			colorRaw+=(*rawLine);
			rawLine++;			
			rawLine += 3;
			
			putPixel(x, lineInfo.ScreenY, colorRaw);
		}
	}
}

void TransitBouncing::Destroy()
{
}