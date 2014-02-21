		
; ----------------------------------------------------------------------------
CreateRubberMove:
		di
		ld ( CreateRubberMove_SaveSP + 1 ), sp

		ld sp, RubberFrameSlicesPtr + 205 + 205
		ld bc, RubberFramePosXPtr + 205
		
		ld a, ( RubberFrameInc1 )
		ld d, a
		ld a, ( RubberFrameInc2 )
		ld e, a		
		ld a, ( RubberFrameInc3 )
		ld ( CreateRubberMove_Offset + 1 ), a
		
		ld h, SineCurvePtr / 256	

		exx
		
		ld a, ( FramePosX1 )
		ld d, a
		ld a, ( FramePosX2 )
		ld e, a
		
		ld a, ( FramePosX ) 
		ld ( CreateRubberMove_PosX + 1 ), a
		
		exx
		
		ld a, 205
CreateRubberMoveLoop:
		ex af, af'
		
		ld l, d
		inc d		
		ld a, (hl)
		
		ld l, e
		inc e		
		inc h
		add a, (hl)
		dec h
		
CreateRubberMove_Offset:
		ld l, 0		
		add a, (hl)
		
		and 127
	
		rla
		
		exx
		ld c, a	
				
		ld h, SineCurvePosXPtr / 256
				
		ld l, d
		inc d
		ld a, (hl)
		
		ld l, e
		inc e
		inc h
		add a, (hl)
		dec h
		
CreateRubberMove_PosX:
		add a, 0
		
		ld h, RubberPrecaOffsetPtr / 256
		ld l, c
		ld c, (hl)
		inc l
		ld b, (hl)		
		
		rra
		jp nc, $ + 6
		ld h, RubberPrecaDataPtr2 / 256
		db &D2 ; jp nc,
		ld h, RubberPrecaDataPtr / 256
		
		ld l, 0
		add hl, bc
		
		push hl
				
		exx
		
		dec c
		ld ( bc ), a
				
		ex af, af'
		dec a
		jp nz, CreateRubberMoveLoop
		
CreateRubberMove_SaveSP:
		ld sp, 0
		ei
		
		ret
		
		