	
; ----------------------------------------------------------------------------
DeltaYPtr:
	dw DeltaY1
	
FrameY:
	db 0
	
; ----------------------------------------------------------------------------
BouncingDrawFX:	
	
	push af
	xor a
	ld ( DestPtrFlip ), a
	ld ( DestPtrFlip + 1 ), a
	ld hl, BANK2LINES_TOPPTR
	ld ( BankLinesPtr + 1 ), hl
	pop af
	or a
	jp nz, BouncingDrawFX_ScreenFlipEnd
	
	ld hl, BANK2LINES2_TOPPTR
	ld ( BankLinesPtr + 1 ), hl
	
	; poke RES 7, D
	ld a, &CB
	ld ( DestPtrFlip ), a
	ld a, &BA
	ld ( DestPtrFlip + 1 ), a
	
BouncingDrawFX_ScreenFlipEnd:
	
	di
	ld b, &7F
BouncingDrawFX_Bank:
	ld c, &C4
	out (c), c

	ld ix, ( DeltaYPtr )
	
	ld a, (ix)
	inc ix
	
	cp &FE
	jp nz, NoBankSwitch
	ld ix, DeltaY2
	ld ( DeltaYPtr ), ix
	ld bc, &7FC6
	out (c), c
	ld a, c
	ld (BouncingDrawFX_Bank+1), a
	ld (BouncingDrawFX_MainLoop_Bank+1), a
	ld a, (ix)
	inc ix
NoBankSwitch:

	cp &FF
	jp nz, NoDeltaReset
	ld ix, DeltaY1
	ld ( DeltaYPtr ), ix
	ld bc, &7FC4
	out (c), c
	ld a, c
	ld (BouncingDrawFX_Bank+1), a
	ld (BouncingDrawFX_MainLoop_Bank+1), a
	ld a, (ix)
	inc ix
	
NoDeltaReset:
	or a
	jp z, BouncingDrawFX_MainLoopEnd_RestoreEIAndLeaveBank
	
	push af
	ld a, (ix)
	inc ix
	dec a
	ld (FrameY), a
	
	ld b, &7F
	ld a, ( &0000 )
	ld c, a
	out (c), c
	ei
	
	pop af
	
	;db &ed, &ff
	push af
	;db &ed, &ff
	ld b, a
	ld a, 56+9
	sub b
waitloop:
	ld hl, &100
	ld de, &100
	ld bc, 96
	ldir
	dec a
	jr nz, waitloop
	pop af
	
BouncingDrawFX_MainLoop:
	push af
	
	di
	ld b, &7F
BouncingDrawFX_MainLoop_Bank:
	ld c, &C4
	out (c), c
	
	ld a, (ix)
	inc ix
	
	;db &ed, &ff
	cp &F0
	jr nc, NoRepeat
	
	ld l, a
	ld a, (PrevValue)
	ld h, a
	jr RepeatEnd
	
PrevValue:
	db 0
	
NoRepeat:
	ld h, a
	ld (PrevValue), a
	
	ld l, (ix)
	inc ix
	
RepeatEnd:
	
	ld b, &7F
	ld c, (hl)
	inc hl
	
	ld e, (hl)
	inc hl
	ld d, (hl)
	
	ld a, c
	cp &C0
	jp nz, SkipBottomBankC0
	ld hl, BANK2LINES2_TOPPTR
	ld ( BankLinesPtr + 1 ), hl
SkipBottomBankC0:
	
	cp &C1
	jp nz, SkipBottomBank
	
	ld a, (DestPtrFlip)
	or a
	jr nz, SkipBottomBank
	;db &ed, &ff
	ld c, &C3
	
SkipBottomBank:
	out (c), c
	
	; de = src
	
BankLinesPtr:
	ld hl, BANK2LINES2_TOPPTR
	
	ld a, (FrameY)
	inc a
	ld (FrameY), a
	
	ld b, 0
	ld c, a
	add hl, bc
	add hl, bc
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
	ex de, hl
	
DestPtrFlip:
	nop
	nop
	
	; hl = src
	; de = dst
	
	REPEAT 96
	ldi
	REND
	
	ld b, &7F
	ld a, ( &0000 )
	ld c, a
	out (c), c
	ei
	
	pop af
	dec a
	jp nz, BouncingDrawFX_MainLoop
	
BouncingDrawFX_MainLoopEnd:
	ld ( DeltaYPtr ), ix
	
	ret
	
BouncingDrawFX_MainLoopEnd_RestoreEIAndLeaveBank:
	ld b, &7F
	ld a, ( &0000 )
	ld c, a
	out (c), c
	ei
	
	jp BouncingDrawFX_MainLoopEnd