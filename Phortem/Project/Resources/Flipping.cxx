
// ----------------------------------------------------------------------------
#ifndef _FLIPPING_CXX_
#define _FLIPPING_CXX_

// ----------------------------------------------------------------------------
static char IsFlipped;

// ----------------------------------------------------------------------------
char **GetHighLines()
{
	return BANK2LINES_TOPPTR;
}

// ----------------------------------------------------------------------------
char **GetLowLines()
{
	return BANK0LINES_TOPPTR;
}

// ----------------------------------------------------------------------------
void FlipScreen()
{
	if ( !IsFlipped )
	{
		IsFlipped = 1;

		//SetCRTC( 12, 0x0C );
		//SetCRTC( 13, 0x40 );
__asm
		ld bc, &BC0C
		di
		out (c), c
		inc b
		ld a, &0C
		out (c), a
		;dec b
		;inc c
		;out (c), c
		;inc b
		;ld a, &40
		;out (c), a
		ei
__endasm;
	}
	else
	{
		IsFlipped = 0;
		
		//SetCRTC( 12, 0x2C );
		//SetCRTC( 13, 0x40 );
__asm
		ld bc, &BC0C
		di
		out (c), c
		inc b
		ld a, &2C
		out (c), a
		;dec b
		;inc c
		;out (c), c
		;inc b
		;ld a, &40
		;out (c), a
		ei
__endasm;
	}	
}

// ----------------------------------------------------------------------------
void PushBankTop()
{
	PushBank( BANKC6 );
}

// ----------------------------------------------------------------------------
void PushBankBottom()
{
	if ( !IsFlipped )
	{	
		PushBank( BANKC1 );
	}
	else
	{
		PushBank( BANKC3 );
	}
}

// ----------------------------------------------------------------------------
void BringBackToFrontLines()
{
	if ( !IsFlipped )
	{
		IsFlipped = 1;
	}
	else
	{
		IsFlipped = 0;
	}	
}

// ----------------------------------------------------------------------------
char IsScreenFlipped()
{
	return IsFlipped;
}

// ----------------------------------------------------------------------------
char *GetTopBackVideoPtr()
{
	if ( !IsFlipped )
	{
		return VIDEOSEGMENT0000;
	}

	return VIDEOSEGMENT8000;
}

// ----------------------------------------------------------------------------
char *GetBottomBackVideoPtr()
{
	return VIDEOSEGMENT4000;
}

// ----------------------------------------------------------------------------
char **GetBackLines()
{
	if ( !IsFlipped )
	{
		return GetLowLines();
	}

	return GetHighLines();
}

// ----------------------------------------------------------------------------
char **GetFrontLines( )
{
	if ( !IsFlipped )
	{	
		return GetHighLines();
	}
	
	return GetLowLines();
}

// ----------------------------------------------------------------------------
#endif // _FLIPPING_CXX_
