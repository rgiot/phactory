
; ----------------------------------------------------------------------------
FireWorksCreateTables:
		ld de, ScreenTablePtrC1
		ld hl, VIDEOSEGMENT8000
		call FireWorksCreateTop
		ld hl, VIDEOSEGMENTC000
		call FireWorksCreateBottom
		
		ld de, ScreenTablePtrC3
		ld hl, VIDEOSEGMENT0000
		call FireWorksCreateTop
		ld hl, VIDEOSEGMENT4000
		call FireWorksCreateBottom
		ret
	
; ----------------------------------------------------------------------------
FireWorksCreateTop:
		ld a, 20
fw_topCharLoop:
		push af
		
		push hl
		
		ld bc, &800
		ld a, 5
fw_topLineLoop:
		push af
		
		ld a, l
		ld ( de ), a
		inc d
		ld a, h
		ld ( de ), a
		dec d
		inc e
		
		add hl, bc
		
		pop af
		dec a
		jr nz, fw_topLineLoop
		
		pop hl
		
		ld bc, 96
		add hl, bc
		
		pop af
		dec a
		jr nz, fw_topCharLoop
		ret
	
; ----------------------------------------------------------------------------
FireWorksCreateBottom:
		ld a, 21
fw_bottomCharLoop:
		push af
		
		push hl
		
		ld bc, &800
		ld a, 5
fw_bottomLineLoop:
		push af
		
		ld a, l
		ld ( de ), a
		inc d
		ld a, h
		ld ( de ), a
		dec d
		inc e
		
		add hl, bc
		
		pop af
		dec a
		jr nz, fw_bottomLineLoop
		
		pop hl
		
		ld bc, 96
		add hl, bc
		
		pop af
		dec a
		jr nz, fw_bottomCharLoop
		ret
