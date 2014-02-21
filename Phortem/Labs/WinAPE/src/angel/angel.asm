		
; ----------------------------------------------------------------------------
		call AngelInit
		call AngelLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"
		
; ----------------------------------------------------------------------------
AngelInit:
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ld bc, &7FC6
		out (c), c
		ld hl, &4080
		ld de, VIDEOSEGMENT8000
		ld bc, VIDEOSEGMENT8000_SIZE
		ldir
		ld bc, &7FC3
		out (c), c
		ld hl, &C000
		ld de, &4000
		ld bc, VIDEOSEGMENTC000_SIZE
		ldir
		ld bc, &7FC0
		out (c), c
		
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
		
		;call FlipScreen
		
AngelFrameLoop:
		;db &ed, &ff
		call AngelDrawFX
		;db &ed, &ff
		
		;call FlipScreen
		
		;call WaitVBL
		
		jp AngelFrameLoop
		
; ----------------------------------------------------------------------------
		read "angelDrawFX.asm"
		read "angelDrawFXMacros.asm"
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &7F00
		limit &7FFF	
IntroPalette:
		incbin 	"Angel.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
; LENS BOTTOM IN C7
write direct -1,-1, &C7
		org &4000
		incbin 	"AngelAsDemon.bmp.bottomBin"
	
; LENS TOP IN C6
write direct -1,-1, &C6
		org &4080
		incbin 	"AngelAsDemon.bmp.topBin"
		
		org &6800
		limit &7FFF
		
; DAEMON TOP IN C4
write direct -1,-1, &C4
		org &4080
		incbin 	"Angel.bmp.topBin"
		
		org &6800
		limit &7FFF
		
; ----------------------------------------------------------------------------		
write direct -1,-1, &C0
		;org &0080
		org &0100
		limit &FFF
AngelScrollOffset:
		incbin "AngelScrollOffset.bin"		

		org &600
		limit &3FFF
AngelScrollImage:
		incbin "AngelScrollImage.bin"
		
; MANGA BOTTOM 1ST PART IN C0 - CODESEGMENT8000
; MANGA BOTTOM 2ND PART IN C0 - CODESEGMENTC000
write direct -1,-1, &C0
		;org &0080
		org &4000
		limit &FFFF
		incbin 	"Angel.bmp.bottomBin"