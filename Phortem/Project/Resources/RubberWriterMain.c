
// ----------------------------------------------------------------------------
#include "Config.h"

#include "RubberData1.h"
#include "RubberData2.h"
#include "RubberWriter.h"
#include "RubberWriter3.h"
#include "Rubber.cpcbitmap.info.h"

// ----------------------------------------------------------------------------
void Dummy()
{
__asm
	SliceWidth 		equ 10
	SliceSkip		equ 26

	RubberFrameSlicesPtr 	equ &3100 + 102 ; for possible stack usage
	RubberFramePosXPtr 		equ &3300
	SineCurvePtr			equ RUBBERSINECURVE1BIN_PTR
	SineCurve2Ptr			equ RUBBERSINECURVE2BIN_PTR		
	SineCurvePosXPtr 		equ RUBBERSINECURVEPOSXBIN_PTR		
	SineCurvePosX2Ptr 		equ RUBBERSINECURVEPOSX2BIN_PTR		
	RubberPrecaDataPtr2 	equ RUBBERPRECADATA2BIN_PTR		
	RubberPrecaOffsetPtr 	equ RUBBERPRECAOFFSETPTRBIN_PTR
	RubberPrecaDataPtr		equ RUBBERPRECADATA1BIN_PTR
	RubberFireCodePtr		equ RUBBERDRAWFIREBIN_PTR	
	
	include "rubberDrawVerticalSlice.asm"		
	include "rubberCreateRubberMove.asm"
	include "rubberDrawRubber.asm"
	
GetTopPtr:
	ld a, (_IsFlipped)
	or a
	jr z, GetTopPtr2
	ld hl, &8080
	ret
GetTopPtr2:
	ld hl, &0080
	ret

GetBottomPtr:
	ld a, (_IsFlipped)
	or a
	jr z, GetBottomPtr2
	ld hl, &C000
	ret
GetBottomPtr2:
	ld hl, &4000
	ret
__endasm;
}

// ----------------------------------------------------------------------------
unsigned char IsFlipped;
unsigned short GoDownCounter;

// ----------------------------------------------------------------------------
void Init()
{
}

// ----------------------------------------------------------------------------
void DrawRubber()
{
__asm
	ld ( _IsFlipped ), a
	
	push bc	
	call InitRubberAll
DrawRubber_GoUp:
	ld e, 0
	call RubberFireCodePtr
	pop bc
	
	di
	ld a, ( &0000 )
	push af
	push bc
	ld bc, &7FC0
	out (c), c		
	ld a, c
	ld ( &0000 ), a
	ei		
	
	pop bc
	push bc
	call DrawVerticalSliceAll
	pop bc
	
	call DrawRubberAll
				
	di
	pop af
	ld c, a
	ld b, &7F
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
__endasm;

	GoDownCounter++;
	if ( GoDownCounter == 289 )
	{
__asm
		ld a, 1
		ld ( DrawRubber_GoUp + 1 ), a
		call SetDrawSliceQuit
__endasm;
	}
}
