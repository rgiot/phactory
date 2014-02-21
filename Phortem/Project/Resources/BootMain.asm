
; ----------------------------------------------------------------------------
	include "TrackLoaderEqu.asm"
	
	org &2200
	
; ----------------------------------------------------------------------------
BitBuster_Unpack equ &17C + 3

; ---------------------------------------------------------------------------
	ld hl, ( &BE7D )
	ld a, (hl)
	ld ( SetDriveValue + 1 ), a
	
	ld sp, &0080
	
	ld hl, &C9FB
	ld ( &0038 ), hl
	
	; FDC wait
	ld a, 50*3
	ld ( &0030 ), a
	
	ld bc, &7FC3
	out (c), c
	ld a, c
	ld ( &0000 ), a
	
	xor a	
	ld ( &0037 ), a ; IS PLUS FEATURE ENABLED	
	
	ei
	
	call InitTrackLoader1	
	
	call _InitLines
	
	ld a, 4
wait1:
	ld hl, &1000
	ld de, &1000
	ld bc, &8000
	ldir
	dec a
	jr nz, wait1
	
	ld b, &F5
WaitVBL_Loop2:
	in a, (c)
	rra
	jr nc, WaitVBL_Loop2
	
	call SetBlackPalette
	
	ld bc, &7F8C ; MODE 0
	out (c), c
	
	ld hl, BootStartMenu_Top+1
	ld de, &8080
	call BitBuster_Unpack
	
	ld hl, BootStartMenu_Bottom+1
	ld de, &4000
	call BitBuster_Unpack
	
	ld hl, &1000
	ld de, &1000
	ld bc, &8000
	ldir
	
	call InitCRTC
	
	ld a, 4
wait2:
	ld hl, &1000
	ld de, &1000
	ld bc, &8000
	ldir
	dec a
	jr nz, wait2
		
	call IsPlus
	jr nz, OldMachine
PlusMachine:
	ld hl, SelectOldPalette_PackedData+1
	call DrawStateBitmap
	ld a, 1
	ld ( &0037 ), a
	ld ( IsPlusMachine ), a
	call UnlockASIC
	call WaitVBL
	ld hl, BootStartMenu_AsicPalette1
	call SetDelayedAsicPalette
	ld hl, BootStartMenu_AsicPalette2
	call SetDelayedAsicPalette
	ld hl, BootStartMenu_AsicPalette3
	call SetDelayedAsicPalette		
	call WaitVBL
	ld hl, BootStartMenu_Palette
	call SetPaletteSkip4Plus
	jr EndDetectedMachine
OldMachine:
	ld hl, AmstradPlusNotDetected_PackedData+1
	call DrawStateBitmap	
	xor a
	ld ( &0037 ), a
	ld ( IsPlusMachine ), a
	call WaitVBL
	ld hl, BootStartMenu_Palette1
	call SetDelayedPalette
	ld hl, BootStartMenu_Palette2
	call SetDelayedPalette
	ld hl, BootStartMenu_Palette3
	call SetDelayedPalette	
	call WaitVBL
	ld hl, BootStartMenu_Palette
	call SetPalette
EndDetectedMachine:

	; PART 1 as default
	xor a
	ld ( &8060 ), a
		
InfiniteLoop:
	call WaitVBL
	
	call SwitchArrow
	
	call ReadKeyboardMatrix
	
	; http://www.cpcwiki.eu/index.php/Programming:Keyboard_scanning
	
	; KEY 1
	ld a, (KeyboardMatrixBuffer+8)
	bit 0, a
	jp nz, StartDemoPart1
	ld a, (KeyboardMatrixBuffer+1)
	bit 5, a
	jp nz, StartDemoPart1
	
	; KEY 2
	ld a, (KeyboardMatrixBuffer+8)
	bit 1, a
	jp nz, StartDemoPart2
	ld a, (KeyboardMatrixBuffer+1)
	bit 6, a
	jp nz, StartDemoPart2

	; KEY 3
	ld a, (KeyboardMatrixBuffer+7)
	bit 1, a
	jp nz, StartDemoPart3
	ld a, (KeyboardMatrixBuffer+0)
	bit 5, a
	jp nz, StartDemoPart3

	; CURSOR UP
	ld a, (KeyboardMatrixBuffer+0)
	bit 0, a
	jp nz, CursorUp

	; CURSOR DOWN
	ld a, (KeyboardMatrixBuffer+0)
	bit 2, a
	jp nz, CursorDown

	; CURSOR RIGHT
	ld a, (KeyboardMatrixBuffer+0)
	bit 1, a
	jp nz, CursorRight

	; CURSOR LEFT
	ld a, (KeyboardMatrixBuffer+1)
	bit 0, a
	jp nz, CursorLeft
	
	ld a, (KeyOPressed)
	or a
	jr nz, SkipKeyO
	; KEY O
	ld a, (KeyboardMatrixBuffer+4)
	bit 2, a
	jp nz, KeyO	
