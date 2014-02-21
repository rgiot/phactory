
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

#include "EndPart.h"

// ----------------------------------------------------------------------------
char *GetCondenseLogoPalette()
{
__asm
	ld hl, CondensePalette
	jr QuitGetCondensePalette
CondensePalette:
	incbin "CondenseLogo.cpcbitmap.palette"
QuitGetCondensePalette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetCondenseLogoFade1Palette()
{
__asm
	ld hl, CondenseFade1Palette
	jr QuitGetCondenseFade1Palette
CondenseFade1Palette:
	incbin "CondenseLogoFade1.bmp.fadePalette"
QuitGetCondenseFade1Palette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetCondenseLogoFade2Palette()
{
__asm
	ld hl, CondenseFade2Palette
	jr QuitGetCondenseFade2Palette
CondenseFade2Palette:
	incbin "CondenseLogoFade2.bmp.fadePalette"
QuitGetCondenseFade2Palette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetCondenseLogoFade3Palette()
{
__asm
	ld hl, CondenseFade3Palette
	jr QuitGetCondenseFade3Palette
CondenseFade3Palette:
	incbin "CondenseLogoFade3.bmp.fadePalette"
QuitGetCondenseFade3Palette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetCondenseLogoFade4Palette()
{
__asm
	ld hl, CondenseFade4Palette
	jr QuitGetCondenseFade4Palette
CondenseFade4Palette:
	incbin "CondenseLogoFade4.bmp.fadePalette"
QuitGetCondenseFade4Palette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetCondenseLogoFade5Palette()
{
__asm
	ld hl, CondenseFade5Palette
	jr QuitGetCondenseFade5Palette
CondenseFade5Palette:
	incbin "CondenseLogoFade5.bmp.fadePalette"
QuitGetCondenseFade5Palette:
__endasm;
}

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "EndPart.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
// STEP 2
char *GetFadePalette1()
{
__asm
	ld hl, palette1
	ld de, outputPalette
	ld bc, 16
	ldir
	
	ld hl, outputPalette
	jr QuitGetFadePalette1
palette1:
	db COLOR0
	db COLOR0
	db COLOR12
	db COLOR3
	db COLOR0
	db COLOR15
	
	db COLOR1
	db COLOR11
	db COLOR14
	db COLOR23
	
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	
outputPalette:
	incbin "EndPart.cpcbitmap.palette"
QuitGetFadePalette1:
__endasm;
}
void SetAsicFadePalette1()
{
__asm
	ld de, asicPalette2
	ld hl, palette_fadeStep2
	call SetAsicPalette
	jp QuitSetAsicFadePalette1
asicPalette2:
	db 0
	db 0
	db 1
	db 2
	db 0
	db 3
	
	db 4
	db 5
	db 6
	db 7
	
	db 0
	db 0
	db 0
	db 0
	db 0
	db 0
	db 0
	db 0
	
palette_fadeStep2:
	incbin "EndPartFade_Step2.cpcbitmap.palette"
	
; ----------------------------------------------------------------------------
SetAsicPalette:
	ld ( SetAsicPalettePtr + 1 ), hl
	ex de, hl
	xor a
SetAsicPaletteLoop:
	push af
	push hl
	ld d, (hl)
	ld e, a
SetAsicPalettePtr:
	ld hl, palette
	call SetAsicColor
	pop hl
	pop af
	inc hl
	inc a
	cp 16
	jr nz, SetAsicPaletteLoop
	ret
	
; ----------------------------------------------------------------------------
SetAsicColor:
	di
	ld bc, &7FB8
	out (c), c
	
	push hl
	
	ld b, 0
	ld c, e
	ld hl, &6400
	add hl, bc
	add hl, bc
	
	ld a, d
	ex de, hl
	
	pop hl
	
	; skip Gate Array palette
	ld bc, 16
	add hl, bc
	
	add a, a
	ld c, a
	add hl, bc
	
	ldi
	ldi
	
	ld bc, &7FA0
	out (c), c
	ei
	ret
QuitSetAsicFadePalette1:
__endasm;
}

// ----------------------------------------------------------------------------
// STEP 3
char *GetFadePalette2()
{
__asm
	ld hl, palette2
	ld de, outputPalette
	ld bc, 16
	ldir
	
	ld hl, outputPalette
	jr QuitGetFadePalette2
palette2:
	db COLOR0
	db COLOR0
	db COLOR3
	db COLOR0
	
	db COLOR0
	db COLOR12
	db COLOR0
	db COLOR0
	
	db COLOR1	
	db COLOR14
	db COLOR0
	db COLOR0
	
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
QuitGetFadePalette2:
__endasm;
}
void SetAsicFadePalette2()
{
__asm
	ld de, asicPalette3
	ld hl, palette_fadeStep3
	call SetAsicPalette
	jp QuitSetAsicFadePalette3
asicPalette3:
	db 0
	db 0
	db 1
	db 0
	
	db 0
	db 2
	db 0
	db 0
	
	db 3	
	db 4
	db 0
	db 0
	
	db 0
	db 0
	db 0
	db 0
	
palette_fadeStep3:
	incbin "EndPartFade_Step3.cpcbitmap.palette"
QuitSetAsicFadePalette3:
__endasm;
}	

// ----------------------------------------------------------------------------
// STEP 4
char *GetFadePalette3()
{
__asm
	ld hl, palette3
	ld de, outputPalette
	ld bc, 16
	ldir
	
	ld hl, outputPalette
	jr QuitGetFadePalette3
palette3:
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	
	db COLOR0
	db COLOR3
	db COLOR0
	db COLOR0
	
	db COLOR0	
	db COLOR1
	db COLOR0
	db COLOR0
	
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
QuitGetFadePalette3:
__endasm;
}
void SetAsicFadePalette3()
{
__asm
	ld de, asicPalette4
	ld hl, palette_fadeStep4
	call SetAsicPalette
	jp QuitSetAsicFadePalette4
asicPalette4:
	db 0
	db 0
	db 0
	db 0
	
	db 0
	db 1
	db 0
	db 0
	
	db 0	
	db 2
	db 0
	db 0
	
	db 0
	db 0
	db 0
	db 0
	
palette_fadeStep4:
	incbin "EndPartFade_Step4.cpcbitmap.palette"
QuitSetAsicFadePalette4:
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
char IsAsic;
void SetPal(char step)
{
	WaitVBL();
	
__asm
	ld a, ( &0037 )
	ld ( _IsAsic ), a
__endasm;
		
	if ( IsAsic != 0 )
	{
		if ( step == 1 )
		{		
			SetAsicFadePalette1();
		}
		else if ( step == 2 )
		{
			SetAsicFadePalette2();
		}
		else if ( step == 3 )
		{
			SetAsicFadePalette3();
		}
		else
		{
			SetPalette(GetPalette());
		}	
	}
	else
	{
		if ( step == 1 )
		{		
			SetPalette(GetFadePalette1());
		}
		else if ( step == 2 )
		{
			SetPalette(GetFadePalette2());
		}
		else if ( step == 3 )
		{
			SetPalette(GetFadePalette3());
		}
		else
		{
			SetPalette(GetPalette());
		}	
	}
		
__asm
	ld hl, &1000
	ld de, &1000
	ld bc, &100
	ldir
__endasm;
}

// ----------------------------------------------------------------------------
void SetCondensePal(char step)
{
	WaitVBL();
	
	if ( step == 1 )
	{		
		SetPalette( GetCondenseLogoFade1Palette() );
	}
	else if ( step == 2 )
	{
		SetPalette( GetCondenseLogoFade2Palette() );
	}
	else if ( step == 3 )
	{
		SetPalette( GetCondenseLogoFade3Palette() );
	}
	else if ( step == 4 )
	{
		SetPalette( GetCondenseLogoFade4Palette() );
	}
	else if ( step == 5 )
	{
		SetPalette( GetCondenseLogoFade5Palette() );
	}
	else
	{
		SetPalette(GetCondenseLogoPalette());
	}
		
__asm
	ld hl, &1000
	ld de, &1000
	ld bc, &100
	ldir
__endasm;
}

// ----------------------------------------------------------------------------
void ShowScreen( char *top, char *bottom )
{
	unsigned short i;
	unsigned char timeFadeEnd;
	
	timeFadeEnd = 5;
	
	WaitVBL();
	SetBlackPalette();
	
	BankUnpack( 0xC0, VIDEOSEGMENT8000, top );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, bottom );
	
	for ( i = 0 ; i < 5; i++ )
	{
		SetPal(3);
	}
	for ( i = 0 ; i < 5; i++ )
	{
		SetPal(2);
	}
	for ( i = 0 ; i < 5; i++ )
	{
		SetPal(1);
	}
	
	for ( i = 0 ; i < 150; i++ )
	{
		SetPal(0);
	}
	
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetPal(1);
	}
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetPal(2);
	}
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetPal(3);
	}
	
	for ( i = 0 ; i < 70; i++ )
	{
		WaitVBL();
		SetBlackPalette();
	}
}

// ----------------------------------------------------------------------------
void ShowCondenseLogo()
{
	unsigned short i;
	unsigned char timeFadeEnd;
	
	timeFadeEnd = 10;
	
	WaitVBL();
	SetBlackPalette();
	
	BankUnpack( 0xC0, VIDEOSEGMENT8000, CONDENSELOGOBMPTOPBINPCK_PTR );
	BankUnpack( 0xC0, VIDEOSEGMENTC000, CONDENSELOGOBMPBOTTOMBINPCK_PTR );
	
	for ( i = 0 ; i < 5; i++ )
	{
		SetCondensePal(1);
	}
	for ( i = 0 ; i < 5; i++ )
	{
		SetCondensePal(2);
	}
	for ( i = 0 ; i < 5; i++ )
	{
		SetCondensePal(3);
	}
	for ( i = 0 ; i < 5; i++ )
	{
		SetCondensePal(4);
	}
	for ( i = 0 ; i < 5; i++ )
	{
		SetCondensePal(5);
	}
	
	for ( i = 0 ; i < 300; i++ )
	{
		SetCondensePal(0);
	}
	
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetCondensePal(5);
	}
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetCondensePal(4);
	}
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetCondensePal(3);
	}
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetCondensePal(2);
	}
	for ( i = 0 ; i < timeFadeEnd; i++ )
	{
		SetCondensePal(1);
	}
}

