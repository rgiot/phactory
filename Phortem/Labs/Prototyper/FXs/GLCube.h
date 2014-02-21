
#ifndef __GLCube_H__
#define __GLCube_H__

#include <SDL.h>
#include "fxbase.h"

class GLCube : public FXBase
{
public:
	GLCube();
	virtual ~GLCube();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:	
};

#endif // __GLCube_H__