SkipKeyO:

	; SPACE
	ld a, (KeyboardMatrixBuffer+5)
	bit 7, a
	jp nz, StartDemo
	jp InfiniteLoop
	
KeyO:
	ld a, (IsPlusMachine)
	or a
	jp z, InfiniteLoop
	ld hl, OldPaletteSelected_PackedData+1
	call DrawStateBitmap
	xor a
	ld ( &0037 ), a
	call LockASIC
	call WaitVBL
	ld hl, BootStartMenu_Palette
	call SetPalette	
	ld a, 1
	ld ( KeyOPressed ), a	
	jp InfiniteLoop
	
CursorUp:
	ld hl, CurrentVerticalOffset
	call IncreaseValue
	ld hl, VerticalValues
	call SetNewCRTCValues	
	jp CursorMoveQuit
	
CursorDown:
	ld hl, CurrentVerticalOffset
	call DecreaseValue
	ld hl, VerticalValues
	call SetNewCRTCValues	
	jp CursorMoveQuit
	
CursorLeft:
	ld hl, CurrentHorizontalOffset
	call DecreaseValue
	ld hl, HorizontalValues
	call SetNewCRTCValues	
	jp CursorMoveQuit
	
CursorRight:
	ld hl, CurrentHorizontalOffset
	call IncreaseValue
	ld hl, HorizontalValues
	call SetNewCRTCValues

CursorMoveQuit:
	jp InfiniteLoop

StartDemoPart3:
	ld a, 2
	ld ( &0004 ), a
	ld a, 4
	ld ( &8060 ), a
	jr StartDemo
StartDemoPart2:
	ld a, 1
	ld ( &0004 ), a
	ld a, 2
	ld ( &8060 ), a
	jr StartDemo
StartDemoPart1:
	ld a, 1
	ld ( &0004 ), a
	xor a	
	ld ( &8060 ), a
StartDemo:	

	ld a, (ArrowCount)
	cp 42-1
	jr nc, SkipFadeInArrow	
FadeInArrowQuit:
	call WaitVBL
	call SwitchArrow	
	ld a, (CurrentArrow)
	or a
	jr nz, FadeInArrowQuit
SkipFadeInArrow:
	
	ld a, (&0037)
	or a
	jp nz, PlusFade	
	ld hl, BootStartMenu_Palette3
	call SetDelayedPalette
	ld hl, BootStartMenu_Palette2
	call SetDelayedPalette
	ld hl, BootStartMenu_Palette1
	call SetDelayedPalette	
	jr FadeQuit
PlusFade:
	ld hl, BootStartMenu_AsicPalette3
	call SetDelayedAsicPalette
	ld hl, BootStartMenu_AsicPalette2
	call SetDelayedAsicPalette
	ld hl, BootStartMenu_AsicPalette1
	call SetDelayedAsicPalette
FadeQuit:
	
	call WaitVBL
	call SetBlackPalette	
	
	ld a, (&8060)
	or a
	jr z, SkipClearScreen
	
	ld hl, &4000
	ld (hl), 0
	ld de, &4001
	ld bc, &27FF
	ldir
	
SkipClearScreen:
	
	call InitTrackLoader2		
	call InitMusic
	call InitMain	
	
	ld hl, &8080
	ld (hl), 0
	ld de, &8081
	ld bc, &27FF
	ldir
	
	ld hl, &4000
	ld (hl), 0
	ld de, &4001
	ld bc, &27FF
	ldir
	
	di
	ld bc, &7FC1
	out (c), c
	ld a, c
	ld ( &0000 ), a
	ei
	
	jp &EF20
	
