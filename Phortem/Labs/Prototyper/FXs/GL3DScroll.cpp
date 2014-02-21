
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <windows.h>
#include <gl/gl.h>
#include <gl\GLU.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"

#include "GL3DScroll.h"
#include "Texture.h"

GL3DScroll::GL3DScroll() :
	FXBase()
{
}

GL3DScroll::~GL3DScroll()
{
}

SDLKey GL3DScroll::GetKey()
{
	return SDLK_1;
}

const char *GL3DScroll::GetName()
{
	return "GL3DScroll";
}

static GLuint texture;
static Texture treeTexture;
static Texture bgTexture;
static int screenshotCounter = 1;

static SDL_Surface *temp;
static SDL_Surface *image;

static int FrameY = 0;

static unsigned char colors[] =
{
	0,0,0, // KEEP THIS
	255,0,0,
	255,0,255,
	0,255,255,
	0,255,255,
	//255,255,255,
	0,255,0,
	255,255,0,
	0,0,255	
};

static unsigned char convertPixelCPC( unsigned char pixel1, unsigned char pixel2 )
{
	unsigned char v;
	v =		( ( ( pixel1      ) & 1 ) << 7 ) + ( ( ( pixel2      ) & 1 ) << 6 ) 
		+   ( ( ( pixel1 >> 2 ) & 1 ) << 5 ) + ( ( ( pixel2 >> 2 ) & 1 ) << 4 ) 
		+   ( ( ( pixel1 >> 1 ) & 1 ) << 3 ) + ( ( ( pixel2 >> 1 ) & 1 ) << 2 ) 
		+   ( ( ( pixel1 >> 3 ) & 1 ) << 1 ) + ( ( ( pixel2 >> 3 ) & 1 ) );

	return (unsigned char) v;
}

static void WriteBinFrame(char *filename, unsigned char *frameBmp)
{
	FILE *file = fopen( (const char*)filename, "wb" );

	unsigned char dataFrameLeft[(SCR_BLOCKSWIDTH+2)/2];
	unsigned char dataFrameRight[(SCR_BLOCKSWIDTH+2)/2];

	memset(dataFrameLeft, 0, (SCR_BLOCKSWIDTH+2)/2);
	memset(dataFrameRight, 0, (SCR_BLOCKSWIDTH+2)/2);

	unsigned char valueLeft;
	unsigned char valueRight;

	for ( int y = 0; y<41; y++ )
	{
		for ( int x = 0; x<SCR_BLOCKSWIDTH; x++ )
		{
			int offset = (y*SCR_BLOCKSWIDTH)+x;
			offset *= 3;

			unsigned char r = frameBmp[offset++];
			unsigned char g = frameBmp[offset++];
			unsigned char b = frameBmp[offset++];

			unsigned char value = 0;

			for(int i = 0; i < 7; i++ )
			{
				if (	(colors[i*3] == b)
					&&	(colors[(i*3)+1] == g)
					&&	(colors[(i*3)+2] == r) )
				{
					value = i;
					i = 8;
				}
			}

			if ( value != 0 )
			{
				value -= 1;
				value *= 2;
				value++;
			}
			
			if ( value == 7 )
			{
				//value = 5;
			}

			if ( value == 1 )
			{
				value = 3;
			}

			if ( value > 14 )
			{
				value = 14;
			}

			if ( (x&1) == 0 )
			{
				valueLeft = value;
			}
			else
			{
				valueRight = value;

				unsigned char final = convertPixelCPC(valueLeft, valueRight);

				bool degrade = false;
				if ( valueLeft == 5 )
				{
					degrade = true;
				}
				if ( valueRight == 5 )
				{
					degrade = true;
				}
				if ( degrade )
				{
					final += 1;
				}

				bool degrade2 = false;
				if ( valueLeft == 7 )
				{
					valueLeft = 5;
					degrade2 = true;
				}
				if ( valueRight == 7 )
				{
					valueRight = 5;
					degrade2 = true;
				}
				if ( degrade2 )
				{
					final += 2;
				}

				// pixel cpc 0 INTERDIT! 
				// on s'en sert pour determiner le mask left right
				
				dataFrameLeft[ ((x-1)/2) ] = final;
			}
		}

		valueLeft = 0;

		for ( int x = 0; x<SCR_BLOCKSWIDTH; x++ )
		{
			int offset = (y*SCR_BLOCKSWIDTH)+x;
			offset *= 3;

			unsigned char r = frameBmp[offset++];
			unsigned char g = frameBmp[offset++];
			unsigned char b = frameBmp[offset++];

			unsigned char value = 0;

			for(int i = 0; i < 7; i++ )
			{
				if (	(colors[i*3] == b)
					&&	(colors[(i*3)+1] == g)
					&&	(colors[(i*3)+2] == r) )
				{
					value = i;
					i = 8;
				}
			}

			if ( value != 0 )
			{
				value -= 1;
				value *= 2;
				value++;
			}
			
			if ( value == 7 )
			{
				//value = 5;
			}

			if ( value == 1 )
			{
				value = 3;
			}

			if ( (x&1) == 1 )
			{
				valueLeft = value;
			}
			else
			{
				valueRight = value;

				unsigned char final = convertPixelCPC(valueLeft, valueRight);

				bool degrade = false;
				if ( valueLeft == 5 )
				{
					degrade = true;
				}
				if ( valueRight == 5 )
				{
					degrade = true;
				}
				if ( degrade )
				{
					final += 1;
				}

				bool degrade2 = false;
				if ( valueLeft == 7 )
				{
					valueLeft = 5;
					degrade2 = true;
				}
				if ( valueRight == 7 )
				{
					valueRight = 5;
					degrade2 = true;
				}
				if ( degrade2 )
				{
					final += 2;
				}

				// pixel cpc 0 INTERDIT! 
				// on s'en sert pour determiner le mask left right
				
				dataFrameRight[ ((x)/2) ] = final;
			}
		}

		fwrite( dataFrameLeft, 1, (SCR_BLOCKSWIDTH+2)/2, file );
		fwrite( dataFrameRight, 1, (SCR_BLOCKSWIDTH+2)/2, file );
	}

	fclose(file);
}

