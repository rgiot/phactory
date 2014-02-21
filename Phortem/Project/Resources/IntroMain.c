
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "memcpy.cxx"
#include "memset.cxx"
#include "BankMemCopy.cxx"
#include "BankUnpack.cxx" 

#include "IntroAnim1.h"
#include "IntroAnim2.h"
#include "IntroAnim3.h"
#include "IntroAnim4.h"
#include "IntroAnim5.h"
#include "IntroAnim6.h"
#include "IntroAnim7.h"

#include "DrawCloud.cxx"
#include "DrawTiles.cxx"
#include "DrawEdges.cxx"

#include "IntroText1.h"
#include "IntroText2.h"
#include "IntroText3.h"

// ----------------------------------------------------------------------------
unsigned short FrameCount;
unsigned char AnimCount;
char **BackLines;

unsigned char* BankScrPtr;
unsigned char BankAnim;
unsigned char DoRotate;

// ----------------------------------------------------------------------------
char *GetIntroTextPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "IntroText.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void Init()
{	
	// CODESEGMENT0000 is overriden by LoadPart
	LoadFile( INTROANIM7BIGFILEPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC0, INTROANIM7H_DATAPTR, VIDEOSEGMENT0000 );
	
	bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );

	AnimCount = 9;
}

// ----------------------------------------------------------------------------
void UnpackAnimFrame()
{
	switch(AnimCount)
	{
	default:
	case 0:
		BankAnim = 0xC4;
		BankScrPtr = INTROROTATELOGO01BMPSPRSCRDATA1_PTR;
		break;
	case 1:
		BankAnim = 0xC4;
		BankScrPtr = INTROROTATELOGO02BMPSPRSCRDATA1_PTR;
		break;
	case 2:
		BankAnim = 0xC4;
		BankScrPtr = INTROROTATELOGO03BMPSPRSCRDATA1_PTR;
		break;
	case 3:
		BankAnim = 0xC0;
		BankScrPtr = INTROROTATELOGO04BMPSPRSCRDATA1_PTR;
		break;
	case 4:
		BankAnim = 0xC0;
		BankScrPtr = INTROROTATELOGO05BMPSPRSCRDATA1_PTR;
		break;
	case 5:
		BankAnim = 0xC7;
		BankScrPtr = INTROROTATELOGO06BMPSPRSCRDATA1_PTR;
		break;
	case 6:
		BankAnim = 0xC6;
		BankScrPtr = INTROROTATELOGO07BMPSPRSCRDATA1_PTR;
		break;
	case 7:
		BankAnim = 0xC1;
		BankScrPtr = INTROROTATELOGO08BMPSPRSCRDATA1_PTR;
		break;
	case 8:
		BankAnim = 0xC6;
		BankScrPtr = INTROROTATELOGO09BMPSPRSCRDATA1_PTR;
		break;
	case 9:
		BankAnim = 0xC0;
		BankScrPtr = INTROROTATELOGO10BMPSPRSCRDATA1_PTR;
		break;
	case 10:
		BankAnim = 0xC6;
		BankScrPtr = INTROROTATELOGO11BMPSPRSCRDATA1_PTR;
		break;
	case 11:
		BankAnim = 0xC4;
		BankScrPtr = INTROROTATELOGO12BMPSPRSCRDATA1_PTR;
		break;
	case 12:
		BankAnim = 0xC6;
		BankScrPtr = INTROROTATELOGO13BMPSPRSCRDATA1_PTR;
		break;
	case 13:
		BankAnim = 0xC7;
		BankScrPtr = INTROROTATELOGO14BMPSPRSCRDATA1_PTR;
		break;
	case 14:
		BankAnim = 0xC7;
		BankScrPtr = INTROROTATELOGO15BMPSPRSCRDATA1_PTR;
		break;
	case 15:
		BankAnim = 0xC6;
		BankScrPtr = INTROROTATELOGO16BMPSPRSCRDATA1_PTR;
		break;
	case 16:
		BankAnim = 0xC6;
		BankScrPtr = INTROROTATELOGO17BMPSPRSCRDATA1_PTR;
		break;
	case 17:
		BankAnim = 0xC0;
		BankScrPtr = INTROROTATELOGO18BMPSPRSCRDATA1_PTR;
		break;
	}
}

// ----------------------------------------------------------------------------
void DrawAnimFrame0000()
{
__asm
	di
	ld a, ( &0000 )
	push af
	ld a, (_BankAnim)
	ld c, a
	ld b, &7F
	out (c), c
	ld ( &0000 ), a
	ei

	ld hl, (_BankScrPtr)
	ld b, 0
	
animFrameLoop0000:
	ld a, (hl)
	cp &FF
	jp z, animFrameLoopEnd0000
	cp &00
	jp nz, skipH0000
	inc hl
	ld a, (hl)
	inc hl
	ld d, a
	ld c, (hl)
	jp skipH0000_End
skipH0000:
	ld c, a
skipH0000_End:
	inc hl
	ld e, (hl)
	inc hl
	
	xor a
	sub c
	and 32-1
	add a, a
	ld ( animFrameInnerLoop0000_initPadding + 1 ), a
animFrameInnerLoop0000_initPadding:
	jr nz, animFrameInnerLoop0000_initPadding
	
animFrameInnerLoop0000_loop:
REPT 32
	ldi
ENDM
	jp pe, animFrameInnerLoop0000_loop	
	jp animFrameLoop0000
	
animFrameLoopEnd0000:
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei	
__endasm;
}