; ----------------------------------------------------------------------------
InitTrackLoader1:
	ld hl, TrackLoader_PackedBinary
	ld de, &E800
	ld bc, TrackLoader_PackedBinary_End - TrackLoader_PackedBinary
	ldir	
	ret
	
InitTrackLoader2:		
	call DiscMotorOn
	
SetDriveValue:
	ld	a, 0
	call SetDrive	
	call RecalDrive
	jp CacheDir
	
TrackLoader_PackedBinary:
	incbin "TrackLoader.bin"
TrackLoader_PackedBinary_End:

; ----------------------------------------------------------------------------
	include "InitLines.asm"
	
; ----------------------------------------------------------------------------
InitMain:	
	ld hl, BootAvailableMem
	ld a, 1
	ld ( FileOrderPtr ), a
	call LoadFile
	
	ld hl, BootAvailableMem+1
	ld de, &EF20
	jp BitBuster_Unpack
	
; ----------------------------------------------------------------------------
InitMusic:	
	di	
	ld hl, InterruptPlayMusic
	ld de, &0005
	ld bc, InterruptPlayMusic_End - InterruptPlayMusic
	ldir
	ld hl, &0005
	call SetMusicRoutine
	ei
	ret
	
InterruptPlayMusic:
	ld bc, &7FC5
	out (c), c
	
	ld a, ( &0004 )
	or a
	call z, &5103
	
	ld a, ( &0000 )
	ld b, &7F
	ld c, a
	out (c), c	
	ret
InterruptPlayMusic_End:

; ----------------------------------------------------------------------------
InitCRTC:
	ld hl, CRTC_Table
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
	db 4, &3D ; 62
	db 5, 2 ;
	db 12, &2C
	db 13, &40
	db 0

; ----------------------------------------------------------------------------
SetNewCRTCValues:
	ld b, 0
	ld c, a
	add hl, bc
	add hl, bc
	add hl, bc
	add hl, bc
	
	ld b, &BC
	ld c, (hl)
	out (c), c
	inc hl
	inc b
	ld a, (hl)
	out (c), a
	inc hl
	dec b
	ld c, (hl)
	out (c), c
	inc hl
	inc b
	ld a, (hl)
	out (c), a
	inc hl
	dec b
	
	ld a, 5
WaitNextKey:
	push af	
	call WaitVBL	
	pop af
	dec a
	jr nz, WaitNextKey	
	
	ret
	
DecreaseValue:
	ld a, (hl)
	dec a
	cp -1
	jr nz, DecreaseValue_End
	xor a
DecreaseValue_End:
	ld (hl), a
	ret

IncreaseValue:
	ld a, (hl)
	inc a
	cp 5
	jr nz, IncreaseValue_End
	ld a, 4
IncreaseValue_End:
	ld (hl), a
	ret
	
CurrentHorizontalOffset:
	db 2
CurrentVerticalOffset:
	db 2
	
HorizontalValues:
	db 3, 11, 2, 52
	db 3, 12, 2, 51
	db 3, 13, 2, 50 ; default
	db 3, 14, 2, 49
	db 3, 15, 2, 48
	
VerticalValues:
	db 7, 47, 7, 47
	db 7, 48, 7, 48
	db 7, 49, 7, 49 ; default
	db 7, 50, 7, 50
	db 7, 51, 7, 51
	
; ----------------------------------------------------------------------------
WaitVBL:	
	ld b, &F5
WaitVBL_Loop:
	in a, (c)
	rra
	jp nc, WaitVBL_Loop	

	ld hl, &8080
	ld de, &8080
	ld bc, &100
	ldir
	
	ld a, ( &0030 )
	dec a
	jr nz, NoWaitFDCReset
	inc a
NoWaitFDCReset:	
	ld ( &0030 ), a
	ret
	
; ----------------------------------------------------------------------------
SetDelayedPalette:
	push hl
	call WaitVBL
	pop hl
	call SetPalette
	
	ld hl, &1000
	ld de, &1000
	ld bc, &4000
	ldir
	
	ret

; ----------------------------------------------------------------------------
SetDelayedAsicPalette:
	push hl
	call WaitVBL
	pop hl
	
	xor a
