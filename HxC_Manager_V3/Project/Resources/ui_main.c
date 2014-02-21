
// ----------------------------------------------------------------------------
#include <string.h>

// ----------------------------------------------------------------------------
#include "config.h"
#include "firmware_define.h"
#include "ui_define.h"
#include "ui_slot.h"
#include "ui_help.h"
#include "ui_init.h"
#include "ui_browse.h"
#include "ui_navigate.h"
#include "ui_select.h"
#include "ui_quit.h"
#include "ui_buzzer.h"
#include "bios.h"

// ----------------------------------------------------------------------------
void main()
{	
	unsigned char key;
	char isQuit;
	
__asm
	call KL_CURR_SELECTION	
	ld ( from_rom_select_address + 1 ), a
	call KL_U_ROM_DISABLE
	ld ( from_rom_previous_state + 1 ), a

	call SCR_GET_MODE
	push af
	ld a, 3
	call SCR_GET_INK
	push bc
	ld a, 2
	call SCR_GET_INK
	push bc
	ld a, 1
	call SCR_GET_INK
	push bc
	xor a
	call SCR_GET_INK
	push bc	
	call SCR_GET_BORDER
	push bc
	call KM_GET_DELAY
	push hl
	ld h, &12 ; first repeat, default is &1E
	ld l, &02 ; repeat speed, default is &02
	call KM_SET_DELAY
__endasm;
	
	InitProgram();
	
	strcpy((char*)BROWSE_CURRENTPATH, "/");
	InitNavigate();
	BrowsePath();
	
	isQuit = 0;
	
	while(isQuit==0)
	{
		key = WaitKey();
		
		if (key==0xF0) // Up
		{
			Up();
		}
		else if (key==0xF1) // Down
		{
			Down();
		}
		else if (key==0xF2) // Left
		{		
			Left();
		}
		else if (key==0xF3) // Right
		{
			Right();
		}
		else if (key==0xFA) // Ctrl+Left
		{
			PrevSlot();
		}		
		else if (key==0xFB) // Ctrl+Right
		{
			NextSlot();
		}
		else if ((key==0x20)||(key==0x8B)||(key==0x0D)) // Space, Enter, Return
		{
			Select();
		}
		else if( (key=='h')||(key=='h'))
		{
			ShowHelp();
			BrowsePage();	
		}
		else if( (key=='s')||(key=='S'))
		{
			ShowSlots();
			BrowsePage();	
		}
		else if( (key=='b')||(key=='B'))
		{
			ShowBuzzerSettings();
			BrowsePage();	
		}
		else if(key==0xFC) // ESC
		{
			isQuit = ShowQuit();
		}
	}
	
__asm
from_rom_select_address:
	ld c, 0
	call KL_ROM_SELECT
from_rom_previous_state:
	ld a, 0
	call KL_ROM_RESTORE

	pop hl
	call KM_SET_DELAY
	pop bc
	call SCR_SET_BORDER
	pop bc
	xor a
	call SCR_SET_INK
	pop bc
	ld a, 1
	call SCR_SET_INK
	pop bc
	ld a, 2
	call SCR_SET_INK
	pop bc
	ld a, 3
	call SCR_SET_INK
	pop af
	call SCR_SET_MODE
__endasm;
}
