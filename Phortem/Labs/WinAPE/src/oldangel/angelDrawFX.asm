
; ----------------------------------------------------------------------------
	read "angelDrawFXBANK0.asm"
	read "angelDrawFXBANK1.asm"
	read "angelDrawFXBANK2.asm"
	read "angelDrawFXBANK3.asm"

; ----------------------------------------------------------------------------
AngelDrawFX:
	ld a, (IsFlipped)
	or a
	jp z, AngelDrawFX_BANK2BANK3

; ----------------------------------------------------------------------------
AngelDrawFX_BANK0BANK1:
	ld bc, &7FC6
	out (c), c		
	
	call AngelDrawFX_BANK0
	
	ld bc, &7FC1
	out (c), c		
	
	call AngelDrawFX_BANK1
	
	ld bc, &7FC0
	out (c), c
	ret
	
; ----------------------------------------------------------------------------
AngelDrawFX_BANK2BANK3:
	ld bc, &7FC6
	out (c), c		
	
	call AngelDrawFX_BANK2
	
	ld bc, &7FC3
	out (c), c		
	
	call AngelDrawFX_BANK3
	
	ld bc, &7FC0
	out (c), c
	
	ret