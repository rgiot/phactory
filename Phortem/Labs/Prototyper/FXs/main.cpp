
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "window.h"
#include "fxmanager.h"

#include "Default.h"
#include "Death.h"
#include "Debauche.h"
#include "Daemon.h"
#include "Rubber.h"
#include "Batman.h"
#include "Angel.h"
#include "TransitLines.h"
#include "GLIntro.h"
#include "GLCube.h"
#include "GL3DScroll.h"
#include "Intro.h"
#include "TransitBouncing.h"
#include "TransitCloudLines.h"
#include "Costix.h"
#include "InfiniteZoom.h"
#include "TransitRect.h"
#include "FireWorks.h"
#include "DoomTransit.h"

extern void CloudConvert();

int main( int argc, char* argv[] )
{
	if ( argc > 1 )
	{
		printf("SDL Template v1.0\n");
		return -1;
    }

	//CloudConvert();

    //CreateWindow(false, false); // software
	//CreateWindow(true, false); // opengl. no blocks
	CreateWindow(true, true); // opengl, with blocks

	//GetFXManager()->RegisterFX( new GLIntro() );
	//GetFXManager()->RegisterFX( new GLCube() );
	GetFXManager()->RegisterFX( new GL3DScroll() );

	//GetFXManager()->RegisterFX( new Default() );
	//GetFXManager()->RegisterFX( new Death() );
	//GetFXManager()->RegisterFX( new Debauche() );
	//GetFXManager()->RegisterFX( new Daemon() );
	//GetFXManager()->RegisterFX( new Rubber() );
	//GetFXManager()->RegisterFX( new TransitLines() );
	//GetFXManager()->RegisterFX( new Intro() );
	//GetFXManager()->RegisterFX( new Angel() );
	//GetFXManager()->RegisterFX( new Batman() );
	//GetFXManager()->RegisterFX( new TransitBouncing() );
	//GetFXManager()->RegisterFX( new TransitCloudLines() );
	//GetFXManager()->RegisterFX( new Costix() );
	//GetFXManager()->RegisterFX( new InfiniteZoom() );
	//GetFXManager()->RegisterFX( new TransitRect() );
	//GetFXManager()->RegisterFX( new FireWorks() );
	//GetFXManager()->RegisterFX( new DoomTransit() );
	
	while( 1 )
	{	
		StartFrame( );

		GetFXManager()->DrawFX();
		
		EndFrame( );
	}

	/* never reached, don't put any code here */	
	return 0; 
}

void ReleaseApp()
{
	GetFXManager()->Release();
}
