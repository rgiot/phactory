
#ifndef __INTRO_H__
#define __INTRO_H__

#include <SDL.h>
#include "fxbase.h"

class Intro : public FXBase
{
public:
	Intro();
	virtual ~Intro();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
};

#endif // __INTRO_H__