
	;write "AngelDrawFX_B900.z80.bin"
	
	org &B900
	limit &B9FF
	
; ----------------------------------------------------------------------------
AngelDrawFX:	

Counter: 
	ld hl, -1
	inc hl
	ld ( Counter + 1 ), hl
	ld bc, 154+96
	ld a, b
	cp h
	jr nz, NoResetCounter
	ld a, c
	cp l
	jr nz, NoResetCounter
	ld hl, 0
	ld ( Counter + 1 ), hl
	ld hl, AngelScrollOffset-2
	ld ( AngelScrollOffsetPtr + 1 ), hl
NoResetCounter:
		
	ld hl, ( AngelScrollOffsetPtr + 1 )
	inc hl
	inc hl
	ld ( AngelScrollOffsetPtr + 1 ), hl
	
	xor a	
DrawVerticalLinesLoop:
	push af
	
	ld b, 0
	ld c, a
	
	ld ( DrawItems_PosX + 1 ), bc
	ld ( DrawItems_PosXBottom + 1 ), bc
	
AngelScrollOffsetPtr:	
	ld hl, AngelScrollOffset-2
	add hl, bc
	add hl, bc
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	; hl = vertical line in image
	
	ld a, (hl)
	inc hl
	; a = nb items in vertical line
	
	or a
	jp z, NoItemsToDrawNoSP
	
	di
	ld ( prevSP + 1), sp
	ld sp, hl
	
	ld b, &7F
	ld c, &C0
		
DrawItems:
	ex af, af'
		
	pop hl
	
	bit 6, h
	jp nz, DrawItemsBottomStart
	; hl = char screen ptr (left)
	
DrawItems_PosX:
	ld de, 0
	add hl, de
	ld d, h
	ld e, l
	res 7, h
	set 6, h
	; de = screen ptr
	
	; Draw Line 1
	ret

NoItemsToDraw:
	ld a, ( &0000 )
	ld c, a
	out (c), c
prevSP:
	ld sp, 0
	ei
NoItemsToDrawNoSP:
	pop af
	inc a
	cp 96
	jp nz, DrawVerticalLinesLoop	
	ret

; ----------------------------------------------------------------------------
DrawItemsBottom:
	ex af, af'
	
	pop hl
		
DrawItemsBottomStart:
	
	; hl = char screen ptr (left)	
DrawItems_PosXBottom:
	ld de, 0
	add hl, de
	ld d, h
	ld e, l
	res 7, h
	; de = screen ptr
	
	; Draw Line 1
	ret
	
	;write "deleteme.bin"