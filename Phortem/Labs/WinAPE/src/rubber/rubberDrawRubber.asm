		
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
		
		ld e, (hl)
		inc l
		ld d, (hl)
		inc hl
		
		ex de, hl
		ld sp, hl
		ex de, hl		
		
		ld a, (bc)
		inc c
		
		exx
				
		pop ix ; raster line display ptr (type 0-3)				
		pop iy ; skip length ptr (jump to right LDI uses)
		pop de ; get scanline spr ptr, already includes PosX				
		pop bc ; screen ptr leftX offset (jump to right screen address)
MEND

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
MEND

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
DrawRubberAll:
		ld a, c
		ld (FramePosX), a
		
		call CreateRubberMove
		call CreateRubberHide
		
		exx
		ld bc, RubberFramePosXPtr
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
		add a, -4
		ld (RubberFrameInc1), a
		ld a, (RubberFrameInc2)
		add a, 3
		ld (RubberFrameInc2), a
		ld a, (RubberFrameInc3)
		add a, -5
		ld (RubberFrameInc3), a
		
		ld a, (FramePosX1)
		add a, -10
		ld ( FramePosX1), a
		ld a, (FramePosX2)
		add a, -16
		ld ( FramePosX2), a
		
		ret

; ----------------------------------------------------------------------------
DrawRubber:			
		;add hl, bc ; add X
		
		di
		ld ( DrawRubber_SaveSP + 1 ), sp		
		
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
		
DrawRubber_SaveSP:
		ld sp, 0
		ei
		
		ret
		
		