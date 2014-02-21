				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call BatmanInit
		call BatmanLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"
		
; ----------------------------------------------------------------------------
BatmanInit:
		call BatmanCreateTables

		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ret
; ----------------------------------------------------------------------------		
BatmanLoop:
		di
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		;call FlipScreen
		
BatmanFrameLoop:

		;db &ed, &ff
		call WaitVBL
		call BatmanDrawFX
		;db &ed, &ff
		
		ld a, 1
WaitNextCircle:
		push af
		call WaitVBL
		ld hl, &1000
		ld de, &1000
		ld bc, &300
		ldir
		pop af
		dec a
		jr nz, WaitNextCircle
		
BatmanFrameLoopCounter:
		ld hl, 269*2
		dec hl
		ld (BatmanFrameLoopCounter+1), hl
		ld a, h
		or l
		jp nz, BatmanFrameLoop
		
InfiniteLoop:		
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "TransitCircleFXCode.z80.bin"

		org &3B00
		limit &BFFF
		
		jp BatmanCreateTables
		jp BatmanDrawFX
		
		read "BatmanTables.asm"
		read "BatmanDrawFX.asm"
		
		;write "DeleteMe.bin"

; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &2800
		limit &3FFF
		
; ----------------------------------------------------------------------------
; FORMAT IS dst ptr (2 bytes), bank src (1 byte), src ptr (2 bytes)
ScreenTable1:
		ds 335*6
		
ScreenTablePos:
		ds 335*2
		
ClipLine:
		ds 98
		
		ALIGN 256
CircleData:
		ds 122

; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &7F00
		limit &7FFF	
IntroPalette:
		incbin 	"Batman1.cpcbitmap.palette"		
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C6
		org &6800
		limit &7FFF
		
		org &6800
		limit &7FFF
		
write direct -1,-1, &C0
		org &0080
		incbin 	"Batman1.bmp.topBin"
		org &4000
		incbin 	"Batman1.bmp.bottomBin"
		
; ----------------------------------------------------------------------------
		org &6800		
		limit &7FFF
		
		;write "TransitCircleFXData6800.z80.bin"
		
BatmanCircleClipLeft:
		incbin "BatmanCircleClipLeft.bin"
CircleFullData:
		incbin "BatmanCircleFull.bin"
CircleFullDataEnd:

		;write "DeleteMe.bin"
		
; ----------------------------------------------------------------------------
		org &E800		
		limit &FFFF
		
		;write "TransitCircleFXDataE800.z80.bin"
		
BatmanCircleClipRight:
		incbin "BatmanCircleClipRight.bin"

		;write "DeleteMe.bin"
