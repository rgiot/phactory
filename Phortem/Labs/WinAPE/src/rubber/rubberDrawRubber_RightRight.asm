		
		;write "DrawRubber_FF80_RightRight.z80.bin"
		
		org &FF80
		limit &FF8F
		
; ----------------------------------------------------------------------------
DrawRubber_RightRight:
		ld ( DrawRubber_RightRight_EndPtr + 1 ), bc

		ld a, (de)
		and &AA
		or (hl)
		ld (de), a
		
		inc l
		inc de
		
		jp (iy)
		
		ds &FF90-$, 0
		
; ----------------------------------------------------------------------------
		limit &FFFF
DrawRubber_RightRight_LDI:

REPEAT 40
		ldi
REND

		ld a, (de)
		and &55
		or (hl)
		ld (de), a
		
DrawRubber_RightRight_EndPtr:
		ld hl, 0
		jp (hl)
		
		;write "delete_me.bin"