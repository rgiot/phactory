
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

#include "LensData1.h"
#include "LensData2.h"
#include "LensWriter.h"

// ----------------------------------------------------------------------------
#define LENSWRITER_CODE 0x3500

#define LENS_BANK2LINES2_TOPCLIPPTR		0x3100
#define LENS_BANK2LINES2_TOPPTR			LENS_BANK2LINES2_TOPCLIPPTR + 0x100
#define LENS_BANK3LINES2_BOTTOMPTR		LENS_BANK2LINES2_TOPPTR + 100+100
#define LENS_BANK3LINES2_BOTTOMCLIPPTR	LENS_BANK3LINES2_BOTTOMPTR + 105+105

// ----------------------------------------------------------------------------
unsigned char frame;
char **BackLines;
char **BackLinesSprite;

char **TopHighLines;
char **BottomHighLines;

char **RelLowLines;
char **RelHighLines;

unsigned short FrameCount;

unsigned char *NextIX;

unsigned char *SpritePtr;
unsigned char PosX;
unsigned short PosY;
unsigned short RestoreHeight;

unsigned char FrameCirclePosXSens;
unsigned char FrameCirclePosX;
unsigned short FrameCirclePosY;
unsigned char *FrameCirclePosYSineCurvePtr;
int FrameCirclePosYSineCurveOffset;

unsigned char CirclePosX;
unsigned short CirclePosY;

unsigned char *SaveCirclePtr;

unsigned char *SaveCirclePtr1;
unsigned char SaveCirclePosX1;
unsigned short SaveCirclePosY1;

unsigned char *SaveCirclePtr2;
unsigned char SaveCirclePosX2;
unsigned short SaveCirclePosY2;

unsigned char *ZoomPtr;
unsigned char ZoomU;
unsigned short ZoomV;
unsigned char ZoomPosX;
unsigned short ZoomPosY;

unsigned char *PrecaZoomPixels;

unsigned short BankPadding;

unsigned short *MovePtr;

// ----------------------------------------------------------------------------
#define ZOOM_WIDTH 			32 // 24
#define ZOOM_HEIGHT			55 // 42

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, palette
	jr QuitGetPalette
palette:
	incbin "Lens.cpcbitmap.palette"
QuitGetPalette:
__endasm;
}

// ----------------------------------------------------------------------------
void InitPrecaZoomPixels()
{
__asm
	ld hl, (_PrecaZoomPixels)
	ex de, hl
	xor a
precaZoomPixelsLoop:
	push af
	
	ld b, a
	and &AA ; left=15  right=0
	ld c, a
	rr c
	or c
	ld (de), a
	
	ld a, b
	and &55 ; left=0  right=15
	ld c, a
	rl c
	or c
	inc d
	ld (de), a
	dec d
	
	inc de
	
	pop af
	inc a
	jr nz, precaZoomPixelsLoop	
__endasm;
}

// ----------------------------------------------------------------------------
void CreateZoomTopLeft()
{	
__asm
	exx
	ld hl, (_ZoomPtr)
	ex de, hl	
	ld hl, (_PrecaZoomPixels)
	exx

	ld bc, (_ZoomV)
	ld hl, BANK0LINES_TOPPTR-256
	add hl, bc
	add hl, bc
	
	; pad to bank C6
	push hl
	ld h, 0
	ld a, (_ZoomU)
	ld l, a
	ld bc, (_BankPadding)
	add hl, bc
	ld b, h
	ld c, l
	pop hl
	
	ld a, (_RestoreHeight)
	call LENSWRITER_CODE + 6 ; _CreateZoomLeft
	
	exx
	ex de, hl
	ld (_NextIX), hl
	exx
__endasm;
}

// ----------------------------------------------------------------------------
void CreateZoomBottomLeft()
{	
__asm
	exx
	ld hl, (_NextIX)
	ex de, hl	
	ld hl, (_PrecaZoomPixels)
	exx

	ld bc,(_ZoomV)
	ld hl, BANK1LINES_BOTTOMPTR-256-100-100
	add hl, bc
	add hl, bc
	
	; X+ pad to bank C1 (using &C000)
	push hl
	ld h, 0
	ld a, (_ZoomU)
	ld l, a
	ld bc, (_BankPadding)
	add hl, bc
	ld b, h
	ld c, l
	pop hl

	ld a, (_RestoreHeight)
	jp LENSWRITER_CODE + 6 ; _CreateZoomLeft
__endasm;
}

