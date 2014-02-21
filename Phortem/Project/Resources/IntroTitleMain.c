
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

#include "IntroAnim1.h"
#include "IntroAnim2.h"
#include "IntroAnim3.h"
#include "IntroAnim4.h"
#include "IntroAnim5.h"
#include "IntroAnim6.h"
#include "IntroAnim7.h"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "Intro.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void ClearScreen()
{	
__asm
	ld hl, VIDEOSEGMENT8000
	ld ( hl ), &0C ; PEN 2 LEFT-RIGHT
	ld de, VIDEOSEGMENT8000+1
	ld bc, VIDEOSEGMENT8000_SIZE-1
	ldir
__endasm;
	PushBank( BANKC3 );
__asm
	ld hl, VIDEOSEGMENT4000
	ld ( hl ), &0C ; PEN 2 LEFT-RIGHT
	ld de, VIDEOSEGMENT4000+1
	ld bc, VIDEOSEGMENT4000_SIZE-1
	ldir
__endasm;
	PopBank();
}

// ----------------------------------------------------------------------------
void LoadAnimEye(int fileId)
{	
	unsigned char w;
	
	LoadFile( fileId, CODESEGMENT0000 );
	BankUnpack( 0xC0, VIDEOSEGMENT0000, CODESEGMENT0000 );
	
	PushBank( BANKC3 );
	
	WaitVBL();
	
__asm
	
EdgeSize equ 16
	
	ld a, 5
	ld hl, EdgeSize + VIDEOSEGMENT0000 + 9*96
	ld de, EdgeSize + VIDEOSEGMENT4000 + 9*96
DrawEyesOnly:
	push af
	push hl
	push de
	
	ld a, 10
DrawEyesOnly_InnerLoop:

ReptSize equ 96-EdgeSize-EdgeSize
MiddleSize equ 20
ReptSizeMinusMiddle equ ReptSize/2-MiddleSize/2

REPT ReptSizeMinusMiddle
	ldi
ENDM

	ld bc, MiddleSize
	add hl, bc
	ex de, hl
	add hl, bc
	ex de, hl

REPT ReptSizeMinusMiddle
	ldi
ENDM

	ld bc, EdgeSize+EdgeSize
	add hl, bc
	ex de, hl
	add hl, bc
	ex de, hl

	dec a
	jp nz, DrawEyesOnly_InnerLoop
	
	pop hl
	ld bc, &800
	add hl, bc
	ex de, hl
	pop hl
	add hl, bc
	pop af
	dec a
	jp nz, DrawEyesOnly
__endasm;
	PopBank();
	
	for( w = 0; w < 50; w++ )
	{
		WaitVBL();
	}
}

// ----------------------------------------------------------------------------
unsigned char AsicEnabled;
char *asicPalette;
	
// ----------------------------------------------------------------------------
void SetPreEyesPalette( unsigned char i )
{
	unsigned char w;
	
	WaitVBL();
	
__asm
	ld a, ( &0037 )
	ld ( _AsicEnabled ), a
__endasm;
	
	if ( AsicEnabled == 0 )
	{		
		if ( i == 0 )
		{
			SetColor( 6, COLOR3 );
		}
		else if ( i == 1 )
		{
			SetColor( 9, COLOR3 );
			SetColor( 6, COLOR6 );
		}
		else if ( i == 2 )
		{
			SetColor( 4, COLOR3 );
			SetColor( 9, COLOR6 );
			SetColor( 6, COLOR6 );
		}
		else // if ( i == 3 )
		{
			SetColor( 3, COLOR3 );
			SetColor( 1, COLOR6 );
			SetColor( 0, COLOR7 );
			SetColor( 7, COLOR8 );
			SetColor( 4, COLOR8 );
			SetColor( 9, COLOR17 );
			SetColor( 6, COLOR25 );
		}
	}
	else
	{
		SetBlackPalette();		
		asicPalette = GetPalette();
		
		if ( i == 0 )
		{
			SetAsicColor( asicPalette, 6, 3 );
		}
		else if ( i == 1 )
		{
			SetAsicColor( asicPalette, 9, 3 );
			SetAsicColor( asicPalette, 6, 1 );
		}
		else if ( i == 2 )
		{
			SetAsicColor( asicPalette, 4, 3 );
			SetAsicColor( asicPalette, 9, 1 );
			SetAsicColor( asicPalette, 6, 1 );
		}
		else // if ( i == 3 )
		{
			SetAsicColor( asicPalette, 3, 3 );
			SetAsicColor( asicPalette, 1, 1 );
			SetAsicColor( asicPalette, 0, 0 );
			SetAsicColor( asicPalette, 7, 7 );
			SetAsicColor( asicPalette, 4, 7 );
			SetAsicColor( asicPalette, 9, 4 );
			SetAsicColor( asicPalette, 6, 9 );
		}
	}
	
	for( w = 0; w < 220; w++ )
	{
		WaitVBL();
	}
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
	FastFlip();
	WaitFlip(100);
	FastFlip();
	WaitFlip(80);
	FastFlip();
	WaitFlip(60);
	FastFlip();
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
	WaitFlip(3);
	FastFlip();
	WaitFlip(3);
	FastFlip();
	WaitFlip(3);
	FastFlip();
	WaitFlip(3);
	
	FlipScreen();
	
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENTC000_SIZE );
	
	FlipScreen();
}

// ----------------------------------------------------------------------------
void Main()
{
	unsigned char i;
	
	ClearScreen();	
	SetBlackPalette();
	
	LoadFile( INTROANIM1BIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, 0x4000, VIDEOSEGMENT0000 );
	
	LoadAnimEye( INTROANIMEYES1BMPBOTTOMBINPCK_TRACKFILEID );	
	
	for (i = 0; i < 4; i++ )
	{
		SetPreEyesPalette(i);
	}
	SetPalette( GetPalette() );
	
	LoadFile( INTROANIM3BIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC6, 0x4000, VIDEOSEGMENT0000 );
	
	LoadAnimEye( INTROANIMEYES2BMPBOTTOMBINPCK_TRACKFILEID );
	
	// &C0-&6800
	LoadFile( INTROANIM4BIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC0, INTROANIM4H_DATAPTR, VIDEOSEGMENT0000 );
	
	LoadAnimEye( INTROANIMEYES3BMPBOTTOMBINPCK_TRACKFILEID );
	
	// &C0-&E800
	LoadFile( INTROANIM5BIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC0, INTROANIM5H_DATAPTR, VIDEOSEGMENT0000 );
	
	// &F100
	LoadFile( INTROANIM6BIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC1, INTROANIM6H_DATAPTR, VIDEOSEGMENT0000 );
	
	LoadAnimEye( INTROANIMEYES4BMPBOTTOMBINPCK_TRACKFILEID );

	LoadFile( INTROBACKGROUNDBMPBOTTOMBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC1, VIDEOSEGMENT4000, VIDEOSEGMENT0000 );
	
	LoadAnimEye( INTROANIMEYES5BMPBOTTOMBINPCK_TRACKFILEID );

	LoadFile( INTROBACKGROUNDBMPTOPBINPCK_TRACKFILEID, VIDEOSEGMENTC000 );
	
	LoadAnimEye( INTROANIMEYES6BMPBOTTOMBINPCK_TRACKFILEID );
	
	BankUnpack( 0xC1, VIDEOSEGMENT0000, VIDEOSEGMENTC000 );
	
	DoFlipFX();
	
	LoadFile( INTROANIM2BIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC1, VIDEOSEGMENTC000, VIDEOSEGMENT0000 );
}