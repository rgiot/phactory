
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <vector>
#include <assert.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Batman.h"

static FILE *file;
static FILE *fileCircleMove;

static void WriteByte(unsigned char value)
{
	fwrite(&value, 1, 1, file);
}

static FILE *fileLeftPixelPos;
static FILE *fileScanlineSpritePtr;
static FILE *fileRelativeJump;
static FILE *fileDoRightPixel;

static void WriteByte(FILE *fileDest, unsigned char value)
{
	fwrite(&value, 1, 1, fileDest);
}

Batman::Batman() :
	FXBase()
{	
}

Batman::~Batman()
{
	fclose(fileCircleMove);
}

SDLKey Batman::GetKey()
{
	return SDLK_2;
}

const char *Batman::GetName()
{
	return "Batman";
}

void Batman::Init()
{
}

static unsigned short BatmanScanlinePtr1[128];
static unsigned short BatmanScanlinePtr2[128];

void Batman::Draw()
{
	DrawImage();
}

static std::vector<int> xTable;
static std::vector<int> yTable;

static float offsetX = 0;
static float offsetY = 0;
static float radiusCircle = 0.0f;

static bool firstFrame = true;
const float radius = 16.0f;

void Batman::ComputeAndWriteCircle(FILE *file, int offsetX, int clipXLeft, int clipXRight)
{
	drawRect(0, 0, SCR_WIDTH, SCR_HEIGHT, 0 );

	drawCircle( radius-1+offsetX, -2+radius*2, radius, false, 0xFFFFFFFF );

	int width = (radius-1)*2;
	int height = (-2+radius*2)*2;

	width += offsetX*2;

	for ( int y = 0; y <= height; y++ )
	{
		unsigned char XLine = -1;
		unsigned char OffOn = false;
		unsigned char OnOff = false;
		unsigned char SizePlainLine = -1;

		int baseXLine = -1;

		int x;
		for ( x = 0; x < width; x+=2 )
		{
			unsigned int leftPixel = getPixel(x, y);
			unsigned int rightPixel = getPixel(x+1, y);

			XLine = x/2;
			
			if ( x < clipXLeft )
			{
				if ( baseXLine == -1 )
				{
					if ( leftPixel && rightPixel )
					{
						baseXLine = XLine;
					}
					else if ( !leftPixel && rightPixel )
					{
						baseXLine = XLine;
						OffOn = true;
					}
				}
			}
			else
			{
				if ( leftPixel && rightPixel )
				{
					if ( baseXLine == -1 )
					{
						baseXLine = XLine;
						SizePlainLine = (width/2)-XLine-baseXLine;
					}
					else
					{
						SizePlainLine = (width/2)-XLine-baseXLine;
						if ( OffOn )
						{
							OffOn = false;
							OnOff = true;						
							//SizePlainLine--;
						}
					}

					break;
				}
				else if ( !leftPixel && rightPixel )
				{
					if ( baseXLine == -1 )
					{
						baseXLine = XLine;
						SizePlainLine = (width/2)-XLine-baseXLine;
						
						OnOff = true;
						OffOn = true;
					}
					else
					{
						SizePlainLine = (width/2)-XLine-baseXLine;

						OffOn = true;
					}
					break;
				}
			}
		}
		
		unsigned char SizeScanline = SizePlainLine;
		if ( SizeScanline >= 0x20 )
		{
			SizeScanline = 0;
		}

		if ( OffOn && ( SizeScanline == 0 ) )
		{
			OffOn = false;
		}

		if ( XLine + SizeScanline > (clipXRight/2) )
		{
			SizeScanline = (clipXRight/2) - XLine;

			OnOff = false;

			if ( SizeScanline >= 0x30 )
			{
				if ( OnOff )
				{
					SizeScanline = 1;
				}
				else
				{
					SizeScanline = 0;
				}
			}
		}	

		if ( XLine >= (clipXRight/2) )
		{
			OffOn = false;
			OnOff = false;
		}

		if ( OffOn )
		{
			if ( SizeScanline > 0 )
			{
				SizeScanline -= 1;
			}
		}

		if ( OnOff )
		{
			if ( SizeScanline > 0 )
			{
				SizeScanline -= 1;
			}		
		}

		if ( SizeScanline >= 16 )
		{
			SizeScanline = 16;
		}

		SizeScanline = ( 16 - SizeScanline ) * 2;

		if ( OffOn )
		{
			SizeScanline += 0x80; 
		}
		if ( OnOff )
		{
			SizeScanline += 0x40; 
		}
		
		XLine -= clipXLeft/2;

		if ( clipXRight < 50 )
		{
			if ( offsetX == 0 )
			{
				XLine++;
			}
		}
	
		fwrite(&XLine, 1, 1, file);
		fwrite(&SizeScanline, 1, 1, file);
	}
}
	
