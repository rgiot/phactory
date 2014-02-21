
// ----------------------------------------------------------------------------
#include "Config.h"

// ----------------------------------------------------------------------------
void DrawFire()
{	
__asm		
RubberFramePosXPtr 		equ &3300

		ld b, 1
GoUpLoop:

		; e = 0: go up
		; e = 1: go down
		ld a, e
		or a
		ld a, ( HideY )
		jp nz, GoDown
		
GoUp:
		cp 1
		ret z
		dec a
		ld ( HideY ), a	
		
		dec b
		jr nz, GoUpLoop
		
		jp DoClear
		
GoDown:
		cp 5+41
		ret z
		inc a
		ld ( HideY ), a	
		
DoClear:		
		ld b, a		
		xor a		
		ld hl, RubberFramePosXPtr		
CreateRubberHide_Loop:
		
REPT 5
		ld ( hl ), a
		inc l
ENDM

		dec b
		jr nz, CreateRubberHide_Loop
		
REPT 1
		inc l
ENDM
REPT 4
		ld ( hl ), a
		inc l
ENDM		
		
REPT 2
		inc l
ENDM
REPT 3
		ld ( hl ), a
		inc l
ENDM		

REPT 3
		inc l
ENDM
REPT 2
		ld ( hl ), a
		inc l
ENDM

REPT 4
		inc l
ENDM

		ld ( hl ), a

		ret
		
HideY:
		db 5+41 ; 5=25/5  41=205/5
__endasm;
}
		
		