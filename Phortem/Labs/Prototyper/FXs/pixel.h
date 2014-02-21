
#ifndef __PIXEL_H__
#define __PIXEL_H__

// ARGB 0x00FF0000 = red
#define PIXEL_RED	0x00FF0000
#define PIXEL_GREEN 0x0000FF00
#define PIXEL_BLUE	0x000000FF
#define PIXEL_BLACK	0x00000000
#define PIXEL_WHITE	0x00FFFFFF
#define PIXEL_GRAY	0x003F3F3F
#define PIXEL_ERROR	0xFFFFFFFF

unsigned int getPixel(int x, int y);
void putPixel( int x, int y, unsigned int color );
void drawRect( int x1, int y1, int x2, int y2, unsigned int color );
void drawLine( int x0, int y0, int x1, int y1, unsigned int color );
void drawHorizontalLine( int x1, int x2, int y, unsigned int color );
void drawCircle( int x, int y, int radius, bool fill, unsigned int color );

#endif // __PIXEL_H__