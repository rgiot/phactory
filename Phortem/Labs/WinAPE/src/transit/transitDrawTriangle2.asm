	
; ----------------------------------------------------------------------------
		read "transitDrawTriangle2BANK0BANK2.asm"
		read "transitDrawTriangle2BANK1.asm"
		read "transitDrawTriangle2BANK3.asm"
			
; ----------------------------------------------------------------------------
; d = start Y (absolute coordinate)
; e = end Y (absolute coordinate)
; hl = start X Left (Fixed)
; de = delta X Left (Fixed)
; hl' = start X Right (Fixed)
; de' = delta X Right (Fixed)
DrawTriangle2:
		ld a, (TriangleIsScreenFlipped)
		or a
		jp z, DrawTriangle2_BANK2BANK3

; ----------------------------------------------------------------------------
DrawTriangle2_BANK0BANK1:
		ld a, d ; d = start Y
		cp 100
		jp nc, DrawTriangle2_BANK1Only
		ld a, e ; e = end Y
		cp 100
		jp c, DrawTriangle2_BANK0Only
		
		push de
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle2_BANK0BANK1_SaveBank + 1 ), a
		ld bc, &7FC4
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		ld e, 100 ; absolute position, top y screen being from 0 to 99
		call DrawTriangle2BANK0
		pop de
		
		ld d, 100
		
		push bc
		di
DrawTriangle2_BANK0BANK1_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		pop bc
		call DrawTriangle2BANK1
		
		ret
		
DrawTriangle2_BANK0Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle2_BANK0Only_SaveBank + 1 ), a
		ld bc, &7FC4
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle2BANK0
		di
DrawTriangle2_BANK0Only_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		ret
		
DrawTriangle2_BANK1Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle2_BANK1Only_SaveBank + 1 ), a
		ld bc, &7FC0
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle2BANK1
		di
DrawTriangle2_BANK1Only_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		ret
		
; ----------------------------------------------------------------------------
DrawTriangle2_BANK2BANK3:
		ld a, e ; e = end Y
		cp 100
		jp c, DrawTriangle2_BANK2Only
		ld a, d ; d = start Y
		cp 100
		jp nc, DrawTriangle2_BANK3Only
		
		push de
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle2_BANK2BANK3_SaveBank + 1 ), a
		ld bc, &7FC4
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		ld e, 100 ; absolute position, top y screen being from 0 to 99
		call DrawTriangle2BANK2
		pop de
		
		ld d, 100
		
		push bc
		di
DrawTriangle2_BANK2BANK3_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		pop bc
		call DrawTriangle2BANK3
		
		ret
		
DrawTriangle2_BANK2Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle2_BANK2Only_SaveBank + 1 ), a
		ld bc, &7FC4
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle2BANK2
		di
DrawTriangle2_BANK2Only_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		ret
		
DrawTriangle2_BANK3Only:
		push bc
		di
		ld a, ( &0000 )
		ld ( DrawTriangle2_BANK3Only_SaveBank + 1 ), a
		ld bc, &7FC0
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei
		pop bc
		call DrawTriangle2BANK3
		di
DrawTriangle2_BANK3Only_SaveBank:
		ld a, &C0
		ld b, &7F
		ld c, a
		out (c), c		
		ld ( &0000 ), a
		ei	
		ret
		
		