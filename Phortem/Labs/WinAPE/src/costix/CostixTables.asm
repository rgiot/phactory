
; ----------------------------------------------------------------------------
CreateTables:
		ld iy, CostixData
		ld bc, CostixLeftX		
		ld ix, ScreenTable1
		ld hl, VIDEOSEGMENT8000
		ld a, 20
		call AppendLines
		ld hl, VIDEOSEGMENTC000
		ld a, 21
		call AppendLines
		
		ld iy, CostixData
		ld bc, CostixLeftX		
		ld ix, ScreenTable2
		ld hl, VIDEOSEGMENT0000
		ld a, 20
		call AppendLines
		ld hl, VIDEOSEGMENT4000
		ld a, 21
		call AppendLines
		
		ret
	
; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		ex af, af'
		
		push hl
		
		ld a, (bc)
		ld e, a
		inc bc
		ld d, 0
		
		add hl, de		
		
		ld de, &2000
		add hl, de
		
		; FORMAT IS dst ptr (2 bytes)
		ld a, l
		ld ( ix ), a
		inc ix
		ld a, h
		ld ( ix ), a
		inc ix
		
		pop hl
		push bc
		ld bc, 96
		add hl, bc
		pop bc
		
		ex af, af'
		dec a
		jr nz, AppendLinesCharLoop
		ret
		