SetDelayedAsicPaletteLoop:
	push af
	push hl
	ld d, (hl)
	ld e, a
	ld hl, BootStartMenu_Palette
	call SetAsicColor
	pop hl
	pop af
	inc hl
	inc a
	cp 16
	jr nz, SetDelayedAsicPaletteLoop
	
	ld hl, &1000
	ld de, &1000
	ld bc, &4000
	ldir
	
	ret

; ----------------------------------------------------------------------------
ReadKeyboardMatrix:
	ld hl, KeyboardMatrixBuffer
	ld bc, &F40E
	out (c), c
	ld b, &F6
	in a, (c)
	and &30
	ld c, a
	or &C0
	out (c), a
	out (c), c
	inc b
	ld a, &92
	out (c), a
	push bc
	set 6, c
ScanKey:
	ld b, &F6 
	out (c), c
	ld b, &F4
	in a, (c)
	cpl
	ld (hl), a
	inc hl
	inc c
	ld a, c
	and &0F
	cp &0A
	jr nz, ScanKey
	pop bc
	ld a, &82
	out (c), a
	dec b
	out (c), c
	ret	
KeyboardMatrixBuffer:
	ds 10
	
; ----------------------------------------------------------------------------
BootAvailableMem:

; ----------------------------------------------------------------------------
UnpackBuffer equ &400
DrawStateBitmap:
	ld de, UnpackBuffer
	call BitBuster_Unpack
	
	ld hl, UnpackBuffer
	ld de, &C000+96*7-&8000
	ld bc, 96
	ldir
	ld de, &C800+96*7-&8000
	ld bc, 96
	ldir
	ld de, &D000+96*7-&8000
	ld bc, 96
	ldir
	ld de, &D800+96*7-&8000
	ld bc, 96
	ldir
	ld de, &E000+96*7-&8000
	ld bc, 96
	ldir
	ld de, &C000+96*8-&8000
	ld bc, 96
	ldir
	ld de, &C800+96*8-&8000
	ld bc, 96
	ldir
	ld de, &D000+96*8-&8000
	ld bc, 96
	ldir
	ret

; ----------------------------------------------------------------------------
IsPlus:
	call UnlockASIC	
	ld hl, &5FFF
	ld (hl), 0	
	ld bc, &7FB8+4 ; page asic
	out (c), c	
	ld (hl), 55
	ld bc, &7FA0+4+8
	out (c), c
	ld a, (hl)
	or a ; if nz then plus
	ret
	
; ----------------------------------------------------------------------------
UnlockASIC:
	di
	ld b, &BC
	ld hl, ASICUnlockData
	ld e, 17
UnlockASIC_Loop:
	ld a, (hl)
	out (c), a
	inc hl
	dec e
	jr nz, UnlockASIC_Loop	
	ei
	ret	
ASICUnlockData:
	db &FF, &00, &FF, &77
	db &b3, &51, &A8, &D4
	db &62, &39, &9C, &46
	db &2B, &15, &8A, &CD
	db &EE
	
; ----------------------------------------------------------------------------
LockASIC:
	di
	ld b, &BC
	ld hl, ASICLockData
	ld e, 16
LockASIC_Loop:
	ld a, (hl)
	out (c), a
	inc hl
	dec e
	jr nz, LockASIC_Loop	
	ei
	ret	
ASICLockData:
	db &FF, &00, &FF, &77
	db &b3, &51, &A8, &D4
	db &62, &39, &9C, &46
	db &2B, &15, &8A, &00
	
; ----------------------------------------------------------------------------
SetAsicColor:
	di
	ld bc, &7FB8
	out (c), c
	
	push hl
	
	ld b, 0
	ld c, e
	ld hl, &6400
	add hl, bc
	add hl, bc
	
	ld a, d
	ex de, hl
	
	pop hl
	
	; skip Gate Array palette
	ld bc, 16
	add hl, bc
	
	add a, a
	ld c, a
	add hl, bc
	
	ldi
	ldi
	
	ld bc, &7FA0
	out (c), c
	ei
	ret
	
