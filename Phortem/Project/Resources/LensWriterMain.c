
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "Lens.cpcbitmap.info.h"
#include "LensWriter.h"

// ----------------------------------------------------------------------------
#define LENS_BANK2LINES2_TOPCLIPPTR		0x3100
#define LENS_BANK2LINES2_TOPPTR			LENS_BANK2LINES2_TOPCLIPPTR + 0x100
#define LENS_BANK3LINES2_BOTTOMPTR		LENS_BANK2LINES2_TOPPTR + 100+100
#define LENS_BANK3LINES2_BOTTOMCLIPPTR	LENS_BANK3LINES2_BOTTOMPTR + 105+105

// ----------------------------------------------------------------------------
char **BackLines;
unsigned char IsWriter;
unsigned char IsQuit;
unsigned char FrameWriterCount;
unsigned char *WriterSpritePtr;
unsigned char *WriterSpriteEndPtr;
unsigned short WriterSpriteSize;
unsigned char FrameCloudWriterEnded;
unsigned char CurrentWord;
unsigned char IsClear;
unsigned char SecondWriter;
unsigned char StartDec;
unsigned char WaitWriterDisplayed;
unsigned short currentHeight;
unsigned char WaitFrame;

// ----------------------------------------------------------------------------
#define ZOOM_WIDTH 			32 // 24
#define ZOOM_HEIGHT			55 // 42

// ----------------------------------------------------------------------------
void CreateZoomLeft()
{
__asm
	ld d, a
	ld e, DUMMYLINEPTR/256
CreateZoomLeftLoop:
	ld a, (hl)
	inc hl
	push hl	
	ld h,(hl)
	ld l, a
	
	ld a, h
	cp e
	jr z, CreateZoomLeftLoopClipping
	
	add hl, bc	
	
CreateZoomLeftLoopClipping:	
REPT ZOOM_WIDTH/2
	ld a, (hl)
	inc hl
	
	exx	
	ld l, a
	
	ld a,(hl)
	ld (de),a
	inc de
	
	inc h
	
	ld a,(hl)
	ld (de),a
	inc de
	
	dec h
	exx
ENDM	
	pop hl
	inc hl
	dec d
	jp nz, CreateZoomLeftLoop
__endasm;
}

// ----------------------------------------------------------------------------
void CreateZoomRight()
{
__asm
CreateZoomRightLoop:
	ex af, af'
	
	ld a, (hl)
	inc hl
	push hl
	ld h,(hl)
	ld l, a
	
	ld a, h
	cp DUMMYLINEPTR/256
	jr z, CreateZoomRightLoopClipping
	
	add hl, bc	
	
CreateZoomRightLoopClipping:	
	push bc
	
	ld a, (hl)
	inc hl
	
REPT ZOOM_WIDTH/2	
	ld (de), a
	inc de
	
	and &55 ; left=0  right=15
	ld b, a
	rl b
	
	ld a, (hl)
	inc hl
	ld c, a
	
	and &AA ; left=15  right=0
	
	rr a
	or b
	ld (de), a
	inc de
	
	ld a, c
ENDM
	
	pop bc	
	pop hl
	inc hl
	
	ex af, af'
	dec a
	jp nz, CreateZoomRightLoop
__endasm;
}

// ----------------------------------------------------------------------------
void CreateClipping()
{
__asm
	ld de, DUMMYLINEPTR
	ld a, 128
CreateClipping_Loop:
	ld ( hl ), e
	inc l
	ld ( hl ), d
	inc hl
	dec a
	jr nz, CreateClipping_Loop
__endasm;
}

// ----------------------------------------------------------------------------
void CreateTop()
{
__asm
	ld a, 20
topCharLoop:
	push af
	
	push hl
	
	ld bc, &800
	ld a, 5
topLineLoop:
	push af
	
	ld a, l
	ld ( de ), a
	inc e
	ld a, h
	ld ( de ), a
	inc de
	
	add hl, bc
	
	pop af
	dec a
	jr nz, topLineLoop
	
	pop hl
	
	ld bc, 96
	add hl, bc
	
	pop af
	dec a
	jr nz, topCharLoop
__endasm;
}

// ----------------------------------------------------------------------------
void CreateBottom()
{
__asm
	ld a, 21
bottomCharLoop:
	push af
	
	push hl
	
	ld bc, &800
	ld a, 5
bottomLineLoop:
	push af
	
	ld a, l
	ld ( de ), a
	inc e
	ld a, h
	ld ( de ), a
	inc de
	
	add hl, bc
	
	pop af
	dec a
	jr nz, bottomLineLoop
	
	pop hl
	
	ld bc, 96
	add hl, bc
	
	pop af
	dec a
	jr nz, bottomCharLoop
__endasm;
}

// ----------------------------------------------------------------------------
#define WRITER_PIXEL_STEP 40 // must be /5
#define WRITER_PIXEL_STEP_BYTES 8*40

