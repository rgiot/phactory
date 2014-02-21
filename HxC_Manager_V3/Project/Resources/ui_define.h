
// ----------------------------------------------------------------------------
#ifndef _UI_DEFINE_H_
#define _UI_DEFINE_H_

// ----------------------------------------------------------------------------
#define TINY_TOP_SCREENPTR		0xF0A0
#define REGULAR_SCREENPTR		0xC140

// ----------------------------------------------------------------------------
#define SCREEN_MAXSLOTS			21

#define TINY_FONT				0x1A80 // 0x300
#define REGULAR_FONT			0x1D80 // 0x600

// ----------------------------------------------------------------------------
#define PATHSLOT_BUFFER			0x2380
#define BROWSE_CURRENTPATH		0x2400
#define SCREEN_CHARPTR			0x2500
#define TINY_FONTCHARPTR		0x2600
#define REGULAR_FONTCHARPTR		0x2700

// here check hxc_define.h

#define HXC_UI_DIRENTRIES		0x3200 // 0x400 bytes (SCREEN_MAXSLOTS*sizeof(DirectoryEntry))
#define HXC_UI_BROWSE_DIRSTATUS	0x3600

// ----------------------------------------------------------------------------
// only used at init time
#define BACKGROUNDTEMP_BUFFER	0x3000 // also defined in config.asm !

// ----------------------------------------------------------------------------
#endif // _UI_DEFINE_H_