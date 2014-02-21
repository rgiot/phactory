
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "BankMemCopy.cxx"
#include "BankUnpack.cxx"
 
#include "CharonBackground1.h"
#include "CharonBackground2.h"
#include "CharonBackground3.h"
#include "CharonBackground4.h"

#include "CharonTitleMain1.h"

// ----------------------------------------------------------------------------
unsigned char frame;
unsigned char StopPart;
unsigned char TitleIndex;

// ----------------------------------------------------------------------------
void ShowHoriz2FX()
{
__asm
	ld hl, TransitVertLinesFXCode
	ld de, &BE80
	ld bc, TransitVertLinesFXCodeEnd-TransitVertLinesFXCode
	ldir
	
	ld hl, &2801
	ld ( TimeLength2 + 1 ), hl

	xor a ; 0 = IsClear OFF
	call &BE80
	
	ld a, 20
LoopHoriz2FX:
	push af
	
	call &BE83
	
	ld hl, &1000
	ld de, &1000
TimeLength2:
	ld bc, &2801
	ldir
	
	ld hl, (TimeLength2+1)
	ld bc, -&200
	add hl, bc
	ld (TimeLength2+1), hl

	pop af
	dec a
	jr nz, LoopHoriz2FX
	
	jp Transit1stFrameEnd
	
TransitVertLinesFXCode:
	incbin "TransitCharonFXCode.z80.bin"
TransitVertLinesFXCodeEnd:
	
Transit1stFrameEnd:
__endasm;
}
	
