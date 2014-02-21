
// ----------------------------------------------------------------------------
#include "config.h"
#include "ui_define.h"
#include "ui_font.h"
#include "ui_global.h"
#include "ui_element.h"
#include "hxc_error.h"
#include "hxc_init.h"
#include "hxc_attach.h"
#include "hxc_config.h"
#include "hxc_struct.h"

// ----------------------------------------------------------------------------
void InfiniteLoop()
{
__asm
	jr _InfiniteLoop
__endasm;
}

// ----------------------------------------------------------------------------
void InitScreen()
{
__asm
	ld a, 1
	call &BC0E
	
	call &BD19
	
	ld bc, 0
	call &BC38
	
	xor a
SetBlackPaletteLoop:
	ld bc, 0
	push af
	call &BC32
	pop af
	inc a
	cp 4
	jr nz, SetBlackPaletteLoop
__endasm;
}

// ----------------------------------------------------------------------------
void InitProgram()
{
	struct DirectAccessStatusSector *directAccessStatusSector;
	char errorCode;
	
	InitScreen();
	
	directAccessStatusSector = GetStatusSector();
	
	InitFont();
	InitUI();
	
	PrintEmptyLine();
	Println(" Press H key for help");
	
	PrintEmptyLine();
	
	Println(" Detecting device");
	
	if (!HxC_Init(directAccessStatusSector, &errorCode))
	{
		StopError(errorCode);
	}
	
	Print(" Found firmware version ");
	Println(directAccessStatusSector->FIRMWAREVERSION);
	
	Println(" Attaching device to fatlib");
	if (!HxC_Attach(&errorCode))
	{
		StopError(errorCode);
	}
	
	Print(" Reading config file");
	if (!HxC_ReadConfigFile(&errorCode))
	{
		StopError(errorCode);
	}
	
	SelectSlotInConfig();
}
