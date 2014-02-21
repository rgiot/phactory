
#ifndef __Angel_H__
#define __Angel_H__

#include <SDL.h>
#include "fxbase.h"

class Angel : public FXBase
{
public:
	Angel();
	virtual ~Angel();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *raw1;
	unsigned char *raw2;
	unsigned char *wobblerRaw;
	void DrawImage();

	void CreateWobblerScanline(int y);
	unsigned char wobblerScanline[192*2];

	float frameSinPos;
};

#endif // __Angel_H__