				
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
		call WaitVBL
		
		ld hl, &1000
		ld de, &1000
		ld bc, &2000
		ldir
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
BouncingFrameLoopCounter:
		ld hl, 19
		dec hl
		ld (BouncingFrameLoopCounter+1), hl
		ld a, h
		or l
		jp BouncingFrameLoop
		
InfiniteLoop:
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "CubeTransitFXCode_2800.z80.bin"

		org &2800
		limit &2BFF
		
		jp TransitCreateTables
		jp TransitDrawFX
		
		read "TransitTables.asm"
		read "TransitDrawFX.asm"
		
		;write "DeleteMe.bin"
		
		
		limit &FFFF
		
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"	

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &0080
		incbin 	"AngelAsDemon.bmp.topBin"
		org &4000
		incbin 	"AngelAsDemon.bmp.bottomBin"
		