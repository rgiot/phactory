
; ----------------------------------------------------------------------------
TransitCreateTables1:
		ld ix, ScreenTable1
		ld hl, &7B60 ; Y offset
		ld de, VIDEOSEGMENT8000
		ld a, 20
		call AppendLines

		ld ( TransitCreateTables2_StartHL + 1 ), hl
		
		push ix
		pop hl
		ld ( TransitCreateTables2_StartIX + 2 ), hl
		ret
		

TransitCreateTables2:
TransitCreateTables2_StartHL:
		ld hl, 0
TransitCreateTables2_StartIX:
		ld ix, 0
		ld de, VIDEOSEGMENTC000
		ld a, 21
		jp AppendLines
	
; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		ex af, af'
		
		push bc
		push de
		
		ld a, 5
AppendLinesCharLoop_BlockLoop:
		push af
		
		; FORMAT IS bank src (2 bytes), dst ptr (2 bytes), src ptr (2 bytes)		
AppendLines_BankStart:
		ld a, &C4
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
		
		ld bc, 32
		add hl, bc
		ld a, h
		cp &80
		jr nz, AppendLines_NoOverflow
		ld hl, &7800
		ld a, (AppendLines_BankStart+1)
		cp &C4
		jr z, AppendLines_BottomTile
		
		ld a, &C4
		ld (AppendLines_BankStart+1), a
		jr AppendLines_NoOverflow
		
AppendLines_BottomTile:
		ld a, &C6
		ld (AppendLines_BankStart+1), a
		
AppendLines_NoOverflow:
		
		push bc		
		ex de, hl
		ld bc, &800
		add hl, bc
		ex de, hl
		pop bc
		
		pop af
		dec a
		jr nz, AppendLinesCharLoop_BlockLoop
		
		ex de, hl
		ld bc, 96
		pop hl
		add hl, bc
		ex de, hl
		pop bc
		
		ex af, af'
		dec a
		jr nz, AppendLinesCharLoop
		ret
		