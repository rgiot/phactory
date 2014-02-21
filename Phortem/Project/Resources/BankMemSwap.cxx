
// ----------------------------------------------------------------------------
#ifndef _BANKMEMSWAP_CXX_
#define _BANKMEMSWAP_CXX_

// ----------------------------------------------------------------------------
void bank_memswap( unsigned char destBank, char *destination, char *source, unsigned int length )
{
	destBank;
	destination;
	source; 
	length;

__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	di
	ld a, ( &0000 )
	push af
	ld a, (ix+4)	
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	ld e, (ix+5)
	ld d, (ix+6)
	
	ld l, (ix+7)
	ld h, (ix+8)
	
	ld c, (ix+9)
	ld b, (ix+10)
	
bank_memswap_loop:
	
	ex af, af'
	ld a, (de)
	ex af, af'
	
	ld a, (hl)	
	ld (de), a
	
	ex af, af'
	ld (hl), a
	ex af, af'
		
	inc hl	
	inc de
	 
	dec bc
	ld a, b
	or c
	jr nz, bank_memswap_loop
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei	
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _BANKMEMSWAP_CXX_