// ----------------------------------------------------------------------------
void CreateZoomTopRight()
{
__asm
	exx
	ld hl, (_PrecaZoomPixels)
	exx
	
	ld hl, (_ZoomPtr)
	push hl
	pop de
	
	ld bc, (_ZoomV)
	ld hl, BANK0LINES_TOPPTR-256
	add hl, bc
	add hl, bc
	
	; X + pad to bank C6
	push hl
	ld h, 0
	ld a, (_ZoomU)
	ld l, a
	ld bc, (_BankPadding)
	add hl, bc
	ld b, h
	ld c, l
	pop hl

	ld a, (_RestoreHeight)	
	call LENSWRITER_CODE + 9 ; _CreateZoomRight
	
	ex de, hl
	ld (_NextIX), hl
__endasm;
}

// ----------------------------------------------------------------------------
void CreateZoomBottomRight()
{
__asm
	exx
	ld hl, (_PrecaZoomPixels)
	exx
	
	ld hl, (_NextIX)
	push hl
	pop de
	
	ld bc, (_ZoomV)
	ld hl, BANK1LINES_BOTTOMPTR - 256-100-100
	add hl, bc
	add hl, bc
	
	; X + pad to bank C1 (using &C000)
	push hl
	ld h, 0
	ld a, (_ZoomU)
	ld l, a
	ld bc, (_BankPadding)
	add hl, bc
	ld b, h
	ld c, l
	pop hl

	ld a, (_RestoreHeight)
	jp LENSWRITER_CODE + 9 ; _CreateZoomRight
__endasm;
}

// ----------------------------------------------------------------------------
void CreateZoom()
{
	memset( DUMMYLINEPTR, 192 /* pen=1=black */, 0x60 );
	
	NextIX = ZoomPtr;

	ZoomU = (ZoomPosX>>1) + 12;		
	ZoomV = ZoomPosY + 20;
	
	if ( ZoomV<128+100-ZOOM_HEIGHT)
	{	
		RestoreHeight = ZOOM_HEIGHT;
		
		BankPadding = 0x4000;
	
__asm
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC6
		out (c), c
		ld a, &C6
		ld ( &0000 ), a
		ei
__endasm;

		if ( (ZoomPosX & 1) == 0 )
		{
			CreateZoomTopLeft();
		}
		else
		{
			CreateZoomTopRight();
		}
		
__asm
		di
		pop af
		ld b, &7F
		ld c, a
		out (c), c
		ld ( &0000 ), a
		ei		
__endasm;
	}
	else if ( ZoomV>=(100+128) )
	{	
		RestoreHeight = ZOOM_HEIGHT;
		
		BankPadding = 0x8000;
		
		if ( (ZoomPosX & 1) == 0 )
		{
			CreateZoomBottomLeft();
		}
		else
		{
			CreateZoomBottomRight();
		}
	}
	else
	{	
		RestoreHeight = 128+100-ZoomV;

__asm
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC6
		out (c), c
		ld a, &C6
		ld ( &0000 ), a
		ei
__endasm;
	
		BankPadding = 0x4000;
	
		if ( (ZoomPosX & 1) == 0 )
		{
			CreateZoomTopLeft();
		}
		else
		{
			CreateZoomTopRight();
		}
		
__asm
		di
		pop af
		ld b, &7F
		ld c, a
		out (c), c
		ld ( &0000 ), a
		ei		
__endasm;
		
		ZoomV = 128+100;
		RestoreHeight = ZOOM_HEIGHT-RestoreHeight;
		
		if ( RestoreHeight != 0 )
		{		
			BankPadding = 0x8000;
		
			if ( (ZoomPosX & 1) == 0 )
			{
				CreateZoomBottomLeft();
			}
			else
			{
				CreateZoomBottomRight();
			}
		}
	}
	
	ZoomPosX >>= 1;
}

// ----------------------------------------------------------------------------
void DrawZoom()
{
__asm
	ld hl, (_ZoomPtr)
	push hl
	pop ix
	
	ld hl, LENSCIRCLEINBIN_PTR
	push hl
	pop iy
	
	ld hl, (_BackLines)
	ld bc, (_ZoomPosY)
	add hl, bc
	add hl, bc
	
	ld b, 0
	ld a, (_ZoomPosX)
	ld c, a
	
	ex af, af'
	ld a, 2
	ex af, af'

	ld a, ZOOM_HEIGHT * 2
DrawZoomLoop:
	push af
	
	ld a, (hl)
	inc hl
	push hl
	ld h,(hl)
	ld l, a
	
	push bc
	
	add hl, bc
	
	ld a, (iy)
	inc iy
	ld c, a
	add hl, bc
	
	ex de, hl
	
	push ix
	pop hl
	
	add hl, bc
	
	ld a, ( iy )
	inc iy
	ld (drawZoom_UnrolledJump+1), a
	
drawZoom_UnrolledJump:
	jr drawZoom_Unrolled
	
drawZoom_Unrolled:
REPT 64
	ldi
ENDM
	
	ex af, af'
	dec a
	jr nz, skipNextZoomSpriteLine	
	ld a, 2
	push ix
	pop hl
	ld bc, ZOOM_WIDTH
	add hl, bc
	push hl
	pop ix	
skipNextZoomSpriteLine:
	ex af, af'
	
	pop bc
	pop hl
	inc hl
	pop af
	dec a
	jp nz, DrawZoomLoop
	
__endasm;
}

