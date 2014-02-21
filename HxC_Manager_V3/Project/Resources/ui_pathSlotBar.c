
// ----------------------------------------------------------------------------
#include <string.h>

// ----------------------------------------------------------------------------
#include "config.h"
#include "crt.h"
#include "ui_define.h"
#include "ui_font.h"
#include "ui_global.h"
#include "hxc_struct.h"
#include "hxc_define.h"

// ----------------------------------------------------------------------------
void RefreshPathSlot()
{
	char slotLabel[24];
	char *path;
	unsigned char len;
	unsigned char offset;
	unsigned char i;
	unsigned char maxPathSize;
	struct DiskInDrive *slots;
	struct DiskInDrive *diskInDrive;
	char selectedSlot;
	char *pathSlotBuffer;
	unsigned long dirEntSize;
	
__asm
	ld hl, PATHSLOT_BUFFER+7 ; +7=SDCard: label
	ld (hl), 32 ; space
	ld de, PATHSLOT_BUFFER+1+7
	ld bc, 80-1-7
	ldir
	ld (hl), 0
__endasm;

	pathSlotBuffer = (char*)PATHSLOT_BUFFER; 

	selectedSlot = GetSelectedSlot();
	
	// SLOT
	offset = 0;
	if (selectedSlot<10)
	{
		slotLabel[0] = '0';
		offset = 1;
	}	
	uitoa( selectedSlot, &slotLabel[offset], 10);	
	slotLabel[2] = ':';
	
	slots = (struct DiskInDrive*) HXC_SLOTS;
	diskInDrive = &slots[ selectedSlot ];
	
	dirEntSize = diskInDrive->DirEnt.size;
	if (dirEntSize)
	{
		if (dirEntSize>15)
		{
			dirEntSize = 16;
		}
		z80_memcpy(&slotLabel[3], diskInDrive->DirEnt.longName, dirEntSize );
		slotLabel[3+dirEntSize]=0;
	}
	else
	{
		slotLabel[3]=0;
	}
	
	len = z80_strlen( slotLabel );
	maxPathSize = 80-len-1;
	z80_memcpy( &pathSlotBuffer[maxPathSize+1], slotLabel, len );
	
	// PATH
	
	path = (char*)BROWSE_CURRENTPATH;
	
	z80_strcpy(pathSlotBuffer, "SDCard:");
		
	maxPathSize -= 7; // sizeof("SDCard:")
	pathSlotBuffer += 7;

	len = (unsigned char) strlen(path);
	
	if ( len >= maxPathSize)
	{
		z80_strcpy(pathSlotBuffer, &path[len-maxPathSize]);
		pathSlotBuffer[maxPathSize] = ' ';
	}
	else
	{
		z80_strcpy(pathSlotBuffer, path);
		pathSlotBuffer[len] = ' ';
	}
	
	for( i=0; i<len; i++ )
	{
		if (pathSlotBuffer[i] == '/')
		{
			pathSlotBuffer[i] = '\\';
		}
	}
	
	if (len>1)
	{
		pathSlotBuffer[len-1] = ' ';
	}
	
	pathSlotBuffer -= 7;
	pathSlotBuffer[80]=0;
	
	PrintTiny(TINY_TOP_SCREENPTR, pathSlotBuffer);
}
