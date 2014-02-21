
// ----------------------------------------------------------------------------
#include "Config.h"
#include "JumpTable.cxx"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "BankUnpack.cxx"

#include "CostixData.h"
#include "CostixData2.h"
#include "CostixTitle1.h"
#include "CostixTitle2.h"
#include "CostixTitle3.h"
 
// ----------------------------------------------------------------------------
void InitFirst();
void DrawBGEdges(unsigned char *src, unsigned char *topPtr, unsigned char *bottomPtr);
void bank_memcpy( unsigned char destBank, char *destination, char *source, unsigned int length );

#define MASKTYPE_NOMASK 0
#define MASKTYPE_MASK 1
void DrawTiles(char maskType, char *topPtr, char *bottomPtr, char *tilePtr);

// ----------------------------------------------------------------------------
char **BackLines;
unsigned char frameCount;

// ----------------------------------------------------------------------------
unsigned char UnpackBank;
unsigned short UnpackSrcPtr;
void UnpackNextFrame()
{
__asm
UnpackId:
		ld a, 1 ; not starting from 0, already 1 frame computed in init
		inc a
		cp 32
		jr nz, UnpackIdNoReset
		xor a
UnpackIdNoReset:		
		ld ( UnpackId + 1 ), a
		
		ld hl, UnpackSrcTable
		ld b, 0
		ld c, a
		add hl, bc
		add hl, bc
		add hl, bc
		add hl, bc
		
		ld a, (hl)
		ld ( _UnpackBank ), a
		inc hl
		ld a, (hl)
		inc hl
		ld h, (hl)
		ld l, a
		ld ( _UnpackSrcPtr ), hl
		
		jr UnpackIdEnd
		
DoFX:
		di
		ld a, (&0000)
		push af
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		ld a, e
		call &BC03		
		ld a, (&0081)
		ld ( &0080), a
		di
		pop af
		ld ( &0000 ), a
		ld b, &7F
		ld c, a
		out (c), c
		ei
		ret
		
UnpackIdEnd:
		
__endasm;

	BankUnpack( UnpackBank, 0x2800, UnpackSrcPtr );	
}

// ----------------------------------------------------------------------------
void DoFX1()
{
__asm
	ld e, 1
	call DoFX
__endasm;
	BackLines = GetLowLines();
	PushBank( BANKC1 );		
	DrawBGEdges( 0xC000+COSTIXDATA2H_DATASIZE, VIDEOSEGMENT0000, VIDEOSEGMENT4000 );
	PopBank();
}

// ----------------------------------------------------------------------------
void Main()
{	
	InitFirst();
	
	Unpack( 0xBC00, COSTIXFXCODE_BC00Z80BINPCK_PTR );
__asm
	ld hl, CostixFXTile + 5 + 5 + 5
	call &BC00
__endasm;
	
	// Unpack first data frame
	BankUnpack( 0xC6, 0x2800, 0x4000 );
	
	DoFX1();	
	
	bank_memcpy( 0xC4, 0x4080, VIDEOSEGMENT0000, VIDEOSEGMENT0000_SIZE); // top
	
	LoadFile( COSTIXTITLE3BIGFILE_TRACKFILEID, COSTIXTITLE3H_DATAPTR );
	BankUnpack( 0xC4, 0x6880, TRANSITLINESFX3BINPCK_PTR );
	Unpack( CODESEGMENT0000, MAINDRAWTRIANGLEONLYZ80BINPCK_PTR );
	
	bank_memcpy( 0xC7, VIDEOSEGMENT4000, VIDEOSEGMENTC000, VIDEOSEGMENTC000_SIZE ); // bottom2
	bank_memcpy( 0xC0, 0x7000, VIDEOSEGMENT4000, 0x1000 ); // bottom1 part 1
	bank_memcpy( 0xC0, 0xE800, 0x5000, 0x1800 ); // bottom1 part 2
	
	BankUnpack( BANKC1, VIDEOSEGMENT4000, 0x3B00 );
	
	LoadFile( COSTIXDATA2BIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
__asm
	call _IsScreenFlipped
	ld a, l 
	call &2800
__endasm;

	bank_memcpy( 0xC1, 0xC000, VIDEOSEGMENT0000, COSTIXDATA2H_DATASIZE );
	bank_memcpy( 0xC1, VIDEOSEGMENT0000, VIDEOSEGMENT8000, COSTIXDATA2H_DATASIZE );
	bank_memcpy( 0xC1, 0xC000 + COSTIXDATA2H_DATASIZE, VIDEOSEGMENT4000, 0x600 );
	bank_memcpy( 0xC0, VIDEOSEGMENT4000, VIDEOSEGMENTC000, 0x600 );
	
	for ( frameCount = 0; frameCount < 31 /* 32 */; frameCount++)
	{	
		UnpackNextFrame();
		DoFX1();
		FlipScreen();
		
		UnpackNextFrame();
__asm
		ld e, 0
		call DoFX
__endasm;		
		BackLines = GetHighLines();
		PushBank( BANKC3 );		
		DrawBGEdges( 0xC000+COSTIXDATA2H_DATASIZE, VIDEOSEGMENT8000, VIDEOSEGMENT4000 );
		PopBank();
		FlipScreen();
	}
	
	// Load Tile (8-15 pixel index range)
	BankUnpack( 0xC1, VIDEOSEGMENTC000, 0xF740 );
	
	// Draw Tiles
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000);
	PopBank();
	
__asm
	call &6800
	
	ld hl, 89
FX2Loop:
	push hl
	call &6803
	pop hl
	dec hl
	ld a, h
	or l
	jr nz, FX2Loop
__endasm;
}

