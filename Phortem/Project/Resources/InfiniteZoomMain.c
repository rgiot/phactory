
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"
#include "BankUnpack.cxx"
#include "BankMemCopy.cxx"

#include "InfiniteZoom.h"
#include "InfiniteZoomData1.h"
#include "InfiniteZoomData2.h"
#include "InfiniteZoomData3.h"

#include "DrawEdges.cxx"

// ----------------------------------------------------------------------------
unsigned char frame;

// ----------------------------------------------------------------------------
void Init();

// ----------------------------------------------------------------------------
void FastFlip()
{
	FlipScreen();
	
	WaitVBL();
__asm
	ld hl, &1000
	ld de, &1000
	ld bc, &100
	ldir
__endasm;

	FlipScreen();
}

// ----------------------------------------------------------------------------
unsigned char w;
void WaitFlip(unsigned char t)
{
	for( w = 0; w < t; w++ )
	{
		WaitVBL();
__asm
	ld hl, &1000
	ld de, &1000
	ld bc, &100
	ldir
__endasm;
	}
}

// ----------------------------------------------------------------------------
void DoFlipFX()
{
	FastFlip();
	WaitFlip(5);
	FastFlip();
	WaitFlip(4);
	FastFlip();
	WaitFlip(4);
	FastFlip();
	WaitFlip(3);
	FastFlip();
	WaitFlip(3);
	FastFlip();
	WaitFlip(2);
	FastFlip();
	WaitFlip(2);
	FastFlip();
	WaitFlip(2);
	FastFlip();
	WaitFlip(1);
	FastFlip();
	WaitFlip(1);
	FastFlip();
	WaitFlip(1);
	FastFlip();
}

// ----------------------------------------------------------------------------
void FrameVideoSegment0000()
{
__asm
	call &BA00
__endasm;

__asm
	ld hl, BANK0LINES_TOPPTR
	call &BA03
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
__endasm;

		DrawBGEdges( 0xFA00, VIDEOSEGMENT0000, VIDEOSEGMENT4000 );
__asm
	di
	pop af
	ld (&0000), a
	ld b, &7F
	ld c, a
	out (c), c
	ei
__endasm;
}

// ----------------------------------------------------------------------------
void FrameVideoSegment8000()
{
__asm
	ld hl, BANK2LINES2_TOPPTR
	call &BA03
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
__endasm;

		DrawBGEdges( 0xFA00, VIDEOSEGMENT8000, VIDEOSEGMENTC000 );
__asm
	di
	pop af
	ld (&0000), a
	ld b, &7F
	ld c, a
	out (c), c
	ei
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	BankUnpack( 0xC0, 0xBA00, INFINITEZOOMFXCODE_B300Z80BINPCK_PTR );
	
	bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	
	FrameVideoSegment0000();
	
	DoFlipFX();
	FlipScreen();
	WaitFlip(4);
	
	for ( frame = 0; frame < 48; frame++ )
	{	
		FrameVideoSegment8000();

		FlipScreen();	
		
		FrameVideoSegment0000();

		FlipScreen();	
	}
	
	PushBank( BANKC3 );
	Unpack( VIDEOSEGMENT8000, VIDEOSEGMENTC000 );
	LoadFile( INFINITEZOOMBACKGROUNDBMPBOTTOMBINPCK_TRACKFILEID_2, VIDEOSEGMENTC000 );
	Unpack( VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	DoFlipFX();
	FlipScreen();
}

// ----------------------------------------------------------------------------
void Init()
{	
	LoadFile( INFINITEZOOMDATA3BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC0, 0x2800, Z01_2BMPSPRRAWDATA1PCK_PTR );
	BankUnpack( 0xC6, 0x5978, INFINITEZOOMDATA06BINPCK_PTR );
	BankUnpack( 0xC6, 0x6634, INFINITEZOOMDATA07BINPCK_PTR );
}