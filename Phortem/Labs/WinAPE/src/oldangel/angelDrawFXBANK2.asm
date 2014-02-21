	
; ----------------------------------------------------------------------------
RightClipping equ 192

; ----------------------------------------------------------------------------
MACRO StartAngel
	ld c, &C6
	out (c), c
MEND

; ----------------------------------------------------------------------------
MACRO StartDemon
	ld c, &C4
	out (c), c
MEND

; ----------------------------------------------------------------------------
MACRO AngelDrawFX_BANK2_DrawScanlineLeft_Angel PosX
	ld ( @Quit + 1 ), hl

	ld bc, PosX
	add hl, bc
	ld a, h
	or a
	jp nz, @Quit ; is negative offset or too high right clipping? if yes, skip	
	ld a, l
	cp RightClipping
	jp nc, @Quit ; right clipping
	ld ( @RightLastPixel + 1 ), a
	ld b, &7F
	srl h
	rr l
	jp c, @Right

@Left:
	add hl, de
	StartAngel
	ld c, h
	res 7, h
	set 6, h
	ld a, (hl)
	ld h, c
	StartDemon
	ld (hl), a
	jp @Quit
	
@Right:
	add hl, de
	StartAngel
	ld c, h
	res 7, h
	set 6, h
	inc hl
	ld a, (hl)
	dec hl
	ld (@RightPixel2 + 1), a
	ld a, (hl)
	and &55
	ld h, c
	StartDemon
	ld c, a
	ld a, (hl)
	and &AA
	add a, c
	ld (hl), a
	
@RightLastPixel:
	ld a, 0
	cp RightClipping-1
	jp z, @Quit

	inc hl
	ld a, (hl)
	and &55
	ld c, a
@RightPixel2:
	ld a, 0
	and &AA
	add a, c
	ld (hl), a
	
@Quit:
	ld hl, 0
MEND

; ----------------------------------------------------------------------------
MACRO AngelDrawFX_BANK2_DrawScanlineLeft_Demon PosX
	ld ( @Quit + 1 ), hl

	ld bc, PosX
	add hl, bc
	ld a, h
	or a
	jp nz, @Quit ; is negative offset or too high right clipping? if yes, skip	
	ld a, l
	cp RightClipping
	jp nc, @Quit ; right clipping
	ld ( @RightLastPixel + 1 ), a
	ld b, &7F
	srl h
	rr l
	jp c, @Right

@Left:
	add hl, de
	StartDemon
	ld c, h
	res 7, h
	set 6, h
	ld a, (hl)
	ld h, c
	StartAngel
	ld (hl), a
	jp @Quit
	
@Right:
	add hl, de
	StartDemon
	ld c, h
	res 7, h
	set 6, h
	inc hl
	ld a, (hl)
	dec hl
	ld (@RightPixel2 + 1), a
	ld a, (hl)
	and &55
	ld h, c
	StartAngel
	ld c, a
	ld a, (hl)
	and &AA
	add a, c
	ld (hl), a
	
@RightLastPixel:
	ld a, 0
	cp RightClipping-1
	jp z, @Quit

	inc hl
	ld a, (hl)
	and &55
	ld c, a
@RightPixel2:
	ld a, 0
	and &AA
	add a, c
	ld (hl), a
	
@Quit:
	ld hl, 0
MEND
	
; ----------------------------------------------------------------------------
LineBatch:
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 1
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 7
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 22
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 27
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 36
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 44
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 51
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 57
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 58
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 134
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 135
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 141
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 148
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 156
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 165
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 170
	
	AngelDrawFX_BANK2_DrawScanlineLeft_Angel 185
	AngelDrawFX_BANK2_DrawScanlineLeft_Demon 191

	ret

; ----------------------------------------------------------------------------
; TO BE MOVED IN angel.asm !
InitPosX:
	ld hl, (PosXDataPtr)
	inc hl
	inc hl
	ld (PosXDataPtr), hl
	
	ld de, PosXData_End
	
	ld a, h
	cp d
	jp nz, InitPosX_NoReset
	ld a, l
	cp e
	jp nz, InitPosX_NoReset
	ld hl, PosXData
	ld ( PosXDataPtr ), hl
InitPosX_NoReset:
	
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	ld ( PosX ), hl
	ret

; ----------------------------------------------------------------------------
; TO BE MOVED IN angel.asm !
InitFrameTable:
	ld hl, (FrameTablePtr)
	inc hl
	inc hl
	ld (FrameTablePtr), hl
	
	ld de, FrameTable2ndPart
	
	ld a, h
	cp d
	jp nz, InitFrameTable_NoReset
	ld a, l
	cp e
	jp nz, InitFrameTable_NoReset
	
	ld hl, FrameTable
	ld (FrameTablePtr), hl
InitFrameTable_NoReset:
	
	ld bc, (PosX)
		
	ld (hl), c
	inc hl
	ld (hl), b
	
	ld de, 200-1
	add hl, de
	
	ld (hl), c
	inc hl
	ld (hl), b

	ret
	
; ----------------------------------------------------------------------------
AngelDrawFX_BANK2:
	call InitPosX
	call InitFrameTable
	call InitFrameTable

	ld de, VIDEOSEGMENT8000
	ld a, 100/5
	
	ld ix, (FrameTablePtr)
	inc ix
	inc ix
	
AngelDrawFX_BANK2_Loop:
	ex af, af'
	
	ld l, (ix)
	inc ix
	ld h, (ix)
	inc ix
	call LineBatch
	
	ld l, (ix)
	inc ix
	ld h, (ix)
	inc ix
	set 3, d
	call LineBatch
	
	ld l, (ix)
	inc ix
	ld h, (ix)
	inc ix
	res 3, d
	set 4, d
	call LineBatch
	
	ld l, (ix)
	inc ix
	ld h, (ix)
	inc ix
	set 3, d
	call LineBatch
	
	ld l, (ix)
	inc ix
	ld h, (ix)
	inc ix
	set 5, d
	res 4, d
	res 3, d
	call LineBatch
	
	ex de, hl
	ld bc, 96-&800-&800-&800-&800
	add hl, bc		
	ex de, hl
	
	ex af, af'
	dec a
	jp nz, AngelDrawFX_BANK2_Loop
	
	ret
	
