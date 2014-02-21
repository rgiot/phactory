
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "memcpy.cxx"
#include "memset.cxx"
#include "BankMemCopy.cxx"
#include "BankMemSwap.cxx"
#include "BankUnpack.cxx"
 
#include "DrawCloud.cxx"
#include "DrawTilesWithMaskPenIndex.cxx"

#include "AngelFireWorks.h"

#include "AngelMain1.h"

// ----------------------------------------------------------------------------
unsigned short frame;
char **BackLines;
unsigned char StopPart;

// ----------------------------------------------------------------------------
void IsMusicStopped()
{	
__asm
	ld a, (&0004)
	or a
	jr z, skipEndPart
	
	ld ( _StopPart ), a	
	
skipEndPart:
__endasm;
}

// ----------------------------------------------------------------------------
unsigned char *musicFrameCounter;

// ----------------------------------------------------------------------------
void DoFireWorks();
void Init();
void EndPart();

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	for ( frame = 0; frame < 154+96; frame++ )
	{
__asm
	xor a
	ld ( &0003 ), a
	
	call &B900
	
	ld a, ( &0003 )
	ld b, a
	ld a, 4 ; 5
	sub b
	
	cp 10
	jr nc, SkipWaitVBL
	
DoWaitVBL:
	or a
	jr z, SkipWaitVBL
	push af
	call _WaitVBL
	ld hl, &100
	ld de, &100
	ld bc, &100
	ldir
	pop af
	dec a
	jr DoWaitVBL
SkipWaitVBL:
	;call _WaitVBL
__endasm;
	}
	
	EndPart();
	
	DoFireWorks();
	
	while ( StopPart == 0 )
	{	
		IsMusicStopped();
	}
}

// ----------------------------------------------------------------------------
void DoFireWorks()
{	
	LoadFile( ANGELFIREWORKSBIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC0, 0xB800, FIREWORKSFXCODEBINPCK_PTR );
	BankUnpack( 0xC0, 0x6A00, FIREWORKSFXDATABINPCK_PTR );
	BankUnpack( 0xC0, 0x2A00, FIREWORKSFXDATA2BINPCK_PTR );
	bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );	
		
	PushBank( BANKC1 );
	
__asm
ScreenTablePtrC1 equ &2800
ScreenTablePtrC3 equ &6800

	call &B800
	
	ld ix, 0
		
FireWorksFrameLoop:
		ld a, ScreenTablePtrC3/256
		call &B803		
		or a
		jr nz, QuitFireWorks
		
		ld hl, &1000
		ld de, &1000
		ld bc, &100
		ldir
		
		call _FlipScreen
		call _WaitVBL
		
		ld a, ScreenTablePtrC1/256
		call &B803
		or a
		jr nz, QuitFireWorks
		
		ld hl, &1000
		ld de, &1000
		ld bc, &100
		ldir
		
		call _FlipScreen
		call _WaitVBL
		
		jr FireWorksFrameLoop
QuitFireWorks:
__endasm;

	PopBank();
}

// ----------------------------------------------------------------------------
void EndPart()
{
	LoadFile( ANGELMAIN1BIGFILE_TRACKFILEID, ANGELMAIN1H_DATAPTR );
	BankUnpack( 0xC1, CODESEGMENT0000, DOOMTRANSIT2FXCODEZ80BINPCK_PTR );
	BankUnpack( 0xC1, VIDEOSEGMENT0000, ANGELTITLEENDPARTBMPTOPBINPCK_PTR );
	BankUnpack( 0xC1, VIDEOSEGMENT4000, ANGELTITLEENDPARTBMPBOTTOMBINPCK_PTR );

	// Load Doom Transit2 
	/*LoadFile( DOOMTRANSIT2FXCODEZ80BINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC0, CODESEGMENT0000, VIDEOSEGMENT0000 );
	
	// Load title
	PushBank( BANKC1 );
	LoadFile( ANGELTITLEENDPARTBMPTOPBINPCK_TRACKFILEID, CODESEGMENT4000 );
	BankUnpack( 0xC0, VIDEOSEGMENT0000, CODESEGMENT4000 );
	LoadFile( ANGELTITLEENDPARTBMPBOTTOMBINPCK_TRACKFILEID, CODESEGMENT4000 );	
	BankUnpack( 0xC0, VIDEOSEGMENT4000, CODESEGMENT4000 );
	PopBank();*/
	
	bank_memcpy( 0xC0, CODESEGMENT4000, CODESEGMENTC000, 0x1000 );	
	PushBank( BANKC1 );
	DrawTilesWithMaskPenIndex(MASKTYPE_MASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, CODESEGMENT4000, 9 );
	PopBank();	
		
	// Play Transit 2
__asm
	call &2800
	
	ld a, 72
PlayTransit2:
	push af
	call &2803
	pop af
	dec a
	jr nz, PlayTransit2	
__endasm;
}

// ----------------------------------------------------------------------------
void Init()
{	
	PushBank( BANKC1 );
	LoadFile( ANGELTILEBMPSPRRAWDATA1PCK_TRACKFILEID_2, 0xB900 );
	PopBank();	
	BankUnpack( BANKC0, CODESEGMENTC000, 0xB900 );	
	
	BankUnpack( BANKC0, 0x0100, CODESEGMENT4000 );
	BankUnpack( BANKC0, 0x0600, CODESEGMENT4000 + 0x200 );
	
	LoadFile( ANGELDRAWFXMACROS_BA00Z80BIN_TRACKFILEID, 0xBA00 );
	LoadFile( ANGELDRAWFX_B900Z80BIN_TRACKFILEID, 0xB900 );
}
