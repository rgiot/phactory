
; ----------------------------------------------------------------------------
TransitCreateTables:
		ld ix, ScreenTable1
		ld hl, VIDEOSEGMENT0000
		ld de, VIDEOSEGMENT8000
		ld a, 20
		db &ed, &dd
		call AppendLines
		ld hl, VIDEOSEGMENT4000
		ld de, VIDEOSEGMENTC000
		ld a, 21
		call AppendLines		
		ret
	
; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		push af
		
		push hl
		push de
		
		ld a, 5
AppendLinesLineLoop:
		push af
		
		; FORMAT IS dst ptr (2 bytes), src ptr (2 bytes)		
		ld a, e
		ld ( ix ), a
		inc ix
		ld a, d
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
		