
#ifndef __GL3DScroll_H__
#define __GL3DScroll_H__

#include <SDL.h>
#include "fxbase.h"

class GL3DScroll : public FXBase
{
public:
	GL3DScroll();
	virtual ~GL3DScroll();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:	
};

#endif // __GL3DScroll_H__