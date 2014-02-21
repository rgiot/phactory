
// ----------------------------------------------------------------------------
#ifndef _VIDEO_CXX_
#define _VIDEO_CXX_

// ----------------------------------------------------------------------------
void SetColor( char pen, char color )
{
	pen; 
	color;

__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	ld e, ( ix + 4 )
	ld l, ( ix + 5 )

	ld b, &7f
	ld c, e
	di
	out (c), c
	ld c, l
	out (c), c
	ei
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
void SetAsicColor( char *asicPalette, char pen, char indexColorInPalette )
{
	asicPalette; 
	pen;
	indexColorInPalette;

__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	ld l, ( ix + 4 )
	ld h, ( ix + 5 )
	ld e, ( ix + 6 ) ; pen
	ld d, ( ix + 7 ) ; indexColorInPalette
	
	di
	ld bc, &7FB8
	out (c), c
	
	push hl
	
	ld b, 0
	ld c, e
	ld hl, &6400
	add hl, bc
	add hl, bc
	
	ld a, d
	ex de, hl
	
	pop hl
	
	; skip Gate Array palette
	ld bc, 16
	add hl, bc
	
	add a, a
	ld c, a
	add hl, bc
	
	ldi
	ldi
	
	ld bc, &7FA0
	out (c), c
	ei
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
void SetBlackPalette()
{
__asm		
	push ix
	ld ix, 0
	add	ix, sp
	
	ld a, ( &0037 )
	or a
	jp nz, SetBlackPalettePlus
	
	xor a
BlackPalette_Loop:
	ld b, &7f
	ld c, a
	di
	out (c), c
	ld c, COLOR0
	out (c), c
	ei
	
	inc a
	cp 17 ; border included
	jr nz, BlackPalette_Loop
	jr SetBlackPaletteQuit
	
SetBlackPalettePlus:
	di
	ld bc, &7FB8
	out (c), c
	
	ld hl, &6400
	ld c, 16*2
	xor a
SetBlackPalettePlusLoop:
	ld (hl), a
	inc hl
	dec c
	jr nz, SetBlackPalettePlusLoop
	
	ld bc, &7FA0
	out (c), c
	ei

SetBlackPaletteQuit:
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
void SetPalette( char *palette )
{
	palette;

__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	ld l, (ix + 4)
	ld h, (ix + 5)
	
	ld a, ( &0037 )
	or a
	jp nz, SetPalettePlus

	ld e, 0

setPaletteBegin:
	push de
	push hl

	ld a, (hl)
	ld l, a

	ld b, &7f
	ld c, e
	di
	out (c), c
	ld c, l
	out (c), c
	ei
	
	pop hl
	pop de
	
	inc hl

	inc e
	ld a, e
	cp 16
	jr nz, setPaletteBegin
	jr SetPaletteQuit
	
SetPalettePlus:
	di
	ld bc, &7FB8
	out (c), c
	
	ld bc, 16
	add hl, bc	
	ld de, &6400
	ld bc, 16*2
	ldir
	
	ld bc, &7FA0
	out (c), c
	ei
SetPaletteQuit:	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
void SetPaletteFrom8( char *palette )
{
	palette;

__asm		
	push ix
	ld ix, 0
	add	ix, sp
	
	ld l, (ix + 4)
	ld h, (ix + 5)
	
	ld a, ( &0037 )
	or a
	jp nz, SetPaletteFrom8Plus	

	ld e, 8

setPaletteBeginFrom8:
	push de
	push hl

	ld a, (hl)
	ld l, a

	ld b, &7f
	ld c, e
	di
	out (c), c
	ld c, l
	out (c), c
	ei
	
	pop hl
	pop de
	
	inc hl

	inc e
	ld a, e
	cp 16
	jr nz, setPaletteBeginFrom8
	jr SetPaletteFrom8Quit
	
SetPaletteFrom8Plus:
	ld bc, 8+8
	add hl, bc
	
	di
	ld bc, &7FB8
	out (c), c
	
	ld de, &6400+8+8
	ld bc, 8*2
	ldir
	
	ld bc, &7FA0
	out (c), c
	ei
	
SetPaletteFrom8Quit:
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _VIDEO_CXX_
