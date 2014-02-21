
; ----------------------------------------------------------------------------
MACRO LeftPixel
	ld a, (de)
	and &AA
	ld b, a
	ld a, (hl)
	and &55
	add b
	ld (hl), a
MEND

MACRO RightPixel
	ld a, (de)
	and &55
	ld b, a
	ld a, (hl)
	and &AA
	add b
	ld (hl), a
MEND
	
; ----------------------------------------------------------------------------
TransitDrawFX:

TransitDrawFX_SinMovePtr:
	ld hl, SinMove1
	inc l
	inc l
	inc l
	inc l
	inc l
	ld ( TransitDrawFX_SinMovePtr + 1 ), hl
	push hl
	pop ix

TransitDrawFX_SinMovePtr2
	ld hl, SinMove2 + 150
	inc l
	inc l
	inc l
	inc l
	ld ( TransitDrawFX_SinMovePtr2 + 1 ), hl
	push hl
	pop iy
	
	ld hl, YTable
	
	xor a
UpdateYTable_Loop:
	push af
	
	ld ( LinePosX + 1 ), a
	
	ld e, (hl)
	inc l
	ld d, (hl)
	dec l
	
	ld a, (ix)
	inc ixl
	add a, (iy)
	inc iyl
	ld c, a	
	
	ex de, hl
	ld a, h
	ld ( LinePosY + 1), a
	
	ld b, 0
REPEAT 8
	add hl, bc
REND
	ld b, a
	ld a, h
	sub b
	ex de, hl	
	
	ld (hl), e	
	inc l
	ld (hl), d
	inc hl
	
	or a
	jr z, SkipVertLine

	push hl
	push ix
	
LinePosX:
	ld e, 0
LinePosY:
	ld c, 0
	
	call DrawVerticalLine
	
	pop ix
	pop hl
	
SkipVertLine:

	pop af
	inc a
	cp 192
	jp nz, UpdateYTable_Loop

	ret
	
; ----------------------------------------------------------------------------
DrawVerticalLine:
	; e = pos x
	; a = height
	
	ld d, a
	
	ld a, c
	cp 205+40
	ret nc
	
	ld hl, ScreenTable1
	ld b, 0
	; c = pos y
REPEAT 6
	add hl, bc
REND
	
	;db &ed, &ff
	
	di
	ld ( TransitPrevSP + 1 ), sp
	ld sp, hl
	
	ld a, e
	srl a
	jp c, DrawVerticalLineRight
	
DrawVerticalLineLeft:
	ld ( DrawVerticalLineLeft_PosX + 1 ), a
DrawVerticalLineHeight:
	ld a, d
TransitDrawFX_LeftMainLoop:
	ex af, af'
	
	pop bc
	out (c), c
	
	pop hl
	pop de
	
DrawVerticalLineLeft_PosX:
	ld bc, &0000
	
	add hl, bc
	ex de, hl
	add hl, bc
	
	LeftPixel
	
	ex af, af'
	dec a
	jr nz, TransitDrawFX_LeftMainLoop
	
	jp DrawVerticalLineEnd
	
DrawVerticalLineRight:
	ld ( DrawVerticalLineRight_PosX + 1 ), a
DrawVerticalLineRightHeight:
	ld a, d
TransitDrawFX_RightMainLoop:
	ex af, af'
	
	pop bc
	out (c), c
	
	pop hl
	pop de
	
DrawVerticalLineRight_PosX:
	ld bc, &0000
	
	add hl, bc
	ex de, hl
	add hl, bc
	
	RightPixel
	
	ex af, af'
	dec a
	jr nz, TransitDrawFX_RightMainLoop
	
DrawVerticalLineEnd:
TransitPrevSP:
	ld sp, 0
	
	ld a, ( &0000 )
	ld b, &7F
	ld c, a
	out (c), c
	ei
	
	ret