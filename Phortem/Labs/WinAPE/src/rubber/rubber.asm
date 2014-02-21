
		SliceWidth 		equ 10
		SliceSkip		equ 27
		
; ----------------------------------------------------------------------------
		call RubberInit
		call RubberLoop
		ret

; ----------------------------------------------------------------------------
		read "rubberDrawVerticalSlice.asm"		
		read "rubberCreateRubberMove.asm"
		read "rubberCreateRubberHide.asm"
		read "rubberDrawRubber.asm"
		
; ----------------------------------------------------------------------------
RubberInit:
		ld bc, &7F8C ; MODE 0
		out (c), c
	
		ld hl, RubberPalette
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
RubberLoop:
		call FlipScreen
		;call WaitVBL
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
		ld bc, &7F00
		out (c), c
		ld a, &54	
		;out (c), a
				
MoveX:
		;db &ed, &ff
		
		ld bc, 2
		push bc
		call DrawVerticalSliceAll
		pop bc
		
		;db &ed, &ff
		
		inc bc
		inc bc
		call DrawRubberAll
		;db &ed, &ff
		
		ld a, ( MoveXSens )
		cp 1
		jr z, RightToLeft
		ld a, (MoveX+1)
		inc a
		ld (MoveX+1), a
		cp 100
		jr nz, MoveEnd
		ld a, 1
		ld ( MoveXSens ), a
		jr MoveEnd
RightToLeft:
		ld a, (MoveX+1)
		dec a
		ld (MoveX+1), a
		cp 2
		jr nz, MoveEnd
		ld a, 2
		ld ( MoveXSens ), a
MoveEnd:
		
		ld bc, &7F00
		out (c), c
		ld a, &47
		;out (c), a
		
		jp RubberLoop
		
MoveXSens:
		db 0
		
; ----------------------------------------------------------------------------
RubberPalette:
		incbin "Rubber.cpcbitmap.palette"
		
		align 256 ; approx. &2D00
SineCurvePtr:
		incbin "RubberSineCurve1.bin"
		
		align 256
SineCurve2Ptr:
		incbin "RubberSineCurve2.bin"
		
		align 256
SineCurvePosXPtr:
		incbin "RubberSineCurvePosX.bin"
		
		align 256
SineCurvePosX2Ptr:
		incbin "RubberSineCurvePosX2.bin"
		
		align 256
RubberPrecaDataPtr2:		
		incbin "RubberPrecaData2.bin"
		
		align 256
RubberPrecaOffsetPtr:		
		incbin "RubberPrecaOffsetPtr.bin"
		
		align 256
RubberFrameSlicesPtr:
		ds 2*256
		
		align 256
RubberFramePosXPtr:
		ds 256
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &6800
		limit &7fff
RubberSprData1:
		incbin "RubberSprite.bmp.sprRawData1"
		
		align 256
RubberPrecaDataPtr:		
		incbin "RubberPrecaData1.bin"
		
write direct -1,-1, &C0
		org &E800
		limit &FFFF
RubberSprData2:
		incbin "RubberSprite.bmp.sprRawData2"
				
; ----------------------------------------------------------------------------
		read "rubberDrawRubber_LeftLeft.asm"
		read "rubberDrawRubber_RightLeft.asm"
		read "rubberDrawRubber_LeftRight.asm"
		read "rubberDrawRubber_RightRight.asm"
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C1
		org &C000
		limit &E7FF
		incbin 	"RubberBackground.bmp.bottomBin"
		
	
write direct -1,-1, &C6
		org &4080
		limit &67FF
		incbin 	"RubberBackground.bmp.topBin"

		