
; ----------------------------------------------------------------------------
PhysicsCreateTables:	
		ld hl, 76
		ld ( PhysicsFrameLoopCounter + 1 ), hl
		ld a, -3
		ld ( FrameDataSkipClear + 1 ), a
		ld hl, FrameData - BallCount-BallCount
		ld ( FrameDataPtr + 1 ), hl
	
		ld iy, ScreenTableOffset
		ld bc, 0
		ld (LineOffset), bc
		ld ix, ScreenTableFlip1
		ld de, ClipLine
		ld hl, &4080
		ld c, &C4
		ld a, 13 ; 65 lines
		call AppendClipLines
		ld de, VIDEOSEGMENT8000
		ld hl, &4080
		ld c, &C4
		ld a, 20
		call AppendLines
		ld de, VIDEOSEGMENT4000
		ld hl, &C000
		ld c, &C1
		ld a, 21
		call AppendLines
		ld de, ClipLine
		ld hl, &4080
		ld c, &C4
		ld a, 13
		call AppendClipLines

		ld iy, ScreenTableOffset
		ld bc, 0
		ld (LineOffset), bc
		ld ix, ScreenTableFlip2
		ld de, ClipLine
		ld hl, &4080
		ld c, &C4
		ld a, 13 ; 65 lines
		call AppendClipLines
		ld de, VIDEOSEGMENT8000
		ld hl, &4080
		ld c, &C4
		ld a, 20
		call AppendLines
		ld de, VIDEOSEGMENT4000
		ld hl, &C000
		ld c, &C3
		ld a, 21
		call AppendLines
		ld de, ClipLine
		ld hl, &4080
		ld c, &C4
		ld a, 13
		call AppendClipLines
		
		ret

; ----------------------------------------------------------------------------
LineOffset:	
		dw 0

; ----------------------------------------------------------------------------
AppendLines:	
AppendLinesCharLoop:
		push af
		
		push hl
		push de
		
		ld a, 5
AppendLinesLineLoop:
		push af
		
		push hl
		push bc
		ld hl, (LineOffset)		
		ld ( iy ), l
		inc iy
		ld ( iy ), h
		inc iy		
		ld bc, 6
		add hl, bc
		ld (LineOffset), hl	
		pop bc
		pop hl
		
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
		
		push hl
		push bc
		ld hl, (LineOffset)		
		ld ( iy ), l
		inc iy
		ld ( iy ), h
		inc iy		
		ld bc, 6
		add hl, bc
		ld (LineOffset), hl	
		pop bc
		pop hl
		
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
		
		push hl
		push bc
		ld hl, (LineOffset)		
		ld ( iy ), l
		inc iy
		ld ( iy ), h
		inc iy		
		ld bc, 6
		add hl, bc
		ld (LineOffset), hl	
		pop bc
		pop hl
		
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