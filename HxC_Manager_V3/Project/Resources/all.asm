
; ----------------------------------------------------------------------------
	include "config.asm"
	
; ----------------------------------------------------------------------------
	org StartAddress
	
; ----------------------------------------------------------------------------
EntryPoint:
	jp _main
	
; ----------------------------------------------------------------------------
	include "bios.asm"
	include "firmware.asm"
	
	include "fat_io_lib.asm"
	include "hxc_error.asm"
	include "hxc_io.asm"
	include "hxc_config.asm"
	include "hxc_attach.asm"
	include "hxc_init.asm"
	
	include "ui_font.asm"
	include "ui_element.asm"
	include "ui_slot.asm"
	include "ui_help.asm"
	include "ui_init.asm"
	include "ui_pathSlotBar.asm"
	include "ui_global.asm"
	include "ui_browse.asm"	
	include "ui_navigate.asm"
	include "ui_select.asm"
	include "ui_main.asm"
	include "ui_quit.asm"
	include "ui_buzzer.asm"
	include "ui_dirFlush.asm"
	
	include "_memcpy.asm"
	include "_memset.asm"
	include "_mullong.asm"
	include "_divulong.asm"
	include "_strcmp.asm"
	include "_uitoa.asm"
	include "_strlen.asm"
	include "_strcpy.asm"
	