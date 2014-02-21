
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "memcpy.cxx"
#include "memset.cxx"
#include "BankMemCopy.cxx" 
#include "BankUnpack.cxx"
#include "DrawTiles.cxx"

// ----------------------------------------------------------------------------
#include "InitLines.c"

// ----------------------------------------------------------------------------
void ShowHoriz2FX()
{
__asm
	ld hl, TransitVertLinesFXCode
	ld de, &BE00
	ld bc, TransitVertLinesFXCodeEnd-TransitVertLinesFXCode
	ldir
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei

	call &BE00
	
	ld a, 36
LoopHoriz2FX:
	push af
	
	call &BE03
	
LoopWaitVBL:
	ld a, &F5
	in a, (&DB)
	rra
	jr nc, LoopWaitVBL
	
	ld hl, &1000
	ld de, &1000
	ld bc, &200
	ldir
	
LoopWaitVBL2:
	ld a, &F5
	in a, (&DB)
	rra
	jr nc, LoopWaitVBL2

	pop af
	dec a
	jr nz, LoopHoriz2FX
	
	di
	pop af
	ld ( &0000 ), a
	ld b, &7F
	ld c, a
	out (c), c
	ei
	
	jp Transit1stFrameEnd
	
TransitVertLinesFXCode:
	incbin "TransitHoriz2FXCode.z80.bin"
TransitVertLinesFXCodeEnd:
	
Transit1stFrameEnd:
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	// Reset lines
	InitLines();
	
	LoadFile( INFINITEZOOMTILE2BMPSPRRAWDATA1PCK_TRACKFILEID_2, VIDEOSEGMENT0000 );
	BankUnpack( BANKC1, VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	
	ShowHoriz2FX();
}
