
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

#include "GLCube.h"
#include "Texture.h"

GLCube::GLCube() :
	FXBase()
{
}

GLCube::~GLCube()
{
}

SDLKey GLCube::GetKey()
{
	return SDLK_1;
}

const char *GLCube::GetName()
{
	return "GLCube";
}

static GLuint texture;
static Texture treeTexture;
static Texture bgTexture;
static int screenshotCounter = 1;

static SDL_Surface *temp;
static SDL_Surface *image;

// 308*76

static const int logoHeight = 205;
static const int logoWidth = 384/2;
static const int logoX = ((384/2)-logoWidth)/2;
static const int logoY = (205-logoHeight)/2;

static void DoScreenshot()
{
	float screenWidth = 384/2;
	float screenHeight = 205;

	    char screenshotName[64];
        sprintf(screenshotName, "CubeRotateLogo%02d.bmp", screenshotCounter);
		screenshotCounter++;
        glReadBuffer(GL_FRONT);
        glReadPixels(0, 0, screenWidth, screenHeight, GL_RGB, GL_UNSIGNED_BYTE, image->pixels);
        int i;

		unsigned char *t = (unsigned char *)temp->pixels;
		unsigned char *img = (unsigned char *)image->pixels;

		for ( int y = 0; y<logoHeight; y++ )
		{
			for ( int x = 0; x<logoWidth; x++ )
			{
				int tempOffset = ((logoHeight-y-1)*logoWidth)+x;
				int imageOffset = ((logoY+y+3)*(screenWidth))+x+logoX;

				tempOffset*=3*2;
				imageOffset*=3;

				unsigned char r = img[imageOffset++];
				unsigned char g = img[imageOffset++];
				unsigned char b = img[imageOffset++];

				if ( g != 255 )
				{
					if ( (r<5)&&(g>250)&&(b<5))
					{
						r = 0;
						g = 255;
						b = 0;
					}
				}

				t[tempOffset++] = r;
				t[tempOffset++] = g;
				t[tempOffset++] = b;

				t[tempOffset++] = r;
				t[tempOffset++] = g;
				t[tempOffset++] = b;
			}
		}
        SDL_SaveBMP(temp, screenshotName);
}

void GLCube::Init()
{
	float screenWidth = 384/2;
	float screenHeight = 205;
        Uint32 rmask, gmask, bmask, amask;
        amask = 0x000000;
      rmask = 0x0000ff;
            gmask = 0x00ff00;
            bmask = 0xff0000;
	temp = SDL_CreateRGBSurface(SDL_SWSURFACE, logoWidth*2, logoHeight, 24, rmask, gmask, bmask, amask);
    image = SDL_CreateRGBSurface(SDL_SWSURFACE, screenWidth, screenHeight, 24, rmask, gmask, bmask, amask);
        

	float width = 384/2;
	float height = 205;

	  /* Enable smooth shading */
    glShadeModel( GL_SMOOTH );

    /* Set the background black */
    glClearColor( 0.0f, 0.0f, 0.0f, 1.0f );

    /* Depth buffer setup */
    glClearDepth( 10.0f );

    /* Enables Depth Testing */
    glEnable( GL_DEPTH_TEST );

    /* The Type Of Depth Test To Do */
    glDepthFunc( GL_LEQUAL );

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
    gluPerspective( 45.0f, ratio, 0.1f, 100.0f );

    /* Make sure we're chaning the model view and not the projection */
    glMatrixMode( GL_MODELVIEW );

    /* Reset The View */
    glLoadIdentity( );

	if (LoadTGA(&treeTexture, "Phortem.tga")) 
	{ 
		// This tells opengl to create 1 texture and put it's ID in the given integer variable
		// OpenGL keeps a track of loaded textures by numbering them: the first one you load is 1, second is 2, ...and so on.
		glGenTextures(1, &treeTexture.texID);
		// Binding the texture to GL_TEXTURE_2D is like telling OpenGL that the texture with this ID is now the current 2D texture in use
		// If you draw anything the used texture will be the last binded texture
		glBindTexture(GL_TEXTURE_2D, treeTexture.texID);
		// This call will actualy load the image data into OpenGL and your video card's memory. The texture is allways loaded into the current texture
		// you have selected with the last glBindTexture call
		// It asks for the width, height, type of image (determins the format of the data you are giveing to it) and the pointer to the actual data
		unsigned int *img32 = new unsigned int[treeTexture.width * treeTexture.height];

		for( int y = 0; y < treeTexture.height; y++ )
		{
			for ( int x = 0; x < treeTexture.width; x++ )
			{
				unsigned char b = treeTexture.imageData[ ( y*treeTexture.width + x ) * 3 ];
				unsigned char g = treeTexture.imageData[ (( y*treeTexture.width + x ) * 3) +1];
				unsigned char r = treeTexture.imageData[ (( y*treeTexture.width + x ) * 3) +2];

				unsigned int texel = (r<<16)+(g<<8)+b;
				if ( texel != 0x00FF00 ) 
				{
					//texel |= 0xFF000000;
				}
				img32[( y*treeTexture.width) + x ] = texel;
			}
		}
		
		glTexImage2D(GL_TEXTURE_2D, 0, 4, treeTexture.width, treeTexture.height, 0, GL_RGBA, GL_UNSIGNED_BYTE, img32);

		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_NEAREST);
		glEnable(GL_TEXTURE_2D);
		if (treeTexture.imageData) 
		{
			free(treeTexture.imageData);
		}
	}

	if (LoadTGA(&bgTexture, "IntroBackground.tga")) 
	{ 
		// This tells opengl to create 1 texture and put it's ID in the given integer variable
		// OpenGL keeps a track of loaded textures by numbering them: the first one you load is 1, second is 2, ...and so on.
		glGenTextures(1, &bgTexture.texID);
		// Binding the texture to GL_TEXTURE_2D is like telling OpenGL that the texture with this ID is now the current 2D texture in use
		// If you draw anything the used texture will be the last binded texture
		glBindTexture(GL_TEXTURE_2D, bgTexture.texID);
		// This call will actualy load the image data into OpenGL and your video card's memory. The texture is allways loaded into the current texture
		// you have selected with the last glBindTexture call
		// It asks for the width, height, type of image (determins the format of the data you are giveing to it) and the pointer to the actual data
		glTexImage2D(GL_TEXTURE_2D, 0, bgTexture.bpp / 8, bgTexture.width, bgTexture.height, 0, bgTexture.type, GL_UNSIGNED_BYTE, bgTexture.imageData);

		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_NEAREST);
		glEnable(GL_TEXTURE_2D);
		if (bgTexture.imageData) 
		{
			free(bgTexture.imageData);
		}
	}
}