; ----------------------------------------------------------------------------
COLOR0	equ &54
COLOR1	equ &44
COLOR2	equ &55
COLOR3	equ &5C
COLOR4	equ &58
COLOR5	equ &5D
COLOR6	equ &4C
COLOR7	equ &45
COLOR8	equ &4D
COLOR9	equ &56	
COLOR10	equ &46
COLOR11	equ &57
COLOR12	equ &5E
COLOR13	equ &40
COLOR14	equ &5F
COLOR15	equ &4E
COLOR16	equ &47
COLOR17	equ &4F
COLOR18	equ &52
COLOR19	equ &42
COLOR20	equ &53
COLOR21	equ &5A
COLOR22	equ &59
COLOR23	equ &5B
COLOR24	equ &4A
COLOR25	equ &43
COLOR26	equ &4B

; ----------------------------------------------------------------------------
BootStartMenu_Palette:
	incbin "Boot.cpcbitmap.palette"
BootStartMenu_Palette1:
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR9
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR1
	db COLOR0
	db COLOR3
	db COLOR0
	db COLOR0
BootStartMenu_AsicPalette1:
	db 0
	db 0
	db 0
	db 0
	db 3
	db 0
	db 0
	db 0
	db 0
	db 0
	db 0
	db 5
	db 0
	db 6
	db 0
	db 0
BootStartMenu_Palette2:
	db COLOR0
	db COLOR9
	db COLOR0
	db COLOR0
	db COLOR18
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR0
	db COLOR15
	db COLOR0
	db COLOR5
	db COLOR1
	db COLOR3
	db COLOR0
	db COLOR0
BootStartMenu_AsicPalette2:
	db 0
	db 3
	db 0
	db 0
	db 2
	db 0
	db 0
	db 0
	db 0
	db 10
	db 0
	db 7
	db 5
	db 6
	db 0
	db 0
BootStartMenu_Palette3:
	db COLOR0
	db COLOR18
	db COLOR9
	db COLOR0
	db COLOR20
	db COLOR0
	db COLOR0
	db COLOR4
	db COLOR1
	db COLOR7
	db COLOR3
	db COLOR14
	db COLOR5
	db COLOR15
	db COLOR0
	db COLOR0
BootStartMenu_AsicPalette3:
	db 0
	db 2
	db 3
	db 0
	db 1
	db 0
	db 0
	db 8
	db 5
	db 9
	db 6
	db 12
	db 7
	db 10
	db 0
	db 0
BootStartMenu_Top:
	incbin "StartMenu.bmp.topBin.pck"
BootStartMenu_Bottom:
	incbin "StartMenu.bmp.bottomBin.pck"
AmstradPlusNotDetected_PackedData:
	incbin "StartMenu_Y135_AmstradPlusNotDetected.bmp.sprRawData1.pck"
SelectOldPalette_PackedData:
	incbin "StartMenu_Y135_SelectOldPalette.bmp.sprRawData1.pck"
OldPaletteSelected_PackedData:
	incbin "StartMenu_Y135_OldPaletteSelected.bmp.sprRawData1.pck"

; ----------------------------------------------------------------------------
ArrowCount:
	db 42-1 ; From Hide
	
CurrentArrow:
	db 0
	
SwitchArrow:
	ld a, (ArrowCount)
	inc a
	and 127
	ld (ArrowCount), a
	cp 0
	jp z, ShowArrow3
	cp 4
	jp z, ShowArrow2
	cp 8
	jp z, ShowArrow1
	cp 12
	jp z, HideArrow
	cp 30
	jp z, ShowArrow1
	cp 34
	jp z, ShowArrow2
	cp 38
	jp z, ShowArrow3
	cp 42
	jp z, ShowArrow
	ret
	
HideArrow:
	ld a, ( &0037 )
	or a
	jp nz, HideArrowPlus
	
	ld bc, &7F01
	out (c), c
	ld a, COLOR0
	out (c), a
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a	
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a
	
	ret
HideArrowPlus:
	ld hl, BootStartMenu_Palette
	ld e, 1
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 2
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 3
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 4
	ld d, 0
	jp SetAsicColor
	
