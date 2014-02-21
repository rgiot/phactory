
// ----------------------------------------------------------------------------
#ifndef _MEMSET_CXX_
#define _MEMSET_CXX_

// ----------------------------------------------------------------------------
void memset( char *destination, char value, int length )
{
	destination; 
	value; 
	length;

__asm
	push ix
	ld ix, 0
	add	ix, sp
	
	ld	e, (ix+4)
	ld	d, (ix+5)
	
 	ld	a, (ix+6)
	
	ld	c, (ix+7)
	ld	b, (ix+8)

	ld h, d
	ld l, e
	dec bc
	inc de
	ld ( hl ), a
	call OptimizedLDIR
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
#endif // _MEMSET_CXX_
