
	org &170
	
start_line_10:
	defw end_line_10-start_line_10  ;; length of basic line in bytes
	defw 10  						;; basic line number
	defb &83 						;; CALL token
	defb &1c 						;; & hex value
	defw main 						;; the value
	defb 0 							;; end of basic line
end_line_10:
	
	defw 0 							;; indicates no more basic lines
	db 0
	
	;; binary data starts here...
main:
	
	; IMPORTANT
	; do not place any code between org and the following jp
	
	jp BootMain_Unpack
	
	include "BitBuster.asm"
	
; ----------------------------------------------------------------------------
BootMain_Unpack:	
	di
	
	ld bc, &FA7E
	ld a, 1
	out (c), a
	
	ld hl, BootMain_Data+1
	ld de, &2200
	call BitBuster_Unpack
	
	call _LeaveAmsdos	
	jp &2200

; ----------------------------------------------------------------------------
BootMain_Data:	
	incbin "BootMain.bin.pck"
	
; ----------------------------------------------------------------------------
	include "LeaveAmsdos.asm"
	