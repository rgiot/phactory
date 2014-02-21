
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include "config.h"
#include "window.h"
#include "pixel.h"

bool YLessPrecision = false;

unsigned int getPixel(int x, int y)
{	
	unsigned int *buffer = GetBuffer();	
	
	if ( x < 0 )
	{
		return PIXEL_ERROR;
		//x = 0;
	}
	if ( x >= SCR_WIDTH )
	{
		return PIXEL_ERROR;
		//x = SCR_WIDTH-1;
	}

	x *= 4;
	y *= 2;

	if ( y < 0 )
	{	
		return PIXEL_ERROR;
		//unsigned int color = buffer[ (0) * (SCR_WIDTH*4) + (x)];
		//return color;
	}
	if ( y >= (205*2) )
	{	
		return PIXEL_ERROR;
		//unsigned int color = buffer[ (204*2) * (SCR_WIDTH*4) + (x)];
		//return color;
	}

	unsigned int color = buffer[ (y) * (SCR_WIDTH*4) + (x)];
	return color;
}

void putPixel( int x, int y, unsigned int color )
{
	if ( x < 0 )
	{
		return;
	}

	if ( y < 0 )
	{
		return;
	}

	if ( x >= SCR_WIDTH )
	{
		return;
	}

	if ( y >= SCR_HEIGHT )
	{
		return;
	}

	unsigned int *buffer = GetBuffer();	
	
	x *= 4;
	y *= 2;

	for(int y1=0; y1<2;y1++)
	{
		for(int x1=0; x1<4;x1++)
		{
			buffer[ (y+y1) * (SCR_WIDTH*4) + (x+x1)] = color;		
		}
	}
}

void drawRect( int x1, int y1, int width, int height, unsigned int color )
{
	int x, y;

	int x2 = x1 + width;
	int y2 = y1 + height;

	for ( y = y1; y < y2; y++ )
	{
		for ( x = x1; x < x2; x++ )
		{
			putPixel( x, y, color );
		}
	}
}

void drawHorizontalLine( int x1, int x2, int y, unsigned int color )
{
	int x;

	if ( x1 == x2 )
	{
		x2++;
	}

	for ( x = x1; x < x2; x++ )
	{
		putPixel( x, y, color );
	}
}

void drawCircle( int xCenter, int yCenter, int radius, bool fill, unsigned int color )
{
	if ( fill )
	{
		int l = (int) radius * cos (PI / 4);

		for (int x = 0; x <= l; x++)
		{
			int y = (int) sqrt ((double) (radius * radius) - (x * x));
			
			putPixel(xCenter+x, yCenter+y*2, color);
			putPixel(xCenter+x, yCenter+-y*2, color);
			putPixel(xCenter+-x, yCenter+y*2, color);
			putPixel(xCenter+-x, yCenter+-y*2, color);

			putPixel(xCenter+y, yCenter+x*2, color);
			putPixel(xCenter+y, yCenter+-x*2, color);
			putPixel(xCenter+-y, yCenter+x*2, color);
			putPixel(xCenter+-y, yCenter+-x*2, color);

			putPixel(xCenter+x, yCenter+y*2+1, color);
			putPixel(xCenter+x, yCenter+-y*2+1, color);
			putPixel(xCenter+-x, yCenter+y*2+1, color);
			putPixel(xCenter+-x, yCenter+-y*2+1, color);

			putPixel(xCenter+y, yCenter+x*2+1, color);
			putPixel(xCenter+y, yCenter+-x*2+1, color);
			putPixel(xCenter+-y, yCenter+x*2+1, color);
			putPixel(xCenter+-y, yCenter+-x*2+1, color);
		}
	}
	else
	{
		for (int y = -radius; y <= radius; y++)
		{
			for (int x = -radius; x <= radius; x++)
			{
				if ((x * x) + (y * y) < (radius * radius))
				{
					putPixel(xCenter+x, yCenter+y*2, color);
					putPixel(xCenter+x, yCenter+y*2+1, color);
				}
			}
		}
	}
}

void drawLine( int x1, int y1, int x2, int y2, unsigned int color )
{
	unsigned short x;
	unsigned char y;

	if ( y2 < y1 )
	{
		y = y1;
		y1 = y2;
		y2 = y;

		x = x1;
		x1 = x2;
		x2 = x;
	}

	int d1 = x2 - x1;
	int d2 = y2 - y1;

	if ( d1 <= d2 )
	{		
		float fDelta = ((float)(d1))/((float)(d2));
		unsigned short delta = (unsigned short)( fDelta * 256.0f );

		unsigned short i = (unsigned short)( x1 << 8 );

		if ( !YLessPrecision )
		{
			for ( y = y1; y < y2; y++ )
			{
				x = (unsigned char)( i >> 8 );

				putPixel( x, y, color );

				i += delta;
			}
		}
		else
		{
			for ( y = y1; y <= y2; y++ )
			{
				x = (unsigned char)( i >> 8 );

				putPixel( x, y, color );

				i += delta;
			}
		}
	}
	else
	{
		float fDelta = ((float)(d2))/((float)(d1));
		unsigned short delta = (unsigned short)( fDelta * 256.0f );

		unsigned short i = (unsigned short)( y1 << 8 );

		for ( x = x1; x <= x2; x++ )
		{
			y = (unsigned char)( i >> 8 );

			putPixel( x, y, color );

			i += delta;
		}
	}
}