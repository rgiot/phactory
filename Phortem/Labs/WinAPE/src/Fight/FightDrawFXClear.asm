
; ----------------------------------------------------------------------------
ClearFrame:
	ld iyh, 0
	
	; 22000

	inc hl ; skip Data Texture
	inc hl
	push hl
	pop ix
	
	exx
	ld hl, (Prev2OffsetXPtr)	
	ld c, (hl)
	ld hl, (Prev2SinFlipPtr1)
	ld de, (Prev2SinFlipPtr2)
	ld b, -4
	exx
	
	ld a, 20
ClearFrameLoop:
	ex af, af'
	
	ld e, (ix) ; bg texture
	ld d, (ix+1)
	ld l, (ix+2) ; src
	ld h, (ix+3)
	
	exx	
	ld a, b
	add a, e
	ld e, a
	ld a, b
	add a, l
	ld l, a
	ld a, (de)
	dec e
	add a, (hl)
	dec l
	add a, c
	exx
	
	rra
	ld b, &0
	ld c, a
	dec bc
	add hl, bc
	ex de, hl
	add hl, bc
	
	di
	ld (PrevSP+1), sp
	
	ld sp, -SCR_BLOCKSWIDTH_DIV2-1+&800
	
REPEAT SCR_BLOCKSWIDTH_DIV2+1
	ldi	
REND
	add hl, sp
	ex de, hl
	add hl, sp
	ex de, hl
REPEAT SCR_BLOCKSWIDTH_DIV2+1
	ldi	
REND
	add hl, sp
	ex de, hl
	add hl, sp
	ex de, hl
REPEAT SCR_BLOCKSWIDTH_DIV2+1
	ldi	
REND
	add hl, sp
	ex de, hl
	add hl, sp
	ex de, hl
REPEAT SCR_BLOCKSWIDTH_DIV2+1
	ldi	
REND
	add hl, sp
	ex de, hl
	add hl, sp
	ex de, hl
REPEAT SCR_BLOCKSWIDTH_DIV2+1
	ldi	
REND

PrevSP:
	ld sp, 0
	ei

	ld bc, 6*5
	add ix, bc
	
	ex af, af'
	dec a
	jp nz, ClearFrameLoop
	
	ld a, iyh
	or a
	jr nz, ClearFrameLoop_End
	
	di
	ld a, ( BottomBank )
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	ld iyh, 1
	
	ld a, 21
	jp ClearFrameLoop
	
ClearFrameLoop_End:
	
	ret
	
	