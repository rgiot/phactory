
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
#include "DrawTiles.cxx"
#include "DrawEdges.cxx"

#include "MangaData1.h"
#include "MangaData2.h"

// ----------------------------------------------------------------------------
#define FXSEGMENT				0xBC00

// ----------------------------------------------------------------------------
unsigned char frame;
char **BackLines;
char *bandOpcodesPtr;
char *spriteScanlinesPtr;
char **fxScanlinesBasePtr;
char **fxScanlinesPtr;

unsigned short offset = 127;

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
void UpdateFXTable()
{
	fxScanlinesPtr = fxScanlinesBasePtr + offset;
	
	offset--;
	if ( offset == -1 )	
	{
		offset = 127;
	}
}

// ----------------------------------------------------------------------------
void DrawBand()
{
__asm
	ld bc, (_bandOpcodesPtr)
	ld ( OpcodeJump + 1 ), bc
	
	xor a
	ld ( TextureOffset + 1 ), a
	xor a
	ld ( OpcodeJump + 1 ), a
	
	ld a, 205
	ld hl, (_BackLines)
	
	ld iy, (_fxScanlinesPtr)
MainLoop:	
	ex af, af'
		
	exx
	ld a, (hl)
	inc hl
	cp 254
	jr nz, nextTest
	exx
	inc hl
	inc hl
	inc iy
	inc iy
	jp EndLoop
nextTest:
	cp 255
	jp z, duplicatedLine
	
	push hl
	ld hl, (_bandOpcodesPtr)
	ld de, 64
	add hl, de
	ld e, &ed ; ldd
	ld d, &a8 ; 
	di
	ld (saveSP+1), sp
	ld sp, hl
REPT 32
	push de
ENDM
saveSP:
	ld sp, 0	
	ei

	pop hl
	
	or a
	jr z, endWriteNewLine
	
	ld de, (_bandOpcodesPtr)
	ld c, a
writeSkipBytes:
	ld e, (hl)
	inc hl
	
	ld a, &2d 
	ld (de), a; dec l
	inc e
	ld (de), a; dec l
	inc e
	ld a, &1d 
	ld (de), a; dec e
	inc e
	ld (de), a; dec e
	
	dec c
	jr nz, writeSkipBytes	
	
endWriteNewLine:
	
LeftOrCenter:
	ld a, 0
	or a
	jr z, LeftOrCenter_ComputeSkipValues
Right_ComputeSkipValues:
	ld a, (hl)
	inc hl
	
	ex de, hl
	
	ld hl, (_bandOpcodesPtr)
	ld l, a
	ld (hl), &dd ; jp (ix)
	inc l
	ld (hl), &e9
	
	ex de, hl
	
	jr End_ComputeSkipValues
	
LeftOrCenter_ComputeSkipValues:
	ld a, (hl)
	inc hl
	ld ( TextureOffset + 1 ), a
	ld a, (hl)
	inc hl
	ld ( OpcodeJump + 1 ), a
End_ComputeSkipValues:

duplicatedLine:
	exx
	
	ld a, (hl)
	inc hl
	push hl
	ld h, (hl)
	ld l, a
	
	ld b, 0	
FXOffset:
	ld c, 32
TextureOffset:
	ld a, 0
	ld e, a
	add a, c
	ld c, a
	add hl, bc	
	
	ex de, hl
	
	ld a, l
	add a, (iy)
	ld l, a
	inc iy
	ld h, (iy)
	inc iy
	
	dec de
	dec hl
	
	ld ix, OpcodeJump_Next
OpcodeJump:
	jp 0	
OpcodeJump_Next:

	pop hl
	inc hl

EndLoop:
	ex af, af'
	dec a
	jp nz, MainLoop	
__endasm;
}

// ----------------------------------------------------------------------------
void DrawBands()
{
__asm
	ld a, 32
	ld ( FXOffset + 1 ), a
	xor a
	ld ( LeftOrCenter + 1 ), a
	exx
	ld hl, MANGABANDLEFTBIN_PTR
	exx
__endasm;
	DrawBand();
	
__asm
	ld a, 64
	ld ( FXOffset + 1 ), a	
	exx
	ld hl, MANGABANDCENTERBIN_PTR
	exx
__endasm;
	DrawBand();	
	
__asm
	ld a, 96
	ld ( FXOffset + 1 ), a	
	ld ( LeftOrCenter + 1 ), a
	exx
	ld hl, MANGABANDRIGHTBIN_PTR
	exx
__endasm;
	DrawBand();
}

// ----------------------------------------------------------------------------
void Init()
{	
	unsigned short i;	
	
	fxScanlinesBasePtr = FXSEGMENT; // size = 205+128*2	
	for( i = 0; i<205+128; i++ )
	{
		if ( (i==0)||(i==128)||(i==256) )
		{
			spriteScanlinesPtr = 31 + MANGAFX128X128BMPSPRRAWDATA1_PTR;	
		}
		
		fxScanlinesBasePtr[i] = spriteScanlinesPtr;
		
		spriteScanlinesPtr += 32;
	}
	
	bandOpcodesPtr = FXSEGMENT + 512 + 256;	
	
__asm	
	ld de,( _bandOpcodesPtr)
	ld hl, WriteStackValue
	ld bc, WriteStackValue_End - WriteStackValue
	ldir
	
	jr WriteStackValue_End
	
WriteStackValue:
REPT 16
	ldd
	ldd
ENDM
WriteStackValue_JPEND:
	jp (ix)
WriteStackValue_End:
__endasm;	
	
	PushBank( BANKC3 );
	LoadFile( MANGADATA1BIGFILEPCK_TRACKFILEID, CODESEGMENT4000 );
	Unpack( CODESEGMENT0000, CODESEGMENT4000 );
	PopBank();
	
	bank_memcpy( 0xC0, CODESEGMENTC000, CODESEGMENT4000, CODESEGMENT4000_SIZE );
}

// ----------------------------------------------------------------------------
void DrawFrame()
{
	BackLines = GetBackLines();
	PushBankBottom();
	
	UpdateFXTable();
	//UpdateFXTable(); // 2 times faster
	
	DrawBands();
	DrawBGEdges( MANGABACKGROUNDMASKBMPSPRDATA1_PTR, GetTopBackVideoPtr(), GetBottomBackVideoPtr() );
	
	PopBank();
}

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	// Draw 1st Frame	
	DrawFrame();
	
	// Draw cloud of 1st Frame
	bank_memswap( 0xC6, CODESEGMENT0000, CODESEGMENT4000, CODESEGMENT4000_SIZE );
	DrawCloud_VIDEOSEGMENT8000();	
	bank_memswap( 0xC6, CODESEGMENT0000, CODESEGMENT4000, CODESEGMENT4000_SIZE );
	
	for ( frame = 0; frame < 40; frame++ )
	{	
		DrawFrame();
	
		FlipScreen();
	}
	
	LoadFile( MANGATILE2BMPSPRRAWDATA1PCK_TRACKFILEID_2, VIDEOSEGMENT0000 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	bank_memcpy( 0xC6, CODESEGMENT0000, CODESEGMENT4000, CODESEGMENT4000_SIZE );
	DrawCloud_VIDEOSEGMENT8000();
	
	// Load Tile (unshifted)
	bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENT4000_SIZE );	
}