static void WriteSinMove()
{
	FILE *file = fopen( "GL3DScrollSinX1.bin", "wb" );

	int tableSize = 256;
	unsigned char *raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 24.0f;
		float v = sin(rad-37)*radius;
				
		v = (radius*0.5f) + (v*0.5f);

		unsigned char finalV = (unsigned char) v;
		finalV += 0;

		raw[i] = finalV;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;

	file = fopen( "GL3DScrollSinX2.bin", "wb" );

	tableSize = 256;
	raw = new unsigned char[tableSize];
	for(int i = 0; i<tableSize; i++)
	{
		float deg = ((((float)i)/((float)tableSize))*360.0f)-180.0f;
		float rad = DEG2RAD(deg);
		float radius = 96.0f;
		float v = sin(rad)*radius;
				
		v = (radius*0.5f) + (v*0.5f);

		unsigned char finalV = (unsigned char) v;
		finalV += 10;

		raw[i] = finalV;
	}
	fwrite(raw, 1, tableSize, file);
	fclose(file);
	delete [] raw;
}

static char charData[5*(202)];

static void ReadData()
{
	FILE *file = fopen( "DawnOfVictory_BitField.raw", "rb" );
	fread(charData, 1, 5*(202), file);
	fclose(file);
}

static void DoScreenshot()
{
	char screenshotName[64];
    sprintf(screenshotName, "GL3DScrollRotateLogo%02d.bmp", screenshotCounter);
	char frameBinName[64];
    sprintf(frameBinName, "GL3DScroll%02d.bin", screenshotCounter);
	screenshotCounter++;
    glReadBuffer(GL_FRONT);
    glReadPixels(0, 0, SCR_BLOCKSWIDTH, SCR_BLOCKSHEIGHT, GL_RGB, GL_UNSIGNED_BYTE, image->pixels);
    int i;

	unsigned char *t = (unsigned char *)temp->pixels;
	unsigned char *img = (unsigned char *)image->pixels;

	for ( int y = 0; y<SCR_BLOCKSHEIGHT; y+=SCR_BLOCKSRATIO )
	{
		for ( int x = 0; x<SCR_BLOCKSWIDTH; x++ )
		{
			int tempOffset = ((SCR_BLOCKSHEIGHT-y-1)*SCR_BLOCKSWIDTH)+x;

			int tempOffset2 = ((y/SCR_BLOCKSRATIO)*SCR_BLOCKSWIDTH)+x;
				
			tempOffset*=3;				
			unsigned char r = img[tempOffset++];
			unsigned char g = img[tempOffset++];
			unsigned char b = img[tempOffset++];

			tempOffset2*=3;
			t[tempOffset2++] = r;
			t[tempOffset2++] = g;
			t[tempOffset2++] = b;
		}
	}

	WriteBinFrame(frameBinName, (unsigned char *)temp->pixels);

    SDL_SaveBMP(temp, screenshotName);
}

