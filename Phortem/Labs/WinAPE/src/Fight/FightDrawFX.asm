
; ----------------------------------------------------------------------------
SinMoveSpeed1 equ 9
SinMoveSpeed2 equ 8
SinMoveSpeedX equ 2

; ----------------------------------------------------------------------------
BottomBank:
	db 0
ScreenTablePtr:
	dw 0
	
SinFlipPtr1:
	dw SinX1 + 54
SinFlipPtr2:
	dw SinX1 + 134
	
PrevSinFlipPtr1:
	dw 0
PrevSinFlipPtr2:
	dw 0
	
Prev2SinFlipPtr1:
	dw 0
Prev2SinFlipPtr2:
	dw 0
	
OffsetXPtr:
	dw SinX2 + 45
PrevOffsetXPtr:
	dw SinX2 + 45
Prev2OffsetXPtr:
	dw SinX2 + 45
	
; ----------------------------------------------------------------------------
FightDrawFX:
	push af
	call UnpackNextFrame		
	pop af

	or a
	jr z, FightDrawFX_InitFlipped
	
	ld a, &C1
	ld hl, ScreenTable2
	jr FightDrawFX_EndInitFlipping
	
FightDrawFX_InitFlipped:
	ld a, &C3
	ld hl, ScreenTable1
	
FightDrawFX_EndInitFlipping:
	ld ( ScreenTablePtr ), hl
	ld ( BottomBank ), a
	
	ld hl, (PrevSinFlipPtr1)
	ld (Prev2SinFlipPtr1), hl	
	ld hl, (SinFlipPtr1)
	ld (PrevSinFlipPtr1), hl
	ld a, SinMoveSpeed1
	add a, l
	ld l, a
	ld (SinFlipPtr1), hl
	
	ld hl, (PrevSinFlipPtr2)
	ld (Prev2SinFlipPtr2), hl	
	ld hl, (SinFlipPtr2)
	ld (PrevSinFlipPtr2), hl
	ld a, SinMoveSpeed2
	add a, l
	ld l, a
	ld (SinFlipPtr2), hl	
	
	ld hl, (PrevOffsetXPtr)
	ld (Prev2OffsetXPtr), hl
	ld hl, (OffsetXPtr)
	ld (PrevOffsetXPtr), hl
	ld a, SinMoveSpeedX
	add a, l
	ld l, a
	ld (OffsetXPtr), hl

SkipClearFrame:
	ld a, 0
	inc a
	ld ( SkipClearFrame + 1), a	
	cp 3
	jp nz, SkipClearFrameEnd
	dec a
	ld ( SkipClearFrame + 1), a
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC4
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	ld hl, (ScreenTablePtr)
	call ClearFrame
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
SkipClearFrameEnd:
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC4
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	ld hl, (ScreenTablePtr)
	call DrawFrame
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	ret

; ----------------------------------------------------------------------------
	read "FightDrawFXClear.asm"
	read "FightDrawFXDraw.asm"
	read "FightDrawFXUnpack.asm"
		
	