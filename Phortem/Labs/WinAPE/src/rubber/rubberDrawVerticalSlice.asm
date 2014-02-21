		
; 001 line 1 / set 3,h
; 011 line 3 / set 4,h
; 010 line 2 / res 3,h
; 110 line 6 / set 5,h
; 100 line 4 / res 4,h
; 101 line 5 / set 3,h
; 111 line 7 / set 4,h

; ----------------------------------------------------------------------------
MACRO PrepareSlice
		ld d, h
		ld e, l
MEND

MACRO DrawSlice	
		push hl
		
REPEAT SliceWidth
		ldi
REND	

		ld bc, SliceSkip
		add hl, bc
		ex de, hl
		add hl, bc
		ex de, hl
		
REPEAT SliceWidth
		ldi
REND		

		pop hl
MEND

; ----------------------------------------------------------------------------
LeftSlicePosX:
		dw 0

; ----------------------------------------------------------------------------
DrawVerticalSliceAll:		
		sra c
		ld ( LeftSlicePosX ), bc		
		
		ld a, (IsFlipped)
		or a
		jr z, DrawVerticalSliceAllVIDEOSEGMENT0000

		ld bc, &7FC6
		out (c), c
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceTop
		ld bc, &7FC3
		out (c), c
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceBottom
		ld bc, &7FC0
		out (c), c	
		ret
		
DrawVerticalSliceAllVIDEOSEGMENT0000:

		ld bc, &7FC6
		out (c), c
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceTopVIDEOSEGMENT0000
		ld bc, &7FC1
		out (c), c
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceBottomVIDEOSEGMENT0000
		ld bc, &7FC0
		out (c), c	
		ret
		
; ----------------------------------------------------------------------------
DrawVerticalSliceTop:		
		ld hl, &4080
		add hl, bc ; add X
		
		; HL = &4080 (bitmap top &C6)
		; DE = &8080
		
		ld bc, 96-&800-&800-&800-&800
		
		ld a, 100/5
DrawVerticalSliceTopInnerLoop:
		push bc
		
		; line 0
		PrepareSlice
		res 6, d
		set 7, d
		DrawSlice
		
		; line 1
		set 3, h
		
		PrepareSlice
		res 6, d
		set 7, d
		DrawSlice
		
		; line 2
		res 3, h
		set 4, h
		
		PrepareSlice
		res 6, d
		set 7, d
		DrawSlice
		
		; line 3
		set 3, h
		
		PrepareSlice
		res 6, d
		set 7, d
		DrawSlice
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		
		PrepareSlice
		res 6, d
		set 7, d
		DrawSlice
				
		pop bc
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceTopInnerLoop

		ret

; ----------------------------------------------------------------------------
DrawVerticalSliceBottom:

		ld hl, &C000
		add hl, bc ; add X
				
		; HL = &C000 (bitmap top &C1)
		; DE = &4000
				
		ld bc, 96-&800-&800-&800-&800
	
		ld a, 105/5
DrawVerticalSliceBottomInnerLoop:
		push bc
		
		; line 0
		PrepareSlice
		res 7, d			
		DrawSlice
		
		; line 1
		set 3, h
		PrepareSlice
		res 7, d			
		DrawSlice
		
		; line 2
		res 3, h
		set 4, h
		PrepareSlice
		res 7, d			
		DrawSlice
		
		; line 3
		set 3, h
		PrepareSlice
		res 7, d			
		DrawSlice
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		PrepareSlice
		res 7, d			
		DrawSlice
		
		pop bc
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceBottomInnerLoop
		
		ret
		
; ----------------------------------------------------------------------------
DrawVerticalSliceTopVIDEOSEGMENT0000:		
		ld hl, &4080
		add hl, bc ; add X
		
		; HL = &4080 (bitmap top &C6)
		; DE = &0080
		
		ld bc, 96-&800-&800-&800-&800
		
		ld a, 100/5
DrawVerticalSliceTopVIDEOSEGMENT0000InnerLoop:
		push bc
		
		; line 0
		PrepareSlice
		res 6, d
		DrawSlice
		
		; line 1
		set 3, h
		
		PrepareSlice
		res 6, d
		DrawSlice
		
		; line 2
		res 3, h
		set 4, h
		
		PrepareSlice
		res 6, d	
		DrawSlice
		
		; line 3
		set 3, h
		
		PrepareSlice
		res 6, d		
		DrawSlice
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		
		PrepareSlice
		res 6, d			
		DrawSlice
				
		pop bc
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceTopVIDEOSEGMENT0000InnerLoop

		ret

; ----------------------------------------------------------------------------
DrawVerticalSliceBottomVIDEOSEGMENT0000:

		ld hl, &C000
		add hl, bc ; add X
				
		; HL = &C000 (bitmap top &C3)
		; DE = &4000
				
		ld bc, 96-&800-&800-&800-&800
	
		ld a, 105/5
DrawVerticalSliceBottomVIDEOSEGMENT0000InnerLoop:
		push bc
		
		; line 0
		PrepareSlice
		res 7, d						
		DrawSlice
		
		; line 1
		set 3, h
		PrepareSlice
		res 7, d		
		DrawSlice
		
		; line 2
		res 3, h
		set 4, h
		PrepareSlice
		res 7, d	
		DrawSlice
		
		; line 3
		set 3, h
		PrepareSlice
		res 7, d	
		DrawSlice
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		PrepareSlice
		res 7, d		
		DrawSlice
		
		pop bc
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceBottomVIDEOSEGMENT0000InnerLoop
		
		ret
		