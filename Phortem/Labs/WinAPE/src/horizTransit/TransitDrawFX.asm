
	
; ----------------------------------------------------------------------------
TransitDrawFX:	

TransitPtr:
	ld hl, HorizTransit-1
	inc hl
	
	ld bc, HorizTransitEnd
	ld a, h
	cp b
	jr nz, TransitPtr_NoEnd
	ld a, l
	cp c
	ret z	
TransitPtr_NoEnd:
	
	ld ( TransitPtr + 1 ), hl
	ld a, (hl)
	
	ld hl, ScreenTable1
	or a
	jr z, SkipCalcPtr
	ld bc, 5
CalcPtr:
	add hl, bc
	dec a
	jr nz, CalcPtr
SkipCalcPtr:
	
	halt
			
	di
	ld c, (hl)
	inc hl
	ld b, &7F
	out (c), c
	
	ld e, (hl)
	inc hl
	ld d, (hl)
	inc hl
	
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	push de
	push de
	
	call DrawLine

	pop hl
	res 7, h
	ex de, hl
	pop hl
	
	ld bc, &7FC0
	out (c), c

	call DrawLine
	
	ld a, ( &0000 )
	ld b, &7F
	ld c, a
	out (c), c
	ei
	
	ret
	
DrawLine:
	
	push hl
REPEAT 32
	ldi
REND
	pop hl
	push hl
REPEAT 32
	ldi
REND
	pop hl
REPEAT 32
	ldi
REND

	ret