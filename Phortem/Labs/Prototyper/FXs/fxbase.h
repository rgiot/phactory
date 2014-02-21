
#ifndef __FXBASE_H__
#define __FXBASE_H__

#include <SDL.h>

class FXBase
{
public:
	FXBase();
	virtual ~FXBase();

	virtual SDLKey GetKey() = 0;

	virtual const char *GetName() = 0;

	virtual void Init() = 0;
	virtual void Draw() = 0;
	virtual void Destroy() = 0;

	virtual void SendQuitEvent();
};

#endif // __FXBASE_H__