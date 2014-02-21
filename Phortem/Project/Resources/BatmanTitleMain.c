
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

#include "BatmanTitle1.h"
#include "BatmanTitle2.h"
#include "BatmanTitle3.h"
#include "BatmanTitle4.h"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, AngelPalette
	jr QuitGetPalette
AngelPalette:
	incbin "Batman.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void InitFirst();

// ----------------------------------------------------------------------------
void Main()
{
	InitFirst();
	
	LoadFile( BATMANTITLE1BIGFILE_TRACKFILEID, BATMANTITLE1H_DATAPTR );
	BankUnpack( 0xC0, 0xF8E0, TRANSITHORIZLINESFXCODE_F8E0Z80BINPCK_PTR );
	BankUnpack( 0xC6, 0x6800, BATMANCIRCLECLIPRIGHTBINPCK_PTR );
	BankUnpack( 0xC4, 0x6800, BATMANCIRCLECLIPLEFTBINPCK_PTR );
	BankUnpack( 0xC0, 0xBB00, BATMANFXCODEZ80BINPCK_PTR );	
	BankUnpack( 0xC0, 0xFB00, BATMANCIRCLEMOVEXYBINPCK_PTR );
	
	LoadFile( BATMANTITLE2BIGFILE_TRACKFILEID, BATMANTITLE2H_DATAPTR );	
	BankUnpack( 0xC4, 0x4080, BATMAN1BMPTOPBINPCK_PTR );	
	BankUnpack( 0xC6, 0x4000, BATMAN1BMPBOTTOMBINPCK_PTR );
	
	LoadFile( BATMANTITLE3BIGFILE_TRACKFILEID, BATMANTITLE3H_DATAPTR );
	BankUnpack( 0xC7, 0x4080, BATMAN2BMPTOPBINPCK_PTR );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, BATMAN2BMPBOTTOMBINPCK_PTR );
}

// ----------------------------------------------------------------------------
void FastFlip();
void DoFlipFX();
void WaitFlip(unsigned char t);

// ----------------------------------------------------------------------------
void InitFirst()
{	
	bank_memcpy( 0xC3, VIDEOSEGMENTC000+0x1000, CODESEGMENT4000, 0x1000 );
	
	LoadFile( BATMANTITLE4BIGFILE_TRACKFILEID, BATMANTITLE4H_DATAPTR );
	BankUnpack( 0xC1, VIDEOSEGMENTC000, BATMANTILEBMPSPRRAWDATA1PCK_PTR );	
	bank_memcpy( 0xC1, CODESEGMENT4000, BATMANTITLEBMPTOPBINPCK_PTR, BATMANTITLEBMPTOPBINPCK_SIZE );	
	bank_memcpy( 0xC3, CODESEGMENT4000, BATMANTITLEBMPBOTTOMBINPCK_PTR, BATMANTITLEBMPBOTTOMBINPCK_SIZE );	
	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000+0x1000 );
	memcpy( CODESEGMENT0000, VIDEOSEGMENTC000, 0x1000 );
	bank_memcpy( 0xC4, 0x7800, CODESEGMENT0000, 0x800 );
	bank_memcpy( 0xC6, 0x7800, CODESEGMENT0000+0x800, 0x800 );	
	ShiftTile( VIDEOSEGMENTC000 );
	PopBank();
	
	FlipScreen();
	DoFlipFX();
	FlipScreen();
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	FlipScreen();
	
	SetPaletteFrom8( GetPalette() );
	
	// Load Cloud
	LoadFile( BATMANTITLECLOUDBMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
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
	
	BankUnpack( 0xC0, VIDEOSEGMENT8000, CODESEGMENT4000 );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, CODESEGMENTC000 );
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
		
	DrawCloudReverse_VIDEOSEGMENT0000();
	
	__asm
	ld a, 5
waitloop:
	ld hl, &1000
	ld de, &1000
	ld bc, &8000
	ldir
	dec a
	jr nz, waitloop
__endasm;
}

// ----------------------------------------------------------------------------
void FastFlip()
{
	unsigned char w;
	
	FlipScreen();
	for( w = 0; w < 50; w++ )
	{
		WaitVBL();
	}
	FlipScreen();
}

// ----------------------------------------------------------------------------
void WaitFlip(unsigned char t)
{
	unsigned char w;
	
	for( w = 0; w < t+t; w++ )
	{
		WaitVBL();
	}
}

// ----------------------------------------------------------------------------
void DoFlipFX()
{
	FlipScreen();
	WaitFlip(40);
	FastFlip();
	WaitFlip(20);
	FastFlip();
	WaitFlip(10);
	FastFlip();
	WaitFlip(5);
	FastFlip();
	WaitFlip(3);
	FastFlip();	
	WaitFlip(3);
	FastFlip();
}