
	org &EF20

; ----------------------------------------------------------------------------
	jp _main
	
; ----------------------------------------------------------------------------
	jp _PushBank 
	jp _PopBank
	jp _GetBank
	jp _GetHighLines
	jp _GetLowLines
	jp _FlipScreen
	jp _PushBankTop
	jp _PushBankBottom
	jp _BringBackToFrontLines	
	jp _IsScreenFlipped
	jp _GetTopBackVideoPtr
	jp _GetBottomBackVideoPtr
	jp _GetBackLines
	jp _GetFrontLines
	jp _WaitVBL
	jp _Unpack	
	jp _SetColor
	jp _SetAsicColor
	jp _SetBlackPalette
	jp _SetPalette
	jp _SetPaletteFrom8	
	jp _LoadFile
	
LoadFileDirect:
IncFilePtr:
	ld bc, FileOrderPtr+1
	ld ( bc ), a
	inc bc
	ld ( IncFilePtr + 1 ), bc	
	jp LoadFile
	
; ----------------------------------------------------------------------------
	include "Main.asm"
	