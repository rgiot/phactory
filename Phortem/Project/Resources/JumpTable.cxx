
// ----------------------------------------------------------------------------
#ifndef _JUMPTABLE_CXX_
#define _JUMPTABLE_CXX_

// IF YOU ADD NEW ENTRY, ALSO UPDATE MAINSTART.ASM !!

// ----------------------------------------------------------------------------
extern void PushBank( unsigned char bank );
extern void PopBank();
extern unsigned char GetBank();
extern char **GetHighLines();
extern char **GetLowLines();
extern void FlipScreen();
extern void PushBankTop();
extern void PushBankBottom();
extern void BringBackToFrontLines();
extern char IsScreenFlipped();
extern char *GetTopBackVideoPtr();
extern char *GetBottomBackVideoPtr();
extern char **GetBackLines();
extern char **GetFrontLines();
extern void Unpack( char *source, char *destination );
extern void SetColor( char pen, char color );
extern void SetAsicColor( char *asicPalette, char pen, char indexColorInPalette );
extern void SetBlackPalette();
extern void SetPalette( char *palette );
extern void SetPaletteFrom8( char *palette );
extern void WaitVBL();
extern void LoadFile( unsigned char fileIdentifier, char *destination );

// ----------------------------------------------------------------------------
void JumpTable()
{
__asm

JumpTablePtr 				equ &EF23
PushBankPtr					equ JumpTablePtr
PopBankPtr					equ PushBankPtr + 3
GetBankPtr					equ PopBankPtr + 3
GetHighLinesPtr				equ GetBankPtr + 3
GetLowLinesPtr				equ GetHighLinesPtr + 3
FlipScreenPtr				equ GetLowLinesPtr + 3
PushBankTopPtr				equ FlipScreenPtr + 3
PushBankBottomPtr			equ PushBankTopPtr + 3
BringBackToFrontLinesPtr	equ PushBankBottomPtr + 3
IsScreenFlippedPtr			equ BringBackToFrontLinesPtr + 3
GetTopBackVideoPtr			equ IsScreenFlippedPtr + 3
GetBottomBackVideoPtr		equ GetTopBackVideoPtr + 3
GetBackLinesPtr				equ GetBottomBackVideoPtr + 3
GetFrontLinesPtr			equ GetBackLinesPtr + 3
WaitVBLPtr					equ GetFrontLinesPtr + 3
UnpackPtr					equ WaitVBLPtr + 3
SetColorPtr					equ UnpackPtr + 3
SetAsicColorPtr 			equ SetColorPtr + 3
SetBlackPalettePtr			equ SetAsicColorPtr + 3
SetPalettePtr				equ SetBlackPalettePtr + 3
SetPaletteFrom8Ptr			equ SetPalettePtr + 3
LoadFilePtr					equ SetPaletteFrom8Ptr + 3

_PushBank:					jp PushBankPtr
_PopBank:					jp PopBankPtr
_GetHighLines:				jp GetHighLinesPtr
_GetLowLines:				jp GetLowLinesPtr
_FlipScreen:				jp FlipScreenPtr
_PushBankTop:				jp PushBankTopPtr
_PushBankBottom:			jp PushBankBottomPtr
_BringBackToFrontLines:		jp BringBackToFrontLinesPtr
_IsScreenFlipped:			jp IsScreenFlippedPtr
_GetTopBackVideoPtr:		jp GetTopBackVideoPtr
_GetBottomBackVideoPtr:		jp GetBottomBackVideoPtr
_GetBackLines:				jp GetBackLinesPtr
_GetFrontLines:				jp GetFrontLinesPtr
_WaitVBL:					jp WaitVBLPtr
_Unpack:					jp UnpackPtr
_SetColor:					jp SetColorPtr
_SetAsicColor:				jp SetAsicColorPtr
_SetBlackPalette:			jp SetBlackPalettePtr
_SetPalette:				jp SetPalettePtr
_SetPaletteFrom8:			jp SetPaletteFrom8Ptr
_LoadFile:					jp LoadFilePtr

__endasm;
}

// ----------------------------------------------------------------------------
#endif // _JUMPTABLE_CXX_
