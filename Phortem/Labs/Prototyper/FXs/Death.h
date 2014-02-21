
#ifndef __Death_H__
#define __Death_H__

#include <SDL.h>
#include "fxbase.h"

class Death : public FXBase
{
public:
	Death();
	virtual ~Death();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *raw;
	unsigned char *highLines;

	void DrawImage();
	void DrawScreenTicks();
	void CreateSpriteLines();
	void CreateBands();
	void CreateBand(char *filename, int type);
	void WriteResultBitmap();
};

#endif // __Death_H__