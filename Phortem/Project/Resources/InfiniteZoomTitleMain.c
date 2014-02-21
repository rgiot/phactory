
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "BankUnpack.cxx"
#include "ShiftTile.cxx"

#include "InfiniteZoom.h"
#include "InfiniteZoomData1.h"
#include "InfiniteZoomData2.h"
#include "InfiniteZoomData3.h"
#include "TransitCircle.h"

#include "InfiniteZoomTitle1.h"
#include "InfiniteZoomTitle2.h"
#include "InfiniteZoomData3.h"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, MangaPalette
	jr QuitGetPalette
MangaPalette:
	incbin "InfiniteZoom.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetTransitVertLinesFXCode_Packed()
{
__asm
	ld hl, FXCode
	jp QuitFXCode
FXCode:
	incbin "TransitVertSinLinesFXCode.z80.bin.pck"
QuitFXCode:
__endasm;
}

// ----------------------------------------------------------------------------
void TransitFrameToVIDEOSEGMENT8000()
{
	Unpack( 0xBD00, GetTransitVertLinesFXCode_Packed() );
	
__asm
	call &BD00

	ld a, 35
TransitLoop:
	push af
	call &BD03
	pop af
	dec a
	jr nz, TransitLoop	
__endasm;
}

// ----------------------------------------------------------------------------
void Init();
void bank_memcpy( unsigned char destBank, char *destination, char *source, unsigned int length );

// ----------------------------------------------------------------------------
void Main()
{	
	Init();
	
	LoadFile( INFINITEZOOMBIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC0, 0x7300, Z02BMPSPRRAWDATA1PCK_PTR );
	BankUnpack( 0xC4, 0x7300, Z02BMPSPRRAWDATA1PCK_PTR );
	BankUnpack( 0xC6, 0x7300, Z02BMPSPRRAWDATA1PCK_PTR );
	BankUnpack( 0xC0, 0xE800, Z03BMPSPRRAWDATA1PCK_PTR );
	BankUnpack( 0xC0, 0xF100, Z04BMPSPRRAWDATA1PCK_PTR );
	BankUnpack( 0xC0, 0xFA00, INFINITEZOOMBACKGROUNDPIXELEDGESBMPSPRDATA1PCK_PTR );
	
	LoadFile( INFINITEZOOMDATA1BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, 0x4000, INFINITEZOOMDATA00BINPCK_PTR );
	BankUnpack( 0xC4, 0x4CBC, INFINITEZOOMDATA01BINPCK_PTR );
	BankUnpack( 0xC4, 0x5978, INFINITEZOOMDATA02BINPCK_PTR );
	
	PushBank( BANKC1 );
	LoadFile( INFINITEZOOMBACKGROUNDBMPBOTTOMBINPCK_TRACKFILEID, VIDEOSEGMENTC000 );
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	LoadFile( INFINITEZOOMBACKGROUNDBMPTOPBINPCK_TRACKFILEID, VIDEOSEGMENTC000 );
	Unpack( VIDEOSEGMENT0000, VIDEOSEGMENTC000 ); // REUSED IN MAIN PART !
	PopBank();	
	
	TransitFrameToVIDEOSEGMENT8000();
	
	LoadFile( INFINITEZOOMDATA2BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, 0x6634, INFINITEZOOMDATA03BINPCK_PTR );
	BankUnpack( 0xC6, 0x4000, INFINITEZOOMDATA04BINPCK_PTR );
	BankUnpack( 0xC6, 0x4CBC, INFINITEZOOMDATA05BINPCK_PTR );
}

// ----------------------------------------------------------------------------
#include "BankMemCopy.cxx"

// ----------------------------------------------------------------------------
void InitFirst();

// ----------------------------------------------------------------------------
#include "DrawCloud.cxx"
#include "DrawTiles.cxx"

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
	
	pop af
	cp 1
	jr z, SkipWaitVbl
	push af
	
LoopWaitVBL:
	ld a, &F5
	in a, (&DB)
	rra
	jr nc, LoopWaitVBL
	
	ld hl, &1000
	ld de, &1000
	ld bc, &100
	ldir
	
LoopWaitVBL2:
	ld a, &F5
	in a, (&DB)
	rra
	jr nc, LoopWaitVBL2

	pop af
SkipWaitVbl:
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
void Init()
{
	InitFirst();
	
	DrawCloudReverse_VIDEOSEGMENT8000();
	
	// Apply Part palette
	UnshiftTile( VIDEOSEGMENTC000 );
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	WaitVBL();	
	SetPalette( GetPalette() );	
	FlipScreen();
	
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENTC000_SIZE );
	FlipScreen();
	
	// Load Title
	bank_memcpy( 0xC0, INFINITEZOOMTITLEBMPTOPBINPCK_PTR, 0xF000, 0x0810 );
	BankUnpack( 0xC0, VIDEOSEGMENT0000, INFINITEZOOMTITLEBMPTOPBINPCK_PTR );
	BankUnpack( 0xC0, VIDEOSEGMENT4000, INFINITEZOOMTITLEBMPBOTTOMBINPCK_PTR );
	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	ShowHoriz2FX();
}

// ----------------------------------------------------------------------------
void InitFirst()
{	
	// Load Title
	PushBank( BANKC1 );
	LoadFile( INFINITEZOOMTITLE1BIGFILE_TRACKFILEID, INFINITEZOOMTITLE1H_DATAPTR );
	PopBank();
	bank_memcpy( 0xC0, 0xF000, INFINITEZOOMTITLEBMPTOPBINPCK_PTR, 0x0810 );
	
	PushBank( BANKC1 );
	LoadFile( INFINITEZOOMTITLE2BIGFILE_TRACKFILEID, INFINITEZOOMTITLE2H_DATAPTR );
	PopBank();		
	
	// Tile (8-15 pixel index range)
	Unpack( VIDEOSEGMENTC000, INFINITEZOOMTILE2BMPSPRRAWDATA1PCK_PTR );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	// Load Cloud
	BankUnpack( 0xC4, CODESEGMENT0000, INFINITEZOOMTITLECLOUDBMPBINPCK_PTR );
	
	// Draw Vert Lines Transit
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
}