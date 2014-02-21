
; ----------------------------------------------------------------------------
DrawFrame:
	;44202
	xor a
	ld ( DoBottom + 1 ), a
	
	push hl
	
FrameDegradePtr:
	ld bc, 700 - 205
	dec bc
	dec bc
	dec bc
	ld ( FrameDegradePtr + 1 ), bc
	
FrameDegradeSwitchValue:
	ld a, 5
	dec a
	or a
	jr nz, FrameDegradeSwitch
FrameDegradeSwitchTextureValue:
	ld a, 1
	or a
	jr z, FrameDegradeSwitchPtrValue1
	ld hl, Degrade
	xor a
	ld ( FrameDegradeSwitchTextureValue + 1 ), a
	ld ( FrameDegradeSwitchPtrValue + 1 ), hl
	ld a, 14-6+1
	jr FrameDegradeSwitch
FrameDegradeSwitchPtrValue1:
	ld hl, Degrade2	
	ld a, 1
	ld ( FrameDegradeSwitchTextureValue + 1 ), a
	ld ( FrameDegradeSwitchPtrValue + 1 ), hl
	ld a, 23-15+1
FrameDegradeSwitch:
	ld ( FrameDegradeSwitchValue + 1 ), a
	
FrameDegradeSwitchPtrValue:
	ld hl, Degrade2
	add hl, bc
	push hl
	pop ix
	
	pop hl
	
	di
	
	ld ( DrawFrame_SaveSP + 1 ), sp
	ld sp, hl
	
	exx
	ld hl, (OffsetXPtr)	
	ld c, (hl)
	ld hl, (SinFlipPtr1)
	ld de, (SinFlipPtr2)
	exx
	
	ld a, 20*5
DrawFrameLoop:
	ex af, af'
	
	;db &ed, &ff
	
	exx
	ld a, (de)
	dec e
	add a, (hl)
	dec l
	add a, c
	exx
	
	pop de
	pop hl ; Skip bg texture
	pop hl
	
	ld b, 0
	rra
	jr nc, SkipAddDiv2
	
	ld c, a
	add hl, bc
	
	ex de, hl
	
	ld c, SCR_BLOCKSWIDTH_DIV2-1
	add hl, bc	
	
	jr SkipAddDiv2_1
SkipAddDiv2:
	
	ld c, a
	add hl, bc
	
	ex de, hl
	
	dec hl
	
	;ld bc, &55AA ; b=&55 c=&AA
		
SkipAddDiv2_1:
	ld c, (ix)
	inc ix
	
	xor a	
REPEAT SCR_BLOCKSWIDTH_DIV2
	inc hl
	or (hl)
	jr z, @SkipPixel
	
	bit 0, a ; Degradé2? (bordure)
	jr z, @Degrade1
	
;
@Degrade2:
	bit 6, a
	jr z, @DoDegradeLeft2	
	bit 7, a
	jr z, @DoDegradeRight2
	
	ld a, &3C
	jr @DoWriteDEAndLeave
	
@DoDegradeLeft2:
	ld a, (de)
	and &55
	or &28 ; 3C AND AA ; PEN 6
	jr @DoWriteDEAndLeave
	
@DoDegradeRight2:
	ld a, (de)
	and &AA
	or &14 ; FC AND 55 ; PEN 6
	jr @DoWriteDEAndLeave
	
;
@Degrade1:
	bit 6, a
	jr z, @DoDegradeLeft	
	bit 7, a
	jr z, @DoDegradeRight
	
	ld a, c
	jr @DoWriteDEAndLeave
	
@DoDegradeLeft:
	ld a, c
	and &AA
	ld b, a
	ld a, (de)
	and &55
	jr @DoORAndLeave
	
@DoDegradeRight:
	ld a, c
	and &55
	ld b, a
	ld a, (de)
	and &AA
	
@DoORAndLeave:
	or b

@DoWriteDEAndLeave:
	ld (de), a
	xor a	
	
@SkipPixel:
	inc de
	
@EndScanline:
REND
	
	ei
	pop hl
	di
	push hl
	
	ex af, af'
	dec a
	jp nz, DrawFrameLoop
	
DoBottom:
	ld a, 0
	or a
	jr nz, DrawFrameLoop_End
	
	inc a
	ld ( DoBottom + 1 ), a
	
	;di
	ld a, ( BottomBank )
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	;ei
	
	ld a, 21*5
	jp DrawFrameLoop
	
DrawFrameLoop_End:

DrawFrame_SaveSP:
	ld sp, 0
	ei
	
	;db &ed, &ff
	ret
	
	