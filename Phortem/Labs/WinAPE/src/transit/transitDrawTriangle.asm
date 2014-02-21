
; ----------------------------------------------------------------------------
TriangleIsScreenFlipped:
		db 0

; ----------------------------------------------------------------------------
LeftPixel:
		ld a, (de)
		and &AA
		ld b, a
		ld a, (hl)
		and &55
		or b
		ld (de), a		
		inc hl
		inc de
		inc c
		inc c
		ret

; ----------------------------------------------------------------------------
RightPixel:
		ld a, (de)
		and &55
		ld b, a
		ld a, (hl)
		and &AA
		or b
		ld (de), a
		ret

; ----------------------------------------------------------------------------
MACRO PrepareScanline	
		; de = screen ptr
		; bc = delta X
		; hl = start X
		push bc
		push de
		push hl
		;db &ed, &ff
		ld l, h
		ld h, 0
		ld b, h
		srl l ; rr l  div2
		jp c, @SkipIncB
		inc b
@SkipIncB:		
		
		; l = x div 2
		ld a, l
		add hl, de
		ex de, hl
		; screen ptr at right X pos
		
		exx
		ld d, h
		srl d
		push af
		neg
		add a, d
		add hl, bc
		exx
		
		ld c, a
		ld a, 127 ; NEG = possible opti, with array length = 256
		sub c
		add a, a ; sra plus haut, puis sla ici = pas besoin de sra+sla	
		
		ld h, d
		ld l, e
ENDM
		
; ----------------------------------------------------------------------------
MACRO DrawScanline
		ld c, a
		xor a
		cp b
		call z, LeftPixel
		ld a, c
		or a
		jp z, @SkipDrawLDI
		ld ( @DrawLDIOffset + 1 ), a		
@DrawLDIOffset:
		call DrawLDI
@SkipDrawLDI:
		
		pop af
		call c, RightPixel
@SkipScanline:
MEND

; ----------------------------------------------------------------------------
MACRO CalculateDeltaScanline
		pop hl
		pop de		
		pop bc
		add hl, bc
MEND

; ----------------------------------------------------------------------------
MACRO CalculateDeltaScanline_EndChar
		pop de
		pop hl
		ld bc, 96-&800-&800-&800-&800
		add hl, bc		
		ex de, hl
		pop bc
		add hl, bc
MEND

; ----------------------------------------------------------------------------
ALIGN 256
DrawLDI:
REPEAT 128
		ldi
REND
		ret

; ----------------------------------------------------------------------------
		read "transitDrawTriangle1.asm"
		read "transitDrawTriangle2.asm"
		