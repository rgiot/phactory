
// ----------------------------------------------------------------------------
#ifndef _LOADFILE_CXX_
#define _LOADFILE_CXX_

// ----------------------------------------------------------------------------
#include "TrackLoader.cxx"

// ----------------------------------------------------------------------------
void LoadFile( unsigned char fileIdentifier, char *destination )
{
	fileIdentifier;
	destination; 

__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	ld	a, (ix+4)
	ld	l, (ix+5)
	ld	h, (ix+6)	
	call LoadFileDirect
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _LOADFILE_CXX_
