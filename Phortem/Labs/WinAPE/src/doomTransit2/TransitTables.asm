
; ----------------------------------------------------------------------------
TransitCreateTables:
		ld ix, ScreenTable1End-1
		ld bc, &7FC0
		ld a, 8
		call AppendClippingLines
		ld de, VIDEOSEGMENT8000
		ld hl, &0080
		ld bc, &7FC0
		ld a, 20
		call AppendLines
		ld de, VIDEOSEGMENTC000
		ld hl, &4000
		ld bc, &7FC0
		ld a, 21
		call AppendLines
		ld bc, &7FC0
		ld a, 8
		call AppendClippingLines
		
		ld hl, YTable
		ld (hl), 0
		ld de, YTable + 1
		ld bc, 383 ; 192*2-1
		ldir
		
		ret
	
; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		ex af, af'
		
		; FORMAT IS src bank (2 bytes), src bg texture (2 bytes), dst ptr (2 bytes) * 5
		; hl = bg texture
		; de = scr ptr
		; bc = bank
		
		push hl
		push de		
		
		ld iyh, 5
		
AppendLinesScanlineLoop:		
		ld a, d
		ld ( ix ), a
		dec ix
		ld a, e
		ld ( ix ), a
		dec ix
		
		ld a, h
		ld ( ix ), a
		dec ix
		ld a, l
		ld ( ix ), a
		dec ix
		
		ld a, b
		ld ( ix ), a
		dec ix
		ld a, c
		ld ( ix ), a
		dec ix
		
		push bc
		ld bc, &800
		ex de, hl
		add hl, bc ; next line scr ptr
		ex de, hl
		add hl, bc ; next line bg texture
		pop bc

		dec iyh
		jr nz, AppendLinesScanlineLoop
		
		pop de
		pop hl
		
		push bc
		ex de, hl
		ld bc, 96
		add hl, bc  ; next char scr ptr
		ex de, hl
		add hl, bc ; next char bg texture
		pop bc
		
		ex af, af'
		dec a
		jr nz, AppendLinesCharLoop
		ret
		
; ----------------------------------------------------------------------------
AppendClippingLines:	
		ld de, TempLine
		
AppendClippingLinesCharLoop:
		ex af, af'
		
		; FORMAT IS src bank (2 bytes), src bg texture (2 bytes), dst ptr (2 bytes) * 5
		; hl = bg texture
		; de = scr ptr
		; bc = bank	
		
		ld iyh, 5
		
AppendClippingLinesScanlineLoop:		
		ld a, d
		ld ( ix ), a
		dec ix
		ld a, e
		ld ( ix ), a
		dec ix
		
		ld a, d
		ld ( ix ), a
		dec ix
		ld a, e
		ld ( ix ), a
		dec ix
		
		ld a, b
		ld ( ix ), a
		dec ix
		ld a, c
		ld ( ix ), a
		dec ix

		dec iyh
		jr nz, AppendClippingLinesScanlineLoop
		
		ex af, af'
		dec a
		jr nz, AppendClippingLinesCharLoop
		ret
		