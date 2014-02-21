				
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
		
		call TransitCreateTables1
		call TransitCreateTables2
		
BouncingFrameLoop:

		call TransitDrawFX
		call TransitDrawFX
		call TransitDrawFX
		call FlipScreen
		call WaitVBL
		
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
BouncingFrameLoopCounter:
		ld a, 205
		dec a
		ld (BouncingFrameLoopCounter+1), a
		jp nz, BouncingFrameLoop
		
InfiniteLoop:
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "TransitHorizLinesFXCode_F8E0.z80.bin"

		org &F8E0
		;limit &BFFF
		limit &FAFF
		
		jp TransitCreateTables1
		jp TransitCreateTables2
		jp TransitDrawFX
		
		read "TransitTables.asm"
		read "TransitDrawFX.asm"

		;write "DeleteMe.bin"
		
; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &2800

		;write "TransitHorizLinesFXData_2800.z80.bin"
		
HorizTransit:
		incbin 	"HorizTransit_data.bin"	
HorizTransitEnd:

		;write "DeleteMe.bin"

		; ----------------------------------------------------------------------------
	; FORMAT IS bank src (2 bytes), dst ptr (2 bytes), src ptr (2 bytes)		
ScreenTable1:
	ds 205*5
		
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"	
		
		; TOP
		write direct -1,-1, &C4
		org &7800
		incbin 	"AngelTile.bmp.sprRawData1"
		
		; BOTTOM
		write direct -1,-1, &C6
		org &7000
		incbin 	"AngelTile.bmp.sprRawData1"

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &0080
		incbin 	"AngelAsDemon.bmp.topBin"
		org &4000
		incbin 	"AngelAsDemon.bmp.bottomBin"
		