// ----------------------------------------------------------------------------
void IsMusicStopped()
{	
__asm
	ld a, (&0004)
	or a
	jr z, skipEndPart
	
	ld a, 2
	ld (&0004), a
	ld ( _StopPart ), a	
	
skipEndPart:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, CharonPalette
	jr QuitGetPalette
CharonPalette:
	incbin "Charon.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetFade1Palette()
{
__asm
	ld hl, CharonPalette1
	jr QuitGetPalette1
CharonPalette1:
	incbin "CharonBackground1Fade1.bmp.fadePalette"
QuitGetPalette1:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetFade2Palette()
{
__asm
	ld hl, CharonPalette2
	jr QuitGetPalette2
CharonPalette2:
	incbin "CharonBackground1Fade2.bmp.fadePalette"
QuitGetPalette2:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetFade3Palette()
{
__asm
	ld hl, CharonPalette3
	jr QuitGetPalette3
CharonPalette3:
	incbin "CharonBackground1Fade3.bmp.fadePalette"
QuitGetPalette3:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetFade4Palette()
{
__asm
	ld hl, CharonPalette4
	jr QuitGetPalette4
CharonPalette4:
	incbin "CharonBackground1Fade4.bmp.fadePalette"
QuitGetPalette4:
__endasm;
}

// ----------------------------------------------------------------------------
void Init();
void LoadBigFiles();

// ----------------------------------------------------------------------------
char PaletteTimeCount;
void ManagePalette()
{
	PaletteTimeCount--;
	
	switch(PaletteTimeCount)
	{
		case 0:
			WaitVBL();
			SetBlackPalette();
			PaletteTimeCount+=2;				
			break;
		case 2: 
			WaitVBL();
			SetPalette( GetFade1Palette() );
			break;
		case 4: 
			WaitVBL();
			SetPalette( GetFade2Palette() ); 
			break;
		case 6: 
			WaitVBL();
			SetPalette( GetFade3Palette() ); 
			break;
		case 9: 
			WaitVBL();
			SetPalette( GetFade4Palette() ); 
			break;
	}
}

// ----------------------------------------------------------------------------
void bg1()
{
	ManagePalette();
	bank_memcpy( 0xC4, VIDEOSEGMENT8000, 0x7800, 0x800 );
	bank_memcpy( 0xC0, VIDEOSEGMENT8000+0x800, 0xA800, 0x800 );
	bank_memcpy( 0xC3, VIDEOSEGMENT8000+0x1000, 0xF800, CHARONBACKGROUND1BMPBOTTOMBINPCK_SIZE-0x1000 );
	PushBank( BANKC3 );
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENT8000 );
	PopBank();
	bank_memcpy( 0xC4, VIDEOSEGMENT8000, 0x5000, VIDEOSEGMENT0000_SIZE );
}

// ----------------------------------------------------------------------------
void bg2()
{
	ManagePalette();
	bank_memcpy( 0xC6, VIDEOSEGMENT0000, 0x6800, CHARONBACKGROUND2BMPBOTTOMBINPCK_SIZE );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENT0000 );
	PopBank();
	bank_memcpy( 0xC6, VIDEOSEGMENT0000, 0x4000, VIDEOSEGMENT0000_SIZE );
}

// ----------------------------------------------------------------------------
void bg3()
{
	ManagePalette();
	BankUnpack( 0xC0, VIDEOSEGMENTC000, 0x6800 );
	bank_memcpy( 0xC7, VIDEOSEGMENT8000, 0x4000, VIDEOSEGMENT0000_SIZE );
}

// ----------------------------------------------------------------------------
void bg4()
{
	ManagePalette();
	BankUnpack( 0xC0, VIDEOSEGMENT4000, 0xE800 );
	bank_memcpy( 0xC4, VIDEOSEGMENT0000, 0x2800, VIDEOSEGMENT0000_SIZE );
}

// ----------------------------------------------------------------------------
void AnimBackground()
{
	switch( TitleIndex )
	{
		case 0:
			bg1();
			break;
			
		case 1:
			bg2();
			break;			
		case 20:
			TitleIndex = 1;
			break;
			
		case 2:	
			bg3();
			break;
			
		case 3:	
			bg4();
			break;
			
		case 4:	
			bg1();
			break;
			
		case 5:	
			bg4();
			break;
			
		case 6:	
			bg3();
			break;
			
		case 7:	
			bg2();
			break;
			
		case 8:
			bg1();
			break;
			
		case 9:
			bg2();
			break;		
			
		case 10:
			bg3();
			break;
			
		case 11:
			bg4();
			break;		
		
	}
	FlipScreen();
	
	TitleIndex++;
	if ( TitleIndex == 12 )
	{
		TitleIndex = 0;
	}
}

// ----------------------------------------------------------------------------
void Main()
{	
	Init();
	
	LoadBigFiles();
	
	// CASE 1
	bank_memcpy( 0xC6, VIDEOSEGMENT0000, 
						0x6800,
						CHARONBACKGROUND2BMPBOTTOMBINPCK_SIZE );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENT0000 );
	PopBank();
	bank_memcpy( 0xC6, VIDEOSEGMENT0000, 0x4000, VIDEOSEGMENT0000_SIZE );
	ShowHoriz2FX();
	
	TitleIndex = 20;
	PaletteTimeCount = 28;
	
	while ( StopPart == 0 )
	{	
		AnimBackground();
		
		IsMusicStopped();
	}
	
	AnimBackground();
	AnimBackground();
	AnimBackground();
	AnimBackground();
	AnimBackground();
	AnimBackground();
	AnimBackground();
		
	if ( IsScreenFlipped() )
	{
		FlipScreen();
	}
	
	WaitVBL();
	SetBlackPalette();
}

// ----------------------------------------------------------------------------
void LoadBigFiles()
{	
	LoadFile( CHARONBACKGROUND1BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, 0x5000, CHARONBACKGROUND1BMPTOPBINPCK_PTR );
	bank_memcpy( 0xC4, 0x7800, CHARONBACKGROUND1BMPBOTTOMBINPCK_PTR, 0x800 );
	bank_memcpy( 0xC0, 0xA800, CHARONBACKGROUND1BMPBOTTOMBINPCK_PTR+0x800, 0x800 );
	bank_memcpy( 0xC3, 0xF800, CHARONBACKGROUND1BMPBOTTOMBINPCK_PTR+0x1000, CHARONBACKGROUND1BMPBOTTOMBINPCK_SIZE-0x1000 );
	
	LoadFile( CHARONBACKGROUND2BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC6, 0x4000, CHARONBACKGROUND2BMPTOPBINPCK_PTR );
	bank_memcpy( 0xC6, 0x6800, CHARONBACKGROUND2BMPBOTTOMBINPCK_PTR, CHARONBACKGROUND2BMPBOTTOMBINPCK_SIZE );
		
	LoadFile( CHARONBACKGROUND3BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC7, 0x4000, CHARONBACKGROUND2BMPTOPBINPCK_PTR );
	bank_memcpy( 0xC0, 0x6800, CHARONBACKGROUND3BMPBOTTOMBINPCK_PTR, CHARONBACKGROUND3BMPBOTTOMBINPCK_SIZE );
	
	LoadFile( CHARONBACKGROUND4BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	bank_memcpy( 0xC0, 0xE800, CHARONBACKGROUND4BMPBOTTOMBINPCK_PTR, CHARONBACKGROUND4BMPBOTTOMBINPCK_SIZE );
	BankUnpack( 0xC4, 0x2800, CHARONBACKGROUND2BMPTOPBINPCK_PTR );
}

// ----------------------------------------------------------------------------
#include "DrawCloud.cxx"
#include "DrawTiles.cxx"
#include "ShiftTile.cxx"

// ----------------------------------------------------------------------------
void Init()
{	
	// Load Cloud
	bank_memcpy( 0xC1, CODESEGMENT0000, 0xC000, CODESEGMENT0000_SIZE );
	
	// Load Tile (8-15 pixel index range)
	LoadFile( CHARONTILE2BMPSPRRAWDATA1PCK_TRACKFILEID, VIDEOSEGMENT0000 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	
	// Draw Cloud
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	
	UnshiftTile( VIDEOSEGMENTC000 );
	bank_memcpy( 0xC1, 0x7000, VIDEOSEGMENTC000, 0x1000 );
	PushBank( BANKC1 );
	LoadFile( CHARONTITLEMAIN1BIGFILE_TRACKFILEID, CHARONTITLEMAIN1H_DATAPTR );
	PopBank();
	
	DrawCloud_VIDEOSEGMENT8000();
	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, 0x7000 );
	PopBank();
	WaitVBL();	
	SetPalette( GetPalette() );	
	FlipScreen();
	
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	FlipScreen();
	
	BankUnpack( 0xC1, VIDEOSEGMENT0000, CHARONTITLEBMPTOPBINPCK_PTR );	
	BankUnpack( 0xC1, VIDEOSEGMENT4000, CHARONTITLEBMPBOTTOMBINPCK_PTR );
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, 0x7000 );
	PopBank();
	DrawCloudReverse_VIDEOSEGMENT8000();
	
	// Prepare for part
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, CODESEGMENT0000_SIZE );	
}