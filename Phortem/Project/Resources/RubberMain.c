
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

#include "RubberData1.h"
#include "RubberData2.h"
#include "RubberWriter.h"
#include "RubberWriter2.h"
#include "RubberWriter3.h"
#include "Rubber.cpcbitmap.info.h"

// ----------------------------------------------------------------------------
#define RUBBERCODE				0x3400

// ----------------------------------------------------------------------------
char **BackLines;
unsigned short FrameCount;
unsigned char RubberPosX;
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
unsigned char WriterSpriteBank;
unsigned char IsFlipped;

// ----------------------------------------------------------------------------
void Init()
{
	LoadFile( RUBBERDATA3BIGFILEPCK_TRACKFILEID, CODESEGMENT0000 );
	BankUnpack( 0xC0, CODESEGMENTC000, CODESEGMENT0000 );
	
	LoadFile( RUBBERDATA1BIGFILE_TRACKFILEID, CODESEGMENT0000 );
	
	memcpy( VIDEOSEGMENT0000, VIDEOSEGMENT8000, VIDEOSEGMENT8000_SIZE );
	
	LoadFile( RUBBERWRITERBIN_TRACKFILEID, RUBBERCODE );
	
	LoadFile( RUBBERWRITER3BIGFILE_TRACKFILEID, RUBBERWRITER3H_DATAPTR );	
	
	RubberPosX = 88+4;
__asm
TempStorage equ &3100

	call RUBBERCODE
	
	xor a
	ld ( &3F ), a
__endasm;
	
	IsWriter = 1;
}

// ----------------------------------------------------------------------------
// 55 is ok!
#define WRITER_PIXEL_STEP 45 // must be /5
#define WRITER_PIXEL_STEP_BYTES 8*45

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
	ld a, (_WriterSpriteBank)
	ld c, a
	ld b, &7F
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
	
;TempStorage:
;	ds WriterPixelCount*8
	
EndWriter:
__endasm;
}


// ----------------------------------------------------------------------------
unsigned char asmSpriteIndex;
void InitWriterSprite(unsigned char spriteIndex)
{
	asmSpriteIndex = spriteIndex;
	
__asm
	ld hl, WriterSpriteTable
	ld a, ( _asmSpriteIndex )
	ld b, 0
	ld c, a
	add hl, bc
	add hl, bc
	add hl, bc
	add hl, bc
	ld e, (hl)
	inc hl
	ld d, (hl)
	inc hl
	ld ( _WriterSpritePtr ), de	
	ld e, (hl)
	inc hl
	ld d, (hl)
	ld ( _WriterSpriteSize ), de
	jr InitWriteSpriteEnd

WriterSpriteTable:
	dw SPR01_THISBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR01_THISBMPCLOUDSPRITESPRRAWDATA1_SIZE
	dw SPR02_ISBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR02_ISBMPCLOUDSPRITESPRRAWDATA1_SIZE
	dw SPR03_HOWBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR03_HOWBMPCLOUDSPRITESPRRAWDATA1_SIZE
	dw SPR04_WEBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR04_WEBMPCLOUDSPRITESPRRAWDATA1_SIZE
	dw SPR05_TWISTBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR05_TWISTBMPCLOUDSPRITESPRRAWDATA1_SIZE
	dw SPR06_THEBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR06_THEBMPCLOUDSPRITESPRRAWDATA1_SIZE
	dw SPR07_DRAGONSBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR07_DRAGONSBMPCLOUDSPRITESPRRAWDATA1_SIZE
	dw SPR08_TALEBMPCLOUDSPRITESPRRAWDATA1_PTR, SPR08_TALEBMPCLOUDSPRITESPRRAWDATA1_SIZE
	
InitWriteSpriteEnd:
__endasm;
}

