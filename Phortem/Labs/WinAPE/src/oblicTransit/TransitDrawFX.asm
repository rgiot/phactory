
; ----------------------------------------------------------------------------
MACRO WritePixel
	ld ( @prevHL + 1 ), hl
	
	exx
	ld a, c
	add a, (hl)
	inc l
	exx	
	cp 192
	jr nc, @SkipWritePixel
	
	srl a
	ld b, 0
	ld c, a
	jr c, @DrawVerticalLineRight
	
	add hl, bc
	
	; LEFT
	res 7, h
	ld a, (hl)
	set 7, h
	and d
	ld b, a
	ld a, (hl)
	and e
	add b
	ld (hl), a
	
	jr @SkipWritePixel
@DrawVerticalLineRight:
	
	add hl, bc
	
	; RIGHT	
	res 7, h
	ld a, (hl)
	set 7, h
	and e
	ld b, a
	ld a, (hl)
	and d
	add b
	ld (hl), a
@SkipWritePixel:

@prevHL:
	ld hl, 0
MEND

; ----------------------------------------------------------------------------
	; FORMAT IS dst ptr (2 bytes), bank src (1 byte), src ptr (2 bytes)
ScreenTable1:
	ds 41*2
	
; ----------------------------------------------------------------------------
TransitDrawFX:

	;ld a, 40
	;call DrawVerticalLine
	;ret

TransitDrawFX_Left:
	ld a, 0-3-3-3-3-3-3-3
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
	
TransitDrawFX_Right:
	ld a, 191+3+3+3+3+3
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
	
	ret
	
DrawVerticalLine:
	exx
SinTablePtr:
	ld hl, SinTable
	ld c, a
	exx

	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld (&0000), a
	
	ld ( TransitPrevSP + 1 ), sp
	ld sp, ScreenTable1
	
DrawVerticalLineLeft:
	
	ld d, &AA
	ld e, &55
	
	ld a, 205/5
TransitDrawFX_LeftMainLoop:
	ex af, af'
	
	pop hl
	
	WritePixel
	res 5, h
	WritePixel
	set 3, h
	WritePixel
	set 4, h
	WritePixel
	res 3, h
	WritePixel
	
	ei
	pop bc
	di
	push bc
	
	ex af, af'
	dec a
	jp nz, TransitDrawFX_LeftMainLoop	
	
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