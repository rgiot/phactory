
#ifndef __CONFIG_H__
#define __CONFIG_H__

#define SCR_WIDTH 192
#define SCR_HEIGHT 205

#define CUBERATIO 2
#define SCR_BLOCKSRATIO 4
#define SCR_BLOCKSWIDTH 16*CUBERATIO
#define SCR_BLOCKSHEIGHT 41*SCR_BLOCKSRATIO

#define COLOR_INDEX_0 ((unsigned int) 0xFFFF00FF)
#define COLOR_INDEX_1 ((unsigned int) 0xFF00FFFF)

#define PI ((double)3.14159265358979323846f)
#define DEG2RAD(degValue) (degValue*(PI/((double)180.0f)))

#endif // __CONFIG_H__