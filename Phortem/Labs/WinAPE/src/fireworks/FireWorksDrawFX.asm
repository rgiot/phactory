
; ----------------------------------------------------------------------------
FireWorksDrawFX:

	ld iyl, a
	ld iyh, a
	
	di
	ld a, (&0000)
	push af
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld (&0000), a
	ei
	
FramePrevBlockRightPtr2:
	ld hl, 0
	ld a, h
	or l
	jr z, SkipClearFrameBlockRight
	call DrawFrameBlockRight
SkipClearFrameBlockRight:
FramePrevBlockRightPtr:
	ld hl, 0
	ld (FramePrevBlockRightPtr2+1), hl
FrameBlockRightPtr:
	ld hl, FrameBlockRight
	ld (FramePrevBlockRightPtr+1), hl
	call DrawFrameBlockRight
	ld (FrameBlockRightPtr+1), hl
	
	di
	ld bc, &7FC0
	out (c), c
	ld a, c
	ld (&0000), a
	ei
	
FramePrevBlockLeftPtr2:
	ld hl, 0
	ld a, h
	or l
	jr z, SkipClearFrameBlockLeft
	call DrawFrameBlockLeft
SkipClearFrameBlockLeft:
FramePrevBlockLeftPtr:
	ld hl, 0
	ld (FramePrevBlockLeftPtr2+1), hl
FrameBlockLeftPtr:
	ld hl, FrameBlockLeft1
	ld (FramePrevBlockLeftPtr+1), hl
	call DrawFrameBlockLeft	
	
	ld de, FrameBlockLeft1End
	
	ld a, h
	cp d 
	jr nz, NoFrameBlockLeft1End
	ld a, l
	cp e
	jr nz, NoFrameBlockLeft1End
	
	ld hl, FrameBlockLeft1
	
NoFrameBlockLeft1End:
	
	ld (FrameBlockLeftPtr+1), hl
	
	di
	pop af
	ld (&0000), a
	ld b, &7F
	ld c, a
	out (c), c
	ei
	
	ld a, ixh
	or a
	ret z	
	ld a, ixl
	ret
	
; ----------------------------------------------------------------------------
DrawFrameBlockLeft:
	di
	ld ( DrawFrameBlockLeft_OldSP + 1 ), sp
	ld sp, hl
	
	ld a, iyh
	ld h, a
		
DrawFrameBlockLeftLoop:
	pop bc
	ld a, b
	or a
	jr z, EndFrameBlockLeft
	
	ld l, b	
	ld b, 0
REPEAT 4
	ld e, (hl)
	inc h
	ld d, (hl)
	dec h
	inc l
	
	ex de, hl
	add hl, bc
	
	ld a, 255
	xor (hl)
	ld (hl), a
	ex de, hl
ENDM

	ei
	pop de
	di
	push de
	
	jr DrawFrameBlockLeftLoop
	
EndFrameBlockLeft:
	ld a, c
	or a
	jr z, FrameBlockLeft_NoReset
	ld hl, FrameBlockLeft1
	ld ixh, 1
	jr FrameBlockLeft_EndCalcNextFramePtr
FrameBlockLeft_NoReset:
	ld hl, 0
	add hl, sp
FrameBlockLeft_EndCalcNextFramePtr:
	
DrawFrameBlockLeft_OldSP: 
	ld sp, 0
	ei
	ret
	
; ----------------------------------------------------------------------------
DrawFrameBlockRight:
	di
	ld ( DrawFrameBlockRight_OldSP + 1 ), sp
	ld sp, hl
		
	ld a, iyl
	ld h, a
		
DrawFrameBlockRightLoop:
	pop bc
	ld a, b
	or a
	jp z, EndFrameBlockRight
	
	ld l, b
REPEAT 4
	ld e, (hl)
	inc h
	ld d, (hl)
	dec h
	inc l
	
	ex de, hl
	ld b, 0
	add hl, bc
	
	ld a, (hl)
	and &AA
	ld b, a
	ld a, 255
	xor (hl)
	and &55
	or b
	ld (hl), a
	
	inc hl
	
	ld a, (hl)
	and &55
	ld b, a
	ld a, 255
	xor (hl)
	and &AA
	or b
	ld (hl), a
	
	ex de, hl
ENDM

	ei
	pop de
	di
	push de
	
	jp DrawFrameBlockRightLoop
	
EndFrameBlockRight:
	ld a, c
	or a
	jr z, FrameBlockRight_NoReset
	ld hl, FrameBlockRight
	ld ixl, 1
	jr FrameBlockRight_EndCalcNextFramePtr
FrameBlockRight_NoReset:
	ld hl, 0
	add hl, sp
FrameBlockRight_EndCalcNextFramePtr:
	
DrawFrameBlockRight_OldSP: 
	ld sp, 0
	ei
	ret
	