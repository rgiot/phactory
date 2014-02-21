
; ----------------------------------------------------------------------------		
UnpackNextFrame:
		ld hl, (ScrollTablePtr)
		ld a, (hl)
		inc hl
		inc hl
		inc hl
		or a
		jr nz, NoResetNextFrame
		ld hl, ScrollTable
NoResetNextFrame:
		ld (ScrollTablePtr), hl
		
		di
		ld a, (&0000)
		push af
		ld a, (hl)
		inc hl
		ld ( &0000 ), a
		ld b, &7F
		ld c, a
		ld a, (hl)
		inc hl
		ld h, (hl)
		ld l, a
		out (c), c
		ei
		
		ld de, CODESEGMENT0000
		call Unpack
		
		di
		pop af
		ld ( &0000 ), a
		ld b, &7F
		ld c, a
		out (c), c
		ei
		ret

; ----------------------------------------------------------------------------
		read "../frameworkUnpack.asm"
	
	