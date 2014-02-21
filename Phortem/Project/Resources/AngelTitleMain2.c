
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "memcpy.cxx"
#include "memset.cxx"
#include "BankMemCopy.cxx"
#include "BankUnpack.cxx"

#include "AngelTitle2.h"

// ----------------------------------------------------------------------------
#include "InitLines.c"

// ----------------------------------------------------------------------------
unsigned short frame;
unsigned char isScreenFlipped;

// ----------------------------------------------------------------------------
void InitBounceTransit()
{
__asm
		di
		ld a, ( &0000 )
		push af
		ld b, &7F
		ld c, &C0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		call &BB00

		ld hl, &0080
		ld de, &E800
		ld a, 9
		call LinearScreenCopy
		ld de, &6800
		ld a, 12
		call LinearScreenCopy
		
		di
		pop af
		ld b, &7F
		ld c, a
		out (c), c
		ld ( &0000 ), a
		ei
		
		jp InitBounceTransitEnd
		
LinearScreenCopy:
LinearScreenCopy_BlockLoop:
		push af
		
		push hl
		
		ld a, 5
LinearScreenCopy_LineLoop:
		push af
		
		ld bc, 96
		ldir
		
		ld bc, &800-96
		add hl, bc
		
		pop af
		dec a
		jr nz, LinearScreenCopy_LineLoop	
		
		pop hl
		ld bc, 96
		add hl, bc
		
		pop af
		dec a
		jr nz, LinearScreenCopy_BlockLoop
		ret
		
InitBounceTransitEnd:
__endasm;
}

// ----------------------------------------------------------------------------
void DrawFrameFX()
{
	isScreenFlipped = IsScreenFlipped();

__asm
		ld a, (_isScreenFlipped)
		call &BB03
__endasm;

	FlipScreen();
}

// ----------------------------------------------------------------------------
void DrawBounceTransit()
{
	for ( frame = 0; frame < 213; frame++ )
	{
__asm
	call _DrawFrameFX
__endasm;
	}
}

// ----------------------------------------------------------------------------
void Main()
{
	FlipScreen();
	
	InitLines();
	
	LoadFile( BOUNCINGFXCODE_BB00Z80BINPCK_TRACKFILEID, CODESEGMENT0000 );
	Unpack( 0xBB00, CODESEGMENT0000 );
	
	bank_memcpy( BANKC0, VIDEOSEGMENT0000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );
	InitBounceTransit();
	
	LoadFile( ANGELTITLE2BIGFILE_TRACKFILEID, ANGELTITLE2H_DATAPTR );
	BankUnpack( BANKC4, 0x4080, ANGELASDEMONBMPTOPBINPCK_PTR );
	BankUnpack( BANKC1, 0xC000, ANGELASDEMONBMPBOTTOMBINPCK_PTR );
	
	bank_memcpy( BANKC6, 0x4000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	
	BankUnpack( BANKC4, 0x6800, BOUNCINGDELTAY_1BINPCK_PTR );
	BankUnpack( BANKC6, 0x6800, BOUNCINGDELTAY_2BINPCK_PTR );
	
	bank_memcpy( BANKC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	bank_memcpy( BANKC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE );
	
	DrawBounceTransit();
	
	FlipScreen();
	
	PushBank (BANKC1);
	LoadFile( ANGELSCROLLOFFSETBINPCK_TRACKFILEID, CODESEGMENT4000 );
	LoadFile( ANGELSCROLLIMAGEBINPCK_TRACKFILEID, CODESEGMENT4000 + 0x200 );
	PopBank();
	
	bank_memcpy( BANKC4, VIDEOSEGMENT0000, 0x4080, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( BANKC6, 0x4080, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	
	LoadFile( ANGELBMPTOPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( BANKC4, 0x4080, VIDEOSEGMENT0000 );
	
	LoadFile( ANGELBMPBOTTOMBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );	
	PushBank (BANKC1);
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENT0000 );
	PopBank();
	
	bank_memcpy( BANKC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
}