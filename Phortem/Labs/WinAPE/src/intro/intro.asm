		
; ----------------------------------------------------------------------------
		call IntroInit
		call IntroLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkInitLines.asm"
		read "../frameworkTiles.asm"
		
; ----------------------------------------------------------------------------
IntroInit:
		call InitLines
		
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ld bc, &7FC6
		out (c), c
		
		ld hl, &4080
		ld de, &8080
		ld bc, &2800-&80
		ldir
		
		ld hl, &4080
		ld de, &0080
		ld bc, &2800-&80
		ldir
		
		ld bc, &7FC7
		out (c), c
		
		ld hl, &4000
		ld de, &C000
		ld bc, &2800
		ldir
		
		ld bc, &7FC0
		out (c), c
		
		ld hl, &C000
		ld de, &4000
		ld bc, &2800
		ldir
		
		ret
		
		
; ----------------------------------------------------------------------------		
IntroLoop:
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
		
IntroFrameLoop:
		
		call FlipScreen
		
		call WaitVBL
		
		jp IntroFrameLoop
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &6800
		limit &7FFF	
IntroPalette:
		incbin 	"Intro.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C1
		org &C000
		limit &E7FF
		incbin 	"IntroBackground.bmp.bottomBin"
	
write direct -1,-1, &C6
		org &4080
		limit &67FF
		incbin 	"IntroBackground.bmp.topBin"
		