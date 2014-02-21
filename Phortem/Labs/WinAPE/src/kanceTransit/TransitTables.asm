
; ----------------------------------------------------------------------------
TransitCreateTables:
		ld ix, ScreenTable1
		ld de, VIDEOSEGMENT8000
		ld hl, &0080
		ld c, &C0
		ld a, 20
		call AppendLines
		ld de, VIDEOSEGMENTC000
		ld hl, &4000
		ld c, &C0
		ld a, 21
		call AppendLines
		
		ret
	
; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		ex af, af'
		
		; FORMAT IS bank src (2 bytes), dst ptr (2 bytes), src ptr (2 bytes)		
		ld a, c
		ld ( ix ), a
		inc ix		
		ld a, &7F
		ld ( ix ), a
		inc ix
		
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
		ld bc, 96
		add hl, bc
		ex de, hl
		add hl, bc
		ex de, hl
		pop bc
		
		ex af, af'
		dec a
		jr nz, AppendLinesCharLoop
		ret
		