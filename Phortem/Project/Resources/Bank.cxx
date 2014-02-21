
// ----------------------------------------------------------------------------
#ifndef _BANK_CXX_
#define _BANK_CXX_

// ----------------------------------------------------------------------------
#define STACKBANKSIZE 4
static unsigned char stackBanks[ STACKBANKSIZE ];
static unsigned char stackBankIndex;

// ----------------------------------------------------------------------------
// never calls directly DirectSetBank, use PushBank/PopBank instead !
void DirectSetBank( unsigned char bank )
{
	bank;

__asm
	push ix
	ld ix, 0
	add	ix, sp
	
	ld a, ( ix + 4 )
	
	di
	ld ( &0000 ), a	
	ld bc, &7f00
	out (c), c
	out (c), a	
	ei
	
	pop ix
__endasm;
}

// ----------------------------------------------------------------------------
void PushBank( unsigned char bank )
{
	stackBanks[ stackBankIndex ] = bank;
	
	stackBankIndex++;
	
	DirectSetBank( bank );
}

// ----------------------------------------------------------------------------
void PopBank()
{
	char bank;

	stackBankIndex--;
	if ( stackBankIndex == 0 )
	{
		bank = BANKC0;
	}
	else
	{
		bank = stackBanks[ stackBankIndex - 1 ];
	}
	
	DirectSetBank( bank );
}

// ----------------------------------------------------------------------------
unsigned char GetBank()
{
	return stackBanks[ stackBankIndex - 1 ];
}

// ----------------------------------------------------------------------------
#endif // _BANK_CXX_