// ----------------------------------------------------------------------------
void DrawOpcodeSprite()
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
	
	ld hl, (_SpritePtr)
	ld (drawOpcodeSprite+1), hl
	
	ld hl, (_BackLinesSprite)	
	ld bc, (_PosY)
	add hl, bc
	add hl, bc
	
	ld a, (_PosX)
	ld b, 0
	ld c, a
	
	ld de, &55AA
		
drawOpcodeSprite:
	call 0
	
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
void DrawCircle()
{	
	if ( BackLines == BANK0LINES_TOPPTR-256 )
	{
		BackLinesSprite = BANK0LINES_TOPPTR-256;
	}
	else
	{
		BackLinesSprite = LENS_BANK2LINES2_TOPCLIPPTR;
	}
	
	if ( (CirclePosX & 1) == 0 )
	{
		SpritePtr = LENSCIRCLEBMPSPRZ802_PTR + 0x8000;
	}
	else
	{
		SpritePtr = LENSCIRCLEBMPSPRZ802_PTR;
	}
	
	CirclePosX >>= 1;
	
	PosX = CirclePosX;
	PosY = CirclePosY;	
	DrawOpcodeSprite();
}

// ----------------------------------------------------------------------------
void DoRestoreHorizontalTop()
{
__asm
	; X+ pad to bank C1 (using &C000)
	ld h, 0
	ld a, (_PosX)
	ld l, a
	ld ( RestoreHorizontalTopPosX + 1 ), hl
	ld bc, (_BankPadding)
	add hl, bc
	ld ( RestoreHorizontalTopPosXPadding + 1 ), hl
	
	ld hl, (_BackLines)
	ld bc, (_PosY)
	add hl, bc
	add hl, bc
	push hl	
	exx
	pop hl
	exx	
	
	ld bc, (_RestoreHeight)
		
doRestoreHorizontalTop_Loop:
	push bc
	
	ld e, (hl)
	inc hl
	ld d, (hl)
	inc hl
	
	push hl
	
	exx
	ld e, (hl)	
	inc hl
	ld d, (hl)
	inc hl
	push de
	exx
	pop hl	

RestoreHorizontalTopPosX:
	ld bc, 0
	add hl, bc
	ex de, hl
RestoreHorizontalTopPosXPadding:
	ld bc, 0
	add hl, bc
	
REPT 33
	ldi
ENDM

	pop hl

	pop bc
	dec bc
	ld a, b
	or c
	jp nz, doRestoreHorizontalTop_Loop
	
__endasm;
}

// ----------------------------------------------------------------------------
void DoRestoreHorizontalBottom()
{
__asm
	; X+ pad to bank C1 (using &C000)
	ld h, 0
	ld a, (_PosX)
	ld l, a
	ld ( RestoreHorizontalBottomPosX + 1 ), hl
	ld bc, (_BankPadding)
	add hl, bc
	ld ( RestoreHorizontalBottomPosXPadding + 1 ), hl
	
	ld bc, (_PosY)
	ld hl, (_BackLines)
	add hl, bc
	add hl, bc
	push hl
	exx
	pop hl
	exx
	
	ld bc, (_RestoreHeight)
		
doRestoreHorizontalBottom_Loop:
	push bc
	
	ld e, (hl)
	inc hl
	ld d, (hl)
	inc hl
	
	push hl
	
	exx
	ld e, (hl)	
	inc hl
	ld d, (hl)
	inc hl
	push de
	exx
	pop hl	

RestoreHorizontalBottomPosX:
	ld bc, 0
	add hl, bc
	ex de, hl
RestoreHorizontalBottomPosXPadding:
	ld bc, 0
	add hl, bc
	
REPT 33
	ldi
ENDM

	pop hl

	pop bc
	dec bc
	ld a, b
	or c
	jp nz, doRestoreHorizontalBottom_Loop
__endasm;
}

