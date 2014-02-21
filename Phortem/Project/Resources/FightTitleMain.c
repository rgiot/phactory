
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "BankMemCopy.cxx"
#include "BankUnpack.cxx"
#include "DrawCloud.cxx"
#include "DrawTiles.cxx"
#include "ShiftTile.cxx"

#include "FightTitleMain1.h"
#include "FightTitleMain2.h"
#include "FightPhysics.h"

// ----------------------------------------------------------------------------
void TransitFrameToVIDEOSEGMENT8000();

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, MangaPalette
	jr QuitGetPalette
MangaPalette:
	incbin "Fight.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void DoFx()
{
	if ( IsScreenFlipped()!=0 )
	{
		FlipScreen();
	}
	
__asm
	call &BC00
	
FrameLoop:
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	xor a
	call &BC03
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	call _FlipScreen
	call _WaitVBL
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	ld a, 1
	call &BC03
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	call _FlipScreen
	call _WaitVBL
	
FrameLoopCounter:
	ld hl, 76/2
	dec hl
	ld (FrameLoopCounter+1), hl
	ld a, h
	or l
	jp nz, FrameLoop
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	InitFirst();
	
	bank_memcpy( 0xC4, 0x4080, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( 0xC7, 0x4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );
	
	BankUnpack( 0xC0, 0x2800, PHYSICSFXDATAZ80BINPCK_PTR );		
	
	DoFx();
}

// ----------------------------------------------------------------------------
void EndInfiniteZoom();

// ----------------------------------------------------------------------------
void InitFirst()
{
	EndInfiniteZoom();
	
	LoadFile( FIGHTTITLEMAIN1BIGFILE_TRACKFILEID, FIGHTTITLEMAIN1H_DATAPTR );

	// Title
	bank_memcpy( 0xC0, CODESEGMENT4000, FIGHTTITLEBMPTOPBINPCK_PTR, FIGHTTITLEBMPTOPBINPCK_SIZE );
	bank_memcpy( 0xC0, CODESEGMENTC000, FIGHTTITLEBMPBOTTOMBINPCK_PTR, FIGHTTITLEBMPBOTTOMBINPCK_SIZE );
	
	// Tile (8-15 pixel index range)
	Unpack( VIDEOSEGMENTC000, FIGHTTILE2BMPSPRRAWDATA1PCK_PTR );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	
	// Cloud
	BankUnpack( 0xC4, CODESEGMENT0000, FIGHTTITLECLOUDBMPBINPCK_PTR );
	
	// Draw Cloud
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	DrawCloud_VIDEOSEGMENT8000();
	
	// Apply Part palette
	UnshiftTile( VIDEOSEGMENTC000 );
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	WaitVBL();	
	SetPalette( GetPalette() );	
	FlipScreen();
	
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	FlipScreen();
	
	// Load title
	BankUnpack( 0xC0, VIDEOSEGMENT0000, CODESEGMENT4000 );	
	BankUnpack( 0xC0, VIDEOSEGMENT4000, CODESEGMENTC000 );	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	// Prepare for part
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, 0x200 /*CODESEGMENT0000_SIZE*/ );
	
	TransitFrameToVIDEOSEGMENT8000();
	
	bank_memcpy( 0xC6, VIDEOSEGMENT0000+0x4000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC1, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	
	LoadFile( FIGHTTITLEMAIN2BIGFILE_TRACKFILEID, FIGHTTITLEMAIN2H_DATAPTR );	
	BankUnpack( 0xC1, VIDEOSEGMENT4000, FIGHTBACKGROUNDBMPBOTTOMBINPCK_PTR );
	Unpack( VIDEOSEGMENT0000, FIGHTBACKGROUNDBMPTOPBINPCK_PTR );
	
	PushBank( BANKC1 );
	LoadFile( FIGHTPHYSICSBIGFILE_TRACKFILEID, FIGHTPHYSICSH_DATAPTR );	
	Unpack( 0xBC00, PHYSICSFXCODEZ80BINPCK_PTR );	
	PopBank();
	
	// Draw cloud of background
	bank_memcpy( 0xC6, CODESEGMENT0000, CODESEGMENT4000, 0x200 /*CODESEGMENT0000_SIZE*/ );
	DrawCloudReverse_VIDEOSEGMENT8000();
}

// ----------------------------------------------------------------------------
char *GetTransitVertLinesFXCode_Packed()
{
__asm
	ld hl, FXCode2
	jp QuitFXCode2
FXCode2:
	incbin "TransitVertLinesFXCode2_BD00.z80.bin.pck"
QuitFXCode2:
__endasm;
}

// ----------------------------------------------------------------------------
void TransitFrameToVIDEOSEGMENT8000()
{
	Unpack( 0xBD00, GetTransitVertLinesFXCode_Packed() );
	
__asm
	call &BD00

	ld a, 37
TransitLoop2:
	push af
	call &BD03
	pop af
	dec a
	jr nz, TransitLoop2
__endasm;
}

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
#include "InitLines.c"

// ----------------------------------------------------------------------------
void EndInfiniteZoom()
{
	InitLines();
	
	LoadFile( INFINITEZOOMTILE2BMPSPRRAWDATA1PCK_TRACKFILEID_2, VIDEOSEGMENT0000 );
	BankUnpack( BANKC1, VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	
	ShowHoriz2FX();
}
	
	