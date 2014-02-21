
; ----------------------------------------------------------------------------
MACRO LeftPixel
	ld a, (hl)
	and &AA
	ld b, a
	ld a, (de)
	and &55
	add b
	ld (de), a
MEND

MACRO RightPixel
	ld a, (hl)
	and &55
	ld b, a
	ld a, (de)
	and &AA
	add b
	ld (de), a
MEND

; ----------------------------------------------------------------------------
	; FORMAT IS dst ptr (2 bytes), bank src (1 byte), src ptr (2 bytes)
ScreenTable1:
	ds 41*6
	
; ----------------------------------------------------------------------------
TransitDrawFX:	

TransitDrawFX_Left:
	ld a, 0-6-6-6-6-6-6
	add a, 6
	ld ( TransitDrawFX_Left + 1 ), a
	push af
	call DrawVerticalLine
	pop af
	add a, 7
	push af
	call DrawVerticalLine
	pop af
	add a, 7
	push af
	call DrawVerticalLine
	pop af
	add a, 7
	push af
	call DrawVerticalLine
	pop af
	add a, 7
	push af
	call DrawVerticalLine
	pop af
	add a, 7
	call DrawVerticalLine
	
TransitDrawFX_Right:
	ld a, 191+6+6+6+6+6+6
	sub a, 6
	ld ( TransitDrawFX_Right + 1 ), a
	push af
	call DrawVerticalLine
	pop af
	sub a, 7
	push af
	call DrawVerticalLine
	pop af
	sub a, 7
	push af
	call DrawVerticalLine
	pop af
	sub a, 7
	push af
	call DrawVerticalLine
	pop af
	sub a, 7
	push af
	call DrawVerticalLine
	pop af
	sub a, 7
	call DrawVerticalLine
	
	ret
	
DrawVerticalLine:
	cp 192
	jr c, DrawVerticalLine_NotClipped
	
	ld hl, &1000
	ld de, &1000
	ld bc, &300
	ldir
	
	ret
	
DrawVerticalLine_NotClipped:
	di
	ld e, a
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld (&0000), a
	
	ld ( TransitPrevSP + 1 ), sp
	ld sp, ScreenTable1
	
	ld a, e
	srl a
	jp c, DrawVerticalLineRight
	
DrawVerticalLineLeft:
	ld ( DrawVerticalLineLeft_PosX + 1 ), a
	ld a, 205/5
TransitDrawFX_LeftMainLoop:
	ex af, af'
	
	pop bc
	out (c), c
	
	pop hl
	pop de
	
DrawVerticalLineLeft_PosX:
	ld bc, &2000
	
	add hl, bc
	ex de, hl
	add hl, bc
	
	LeftPixel
	res 5, d
	res 5, h
	LeftPixel
	set 3, d
	set 3, h
	LeftPixel
	set 4, d
	set 4, h
	LeftPixel
	res 3, d
	res 3, h
	LeftPixel
	
	ei
	pop bc
	di
	push bc
	
	ex af, af'
	dec a
	jp nz, TransitDrawFX_LeftMainLoop
	
	jp DrawVerticalLineEnd
	
DrawVerticalLineRight:
	ld ( DrawVerticalLineRight_PosX + 1 ), a
	ld a, 205/5
TransitDrawFX_RightMainLoop:
	ex af, af'
	
	pop bc
	out (c), c
	
	pop hl
	pop de
	
DrawVerticalLineRight_PosX:
	ld bc, &2000
	
	add hl, bc
	ex de, hl
	add hl, bc
	
	RightPixel
	res 5, d
	res 5, h
	RightPixel
	set 3, d
	set 3, h
	RightPixel
	set 4, d
	set 4, h
	RightPixel
	res 3, d
	res 3, h
	RightPixel
	
	ei
	pop bc
	di
	push bc
	
	ex af, af'
	dec a
	jp nz, TransitDrawFX_RightMainLoop
	
DrawVerticalLineEnd:
TransitPrevSP:
	ld sp, 0
	
	pop af
	ld ( &0000 ), a
	ld b, &7F
	ld c, a
	out (c), c
	ei
	
	ret