
; ----------------------------------------------------------------------------
DrawTriangle1BANK3:
		push hl
		push bc
		push de
		
		ld hl, BANK3LINES_BOTTOMPTR
		ld b, 0
		ld a, d
		sub 100
		ld c, a
		add hl, bc
		add hl, bc
		ld e, (hl)
		inc hl
		ld d, (hl)
		ld ( DrawTriangle1BANK3_ScreenPtr + 1 ), de
		
		ld h, b
		ld l, c
		add hl, hl
		add hl, bc
		ld de, DrawTriangle1BANK3_DrawScanlines
		add hl, de
		ld ( DrawTriangle1BANK3_StartY_Jump + 1 ), hl
		
		pop de
		ld a, e
		sub 100
		ld c, a
		ld h, b
		ld l, c
		add hl, hl
		add hl, bc
		ld de, DrawTriangle1BANK3_DrawScanlines
		add hl, de
		ld a, (hl)
		ld ( DrawTriangle1BANK3_EndY_RestoreJump + 1 ), hl
		ld ( DrawTriangle1BANK3_EndY_RestoreValue + 1 ), a
		ld ( hl ), &C9 ; discard scanline draw
		
		pop bc
		pop hl
		
DrawTriangle1BANK3_ScreenPtr: 
		ld de, 0
DrawTriangle1BANK3_StartY_Jump:
		call 0
		
		push hl
DrawTriangle1BANK3_EndY_RestoreJump:
		ld hl, 0
DrawTriangle1BANK3_EndY_RestoreValue:
		ld (hl), 0
		pop hl
		ret

; ----------------------------------------------------------------------------
DrawTriangle1BANK3Scanline:
		PrepareScanline
		set 7, h
		DrawScanline
		CalculateDeltaScanline
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK3Scanline_EndChar:
		PrepareScanline
		set 7, h
		DrawScanline
		CalculateDeltaScanline_EndChar
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK3_DrawScanlines:		
REPEAT 105/5
		call DrawTriangle1BANK3Scanline
		call DrawTriangle1BANK3Line1
		call DrawTriangle1BANK3Line2
		call DrawTriangle1BANK3Line3
		call DrawTriangle1BANK3Line4
REND
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK3Line1:
		set 3, d
		jp DrawTriangle1BANK3Scanline
		
; ----------------------------------------------------------------------------
DrawTriangle1BANK3Line2:
		res 3, d
		set 4, d
		jp DrawTriangle1BANK3Scanline

; ----------------------------------------------------------------------------
DrawTriangle1BANK3Line3:
		set 3, d
		jp DrawTriangle1BANK3Scanline

; ----------------------------------------------------------------------------
DrawTriangle1BANK3Line4:
		set 5, d
		res 4, d
		res 3, d		
		jp DrawTriangle1BANK3Scanline_EndChar
		