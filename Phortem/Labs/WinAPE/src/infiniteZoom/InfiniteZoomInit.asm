		
; ----------------------------------------------------------------------------
InfiniteZoomInit:		
		
	ld bc, 24+16
	jp InitLinesWithOffset
		
; ----------------------------------------------------------------------------
	read "../frameworkInitLines.asm"
	
; ----------------------------------------------------------------------------
DataTable:
		db &C4
		dw Data00
		
		db &C4
		dw Data01
		
		db &C4
		dw Data02
		
		db &C4
		dw Data03
		
		db &C6
		dw Data04
		
		db &C6
		dw Data05
		
		db &C6
		dw Data06
		
		db &C6
		dw Data07
		
		db 0
		
; ----------------------------------------------------------------------------
LineEdges:
		incbin "InfiniteZoomLineEdges.bin"