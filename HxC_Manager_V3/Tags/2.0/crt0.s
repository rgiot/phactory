;; File: crt0.s
;; Generic crt0.s for a Z80
;; From SDCC..
;; Modified to suit execution on the Amstrad CPC!
;; by H. Hansen 2003

    .module crt0
	.globl	_main

	.area _HEADER (ABS)

	.org 	0x1000 ;; Start from address &1000

;; &0000-&0040 is used by low kernel jumpblock
;; Stack is already setup by CPC firmware.

init:

	;ret

	push af
   
;; .db #0xed, #0xff
	or a
	call z, initDisc
	
	call gsinit
	
	call #0xBC3B ; SCR_GET_BORDER
	ld a, c
	ld ( border+1 ), a
	xor a
	call #0xBC35 ; SCR_GET_INK
	ld a, b
	ld ( background+1 ), a
	ld a, #1
	call #0xBC35 ; SCR_GET_INK
	ld a, b
	ld ( pen+1 ), a
	call #0xbc11 ; SCR_GET_MODE
	ld ( mode+1 ), a
	
	ld a, #2
	call #0xbc0e
   ld   b,#13
   ld   c,b
   call #0xBC38 ; SCR SET BORDER   
   xor a
   ld   b,#13
   ld   c,b
   call #0xBC32 ; background   
   ld a, #1
   ld   b,#0
   ld   c,b
   call #0xBC32 ; background
   	
	;call initDisc
       
    call _main
    
    pop af
	or a
	call z, #0x0 ; reset cpc
	 	
mode:
	ld a, #0
	call #0xbc0e
border:
	ld a, #0
   ld   b,a
   ld   c,b
   call #0xBC38 ; SCR SET BORDER   
background:
	ld a, #0
   ld   b,a
   ld   c,b
   xor a
   call #0xBC32 ; background   
pen:
	ld a, #0
   ld   b,a
   ld   c,b
   ld a, #1
   call #0xBC32 ; background
   
   ret
   	
	;;call #0xbb00 ; km init
	
initDisc:
	LD      HL,(#0xBE7D)              ; Adresses variables Amsdos
    LD      A,(HL)                  ; Lecteur courant
    PUSH    AF                      ; Sauvegarde lecteur courant    
    LD BC, #0x4FF
    ADD HL, BC    
    LD      C,#7
    LD      DE,#0x40
    
    CALL    #0xBCCE                   ; Initalisation ROM DISC
    POP     AF
    LD      HL,(#0xBE7D)
    LD      (HL),A	
	ret

	.area	_HOME
	.area	_CODE
    .area   _GSINIT
    .area   _GSFINAL
        
	.area	_DATA
    .area   _BSS
    .area   _HEAP

   .area   _CODE
__clock::
	ret
	
_exit::
	ret
	
	.area   _GSINIT
gsinit::	

    .area   _GSFINAL
    ret