// ----------------------------------------------------------------------------
void InitLinesFirst();

// ----------------------------------------------------------------------------
void Main()
{
	unsigned short i;
	
	ClearScreen();
	
	InitLinesFirst();
	
	LoadFile( ENDPARTBIGFILE_TRACKFILEID, ENDPARTH_DATAPTR );
	
	ClearScreen();	
	for ( i = 0 ; i < 30; i++ )
	{
		WaitVBL();
		SetBlackPalette();
	}
	
	ShowScreen( ENDPART01_VISUALSBMPTOPBINPCK_PTR, ENDPART01_VISUALSBMPBOTTOMBINPCK_PTR );
	ShowScreen( ENDPART02_AUDIOBMPTOPBINPCK_PTR, ENDPART02_AUDIOBMPBOTTOMBINPCK_PTR );
	ShowScreen( ENDPART03_PROGRAMMINGBMPTOPBINPCK_PTR, ENDPART03_PROGRAMMINGBMPBOTTOMBINPCK_PTR );
	ShowScreen( ENDPART04_ADDPROGRAMMINGBMPTOPBINPCK_PTR, ENDPART04_ADDPROGRAMMINGBMPBOTTOMBINPCK_PTR );
	ShowScreen( ENDPART05_THANKSFORWATCHINGBMPTOPBINPCK_PTR, ENDPART05_THANKSFORWATCHINGBMPBOTTOMBINPCK_PTR );
	ShowCondenseLogo();
	
	ClearScreen();	
	
	for ( i = 0 ; i < 1000; i++ )
	{
		WaitVBL();
		SetBlackPalette();
	}
	
__asm	
	ld a, 1 ; wait part to load to launch music
	ld ( &0004 ), a	
__endasm;
}

// ----------------------------------------------------------------------------
#include "InitLines.c"

// ----------------------------------------------------------------------------
void InitLinesFirst()
{
	InitLines();
}
