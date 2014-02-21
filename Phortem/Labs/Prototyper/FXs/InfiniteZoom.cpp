
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "InfiniteZoom.h"

#define TextureWidth 48
#define TextureHeight 48

InfiniteZoom::InfiniteZoom() :
	FXBase()
{	
	FILE *file = fopen("InfiniteZoomBackground_384x205.raw", "rb" );
	bgRaw = new unsigned char[384*205*3];
	fread(bgRaw, 1, 384*205*3, file);
	fclose(file);	
}

InfiniteZoom::~InfiniteZoom()
{
}

SDLKey InfiniteZoom::GetKey()
{
	return SDLK_2;
}

const char *InfiniteZoom::GetName()
{
	return "InfiniteZoom";
}

struct SLineEdge
{
	int left;
	int right;
};
SLineEdge LinesEdges[TextureHeight];

void InfiniteZoom::Init()
{
	FILE *file = fopen("Z01.raw", "rb" );
	raw1 = new unsigned char[TextureWidth*TextureHeight*3];
	int size = TextureWidth*TextureHeight*3;
	fread(raw1, 1, size, file);
	fclose(file);

	file = fopen("Z02.raw", "rb" );
	raw2 = new unsigned char[TextureWidth*TextureHeight*3];
	fread(raw2, 1, TextureWidth*TextureHeight*3, file);
	fclose(file);

	file = fopen("Z03.raw", "rb" );
	raw3 = new unsigned char[TextureWidth*TextureHeight*3];
	fread(raw3, 1, TextureWidth*TextureHeight*3, file);
	fclose(file);

	file = fopen("Z04.raw", "rb" );
	raw4 = new unsigned char[TextureWidth*TextureHeight*3];
	fread(raw4, 1, TextureWidth*TextureHeight*3, file);
	fclose(file);

	CpcPixels = new CPCPixel[TextureWidth*TextureHeight];
	frameCount = 0;
	canWriteFrame = true;
}

static double zoomRatio = 0.0f;
static const double zoomSpeed = 0.12f;
static int stepCount = 0;

static unsigned char *ptr1 = 0;
static unsigned char *ptr2 = 0;
static unsigned char *ptr3 = 0;

void InfiniteZoom::Draw()
{
	//DrawImage();
	DrawBGImage();

//	drawRect( 0, 0, SCR_WIDTH, SCR_HEIGHT, PIXEL_GRAY );

	switch(stepCount)
	{
	case 0:	ptr1 = raw1; ptr2 = raw2; ptr3 = raw3; break;
	case 1:	ptr1 = raw2; ptr2 = raw3; ptr3 = raw4; break;
	case 2:	ptr1 = raw3; ptr2 = raw4; ptr3 = raw1; break;
	case 3:	ptr1 = raw4; ptr2 = raw1; ptr3 = raw2; break;
	default: 
		break;
	}

	const float blockWidth = float(TextureWidth)/6.0f;
	const float blockHeight = float(TextureHeight)/6.0f;

	const float nbBlocksHoriz = float(TextureWidth) / blockWidth;
	const float nbBlocksVert = float(TextureHeight) / blockHeight;

	const float targetBlocksHoriz = (nbBlocksHoriz/2) / ( (nbBlocksHoriz/2) - 2 ); // remove 2 at left, right

	
	for ( int i = 0; i < TextureHeight; i++ )
	{
		LinesEdges[i].left = -2;
		LinesEdges[i].right = -2;
	}

	
	double ratio1 = 1.0f + ( 1.9f * zoomRatio );
	double ratio2 = 0.333f + ( 0.666f * zoomRatio );
	double ratio3 = ( 0.35f * zoomRatio );
	
	DrawZoomImage(ptr1, 0, ratio1);
	DrawZoomImage(ptr2, 1, ratio2);
	DrawZoomImage(ptr3, 2, ratio3);

	
	// UNCOMENT TO DRAW EDGES
	/*DrawMaskImage(ptr1, 0, ratio1);
	DrawMaskImage(ptr2, 1, ratio2);
	DrawMaskImage(ptr3, 2, ratio3);*/
	

	if ( canWriteFrame )
	{
		std::string filename;
		filename += "InfiniteZoomData";
		if ( frameCount < 10 )
		{
			filename += "0";
		}
		char szFrameCount[100];   
		sprintf( szFrameCount, "%d", frameCount );
		filename += szFrameCount;
		filename += ".bin";

		WriteCPCPixelFrame( (unsigned char*) filename.c_str(), (unsigned char *) "InfiniteZoomLineEdges.bin" );

		frameCount++;
	}
	
	zoomRatio += zoomSpeed;
	if ( zoomRatio >= (1.0f-zoomSpeed) )
	{
		canWriteFrame = false;

		zoomRatio = 0.0f;

		stepCount++;
		if ( stepCount == 4 )
		{
			stepCount = 0;
		}
	}
}

