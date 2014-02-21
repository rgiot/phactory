		
; ----------------------------------------------------------------------------
		call AngelInit
		call AngelLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkInitLines.asm"
		read "../frameworkTiles.asm"
		
; ----------------------------------------------------------------------------
		read "angelDrawFX.asm"
		
; ----------------------------------------------------------------------------
AngelInit:
		call InitLines
		
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ld bc, &7FC0
		out (c), c
	
		; ANGEL BOTTOM 1ST PART IN C0 - CODESEGMENT8000
		ld hl, VIDEOSEGMENT0000
		ld de, CODESEGMENT4000 + &800
		ld bc, &1000
		ldir
		
		; ANGEL BOTTOM 2ND PART IN C0 - CODESEGMENTC000
		ld hl, VIDEOSEGMENT0000 + &1000
		ld de, CODESEGMENTC000
		ld bc, &1800
		ldir
		
		;call ClearScreen
		ld bc, &7FC4
		out (c), c
		ld hl, &4080
		ld de, VIDEOSEGMENT8000
		ld bc, &2800
		ldir
		ld bc, &7FC0
		out (c), c
		
		ret
		
; ----------------------------------------------------------------------------	
ClearScreen:
		ld hl, VIDEOSEGMENT0000
		ld ( hl ), 255
		ld de, VIDEOSEGMENT0000+1
		ld bc, VIDEOSEGMENT0000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENT4000
		ld ( hl ), 255
		ld de, VIDEOSEGMENT4000+1
		ld bc, VIDEOSEGMENT4000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENT8000
		ld ( hl ), 255
		ld de, VIDEOSEGMENT8000+1
		ld bc, VIDEOSEGMENT8000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENTC000
		ld ( hl ), 255
		ld de, VIDEOSEGMENTC000+1
		ld bc, VIDEOSEGMENTC000_SIZE-1
		ldir
		ret
		
; ----------------------------------------------------------------------------		
AngelLoop:
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
		
AngelFrameLoop:
		call AngelDrawFX
		
		;call FlipScreen
		
		call WaitVBL
		
		jp AngelFrameLoop
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &6800
		limit &7FFF	
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
; LENS BOTTOM IN C7
write direct -1,-1, &C7
		org &4000
		incbin 	"Angel.bmp.bottomBin"
	
; LENS TOP IN C6
write direct -1,-1, &C6
		org &4080
		incbin 	"Angel.bmp.topBin"
		
; MANGA TOP IN C4
write direct -1,-1, &C4
		org &4080
		incbin 	"AngelAsDemon.bmp.topBin"
		
; MANGA BOTTOM 1ST PART IN C0 - CODESEGMENT8000
; MANGA BOTTOM 2ND PART IN C0 - CODESEGMENTC000
write direct -1,-1, &C0
		org &0080
		incbin 	"AngelAsDemon.bmp.bottomBin"
		
; ----------------------------------------------------------------------------
	write direct -1,-1, &C0
	org CODESEGMENT0000

; ----------------------------------------------------------------------------
PosXData:
	incbin 	"AngelPosX.raw"	
PosXData_End:
PosXDataPtr:
	dw PosXData-2

; ----------------------------------------------------------------------------
FrameTablePtr:
	dw FrameTable-2
PosX:
	dw 0
	
; ----------------------------------------------------------------------------
	ALIGN 256
FrameTable:
REPEAT 100
	dw 192
REND
FrameTable2ndPart:
REPEAT 100
	dw 192
REND
FrameTable_End: