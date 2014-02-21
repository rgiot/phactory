
; ----------------------------------------------------------------------------
CostixDrawFX:
	ld hl, ScreenTable1
	or a
	jr z, CostixDrawFX_ScreenTableSwitchEnd
CostixDrawFX_ScreenTable2Switch:
	ld hl, ScreenTable2
CostixDrawFX_ScreenTableSwitchEnd:
	
	ld bc, CostixUnpackBuffer	
	ld iy, CostixLength
	
	ld a, 41
CostixDrawYLoop:
	push af
	
	ld a, (hl)
	inc hl
	push hl
	ld h, (hl)
	ld l, a
	;ex de, hl ; de = screen ptr at right X pos	
	
	ld a, (iy) ; length
	inc iy
	
CostixDrawXLoop:
	push af
	
	ld a, (bc)
	inc bc
	ld (CostixJump + 1 ), a
CostixJump:
	jp FXTiles
	
CostixDrawXLoopEndChar:
	res 4, h
	set 5, h
	inc hl

	pop af
	dec a
	jp nz, CostixDrawXLoop
	
	pop hl
	inc hl
	
	pop af
	dec a
	jp nz, CostixDrawYLoop:

	ret

; ----------------------------------------------------------------------------
	; FORMAT IS dst ptr (2 bytes), bank src (1 byte), src ptr (2 bytes)
ScreenTable1:
	ds 41*2
ScreenTable2:
	ds 41*2
