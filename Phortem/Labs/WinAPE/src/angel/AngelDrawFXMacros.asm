
; DAEMON TOP
;   &C4 &4080

; DAEMON BOTTOM
;   CODESEGMENT0000 + &800   &1000
;   CODESEGMENTC000  &1800

; ANGEL TOP
; &C6 &4080

; ANGEL BOTTOM
; &C7 &4000

; ----------------------------------------------------------------------------
	;write "AngelDrawFXMacros_BA00.z80.bin"
	
	org &BA00
	limit &BFFF
	
	; BB00 - DrawPixelTop0 - Line 1
	; BB20 - DrawPixelTop1 - Line 1
	; BB40 - DrawPixelTop2 - Line 1
	; BB60 - DrawPixelTop3 - Line 1
	
	; BB80 - DrawPixelTop0 - Line 2
	; BBA0 - DrawPixelTop1 - Line 2
	; BBC0 - DrawPixelTop2 - Line 2
	; BBE0 - DrawPixelTop3 - Line 2
	
	; BC00 - DrawPixelTop0 - Line 3
	; BC20 - DrawPixelTop1 - Line 3
	; BC40 - DrawPixelTop2 - Line 3
	; BC60 - DrawPixelTop3 - Line 3
	
	; BC80 - DrawPixelTop0 - Line 4
	; BCA0 - DrawPixelTop1 - Line 4
	; BCC0 - DrawPixelTop2 - Line 4
	; BCE0 - DrawPixelTop3 - Line 4
	
	; BD00 - DrawPixelTop0 - Line 5
	; BD20 - DrawPixelTop1 - Line 5
	; BD40 - DrawPixelTop2 - Line 5
	; BD60 - DrawPixelTop3 - Line 5
	
	; BD80 - DrawPixelBottom0 - Line 1
	; BDA0 - DrawPixelBottom1 - Line 1
	; BDC0 - DrawPixelBottom2 - Line 1
	; BDE0 - DrawPixelBottom3 - Line 1
	
	; BE00 - DrawPixelBottom0 - Line 2
	; BE20 - DrawPixelBottom1 - Line 2
	; BE40 - DrawPixelBottom2 - Line 2
	; BE60 - DrawPixelBottom3 - Line 2
	
	; BE80 - DrawPixelBottom0 - Line 3
	; BEA0 - DrawPixelBottom1 - Line 3
	; BEC0 - DrawPixelBottom2 - Line 3
	; BEE0 - DrawPixelBottom3 - Line 3
	
	; BF00 - DrawPixelBottom0 - Line 4
	; BF20 - DrawPixelBottom1 - Line 4
	; BF40 - DrawPixelBottom2 - Line 4
	; BF60 - DrawPixelBottom3 - Line 4
	
	; BF80 - DrawPixelBottom0 - Line 5
	; BFA0 - DrawPixelBottom1 - Line 5
	; BFC0 - DrawPixelBottom2 - Line 5
	; BFE0 - DrawPixelBottom3 - Line 5
	
; ----------------------------------------------------------------------------
MACRO DrawPixelTop0
	; left OFF - right OFF
	ld c, &C6 ; FOR ALL MACROS - DO NOT CHANGE LD C, XX  OUT (C), C,
	out (c), c ; THERE IS OPTI HERE!! SKIPPED IF UNCHANGED !
	ld a, (hl)	
	ld (de), a		
MEND

MACRO DrawPixelTop1
	; left OFF - right ON
	ld c, &C6
	out (c), c	
	ld a, (hl)
	ld c, &C4
	out (c), c	
	and &AA
	ld c, a	
	ld a, (hl)
	and &55
	add c	
	ld (de), a
	ld c, &C4	
MEND

MACRO DrawPixelTop2
	; left ON - right OFF
	ld c, &C6
	out (c), c	
	ld a, (hl)
	ld c, &C4
	out (c), c	
	and &55
	ld c, a	
	ld a, (hl)
	and &AA
	add c	
	ld (de), a
	ld c, &C4	
MEND

MACRO DrawPixelTop3
	; left ON - right ON
	ld c, &C4
	out (c), c
	ld a, (hl)
	ld (de), a
MEND

; ----------------------------------------------------------------------------
MACRO DrawPixelBottom0
	; left OFF - right OFF
	ld c, &C7
	out (c), c	
	ld a, (hl)	
	ld c, &C0
	out (c), c		
	ld (de), a	
MEND

MACRO DrawPixelBottom1
	; left OFF - right ON
	ld c, &C7
	out (c), c	
	ld a, (hl)	
	ld c, &C0
	out (c), c
	and &AA
	ld c, a	
	ld a, (hl)
	and &55
	add c
	ld (de), a	
	ld c, &C0	
MEND

MACRO DrawPixelBottom2
	; left ON - right OFF
	ld c, &C7
	out (c), c	
	ld a, (hl)	
	ld c, &C0
	out (c), c
	and &55
	ld c, a	
	ld a, (hl)
	and &AA
	add c
	ld (de), a	
	ld c, &C0	
MEND

MACRO DrawPixelBottom3
	; left ON - right ON
	ld c, &C0
	out (c), c
	ld a, (hl)
	ld (de), a
MEND

; ----------------------------------------------------------------------------
MACRO DrawPixelTopEnd5:
	; Draw Line 0
	res 5, d
	res 5, h
	ret
MEND
MACRO DrawPixelTopEnd1:
	; Draw Line 1
	set 3, d
	set 3, h
	ret
