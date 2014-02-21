
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

#include "DrawTiles.cxx"
#include "DrawEdges.cxx"

#include "PlasmaData1.h"

// ----------------------------------------------------------------------------
unsigned char frame;
char **BackLines;

unsigned char *ScreenTopPtr;
unsigned char *ScreenBottomPtr;

unsigned char *SpritePtr;
unsigned char PosX;
unsigned char PosY;
unsigned char Height;
unsigned char OffsetY;

unsigned char SineCurveFrameInc1;
unsigned char SineCurveFrameInc2;
unsigned char FramePlasmaOffset1;
unsigned char FramePlasmaOffset2;
unsigned char *SineCurveFrame;
unsigned char *RasterBar;
unsigned char RasterBarFrame;

// ----------------------------------------------------------------------------
#define FXSEGMENT				0x2800

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "Plasma.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void CreatePlasmaCurve()
{
__asm
	ld a, (_FramePlasmaOffset1)
	ld b, a
	ld a, (_FramePlasmaOffset2)
	ld c, a
	
	ld hl, (_SineCurveFrame)
	ex de, hl
	
	ld hl, PLASMASINECURVE1BIN_PTR
	
	ld a, 256/8
CreatePlasmaCurveLoop:
	ex af, af'
	
REPT 8
	ld l, b
	ld a, (hl)
	ld l, c		
	add a, (hl)
	
	inc b
	inc c
	
	ld (de), a
	inc de
ENDM
	
	ex af, af'
	dec a
	jr nz, CreatePlasmaCurveLoop
	
__endasm;
}

// ----------------------------------------------------------------------------
unsigned char RasterBarPos;
void AddRasterBar()
{
__asm
	ld hl, (_RasterBar)
	ld a, ( _RasterBarPos )
	ld b, 0
	ld c, a
	add hl, bc
	add hl, bc
	
	ex de, hl
	
	ld hl, RasterBarData
	
REPT 20
	ldi
ENDM

	jr RasterBarEnd
	
RasterBarData:
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96+96+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96+96+96+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96+96+96+96+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96+96+96+96+96+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96+96+96+96+96+96+96
	dw PLASMARASTERBARBMPSPRRAWDATA1_PTR+96+96+96+96+96+96+96+96+96
	
RasterBarEnd:
__endasm;
}

// ----------------------------------------------------------------------------
void CreateRasterBar()
{
	memset( RasterBar, 0, 410 );
	
__asm
	xor a
AddRasterLoop:
	push af

	ld h, PLASMASINECURVE3BIN_PTR / 256
	
	sla a
	sla a
	sla a
	ld b, a
	ld a, ( _RasterBarFrame )	
	add a, b
	
	ld l, a
	
	ld a, (hl)
	ld ( _RasterBarPos ), a
	call _AddRasterBar	
	
	pop af
	inc a
	cp 4
	jr nz, AddRasterLoop
	
	xor a
AddRasterLoop2:
	push af

	ld h, PLASMASINECURVE3BIN_PTR / 256
	
	sla a
	sla a
	sla a
	ld b, a
	ld a, ( _RasterBarFrame )	
	add a, b
	add a, 128
	
	ld l, a
	
	ld a, (hl)
	ld ( _RasterBarPos ), a
	call _AddRasterBar	
	
	pop af
	inc a
	cp 4
	jr nz, AddRasterLoop2
__endasm;
}

// ----------------------------------------------------------------------------
void DrawZone()
{
__asm
DrawZone:
REPT 96
	ldi
ENDM
	ret
DrawZoneEnd:
__endasm;
}