void InfiniteZoom::DrawBGImage()
{
	unsigned char *p = bgRaw;

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

void InfiniteZoom::DrawImage(unsigned char *raw)
{
	unsigned char *pRaw1 = raw1;
	
	for( int y = 0; y < TextureHeight; y++ )
	{
		for( int x = 0; x < TextureWidth; x++ )
		{
			unsigned int color = GetImagePixel( raw, x, y );
			
			putPixel(x, y, color);
		}
	}
}


void InfiniteZoom::DrawMaskImage(unsigned char *raw, unsigned char imageType, double ratio)
{
	int scrX = (24+16)*2;
	int scrY = 8;

	drawRect( 0, 0, scrX, SCR_HEIGHT, 0xFF4CFF00 );
	drawRect( 0, 0, SCR_WIDTH, scrY, 0xFF4CFF00 );
	drawRect( 0, scrY+TextureHeight*4, SCR_WIDTH, SCR_HEIGHT, 0xFF4CFF00 );
	drawRect( scrX+TextureWidth*2, 0, SCR_WIDTH, SCR_HEIGHT, 0xFF4CFF00 );
	
	double xCenter = (double(TextureWidth)/2.0f);
	double yCenter = (double(TextureHeight)/2.0f);

	for( int y = 0; y < TextureHeight; y++ )
	{
		for( int x = 0; x < TextureWidth; x++ )
		{
			double fX = (double) x;
			double fY = (double) y;

			fX -= xCenter;
			fY -= yCenter;

			fX /= ratio;
			fY /= ratio;

			fX += xCenter;
			fY += yCenter;

			int finalX = (int) (fX+0);
			int finalY = (int) (fY+0);

			if (	((finalX>=0)&&(finalX<TextureWidth))
				&&	((finalY>=0)&&(finalY<TextureHeight))	)
			{
				unsigned int color = GetImagePixel( raw, finalX, finalY );

				int startX = (x*2) + scrX;
				int startY = (y*4) + scrY;

				bool hasGreen = false;

				int count = 0;

				for ( int iY = 0; iY < 4; iY++)
				{
					for ( int iX = 0; iX < 2; iX++)
					{
						unsigned char *pbgRaw = &bgRaw[((startY+iY)*6*192)+(6*(startX+(iX*2)))];
						unsigned int bgColor = 0;
						bgColor+=(*pbgRaw<<16);
						pbgRaw++;
						bgColor+=(*pbgRaw<<8);
						pbgRaw++;
						bgColor+=(*pbgRaw);

						if ( (((bgColor>>8)&255)==0xFF) && (((bgColor>>16)&255)==0x4C) )
						{
							hasGreen = true;
							count++;
						}
					}
				}
				bool isFull = (count == 2*4);
				
				if ( hasGreen && !isFull)
				{
					//if ( !isFull )
					
				}
				else
				{
					{
						for ( int iY = 0; iY < 4; iY++)
						{
							for ( int iX = 0; iX < 2; iX++)
							{
								putPixel(startX+iX, startY+iY, 0xFF4CFF00);
							}
						}
					}
				}
			}
		}
	}

	static bool wrote = false;

	if ( !wrote )
	{
		wrote = true;

		FILE *file = fopen("InfiniteZoomBackgroundPixelEdges.raw", "wb" );

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
}

void InfiniteZoom::DrawZoomImage(unsigned char *raw, unsigned char imageType, double ratio)
{
	int scrX = (24+16)*2;
	int scrY = 8;

	double xCenter = (double(TextureWidth)/2.0f);
	double yCenter = (double(TextureHeight)/2.0f);

	for( int y = 0; y < TextureHeight; y++ )
	{
		int left = -1;
		int right = -1;

		for( int x = 0; x < TextureWidth; x++ )
		{
			double fX = (double) x;
			double fY = (double) y;

			fX -= xCenter;
			fY -= yCenter;

			fX /= ratio;
			fY /= ratio;

			fX += xCenter;
			fY += yCenter;

			int finalX = (int) (fX+0);
			int finalY = (int) (fY+0);

			if (	((finalX>=0)&&(finalX<TextureWidth))
				&&	((finalY>=0)&&(finalY<TextureHeight))	)
			{
				unsigned int color = GetImagePixel( raw, finalX, finalY );

				int startX = (x*2) + scrX;
				int startY = (y*4) + scrY;

				bool hasGreen = false;

				for ( int iY = 0; iY < 4; iY++)
				{
					for ( int iX = 0; iX < 2; iX++)
					{
						unsigned char *pbgRaw = &bgRaw[((startY+iY)*6*192)+(6*(startX+(iX*2)))];
						unsigned int bgColor = 0;
						bgColor+=(*pbgRaw<<16);
						pbgRaw++;
						bgColor+=(*pbgRaw<<8);
						pbgRaw++;
						bgColor+=(*pbgRaw);

						if ( (((bgColor>>8)&255)==0xFF) && (((bgColor>>16)&255)==0x4C) )
						{
							hasGreen = true;
						}
					}
				}

				
				if ( hasGreen )
				{
					if (left==-1)
					{
						left = x;
					}
					right = x+1;

					for ( int iY = 0; iY < 4; iY++)
					{
						for ( int iX = 0; iX < 2; iX++)
						{
							putPixel(startX+iX, startY+iY, color);
						}
					}
				}

				PutCpcPixelFrame(imageType, x, y, finalX, finalY);
			}
		}

		if ( left != -1 )
		{
			SLineEdge &lineEdge = LinesEdges[y];
			if ( lineEdge.left == -2 )
			{
				lineEdge.left = left;
				lineEdge.right = right;
			}
			else
			{
				if ( left < lineEdge.left )
				{
					lineEdge.left = left;
				}
				if ( right >= lineEdge.right )
				{
					lineEdge.right = right;
				}
			}
		}
	}
}

unsigned int InfiniteZoom::GetImagePixel( unsigned char *raw, int x, int y )
{
	raw += ( y * TextureWidth * 3 );
	raw += ( x * 3 );

	unsigned int color = 0;			
	color+=(*raw<<16);
	raw++;
	color+=(*raw<<8);
	raw++;
	color+=(*raw);

	return color;
}

void InfiniteZoom::PutCpcPixelFrame( unsigned char imageType, int x, int y, int u, int v )
{
	CPCPixel *ptr = &CpcPixels[(TextureWidth*y)+x];

	ptr->imageType = imageType;
	ptr->u = (unsigned char) u;
	ptr->v = (unsigned char) v;
}

void InfiniteZoom::WriteCPCPixelFrame( unsigned char *filename, unsigned char *filenameLineEdges )
{
	FILE *file = fopen( (const char *) filenameLineEdges, "wb" );

	for( int y = 0; y < TextureHeight; y++ )
	{
		SLineEdge &lineEdge = LinesEdges[y];
		unsigned char posX = lineEdge.left;
		unsigned char size = lineEdge.right-lineEdge.left;
		fwrite(&posX, 1, sizeof( unsigned char), file );
		fwrite(&size, 1, sizeof( unsigned char), file );
	}

	fclose(file);

	file = fopen( (const char *) filename, "wb" );

	for( int y = 0; y < TextureHeight; y++ )
	{
		SLineEdge &lineEdge = LinesEdges[y];
		
		for( int x = 0; x < TextureWidth; x++ )
		{
			CPCPixel *ptr = &CpcPixels[(TextureWidth*y)+x];

			unsigned short cpcOffset;
			
			cpcOffset = ( unsigned short(ptr->v) * TextureWidth );
			cpcOffset += ptr->u;

			switch( ptr->imageType )
			{
			case 0: cpcOffset += unsigned short((0<<15)+(0<<14)); break;
			case 1: cpcOffset += unsigned short((0<<15)+(1<<14)); break;
			case 2: cpcOffset += unsigned short((1<<15)+(0<<14)); break;
			case 3: cpcOffset += unsigned short((1<<15)+(1<<14)); break;
			}

			if ( (x>=lineEdge.left)&&(x<lineEdge.right))
			{
				fwrite(&cpcOffset, 1, sizeof( unsigned short), file );
			}
		}
	}

	fclose( file );
}

void InfiniteZoom::Destroy()
{
}