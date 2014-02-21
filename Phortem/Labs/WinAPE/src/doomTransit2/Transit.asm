				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call InitLines
		
		call TransitInit
		
		ld hl, VIDEOSEGMENT0000
		ld de, VIDEOSEGMENT8000
		ld bc, VIDEOSEGMENT0000_SIZE
		;ldir
		
		ld hl, VIDEOSEGMENT4000
		ld de, VIDEOSEGMENTC000
		ld bc, VIDEOSEGMENT4000_SIZE
		;ldir
		
		call BouncingLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"
		read "../frameworkInitLines.asm"
		
; ----------------------------------------------------------------------------
TransitInit:
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
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
		
		call TransitCreateTables
		
BouncingFrameLoop:

		call TransitDrawFX
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
BouncingFrameLoopCounter:
		ld hl, 72
		dec hl
		ld (BouncingFrameLoopCounter+1), hl
		ld a, h
		or l
		jp nz, BouncingFrameLoop
		
InfiniteLoop:
		jp InfiniteLoop
		
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"	
		
; ----------------------------------------------------------------------------
		;write "DoomTransit2FXCode.z80.bin"

		org &2800
		limit &C000
		
		jp TransitCreateTables
		jp TransitDrawFX
		
		read "TransitTables.asm"
		read "TransitDrawFX.asm"
		
		ALIGN 256
SinMove1:
		incbin "DoomTransitSin1.bin"
SinMove2:
		incbin "DoomTransitSin2.bin"
		
		ALIGN 256
YTable:
		ds 192*2

		ALIGN 256
ScreenTable1:
		ds 57*5*6
ScreenTable1End:
		
TempLine:
		ds 192
		
		;write "DeleteMe.bin"

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		limit &FFFF
		org &0080
		incbin 	"AngelAsDemon.bmp.topBin"
		org &4000
		incbin 	"AngelAsDemon.bmp.bottomBin"
		org &8080
		;incbin 	"Angel.bmp.topBin"
		org &C000
		;incbin 	"Angel.bmp.bottomBin"
		
		