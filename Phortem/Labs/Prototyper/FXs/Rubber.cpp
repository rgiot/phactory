
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Rubber.h"

static unsigned int CPCPalette[] = {
			0x020702, 
            0x050663,
            0x0507f1,
            0x670600,
            0x680764,
            0x6807F1,
            0xFD0704,
            0xFF0764,
            0xFD07F2,
            0x046703,
            0x046764,
            0x0567f1,
            0x686704,
            0x686764,
            0x6867f1,
            0xfd6704,
            0xfd6763,
            0xfd67f1,
            0x04f502,
            0x04f562,
            0x04f5f1,
            0x68f500,
            0x68f564,
            0x68f5f1,
            0xfef504,
            0xfdf563,
			0xfdf5f0 };

static unsigned char ImgPalette[] = { 10, 1, 0, 13, 17, 19, 25, 14, 26, 23, 16, 4, 7, 3, 11, 6 };

static FILE *file;
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

Rubber::Rubber() :
	FXBase()
{
	FILE *file = fopen("RubberSineCurve1.bin", "wb" );
	int tableSize = 256;
	unsigned char *raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 126.5f;
		float v = sin(rad)*radius;
				
		v = (radius*0.5f) + (v*0.49f);

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;

	/*file = fopen("RubberSineCurve2.bin", "wb" );
	tableSize = 256;
	raw = new unsigned char[tableSize];
	int i;
	for(i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)(tableSize)))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 112.5f;
		float v = cos(rad)*radius;
				
		v = (radius*0.5f) + (v*0.49f);

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;*/
	
	file = fopen("RubberSineCurve2.bin", "wb" );
	tableSize = 256;
	raw = new unsigned char[tableSize];
	int i;
	for(i = 0; i<tableSize/2; i++)
	{
		float deg = ((((float)i)/((float)(tableSize/2)))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 112.5f;
		float v = cos(rad)*radius;
				
		v = (radius*0.5f) + (v*0.49f);

		raw[i] = (unsigned char) v;
	}
	for(; i<tableSize; i++)
	{
		float deg = ((((float)(i-(tableSize/2)))/((float)(tableSize/2)))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 52.5f;
		float v = cos(rad)*radius;
				
		v = (radius*0.5f) + (v*0.49f);

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;

	file = fopen("RubberSineCurvePosX.bin", "wb" );
	tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 8.0f;
		float v = sin(rad)*radius;
				
		v = (radius*0.5f) + (v*0.49f);

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;
	
	file = fopen("RubberSineCurvePosX2.bin", "wb" );
	tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg*2);
		float radius = 4.0f;
		float v = cos(rad)*radius;
				
		v = (radius*0.5f) + (v*0.49f);

		raw[i] = (unsigned char) v;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;

	file = fopen("Rubber_160x128.raw", "rb" );
	rawImage = new unsigned char[160*128*3];
	fread(rawImage, 1, 160*128*3, file);
	fclose(file);


	file = fopen("RubberPrecaOffsetPtr.bin", "wb" );
	tableSize = 256;
	raw = new unsigned char[tableSize];
	unsigned short dataPtr = 0;
	for(int i = 0; i<tableSize; i+=2)
	{		
		raw[i] = dataPtr&255;
		raw[i+1] = dataPtr>>8;
		
		dataPtr += 8;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;
}

Rubber::~Rubber()
{
}

SDLKey Rubber::GetKey()
{
	return SDLK_2;
}

const char *Rubber::GetName()
{
	return "Rubber";
}

void Rubber::Init()
{
}

static unsigned short rubberScanlinePtr1[128];
static unsigned short rubberScanlinePtr2[128];

void Rubber::Draw()
{
	DrawImage();

	CreateRubberScanlines(rubberScanlinePtr1, (unsigned char*)0x6800, 40 ); 
	CreateRubberScanlines(rubberScanlinePtr2, (unsigned char*)0xE800, 41 ); 
	
	/*FILE *fileScanlineSpritePtr = fopen("Rubber1_2_ScanlineSpritePtr.bin", "wb" );
	CreateRubberScanlines(fileScanlineSpritePtr, (unsigned char*)0x6800, 40 ); 
	fclose(fileScanlineSpritePtr);	

	fileScanlineSpritePtr = fopen("Rubber2_2_ScanlineSpritePtr.bin", "wb" );
	CreateRubberScanlines(fileScanlineSpritePtr, (unsigned char*)0xE800, 41 ); 
	fclose(fileScanlineSpritePtr);*/

	CreateRubberPreca(false);
	CreateRubberPreca(true);

	//CreateRubberEdges( false, "Rubber1_1_LeftPixelPos.bin", "Rubber1_3_RelativeJump.bin", "Rubber1_4_DoRightPixel.bin" );
	//CreateRubberEdges( true, "Rubber2_1_LeftPixelPos.bin", "Rubber2_3_RelativeJump.bin", "Rubber2_4_DoRightPixel.bin" );
}

void Rubber::CreateRubberPreca(bool isPixelRight)
{
	FILE *fileRubberPrecaSpritePtr = 0;
	
	if ( isPixelRight )
	{
		fileRubberPrecaSpritePtr = fopen("RubberPrecaData2.bin", "wb" );
	}
	else
	{
		fileRubberPrecaSpritePtr = fopen("RubberPrecaData1.bin", "wb" );
	}
	
	unsigned char ScanlinePtr[256];
	unsigned char ScreenOffsetPtr[256];
	unsigned char DrawLinePtr[256];
	unsigned char SkipLengthPtr[256];

	unsigned char *p = rawImage;
	if ( isPixelRight )
	{
		p -= 3;
	}
	
	for( int y = 0; y < 128; y++ )
	{
		int leftX = GetLeftOutX(p, isPixelRight);
		int rightX = GetRightOutX(p, isPixelRight);

		bool isLeft = ((leftX&1)==1);
		bool isRight = ((rightX&1)==0);

		unsigned char cpcLeftX = leftX/2;
		unsigned char cpcRightX = rightX/2;			

		if ( !isPixelRight )
		{
			putPixel(leftX, y, 0x123456);
			putPixel(rightX, y, 0x123456);
		}
		
		// get scanline spr ptr, already includes PosX
		unsigned short spritePtr = 0;
		if ( isPixelRight )
		{
			spritePtr = rubberScanlinePtr2[y];
		}
		else
		{
			spritePtr = rubberScanlinePtr1[y];
		}
		spritePtr += cpcLeftX;

		unsigned short codePtr = 0;

		unsigned char skipBytes = 0;

		// raster line display ptr (type 0-3)		
		// type0: left, left : ptr1
		// type1: right, left : ptr2
		// type2: left, right : ptr3
		// type3: right, right : ptr4
		if ((!isLeft) && (!isRight)) // type 0
		{ 
			codePtr = 0xFC80;
		}
		else if ((!isLeft) && (isRight)) // type 2
		{
			codePtr = 0xFD80;			
			skipBytes = 1;
		}
		else if ((isLeft) && (!isRight)) // type 1
		{
			codePtr = 0xFE80;
			skipBytes = 1;
		}
		else if ((isLeft) && (isRight)) // type 3
		{
			codePtr = 0xFF80;
			skipBytes = 2;
		}
		DrawLinePtr[y*2] = codePtr&255;
		DrawLinePtr[(y*2)+1] = codePtr>>8;

		unsigned char cpcSkipBytes = skipBytes + 39-cpcRightX + cpcLeftX;

		if ( isPixelRight )
		{
			if ( cpcSkipBytes!= 0 )
			{
				cpcSkipBytes--;
			}
		}

		codePtr += 0x10 + cpcSkipBytes*2; // 2=LDI opcode size

		SkipLengthPtr[y*2] = codePtr&255;
		SkipLengthPtr[(y*2)+1] = codePtr>>8;

		ScanlinePtr[y*2] = spritePtr&255;
		ScanlinePtr[(y*2)+1] = spritePtr>>8;
		
		ScreenOffsetPtr[y*2] = cpcLeftX&255;
		ScreenOffsetPtr[(y*2)+1] = 0;
		
		p += 160*3;
	}

	/*fwrite( DrawLinePtr, 256, 1, fileRubberPrecaSpritePtr );
	fwrite( SkipLengthPtr, 256, 1, fileRubberPrecaSpritePtr );
	fwrite( ScanlinePtr, 256, 1, fileRubberPrecaSpritePtr );
	fwrite( ScreenOffsetPtr, 256, 1, fileRubberPrecaSpritePtr );*/

	for ( int i = 0; i < 256; i += 2 )
	{
		fwrite( &DrawLinePtr[i], 1, 2, fileRubberPrecaSpritePtr );
		fwrite( &SkipLengthPtr[i], 1, 2, fileRubberPrecaSpritePtr );
		fwrite( &ScanlinePtr[i], 1, 2, fileRubberPrecaSpritePtr );
		fwrite( &ScreenOffsetPtr[i], 1, 2, fileRubberPrecaSpritePtr );
	}
	
	fclose(fileRubberPrecaSpritePtr);
}

// CreateRubberEdges( false, "Rubber1_1_LeftPixelPos.bin", "Rubber1_3_RelativeJump.bin", "Rubber1_4_DoRightPixel.bin" );
void Rubber::CreateRubberEdges(bool isPixelRight, char *filenameLeftPixelPos, char *filenameRelativeJump, char *filenameDoRightPixel)
{
	unsigned char *p = rawImage;

	bool prevIsPixelRight = isPixelRight;
	isPixelRight = false;

	if ( isPixelRight )
	{
		p -= 3;
	}

	fileLeftPixelPos = fopen(filenameLeftPixelPos, "wb" );
	fileRelativeJump = fopen(filenameRelativeJump, "wb" );
	fileDoRightPixel = fopen(filenameDoRightPixel, "wb" );
	
	for( int y = 0; y < 205; y++ )
	{
		int leftX = GetLeftOutX(p, isPixelRight);
		int rightX = GetRightOutX(p, isPixelRight);

		if ( (leftX!=-1)&&(rightX!=-1) )
		{
			bool isLeft = false;
			bool isRight = false;

			if ( prevIsPixelRight )
			{
				leftX++;
			//	rightX++;
			}

			unsigned char cpcLeftX = leftX/2;

			if ( (leftX&1==1) )
			{
				isLeft = true;
			}
			
			WriteByte(fileLeftPixelPos, leftX);
			
			//if ( !prevIsPixelRight )
			{
				rightX++;
			}
			
			unsigned char cpcRightX = rightX/2;
			isRight = true;
			if ( (rightX&1==1) )
			{
				isRight = false;
			}

			unsigned char cpcLength = cpcRightX - cpcLeftX;
			
			unsigned char relativeJump = 41-cpcLength;
			
			relativeJump--;
			
			if ( !prevIsPixelRight )
			{
				if (isLeft==true)
				{
					relativeJump++;
				}
				if (isRight==true)
				{
					relativeJump++;
				}
			}
			else
			{
				if (isLeft==true)
				{
					relativeJump++;
				}
				/*if (isRight==true)
				{
					relativeJump++;
				}*/
			}

			WriteByte(fileRelativeJump, relativeJump*2);
			
			WriteByte(fileDoRightPixel, rightX);
		}

		p += 192*6;
	}

	fclose(fileLeftPixelPos);
	fclose(fileRelativeJump);
	fclose(fileDoRightPixel);
}

void Rubber::DrawImage()
{
	unsigned char *p = rawImage;

	for( int y = 0; y < 128; y++ )
	{
		for( int x = 0; x < 160/2; x++ )
		{
			unsigned int color = 0;
			
			color+=(*p<<16);
			p++;

			color+=(*p<<8);
			p++;

			color+=(*p);
			p++;

			p += 3;
			
			/*if ( color == 0xFFFFFF )
			{
				color = CPCPalette[ImgPalette[4]];
			}*/

			putPixel(x, y, color);
		}
	}
}

int Rubber::GetLeftOutX(unsigned char *p, bool isRight)
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

		if ( isRight && (x==0) )
		{
		}
		else
		{
			if ( color != 0xFFFFFF )
			{
				return x;
			}
		}
	}

	return -1;
}

int Rubber::GetRightOutX(unsigned char *p, bool isRight)
{
	int charStart = 79;
	if ( isRight )
	{
		charStart--;
	}
	
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

// ----------------------------------------------------------------------------
void Rubber::CreateRubberScanlines(unsigned short *outPtr, unsigned char *rubberPtr, unsigned char scanlineWidth)
{
	unsigned short i;
	unsigned char *v;
	
	v = (unsigned char*) rubberPtr;
	for (i=0; i<128; i++)
	{
		unsigned short vWrite = (unsigned short)v;

		*outPtr = vWrite;
		outPtr++;

		v += scanlineWidth; // scanline width
	}
}

void Rubber::Destroy()
{
}
