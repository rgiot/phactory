
// ----------------------------------------------------------------------------
#ifndef _CLEARSCREEN_CXX_
#define _CLEARSCREEN_CXX_

// ----------------------------------------------------------------------------
void ClearScreen()
{
__asm
	ld hl, VIDEOSEGMENT0000
	ld ( hl ), 0
	ld de, VIDEOSEGMENT0000+1
	ld bc, VIDEOSEGMENT0000_SIZE-1
	ldir
	ld hl, VIDEOSEGMENT8000
	ld ( hl ), 0
	ld de, VIDEOSEGMENT8000+1
	ld bc, VIDEOSEGMENT8000_SIZE-1
	ldir
__endasm;

	PushBank( BANKC1 );
__asm
	ld hl, VIDEOSEGMENT4000
	ld ( hl ), 0
	ld de, VIDEOSEGMENT4000+1
	ld bc, VIDEOSEGMENT4000_SIZE-1
	ldir
__endasm;
	PopBank();

	PushBank( BANKC3 );
__asm
	ld hl, VIDEOSEGMENT4000
	ld ( hl ), 0
	ld de, VIDEOSEGMENT4000+1
	ld bc, VIDEOSEGMENT4000_SIZE-1
	ldir
__endasm;
	PopBank();
}

// ----------------------------------------------------------------------------
#endif // _DRAWEDGES_CXX_
