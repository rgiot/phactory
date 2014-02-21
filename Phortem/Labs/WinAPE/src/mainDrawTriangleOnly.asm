
		write "MainDrawTriangleOnly.z80.bin"
		
		nolist
		
		org &2800
		
		read "frameworkConst.asm"
		
Start:
		ld ( TriangleIsScreenFlipped ), a
		
		di
		ld a, (&0000)
		ld ( DrawTrianglesEnded_SaveBank+1 ), a
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei

		ld ix, &6880
DrawTransitLinesLoop:
		; call _WaitVBL
		
		ld b, &F5
WaitVBL_1:
		in a, (c)
		rra
		jr nc, WaitVBL_1

DrawTransitLinesInnerLoop:
		di
		ld a, (&0000)
		ld bc, &7FC4
		out (c), c
		ld c, a
		ld a, (ix)
		out (c), c
		ei
		
		cp 198
		jp z, DrawTrianglesEnded ; end of anim
		cp 199
		jp z, DrawTrianglesFrameLoop_Empty
		
		push ix
			
		di
		ld a, ( &0000 )
		ld ( TransitData_SaveBank + 1 ), a
		ld bc, &7FC4
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		ld h, ( ix + 0 ) ; start X left
		ld l, 0
		ld c, ( ix + 1 ) ; delta X left
		ld b, ( ix + 2 )
		exx
		ld h, ( ix + 3 ) ; start X right
		ld l, 0
		ld c, ( ix + 4 ) ; delta X right
		ld b, ( ix + 5 )
		exx
		ld d, ( ix + 6 ) ; start Y
		ld e, ( ix + 7 ) ; end Y
		
		ld a, ( ix + 8 ) ; triangle
		
		di
		push af
		push bc
TransitData_SaveBank:
		ld a, 0
		ld b, &7F
		ld c, a
		out (c), c
		ld ( &0000 ), a
		pop bc
		pop af
		ei
		
		or a
		push af
		call z, DrawTriangle1
		pop af
		call nz, DrawTriangle2

		pop ix
		
		ld bc, 9
		add ix, bc
		
		jp DrawTransitLinesInnerLoop
		
DrawTrianglesFrameLoop_Empty:	
		inc ix
		jp DrawTransitLinesLoop
		
DrawTrianglesEnded:
		di
DrawTrianglesEnded_SaveBank:
		ld a, 0
		ld b, &7F
		ld c, a
		out (c), c
		ld ( &0000 ), a
		ei
			
		ret

		
		read "transit\transitDrawTriangle.asm"
		
		