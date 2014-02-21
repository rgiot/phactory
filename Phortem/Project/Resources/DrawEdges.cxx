
// ----------------------------------------------------------------------------
#ifndef _DRAWEDGES_CXX_
#define _DRAWEDGES_CXX_

// ----------------------------------------------------------------------------
void DrawWriter()
{
__asm
	di
	ld a, ( &0000 )
	push af
	ld bc, &7FC6
	out (c), c
	ld a, &C6
	ld ( &0000 ), a
	ei
		
	ld hl, VIDEOSEGMENTC000 ; BOTTOM
	push hl
	ld hl, VIDEOSEGMENT8000 ; TOP
	push hl
	ld hl, &4000 ; DATA SRC
	push hl
	call _DrawBGEdges
	pop af
	pop af
	pop af
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei		
__endasm;
}

// ----------------------------------------------------------------------------
void DrawBGEdges(unsigned char *src, unsigned char *topPtr, unsigned char *bottomPtr)
{
	src;
	topPtr;
	bottomPtr;
	
__asm
	push ix
	ld ix, 0
	add	ix, sp
	
	ld e, (ix+4)
	ld d, (ix+5)

	ld l, (ix+6)
	ld h, (ix+7)
	
	ld a, 20 ; TOP
	ld ( DrawBGEdges_LineCount + 1 ), a
	
	ld b, 0

DrawBGEdges_LineCount:
	ld a, 20
DrawBGEdges_Top:
	ex af, af'
	
	push hl

	call DrawBGEdges_ScanlineLoop
	
	set 3, h
	call DrawBGEdges_ScanlineLoop
	
	res 3, h
	set 4, h
	call DrawBGEdges_ScanlineLoop
	
	set 3, h
	call DrawBGEdges_ScanlineLoop
	
	set 5, h
	res 4, h
	res 3, h
	call DrawBGEdges_ScanlineLoop
	
	pop hl
	ld c, 96
	add hl, bc
	
	ex af, af'
	dec a
	jr nz, DrawBGEdges_Top
	
	ld a, (DrawBGEdges_LineCount+1)
	cp 21
	jp z, DrawBGEdges_End
	inc a
	ld (DrawBGEdges_LineCount+1), a
	ld l, (ix+8)
	ld h, (ix+9)	
	jr DrawBGEdges_LineCount

DrawBGEdges_ScanlineLoop:
	ld a, (de) ; posX
	inc de
	cp -1
	ret z
	
	bit 7, a
	jr nz, DrawBGEdges_PixelType0
	
	ld c, a
	
	ld a, (de) ; type
	inc de
	cp 1
	jr z, DrawBGEdges_PixelType1
	
DrawBGEdges_PixelType2:
	push hl
	add hl, bc
	
	ld a, (de)
	inc de
	ld c, a
	ld a, (hl)
	and &AA
	or c
	ld (hl), a
	
	pop hl
	jr DrawBGEdges_ScanlineLoop
	
DrawBGEdges_PixelType0:
	res 7, a
	ld c, a
	push hl
	add hl, bc
	
	ld a, (de)
	inc de
	ld (hl), a
	
	pop hl
	jr DrawBGEdges_ScanlineLoop
	
DrawBGEdges_PixelType1:
	push hl
	add hl, bc
	
	ld a, (de)
	inc de
	ld c, a
	ld a, (hl)
	and &55
	or c
	ld (hl), a
	
	pop hl
	jr DrawBGEdges_ScanlineLoop
	
DrawBGEdges_End:
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _DRAWEDGES_CXX_
