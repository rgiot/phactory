
// ----------------------------------------------------------------------------
#include <string.h>

// ----------------------------------------------------------------------------
#include "config.h"
#include "crt.h"
#include "bios.h"
#include "firmware.h"
#include "ui_define.h"
#include "ui_font.h"
#include "ui_element.h"
#include "ui_slot.h"
#include "ui_help.h"
#include "ui_init.h"
#include "hxc_struct.h"
#include "hxc_define.h"

// ----------------------------------------------------------------------------
static struct DirectAccessStatusSector StatusSector;
static char SelectedSlot;
static char LastSetSlot;

// ----------------------------------------------------------------------------
struct DirectAccessStatusSector *GetStatusSector()
{
	return &StatusSector;
}

// ----------------------------------------------------------------------------
char GetSelectedSlot()
{
	return SelectedSlot;
}

// ----------------------------------------------------------------------------
char SetSelectedSlot(char selectedSlot)
{
	SelectedSlot = selectedSlot;
}

// ----------------------------------------------------------------------------
char GetLastSetSlot()
{
	return LastSetSlot;
}

// ----------------------------------------------------------------------------
char SetLastSetSlot(char lastSetSlot)
{
	LastSetSlot = lastSetSlot;
}
