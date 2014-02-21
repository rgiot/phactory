	
; ----------------------------------------------------------------------------
CircleHeight equ 61

ClippingX equ 162

MovePtr:
	dw MoveXY
	
; ----------------------------------------------------------------------------
MACRO HorizLDIR
	ld ( @DoLDIR_SelfM + 1 ), a
@DoLDIR_SelfM:
	jr @DoLDIR_SelfM
	REPEAT 16
	ldi
	REND
MEND

; ----------------------------------------------------------------------------
BatmanDrawFX:	

BatmanDrawFX_Switch:
	ld a, 0
	or a
	jr nz, BatmanDrawFX_SwitchCase1
	ld hl, ScreenTable1
	ld ( BatmanDrawFX_ScreenTablePtr + 1 ), hl	
	ld a, 1
	jr BatmanDrawFX_SwitchEnd
BatmanDrawFX_SwitchCase1:

	ld hl, ( MovePtr )
	dec hl
	dec hl
	dec hl
	dec hl
	ld ( MovePtr ), hl
	
	ld hl, ScreenTable2
	ld ( BatmanDrawFX_ScreenTablePtr + 1 ), hl		

	xor a
BatmanDrawFX_SwitchEnd:
	ld ( BatmanDrawFX_Switch + 1 ), a

	; POSX= -30 no more pix
	; POSX = 191 no more pix
	; POSY = 65 first pixel
	; POSY = 65+205 not visible anymore
	
	ld hl, ( MovePtr )
	inc l
	inc l
	inc l
	inc hl
	ld ( MovePtr ), hl
	
	ld c, (hl)
	inc hl
	ld b, (hl)
	inc hl
	; ld bc, 65 + 105 ; Pos Y
	push bc 
	
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a	
	; ld hl, -29; Pos X
	call InitCircle
	
	pop hl
	
	add hl, hl
	ld bc, ScreenTablePos
	add hl, bc
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	push hl
BatmanDrawFX_ScreenTablePtr:
	ld bc, ScreenTable1
	add hl, bc
	
	call DrawCircle
	
	pop hl
	;ld bc, ScreenTable2
	;add hl, bc
	
	;call DrawCircle	
	ret
	
; ----------------------------------------------------------------------------
InitCircle:	
	bit 7, h
	jr z, NoClipLeft
	
	ld a, l
	neg
	
	ld hl, BatmanCircleClipLeft - 122
	
	ld bc, 122
CircleLeftAdjust:
	add hl, bc
	dec a
	jr nz, CircleLeftAdjust
	
	ld ( DrawCircleSrcData + 1), hl
	
	ld a, &C4
	ld ( DrawCircleCopyBank + 1 ), a
	
	xor a
	jr ClippingCalcEnd
	
ClipRight:
	push af
	
	sub a, b
	
	ld hl, BatmanCircleClipRight
	or a
	jr z, CircleRightAdjustEnd	
	ld bc, 122
CircleRightAdjust:
	add hl, bc
	dec a
	jr nz, CircleRightAdjust
CircleRightAdjustEnd:
	
	ld ( DrawCircleSrcData + 1), hl
	
	ld a, &C6
	ld ( DrawCircleCopyBank + 1 ), a
	
	;ld a, ClippingX/2
	pop af
	srl a
	jr ClippingCalcEnd
		
NoClipLeft:	
	ld a, l
	
	ld b, ClippingX+1
	cp b
	jp nc, ClipRight
	
	ld hl, CircleFullData	
	srl a
	jr nc, SkipPixelRight	
	ld hl, CircleFullData + 122
SkipPixelRight:
	ld ( DrawCircleSrcData + 1 ), hl

ClippingCalcEnd:
	ld ( BatmanDrawCirclePosX + 1 ), a
	
	di
	ld b, &7F
DrawCircleCopyBank:
	ld c, &C0
	out (c), c

DrawCircleSrcData:
	ld hl, CircleFullData
	ld de, CircleData
REPEAT 122
	ldi
REND
	ld b, &7F
	ld a, ( &0000 )
	ld c, a
	out (c), c
	ei	
	ret
	
; ----------------------------------------------------------------------------
DrawCircle:
	;db &ed, &ff
	
	exx
	ld hl, CircleData
BatmanDrawCirclePosX:
	ld c, 0 ; POS X
	exx
	
	di
	ld ( DrawCircle_SaveSP + 1 ), sp
	
	ld sp, hl
	
	ld a, CircleHeight
BatmanDrawCircleY:
	ex af, af'

	ei
	pop bc
	di
	push bc
	pop bc
	out (c), c
	
	pop hl
	pop de
	
	exx
	ld a, c ; POS X
	add a, (hl)
	inc l
	exx
	
	ld b, 0
	ld c, a
	
	add hl, bc
	ex de, hl
	add hl, bc
	
	exx
	ld a, (hl) ; SIZE SCANLINE / LEFT PIXEL PLAIN
	inc l
	exx
	
	bit 7, a
	jr nz, IsOffOn
	bit 6, a
	jr nz, OnOff
	
OffOff:
	HorizLDIR
	
	ex af, af'
	dec a
	jr nz, BatmanDrawCircleY	
	jp EndRoutine
	
IsOffOn:
	bit 6, a
	jr nz, OnOn
	
OffOn:
	res 7, a	
	ld c, a
	
	ld a, (hl)
	inc hl
	and &55
	ld b, a
	ld a, (de)
	and &AA
	add b
	ld (de), a
	inc de
	
	ld a, c
	HorizLDIR
	
	ex af, af'
	dec a
	jp nz, BatmanDrawCircleY
	jr EndRoutine
	
OnOff:
	res 6, a	
	
	HorizLDIR
		
	ld a, (hl)
	and &AA
	ld c, a
	ld a, (de)
	and &55
	add c
	ld (de), a
	
	ex af, af'
	dec a
	jp nz, BatmanDrawCircleY	
	jr EndRoutine
	
OnOn:
	and %00111111
	ld c, a
	
	ld a, (hl)
	inc hl
	and &55
	ld b, a
	ld a, (de)
	and &AA
	add b
	ld (de), a
	inc de
	
	ld a, c
	HorizLDIR
	
	ld a, (hl)
	and &AA
	ld c, a
	ld a, (de)
	and &55
	add c
	ld (de), a
	
	ex af, af'
	dec a
	jp nz, BatmanDrawCircleY	
	
EndRoutine:
	ld a, ( &0000 )
	ld b, &7F
	ld c, a
	out (c), c
	
DrawCircle_SaveSP:
	ld sp, 0
	ei
		
	ret
	