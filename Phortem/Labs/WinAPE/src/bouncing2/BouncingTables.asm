
; ----------------------------------------------------------------------------
BouncingCreateTables:
		ld ix, ScreenTable1
		ld hl, &4080
		ld c, &C4
		ld a, 20
		call AppendLines
		ld hl, &C000
		ld c, &C1
		ld a, 21
		call AppendLines
		
		ld ix, ScreenTable2
		ld hl, &4000
		ld c, &C6
		ld a, 20
		call AppendLines
		ld hl, &E800
		ld c, &C0
		ld a, 9
		call AppendBottomLines
		ld hl, &6800
		ld a, 12
		jp AppendBottomLines
	
; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		push af
		
		push hl
		push de
		
		ld a, 5
AppendLinesLineLoop:
		push af
		
		; FORMAT IS bank src (1 byte), src ptr (2 bytes)		
		ld a, c
		ld ( ix ), a
		inc ix	
		
		ld a, l
		ld ( ix ), a
		inc ix
		ld a, h
		ld ( ix ), a
		inc ix
		
		push bc
		ld bc, &800
		ex de, hl
		add hl, bc
		ex de, hl
		add hl, bc
		pop bc
		
		pop af
		dec a
		jr nz, AppendLinesLineLoop
		
		pop de
		pop hl
		push bc
		ld bc, 96
		ex de, hl
		add hl, bc
		ex de, hl
		add hl, bc
		pop bc
		
		pop af
		dec a
		jr nz, AppendLinesCharLoop
		ret
		
; ----------------------------------------------------------------------------
AppendBottomLines:	
AppendBottomLinesCharLoop:
		push af
		
		push de
		
		ld a, 5
AppendBottomLinesLineLoop:
		push af
		
		; FORMAT IS bank src (1 byte), src ptr (2 bytes)		
		ld a, c
		ld ( ix ), a
		inc ix		
		
		ld a, l
		ld ( ix ), a
		inc ix
		ld a, h
		ld ( ix ), a
		inc ix
		
		ld a, d
		add a, &08
		ld d, a
		
		push bc
		ld bc, 96
		add hl, bc
		pop bc
		
		pop af
		dec a
		jr nz,AppendBottomLinesLineLoop
		
		pop de
		push bc
		ld bc, 96
		ex de, hl
		add hl, bc
		ex de, hl
		pop bc
		
		pop af
		dec a
		jr nz, AppendBottomLinesCharLoop
		ret
	
; ----------------------------------------------------------------------------
AppendClipLines:	
AppendClipLinesCharLoop:
		push af
		
		ld a, 5
AppendClipLinesLineLoop:
		push af
		
		; FORMAT IS bank src (1 bytes), src ptr (2 bytes)		
		ld a, c
		ld ( ix ), a
		inc ix		
		
		ld a, l
		ld ( ix ), a
		inc ix
		ld a, h
		ld ( ix ), a
		inc ix
		
		pop af
		dec a
		jr nz, AppendClipLinesLineLoop
		
		pop af
		dec a
		jr nz, AppendClipLinesCharLoop
		ret
		