// ----------------------------------------------------------------------------
void DrawWriter()
{
	if ( IsQuit == 1 )
	{
		return;
	}
	
	if ( FrameWriterCount == 0 )
	{
		FrameCloudWriterEnded = 0;
		
		WriterSpriteBank = 0xC4;				
				
		if ( CurrentWord == 0 )
		{
			if ( SecondWriter == 0 )
			{
				InitWriterSprite(0);
			}
			else
			{
				WriterSpriteBank = 0xC6;
			
				InitWriterSprite(4);				
				
				if ( IsClear == 1 )
				{	
					IsQuit = 1;
					return;
				}
			}
		}
		else if ( CurrentWord == 1 )
		{
			if ( SecondWriter == 0 )
			{
				WriterSpriteBank = 0xC1;
				
				InitWriterSprite(1);
			}
			else
			{
				InitWriterSprite(5);
			}
		}
		else if ( CurrentWord == 2 )
		{
			if ( SecondWriter == 0 )
			{
				InitWriterSprite(2);
			}
			else
			{
				InitWriterSprite(6);
			}
		}
		else if ( CurrentWord == 3 )
		{
			if ( SecondWriter == 0 )
			{				
				InitWriterSprite(3);
				
				if ( IsClear == 1 )		
				{
					StartDec = 58;
					IsClear = 2;					
				}
			}
			else
			{
				InitWriterSprite(7);
			}
			
			WaitWriterDisplayed = 8;
		}
		else if ( CurrentWord == 4 )
		{		
			if ( IsClear == 2 )
			{
				return;
			}
			
			if ( IsClear == 1 )			
			{
				if ( SecondWriter == 0 )
				{
					return; // ended..
				}				
			}
			
			IsClear = 1;
			CurrentWord = 0;
			FrameWriterCount = 0;
			
			return;
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
			CurrentWord++;
			FrameWriterCount = -1;
			
			/*if ( (IsClear==1) && (WriterPosX == SPR08_TALEBMP_MERGE_POSX) )
			{
				IsQuit = 1;
			}	*/
		}
	}
	
	FrameWriterCount++;
}

// ----------------------------------------------------------------------------
void DrawFrame()
{	
	if ( IsQuit == 1 )
	{
		return;
	}
	
	BackLines = GetBackLines();
	PushBankBottom();
	
	DrawWriter();
	
	IsFlipped = IsScreenFlipped();
	
__asm
	;ld a, (_StartDec)
	;ld b, a
	;ld a, (_RubberPosX)
	
	ld hl, (_BackLines)	
	ld a, (_RubberPosX)
	ld b, 0
	ld c, a	
	ld a, (_IsFlipped)
	call RUBBERCODE+3
__endasm;
	
	PopBank();
	
	if ( StartDec != 0 )
	{
		StartDec--;
		
		if ( StartDec == 11 )
		{			
			// Start Writer 2..

			SecondWriter = 1;
			
			IsClear = 0;
			CurrentWord = 0;
			FrameWriterCount = 0;
		}	
		
		if ( StartDec >= 58-14 )
		{
			RubberPosX--;
		}
		else if ( StartDec <= 14 )
		{
			RubberPosX--;
		}
		else
		{
			RubberPosX-=2;
		}
	}
}

// ----------------------------------------------------------------------------
void Main()
{
	Init();
	
	for( FrameCount=0; FrameCount<294; FrameCount++)
	{	
		DrawFrame();
		FlipScreen();
		
		if ( FrameCount == 164 ) // 210 )
		{
__asm
	ld a, 1
	ld ( &3F ), a
__endasm;
		}
	};
	
	// Load Cloud
	LoadFile( RUBBERTITLECLOUD2BMPBINPCK_TRACKFILEID, VIDEOSEGMENT0000 );
	BankUnpack( 0xC4, CODESEGMENT0000, VIDEOSEGMENT0000 );
	
	LoadFile( RUBBERTILE2BMPSPRRAWDATA1PCK_TRACKFILEID_2, VIDEOSEGMENT0000 );
	PushBank( BANKC1 );
	Unpack( VIDEOSEGMENTC000, VIDEOSEGMENT0000 );	
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	DrawCloud_VIDEOSEGMENT8000();
}