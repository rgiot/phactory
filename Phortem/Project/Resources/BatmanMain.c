
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "BankUnpack.cxx"
#include "BankMemCopy.cxx"

// ----------------------------------------------------------------------------
char *GetPalette1()
{
__asm
	ld hl, Batman1Palette
	jr QuitGetPalette1
Batman1Palette:
	incbin "Batman1.cpcbitmap.palette"
QuitGetPalette1:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetPalette2()
{
__asm
	ld hl, Batman2Palette
	jr QuitGetPalette2
Batman2Palette:
	incbin "Batman2.cpcbitmap.palette"
QuitGetPalette2:
__endasm;
}

// ----------------------------------------------------------------------------
unsigned char frame;

// ----------------------------------------------------------------------------
void DoWait()
{	
__asm
	ld hl, &100
	ld de, &100
	ld bc, &200
	ldir
__endasm;
}

// ----------------------------------------------------------------------------
void Init();

// ----------------------------------------------------------------------------
void InitHorizFX1()
{
__asm
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	call &F8E0
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld (&0000), a
	ei
__endasm;
}

// ----------------------------------------------------------------------------
void InitHorizFX2()
{
__asm
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	call &F8E3
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld (&0000), a
	ei
__endasm;
}

// ----------------------------------------------------------------------------
void DoHorizFX()
{
__asm
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	call &F8E6
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld (&0000), a
	ei
__endasm;
}

// ----------------------------------------------------------------------------
void WaitVBLCustom()
{
__asm
	ld a, ( &0003 )
	ld b, a
WaitVBLCustomLoop:
	ld a, ( &0003 )
	cp b
	jr z, WaitVBLCustomLoop	
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	if ( IsScreenFlipped() == 0 )
	{
		FlipScreen();
	}
	
	WaitVBL();
	SetPalette( GetPalette2() );
		
	while( frame != 170 )
	{
		DoFX();
		FlipScreen();
		WaitVBL();
		SetPalette( GetPalette1() );
		
		DoFX();		
		FlipScreen();
		WaitVBL();
		SetPalette( GetPalette2() );
		
		frame++;
	}
	
	frame = 0;
	while( frame != 54 )
	{
		InitHorizFX1();
		FlipScreen();
		WaitVBL();
		SetPalette( GetPalette1() );
		
		InitHorizFX2();
		FlipScreen();
		WaitVBL();
		SetPalette( GetPalette2() );
		
		frame++;
	}
		
	frame = 0;
	while( frame != 110 )
	{
		DoHorizFX();
		DoWait();
		FlipScreen();
		WaitVBL();
		SetPalette( GetPalette1() );
		
		DoHorizFX();	
		DoWait();	
		FlipScreen();
		WaitVBL();
		SetPalette( GetPalette2() );
		
		frame++;
	}
	
	DoHorizFX();
	DoWait();	
	FlipScreen();
	WaitVBL();
	SetPalette( GetPalette1() );
}

// ----------------------------------------------------------------------------
void DoFX()
{
__asm
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		;db &ed, &ff
		
		call &BB03
		
		di
		pop af
		ld ( &0000 ), a
		ld b, &7F
		ld c, a
		out (c), c
		ei
__endasm;
}

// ----------------------------------------------------------------------------
void InitBottomPart()
{
__asm	
		di
		ld a, (&0000)
		push af
		ld bc, &7FC0
		out (c), c		
		ld a, c
		ld (&0000), a
		ei
		
		ld hl, VIDEOSEGMENTC000
		ld de, &E800
		ld a, 9
		call LinearScreenCopy
		ld de, &6800
		ld a, 12
		call LinearScreenCopy
		
		di
		pop af
		ld (&0000), a
		ld b, &7F
		ld c, a
		out (c), c		
		ei
		
		jr InitBottomPart_End
		
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
		
InitBottomPart_End:
__endasm;
}

// ----------------------------------------------------------------------------
void Init()
{	
	LoadFile( BATMANCIRCLEFULLBINPCK_TRACKFILEID, VIDEOSEGMENT8000 );
	BankUnpack( 0xC0, 0x2B00, VIDEOSEGMENT8000 );
	
	LoadFile( TRANSITHORIZLINESFXDATA_2800Z80BINPCK_TRACKFILEID, VIDEOSEGMENT8000 );
	BankUnpack( 0xC0, 0x2800, VIDEOSEGMENT8000 );
	
	InitBottomPart();
	
__asm
	call &BB00
__endasm;
	
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );	
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
}
