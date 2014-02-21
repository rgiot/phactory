
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

#include "AngelTitle1.h"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, AngelPalette
	jr QuitGetPalette
AngelPalette:
	incbin "Angel.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	PushBank( BANKC1 );
	LoadFile( ANGELTITLE1BIGFILE_TRACKFILEID, ANGELTITLE1H_DATAPTR );
	
	// TITLE TOP IN C1-CODESEGMENT4000
	memcpy( CODESEGMENT4000, ANGELTITLEBMPTOPBINPCK_PTR, ANGELTITLEBMPTOPBINPCK_SIZE );
	PopBank();
	
	// TITLE BOTTOM IN C3-CODESEGMENT4000
	bank_memcpy( 0xC0, CODESEGMENTC000, ANGELTITLEBMPBOTTOMBINPCK_PTR, ANGELTITLEBMPBOTTOMBINPCK_SIZE );
	
	Unpack( VIDEOSEGMENTC000, ANGELTILEBMPSPRRAWDATA1PCK_PTR );
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );	
	
	// Cloud
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
	
	// Load Cloud
	LoadFile( ANGELTITLECLOUDBMPBINPCK_TRACKFILEID, VIDEOSEGMENT8000 );	
	
	BankUnpack( 0xC4, CODESEGMENT0000, VIDEOSEGMENT8000 );
	BankUnpack( 0xC1, VIDEOSEGMENT8000, CODESEGMENT4000 );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, CODESEGMENTC000 );
	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	DrawCloudReverse_VIDEOSEGMENT0000();
}