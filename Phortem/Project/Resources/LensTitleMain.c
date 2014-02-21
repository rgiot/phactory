
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

#include "LensData1.h"
#include "LensData2.h"
#include "LensTitle.h"

// ----------------------------------------------------------------------------
#include "InitLines.c"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "Lens.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	// Fix crash for Lens
	InitLines();
	
	// Load title
	PushBank( BANKC1 );
	LoadFile( LENSTITLEBIGFILE_TRACKFILEID, CODESEGMENT4000 );
	PopBank();
	
	// Load Tile (8-15 pixel index range)
	LoadFile( LENSTILE2BMPSPRRAWDATA1PCK_TRACKFILEID, VIDEOSEGMENT0000 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	
	// Load Cloud
	LoadFile( LENSTITLECLOUDBMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
	BankUnpack( 0xC4, CODESEGMENT0000, VIDEOSEGMENT0000 );

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
	
	// Load title
	BankUnpack( 0xC0, VIDEOSEGMENT8000, LENSTITLEBMPTOPBINPCK_PTR );	
	BankUnpack( 0xC0, VIDEOSEGMENTC000, LENSTITLEBMPBOTTOMBINPCK_PTR );	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	// Prepare for part
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, CODESEGMENT0000_SIZE );
	
	DrawCloudReverse_VIDEOSEGMENT0000();
	
	FlipScreen();
	
	LoadFile( LENSDATA1BIGFILEPCK_TRACKFILEID, CODESEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( CODESEGMENT4000, CODESEGMENT0000 );
	PopBank();
	bank_memcpy( 0xC0, CODESEGMENTC000, CODESEGMENT4000, CODESEGMENT4000_SIZE );
	
	LoadFile( LENSDATA2BIGFILEPCK_TRACKFILEID, CODESEGMENT0000 );
	PushBank( BANKC3 );
	Unpack( LENSCIRCLEBMPSPRZ802_PTR, CODESEGMENT0000 );	
	PopBank();
	
	LoadFile( LENSBACKGROUNDBMPBOTTOMBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENT0000 );
	PopBank();
	
	LoadFile( LENSBACKGROUNDBMPTOPBINPCK_TRACKFILEID, CODESEGMENT0000 );
	Unpack( VIDEOSEGMENT0000, CODESEGMENT0000 );
	
	bank_memcpy( 0xC6, VIDEOSEGMENT0000+0x4000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	PushBank( BANKC1 );
	memcpy( VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	PopBank();
	
	// Draw cloud of background
	bank_memcpy( 0xC6, CODESEGMENT0000, CODESEGMENT4000, CODESEGMENT0000_SIZE );
	DrawCloud_VIDEOSEGMENT8000();
	
	LoadFile( LENSWRITERBIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, 0x4000, VIDEOSEGMENT0000 );
	
	memcpy( VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT0000_SIZE );
}