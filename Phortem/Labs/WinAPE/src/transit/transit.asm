		
; ----------------------------------------------------------------------------
		call TransitInit
		call TransitLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkInitLines.asm"
		read "../frameworkTiles.asm"
		
; ----------------------------------------------------------------------------
TransitInit:
		call InitLines
		
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, LensPalette
		call SetPalette
		
		ld hl, MangaData
		call ShiftTile	
		ld hl, MangaPalette
		call SetPalette8
		
		ld hl, LensData
		ld de, BANK2LINES2_TOPPTR
		call DrawTiles
		
		;db &Ed, &ff
		
		; LENS BOTTOM IN C7
		ld bc, &7FC7
		out (c), c
		
		ld hl, VIDEOSEGMENTC000
		ld de, VIDEOSEGMENT4000
		ld bc, VIDEOSEGMENTC000_SIZE
		ldir
		
		; LENS TOP IN C6
		ld bc, &7FC6
		out (c), c
		
		ld hl, VIDEOSEGMENT8000
		ld de, VIDEOSEGMENT0000+&4000
		ld bc, VIDEOSEGMENT8000_SIZE
		ldir
		
		ld bc, &7FC0
		out (c), c
		
		ld hl, MangaData
		ld de, BANK2LINES2_TOPPTR
		call DrawTiles
		
		; MANGA TOP IN C4
		ld bc, &7FC4
		out (c), c
		
		ld hl, VIDEOSEGMENT8000
		ld de, VIDEOSEGMENT0000+&4000
		ld bc, VIDEOSEGMENT8000_SIZE
		ldir
		
		; MANGA BOTTOM C0
		ld bc, &7FC0
		out (c), c
		
		; PREPARE MANGA BOTTOM COPY
		ld hl, VIDEOSEGMENTC000
		ld de, VIDEOSEGMENT4000
		ld bc, VIDEOSEGMENTC000_SIZE
		ldir
		
		; MANGA BOTTOM 1ST PART IN C0 - CODESEGMENT8000
		ld hl, VIDEOSEGMENT4000
		ld de, VIDEOSEGMENTC000 - &800-&800-&4000 ; CODESEGMENT4000 + &800 = &7000
		ld bc, &1000
		ldir
		
		; MANGA BOTTOM 2ND PART IN C0 - CODESEGMENTC000
		ld hl, VIDEOSEGMENT4000 + &1000
		ld de, VIDEOSEGMENTC000 + &800+&800+&800+&800+&800 ; CODESEGMENTC000 = &E800
		ld bc, &1800
		ldir
		
		call DrawLens
		ret
		
DrawLens:

		ld a, 2
Loop:
		push af
		; LENS BOTTOM IN C7
		ld bc, &7FC7
		out (c), c
		
		ld de, VIDEOSEGMENTC000
		ld hl, VIDEOSEGMENTC000
		ld bc, VIDEOSEGMENTC000_SIZE
		ldir
		
		; LENS TOP IN C6
		ld bc, &7FC6
		out (c), c
		pop af
		dec a
		jr nz, Loop

		ld a, 1
DrawLensLoop:
		push af
		
		; LENS BOTTOM IN C7
		ld bc, &7FC7
		out (c), c
		
		ld de, VIDEOSEGMENTC000
		ld hl, VIDEOSEGMENT4000
		ld bc, VIDEOSEGMENTC000_SIZE
		ldir
		
		; LENS TOP IN C6
		ld bc, &7FC6
		out (c), c
		
		ld de, VIDEOSEGMENT8000
		ld hl, VIDEOSEGMENT0000+&4000
		ld bc, VIDEOSEGMENT8000_SIZE
		ldir
		
		ld bc, &7FC0
		out (c), c
		
		ld de, VIDEOSEGMENT0000
		ld hl, VIDEOSEGMENT8000
		ld bc, VIDEOSEGMENT8000_SIZE
		ldir
		
		ld de, VIDEOSEGMENT4000
		ld hl, VIDEOSEGMENTC000
		ld bc, VIDEOSEGMENTC000_SIZE
		ldir
		
		pop af
		dec a
		jr nz, DrawLensLoop
		
		ret

		
		;ld hl, VIDEOSEGMENTC000
		;ld de, VIDEOSEGMENT4000
		;ld bc, VIDEOSEGMENTC000_SIZE
		;ldir
		
		;ld hl, LensData
		;ld de, BANK0LINES_TOPPTR
		;call DrawTiles
		;ld hl, LensData
		;ld de, BANK2LINES2_TOPPTR
		;call DrawTiles
		;ret
		
