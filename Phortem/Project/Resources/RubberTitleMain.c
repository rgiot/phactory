
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

#include "RubberWriter.h"
#include "RubberWriter2.h"
#include "RubberTitle.h"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "Rubber.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{	
	// Load title
	PushBank( BANKC1 );
	LoadFile( RUBBERTITLEBIGFILE_TRACKFILEID, CODESEGMENT4000 );
	PopBank();
	
	// Load Tile (8-15 pixel index range)
	LoadFile( RUBBERTILE2BMPSPRRAWDATA1PCK_TRACKFILEID, VIDEOSEGMENT0000 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	
	// Load Cloud
	LoadFile( RUBBERTITLECLOUDBMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
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
	BankUnpack( 0xC0, VIDEOSEGMENT8000, RUBBERTITLEBMPTOPBINPCK_PTR );	
	BankUnpack( 0xC0, VIDEOSEGMENTC000, RUBBERTITLEBMPBOTTOMBINPCK_PTR );	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	// Prepare for part
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, CODESEGMENT0000_SIZE );
	
	DrawCloudReverse_VIDEOSEGMENT0000();
	
	FlipScreen();	
	
	LoadFile( RUBBERBACKGROUNDBMPBOTTOMBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENT0000 );
	PopBank();
	
	// Load Transit Data
	LoadFile( TRANSITLINESFX1BINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
	BankUnpack( 0xC4, 0x6880, VIDEOSEGMENT0000 );
	
	PushBank( BANKC1 );
	memcpy( DUMMYLINEPTR, VIDEOSEGMENT4000, 0x100 );
	LoadFile( RUBBERBACKGROUNDBMPTOPBINPCK_TRACKFILEID, CODESEGMENT0000 );
	Unpack( VIDEOSEGMENT0000, CODESEGMENT0000 );
	memcpy( VIDEOSEGMENT4000, DUMMYLINEPTR, 0x100 );
	PopBank();
	
	// Load Transit Code
	LoadFile( MAINDRAWTRIANGLEONLYZ80BINPCK_TRACKFILEID, 0x7000 );	
	Unpack( CODESEGMENT0000, 0x7000 );
	
	// Draw cloud of background
	//bank_memcpy( 0xC6, CODESEGMENT0000, CODESEGMENT4000, CODESEGMENT0000_SIZE );
	//DrawCloud_VIDEOSEGMENT8000();
	
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
	
	bank_memcpy( 0xC6, VIDEOSEGMENT0000+0x4000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	PushBank( BANKC1 );
	memcpy( VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	PopBank();
	
	LoadFile( RUBBERDATA2BIGFILEPCK_TRACKFILEID, CODESEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( CODESEGMENT4000, CODESEGMENT0000 );	
	PopBank();
	
	LoadFile( RUBBERWRITERBIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, RUBBERWRITERH_DATAPTR, VIDEOSEGMENT0000 );
	
	LoadFile( RUBBERWRITER2BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	bank_memcpy( 0xC6, RUBBERWRITER2H_DATAPTR, VIDEOSEGMENT0000, CODESEGMENT4000_SIZE );
	
	memcpy( VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT0000_SIZE );
}