static const float fovy = 0.5f;
static const float posZ = -11.8f;

void GL3DScroll::Init()
{
	WriteSinMove();
	ReadData();

	Uint32 rmask, gmask, bmask, amask;
	amask = 0x000000;
	rmask = 0x0000ff;
	gmask = 0x00ff00;
	bmask = 0xff0000;

	temp = SDL_CreateRGBSurface(SDL_SWSURFACE, SCR_BLOCKSWIDTH, SCR_BLOCKSHEIGHT/SCR_BLOCKSRATIO, 24, rmask, gmask, bmask, amask);
    image = SDL_CreateRGBSurface(SDL_SWSURFACE, SCR_BLOCKSWIDTH, SCR_BLOCKSHEIGHT, 24, rmask, gmask, bmask, amask);
        
	float width = SCR_BLOCKSWIDTH;
	float height = SCR_BLOCKSHEIGHT;

	  /* Enable smooth shading */
    glShadeModel( GL_SMOOTH );

    /* Set the background black */
    glClearColor( 0.0f, 0.0f, 0.0f, 1.0f );

    /* Depth buffer setup */
    glClearDepth( 10.0f );

    /* Enables Depth Testing */
    glEnable( GL_DEPTH_TEST );

    /* The Type Of Depth Test To Do */
    glDepthFunc( GL_LESS );

    /* Really Nice Perspective Calculations */
    glHint( GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST );
	 GLfloat ratio;

    /* Protect against a divide by zero */
    if ( height == 0 )
	height = 1;

    ratio = ( GLfloat )width / ( GLfloat )height;

    /* Setup our viewport. */
    glViewport( 0, 0, ( GLsizei )width, ( GLsizei )height );

    /* change to the projection matrix and set our viewing volume. */
    glMatrixMode( GL_PROJECTION );
    glLoadIdentity( );

    /* Set our perspective */
    //gluPerspective( 45.0f, ratio, 0.1f, 100.0f );
	gluPerspective( fovy, ratio, 0.1f, 1000.0f );

    /* Make sure we're chaning the model view and not the projection */
    glMatrixMode( GL_MODELVIEW );

    /* Reset The View */
    glLoadIdentity( );
}

static float     rtri;                       // Angle For The Triangle ( NEW )
static int frameCount = 0;
static const int bitWidth = 5;
static const int bitHeight = 41;	
static GLfloat rquad=0;
static double cubeSize = 0.005f;
static double offsetX;
static double offsetY;