static float     rtri;                       // Angle For The Triangle ( NEW )

static int frameCount = 0;

void GLCube::Draw()
{
	   static GLfloat rtri=0, rquad=0;
    /* These are to calculate our fps */
    static GLint T0     = 0;
    static GLint Frames = 0;


    /* Clear The Screen And The Depth Buffer */
    glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );

	glEnable(GL_DEPTH_TEST);

    
	
    glLoadIdentity( );
    glTranslatef( 0, 0.0f, -2.0f );

	glRotatef( 0.0f, 0.0f, 0.0f, 1.0f );
	
	
    glLoadIdentity( );
	glTranslatef( 0.0f, 0.0f, -8.0f );

	glRotatef( rquad, 0.0f, 0.0f, 1.0f );
    glRotatef( rquad, 0.0f, -1.0f, 0.0f );
	glRotatef( rquad, 1.0f, 0.0f, 0.0f );

    glColor3f( 1, 1, 1);
	glEnable(GL_TEXTURE_2D);

	//glBindTexture (GL_TEXTURE_2D, treeTexture.texID);

    glBegin(GL_QUADS);
	
		// front
		glColor3f( 1, 0, 0);
		glVertex3f(-1.0f, -1.0f, 1.0f);
		glVertex3f(1.0f, -1.0f, 1.0f);
		glVertex3f(1.0f, 1.0f, 1.0f);
		glVertex3f(-1.0f, 1.0f, 1.0f);
		// back
		glColor3f( 0, 1, 0);
		glVertex3f(-1.0f, -1.0f, -1.0f);
		glVertex3f(1.0f, -1.0f, -1.0f);
		glVertex3f(1.0f, 1.0f, -1.0f);
		glVertex3f(-1.0f, 1.0f, -1.0f);
		// right
		glColor3f( 1, 0, 1);
		glVertex3f(1.0f, -1.0f, 1.0f);
		glVertex3f(1.0f, -1.0f, -1.0f);
		glVertex3f(1.0f, 1.0f, -1.0f);
		glVertex3f(1.0f, 1.0f, 1.0f);
		// left
		glColor3f( 0, 0, 1);
		glVertex3f(-1.0f, -1.0f, 1.0f);
		glVertex3f(-1.0f, -1.0f, -1.0f);
		glVertex3f(-1.0f, 1.0f, -1.0f);
		glVertex3f(-1.0f, 1.0f, 1.0f);
		// top
		glColor3f( 0, 1, 1);
		glVertex3f(-1.0f, 1.0f, 1.0f);
		glVertex3f(1.0f, 1.0f, 1.0f);
		glVertex3f(1.0f, 1.0f, -1.0f);
		glVertex3f(-1.0f, 1.0f, -1.0f);
		// bottom
		glColor3f( 1, 1, 0);
		glVertex3f(-1.0f, -1.0f, 1.0f);
		glVertex3f(1.0f, -1.0f, 1.0f);
		glVertex3f(1.0f, -1.0f, -1.0f);
		glVertex3f(-1.0f, -1.0f, -1.0f);
	glEnd();

	
	glDisable(GL_BLEND);

    /* Draw it to the screen */
    SDL_GL_SwapBuffers( );

    /* Gather our frames per second */
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

	 //rquad   = (float)SDL_GetTicks();//+= 5.0f;
	 //rquad*= -0.1f;
   
    }


    /* Increase The Rotation Variable For The Triangle ( NEW ) */
    //rtri   = (float)t;//+= 5.0f;
    /* Decrease The Rotation Variable For The Quad     ( NEW ) */
    //rquad -=0.15f;


  SDL_GL_SwapBuffers();

  if ( frameCount < 18 )
  {
	DoScreenshot();
  }

  //rquad += 8.0f;
  rquad += 10.0f;
	
  frameCount++;
}

void GLCube::Destroy()
{
        SDL_FreeSurface(image);
        SDL_FreeSurface(temp);
}