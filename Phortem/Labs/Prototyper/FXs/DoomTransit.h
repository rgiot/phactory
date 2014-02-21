
#ifndef __DoomTransit_H__
#define __DoomTransit_H__

#include <SDL.h>
#include "fxbase.h"

class DoomTransit : public FXBase
{
public:
	DoomTransit();
	virtual ~DoomTransit();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
};

#endif // __DoomTransit_H__