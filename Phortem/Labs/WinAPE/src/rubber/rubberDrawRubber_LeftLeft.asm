		
		;write "DrawRubber_FC80_LeftLeft.z80.bin"
		
		org &FC80
		limit &FC8F
		
; ----------------------------------------------------------------------------
DrawRubber_LeftLeft:		
		ld ( DrawRubber_LeftLeft_EndPtr + 1 ), bc
		
		jp (iy)
		
		ds &FC90-$, 0
		
; ----------------------------------------------------------------------------
		limit &FCFF
DrawRubber_LeftLeft_LDI:

REPEAT 40
		ldi
REND
				
DrawRubber_LeftLeft_EndPtr:
		ld hl, 0
		jp (hl)
		
		;write "delete_me.bin"