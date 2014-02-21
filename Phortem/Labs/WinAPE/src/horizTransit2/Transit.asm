				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call InitLines
		
		call TransitInit
		
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
BouncingLoop:
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
		;call FlipScreen
		
		;call TransitDrawFX
		;call WaitVBL
		;call FlipScreen
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
BouncingFrameLoopCounter:
		ld a, 36
		dec a
		ld (BouncingFrameLoopCounter+1), a
		jp nz, BouncingFrameLoop
		
InfiniteLoop:
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "TransitHoriz2FXCode.z80.bin"

		org &BE00
		limit &BFFF
		
		jp TransitCreateTables
		jp TransitDrawFX
		
		read "TransitTables.asm"
		read "TransitDrawFX.asm"

		;write "DeleteMe.bin"
		
; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &2800

		;write "TransitHorizLinesFXData_2800.z80.bin"

		;write "DeleteMe.bin"

		; ----------------------------------------------------------------------------
	; FORMAT IS bank dst ptr (2 bytes), src ptr (2 bytes)
	
ScreenTable1:
	ds 205*4
		
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &0080
		limit &7FFF
		incbin 	"AngelAsDemon.bmp.topBin"
		org &4000
		incbin 	"AngelAsDemon.bmp.bottomBin"
		