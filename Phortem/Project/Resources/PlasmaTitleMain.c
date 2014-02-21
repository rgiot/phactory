
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"
 
#include "memcpy.cxx"
#include "memset.cxx"
#include "BankMemCopy.cxx"
#include "BankUnpack.cxx"
#include "DrawCloud.cxx"
#include "DrawTiles.cxx"
#include "DrawEdges.cxx"
#include "ShiftTile.cxx"

#include "PlasmaTitle.h"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "Plasma.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	// Load Tile (8-15 pixel index range)
	LoadFile( PLASMATILE2BMPSPRRAWDATA1PCK_TRACKFILEID, VIDEOSEGMENT0000 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	
	// Load Transit Data
	LoadFile( TRANSITLINESFX2BINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
	BankUnpack( 0xC4, 0x6880, VIDEOSEGMENT0000 );
	
	// Load Transit Code
	LoadFile( MAINDRAWTRIANGLEONLYZ80BINPCK_TRACKFILEID_2, VIDEOSEGMENT0000 );	
	Unpack( CODESEGMENT0000, VIDEOSEGMENT0000 );
	
	// Draw Transit Line FX
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	bank_memcpy( 0xC6, 0x4080, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( 0xC7, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );
	bank_memcpy( 0xC4, 0x4080, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE);
	bank_memcpy( 0xC0, 0x7000, VIDEOSEGMENT4000, 0x1000 );
	bank_memcpy( 0xC0, 0xE800, 0x5000, 0x1800 );
__asm
	call _IsScreenFlipped
	ld a, l 
	call &2800
__endasm;
	// PATCH
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	
	// Load title
	PushBank( BANKC1 );
	LoadFile( PLASMATITLEBIGFILE_TRACKFILEID, CODESEGMENT4000 );
	PopBank();	
	
	// Load Cloud
	LoadFile( PLASMATITLECLOUDBMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
	BankUnpack( 0xC4, CODESEGMENT0000, VIDEOSEGMENT0000 );
	
	// Apply Part palette
	LoadFile( PLASMATILE2BMPSPRRAWDATA1PCK_TRACKFILEID_2, VIDEOSEGMENT0000 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	UnshiftTile( VIDEOSEGMENTC000 );	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	WaitVBL();	
	SetPalette( GetPalette() );	
	FlipScreen();
	
	// Load title
	BankUnpack( 0xC0, VIDEOSEGMENT8000, PLASMATITLEBMPTOPBINPCK_PTR );	
	BankUnpack( 0xC0, VIDEOSEGMENTC000, PLASMATITLEBMPBOTTOMBINPCK_PTR );	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	// Prepare for part
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, CODESEGMENT0000_SIZE );
	
	DrawCloudReverse_VIDEOSEGMENT0000();
	
	FlipScreen();
	
	// Load Cloud
	LoadFile( PLASMATITLECLOUD3BMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
	BankUnpack( 0xC4, CODESEGMENT0000, VIDEOSEGMENT0000 );
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, CODESEGMENT0000_SIZE );
	
	LoadFile( PLASMABACKGROUNDBMPTOPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC6, VIDEOSEGMENT4000, VIDEOSEGMENT0000 );
	
	LoadFile( PLASMADATA1BIGFILEPCK_TRACKFILEID, CODESEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( CODESEGMENT4000, CODESEGMENT0000 );
	PopBank();
	bank_memcpy( 0xC0, CODESEGMENTC000, CODESEGMENT4000, CODESEGMENT4000_SIZE );
	
	bank_memcpy( 0xC6, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENT0000_SIZE );
	
	LoadFile( PLASMABACKGROUNDBMPBOTTOMBINPCK_TRACKFILEID, CODESEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENT4000, CODESEGMENT0000 );
	PopBank();
	
	LoadFile( PLASMATITLECLOUD2BMPBINPCK_TRACKFILEID, VIDEOSEGMENTC000 );
}