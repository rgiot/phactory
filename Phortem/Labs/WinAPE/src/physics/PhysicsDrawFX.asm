	
; ----------------------------------------------------------------------------
CircleHeight equ 61
ClippingX equ 162

BallCount equ 6

; ----------------------------------------------------------------------------
PhysicsDrawFX_IsFlipped:
	db 0
	
; ----------------------------------------------------------------------------
PhysicsDrawFX:
	ld (PhysicsDrawFX_IsFlipped), a
	
	ld hl, NOPS
	ld a, (PhysicsDrawFX_IsFlipped)
	or a
	jr nz, SkipFlip
	ld hl, RES7
SkipFlip:
	ld de, RES7OPERATION
	ldi
	ldi	
	dec hl
	dec hl
	ld de, RES7OPERATION2
	ldi
	ldi

FrameDataPtr:
	ld hl, FrameData - BallCount-BallCount
	ld bc, BallCount*2
	add hl, bc
	ld (FrameDataPtr+1), hl
	
FrameDataSkipClear:
	ld a, -3
	inc a
	jr nz, SkipClear
	
	push hl
	ld bc, -BallCount*4
	add hl, bc
	call PhysicsClearBoxes
	pop hl
	
	jr SkipClearEnd	
SkipClear:
	ld ( FrameDataSkipClear + 1 ), a
SkipClearEnd:
	
	call PhysicsDrawBalls
	ret
	
; ----------------------------------------------------------------------------
BallsCopy:
	ds BallCount*2
	
; ----------------------------------------------------------------------------
PhysicsClearBoxes:
	di
	ld bc, &7FC4
	out (c), c
	ld de, BallsCopy
	ld bc, BallCount*2
	ldir
	ld a, (&0000)
	ld b, &7F
	ld c, a
	out (c), c
	ei
	ld hl, BallsCopy

	ld a, BallCount
FrameDataClearLoop:
	push af
	
	ld a, (hl)
	inc hl
	ld c, (hl)
	inc hl
	push hl
	
	; Y (StartScreen= 65)
	ld d, 0
	ld e, c
	
	; X
	ld c, a
	
	call PhysicsClearBox
	
	pop hl
	pop af
	dec a
	jr nz, FrameDataClearLoop
	
	ret
	
; ----------------------------------------------------------------------------
MACRO DrawCircleScanline
	ld ( @DoLDIR_SelfM + 1 ), a
@DoLDIR_SelfM:
	jr @DoLDIR_SelfM
	REPEAT 16
	ld (hl), e
	inc hl
	REND
MEND

; ----------------------------------------------------------------------------
PhysicsDrawBalls:
	di
	ld bc, &7FC4
	out (c), c
	ld de, BallsCopy
	ld bc, BallCount*2
	ldir
	ld a, (&0000)
	ld b, &7F
	ld c, a
	out (c), c
	ei
	ld hl, BallsCopy

	ld a, BallCount
FrameDataDrawLoop:
	push af
	
	ld a, (hl)
	inc hl
	ld c, (hl)
	inc hl
	push hl
	
	; Y (StartScreen= 65)
	ld d, 0
	ld e, c
	
	; X
	ld c, a
	
	call PhysicsDrawCircle
	
	pop hl
	pop af
	dec a
	jr nz, FrameDataDrawLoop
	
	ret


; ----------------------------------------------------------------------------
RES7:
	res 7, h
NOPS:
	nop
	nop
	
; ----------------------------------------------------------------------------
PhysicsDrawCircle:	

	ld hl, ScreenTableOffset
	add hl, de
	add hl, de
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	ld a, (PhysicsDrawFX_IsFlipped)
	ld de, ScreenTableFlip2
	or a
	jr nz, PhysicsDrawCircleSetSprite
	ld de, ScreenTableFlip1
PhysicsDrawCircleSetSprite:
	
	add hl, de ; ScreenTable	
	push hl
	
	ld a, c
	exx
	ld hl, CircleFullData
	srl a
	jr nc, SkipPixelRight	
	ld hl, CircleFullData + 122