// ----------------------------------------------------------------------------
void DrawCloudSprite()
{	
	if ( (WriterSpritePtr + WRITER_PIXEL_STEP_BYTES) >= WriterSpriteEndPtr )
	{
		WriterSpritePtr = WriterSpriteEndPtr - WRITER_PIXEL_STEP_BYTES;
		FrameCloudWriterEnded = 1;
	}
	
	DrawWriterAsm();
	
	WriterSpritePtr += WRITER_PIXEL_STEP_BYTES;
}

// ----------------------------------------------------------------------------
void DrawWriterAsm()
{	
__asm
WriterPixelCount equ WRITER_PIXEL_STEP

	di
	ld a, ( &0000 )
	push af
	ld bc, &7FC4
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei

	ld hl, (_WriterSpritePtr)	
	ld de, TempStorage

	ld a, WriterPixelCount/5
copyFromBank:
REPT 8*5
	ldi
ENDM
	dec a
	jr nz, copyFromBank

	di
	ld bc, &7FC0
	out (c), c
	
	ld (oldSP+1), sp
	ld sp, TempStorage
	
	ld a, (_IsClear)
	or a
	jp nz, ClearEndInnerLoop
	jr DrawEndInnerLoop
	
EndInnerLoop:
	
oldSP:
	ld sp, 0
	
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei	
	
	jp EndWriter
	
DrawEndInnerLoop:
REPT WriterPixelCount*1
	pop bc
	pop de
	
	pop hl		
	ld (hl), c
	set 7, h
	ld (hl), c
	
	pop hl	
	ld (hl), b
	set 7, h
	ld (hl), b
ENDM
	jp EndInnerLoop

ClearEndInnerLoop:
REPT WriterPixelCount*1
	pop de
	pop bc
	
	pop hl
	ld (hl), c
	set 7, h
	ld (hl), c
	
	pop hl	
	ld (hl), b
	set 7, h
	ld (hl), b
ENDM
	jp EndInnerLoop
	
TempStorage:
	ds WriterPixelCount*8
	
EndWriter:
__endasm;
}

// ----------------------------------------------------------------------------
void InitWriter()
{
	WaitFrame = 10;
	
__asm
	ld hl, LENS_BANK2LINES2_TOPCLIPPTR
	call _CreateClipping
	ld hl, VIDEOSEGMENT8000
	ld de, LENS_BANK2LINES2_TOPPTR
	call _CreateTop
	ld hl, VIDEOSEGMENTC000
	ld de, LENS_BANK3LINES2_BOTTOMPTR
	call _CreateBottom
	ld hl, LENS_BANK3LINES2_BOTTOMCLIPPTR
	call _CreateClipping
__endasm;
}

// ----------------------------------------------------------------------------
void DrawWriter()
{
	if ( WaitFrame != 0 )
	{
		WaitFrame--;
		return;
	}
	
	if ( IsQuit == 1 )
	{
		return;
	}
	
	if ( FrameWriterCount == 0 )
	{
		FrameCloudWriterEnded = 0;
				
		if ( CurrentWord == 0 )
		{
			WriterSpritePtr = LENS_SPR01_PRINCEBMPCLOUDSPRITESPRRAWDATA1_PTR;
			WriterSpriteSize = LENS_SPR01_PRINCEBMPCLOUDSPRITESPRRAWDATA1_SIZE;
		}
		else if ( CurrentWord == 1 )
		{
			WriterSpritePtr = LENS_SPR02_OFBMPCLOUDSPRITESPRRAWDATA1_PTR;
			WriterSpriteSize = LENS_SPR02_OFBMPCLOUDSPRITESPRRAWDATA1_SIZE;
		}
		else if ( CurrentWord == 2 )
		{
			WriterSpritePtr = LENS_SPR03_DARKNESSBMPCLOUDSPRITESPRRAWDATA1_PTR;
			WriterSpriteSize = LENS_SPR03_DARKNESSBMPCLOUDSPRITESPRRAWDATA1_SIZE;
		}
		
		WriterSpriteEndPtr = WriterSpritePtr+WriterSpriteSize;			
	}
	else if ( FrameWriterCount == 1 )
	{	
	}
	else if ( FrameCloudWriterEnded == 0 )	
	{
		DrawCloudSprite();
	}
	else
	{
		if ( WaitWriterDisplayed != 0 )
		{	
			WaitWriterDisplayed--;
			FrameWriterCount--;
		}
		else
		{	
			if ( IsClear == 0 )
			{
				IsClear = 1;
				FrameWriterCount = -1;	
				WaitFrame = 10;
			}
			else
			{	
				WaitFrame = 2;
				IsClear = 0;				
				
				if (CurrentWord == 2)
				{
					IsQuit = 1;
					WaitFrame = 16;
				}				
				
				CurrentWord++;
				FrameWriterCount = -1;
			}
		}
	}
	
	FrameWriterCount++;
}