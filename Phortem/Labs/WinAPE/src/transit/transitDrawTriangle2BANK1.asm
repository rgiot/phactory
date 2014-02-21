
; ----------------------------------------------------------------------------
DrawTriangle2BANK1:
		push hl
		push bc
		push de
		
		ld hl, BANK1LINES_BOTTOMPTR
		ld b, 0
		ld a, d
		sub 100
		ld c, a
		add hl, bc
		add hl, bc
		ld e, (hl)
		inc hl
		ld d, (hl)
		ld ( DrawTriangle2BANK1_ScreenPtr + 1 ), de
		
		ld h, b
		ld l, c
		add hl, hl
		add hl, bc
		ld de, DrawTriangle2BANK1_DrawScanlines
		add hl, de
		ld ( DrawTriangle2BANK1_StartY_Jump + 1 ), hl
		
		pop de
		ld a, e
		sub 100
		ld c, a
		ld h, b
		ld l, c
		add hl, hl
		add hl, bc
		ld de, DrawTriangle2BANK1_DrawScanlines
		add hl, de
		ld a, (hl)
		ld ( DrawTriangle2BANK1_EndY_RestoreJump + 1 ), hl
		ld ( DrawTriangle2BANK1_EndY_RestoreValue + 1 ), a
		ld ( hl ), &C9 ; discard scanline draw
		
		pop bc
		pop hl
		
DrawTriangle2BANK1_ScreenPtr: 
		ld de, 0
DrawTriangle2BANK1_StartY_Jump:
		call 0
		
		push hl
DrawTriangle2BANK1_EndY_RestoreJump:
		ld hl, 0
DrawTriangle2BANK1_EndY_RestoreValue:
		ld (hl), 0
		pop hl
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle2BANK1Scanline1stPart:
		PrepareScanline
		; & C000
		res 7, h ; &4000
		set 5, h ; &6000
		set 4, h ; &6800
		DrawScanline
		CalculateDeltaScanline
		ret

; ----------------------------------------------------------------------------
DrawTriangle2BANK1Scanline2ndPart:
		PrepareScanline
		; &C000
		ex af, af'
		ld a, h
		add a, &E8-&50
		ld h, a
		ex af, af'
		; &E800
		DrawScanline
		CalculateDeltaScanline
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle2BANK1Scanline_EndChar:
		PrepareScanline
		; &C000
		ex af, af'
		ld a, h
		add a, &E8-&50
		ld h, a
		ex af, af'
		; &E800
		DrawScanline
		CalculateDeltaScanline_EndChar
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle2BANK1_DrawScanlines:		
REPEAT 105/5
		call DrawTriangle2BANK1Scanline1stPart
		call DrawTriangle2BANK1Line1
		call DrawTriangle2BANK1Line2
		call DrawTriangle2BANK1Line3
		call DrawTriangle2BANK1Line4
REND
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle2BANK1Line1:
		set 3, d
		jp DrawTriangle2BANK1Scanline1stPart
		
; ----------------------------------------------------------------------------
DrawTriangle2BANK1Line2:
		res 3, d
		set 4, d
		jp DrawTriangle2BANK1Scanline2ndPart

; ----------------------------------------------------------------------------
DrawTriangle2BANK1Line3:
		set 3, d
		jp DrawTriangle2BANK1Scanline2ndPart

; ----------------------------------------------------------------------------
DrawTriangle2BANK1Line4:
		set 5, d
		res 4, d
		res 3, d		
		jp DrawTriangle2BANK1Scanline_EndChar
		
		