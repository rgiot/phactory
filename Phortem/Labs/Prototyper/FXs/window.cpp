
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <SDL.h>
#include "config.h"
#include "window.h"
#include "fxmanager.h"
#include "main.h"

SDL_Surface *s_sdlSurface;
static unsigned int *s_buffer;

SDL_Surface *GetSDLSurface()
{
	return s_sdlSurface;
}

unsigned int *GetBuffer()
{
	return s_buffer;
}

void QuitApp()
{
	ReleaseApp();	
	SDL_Quit();
	exit( 0 );	
}

void ProcessEvents()
{
	SDL_Event event;

    SDL_Delay(20);
		
	while( SDL_PollEvent( &event ) )
	{
        switch( event.type )
		{
		case SDL_KEYDOWN:
			{
				SDLKey key = event.key.keysym.sym;

				switch( key )
				{
				case SDLK_ESCAPE:
					QuitApp();
					break;

				default:
					GetFXManager()->KeyPress( key );
					break;
				}
			}
            break;
        case SDL_QUIT:
            QuitApp();
			break;
		}
    }
}

void CreateWindow(bool useGL, bool useBlocks)
{
	if ( SDL_Init(SDL_INIT_VIDEO) < 0 )
	{
		fprintf( stderr, "Couldn't initialize SDL: %s\n", SDL_GetError() );
		exit(1);
	}

	unsigned int flags = 0;
	if ( useGL )
	{
		    flags  = SDL_OPENGL;          /* Enable OpenGL in SDL */
		flags |= SDL_GL_DOUBLEBUFFER; /* Enable double buffering */
		flags |= SDL_HWPALETTE;       /* Store the palette in hardware */
		flags |= SDL_RESIZABLE;       /* Enable window resizing */
		flags |= SDL_SWSURFACE;
	
		if ( useBlocks )
		{
			s_sdlSurface = SDL_SetVideoMode( SCR_BLOCKSWIDTH, SCR_BLOCKSHEIGHT, 32, flags );
		}
		else
		{
			s_sdlSurface = SDL_SetVideoMode( 384/2, 205, 32, flags );
		}
	}
	else
	{
		flags = SDL_SWSURFACE | SDL_HWPALETTE;
	s_sdlSurface = SDL_SetVideoMode( SCR_WIDTH*4, SCR_HEIGHT*2, 32, flags );
	}
	
	if ( s_sdlSurface == NULL )
	{
		fprintf(stderr, "Couldn't set display mode: %s\n", SDL_GetError() );

		SDL_Quit();
		exit(5);
	}
}

void StartFrame()
{
	ProcessEvents();

    if ( SDL_MUSTLOCK( s_sdlSurface ) )
	{
		if ( SDL_LockSurface( s_sdlSurface ) < 0 )
		{
			fprintf(stderr, "Couldn't lock display surface: %s\n", SDL_GetError( ) );
			exit( -1 );
		}
	}

	s_buffer = (unsigned int *) s_sdlSurface->pixels;
}

void EndFrame()
{
	if ( SDL_MUSTLOCK( s_sdlSurface ) )
	{
		SDL_UnlockSurface( s_sdlSurface );
	}

	SDL_UpdateRect( s_sdlSurface, 0, 0, 0, 0 );
}

unsigned int GetTicks()
{
	return SDL_GetTicks();
}