// ----------------------------------------------------------------------------
void DummyTable()
{
__asm
UnpackSrcTable:
		db &C6
		dw COSTIXFRAME00BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME01BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME02BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME03BINPCK_PTR
		db 0		
		
		db &C6
		dw COSTIXFRAME04BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME05BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME06BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME07BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME08BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME09BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME10BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME11BINPCK_PTR
		db 0		
		
		db &C6
		dw COSTIXFRAME12BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME13BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME14BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME15BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME16BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME17BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME18BINPCK_PTR
		db 0
				
		db &C6
		dw COSTIXFRAME19BINPCK_PTR
		db 0		
		
		db &C6
		dw COSTIXFRAME20BINPCK_PTR
		db 0
		
		db &C6
		dw COSTIXFRAME21BINPCK_PTR
		db 0
				
		db &C7
		dw COSTIXFRAME22BINPCK_PTR
		db 0
				
		db &C7
		dw COSTIXFRAME23BINPCK_PTR
		db 0
				
		db &C7
		dw COSTIXFRAME24BINPCK_PTR
		db 0
				
		db &C7
		dw COSTIXFRAME25BINPCK_PTR
		db 0
				
		db &C7
		dw COSTIXFRAME26BINPCK_PTR
		db 0
				
		db &C7
		dw COSTIXFRAME27BINPCK_PTR
		db 0		
		
		db &C7
		dw COSTIXFRAME28BINPCK_PTR
		db 0
		
		db &C7
		dw COSTIXFRAME29BINPCK_PTR
		db 0
		
		db &C7
		dw COSTIXFRAME30BINPCK_PTR
		db 0
		
		db &C7
		dw COSTIXFRAME31BINPCK_PTR
		db 0
__endasm;
}

// ----------------------------------------------------------------------------
#include "DrawEdges.cxx"

// ----------------------------------------------------------------------------
char *GetPalette()
{
__asm
	ld hl, CostixPalette
	jp QuitGetPalette
CostixPalette:
	incbin "Costix.cpcbitmap.palette"
	
CostixFXTile:
	incbin "CostixFXTiles.bmp.sprRawData1"
	
QuitGetPalette:
__endasm;
}

char i;
void ShiftTile(char *tileSource);
void UnshiftTile(char *tileSource);

// ----------------------------------------------------------------------------
#include "ShiftTile.cxx"
#include "BankMemCopy.cxx"
#include "DrawCloud.cxx"
#include "DrawTiles.cxx"

