
; ----------------------------------------------------------------------------
BatmanCreateTables:	
		; 13 x 5 x 6
		ld bc, 6
		ld de, 335 ; 13+13+20+21 le tout x 5
		ld hl,  0
		ld ix, 	ScreenTablePos
		
CreateScreenTablePos:
		ld a, l
		ld ( ix ), a
		inc ix
		
		ld a, H
		ld ( ix ), a
		inc ix
		
		add hl, bc
		
		dec de
		ld a, d
		or e
		jr nz, CreateScreenTablePos

		ld ix, ScreenTable1
		ld de, ClipLine
		ld hl, &0080
		ld c, &C0
		ld a, 13
		call AppendClipLines
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
		ld de, ClipLine
		ld hl, &0080
		ld c, &C0
		ld a, 13
		call AppendClipLines
		
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
		
		pop af
		dec a
		jr nz, AppendClipLinesLineLoop
		
		pop af
		dec a
		jr nz, AppendClipLinesCharLoop
		ret