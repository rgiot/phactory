
// ----------------------------------------------------------------------------
#ifndef _MEMCPY_CXX_
#define _MEMCPY_CXX_

// ----------------------------------------------------------------------------
void memcpy( char *destination, char *source, unsigned int length )
{
	destination;
	source; 
	length;

__asm	
	push ix
	ld ix, 0
	add	ix, sp
	
	ld	e, (ix+4)
	ld	d, (ix+5)
	
	ld	l, (ix+6)
	ld	h, (ix+7)
	
	ld	c, (ix+8)
	ld	b, (ix+9)
	
	call OptimizedLDIR
	jr QuitMemCpy
	
OptimizedLDIR:
	xor a
	sub c
	and 16-1
	add a, a

	ld ( memcpy_initPadding + 1 ), a
memcpy_initPadding:
	jr nz, memcpy_initPadding

memcpy_loop:
REPT 16
	ldi
ENDM
	jp pe, memcpy_loop	
	ret
	
QuitMemCpy:
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _MEMCPY_CXX_