// ----------------------------------------------------------------------------
void DoRestoreCircle()
{
	if ( BackLines == BANK0LINES_TOPPTR-256 )
	{
		BankPadding = 0x4000;
	}
	else
	{
		BankPadding = -0x4000;
	}
	
	if ( PosY < 128 )
	{	
		RestoreHeight = 100;
		PosY = 128;
__asm
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC6
		out (c), c
		ld a, &C6
		ld ( &0000 ), a
		ei
__endasm;

	DoRestoreHorizontalTop();

__asm
		di
		pop af
		ld b, &7F
		ld c, a
		out (c), c
		ld ( &0000 ), a
		ei		
__endasm;

		PosY = 128+100;		
		RestoreHeight = 128-100;
		
		BankPadding = 0x8000;
		DoRestoreHorizontalBottom();
	}
	else if ( PosY > 127+100 )
	{
		RestoreHeight = 128;
		
		BankPadding = 0x8000;
		DoRestoreHorizontalBottom();
	}
	else
	{	
		RestoreHeight = 128+100-PosY;
		
__asm
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC6
		out (c), c
		ld a, &C6
		ld ( &0000 ), a
		ei
__endasm;

		DoRestoreHorizontalTop();

__asm
		di
		pop af
		ld b, &7F
		ld c, a
		out (c), c
		ld ( &0000 ), a
		ei		
__endasm;

		PosY += RestoreHeight;
		
		RestoreHeight = 128-RestoreHeight;
		
		BankPadding = 0x8000;
		DoRestoreHorizontalBottom();
	}
}

// ----------------------------------------------------------------------------
void RestoreCircle()
{	
	if ( (frame & 1) == 0)
	{		
		if ( SaveCirclePosX1 != 255 )
		{
			PosX = SaveCirclePosX1;
			PosY = SaveCirclePosY1;
			
			DoRestoreCircle();
		}
	}
	else
	{
		if ( SaveCirclePosX2 != 255 )
		{
			PosX = SaveCirclePosX2;
			PosY = SaveCirclePosY2;
			
			DoRestoreCircle();
		}
	}
}

// ----------------------------------------------------------------------------
void Init()
{	
	//memset( CODESEGMENT0000, 0, CODESEGMENT0000_SIZE );

	SaveCirclePosX1 = 255;
	SaveCirclePosX2 = 255;
	FrameCirclePosYSineCurvePtr = LENSSINECURVEBOUNCEBIN_PTR;	
	ZoomPtr = CODESEGMENT0000;
	PrecaZoomPixels = 0x2F00;
	InitPrecaZoomPixels();
	
	MovePtr = MOVECIRCLE_XYBIN_PTR;
	
	// 3100 to 3500 is Lines
	
	// Memory from 3500 to 4000 is empty!
	LoadFile( LENSWRITERBIN_TRACKFILEID, LENSWRITER_CODE );	
	
__asm
	call LENSWRITER_CODE
__endasm;
}

// ----------------------------------------------------------------------------
void DrawFrame()
{
	BackLines = GetBackLines()-128;
	
	PushBankBottom();
	
__asm
	call LENSWRITER_CODE+3
__endasm;
	
	if ( MovePtr == MOVECIRCLE_XYBIN_ENDPTR )
	{
		MovePtr = MOVECIRCLE_XYBIN_PTR;
	}
__asm
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC4
		out (c), c
__endasm;
	
	FrameCirclePosX = *MovePtr;
	MovePtr++;	
	FrameCirclePosY = *MovePtr;
	MovePtr++;

__asm
		pop af
		ld b, &7F
		ld c, a
		out (c), c
		ei		
__endasm;

	CirclePosX = FrameCirclePosX;
	CirclePosY = FrameCirclePosY;
	
	ZoomPosX = CirclePosX;
	ZoomPosY = 9+CirclePosY;	
	
	//if ( FrameCount > 2 )
	{
		CreateZoom();
		RestoreCircle();		
		DrawZoom();
	}
	
	if ( (frame & 1) == 0)
	{	
		SaveCirclePosX1 = CirclePosX >> 1;
		SaveCirclePosY1 = CirclePosY;	
	}
	else
	{		
		SaveCirclePosX2 = CirclePosX >> 1;
		SaveCirclePosY2 = CirclePosY;	
	}	
			
	//if ( FrameCount > 2 )
	{
		DrawCircle();
	}
	
	PopBank();
}

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	for ( FrameCount = 0; FrameCount < 186; FrameCount++ )
	{	
		DrawFrame();
		
		FlipScreen();
		// WaitVBL();
		frame++;
	}
	
	// Load Cloud
	LoadFile( LENSTITLECLOUDBMPBINPCK_TRACKFILEID_2, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, CODESEGMENT0000, VIDEOSEGMENT0000 );
	
	LoadFile( LENSTILE2BMPSPRRAWDATA1PCK_TRACKFILEID_2, VIDEOSEGMENT0000 );
	BankUnpack( BANKC1, VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();	
	//bank_memcpy( 0xC6, CODESEGMENT0000, CODESEGMENT4000, CODESEGMENT4000_SIZE );
	DrawCloudReverse_VIDEOSEGMENT8000();
	
	// Load Tile (unshifted)
	bank_memcpy( 0xC0, VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT0000_SIZE );
	bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENT4000_SIZE );	
}