		
; ----------------------------------------------------------------------------
		call CostixPartInit
		call CostixPartLoop
		ret

; ----------------------------------------------------------------------------
CostixUnpackBuffer equ &2800

; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"
		read "../frameworkUnpack.asm"
		
; ----------------------------------------------------------------------------
CostixPartInit:
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ret
		
; ----------------------------------------------------------------------------
UnpackCostixFrame:

CostixFrame:
	ld a, 0
	ld a, -1
	inc a	
	cp 32
	jr nz, NoCostixFrameReset
	xor a
NoCostixFrameReset:	
	ld ( CostixFrame + 1 ), a

	ld hl, &4000
	ld bc, &300
	
	cp 16
	jr nc, FromBankC6
	
FromBankC4:
	
	or a
	jr z, FromBankC4_End
FromBankC4_CalcPtr:
	add hl, bc
	dec a
	jr nz, FromBankC4_CalcPtr	
FromBankC4_End:
	ld bc, &7FC4
	jr UnpackPtrFound

FromBankC6:	
	sub 16

	or a
	jr z, FromBankC6_End
FromBankC6_CalcPtr:
	add hl, bc
	dec a
	jr nz, FromBankC6_CalcPtr	
FromBankC6_End:
	ld bc, &7FC6
	
UnpackPtrFound:
	; HL = src
	; BC = bank
		
	out (c), c
	
	;db &ed, &ff
	ld de, CostixUnpackBuffer
	call Unpack
	
	ld bc, &7FC0
	out (c), c
	
	ret

; ----------------------------------------------------------------------------		
CostixPartLoop:
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
		
		ld hl, FXTilesBase
		call &BC00
		
CostixFrameLoop:

		call UnpackCostixFrame
		ld a, 1
		;db &ed, &ff
		call &BC03
		;db &ed, &ff
		call FlipScreen
		
		call UnpackCostixFrame
		xor a
		call &BC03
		call FlipScreen
		
		jp CostixFrameLoop
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &BC00
		limit &BFFF
		
		;write "CostixFXCode_BC00.z80.bin"
		
		jp CostixInit
		jp CostixDrawFX
		
		read "costixInit.asm"
		read "costixDrawFX.asm"
		read "costixTables.asm"
		
		ALIGN 256
FXTiles:
		ds 273
		
CostixLength:
		incbin "CostixLength.bin"
CostixLeftX:
		incbin "CostixLeftX.bin"

		;write "DeleteMe.bin"

FXTilesBase:
		incbin "CostixFXTiles.bmp.sprRawData1"
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &7F00
		limit &7FFF	
IntroPalette:
		incbin 	"Costix.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
		limit &7FFF	
write direct -1,-1, &C4
		org &4000
CostixData:
		incbin "CostixData.bigfile"		

		limit &7FFF	
write direct -1,-1, &C6
		org &4000
CostixData2:
		incbin "CostixData2.bigfile"
		
		limit &FFFF	
write direct -1,-1, &C0
		org &0080
		incbin 	"Costix.bmp.topBin"
write direct -1,-1, &C0
		org &4000
		incbin 	"Costix.bmp.bottomBin"
write direct -1,-1, &C0
		org &8080
		incbin 	"Costix.bmp.topBin"
write direct -1,-1, &C0
		org &C000
		incbin 	"Costix.bmp.bottomBin"
