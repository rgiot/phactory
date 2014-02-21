
// ----------------------------------------------------------------------------
#ifndef _CLOUD_CXX_
#define _CLOUD_CXX_

// ----------------------------------------------------------------------------
void DrawCloud_VIDEOSEGMENT8000()
{
__asm
	ld hl, BANK2LINES2_TOPPTR	
	ld ( LinesTopPtr1+1 ), hl
	ld ( LinesTopPtr2+1 ), hl
	
	ld hl, -&8000
	ld ( SrcPtr1+1 ), hl
	ld ( SrcPtr2+1 ), hl
	
	xor a
	ld ( isReverse + 1 ), a
	
	ld bc, 96*103
	ld ix, CODESEGMENT0000
	ld iy, CODESEGMENT0000 + 9888
	
	call _DrawCloud
__endasm;
}

// ----------------------------------------------------------------------------
void DrawCloud_VIDEOSEGMENT0000()
{
__asm
	ld hl, BANK0LINES_TOPPTR	
	ld ( LinesTopPtr1+1 ), hl
	ld ( LinesTopPtr2+1 ), hl
	
	ld hl, &8000
	ld ( SrcPtr1+1 ), hl
	ld ( SrcPtr2+1 ), hl
	
	xor a
	ld ( isReverse + 1 ), a
	
	ld bc, 96*103
	ld ix, CODESEGMENT0000
	ld iy, CODESEGMENT0000 + 9888
	
	call _DrawCloud
__endasm;
}

// ----------------------------------------------------------------------------
void DrawCloudReverse_VIDEOSEGMENT8000()
{
__asm
	ld hl, BANK2LINES2_TOPPTR	
	ld ( LinesTopPtr1+1 ), hl
	ld ( LinesTopPtr2+1 ), hl
	
	ld hl, -&8000
	ld ( SrcPtr1+1 ), hl
	ld ( SrcPtr2+1 ), hl
	
	ld a, 1
	ld ( isReverse + 1 ), a
	
	ld bc, 96*103
	ld ix, CODESEGMENT0000 + 9888-1
	ld iy, CODESEGMENT0000 + 9888 + 9888-1
	
	call _DrawCloud
__endasm;
}

// ----------------------------------------------------------------------------
void DrawCloudReverse_VIDEOSEGMENT0000()
{
__asm
	ld hl, BANK0LINES_TOPPTR	
	ld ( LinesTopPtr1+1 ), hl
	ld ( LinesTopPtr2+1 ), hl
	
	ld hl, &8000
	ld ( SrcPtr1+1 ), hl
	ld ( SrcPtr2+1 ), hl
	
	ld a, 1
	ld ( isReverse + 1 ), a
	
	ld bc, 96*103
	ld ix, CODESEGMENT0000 + 9888-1
	ld iy, CODESEGMENT0000 + 9888 + 9888-1
	
	call _DrawCloud
__endasm;
}

// ----------------------------------------------------------------------------
void DrawCloud()
{
__asm
	di
	ld a, ( &0000 )
	push af
	push bc
	ld bc, &7FC0
	out (c), c
	ld a, &C0
	ld ( &0000 ), a
	pop bc
	ei
		
	;ld bc, 96*103
	;ld ix, CODESEGMENT0000
	;ld iy, CODESEGMENT0000 + 9888
DrawCloudLoop:
	push bc
	
	di
	ld bc, &7FC4
	out (c), c
	ld d, (ix) ; X
	ld e, (iy) ; Y
	ld c, &C0
	out (c), c
	ei
	
LinesTopPtr1:
	ld hl, BANK2LINES2_TOPPTR	
	ld b, 0
	ld c, e
	add hl, bc
	add hl, bc
	ld a, (hl)
	inc hl
	
	ld h, (hl)
	ld l, a
		
	ld c, d
	add hl, bc
	
	push de
	
	ld d, h
	ld e, l
	
SrcPtr1:
	ld bc, -&8000
	add hl, bc
	
	ldi

	pop de
	
LinesTopPtr2:
	ld hl, BANK2LINES2_TOPPTR	
	ld b, 0	
	ld a, e
	inc a
	cp 205
	jr z, skip
	ld c, a
	add hl, bc
	add hl, bc
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	ld c, d
	add hl, bc
	
	ld d, h
	ld e, l
	
SrcPtr2:
	ld bc, -&8000
	add hl, bc
	
	ldi
	
skip:
	
isReverse:
	ld a, 0
	or a
	jr nz, reverseTrue
	inc ix
	inc iy
	jr reverseEnd
reverseTrue:
	dec ix
	dec iy
reverseEnd:

	pop bc
	dec bc
	ld a, b
	or c
	jr nz, DrawCloudLoop
	
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
#endif // _CLOUD_CXX_
