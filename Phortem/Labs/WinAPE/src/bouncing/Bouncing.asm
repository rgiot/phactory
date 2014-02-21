				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call InitLines
		
		call BouncingInit
		
		ld hl, VIDEOSEGMENT0000
		ld de, VIDEOSEGMENT8000
		ld bc, VIDEOSEGMENT0000_SIZE
		ldir
		
		ld hl, VIDEOSEGMENT4000
		ld de, VIDEOSEGMENTC000
		ld bc, VIDEOSEGMENT4000_SIZE
		ldir
		
		call BouncingLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"
		read "../frameworkInitLines.asm"
		
; ----------------------------------------------------------------------------
BouncingInit:
		call BouncingCreateTables

		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ld bc, &7FC0
		out (c), c		
		ld hl, &8080
		ld de, &E800
		ld a, 9
		call LinearScreenCopy
		ld de, &6800
		ld a, 12
		call LinearScreenCopy
		ld bc, &7FC0
		out (c), c
		
		call ClearScreen
		
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
		;ld hl, VIDEOSEGMENT0000
		;ld ( hl ), 0
		;ld de, VIDEOSEGMENT0000+1
		;ld bc, VIDEOSEGMENT0000_SIZE-1
		;ldir
		;ld hl, VIDEOSEGMENT4000
		;ld ( hl ), 0
		;ld de, VIDEOSEGMENT4000+1
		;ld bc, VIDEOSEGMENT4000_SIZE-1
		;ldir
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
BouncingCRTC:
		db 2, 50
		db 0

; ----------------------------------------------------------------------------		
BouncingLoop:
		ld hl, BouncingCRTC
		call InitCRTC1
		
		di
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		;call FlipScreen
		
BouncingFrameLoop:

		;ld bc, &7F00+16
		;out (c), c
		;ld a, &5C	
		;out (c), a
		
		;db &ed, &ff
		ld a, (IsFlipped)
		call BouncingDrawFX
		;db &ed, &ff
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
		call FlipScreen
		
		call WaitVBL
		
BouncingFrameLoopCounter:
		ld hl, 269
		dec hl
		ld (BouncingFrameLoopCounter+1), hl
		ld a, h
		or l
		;jp nz, BouncingFrameLoop
		jp BouncingFrameLoop
		
InfiniteLoop:
		ld hl, &1000
		ld de, &1000
		ld bc, &400
		ldir
		
		;call FlipScreen
		
		call WaitVBL
		
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "BouncingFXCode_BB00.z80.bin"

		org &BB00
		limit &BFFF
		
		jp BouncingCreateTables
		jp BouncingDrawFX
		
		read "BouncingTables.asm"
		read "BouncingDrawFX.asm"
		
		;write "DeleteMe.bin"

; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &7F00
		limit &7FFF	
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C6
		org &4000
		incbin 	"AngelAsDemon.bmp.topBin"
		
		org &6800
		limit &7FFF
DeltaY2:
		incbin "BouncingDeltaY_2.bin"
DeltaY2End:
	
write direct -1,-1, &C7
		org &4000
		incbin 	"Angel.bmp.bottomBin"
		
		org &6800
		limit &7FFF
		
write direct -1,-1, &C4
		org &4080
		incbin 	"Angel.bmp.topBin"
		
		org &6800
		limit &7FFF
		
DeltaY1:
		incbin "BouncingDeltaY_1.bin"
DeltaY1End:
		
; MANGA BOTTOM 1ST PART IN C0 - CODESEGMENT8000
; MANGA BOTTOM 2ND PART IN C0 - CODESEGMENTC000
		write direct -1,-1, &C0
		org &2800
		limit &3FFF
		
		org &F900
		limit &FB6F
ScreenTable1:
		ds 205*3

		org &FB70
		limit &FFFF
ScreenTable2:
		ds 205*3

		; TEMP ! used to cleanly init screen in winape
		org &0080
		incbin 	"AngelAsDemon.bmp.topBin"
		org &4000
		incbin 	"AngelAsDemon.bmp.bottomBin"
		
		org &8080
		limit &FFFF
		incbin 	"AngelAsDemon.bmp.bottomBin"
		