
; ----------------------------------------------------------------------------
TransitCreateTables:
		ld ix, ScreenTable1
		ld de, VIDEOSEGMENT8000 + &2000
		ld hl, &0080
		ld c, &C0
		ld a, 20
		call AppendLines
		ld de, VIDEOSEGMENTC000 + &2000
		ld hl, &4000
		ld c, &C0
		ld a, 21
		call AppendLines
		
		ret
	
; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		ex af, af'
		
		ld a, e
		ld ( ix ), a
		inc ix
		ld a, d
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
		