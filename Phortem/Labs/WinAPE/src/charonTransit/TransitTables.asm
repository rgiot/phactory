
; ----------------------------------------------------------------------------
TransitInitFX:
	; A = IsClear
	; B = ClearColor
	
	ld ( IsClear + 1 ), a
	ld hl, ClearMoveChar-2
	or a
	jr z, TableSrcClear
	ld hl, DrawMoveChar-2
TableSrcClear:
	ld ( MoveCharPtr + 1), hl
	
	ld a, b
	and &55
	ld ( ClearColor1 + 1 ), a

	ld a, b
	and &AA
	ld ( ClearColor2 + 1 ), a
	
	ret