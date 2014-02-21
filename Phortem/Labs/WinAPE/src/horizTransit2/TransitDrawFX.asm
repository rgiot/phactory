
; ----------------------------------------------------------------------------
Height equ 5
Height1 equ Height+1

; ----------------------------------------------------------------------------
TransitDrawFX:

TransitDrawFX_Top:
	ld a, 0-Height-Height-Height-Height-Height
	add a, Height
	ld ( TransitDrawFX_Top + 1 ), a
	push af
	call DrawHorizLine
	pop af
	add a, Height1
	push af
	call DrawHorizLine
	pop af
	add a, Height1
	push af
	call DrawHorizLine
	pop af
	add a, Height1	
	push af
	call DrawHorizLine
	pop af
	add a, Height1
	push af
	call DrawHorizLine
	pop af
	add a, Height1
	call DrawHorizLine
	
TransitDrawFX_Bottom:
	ld a, 204+Height+Height+Height+Height+Height
	sub a, Height
	ld ( TransitDrawFX_Bottom + 1 ), a
	push af
	call DrawHorizLine
	pop af
	sub a, Height1
	push af
	call DrawHorizLine
	pop af
	sub a, Height1
	push af
	call DrawHorizLine
	pop af
	sub a, Height1
	push af
	call DrawHorizLine
	pop af
	sub a, Height1
	push af
	call DrawHorizLine
	pop af
	sub a, Height1
	call DrawHorizLine
	
	ret
	
DrawHorizLine:
	cp 205
	ret nc

	ld hl, ScreenTable1
	ld b, 0
	ld c, a
	add hl, bc ; * 4
	add hl, bc
	add hl, bc
	add hl, bc
		
	ld e, (hl)
	inc hl
	ld d, (hl)
	inc hl
	
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
REPEAT 96
	ldi
REND
	
	ret