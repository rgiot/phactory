// ----------------------------------------------------------------------------
#ifndef _SHIFTTILE_CXX_
#define _SHIFTTILE_CXX_

// ----------------------------------------------------------------------------
void ShiftTile(char *tileSource)
{
	tileSource;
	
__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	ld	l, (ix+4)
	ld	h, (ix+5)
	
	; 128x32
	ld e, 128
ShiftTile:
	
REPT 32
	set 0, (hl) ; add +8 in color index	(pixel 1)
	set 1, (hl) ; add +8 in color index	(pixel 0)
	inc hl	
ENDM

	inc e
	jp nz, ShiftTile
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
void UnshiftTile(char *tileSource)
{
	tileSource;
	
__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	ld	l, (ix+4)
	ld	h, (ix+5)
	
	; 128x32
	ld e, 0
UnshiftTile:
	
REPT 32
	res 0, (hl) ; reset color index	to 0-7 (pixel 1)
	res 1, (hl) ; reset color index	to 0-7 (pixel 0)
	inc hl	
ENDM

	inc e
	jp nz, UnshiftTile
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _SHIFTTILE_CXX_
