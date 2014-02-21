		
		;write "DrawRubber_FE80_RightLeft.z80.bin"
		
		org &FE80
		limit &FE8F
		
; ----------------------------------------------------------------------------
DrawRubber_RightLeft:
		ld ( DrawRubber_RightLeft_EndPtr + 1 ), bc
		
		ld a, (de)
		and &AA
		or (hl)
		ld (de), a
		
		inc l
		inc de
		
		jp (iy)
		
		ds &FE90-$, 0
		
; ----------------------------------------------------------------------------
		limit &FEFF
DrawRubber_RightLeft_LDI:

REPEAT 40
		ldi
REND
		
DrawRubber_RightLeft_EndPtr:
		ld hl, 0
		jp (hl)
		
		;write "delete_me.bin"