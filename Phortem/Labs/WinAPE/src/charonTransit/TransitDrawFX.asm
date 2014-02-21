
; ----------------------------------------------------------------------------
TransitDrawFX:
	
MoveCharPtr:
	ld hl, ClearMoveChar-2
	inc hl
	inc hl
	ld (MoveCharPtr+1), hl
	
	ld d, (hl) ; X
	inc hl
	ld e, (hl) ; Y

	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld (&0000), a
	ei

	push de
	ld hl, VIDEOSEGMENT8000
	ld a, 20
	call DrawScreenTransit
	pop de	
	ld hl, VIDEOSEGMENTC000
	ld a, 21
	call DrawScreenTransit
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld (&0000), a
	ei
	
	ret
	
; ----------------------------------------------------------------------------
DrawScreenTransit:

	push af
	
	ld a, e ; Y
	or a
	jr z, SkipPosInChar
	
	ld bc, &800
AdjustPosInChar_Loop:
	add hl, bc
	dec a
	jr nz, AdjustPosInChar_Loop
	
SkipPosInChar:	

	ld a, d ; X
	or a
	jr z, SkipPosXInChar
	cp 1
	jr z, SkipPosXInChar
	inc hl	
SkipPosXInChar:

	ld d, a
IsClear:
	ld a, 0
	or a
	jr nz, SetClear

	ld bc, DrawScanlinePixelLeft
	bit 0, d
	jr z, SkipSetDrawScanlineRight
	ld bc, DrawScanlinePixelRight
SkipSetDrawScanlineRight:
	ld ( DrawScanlinePtr + 1 ), bc
	
	jr EndSet
	
SetClear:

	ld bc, ClearScanlinePixelLeft
	bit 0, d
	jr z, SkipSetClearScanlineRight
	ld bc, ClearScanlinePixelRight
SkipSetClearScanlineRight:
	ld ( DrawScanlinePtr + 1 ), bc

EndSet:

	pop af
		
DrawScanlinePtr:
	call DrawScanlinePixelLeft
	
	ret
	
; ----------------------------------------------------------------------------
DrawScanlinePixelLeft:

	ld d, a
DrawScanlinePixelLeft_CharLoop:

	ld e, 48
DrawScanlinePixelLeft_Loop:

	ld a, (hl)
	and &AA
	ld b, a
	
	res 7, h
	ld a, (hl)
	and &55
	add b
	set 7, h
	ld (hl), a

	inc hl
	inc hl

	dec e
	jr nz, DrawScanlinePixelLeft_Loop
	
	dec d
	jr nz, DrawScanlinePixelLeft_CharLoop	
	ret
	
; ----------------------------------------------------------------------------
DrawScanlinePixelRight:

	ld d, a
DrawScanlinePixelRightft_CharLoop:

	ld e, 48
DrawScanlinePixelRight_Loop:


	ld a, (hl)
	and &55
	ld b, a
	
	res 7, h
	ld a, (hl)
	and &AA
	add b
	set 7, h
	ld (hl), a

	inc hl
	inc hl

	dec e
	jr nz, DrawScanlinePixelRight_Loop
	
	dec d
	jr nz, DrawScanlinePixelRightft_CharLoop
	ret
	
; ----------------------------------------------------------------------------
ClearScanlinePixelLeft:

	ld d, a
ClearScanlinePixelLeft_CharLoop:

	ld e, 48
ClearScanlinePixelLeft_Loop:

	ld a, (hl)
	and &AA
	ld b, a
	
ClearColor1:
	ld a, 0
	;and &55
	add b
	ld (hl), a

	inc hl
	inc hl

	dec e
	jr nz, ClearScanlinePixelLeft_Loop
	
	dec d
	jr nz, ClearScanlinePixelLeft_CharLoop	
	ret
	
; ----------------------------------------------------------------------------
ClearScanlinePixelRight:

	ld d, a
ClearScanlinePixelRightft_CharLoop:

	ld e, 48
ClearScanlinePixelRight_Loop:

	ld a, (hl)
	and &55
	ld b, a
	
ClearColor2:
	ld a, 0
	;and &AA
	add b
	ld (hl), a

	inc hl
	inc hl

	dec e
	jr nz, ClearScanlinePixelRight_Loop
	
	dec d
	jr nz, ClearScanlinePixelRightft_CharLoop
	ret
	
; ----------------------------------------------------------------------------
DrawMoveChar:
	db 0,0, 2,3, 1,4, 3,2
	db 0,1, 2,4, 1,0, 3,3
	db 0,2, 2,0, 1,1, 3,4
	db 0,3, 2,1, 1,2, 3,0
	db 0,4, 2,2, 1,3, 3,1

; ----------------------------------------------------------------------------
ClearMoveChar:
	db 0,0
	db 1,0
	db 2,0
	db 3,0
	db 3,1
	db 3,2
	db 3,3
	db 3,4
	db 2,4
	db 1,4
	db 0,4
	db 0,3
	db 0,2
	db 0,1
	db 1,1
	db 2,1
	db 2,2
	db 2,3
	db 1,3
	db 1,2
	db 2,2
	db -1