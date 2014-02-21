		
; ----------------------------------------------------------------------------
CostixInit:		
		ld de, FXTilesBase
		ld bc, 16*5
		ldir

		call CreateTables
		
		;db &ed, &ff
		ld a, 13
		ld de, FXTiles		
CostixInitOpcode:
		ld hl, DrawCharBase
		ld bc, DrawCharBaseEnd-DrawCharBase
		ldir
		dec a
		jr nz, CostixInitOpcode
		
		; 4,0,1,3,2
		
		ld ix, FXTiles
		ld iy, FXTilesBase
		ld a, 13
CostixAdapTile:
		push af
		
		ld a, (iy + 4)
		ld (ix + DrawCharPixel0 - DrawCharBase + 1), a
		
		ld a, (iy + 0 )
		ld (ix + DrawCharPixel1 - DrawCharBase + 1), a
		
		ld a, (iy + 1 )
		ld (ix + DrawCharPixel2 - DrawCharBase + 1), a
		
		ld a, (iy + 3 )
		ld (ix + DrawCharPixel3 - DrawCharBase + 1), a
		
		ld a, (iy + 2 )
		ld (ix + DrawCharPixel4 - DrawCharBase + 1), a
		
		ld de, DrawCharBaseEnd-DrawCharBase
		add ix, de
		ld de, 5
		add iy, de
		
		pop af
		dec a
		jr nz, CostixAdapTile
		
		ret
		
; ----------------------------------------------------------------------------
DrawCharBase:
DrawCharPixel0:
		ld (hl), 0
		res 5, h

DrawCharPixel1:
		ld (hl), 1
		set 3, h
		
DrawCharPixel2:
		ld (hl), 2
		set 4, h

DrawCharPixel3:
		ld (hl), 3
		res 3, h

DrawCharPixel4:
		ld (hl), 4
		
		jp CostixDrawXLoopEndChar
DrawCharBaseEnd: