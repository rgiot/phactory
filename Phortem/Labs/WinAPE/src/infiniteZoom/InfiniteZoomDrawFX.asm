
; ----------------------------------------------------------------------------
TextureWidth equ 48
TextureHeight equ 48

; ----------------------------------------------------------------------------
MACRO DoScanline
	; &3A-&28 = &12  
	; 3 bits available to shift right in h	
	
	exx
	pop hl
	
	ld a, h
	and %11100000
	rlca
	rlca
	set 7, a
	ld ( @Add ), a
	
	ld a, h
	and %00011111 ; can be removed, cf. b contains the bits
	
@Add:
	nop ; add a, b  .. add a, e
	
	ld h, a
	ld a, (hl)
	exx
	
	ld (de), a
	inc de
MEND
	
; ----------------------------------------------------------------------------
InfiniteZoomDrawFX:
	ld bc, 8*2 ; 2*4 + 2*2
	
SwitchLine:
	ld a, 3
	or a
	jr z, SwitchLine1
	dec a
	jr z, SwitchLine2
	dec a
	jr z, SwitchLine3
	
	ld a, 2
	jr SwitchLineEnd
	
SwitchLine3:
	ld a, 1
	jr SwitchLineEnd
	
SwitchLine2:	
	jr SwitchLineEnd

SwitchLine1:
	;inc c
	;inc c
	ld a, 3
	
SwitchLineEnd:
	ld ( SwitchLine + 1 ), a
	
	add hl, bc

	push hl
	pop iy
	
	ld ix, LineEdges

DataTableSrc:
	ld hl, DataTable
	ld a, (hl)
	or a
	jr nz, DataTable_SkipReset
	
	ld a, (TextureOrder+1)
	inc a
	cp 4
	jr nz, TextureOrder_NoReset
	xor a
TextureOrder_NoReset:
	ld (TextureOrder+1), a
	
	ld hl, DataTable
	ld a, (hl)
DataTable_SkipReset:
	inc hl
	ld e, (hl)
	inc hl
	ld d, (hl)
	inc hl
	
;ld hl, DataTable + 6
	ld ( DataTableSrc + 1 ), hl
	
	ld ( DataTable_Ptr + 1 ), de
	ld ( DataTable_Bank + 1 ), a
	
	di
	
	ld a, ( &0000 )
	push af
DataTable_Bank:
	ld a, &C0
	ld ( &0000 ), a
	ld b, &7F
	ld c, a
	out (c), c
	
	ld ( DataTable_OldSP + 1 ), sp
DataTable_Ptr:
	ld sp, 0
	ld de, &8080
	
	exx	
TextureOrder:
	ld a, 0
	
	or a
	jr z, TextureOrder0
	dec a
	jr z, TextureOrder1
	dec a
	jr z, TextureOrder2
	
TextureOrder3:
	ld b, Z03/256
	ld c, Z00/256
	ld d, Z01/256
	ld e, Z02/256
	jr TextureOrder_End
	
TextureOrder2:
	ld b, Z02/256
	ld c, Z03/256
	ld d, Z00/256
	ld e, Z01/256
	jr TextureOrder_End
	
TextureOrder1:
	ld b, Z01/256
	ld c, Z02/256
	ld d, Z03/256
	ld e, Z00/256
	jr TextureOrder_End
	
TextureOrder0:
	ld b, Z00/256
	ld c, Z01/256
	ld d, Z02/256
	ld e, Z03/256
TextureOrder_End:	
	exx
	
	xor a
	ld (DrawDataTable_BottomDone+1), a
	
	ld a, 23
DrawDataTable_YLoop:
	ex af, af'
	
	ld l, (iy)
	inc iy
	ld h, (iy)
	inc iy
	
	;db &ed, &ff
	;pop de
	ld e, (ix)
	inc ix
	ld a, (ix)
	inc ix
	
	or a
	jp z, SkipAll
	
	ld  (Size1+1), a
	ld  (Size2+1), a
	ld  (Size3+1), a
	
	ld c, a
	ld d, 0
	ld ( Offset1 + 1 ), de
	ld ( Offset2 + 1 ), de
	ld ( Offset3 + 1 ), de
	add hl, de
	
	ex de, hl
	
	ld ( BlockCopySrc + 1 ), de
	
LoopScanline1:
	;REPEAT 48
	DoScanline
	;REND
	dec c
	jr nz, LoopScanline1
	
	ei
	pop hl
	di
	push hl
	
BlockCopySrc:
	ld de, 0
	
	ld l, (iy)
	inc iy
	ld h, (iy)
	inc iy	
	
Offset1:
	ld bc, 0
	add hl, bc
	ex de, hl
	
	;ld b, 0	
Size1:
	ld c, 0
	;ldir
	xor a
	sub c
	and 64-1
	add a, a

	ld ( memcpy_initPadding1 + 1 ), a
	ld ( memcpy_initPadding2 + 1 ), a
	ld ( memcpy_initPadding3 + 1 ), a
memcpy_initPadding1:
	jr memcpy_initPadding1
REPEAT 64
	ldi
REND	
	
	ei
	pop hl
	di
	push hl

	ld de, (BlockCopySrc+1)
	ld l, (iy)
	inc iy
	ld h, (iy)
	inc iy	
	
Offset2:
	ld bc, 0
	add hl, bc
	ex de, hl
	
	;ld b, 0	
Size2:
	ld c, 0	
	
	;ldir
memcpy_initPadding2:
	jr memcpy_initPadding2
REPEAT 64
	ldi
REND
	ei
	pop hl
	di
	push hl

	ld de, (BlockCopySrc+1)
	ld l, (iy)
	inc iy
	ld h, (iy)
	inc iy	
Offset3:
	ld bc, 0
	add hl, bc
	ex de, hl
	
	;ld b, 0	
Size3:
	ld c, 0	
	
	;ldir	
memcpy_initPadding3:
	jr memcpy_initPadding3
REPEAT 64
	ldi
REND

	ei
	pop hl
	di
	push hl
	
SkipAll:

	ex af, af'
	dec a
	jp nz, DrawDataTable_YLoop
	
	;db &ed, &ff
DrawDataTable_BottomDone:
	ld a, 0
	cp 1
	jr z, DrawDataTable_End
	inc a
	ld (DrawDataTable_BottomDone+1), a
	
	;di
	ld a, (DataTable_Bank+1)
	ld ( &0000 ), a
	ld b, &7F
	ld c, a
	out (c), c
	
	ld ( SrcCopy+1 ), sp
	
	ld sp, (DataTable_OldSP+1)
	ei
	
SrcCopy:
	ld hl, 0
	ld de, BufferBottomPart1	
	ld bc, BufferBottomPart1_End - BufferBottomPart1
	jp OptimizedLDIR
CopyEnd1:
	
	di
	ld (DataTable_OldSP+1), sp
	ld sp, BufferBottomPart1
	
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld (&0000), a
	
	ld a, 25
	jp DrawDataTable_YLoop
	
DrawDataTable_End:
	
DataTable_OldSP:
	ld sp, 0
	
	pop af
	ld ( &0000 ), a
	ld b, &7F
	ld c, a
	out (c), c
	ei	
	ret
	
OptimizedLDIR:
	xor a
	sub c
	and 64-1
	add a, a

	ld ( memcpy_initPadding + 1 ), a
memcpy_initPadding:
	jr nz, memcpy_initPadding

memcpy_loop:
REPEAT 64
	ldi
REND
	jp pe, memcpy_loop	
	jp CopyEnd1