// ----------------------------------------------------------------------------
void DrawPlasma()
{
__asm	
	exx
	ld h, FXSEGMENT/256
	ld bc, PLASMAZONESBIN_PTR
	exx

	ld hl, PLASMASINECURVE2BIN_PTR
	ld a, h
	ld ( SineCurve2Ptr + 1 ), a
	
	ld hl, (_SineCurveFrame)
	ld a, h
	ld ( SineCurveFramePtr + 1 ), a	
	
	ld hl, (_RasterBar)
	push hl
	pop iy
	
	ld de, (_ScreenTopPtr)
	
	ld a, (_SineCurveFrameInc1)
	ld b, a
	ld a, (_SineCurveFrameInc2)
	ld c, a
	
	ld a, 20 ; TOP
	ld (DrawPlasma_CharCount+1), a

DrawPlasma_CharCount:	
	ld a, 20
DrawPlasmaLoop:
	ex af, af'
	
	push de

	call DrawPlasmaLine
	
	set 3, d
	call DrawPlasmaLine
	
	res 3, d
	set 4, d
	call DrawPlasmaLine
	
	set 3, d
	call DrawPlasmaLine
	
	set 5, d
	res 4, d
	res 3, d
	call DrawPlasmaLine
	
	pop hl
	ld de, 96
	add hl, de
	ex de, hl
		
	ex af, af'
	dec a
	jr nz, DrawPlasmaLoop
	
	ld a, (DrawPlasma_CharCount+1)
	cp 21
	jp z, DrawPlasmaEnd
	inc a
	ld (DrawPlasma_CharCount+1), a
	ld de, (_ScreenBottomPtr)
	jr DrawPlasma_CharCount
	
DrawPlasmaLine:	
	push de
		
SineCurve2Ptr:
	ld h, 0
	ld l, b
	ld a, (hl)
	ld l, c		
	add a, (hl)	
SineCurveFramePtr:
	ld h, 0
	ld l, a
	ld a, (hl)
	push bc
	
	; a = x offset
	sra a
	jr nc, DrawPlasmaOffset
	ld h, PLASMALINEINTERLACED1BMPSPRRAWDATA1_PTR/256
	jr DrawPlasmaOffsetEnd
DrawPlasmaOffset:
	ld h, PLASMALINEINTERLACED1BMPSPRRAWDATA2_PTR/256
DrawPlasmaOffsetEnd:	
	ld l, a
	
	ex af, af'
	bit 0, a
	jr z, NotInterlaced
	inc h
NotInterlaced:
	ex af, af'	
	
	ld a, (iy)
	inc iy
	or a
	jr z, skipRaster
	ld l, a
	ld h, (iy)
skipRaster:
	inc iy
	
	exx
	ld a, (bc)	
	inc bc
	exx
	bit 7, a
	jr nz, EndZoneLine	
DrawZones_Loop:
	push hl
	push de
	
	ld b, 0
	ld c, a	
	add hl, bc ; inc src
	
	ex de, hl
	add hl, bc ; inc dst
	ex de, hl
	
	exx
	ld a, (bc)
	inc bc
	ld l, a	
	ld (DrawZone_Call + 1), hl
	exx
	
DrawZone_Call:
	call DrawZone
	
	pop de
	pop hl
	
	exx
	ld a, (bc)
	inc bc
	exx
	bit 7, a
	jr z, DrawZones_Loop
	
EndZoneLine:	
	pop bc
	inc b
	inc c	
	pop de
	ret
	
DrawPlasmaEnd:
__endasm;
}

// ----------------------------------------------------------------------------
void Init()
{	
__asm
	ld hl, DrawZone
	ld de, FXSEGMENT
	ld bc, DrawZoneEnd - DrawZone
	ldir
__endasm;

	SineCurveFrame = FXSEGMENT + 256;
	
	RasterBar = SineCurveFrame + 256;
	
	FramePlasmaOffset1 = 34;
	FramePlasmaOffset2 = -42;
}

// ----------------------------------------------------------------------------
void DrawFrame()
{
	BackLines = GetBackLines();
	
	PushBankBottom();
	
	CreatePlasmaCurve();	
	CreateRasterBar();
	
	ScreenTopPtr = GetTopBackVideoPtr();
	ScreenBottomPtr = GetBottomBackVideoPtr();
	
	DrawPlasma();
	
	DrawBGEdges( PLASMABITMAPMASKBMPSPRDATA1_PTR, GetTopBackVideoPtr(), GetBottomBackVideoPtr() );
	
	SineCurveFrameInc1 += 3;
	SineCurveFrameInc2 += 7;
	FramePlasmaOffset1 += 12;
	FramePlasmaOffset2 -= 8;
	
	RasterBarFrame += 4;
	
	PopBank();
}

// ----------------------------------------------------------------------------
void TransitFrameToVIDEOSEGMENT8000()
{
__asm
	ld hl, TransitVertLinesFXCode
	ld de, &BD00
	ld bc, TransitVertLinesFXCodeEnd-TransitVertLinesFXCode
	ldir
	
	call &BD00

	ld a, 19
TransitLoop:
	push af
	call &BD03
	pop af
	dec a
	jr nz, TransitLoop
	
	jp Transit1stFrameEnd
	
TransitVertLinesFXCode:
	incbin "TransitVertLinesFXCode_BD00.z80.bin"
TransitVertLinesFXCodeEnd:
	
Transit1stFrameEnd:
	
__endasm;
}

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	// Draw 1st Frame	
	DrawFrame();
	
	// Transit 1st Frame
	TransitFrameToVIDEOSEGMENT8000();
	
	for ( frame = 0; frame < 73; frame++ )
	{	
		DrawFrame();
	
		FlipScreen();
	}
	
	bank_memcpy( 0xC0, VIDEOSEGMENT8000, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENTC000, VIDEOSEGMENT4000, VIDEOSEGMENT4000_SIZE );
	FlipScreen();
	
	LoadFile( PLASMATILE2BMPSPRRAWDATA1PCK_TRACKFILEID_3, VIDEOSEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	TransitFrameToVIDEOSEGMENT8000();
}