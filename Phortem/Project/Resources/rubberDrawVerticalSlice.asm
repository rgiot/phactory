
; ----------------------------------------------------------------------------
MACRO PrepareSlice
		ld d, h
		ld e, l
ENDM

; ----------------------------------------------------------------------------
SetDrawSliceQuit:
		ld a, &C3 ; JP
		ld ( DrawVerticalSliceAll ), a
		ld hl, DrawVerticalSliceAll2
		ld ( DrawVerticalSliceAll + 1 ), hl
		ret
		
; ----------------------------------------------------------------------------
MACRO DrawSlice
		push hl
		
		ld c, SliceSkip + SliceWidth-1
				
REPT SliceWidth-1 ;+1
		ldi
ENDM	

		add hl, bc
		ex de, hl
		add hl, bc
		ex de, hl
		
REPT SliceWidth+1 ;+1
		ldi
ENDM		

		pop hl
ENDM

; ----------------------------------------------------------------------------
DrawSliceEnd:
		push hl
		
REPT SliceWidth-1
		ldi
ENDM	

REPT SliceSkip
		ldi
ENDM
		
REPT SliceWidth
		ldi
ENDM		

		pop hl
		ret

; ----------------------------------------------------------------------------
LeftSlicePosX:
		dw 0

; ----------------------------------------------------------------------------
DrawVerticalSliceAll:		
		sra c
		ld ( LeftSlicePosX ), bc	
		
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC6
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei		
		
		ld bc, ( LeftSlicePosX )
		
		ld a, (_IsFlipped)
		or a
		jr z, DrawVerticalSliceAllVIDEOSEGMENT0000
		
		call DrawVerticalSliceTop
		
		di
		ld bc, &7FC3
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei		
		
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceBottom
		
		jp DrawVerticalSliceAll_End
		
DrawVerticalSliceAllVIDEOSEGMENT0000:

		call DrawVerticalSliceTopVIDEOSEGMENT0000
		
		di
		ld bc, &7FC1
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei		
		
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceBottomVIDEOSEGMENT0000		

DrawVerticalSliceAll_End:			
		di
		pop af
		ld c, a
		ld b, &7F
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		ret
		

; ----------------------------------------------------------------------------
DrawVerticalSliceAll2:		
		sra c
		ld ( LeftSlicePosX ), bc	
		
		di
		ld a, ( &0000 )
		push af
		ld bc, &7FC6
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei		
		
		ld bc, ( LeftSlicePosX )
		
		ld a, (_IsFlipped)
		or a
		jr z, DrawVerticalSliceAllVIDEOSEGMENT00002
		
		call DrawVerticalSliceTop2
		
		di
		ld bc, &7FC3
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei		
		
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceBottom2
		
		jp DrawVerticalSliceAll_End2
		
DrawVerticalSliceAllVIDEOSEGMENT00002:

		call DrawVerticalSliceTopVIDEOSEGMENT00002
		
		di
		ld bc, &7FC1
		out (c), c		
		ld a, c
		ld ( &0000 ), a
		ei		
		
		ld bc, ( LeftSlicePosX )
		call DrawVerticalSliceBottomVIDEOSEGMENT00002		

DrawVerticalSliceAll_End2:			
		di
		pop af
		ld c, a
		ld b, &7F
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		ret
		
; ----------------------------------------------------------------------------
DrawVerticalSliceTop:		
		ld hl, &4080
		add hl, bc ; add X
		
		; HL = &4080 (bitmap top &C6)
		; DE = &8080
		
		ld a, 100/5
DrawVerticalSliceTopInnerLoop:
		
		ld b, 0
		
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
				
		ld bc, 96-&800-&800-&800-&800
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
	
		ld a, 105/5
DrawVerticalSliceBottomInnerLoop:
		
		ld b, 0
		
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
		
		ld bc, 96-&800-&800-&800-&800
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
		
		ld a, 100/5
DrawVerticalSliceTopVIDEOSEGMENT0000InnerLoop:
		
		ld b, 0
		
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
				
		ld bc, 96-&800-&800-&800-&800
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
	
		ld a, 105/5
DrawVerticalSliceBottomVIDEOSEGMENT0000InnerLoop:
		
		ld b, 0
		
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
		
		ld bc, 96-&800-&800-&800-&800
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceBottomVIDEOSEGMENT0000InnerLoop
		
		ret
		
