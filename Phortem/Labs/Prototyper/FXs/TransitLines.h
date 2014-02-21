
#ifndef __TransitLines_H__
#define __TransitLines_H__

#include <SDL.h>
#include "fxbase.h"
#include "renderer.h"

class TransitLines : public FXBase
{
public:
	TransitLines();
	virtual ~TransitLines();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	Renderer m_renderer;
	
	FILE *file;
	void TransitLinesFX1();
	void TransitLinesFX2();
	void TransitLinesFX3();
};

#endif // __TransitLines_H__