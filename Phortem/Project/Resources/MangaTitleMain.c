
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
#include "ShiftTile.cxx"
#include "MangaTitle.h"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, MangaPalette
	jr QuitGetPalette
MangaPalette:
	incbin "Manga.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	// Load title
	PushBank( BANKC3 );
	LoadFile( MANGATITLEBIGFILE_TRACKFILEID, CODESEGMENT4000 );
	PopBank();	
	
	// Load Tile (8-15 pixel index range)
	LoadFile( MANGATILE2BMPSPRRAWDATA1PCK_TRACKFILEID, VIDEOSEGMENT0000 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	
	// Load Cloud
	LoadFile( MANGATITLECLOUDBMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
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
	BankUnpack( 0xC0, VIDEOSEGMENT8000, MANGATITLEBMPTOPBINPCK_PTR );	
	BankUnpack( 0xC0, VIDEOSEGMENTC000, MANGATITLEBMPBOTTOMBINPCK_PTR );	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	// Prepare for part
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, CODESEGMENT0000_SIZE );
	
	DrawCloudReverse_VIDEOSEGMENT0000();
	
	FlipScreen();
	
	LoadFile( MANGADATA2BIGFILEPCK_TRACKFILEID, CODESEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( CODESEGMENT4000, CODESEGMENT0000 );
	PopBank();
	
	LoadFile( MANGABACKGROUNDBMPTOPBINPCK_TRACKFILEID, VIDEOSEGMENT4000 );
	BankUnpack( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT4000 );
	
	LoadFile( MANGABACKGROUNDBMPBOTTOMBINPCK_TRACKFILEID, CODESEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENT4000, CODESEGMENT0000 );
	PopBank();
	
	bank_memcpy( 0xC6, CODESEGMENT0000, CODESEGMENT4000, CODESEGMENT0000_SIZE );
}