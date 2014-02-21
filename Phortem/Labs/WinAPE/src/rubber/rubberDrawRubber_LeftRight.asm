		
		;write "DrawRubber_FD80_LeftRight.z80.bin"
		
		org &FD80
		limit &FD8F
		
; ----------------------------------------------------------------------------
DrawRubber_LeftRight:		
		ld ( DrawRubber_LeftRight_EndPtr + 1 ), bc

		jp (iy)
		
		ds &FD90-$, 0
		
; ----------------------------------------------------------------------------
		limit &FDFF
DrawRubber_LeftRight_LDI:

REPEAT 40
		ldi
REND

		ld a, (de)
		and &55
		or (hl)
		ld (de), a
		
DrawRubber_LeftRight_EndPtr:
		ld hl, 0
		jp (hl)
		
		;write "delete_me.bin"