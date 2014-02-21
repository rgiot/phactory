
; ----------------------------------------------------------------------------
DrawTriangle1BANK0:
		push hl
		ld hl, BANK0LINES_TOPPTR
		jp DrawTriangle1BANK0BANK2
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK2:
		push hl
		ld hl, BANK2LINES_TOPPTR
		
DrawTriangle1BANK0BANK2:
		push bc
		push de
		
		ld b, 0
		ld c, d
		add hl, bc
		add hl, bc
		ld e, (hl)
		inc hl
		ld d, (hl)
		ld ( DrawTriangle1BANK2_ScreenPtr + 1 ), de
		
		ld h, b
		ld l, c
		add hl, hl
		add hl, bc
		ld de, DrawTriangle1BANK2_DrawScanlines
		add hl, de
		ld ( DrawTriangle1BANK2_StartY_Jump + 1 ), hl
		
		pop de
		ld c, e
		ld h, b
		ld l, c
		add hl, hl
		add hl, bc
		ld de, DrawTriangle1BANK2_DrawScanlines
		add hl, de
		ld a, (hl)
		ld ( DrawTriangle1BANK2_EndY_RestoreJump + 1 ), hl
		ld ( DrawTriangle1BANK2_EndY_RestoreValue + 1 ), a
		ld ( hl ), &C9 ; discard scanline draw
		
		pop bc
		pop hl
		
DrawTriangle1BANK2_ScreenPtr: 
		ld de, 0
DrawTriangle1BANK2_StartY_Jump:
		call 0
		
		push hl
DrawTriangle1BANK2_EndY_RestoreJump:
		ld hl, 0
DrawTriangle1BANK2_EndY_RestoreValue:
		ld (hl), 0
		pop hl
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK2Scanline:	
		PrepareScanline
		set 6, h
		res 7, h
		DrawScanline
		CalculateDeltaScanline
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK2Scanline_EndChar:	
		PrepareScanline
		set 6, h
		res 7, h
		DrawScanline
		CalculateDeltaScanline_EndChar
		ret

; ----------------------------------------------------------------------------
DrawTriangle1BANK2_DrawScanlines:
REPEAT 100/5
		call DrawTriangle1BANK2Scanline
		call DrawTriangle1BANK2Line1
		call DrawTriangle1BANK2Line2
		call DrawTriangle1BANK2Line3
		call DrawTriangle1BANK2Line4
REND
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK2Line1:
		set 3, d
		jp DrawTriangle1BANK2Scanline

; ----------------------------------------------------------------------------
DrawTriangle1BANK2Line2:
		res 3, d
		set 4, d
		jp DrawTriangle1BANK2Scanline

; ----------------------------------------------------------------------------
DrawTriangle1BANK2Line3:
		set 3, d
		jp DrawTriangle1BANK2Scanline

; ----------------------------------------------------------------------------
DrawTriangle1BANK2Line4:
		set 5, d
		res 4, d
		res 3, d		
		jp DrawTriangle1BANK2Scanline_EndChar
