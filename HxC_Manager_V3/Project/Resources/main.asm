
; ----------------------------------------------------------------------------
	include "config.asm"

; ----------------------------------------------------------------------------
main:
	ld hl, all_packed+1
	ld de, StartAddress
	call BitBuster_Unpack

	ld hl, Background+1
	ld de, BACKGROUNDTEMP_BUFFER
	call BitBuster_Unpack
	
	ld hl, TinyFont+1
	ld de, TINY_FONT_BUFFER
	call BitBuster_Unpack
	
	ld hl, RegularFont+1
	ld de, REGULAR_FONT_BUFFER
	call BitBuster_Unpack
	
	jp StartAddress
	
; ----------------------------------------------------------------------------
	include "bitbuster.asm"
	
; ----------------------------------------------------------------------------
TinyFont:
	incbin "tinyFont.bmp.font.pck"
RegularFont:
	incbin "regularFont.bmp.font.pck"
Background:
	incbin "background.bmp.sprRawData1.pck"
all_packed:
	incbin "all.bin.pck"