MEND
MACRO DrawPixelTopEnd2:
	; Draw Line 3
	set 4, d
	set 4, h
	ret
MEND
MACRO DrawPixelTopEnd4:
	; Draw Line 2
	res 3, d
	res 3, h
	ret
MEND
MACRO DrawPixelTopEnd3:
	ex af, af'
	dec a
	jp nz, DrawItems
	jp NoItemsToDraw
MEND

; ----------------------------------------------------------------------------
MACRO DrawPixelBottomEnd5:
	; Draw Line 0
	res 5, d
	res 5, h
	ret
MEND
MACRO DrawPixelBottomEnd1:
	; Draw Line 1
	set 3, d
	set 3, h
	ret
MEND
MACRO DrawPixelBottomEnd2:
	; Draw Line 3
	set 4, d
	set 4, h
	ret
MEND
MACRO DrawPixelBottomEnd4:
	; Draw Line 2
	res 3, d
	res 3, h
	ret
MEND
MACRO DrawPixelBottomEnd3:
	ex af, af'
	dec a
	jp nz, DrawItemsBottom
	jp NoItemsToDraw
MEND

; ----------------------------------------------------------------------------
	DrawPixelTopEnd1
	ALIGN 16
	DrawPixelTopEnd2
	ALIGN 16
	DrawPixelTopEnd3
	ALIGN 16
	DrawPixelTopEnd4
	ALIGN 16
	DrawPixelTopEnd5
	
	ALIGN 16
	DrawPixelBottomEnd1
	ALIGN 16
	DrawPixelBottomEnd2
	ALIGN 16
	DrawPixelBottomEnd3
	ALIGN 16
	DrawPixelBottomEnd4
	ALIGN 16
	DrawPixelBottomEnd5
	
	ALIGN 16
	ei
	pop af
	di
	push af
	out (c), c
	ret
	
	ALIGN 256
	DrawPixelTop0
	DrawPixelTopEnd1
	ALIGN 32
	DrawPixelTop1
	DrawPixelTopEnd1
	ALIGN 32
	DrawPixelTop2
	DrawPixelTopEnd1
	ALIGN 32
	DrawPixelTop3
	DrawPixelTopEnd1
	
	ALIGN 32
	DrawPixelTop0
	DrawPixelTopEnd2
	ALIGN 32
	DrawPixelTop1
	DrawPixelTopEnd2
	ALIGN 32
	DrawPixelTop2
	DrawPixelTopEnd2
	ALIGN 32
	DrawPixelTop3
	DrawPixelTopEnd2
	
	ALIGN 256
	DrawPixelTop0
	DrawPixelTopEnd3
	ALIGN 32
	DrawPixelTop1
	DrawPixelTopEnd3
	ALIGN 32
	DrawPixelTop2
	DrawPixelTopEnd3
	ALIGN 32
	DrawPixelTop3
	DrawPixelTopEnd3
	
	ALIGN 32
	DrawPixelTop0
	DrawPixelTopEnd4
	ALIGN 32
	DrawPixelTop1
	DrawPixelTopEnd4
	ALIGN 32
	DrawPixelTop2
	DrawPixelTopEnd4
	ALIGN 32
	DrawPixelTop3
	DrawPixelTopEnd4
	
	ALIGN 256
	DrawPixelTop0
	DrawPixelTopEnd5
	ALIGN 32
	DrawPixelTop1
	DrawPixelTopEnd5
	ALIGN 32
	DrawPixelTop2
	DrawPixelTopEnd5
	ALIGN 32
	DrawPixelTop3
	DrawPixelTopEnd5
	
	ALIGN 32
	DrawPixelBottom0
	DrawPixelBottomEnd1
	ALIGN 32
	DrawPixelBottom1
	DrawPixelBottomEnd1
	ALIGN 32
	DrawPixelBottom2
	DrawPixelBottomEnd1
	ALIGN 32
	DrawPixelBottom3
	DrawPixelBottomEnd1
	
	ALIGN 256
	DrawPixelBottom0
	DrawPixelBottomEnd2
	ALIGN 32
	DrawPixelBottom1
	DrawPixelBottomEnd2
	ALIGN 32
	DrawPixelBottom2
	DrawPixelBottomEnd2
	ALIGN 32
	DrawPixelBottom3
	DrawPixelBottomEnd2
	
	ALIGN 32
	DrawPixelBottom0
	DrawPixelBottomEnd3
	ALIGN 32
	DrawPixelBottom1
	DrawPixelBottomEnd3
	ALIGN 32
	DrawPixelBottom2
	DrawPixelBottomEnd3
	ALIGN 32
	DrawPixelBottom3
	DrawPixelBottomEnd3
	
	ALIGN 256
	DrawPixelBottom0
	DrawPixelBottomEnd4
	ALIGN 32
	DrawPixelBottom1
	DrawPixelBottomEnd4
	ALIGN 32
	DrawPixelBottom2
	DrawPixelBottomEnd4
	ALIGN 32
	DrawPixelBottom3
	DrawPixelBottomEnd4
	
	ALIGN 32
	DrawPixelBottom0
	DrawPixelBottomEnd5
	ALIGN 32
	DrawPixelBottom1
	DrawPixelBottomEnd5
	ALIGN 32
	DrawPixelBottom2
	DrawPixelBottomEnd5
	ALIGN 32
	DrawPixelBottom3
	DrawPixelBottomEnd5
	
	;write "deleteme.bin"