// ----------------------------------------------------------------------------
void DrawAnimFrame8000()
{
__asm
	di
	ld a, ( &0000 )
	push af
	ld a, (_BankAnim)
	ld c, a
	ld b, &7F
	out (c), c
	ld ( &0000 ), a
	ei
	
	ld hl, (_BankScrPtr)
	ld b, 0
	
animFrameLoop8000:
	ld a, (hl)
	cp &FF
	jp z, animFrameLoopEnd8000
	cp &00
	jp nz, skipH8000
	inc hl
	ld a, (hl)
	inc hl
	ld d, a
	set 7, d
	ld c, (hl)
	jp skipH8000_End
skipH8000:
	ld c, a
skipH8000_End:
	inc hl
	ld e, (hl)
	inc hl
	
	xor a
	sub c
	and 32-1
	add a, a
	ld ( animFrameInnerLoop8000_initPadding + 1 ), a
animFrameInnerLoop8000_initPadding:
	jr nz, animFrameInnerLoop8000_initPadding
	
animFrameInnerLoop8000_loop:
REPT 32
	ldi
ENDM
	jp pe, animFrameInnerLoop8000_loop	
	jp animFrameLoop8000
	
animFrameLoopEnd8000:
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei	
	
__endasm;
}

// ----------------------------------------------------------------------------
void DrawFrame()
{
	BackLines = GetBackLines();
	
	PushBankBottom();
	
	UnpackAnimFrame();
	
	if ( IsScreenFlipped() )
	{
		DrawAnimFrame8000();
	}
	else
	{
		DrawAnimFrame0000();
	}
	
	PopBank();
}

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	while ( FrameCount < 153-20 ) //273-10 )
	{
		DrawFrame();
		
		DoRotate = 0;
		if ( (FrameCount>0)&&(FrameCount<(36+10+1)))
		{
			DoRotate = 1;
		}
		else if ( (FrameCount>80-20)&&(FrameCount<(80-20+36+10+26+1)))
		{
			DoRotate = 1;
		}
		else if ( (FrameCount>(160-20+26+1))&&(FrameCount<(160-20+36+10+26+1)))
		{
			DoRotate = 1;
		}
		
		if ( DoRotate == 1 )
		{
			AnimCount++;
			if ( AnimCount == 18 )
			{
				AnimCount = 0;
			}
		}
		
		if ( DoRotate == 1 )
		{
			FlipScreen();
			//WaitVBL();
		}
		
		if ( FrameCount==47 )
		{
			//bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
			LoadFile( INTROLOGOBIN_TRACKFILEID, VIDEOSEGMENT4000 );
__asm
			call VIDEOSEGMENT4000
__endasm;	
			//bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENT0000_SIZE );
			bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );
		}
	
		FrameCount++;
	}
	
	PushBank( BANKC1 );
	LoadFile( INTROTEXT1BIGFILE_TRACKFILEID, CODESEGMENT4000 );
	PopBank();
	
	// Load Cloud
	LoadFile( INTROTITLECLOUD2BMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, CODESEGMENT0000, VIDEOSEGMENT0000 );
	
	LoadFile( INTROTILEBMPSPRRAWDATA1PCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( BANKC1, VIDEOSEGMENTC000, VIDEOSEGMENT0000 );		
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	DrawCloudReverse_VIDEOSEGMENT8000();
	
	SetPalette( GetIntroTextPalette() );
	
	bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );
	FlipScreen();
	
	BankUnpack( 0xC0, VIDEOSEGMENT8000, INTROTEXT1BMPTOPBINPCK_PTR );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, INTROTEXT1BMPBOTTOMBINPCK_PTR );	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	DrawCloud_VIDEOSEGMENT0000();
	for ( FrameCount = 0; FrameCount < 40; FrameCount++ )
	{
		bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	}	
	
	PushBank( BANKC1 );
	LoadFile( INTROTEXT2BIGFILE_TRACKFILEID, CODESEGMENT4000 );
	PopBank();
	BankUnpack( 0xC0, VIDEOSEGMENT8000, INTROTEXT2BMPTOPBINPCK_PTR );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, INTROTEXT2BMPBOTTOMBINPCK_PTR );	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	DrawCloudReverse_VIDEOSEGMENT0000();
	for ( FrameCount = 0; FrameCount < 40; FrameCount++ )
	{
		bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	}	
	
	LoadFile( INTROTEXT3BIGFILE_TRACKFILEID, VIDEOSEGMENT8000 );
	BankUnpack( 0xC6, VIDEOSEGMENT4000, INTROTEXT3BMPTOPBINPCK_PTR );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, INTROTEXT3BMPBOTTOMBINPCK_PTR );	
	bank_memcpy( 0xC6, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENT8000_SIZE );
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	DrawCloud_VIDEOSEGMENT0000();
	for ( FrameCount = 0; FrameCount < 100; FrameCount++ )
	{
		bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	}	
		
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	DrawCloudReverse_VIDEOSEGMENT0000();
	
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	FlipScreen();
}