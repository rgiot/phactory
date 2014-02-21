		
; ----------------------------------------------------------------------------
CreateRubberHide:
		ld a, ( HideY )
		cp 1
		ret z
		dec a
		ld ( HideY ), a	
		
		ld b, a		
		xor a		
		ld hl, RubberFramePosXPtr		
CreateRubberHide_Loop:
		
REPEAT 5
		ld ( hl ), a
		inc l
REND

		dec b
		jr nz, CreateRubberHide_Loop
		
		ret
		
HideY:
		db 205/5
		
		