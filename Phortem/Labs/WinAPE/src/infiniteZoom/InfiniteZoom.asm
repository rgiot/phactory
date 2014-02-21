		
; ----------------------------------------------------------------------------
		call InfiniteZoomPartInit
		jp InfiniteZoomPartLoop

; ----------------------------------------------------------------------------
InfiniteZoomPartInit:
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ret
	
; ----------------------------------------------------------------------------		
InfiniteZoomPartLoop:
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
				
		;call FlipScreen
		
		call InfiniteZoomInit
		
InfiniteZoomFrameLoop:
		;call FlipScreen
		
		;call WaitVBL
		;ld hl, &1000
		;ld de, &1000
		;ld bc, &400
		;ldir
		
		ld hl, BANK0LINES_TOPPTR
		;db &ed, &ff
		call InfiniteZoomDrawFX
		;db &ed, &ff
		
		call FlipScreen
		
		ld hl, BANK2LINES2_TOPPTR
		call InfiniteZoomDrawFX
		
		call FlipScreen
		
		;call WaitVBL
		
		jp InfiniteZoomFrameLoop
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &BA00 ; B500
		limit &BFFF
		
		;write "InfiniteZoomFXCode_B300.z80.bin"
		
		jp InfiniteZoomInit
		jp InfiniteZoomDrawFX
		
		read "InfiniteZoomInit.asm"
		read "InfiniteZoomDrawFX.asm"

		;write "DeleteMe.bin"
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &7F00
		limit &7FFF	
IntroPalette:
		incbin 	"InfiniteZoom.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
		write direct -1,-1, &C4
		limit &7FFF	
		org &4000		
Data00:
		incbin "InfiniteZoomData00.bin"
				
		org &4CBC
Data01:
		incbin "InfiniteZoomData01.bin"
				
		org &5978
Data02:
		incbin "InfiniteZoomData02.bin"
				
		org &6634
Data03:
		incbin "InfiniteZoomData03.bin"
		
		org &7300
		incbin "Z02.bmp.sprRawData1"
				
		write direct -1,-1, &C6
		limit &7FFF	
		org &4000		
Data04:
		incbin "InfiniteZoomData04.bin"
				
		org &4CBC
Data05:
		incbin "InfiniteZoomData05.bin"
				
		org &5978
Data06:
		incbin "InfiniteZoomData06.bin"
		
		org &6634
Data07:
		incbin "InfiniteZoomData07.bin"
				
		org &7300
		incbin "Z02.bmp.sprRawData1"
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &2800
		limit &3FFF	
Z00:
		incbin "Z01.bmp.sprRawData1"
		
		org &3100
BufferBottomPart1:
	ds TextureWidth * 2 * 23
BufferBottomPart1_End:
		
; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		limit &7FFF	
		
		org &7300
Z01:		
		incbin "Z02.bmp.sprRawData1"

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &E800
		limit &FFFF	
Z02:
		incbin "Z03.bmp.sprRawData1"
		
		org &F100
Z03:
		incbin "Z04.bmp.sprRawData1"	
		
		;org &FA00
		; Edges
		
; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &0080
		incbin 	"InfiniteZoomBackground.bmp.topBin"
		org &4000
		incbin 	"InfiniteZoomBackground.bmp.bottomBin"
		org &8080
		incbin 	"InfiniteZoomBackground.bmp.topBin"
		org &C000
		incbin 	"InfiniteZoomBackground.bmp.bottomBin"
		