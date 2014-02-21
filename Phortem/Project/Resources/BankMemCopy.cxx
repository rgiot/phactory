
// ----------------------------------------------------------------------------
#ifndef _BANKCOPY_CXX_
#define _BANKCOPY_CXX_

// ----------------------------------------------------------------------------
void bank_memcpy( unsigned char destBank, char *destination, char *source, unsigned int length )
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
	
	call bank_OptimizedLDIR
	
	di
	pop af
	ld b, &7F
	ld c, a
	out (c), c
	ld ( &0000 ), a
	ei
	
	jp bank_QuitMemCpy
	
bank_OptimizedLDIR:
	xor a
	sub c
	and 32-1
	add a, a
	ld ( bank_memcpy_initPadding + 1 ), a
bank_memcpy_initPadding:
	jr nz, bank_memcpy_initPadding

bank_memcpy_loop:
REPT 32
	ldi
ENDM
	jp pe, bank_memcpy_loop	
	ret
	
bank_QuitMemCpy:
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _BANKCOPY_CXX_
