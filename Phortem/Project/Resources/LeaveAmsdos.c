
// ----------------------------------------------------------------------------
#include "Config.h"

// ----------------------------------------------------------------------------
unsigned char frame;

unsigned char *screenPtr1;
unsigned char *screenPtr2;

// ----------------------------------------------------------------------------
void DrawFrame()
{
}

// ----------------------------------------------------------------------------
void LeaveAmsdos()
{
	unsigned char i;
	
	screenPtr1 = 0xC000;
	screenPtr2 = 0x0000;
	
	while ( screenPtr1 != screenPtr2 )
	{
		screenPtr2--;
		
		for ( i = 0; i < 4; i++ )
		{		
			*screenPtr1 = 0x00;
			*screenPtr2 = 0x00;
		}
		
		screenPtr1++;
	}
}