; ----------------------------------------------------------------------------	
ClearScreen:
		ld hl, VIDEOSEGMENT0000
		ld ( hl ), 0
		ld de, VIDEOSEGMENT0000+1
		ld bc, VIDEOSEGMENT0000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENT4000
		ld ( hl ), 0
		ld de, VIDEOSEGMENT4000+1
		ld bc, VIDEOSEGMENT4000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENT8000
		ld ( hl ), 0
		ld de, VIDEOSEGMENT8000+1
		ld bc, VIDEOSEGMENT8000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENTC000
		ld ( hl ), 0
		ld de, VIDEOSEGMENTC000+1
		ld bc, VIDEOSEGMENTC000_SIZE-1
		ldir
		ret

; ----------------------------------------------------------------------------		
StreamAndDrawTriangles:
		ld a, (IsFlipped)
		ld ( TriangleIsScreenFlipped), a
		
DrawTrianglesFrameLoop:
		ld bc, &7FC4
		out (c), c
		ld a, (ix)
		cp 198
		jp nz, DrawTrianglesAnimLoop_NoReset
		ld bc, &7FC0
		out (c), c
		;call ClearScreen
		call DrawLens
		
		ld bc, &7FC4
		out (c), c
		ld ix, TransitData
		ld a, ( ix )
DrawTrianglesAnimLoop_NoReset:
		ld bc, &7FC0
		out (c), c
		cp 199 ; endTag
		jp z, DrawTrianglesFrameLoop_Empty
		
		push ix
		
		ld bc, &7FC4
		out (c), c
		
		ld h, ( ix + 0 ) ; start X left
		ld l, 0
		ld c, ( ix + 1 ) ; delta X left
		ld b, ( ix + 2 )
		exx
		ld h, ( ix + 3 ) ; start X right
		ld l, 0
		ld c, ( ix + 4 ) ; delta X right
		ld b, ( ix + 5 )
		exx
		ld d, ( ix + 6 ) ; start Y
		ld e, ( ix + 7 ) ; end Y
		
		ld a, ( ix + 8 ) ; triangle
		
		push bc
		ld bc, &7FC0
		out (c), c
		pop bc
		
		or a
		push af
		call z, DrawTriangle1
		pop af
		call nz, DrawTriangle2
	
		pop ix
		
		ld bc, 9
		add ix, bc
		
		jp DrawTrianglesFrameLoop
		
DrawTrianglesFrameLoop_Empty:	
		inc ix
		
		
		;ld ix, TransitData
		ret
		
; ----------------------------------------------------------------------------		
TransitLoop:
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
		di
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		ld ix, TransitData
		
DrawTrianglesAnimLoop:
		
		;call FlipScreen ;;;;;
		
		call WaitVBL

		;call ClearScreen

		call StreamAndDrawTriangles
		
		;call StreamAndDrawTriangles
		
		jp DrawTrianglesAnimLoop
		
MoveXSens:
		db 0
		
; ----------------------------------------------------------------------------
		read "transitDrawTriangle.asm"
		
; ----------------------------------------------------------------------------		
write direct -1,-1, &C4
		org &6800
TransitData:
		incbin "TransitLinesFX3.bin"
		limit &7FFF
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &6800
		limit &7FFF
LensData:
		incbin 	"LensTile.bmp.sprRawData1"		
LensPalette:
		incbin 	"LensTile.cpcbitmap.palette"		
	
write direct -1,-1, &C0
		org &E800
		limit &FFFF
MangaData:
		incbin 	"MangaTile.bmp.sprRawData1"		
MangaPalette:
		incbin 	"MangaTile.cpcbitmap.palette"

		