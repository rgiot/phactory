
; ----------------------------------------------------------------------------
IsFlipped:
	db 0
	
FlipScreen:
	ld bc, &BC00+12
	out (c), c
	
	ld hl, IsFlipped	
	ld a, ( hl )
	or a
	jr nz, FlipScreen2
	
	ld bc, &BD00+&0C
	
	ld a, 1
	jr FlipScreenEnd
	
FlipScreen2:
	
	ld bc, &BD00+&2C	
	xor a
	
FlipScreenEnd:		
	ld ( hl ), a
	
	out (c), c	
	ld bc, &BC00+13
	out (c), c
	ld bc, &BD00+&40
	out (c), c
	ret
	
; ----------------------------------------------------------------------------
GetTopPtr:
	ld a, (IsFlipped)
	or a
	jr z, GetTopPtr2
	ld hl, &8080
	ret
GetTopPtr2:
	ld hl, &0080
	ret

GetBottomPtr:
	ld a, (IsFlipped)
	or a
	jr z, GetBottomPtr2
	ld hl, &C000
	ret
GetBottomPtr2:
	ld hl, &4000
	ret
	
; ----------------------------------------------------------------------------
WaitVBL:	
	ld b, &F5
WaitVBL_Loop:
	in a, (c)
	rra
	jp nc, WaitVBL_Loop	
	ret
		
; ----------------------------------------------------------------------------
InitCRTC:
	ld hl, CRTC_Table
InitCRTC1:
	ld b, &BC
InitCRTC_Loop:
	ld a, (hl)
	or a
	ret z
	inc hl
	ld c, a
	out (c), c
	inc b
	ld a, (hl)
	inc hl
	out (c), a
	dec b
	jr InitCRTC_Loop
CRTC_Table:
	db 1, 48 
;Petite remarque, ta démo plante sur CRTC 2.
;Reg 2 + Reg 3 < 64 sinon le crtc ne génère plus de VBL et le moniteur ne peut plus le synchroniser.
;Si tu mets le registre 2 supérieur à 49, et que reg 3=14, il faut que tu le diminues.
;Donc 50, 13 / 51, 12 / ...
	db 3, 13
	db 2, 50
	db 6, 41
	db 7, 49
	db 9, 4
	db 4, 62
	db 12, &2C
	db 13, &40
	db 0
	
; ----------------------------------------------------------------------------
; HL = palette (16 entries)
SetPalette:
	ld e, 0

setPaletteBegin:
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
	jr nz, setPaletteBegin
	
	ret