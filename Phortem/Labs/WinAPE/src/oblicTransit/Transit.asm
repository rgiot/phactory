				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call InitLines
		
		call TransitInit
		
		ld hl, VIDEOSEGMENT8000
		ld (hl), 255
		ld de, VIDEOSEGMENT8000+1
		ld bc, VIDEOSEGMENT0000_SIZE-1
		ldir
		
		ld hl, VIDEOSEGMENTC000
		ld (hl), 255
		ld de, VIDEOSEGMENTC000+1
		ld bc, VIDEOSEGMENT4000_SIZE-1
		ldir
		
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
		ld hl, 35
		dec hl
		ld (BouncingFrameLoopCounter+1), hl
		ld a, h
		or l
		jp nz, BouncingFrameLoop
		
InfiniteLoop:
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "TransitVertSinLinesFXCode.z80.bin"

		org &BD00
		limit &BFFF
		
		jp TransitCreateTables
		jp TransitDrawFX
		
SinTable:
		incbin "OblicTransitSin.bin"
		
		read "TransitTables.asm"
		read "TransitDrawFX.asm"
		
		;write "DeleteMe.bin"
		
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"	

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &0080
		incbin 	"AngelAsDemon.bmp.topBin"
		org &4000
		incbin 	"AngelAsDemon.bmp.bottomBin"
		