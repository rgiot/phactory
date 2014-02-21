
; ----------------------------------------------------------------------------
InitLines:
		; LOW LINES - TOP C6  BOTTOM C1
		ld hl, BANK0LINES_TOPCLIPPTR
		call _CreateClipping
		ld hl, VIDEOSEGMENT0000
		ld de, BANK0LINES_TOPPTR
		call _CreateTop
		ld hl, VIDEOSEGMENT4000
		ld de, BANK1LINES_BOTTOMPTR
		call _CreateBottom
		ld hl, BANK1LINES_BOTTOMCLIPPTR
		call _CreateClipping
		
		; HIGH LINES - TOP C6  BOTTOM C3
		ld hl, BANK2LINES_TOPCLIPPTR
		call _CreateClipping
		ld hl, VIDEOSEGMENT8000
		ld de, BANK2LINES_TOPPTR
		call _CreateTop
		ld hl, VIDEOSEGMENT4000
		ld de, BANK3LINES_BOTTOMPTR
		call _CreateBottom
		ld hl, BANK3LINES_BOTTOMCLIPPTR
		call _CreateClipping
		
		; HIGH LINES 2 - TOP C0  BOTTOM C0/C4/C5/etc
		ld hl, VIDEOSEGMENT8000
		ld de, BANK2LINES2_TOPPTR
		call _CreateTop
		ld hl, VIDEOSEGMENTC000
		ld de, BANK3LINES2_BOTTOMPTR
		jp _CreateBottom
		
; ----------------------------------------------------------------------------
InitLinesWithOffset:
		; LOW LINES - TOP C6  BOTTOM C1
		ld hl, BANK0LINES_TOPCLIPPTR
		push bc
		call _CreateClipping
		ld hl, VIDEOSEGMENT0000
		pop bc
		add hl, bc
		push bc
		ld de, BANK0LINES_TOPPTR
		call _CreateTop
		ld hl, VIDEOSEGMENT4000
		pop bc
		add hl, bc
		push bc
		ld de, BANK1LINES_BOTTOMPTR
		call _CreateBottom
		ld hl, BANK1LINES_BOTTOMCLIPPTR
		call _CreateClipping
		
		; HIGH LINES - TOP C6  BOTTOM C3
		ld hl, BANK2LINES_TOPCLIPPTR
		call _CreateClipping
		ld hl, VIDEOSEGMENT8000
		pop bc
		add hl, bc
		push bc
		ld de, BANK2LINES_TOPPTR
		call _CreateTop
		ld hl, VIDEOSEGMENT4000
		pop bc
		add hl, bc
		push bc
		ld de, BANK3LINES_BOTTOMPTR
		call _CreateBottom
		ld hl, BANK3LINES_BOTTOMCLIPPTR
		call _CreateClipping
		
		; HIGH LINES 2 - TOP C0  BOTTOM C0/C4/C5/etc
		ld hl, VIDEOSEGMENT8000
		pop bc
		add hl, bc
		push bc
		ld de, BANK2LINES2_TOPPTR
		call _CreateTop
		ld hl, VIDEOSEGMENTC000
		pop bc
		add hl, bc
		push bc
		ld de, BANK3LINES2_BOTTOMPTR
		call _CreateBottom
		pop bc
		ret
		
; ----------------------------------------------------------------------------
_CreateClipping:
		ld de, DUMMYLINEPTR
		ld a, 128
CreateClipping_Loop:
		ld ( hl ), e
		inc hl
		ld ( hl ), d
		inc hl
		dec a
		jr nz, CreateClipping_Loop
		ret
	
; ----------------------------------------------------------------------------
_CreateTop:
		ld a, 20
topCharLoop:
		push af
		
		push hl
		
		ld bc, &800
		ld a, 5
topLineLoop:
		push af
		
		ld a, l
		ld ( de ), a
		inc de
		ld a, h
		ld ( de ), a
		inc de
		
		add hl, bc
		
		pop af
		dec a
		jr nz, topLineLoop
		
		pop hl
		
		ld bc, 96
		add hl, bc
		
		pop af
		dec a
		jr nz, topCharLoop
		ret
	
; ----------------------------------------------------------------------------
_CreateBottom:
		ld a, 21
bottomCharLoop:
		push af
		
		push hl
		
		ld bc, &800
		ld a, 5
bottomLineLoop:
		push af
		
		ld a, l
		ld ( de ), a
		inc de
		ld a, h
		ld ( de ), a
		inc de
		
		add hl, bc
		
		pop af
		dec a
		jr nz, bottomLineLoop
		
		pop hl
		
		ld bc, 96
		add hl, bc
		
		pop af
		dec a
		jr nz, bottomCharLoop
		ret
