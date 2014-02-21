
#ifndef __DEFAULT_H__
#define __DEFAULT_H__

#include <SDL.h>
#include "fxbase.h"

class Default : public FXBase
{
public:
	Default();
	virtual ~Default();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();
};

#endif // __DEFAULT_H__