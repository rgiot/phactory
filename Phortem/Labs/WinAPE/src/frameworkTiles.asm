		
; ----------------------------------------------------------------------------
SetPalette8:
		ld e, 8

setPaletteBeginFrom8:
		push de
		push hl

		ld a, (hl)
		ld l, a

		ld b, &7f
		ld c, e
		di
		out (c), c
		ld c, l
		out (c), c
		ei
		
		pop hl
		pop de
		
		inc hl

		inc e
		ld a, e
		cp 16
		jr nz, setPaletteBeginFrom8
		ret
		
; ----------------------------------------------------------------------------
DrawTiles:
		ld ( DrawTileSpr1 + 1 ), hl
		ld ( DrawTileSpr2 + 1 ), hl
		
		ex de, hl
		ld ( DrawTileTableDestPtr + 1 ), hl
		
		xor a
		call DrawTileBand
		ld a, 32
		call DrawTileBand
		ld a, 64
		jp DrawTileBand		
DrawTileBand:
		ld h, 0
		ld l, a
		ld ( DrawTilePosX + 1 ), hl

		exx
DrawTileSpr1:
		ld hl, 0
		ld bc, 26*32
		add hl, bc
		ld ( SpriteSrc + 1 ), hl
		ld b, 26
		exx
		
DrawTileTableDestPtr:
		ld hl, 0
		ld a, 205
DrawTileYLoop:
		push af
		
		ld a, (hl)
		inc hl
		push hl
		ld h, (hl)
		ld l, a
DrawTilePosX:
		ld bc, 0
		add hl, bc
		ex de, hl
		
		exx
		ld a, b
		inc a
		cp 128
		jr nz, NoReset
		xor a	
DrawTileSpr2:
		ld hl, 0
		ld ( SpriteSrc + 1 ), hl
NoReset:
		ld b, a
		exx
		
SpriteSrc:
		ld hl, 0
		
REPEAT 32 
	ldi
REND

		ld ( SpriteSrc + 1 ), hl
		
		pop hl
		inc hl	
		pop af
		dec a
		jp nz, DrawTileYLoop
		ret
	
; ----------------------------------------------------------------------------
ShiftTile:
		ld e, 128
ShiftTileLoop:	
REPEAT 32
		set 0, (hl) ; add +8 in color index	(pixel 1)
		set 1, (hl) ; add +8 in color index	(pixel 0)
		inc hl	
REND

		inc e		
		jp nz, ShiftTileLoop
		ret
		