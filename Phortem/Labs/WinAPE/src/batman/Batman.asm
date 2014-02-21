				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call BatmanInit
		call BatmanLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"
		
; ----------------------------------------------------------------------------
BatmanInit:
		call BatmanCreateTables

		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ld bc, &7FC0
		out (c), c		
		ld hl, &0080
		ld de, &E800
		ld a, 9
		call LinearScreenCopy
		ld de, &6800
		ld a, 12
		call LinearScreenCopy
		ld bc, &7FC0
		out (c), c
		
		call ClearScreen
		
		ld ix, ScreenTable1
		;call DrawAllLines
		
		ld ix, ScreenTable2
		;call DrawAllLines
		
		ret
		
; ----------------------------------------------------------------------------	
LinearScreenCopy:
LinearScreenCopy_BlockLoop:
		push af
		
		push hl
		
		ld a, 5
LinearScreenCopy_LineLoop:
		push af
		
		ld bc, 96
		ldir
		
		ld bc, &800-96
		add hl, bc
		
		pop af
		dec a
		jr nz, LinearScreenCopy_LineLoop	
		
		pop hl
		ld bc, 96
		add hl, bc
		
		pop af
		dec a
		jr nz, LinearScreenCopy_BlockLoop
		ret
		
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
BatmanCRTC:
		db 2, 49
		db 0

; ----------------------------------------------------------------------------		
BatmanLoop:
		ld hl, BatmanCRTC
		call InitCRTC1

		ld bc, &7F00
		out (c), c
		ld a, &54	
		out (c), a
		
		di
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		;call FlipScreen
		
BatmanFrameLoop:

		ld bc, &7F00+16
		out (c), c
		ld a, &5C	
		out (c), a
		
		;db &ed, &ff
		call BatmanDrawFX
		;db &ed, &ff
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
		call FlipScreen
		
		call WaitVBL
		
BatmanFrameLoopCounter:
		ld hl, 269*2
		dec hl
		ld (BatmanFrameLoopCounter+1), hl
		ld a, h
		or l
		jp nz, BatmanFrameLoop
		
InfiniteLoop:
		ld hl, &1000
		ld de, &1000
		ld bc, &400
		ldir
		
		call FlipScreen
		
		call WaitVBL
		
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "BatmanFXCode.z80.bin"

		org &BB00
		limit &BFFF
		
		jp BatmanCreateTables
		jp BatmanDrawFX
		
		read "BatmanTables.asm"
		read "BatmanDrawFX.asm"
		
		;write "DeleteMe.bin"

; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &2B00
		limit &3FFF
		
CircleFullData:
		incbin "BatmanCircleFull.bin"
CircleFullDataEnd:
		
; ----------------------------------------------------------------------------
; FORMAT IS dst ptr (2 bytes), bank src (1 byte), src ptr (2 bytes)
ScreenTable1:
		ds 335*6

ScreenTable2:
		ds 335*6
		
ScreenTablePos:
		ds 335*2
		
ClipLine:
		ds 98
		
		ALIGN 256
CircleData:
		ds 122

; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &7F00
		limit &7FFF	
IntroPalette:
		incbin 	"Batman1.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C6
		org &4000
		incbin 	"Batman1.bmp.bottomBin"
		
		org &6800
BatmanCircleClipRight:
		incbin "BatmanCircleClipRight.bin"
		limit &7FFF
	
write direct -1,-1, &C7
		org &4080
		incbin 	"Batman2.bmp.topBin"
		
		org &6800
		limit &7FFF
		
write direct -1,-1, &C4
		org &4080
		incbin 	"Batman1.bmp.topBin"
		
		org &6800
BatmanCircleClipLeft:
		incbin "BatmanCircleClipLeft.bin"
		limit &7FFF
		
; MANGA BOTTOM 1ST PART IN C0 - CODESEGMENT8000
; MANGA BOTTOM 2ND PART IN C0 - CODESEGMENTC000
write direct -1,-1, &C0
		org &0080
		limit &FFFF
		incbin 	"Batman2.bmp.bottomBin"
		
		org &FB00
MoveXY:
		incbin "BatmanCircleMoveXY.bin"
MoveXYEnd: