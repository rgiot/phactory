
; ----------------------------------------------------------------------------
		read "transitDrawTriangle1BANK0BANK2.asm"
		read "transitDrawTriangle1BANK1.asm"
		read "transitDrawTriangle1BANK3.asm"
		
; ----------------------------------------------------------------------------
; d = start Y (absolute coordinate)
; e = end Y (absolute coordinate)
; hl = start X Left (Fixed)
; bc = delta X Left (Fixed)
; hl' = start X Right (Fixed)
; bc' = delta X Right (Fixed)
DrawTriangle1:
		ld a, (TriangleIsScreenFlipped)
		or a
		jp z, DrawTriangle1_BANK2BANK3

; ----------------------------------------------------------------------------
DrawTriangle1_BANK0BANK1:
		ld a, d ; d = start Y
		cp 100
		jp nc, DrawTriangle1_BANK1Only
		ld a, e ; e = end Y
		cp 100
		jp c, DrawTriangle1_BANK0Only
		
		push de
		push bc
		
		di
		ld a, ( &0000 )
		ld ( DrawTriangle1_BANK0BANK1_SaveBank + 1 ), a
		ld bc, &7FC6
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		
		pop bc
		ld e, 100 ; absolute position, top y screen being from 0 to 99
		call DrawTriangle1BANK0
		pop de
		
		ld d, 100
		
		push bc
		
		di
		ld bc, &7FC1
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei		
		
		pop bc
		call DrawTriangle1BANK1
		
		di
DrawTriangle1_BANK0BANK1_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei		
		
		ret
		
DrawTriangle1_BANK0Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle1_BANK0Only_SaveBank + 1 ), a
		ld bc, &7FC6
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle1BANK0
		
		di
DrawTriangle1_BANK0Only_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		
		ret
		
DrawTriangle1_BANK1Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle1BANK1_SaveBank + 1 ), a
		ld bc, &7FC1
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle1BANK1
		di
DrawTriangle1BANK1_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle1_BANK2BANK3:
		ld a, e ; e = end Y
		cp 100
		jp c, DrawTriangle1_BANK2Only
		ld a, d ; d = start Y
		cp 100
		jp nc, DrawTriangle1_BANK3Only
		
		push de
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle1_BANK2BANK3_SaveBank + 1 ), a
		ld bc, &7FC6
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		ld e, 100 ; absolute position, top y screen being from 0 to 99
		call DrawTriangle1BANK2
		pop de
		
		ld d, 100
		
		push bc
		di
		ld bc, &7FC3
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei	
		pop bc
		call DrawTriangle1BANK3
		
		di
DrawTriangle1_BANK2BANK3_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		
		ret
		
DrawTriangle1_BANK2Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle1_BANK2Only_SaveBank + 1 ), a
		ld bc, &7FC6
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle1BANK2
		di
DrawTriangle1_BANK2Only_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		ret
		
DrawTriangle1_BANK3Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle1_BANK3Only_SaveBank + 1 ), a
		ld bc, &7FC3
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle1BANK3
		di
DrawTriangle1_BANK3Only_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		ret

		