ShowArrow:
	xor a
	ld ( CurrentArrow ), a
	ld a, ( &0037 )
	or a
	jp nz, ShowArrowPlus
	ld bc, &7F01
	out (c), c
	ld a, COLOR20
	out (c), a
	inc c
	out (c), c
	ld a, COLOR18
	out (c), a	
	inc c
	out (c), c
	ld a, COLOR9
	out (c), a
	inc c
	out (c), c
	ld a, COLOR26
	out (c), a
	ret	
ShowArrowPlus:
	ld hl, BootStartMenu_Palette
	ld e, 1
	ld d, 1
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 2
	ld d, 2
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 3
	ld d, 3
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 4
	ld d, 4
	jp SetAsicColor

ShowArrow1:
	ld a, 1
	ld ( CurrentArrow ), a
	ld a, ( &0037 )
	or a
	jp nz, ShowArrowPlus1
	ld bc, &7F01
	out (c), c
	ld a, COLOR0
	out (c), a
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a	
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a
	inc c
	out (c), c
	ld a, COLOR9
	out (c), a
	ret	
ShowArrowPlus1:
	ld hl, BootStartMenu_Palette
	ld e, 1
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 2
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 3
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 4
	ld d, 3
	jp SetAsicColor
	
ShowArrow2:
	ld a, 2
	ld ( CurrentArrow ), a
	ld a, ( &0037 )
	or a
	jp nz, ShowArrowPlus2
	ld bc, &7F01
	out (c), c
	ld a, COLOR9
	out (c), a
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a	
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a
	inc c
	out (c), c
	ld a, COLOR18
	out (c), a
	ret	
ShowArrowPlus2:
	ld hl, BootStartMenu_Palette
	ld e, 1
	ld d, 3
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 2
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 3
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 4
	ld d, 2
	jp SetAsicColor
	
ShowArrow3:
	ld a, 3
	ld ( CurrentArrow ), a
	ld a, ( &0037 )
	or a
	jp nz, ShowArrowPlus3
	ld bc, &7F01
	out (c), c
	ld a, COLOR18
	out (c), a
	inc c
	out (c), c
	ld a, COLOR9
	out (c), a	
	inc c
	out (c), c
	ld a, COLOR0
	out (c), a
	inc c
	out (c), c
	ld a, COLOR20
	out (c), a
	ret	
ShowArrowPlus3:
	ld hl, BootStartMenu_Palette
	ld e, 1
	ld d, 2
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 2
	ld d, 3
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 3
	ld d, 0
	call SetAsicColor
	ld hl, BootStartMenu_Palette
	ld e, 4
	ld d, 1
	jp SetAsicColor
	
; ----------------------------------------------------------------------------
SetBlackPalette:
	ld a, ( &0037 )
	or a
	jp nz, SetBlackPalettePlus
	
	xor a
BlackPalette_Loop:
	ld b, &7f
	ld c, a
	di
	out (c), c
	ld c, COLOR0
	out (c), c
	ei
	
	inc a
	cp 17 ; border included
	jr nz, BlackPalette_Loop
	jr SetBlackPaletteQuit
	
SetBlackPalettePlus:
	di
	ld bc, &7FB8
	out (c), c
	
	ld hl, &6400
	ld c, 16*2
	xor a
SetBlackPalettePlusLoop:
	ld (hl), a
	inc hl
	dec c
	jr nz, SetBlackPalettePlusLoop
	
	ld bc, &7FA0
	out (c), c
	ei

SetBlackPaletteQuit:
	ret
	
; ----------------------------------------------------------------------------
SetPalette:
	ld a, ( &0037 )
	or a
	jp nz, SetPalettePlus

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
	jr SetPaletteQuit
	
SetPalettePlus:
	di
	ld bc, &7FB8
	out (c), c
	
	ld bc, 16
	add hl, bc	
	ld de, &6400
	ld bc, 16*2
	ldir
	
	ld bc, &7FA0
	out (c), c
	ei
SetPaletteQuit:	
	ret
	
; ----------------------------------------------------------------------------
SetPaletteSkip4Plus:	
	di
	ld bc, &7FB8
	out (c), c
	
	ld bc, 16+4+4
	add hl, bc	
	ld de, &6400 + 4+4
	ld bc, 12*2
	ldir
	
	ld bc, &7FA0
	out (c), c
	ei
	ret

; ----------------------------------------------------------------------------
KeyOPressed:
	db 0
IsPlusMachine:
	db 0