static void DrawQuads(char *bitData, int frameY)
{
	glLoadIdentity( );

	glTranslatef( 0, 0, posZ );
					
	glRotatef( rquad, 0.0f, -1.0f, 0.0f );
	
	for ( double y = 0; y < bitHeight; y++ )
	{
		for ( double x = 0; x < bitWidth; x++ )
		{
			int iX = (int)x;
			int iY = (int)y;

			double cubeWidth = cubeSize * 0.25f * CUBERATIO;
			double cubeHeight = cubeSize *0.25f;
			double cubeDepth = cubeSize * 0.35f;

			char bit = bitData[ ((iY+frameY)*bitWidth)+iX ];

			offsetX = (cubeWidth*x)-((cubeWidth*double(bitWidth))/2.0);
			offsetY = (cubeHeight*y)-((cubeHeight*double(bitHeight))/2.0);

			offsetY += cubeHeight*0.5f;

			cubeWidth /= 2;
			cubeDepth /= 2;

			offsetY *= -1*2;

			if ( bit != 0 )
			{
				glBegin(GL_QUADS);

				double min = 0;

				unsigned char *c = &colors[3];
	
				// front
				glColor3ub( *(c++), *(c++), *(c++));
				glVertex3f(-cubeWidth+offsetX, -cubeHeight+offsetY, cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, -cubeHeight+offsetY, cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, cubeHeight+offsetY-min, cubeDepth);
				glVertex3f(-cubeWidth+offsetX, cubeHeight+offsetY-min, cubeDepth);
				// back
				glColor3ub( *(c++), *(c++), *(c++));
				glVertex3f(-cubeWidth+offsetX, -cubeHeight+offsetY, -cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, -cubeHeight+offsetY, -cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, cubeHeight+offsetY-min, -cubeDepth);
				glVertex3f(-cubeWidth+offsetX, cubeHeight+offsetY-min, -cubeDepth);
				// right
				glColor3ub( *(c++), *(c++), *(c++));
				glVertex3f(cubeWidth+offsetX-min, -cubeHeight+offsetY, cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, -cubeHeight+offsetY, -cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, cubeHeight+offsetY-min, -cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, cubeHeight+offsetY-min, cubeDepth);
				// left
				glColor3ub( *(c++), *(c++), *(c++));
				glVertex3f(-cubeWidth+offsetX, -cubeHeight+offsetY, cubeDepth);
				glVertex3f(-cubeWidth+offsetX, -cubeHeight+offsetY, -cubeDepth);
				glVertex3f(-cubeWidth+offsetX, cubeHeight+offsetY-min, -cubeDepth);
				glVertex3f(-cubeWidth+offsetX, cubeHeight+offsetY-min, cubeDepth);
				// top
				glColor3ub( *(c++), *(c++), *(c++));
				glVertex3f(-cubeWidth+offsetX, cubeHeight+offsetY-min, cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, cubeHeight+offsetY-min, cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, cubeHeight+offsetY-min, -cubeDepth);
				glVertex3f(-cubeWidth+offsetX, cubeHeight+offsetY-min, -cubeDepth);
				// bottom
				glColor3ub( *(c++), *(c++), *(c++));
				glVertex3f(-cubeWidth+offsetX, -cubeHeight+offsetY, cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, -cubeHeight+offsetY, cubeDepth);
				glVertex3f(cubeWidth+offsetX-min, -cubeHeight+offsetY, -cubeDepth);
				glVertex3f(-cubeWidth+offsetX, -cubeHeight+offsetY, -cubeDepth);
				glEnd();
			}
		}
	}
}

void GL3DScroll::Draw()
{
	static GLfloat rtri=0;
    static GLint T0     = 0;
    static GLint Frames = 0;
	
	glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
	glEnable(GL_DEPTH_TEST);
	glColor3f( 1, 1, 1);
	glEnable(GL_TEXTURE_2D);

	/*char charData[bitWidth*bitHeight] = 
	{
		// A
		0,1,1,1,0,
		1,0,0,0,1,
		1,0,0,0,1,
		1,1,1,1,1,
		1,0,0,0,1,
		1,0,0,0,1,
		1,0,0,0,1,
		0,0,0,0,0,

		// B 8
		1,1,1,1,0,
		1,0,0,0,1,
		1,0,0,0,1,
		1,1,1,1,0,
		1,0,0,0,1,
		1,0,0,0,1,
		1,1,1,1,0,
		0,0,0,0,0,
		
		// A 16
		0,1,1,1,0,
		1,0,0,0,1,
		1,0,0,0,1,
		1,1,1,1,1,
		1,0,0,0,1,
		1,0,0,0,1,
		1,0,0,0,1,
		0,0,0,0,0,

		// B 24
		1,1,1,1,0,
		1,0,0,0,1,
		1,0,0,0,1,
		1,1,1,1,0,
		1,0,0,0,1,
		1,0,0,0,1,
		1,1,1,1,0,
		0,0,0,0,0,
		
		// A 32
		0,1,1,1,0,
		1,0,0,0,1,
		1,0,0,0,1,
		1,1,1,1,1,
		1,0,0,0,1,
		1,0,0,0,1,
		1,0,0,0,1,
		0,0,0,0,0,

		// B 40
		1,1,1,1,1
	};*/

	DrawQuads(charData, FrameY);
	
	glDisable(GL_BLEND);

    Frames++;
    {
	GLint t = SDL_GetTicks();
	if (t - T0 >= 5000) {
	    GLfloat seconds = (t - T0) / 1000.0;
	    GLfloat fps = Frames / seconds;
	    printf("%d frames in %g seconds = %g FPS\n", Frames, seconds, fps);
	    T0 = t;
	    Frames = 0;
	}   
    }


  SDL_GL_SwapBuffers();

  /*if ( frameCount < 17 )
  {
	DoScreenshot();
  }*/

  FrameY++;
  if ( FrameY <= 121+41+1 )
  {
	DoScreenshot();
  }

  rquad += 20.0f;
	
  frameCount++;
}

void GL3DScroll::Destroy()
{
    SDL_FreeSurface(image);
    SDL_FreeSurface(temp);
}