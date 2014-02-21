
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "BankMemCopy.cxx"
#include "BankUnpack.cxx"
 
#include "DrawTiles.cxx"
#include "DrawCloud.cxx"
#include "ClearScreen.cxx"

#include "FightFX.h"

#include "FightMain1.h"

// ----------------------------------------------------------------------------
unsigned char frame;

// ----------------------------------------------------------------------------
void DoFx()
{
	BankUnpack( 0xC0, 0xB750, FIGHTFXCODE_B800BINPCK_PTR );	
	
	bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );	
	if ( IsScreenFlipped()!=0 )
	{
		FlipScreen();
	}
	
__asm
	call &B750
	
FightFrameLoop:
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	ld a, 1
	call &B753
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	call _FlipScreen
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	xor a
	call &B753
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	call _FlipScreen
	
FightFrameLoopCounter:
	ld hl, 162/2
	dec hl
	ld (FightFrameLoopCounter+1), hl
	ld a, h
	or l
	jp nz, FightFrameLoop
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	InitFx();	
	DoFx();
	
	LoadFile( FIGHTMAIN1BIGFILE_TRACKFILEID, FIGHTMAIN1H_DATAPTR );
	
	// Tile
	BankUnpack( BANKC1, VIDEOSEGMENTC000, FIGHTTILE2BMPSPRRAWDATA1PCK_PTR );
	
	// Cloud
	BankUnpack( 0xC4, CODESEGMENT0000, FIGHTTITLECLOUD2BMPBINPCK_PTR );
	
	// Tile
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	DrawCloudReverse_VIDEOSEGMENT8000();
	
	bank_memcpy( 0xC1, 0xC000, CODESEGMENT0000, CODESEGMENT0000_SIZE );
}

// ----------------------------------------------------------------------------
void InitFx()
{
	bank_memcpy( 0xC4, 0x4000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( 0xC7, 0x4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );		
	
	LoadFile( FIGHTFXBIGFILE_TRACKFILEID, FIGHTFXH_DATAPTR );	
	BankUnpack( 0xC0, 0x6800, FIGHTFXDATA_C0_6800BINPCK_PTR );	
	BankUnpack( 0xC0, 0xE800, FIGHTFXDATA_C0_E800BINPCK_PTR );	
	BankUnpack( 0xC4, 0x6800, FIGHTFXDATA_C4_6800BINPCK_PTR );	
	BankUnpack( 0xC0, 0x3800, FIGHTFXCODE_3800BINPCK_PTR );	
//	bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, 0x0100 );	
}