SkipPixelRight:
	ld c, a
	; HL, C are updated
	exx
	
	pop hl ; ScreenTable1 or 2 at right Y pos

	di
	ld ( DrawCircle_SaveSP + 1 ), sp
	
	ld sp, hl
	
	ld e, &3C ; black color
	
	ld d, CircleHeight
PhysicsDrawCircleY:
	ei
	pop bc ; bank
	di
	push bc
	pop bc
	out (c), c
	
	pop hl ; dst ptr
	pop bc ; src ptr, ignored
	
	exx
	ld a, c
	add a, (hl)
	inc l
	exx
	
	; de = posx in background
	ld b, 0
	ld c, a	
	add hl, bc
	
RES7OPERATION:
	res 7, h
	
	; hl = dst
	
	exx
	ld a, (hl) ; SIZE SCANLINE / LEFT PIXEL PLAIN
	inc l
	exx
	
	bit 7, a
	jr nz, IsOffOn
	bit 6, a
	jr nz, OnOff
	
OffOff:
	DrawCircleScanline
	
	dec d
	jr nz, PhysicsDrawCircleY	
	jp EndRoutine
	
IsOffOn:
	bit 6, a
	jr nz, OnOn
	
OffOn:
	res 7, a	
	ld c, a
	
	ld a, (hl)
	and &AA
	or &14 ; FC AND 55 ; PEN 6
	ld (hl), a
	inc hl
	
	ld a, c
	DrawCircleScanline
	
	dec d
	jp nz, PhysicsDrawCircleY
	jr EndRoutine
	
OnOff:
	res 6, a	
	
	DrawCircleScanline
		
	ld a, (hl)
	and &55
	or &28 ; 3C AND AA ; PEN 6
	ld (hl), a
	
	dec d
	jp nz, PhysicsDrawCircleY	
	jr EndRoutine
	
OnOn:
	and %00111111
	ld c, a
	
	ld a, (hl)
	and &AA
	or &14 ; FC AND 55 ; PEN 6
	ld (hl), a
	inc hl
	
	ld a, c
	DrawCircleScanline
	
	ld a, (hl)
	and &55
	or &28 ; 3C AND AA ; PEN 6
	ld (hl), a
		
	dec d
	jp nz, PhysicsDrawCircleY	
	
EndRoutine:
	ld a, ( &0000 )
	ld b, &7F
	ld c, a
	out (c), c
	
DrawCircle_SaveSP:
	ld sp, 0
	ei
		
	ret
	
; ----------------------------------------------------------------------------
PhysicsClearBox:	

	ld hl, ScreenTableOffset
	add hl, de
	add hl, de
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	ld a, (PhysicsDrawFX_IsFlipped)
	ld de, ScreenTableFlip2
	or a
	jr nz, PhysicsClearCircleSetSprite
	ld de, ScreenTableFlip1
PhysicsClearCircleSetSprite:
	
	add hl, de ; ScreenTable	
	
	ld a, c
	srl a
	cp 96-16
	jr c, lastVertScanlineToSkip
	ld a, 96-16
lastVertScanlineToSkip:
	ld (ClearPosX+1), a
	
	; HL = ScreenTable1 or 2 at right Y pos
	
	di
	ld ( ClearCircle_SaveSP + 1 ), sp
	
	ld sp, hl
	
	ld a, CircleHeight
PhysicsClearCircleY:
	ei
	pop bc ; bank
	di
	push bc
	pop bc
	out (c), c
	
	pop hl ; dst ptr
	pop de ; src ptr
	
ClearPosX:
	ld bc, 0
	add hl, bc
	
RES7OPERATION2:
	res 7, h
	
	ex de, hl
	add hl, bc
	
	; hl = src
	; de = dst

	REPEAT 16
	ldi
	REND
	
	dec a
	jr nz, PhysicsClearCircleY
	
	ld a, ( &0000 )
	ld b, &7F
	ld c, a
	out (c), c
	
ClearCircle_SaveSP:
	ld sp, 0
	ei
		
	ret
	
	