		
; 001 line 1 / set 3,h
; 011 line 3 / set 4,h
; 010 line 2 / res 3,h
; 110 line 6 / set 5,h
; 100 line 4 / res 4,h
; 101 line 5 / set 3,h
; 111 line 7 / set 4,h
	
; ----------------------------------------------------------------------------
MACRO PrepareScanline
		exx		
		
		ld a, (bc)
		inc c
		
		ld e, (hl)
		inc l
		ld d, (hl)
		inc hl
		
		ex de, hl
				
		di
		ld ( $ + 14 ), sp
		ld sp, hl
		ex de, hl			
		exx
				
		pop ix ; raster line display ptr (type 0-3)				
		pop iy ; skip length ptr (jump to right LDI uses)
		pop de ; get scanline spr ptr, already includes PosX				
		pop bc ; screen ptr leftX offset (jump to right screen address)
		
		ld sp, 0
		ei
ENDM

MACRO ExecScanline
		ld ( $ + 17 ), hl
		
		or a
		jp z, $ + 15 ; maybe introduce wait time..
		add a, c
		ld c, a
		
		add hl, bc
				
		ex de, hl
		ld bc, $ + 5
		
		jp (ix)
		
		ld hl, 0
ENDM

; ----------------------------------------------------------------------------
RubberFrameInc1:
		db 80
RubberFrameInc2:
		db 180
RubberFrameInc3:
		db 80
FramePosX:
		db 0
FramePosX1:
		db 128-120
FramePosX2:
		db 0-120
		
; ----------------------------------------------------------------------------
InitRubberAll:
		ld a, c
		ld (FramePosX), a
		jp CreateRubberMove

; ----------------------------------------------------------------------------
DrawRubberAll:
		exx
		ld bc, RubberFramePosXPtr+25
		ld hl, RubberFrameSlicesPtr
		exx
		
		call GetTopPtr		
		ld a, 100/5
		push bc
		call DrawRubber
		pop bc
		call GetBottomPtr
		ld a, 105/5	
		call DrawRubber
		
		ld a, (RubberFrameInc1)
RubberFrameInc1Add:
		add a, -5
		ld (RubberFrameInc1), a
		ld a, (RubberFrameInc2)
RubberFrameInc2Add:
		add a, 6
		ld (RubberFrameInc2), a
		ld a, (RubberFrameInc3)
RubberFrameInc3Add:
		add a, -5
		ld (RubberFrameInc3), a
		
		ld a, (FramePosX1)
FramePosX1Add:
		add a, -15
		ld ( FramePosX1), a
		ld a, (FramePosX2)
FramePosX2Add:
		add a, -10
		ld ( FramePosX2), a
		
		ld a, ( &3F )
		or a
		ret z
		
		ld a, (RubberFrameInc1Add+1)
		or a
		jr z, SkipRubberFrameInc1Add
		inc a
		ld (RubberFrameInc1Add+1), a
SkipRubberFrameInc1Add:
		
		ld a, (RubberFrameInc2Add+1)
		or a
		jr z, SkipRubberFrameInc2Add
		dec a
		ld (RubberFrameInc2Add+1), a
SkipRubberFrameInc2Add:
		
		ld a, (RubberFrameInc3Add+1)
		or a
		jr z, SkipRubberFrameInc3Add
		inc a
		ld (RubberFrameInc3Add+1), a
SkipRubberFrameInc3Add:
		
		ld a, (FramePosX1Add+1)
		or a
		jr z, SkipFramePosX1Add
		inc a
		ld (FramePosX1Add+1), a
SkipFramePosX1Add:
		
		ld a, (FramePosX2Add+1)
		or a
		jr z, SkipFramePosX2Add
		inc a
		ld (FramePosX2Add+1), a
SkipFramePosX2Add:
		
		ret

; ----------------------------------------------------------------------------
DrawRubber:			
		;add hl, bc ; add X
		
DrawRubberInnerLoop:
		ex af, af'		
		
		; line 0		
		PrepareScanline
		ExecScanline
		
		; line 1
		PrepareScanline
		set 3, h
		ExecScanline
		
		; line 2
		PrepareScanline	
		res 3, h
		set 4, h
		ExecScanline
		
		; line 3
		PrepareScanline	
		set 3, h
		ExecScanline
		
		; line 4
		PrepareScanline	
		set 5, h
		res 4, h
		res 3, h
		ExecScanline
				
		ld bc, 96-&800-&800-&800-&800
		add hl, bc	
		
		ex af, af'
		dec a
		jp nz, DrawRubberInnerLoop
		
		ret
		