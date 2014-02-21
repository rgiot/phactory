; FILE: cpcbioscall.s

;-------------------------------------------------------------------------------------------
;char _read_sector(unsigned char * buffer,unsigned char drive,unsigned char track,unsigned char sector);

_read_sector::

        ld a,#01
        ld (0xbe66),a ; inhibe les erreurs
        ld (0xbe78),a ;       1 seul essai
        ld      c,#07
        call    0xb90f   ;ROM7 disquette

        ld      hl,#4
        add     hl,sp
        ld      e,(hl)   ;drive number

        ld      hl,#5
        add     hl,sp
        ld      d,(hl)   ;track number

        ld      hl,#6
        add     hl,sp
        ld      c,(hl)   ; sector number

        ld      hl,#2
        add     hl,sp
        ld      a,(hl)

        ld      hl,#3
        add     hl,sp
        ld      h,(hl)   ; PTR
        ld      l,a

        call  0xC03C
        ld l,a

        call    0xb903   ; disable upper ROM

        ret

;-------------------------------------------------------------------------------------------
;char _write_sector(unsigned char * buffer,unsigned char drive,unsigned char track,unsigned char sector);

_write_sector::

        ld a,#01
        ld (0xbe66),a ; inhibe les erreurs
        ld (0xbe78),a ;       1 seul essai
        ld      c,#07
        call    0xb90f   ;ROM7 disquette

        ld      hl,#4
        add     hl,sp
        ld      e,(hl)   ;drive number

        ld      hl,#5
        add     hl,sp
        ld      d,(hl)   ;track number

        ld      hl,#6
        add     hl,sp
        ld      c,(hl)   ; sector number

        ld      hl,#2
        add     hl,sp
        ld      a,(hl)

        ld      hl,#3
        add     hl,sp
        ld      h,(hl)   ; PTR
        ld      l,a

        call  0xC03F
        ld l,a

        call    0xb903   ; disable upper ROM

        ret


fpcfgbuffer:
		.db #0x23
		.db #0x00
		.db #0xFF
		.db #0x03
		.db #0xAF
		.db #0x02
		.db #0x0A
		.db #0x01
		.db #0x03
		
fpcfgbuffer_default:
		.db #0x32
		.db #0x00
		.db #0xFA
		.db #0x00
		.db #0xAF
		.db #0x0F
		.db #0x0C
		.db #0x01
		.db #0x03
		
;-------------------------------------------------------------------------------------------
;void _cfg_disk_drive();

_cfg_disk_drive::
		ld      c,#07
        call    0xb90f   ;ROM7 disquette

        ld hl, #fpcfgbuffer
        call 0xC036
        
        DI
        ld bc,#0xFB7E
        ld a,#0x03
        call putfdc
        ld a,#0xF1
        call putfdc
        ld a,#0x03
        call putfdc
        EI
        
        call    0xb903   ; disable upper ROM
        ret


putfdc:
        push af
lieta:
        in a,(c)
        rla
        jr nc,lieta
sors: 
        inc c
        pop af
        out (c),a
        dec  c
        ret

;-------------------------------------------------------------------------------------------
;void _move_to_track(unsigned char track);