void Batman::DrawImageFirstFrame()
{
	FILE *file;
	
	file = fopen("..\\..\\winape\\src\\batman\\BatmanCircleFull.bin", "wb" );
	ComputeAndWriteCircle(file, 0, 0, 50);	
	ComputeAndWriteCircle(file, 1, 0, 50);	
	fclose(file);

	file = fopen("..\\..\\winape\\src\\batman\\BatmanCircleClipLeft.bin", "wb" );
	for ( int clipXLeft = 2; clipXLeft <= 30; clipXLeft += 2 )
	{
		ComputeAndWriteCircle(file, 1, clipXLeft, 50);	
		ComputeAndWriteCircle(file, 0, clipXLeft, 50);			
	}
	fclose(file);

	file = fopen("..\\..\\winape\\src\\batman\\BatmanCircleClipRight.bin", "wb" );
	for ( int clipXRight = 28; clipXRight >= 0; clipXRight -= 2 )
	{
		ComputeAndWriteCircle(file, 0, 0, clipXRight);	
		ComputeAndWriteCircle(file, 1, 0, clipXRight);	
	}
	fclose(file);

	//exit(-1);

	fileCircleMove = fopen("..\\..\\winape\\src\\batman\\BatmanCircleMoveXY.bin", "wb" );
}

void Batman::DrawImage()
{
	if ( firstFrame )
	{
		DrawImageFirstFrame();

		firstFrame = false;
		return;
	}
	//return;
	//x = radius;
	//y = radius*2;
	
	int x = (SCR_WIDTH/2)+(sin(offsetX)*radiusCircle);
	int y = (SCR_HEIGHT/2)+((cos(offsetY)*radiusCircle)*2.0f);

	drawCircle( x, y, radius, false, 0xFFFFFFFF );
	
	x -= 15;
	y -= 30;

	if ( x < -30 )
	{
		x = -30;
	}
	if ( x > 191 )
	{
		x = 191;
	}
	if ( y < -65 )
	{
		y = -65;
	}
	if ( y > 205 )
	{
		y = 205;
	}

	unsigned short writeX = (unsigned short) (x);
	unsigned short writeY = (unsigned short) (y+65);
	fwrite(&writeY, 2, 1, fileCircleMove);
	fwrite(&writeX, 2, 1, fileCircleMove);
	
	offsetX += 0.4f;
	offsetY += 0.4f;
	radiusCircle += 0.7f;

	xTable.push_back(x);
	yTable.push_back(y);
}

int Batman::GetLeftOutX(unsigned char *p)
{
	for( int x = 0; x < 40; x++ )
	{
		unsigned int color = 0;
			
		color+=(*p<<16);
		p++;

		color+=(*p<<8);
		p++;

		color+=(*p);
		p++;

		p += 3;

		if ( color != 0xFFFFFF )
		{
			return x;
		}
	}

	return -1;
}

int Batman::GetRightOutX(unsigned char *p)
{
	int charStart = 79;
	
	p += 159*3;			
	for( int x = charStart; x >= 0; x-- )
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

		if ( color != 0xFFFFFF )
		{
			return x;
		}
	}

	return -1;
}

void Batman::Destroy()
{
}
