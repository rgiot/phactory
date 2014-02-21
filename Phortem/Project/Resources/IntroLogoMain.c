
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "memcpy.cxx"
#include "memset.cxx"
#include "BankMemCopy.cxx"
#include "BankUnpack.cxx"

// ----------------------------------------------------------------------------
#define WRITER_PIXEL_STEP 40 // must be /5
#define WRITER_PIXEL_STEP_BYTES 8*40

unsigned char *WriterSpritePtr;
unsigned char *WriterSpriteEndPtr;
unsigned short WriterSpriteSize;
unsigned char FrameCloudWriterEnded;
unsigned char IsClear;

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
	ld bc, &7FC0
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
	;ld (hl), c
	set 7, h
	ld (hl), c
	
	pop hl	
	;ld (hl), b
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
void DrawIntroCondense()
{	
__asm
	ld hl, IntroLogo_Data
	ld ( _WriterSpritePtr ), hl
	ld hl, IntroLogo_DataEnd-IntroLogo_Data
	ld ( _WriterSpriteSize ), hl
__endasm;
	WriterSpriteEndPtr = WriterSpritePtr+WriterSpriteSize;		
	
	while (FrameCloudWriterEnded==0)
	{
		WaitVBL();
		DrawCloudSprite();
	}
}

// ----------------------------------------------------------------------------
void Data()
{
__asm
IntroLogo_Data:	
	incbin "IntroCondense2013.bmp.cloudSprite.sprRawData1"
IntroLogo_DataEnd:
__endasm;
}
