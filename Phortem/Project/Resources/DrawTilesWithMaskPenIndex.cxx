
// ----------------------------------------------------------------------------
#ifndef _DRAWTILESMASKPENINDEX_CXX_
#define _DRAWTILESMASKPENINDEX_CXX_

// ----------------------------------------------------------------------------
#define MASKTYPE_NOMASK 0
#define MASKTYPE_MASK 1

// ----------------------------------------------------------------------------
void DrawTilesWithMaskPenIndex(char maskType, char *topPtr, char *bottomPtr, char *tilePtr, char maskPenIndex)
{
	maskType;
	topPtr;
	bottomPtr;
	tilePtr;
	maskPenIndex;

__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	;ld a, (ix+11) ; mask pen index
	;ld iyh, a
	
	ld a, (ix+4) ; maskType
	
	ld e, (ix+5) ; topPtr
	ld d, (ix+6)
	
	ld c, (ix+7) ; bottomPtr
	ld b, (ix+8)

	ld l, (ix+9) ; tilePtr
	ld h, (ix+10)
	
	ld ( BottomPtr + 1 ), bc
	
	ld bc, TreatPixelMask
	or a
	jr nz, FullCopy
	ld bc, TreatPixelNoMask
FullCopy:
	ld ( TreatPixelPtr + 1 ), bc
	
	ld a, h
	add a, &10
	ld ( MaxTilePtrH + 1 ), a
	
	ld bc, 26*32
	add hl, bc
	
	push hl
	exx
	pop hl
	ld d, &AA
	ld e, &55
	exx
	
	ld iyl, 0	
	ld b, 20 ; Nb Chars
TileYLoop:

	ld c, 5 ; Char Y Size
TileCharYLoop:
	push bc
	push de

	ld b, 3 ; Nb Bands	
TileBandLoop:	
	push bc
	exx
	push hl
	ld c, &C3 ; was FF .. C3 = pen 9 left, pen 9 right
	exx
TreatPixelPtr:
	call TreatPixelMask
	exx
	pop hl
	exx
	pop bc
	dec b
	jr nz, TileBandLoop
	
	pop de	
	ld a, d
	add a, 8
	ld d, a
	
	exx
	ld bc, 32 ; Texture Offset
	add hl, bc
	ld a, h
MaxTilePtrH:
	cp 0
	jr nz, NoTilePtrReset	
	add a, &F0
	ld h, a
NoTilePtrReset:
	exx
	
	pop bc
	dec c
	jr nz, TileCharYLoop
	
	push bc
	ex de, hl
	ld bc, &D860 ; 96 - &800 *5
	add hl, bc
	ex de, hl
	pop bc
	
	dec b
	jr nz, TileYLoop
	
	ld a, iyl
	or a
	jr nz, DrawTiles_End
	
	ld iyl, 1
	ld b, 21
BottomPtr:
	ld de, 0
	jr TileYLoop
	
; ----------------------------------------------------------------------------
TreatPixelMask:
	ld c, 32 ; Size Band
TreatPixelMask_TileScanlineLoop:
TreatPixelMask_Full:
	ld a, (de)
	exx
	cp c
	jr nz, TreatPixelMask_Left
	ld a, (hl)
	inc l
	jr TreatPixelMask_EndWithWrite

;	ld d, &AA
;	ld e, &55
	
TreatPixelMask_Left:
	ld b, a
	and d
	cp &82 ; 82 = pen 9 left, pen 0 right
	jr nz, TreatPixelMask_Right	
	ld a, b
	and e
	ld b, a
	ld a, (hl)
	inc l
	and d
	add a, b
	jr TreatPixelMask_EndWithWrite
	
TreatPixelMask_Right:
	ld a, b
	and e
	cp &41 ; 41 = pen 0 left, pen 9 right
	jr nz, TreatPixelMask_EndRight
	ld a, b
	and d
	ld b, a
	ld a, (hl)
	inc l
	and e
	add a, b
	jr TreatPixelMask_EndWithWrite
TreatPixelMask_EndRight:
	inc l
	exx
	jr TreatPixelMask_End
	
TreatPixelMask_EndWithWrite:
	exx
	ld (de), a
	
TreatPixelMask_End:
	inc de
	
	dec c
	jr nz, TreatPixelMask_TileScanlineLoop
	ret
	
; ----------------------------------------------------------------------------
TreatPixelNoMask:
	push de
	exx
	ld bc, 32 ; Size Band
	pop de
	ldir
	push de
	exx
	pop de	
	ret
	
; ----------------------------------------------------------------------------
DrawTiles_End:	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _DRAWTILESMASKPENINDEX_CXX_
