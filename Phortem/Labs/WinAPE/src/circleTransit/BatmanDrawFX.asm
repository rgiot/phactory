	
; ----------------------------------------------------------------------------
CircleHeight equ 61

ClippingX equ 162
	
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
CircleMoveXY: 
	db 0, 0
	db 1, 0
	db 2, 0
	db 0, 1
	db 1, 1
	db 2, 2
	db 1, 2
	db 2, 1
	db 3, 3
	db 3, 2
	db 2, 3
	db 3, 1
	db 1, 3
	db 0, 3
	db 0, 4
	db 0, 5
	db 1, 5
	db 2, 5
	db 4, 0
	db 5, 0
	db 4, 2
	db 4, 1
	db 4, 4
	db 1, 4
	db 2, 4
	db 3, 4
	db 3, 5
	db 5, 5
	db 4, 5
	db 4, 3
	db 5, 1
	db 5, 2
	db 6, 3
	db 5, 3
	db 6, 4
	db 5, 4
	db 6, 3
	db 6, 0
	db 6, 1
	db 6, 2
	db 8, 0
	db 7, 0
	db 8, 2
	db 8, 1
	db 7, 1
	db 7, 2
	db 7, 3
	db 7, 4
	db 8, 3
	db 7, 5
	db 8, 5
	db 6, 5
	db 8, 4
	db 9, 4
	db 9, 1
	db 9, 0
	db 9, 2
	db 10, 2
	db 10, 6
	db 10, 0
	db 9, 5
	db 9, 3
	db 10, 3
	db 10, 4
	db 10, 5
	db 10, 1
	db -1
	
; ----------------------------------------------------------------------------
BatmanDrawFX:	
	; top left  X -4  Y -8
	
CircleMovePtr:
	ld hl, CircleMoveXY - 2
	inc hl
	inc hl	
	ld a, (hl)
	cp -1
	jr nz, NoMoveReset
	ld hl, CircleMoveXY
NoMoveReset:
	ld ( CircleMovePtr + 1 ), hl
	
	inc hl
	ld a, (hl)
	dec hl
	or a
	jr z, CircleMovePtr
	;cp 1
	;jr z, CircleMovePtr
	;cp 5
	;jr z, CircleMovePtr	
	cp 6
	jr z, CircleMovePtr	
	
	call CalcXY

	push bc
	;ld hl, -4 - 20; Pos X
	call InitCircle
	
	;ld hl, 65 + 0 - 8  -40 ; Pos Y
	pop hl
	add hl, hl
	ld bc, ScreenTablePos
	add hl, bc
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	ld bc, ScreenTable1
	add hl, bc
	
	call DrawCircle
	
	ret
	
; ----------------------------------------------------------------------------
CalcXY:
	ld b, (hl) ; X
	inc hl
	ld c, (hl) ; Y
	
	ld d, 0 ; Final X default topleft
	ld e, 65+0-8-40 ; Final Y default topleft
	
	ld a, b
	or a
	jr z, SkipX
	ld a, d
AddX:
	add a, 20
	dec b
	jr nz, AddX	
	ld d, a
SkipX:

	ld a, c
	or a
	jr z, SkipY
	ld a, e
AddY:
	add a, 40
	dec c
	jr nz, AddY
	ld e, a
SkipY:
	
	; Final X
	ld h, 0
	ld l, d
	ld bc, -4-20
	add hl, bc
	
	; Final Y
	ld b, 0
	ld c, e
	
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
	
	ld ( CircleSrc + 1), hl
	
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
	
	ld ( CircleSrc + 1), hl
	
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
	ld ( CircleSrc + 1 ), hl

ClippingCalcEnd:
	ld ( BatmanDrawCirclePosX + 1 ), a
	ret
	
; ----------------------------------------------------------------------------
DrawCircle:
	;db &ed, &ff
	
	exx
CircleSrc:
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
	