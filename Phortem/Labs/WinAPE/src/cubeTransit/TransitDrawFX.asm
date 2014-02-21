
; ----------------------------------------------------------------------------
BlockWidth equ 12 ; 8 blocks horizontal
BlockHeight equ 10 ; 4 blocks vert

; ----------------------------------------------------------------------------
	; FORMAT IS dst ptr (2 bytes), bank src (1 byte), src ptr (2 bytes)
ScreenTable1:
	ds 41*6
	
; ----------------------------------------------------------------------------
TransitMoveTable:	
	db 7, 3
	db 6, 3
	db 6, 2
	db 5, 2
	db 5, 3
	db 4, 3
	db 3, 3
	db 2, 3
	db 1, 3
	db 0, 3
	db 0, 2
	db 1, 2
	db 2, 2
	db 3, 2
	db 3, 1
	db 2, 1
	db 1, 1
	db 0, 1
	db 0, 0
	db 1, 0
	db 2, 0
	db 3, 0
	db 4, 0
	db 5, 0
	db 6, 0
	db 7, 0
	db 7, 1
	db 7, 2
	db 6, 1
	db 5, 1
	db 4, 1
	db 4, 2
	db -1

; ----------------------------------------------------------------------------
TransitDrawFX:	

TransitDrawFX_MoveTablePtr:
	ld hl, TransitMoveTable-2
	ld bc, 2
	add hl, bc
	ld a, (hl)
	cp -1
	jr nz, TransitDrawFX_MoveTablePtr_NoReset
	ld hl, TransitMoveTable
TransitDrawFX_MoveTablePtr_NoReset:
	ld ( TransitDrawFX_MoveTablePtr + 1 ), hl
	
	ld a, (hl)
	inc hl
	ld l, (hl)
	ld h, a
	
	call DrawBGBlock

	ret

; ----------------------------------------------------------------------------
DrawBGBlock:
	
	ld a, BlockHeight	
	ld ( DrawBGBlock_BlockHeight + 1 ), a
	ld a, l
	cp 3
	jr nz, DrawBGBlock_BlockHeight_NoLastLine
	ld a, BlockHeight+1
	ld ( DrawBGBlock_BlockHeight + 1 ), a
DrawBGBlock_BlockHeight_NoLastLine:

	ld a, h
	or a
	jr nz, DrawBGBlock_CalcPosX_NoZero
	ld bc, &2000
	jr DrawBGBlock_CalcPosXEnd
DrawBGBlock_CalcPosX_NoZero:
	xor a	
DrawBGBlock_CalcPosX:
	add a, BlockWidth
	dec h
	jr nz, DrawBGBlock_CalcPosX
	ld b, &20 ; add &2000 (res, set) 
	ld c, a
DrawBGBlock_CalcPosXEnd:
	ld ( DrawBGBlock_OffsetX + 1 ), bc
	
	ld a, l
	or a
	jr nz, DrawBGBlock_CalcPosY_NoZero
	ld hl, ScreenTable1	
	jr DrawBGBlock_CalcPosYEnd
DrawBGBlock_CalcPosY_NoZero:
	ld hl, ScreenTable1
	ld bc, 6 * BlockHeight
DrawBGBlock_CalcPosY:
	add hl, bc
	dec a
	jr nz, DrawBGBlock_CalcPosY
DrawBGBlock_CalcPosYEnd:
	push hl
	pop ix

DrawBGBlock_BlockHeight:
	ld a, BlockHeight
DrawBGBlock_YLoop:
	push af
	
	push ix
	
	ld c, (ix+0)
	ld b, (ix+1)
	ld l, (ix+2)
	ld h, (ix+3)
	ld e, (ix+4)
	ld d, (ix+5)
	
	di
	ld a, (&0000)
	push af
	out (c), c
	ld a, c
	ld (&0000), a
	ei
	
DrawBGBlock_OffsetX:
	ld bc, 0
	add hl, bc
	ex de, hl
	add hl, bc
	
	call DrawBlockLine
	res 5, d
	res 5, h
	call DrawBlockLine
	set 3, d
	set 3, h
	call DrawBlockLine
	set 4, d
	set 4, h
	call DrawBlockLine
	res 3, d
	res 3, h
	call DrawBlockLine
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld (&0000), a
	ei

	pop hl
	ld bc, 6
	add hl, bc
	push hl
	pop ix
	
	pop af
	dec a
	jr nz, DrawBGBlock_YLoop
	
	ret
	
DrawBlockLine:
	push hl
	push de
REPEAT BlockWidth
	ldi
REND
	pop de
	pop hl
	ret