
// ----------------------------------------------------------------------------
#include <string.h>

// ----------------------------------------------------------------------------
#include "config.h"
#include "bios.h"
#include "crt.h"
#include "hxc_struct.h"
#include "hxc_define.h"
#include "fat_filelib.h"
#include "hxc_io.h"
#include "hxc_config.h"

// ----------------------------------------------------------------------------
char HxC_Init(struct DirectAccessStatusSector *statusSector, char *errorCode)
{
	unsigned char retry;
	
	// SETUP DRIVE : NO ERROR MESSAGES, ETC.
	ConfigDiskDrive();
	
	// DETECTING HXC FLOPPY EMULATOR
	
	retry = 0;
	do
	{
#ifdef USE_CPCEMU
		if (!ReadSector(HXC_SECTORDATA, 8, 0xC1)) // CPC DATA FORMAT
#else
		if (!ReadSector(HXC_SECTORDATA, 255, 0)) // HXC FORMAT
#endif
		{
			if ( retry == 8 )
			{		
				*errorCode = HXCERROR_FLOPPYACCESSERROR;
				return OPERATIONRESULT_ERROR;
			}
			retry++;
		}
		else
		{
			retry = 0;
		}
	} while ( retry != 0 );

	z80_memcpy( statusSector, HXC_SECTORDATA, sizeof(struct DirectAccessStatusSector));

#ifdef USE_CPCEMU
	z80_strcpy( statusSector->DAHEADERSIGNATURE, "HxCFEDA");
	z80_strcpy( statusSector->FIRMWAREVERSION, "1.2.3.4");
#endif

	if(strcmp(statusSector->DAHEADERSIGNATURE, "HxCFEDA"))
	{
		*errorCode = HXCERROR_BADSIGNATURE;
		return OPERATIONRESULT_ERROR;
	}
	
	*errorCode = HXCERROR_OK;
	return OPERATIONRESULT_SUCCESS;
}