; ----------------------------------------------------------------------------
DrawVerticalSliceTop2:		
		ld hl, &4080
		add hl, bc ; add X
		
		; HL = &4080 (bitmap top &C6)
		; DE = &8080
		
		ld a, 100/5
DrawVerticalSliceTopInnerLoop2:
		
		ld b, 0
		
		; line 0
		PrepareSlice
		res 6, d
		set 7, d
		call DrawSliceEnd
		
		; line 1
		set 3, h
		
		PrepareSlice
		res 6, d
		set 7, d
		call DrawSliceEnd
		
		; line 2
		res 3, h
		set 4, h
		
		PrepareSlice
		res 6, d
		set 7, d
		call DrawSliceEnd
		
		; line 3
		set 3, h
		
		PrepareSlice
		res 6, d
		set 7, d
		call DrawSliceEnd
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		
		PrepareSlice
		res 6, d
		set 7, d
		call DrawSliceEnd
				
		ld bc, 96-&800-&800-&800-&800
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceTopInnerLoop2

		ret

; ----------------------------------------------------------------------------
DrawVerticalSliceBottom2:

		ld hl, &C000
		add hl, bc ; add X
				
		; HL = &C000 (bitmap top &C1)
		; DE = &4000
	
		ld a, 105/5
DrawVerticalSliceBottomInnerLoop2:
		
		ld b, 0
		
		; line 0
		PrepareSlice
		res 7, d			
		call DrawSliceEnd
		
		; line 1
		set 3, h
		PrepareSlice
		res 7, d			
		call DrawSliceEnd
		
		; line 2
		res 3, h
		set 4, h
		PrepareSlice
		res 7, d			
		call DrawSliceEnd
		
		; line 3
		set 3, h
		PrepareSlice
		res 7, d			
		call DrawSliceEnd
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		PrepareSlice
		res 7, d			
		call DrawSliceEnd
		
		ld bc, 96-&800-&800-&800-&800
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceBottomInnerLoop2
		
		ret
		
; ----------------------------------------------------------------------------
DrawVerticalSliceTopVIDEOSEGMENT00002:		
		ld hl, &4080
		add hl, bc ; add X
		
		; HL = &4080 (bitmap top &C6)
		; DE = &0080
		
		ld a, 100/5
DrawVerticalSliceTopVIDEOSEGMENT0000InnerLoop2:
		
		ld b, 0
		
		; line 0
		PrepareSlice
		res 6, d
		call DrawSliceEnd
		
		; line 1
		set 3, h
		
		PrepareSlice
		res 6, d
		call DrawSliceEnd
		
		; line 2
		res 3, h
		set 4, h
		
		PrepareSlice
		res 6, d	
		call DrawSliceEnd
		
		; line 3
		set 3, h
		
		PrepareSlice
		res 6, d		
		call DrawSliceEnd
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		
		PrepareSlice
		res 6, d			
		call DrawSliceEnd
				
		ld bc, 96-&800-&800-&800-&800
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceTopVIDEOSEGMENT0000InnerLoop2

		ret

; ----------------------------------------------------------------------------
DrawVerticalSliceBottomVIDEOSEGMENT00002:

		ld hl, &C000
		add hl, bc ; add X
				
		; HL = &C000 (bitmap top &C3)
		; DE = &4000
	
		ld a, 105/5
DrawVerticalSliceBottomVIDEOSEGMENT0000InnerLoop2:
		
		ld b, 0
		
		; line 0
		PrepareSlice
		res 7, d						
		call DrawSliceEnd
		
		; line 1
		set 3, h
		PrepareSlice
		res 7, d		
		call DrawSliceEnd
		
		; line 2
		res 3, h
		set 4, h
		PrepareSlice
		res 7, d	
		call DrawSliceEnd
		
		; line 3
		set 3, h
		PrepareSlice
		res 7, d	
		call DrawSliceEnd
		
		; line 4
		set 5, h
		res 4, h
		res 3, h
		PrepareSlice
		res 7, d		
		call DrawSliceEnd
		
		ld bc, 96-&800-&800-&800-&800
		add hl, bc		
		dec a
		jp nz, DrawVerticalSliceBottomVIDEOSEGMENT0000InnerLoop2
		
		ret
		