_move_to_track::

        ld      c,#07
        call    0xb90f   ;ROM7 disquette


        LD HL,(#0xBE7D)
        LD E,(HL) ; lecteur courant dans A (0 ou 1)

        ld      hl,#2
        add     hl,sp
        ld      d,(hl)
        call 0xC045

        call    0xb903   ; disable upper ROM
        ret
;-------------------------------------------------------------------------------------------
;char _wait_key();

_wait_key::
        call  0xBB18
        
		;;.db #0xed, #0xff
        
		ld l,a
        
        ret

;char _wait_key2();

_wait_key2::
        call  0xBB06
        
		ld l,a

        ret
        
        
;char _hasOtherChar();

_hasOtherChar::
        call 0xBB09
		jr nc, hasOtherChar_no        
        call 0xBB06        
		ld l, a
		ret
hasOtherChar_no:
		xor a
		ld l,a
        ret

;char _reboot();

_reboot::
        call 0x0
	
; IN: a = char to print
; IN: de = screen ptr
PrivatePrintChar:	
	ld l, a
	ld h, #0
	add hl, hl ; * 2
	add hl, hl ; * 4
	add hl, hl ; * 8
	ld bc, #0x3800
	add hl, bc
	
	ld b, #8
	ld a, ( hl )
	ld ( de ), a
	inc hl
	set 3, d
	ld a, ( hl )
	ld ( de ), a
	inc hl
	ld a, d
	add a, b
	ld d, a
	ld a, ( hl )
	ld ( de ), a
	inc hl
	set 3, d
	ld a, ( hl )
	ld ( de ), a
	inc hl
	ld a, d
	add a, b
	ld d, a
	ld a, ( hl )
	ld ( de ), a
	inc hl
	set 3, d
	ld a, ( hl )
	ld ( de ), a
	inc hl
	ld a, d
	add a, b
	ld d, a
	ld a, ( hl )
	ld ( de ), a
	inc hl
	set 3, d
	ld a, ( hl )
	ld ( de ), a

	ret

strBuffer:
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	.db #0, #0, #0, #0, #0, #0, #0, #0
	
;-------------------------------------------------------------------------------------------
;void fastPrintString(unsigned char *screenBuffer, unsigned char *string );
.globl _fastPrintString
_fastPrintString::
	pop bc
	pop de
	pop hl
	
	push hl
	push de
	push bc
	
	;.db 0xed, 0xff
	
	push de
	
	ld de, #strBuffer
	ld bc, #80
	ldir
	
	call #0xB906 ; KL_L_ROM_ENABLE
	
	ld ix, #strBuffer
	pop de
	
	; ix = string
	; de = screenBuffer
	
fastPrintString_loop:
	ld a, ( ix )
	or a
	jr z, fastPrintString_loop_end
	inc ix
	push de
	call PrivatePrintChar
	pop de
	inc de
	jr fastPrintString_loop
	
fastPrintString_loop_end:
	jp 0xB909 ; KL_L_ROM_DISABLE
	
;-------------------------------------------------------------------------------------------
;void clear_line(unsigned char y_pos);

_clear_line::

        ld      hl,#2
        add     hl,sp
        ld      a,(hl)
        
        ld hl, #0xc000
        ld bc, #0x0050
        
        or a
        
clearLineCalcYLoop:
        jr z, endClearLineCalcY
        add hl, bc
        dec a
        jr clearLineCalcYLoop
        
endClearLineCalcY:

		ld a, #8
clearLineDrawLoop:
		push af
		push hl
		ld d, h
		ld e, l
		inc de
		ld bc, #79
		ld ( hl ), #0
		ldir
		pop hl
		ld bc, #0x800
		add hl, bc
		pop af
		dec a
		jr nz, clearLineDrawLoop

        ret
        
;-------------------------------------------------------------------------------------------
;void invert_line(unsigned char y_pos);

_invert_line::

        ld      hl,#2
        add     hl,sp
        ld      a,(hl)
        
        ld hl, #0xc000
        ld bc, #0x0050
        
        or a
        
invertLineCalcYLoop:
        jp z, endInvertLineCalcY
        add hl, bc
        dec a
        jp invertLineCalcYLoop
        
endInvertLineCalcY:

		ld a, #8
invertLineDrawLoop:
		push af
		push hl
		ld c, #79
invertLineDrawLoopScanline:
		ld a, (hl)
		xor #255
		ld ( hl ), a
		inc hl
		dec c
		jp nz, invertLineDrawLoopScanline
		pop hl
		ld bc, #0x800
		add hl, bc
		pop af
		dec a
		jp nz, invertLineDrawLoop

        ret