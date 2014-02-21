
// ----------------------------------------------------------------------------
#ifndef _CONFIG_H_
#define _CONFIG_H_

// ----------------------------------------------------------------------------
#define CODESEGMENT0000			0x2800
#define CODESEGMENT0000_SIZE	0x1800
#define CODESEGMENT4000			0x6800
#define CODESEGMENT4000_SIZE	0x1800
#define CODESEGMENT8000			0xA800
#define CODESEGMENT8000_SIZE	0x1800
#define CODESEGMENTC000			0xE800
#define CODESEGMENTC000_SIZE	0x1800

#define VIDEOSEGMENT0000		0x0080
#define VIDEOSEGMENT0000_SIZE	0x2780
#define VIDEOSEGMENT4000		0x4000
#define VIDEOSEGMENT4000_SIZE	0x2800
#define VIDEOSEGMENT8000		0x8080
#define VIDEOSEGMENT8000_SIZE	0x2780
#define VIDEOSEGMENTC000		0xC000
#define VIDEOSEGMENTC000_SIZE	0x2800

// ----------------------------------------------------------------------------
#define LINES_START					0xA800
#define DUMMYLINEPTR				LINES_START
#define BANK0LINES_TOPCLIPPTR		DUMMYLINEPTR + 0x60
#define BANK0LINES_TOPPTR			BANK0LINES_TOPCLIPPTR + 0x100
#define BANK1LINES_BOTTOMPTR		BANK0LINES_TOPPTR + 100+100
#define BANK1LINES_BOTTOMCLIPPTR	BANK1LINES_BOTTOMPTR + 105+105
#define BANK2LINES_TOPCLIPPTR		BANK1LINES_BOTTOMCLIPPTR + 0x100
#define BANK2LINES_TOPPTR			BANK2LINES_TOPCLIPPTR + 0x100
#define BANK3LINES_BOTTOMPTR		BANK2LINES_TOPPTR + 100+100
#define BANK3LINES_BOTTOMCLIPPTR	BANK3LINES_BOTTOMPTR + 105+105
#define BANK2LINES2_TOPPTR			BANK3LINES_BOTTOMCLIPPTR + 0x100
#define BANK3LINES2_BOTTOMPTR		BANK2LINES2_TOPPTR + 100+100
#define LINES_END					BANK3LINES2_BOTTOMPTR + 105+105

// ----------------------------------------------------------------------------
#define BANKC0	0xC0
#define BANKC1	0xC1
#define BANKC2	0xC2
#define BANKC3	0xC3
#define BANKC4	0xC4
#define BANKC5	0xC5
#define BANKC6	0xC6
#define BANKC7	0xC7

// ----------------------------------------------------------------------------
#define CRTC_REG1		48
#define CRTC_REG2		50
#define CRTC_REG6		41
#define CRTC_REG7		49
#define CRTC_REG9		4
#define CRTC_REG9INC	5
#define CRTC_REG4		62

// ----------------------------------------------------------------------------
#define FRAMEWORK_JUMPTABLE		0x8000
#define STACKINTERRUPTPOINTER	0x8080

// ----------------------------------------------------------------------------
#define GRID_TOP_CHARHEIGHT 	20
#define GRID_BOTTOM_CHARHEIGHT 	21
#define GRID_TOPSCREEN1PTR 		( VIDEOSEGMENT0000 + ( GRID_TOP_CHARHEIGHT * SCREEN_WIDTH_BYTES ) )
#define GRID_BOTTOMSCREEN1PTR 	( VIDEOSEGMENT4000 + ( GRID_BOTTOM_CHARHEIGHT * SCREEN_WIDTH_BYTES ) )
#define GRID_TOPSCREEN2PTR 		( VIDEOSEGMENT8000 + ( GRID_TOP_CHARHEIGHT * SCREEN_WIDTH_BYTES ) )
#define GRID_BOTTOMSCREEN2PTR 	( VIDEOSEGMENTC000 + ( GRID_BOTTOM_CHARHEIGHT * SCREEN_WIDTH_BYTES ) )

// ----------------------------------------------------------------------------
#define COLOR0	0x54
#define COLOR1	0x44
#define COLOR2	0x55
#define COLOR3	0x5C
#define COLOR4	0x58
#define COLOR5	0x5D
#define COLOR6	0x4C
#define COLOR7	0x45
#define COLOR8	0x4D
#define COLOR9	0x56	
#define COLOR10	0x46
#define COLOR11	0x57
#define COLOR12	0x5E
#define COLOR13	0x40
#define COLOR14	0x5F
#define COLOR15	0x4E
#define COLOR16	0x47
#define COLOR17	0x4F
#define COLOR18	0x52
#define COLOR19	0x42
#define COLOR20	0x53
#define COLOR21	0x5A
#define COLOR22	0x59
#define COLOR23	0x5B
#define COLOR24	0x4A
#define COLOR25	0x43
#define COLOR26	0x4B

// ----------------------------------------------------------------------------
#define MODE0	0x8C
#define MODE1	0x8D
#define MODE2	0x8E

// ----------------------------------------------------------------------------
#endif // _CONFIG_H_
