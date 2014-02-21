
#ifndef __TransitRect_H__
#define __TransitRect_H__

#include <SDL.h>
#include "fxbase.h"

#include <vector>

#define TUBE_HEIGHT 50

class TransitRect : public FXBase
{
public:
	TransitRect();
	virtual ~TransitRect();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *raw1;
	unsigned char *raw2;
	void DrawImage();
};

#endif // __TransitRect_H__