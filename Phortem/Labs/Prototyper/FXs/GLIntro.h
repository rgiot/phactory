
#ifndef __GLINTRO_H__
#define __GLINTRO_H__

#include <SDL.h>
#include "fxbase.h"

class GLIntro : public FXBase
{
public:
	GLIntro();
	virtual ~GLIntro();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:	
};

#endif // __GLINTRO_H__