// ----------------------------------------------------------------------------
void InitFirst()
{	
	LoadFile( COSTIXTITLE1BIGFILE_TRACKFILEID, COSTIXTITLE1H_DATAPTR );

	// Title
	bank_memcpy( 0xC1, CODESEGMENT4000, COSTIXTITLEBMPTOPBINPCK_PTR, COSTIXTITLEBMPTOPBINPCK_SIZE );
	bank_memcpy( 0xC3, CODESEGMENT4000, COSTIXTITLEBMPBOTTOMBINPCK_PTR, COSTIXTITLEBMPBOTTOMBINPCK_SIZE );
	
	// CubeTransit FX
	bank_memcpy( 0xC0, 0xFD00, CUBETRANSITFXCODE_2800Z80BINPCK_PTR, CUBETRANSITFXCODE_2800Z80BINPCK_SIZE );
	
	// Tile (8-15 pixel index range)
	bank_memcpy( 0xC1, 0xF740, COSTIXTILE2BMPSPRRAWDATA1PCK_PTR, COSTIXTILE2BMPSPRRAWDATA1PCK_SIZE );	
	BankUnpack( 0xC1, VIDEOSEGMENTC000, COSTIXTILE2BMPSPRRAWDATA1PCK_PTR );	
	ShiftTile( VIDEOSEGMENTC000 );
	SetPaletteFrom8( GetPalette() );
	
	// Cloud
	BankUnpack( 0xC4, CODESEGMENT0000, COSTIXTITLECLOUDBMPBINPCK_PTR );
	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	// CubeTransit FX
	BankUnpack( 0xC0, CODESEGMENT0000, 0xFD00 );
__asm
		call CODESEGMENT0000
__endasm;

	for ( i = 0; i < 42; i++ )
	{
__asm	
		call _WaitVBL
		
		call CODESEGMENT0000 + 3
		
		ld hl, &1000
		ld de, &1000
		ld bc, &2C00
		ldir
__endasm;
	}
	
	// Apply Part palette
	UnshiftTile( VIDEOSEGMENTC000 );	
	PushBank( BANKC1 );
	DrawTiles(MASKTYPE_NOMASK, VIDEOSEGMENT0000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	WaitVBL();	
	SetPalette( GetPalette() );	
	FlipScreen();
		
	// Load title
	BankUnpack( 0xC0, VIDEOSEGMENT8000, CODESEGMENT4000 );	
	BankUnpack( 0xC0, VIDEOSEGMENTC000, CODESEGMENTC000 );	
	PushBank( BANKC3 );
	DrawTiles(MASKTYPE_MASK, VIDEOSEGMENT8000, VIDEOSEGMENT4000, VIDEOSEGMENTC000 );
	PopBank();
	
	bank_memcpy( 0xC6, CODESEGMENT4000, CODESEGMENT0000, CODESEGMENT0000_SIZE );

	DrawCloudReverse_VIDEOSEGMENT0000();
	FlipScreen();
	
__asm
	ld a, 38
waitTitle2:
	ld hl, &1000
	ld de, &1000
	ld bc, &1000
	ldir
	dec a
	jr nz, waitTitle2
__endasm;
	
	LoadFile( COSTIXDATABIGFILE_TRACKFILEID, VIDEOSEGMENT0000 );
	bank_memcpy( 0xC6, 0x4000, VIDEOSEGMENT0000, COSTIXDATAH_DATASIZE );
	
	PushBank( BANKC1 );
	LoadFile( COSTIXTITLE2BIGFILE_TRACKFILEID, COSTIXTITLE2H_DATAPTR );
	PopBank();
	
	BankUnpack( BANKC0, VIDEOSEGMENT0000, COSTIXBMPTOPBINPCK_PTR );		
	BankUnpack( BANKC1, VIDEOSEGMENT4000, COSTIXBMPBOTTOMBINPCK_PTR );
	BankUnpack( BANKC0, 0x6800, DOOMTRANSITFXCODE_BE00Z80BINPCK_PTR );
	bank_memcpy( 0xC0, 0x3B00, COSTIXBITMAPMASKBMPSPRDATA1PCK_PTR, COSTIXBITMAPMASKBMPSPRDATA1PCK_SIZE );
	Unpack( 0xC000 + COSTIXDATA2H_DATASIZE, 0x3B00 );
}
