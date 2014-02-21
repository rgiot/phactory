
; ----------------------------------------------------------------------------
FightCreateTables:
		ld ix, ScreenTable1
		ld de, VIDEOSEGMENT8000
		ld hl, CODESEGMENT0000
		exx
		ld hl, &4000
		exx
		ld a, 20
		call AppendLines
		ld de, VIDEOSEGMENT4000
		exx
		ld hl, &C000
		exx
		ld a, 21
		call AppendLines
		
		ld ix, ScreenTable2
		ld de, VIDEOSEGMENT0000
		ld hl, CODESEGMENT0000
		exx
		ld hl, &4000
		exx
		ld a, 20
		call AppendLines
		ld de, VIDEOSEGMENT4000
		exx
		ld hl, &C000
		exx
		ld a, 21
		call AppendLines
		
		ret
	
; ----------------------------------------------------------------------------
AppendLines:	
		exx
		ld bc, &800
		ld de, 96
		exx
		
AppendLinesCharLoop:
		ex af, af'
		
		; FORMAT IS src ptr texture (2 bytes), src bg texture (2 bytes), dst ptr (2 bytes) * 5
		
		push hl
		push de		
		exx
		push hl
		exx
		
		ld iyh, 5
		
AppendLinesScanlineLoop:
		ld a, l
		ld ( ix ), a
		inc ix
		ld a, h
		ld ( ix ), a
		inc ix
		
		exx		
		ld a, l
		ld ( ix ), a
		inc ix
		ld a, h
		ld ( ix ), a
		inc ix
		
		add hl, bc
		exx
		
		ld a, e
		ld ( ix ), a
		inc ix
		ld a, d
		ld ( ix ), a
		inc ix
		
		push bc
		ld bc, &800
		ex de, hl
		add hl, bc
		ex de, hl
		pop bc

		dec iyh
		jr nz, AppendLinesScanlineLoop
		
		exx
		pop hl
		add hl, de
		exx
		pop de
		pop hl
		
		push bc
		ex de, hl
		ld bc, 96
		add hl, bc
		ex de, hl
		
		ld bc, SCR_BLOCKSWIDTH_DIV2*2 ; Left, Right
		add hl, bc
		pop bc
		
		ex af, af'
		dec a
		jr nz, AppendLinesCharLoop
		ret
		