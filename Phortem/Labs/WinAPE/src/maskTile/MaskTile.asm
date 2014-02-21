				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call TransitInit
		
		call BouncingLoop
		ret

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
		
		call FlipScreen
		
		call MaskTileDrawFX
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
InfiniteLoop:
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "TransitVertLinesFXCode_BD00.z80.bin"

		org &BD00
		limit &BFFF
		
		jp MaskTileDrawFX
		
		read "MaskTileDrawFX.asm"
		
		;write "DeleteMe.bin"
		
IntroPalette:
		incbin 	"Manga.cpcbitmap.palette"	

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &0080
		incbin 	"MangaTitleFS.bmp.topBin"
		org &2800
Tile:
		incbin 	"MangaTile2.bmp.sprRawData1"
		org &4000
		incbin 	"MangaTitleFS.bmp.bottomBin"
		