
#ifndef __TransitCloudLines_H__
#define __TransitCloudLines_H__

#include <SDL.h>
#include "fxbase.h"

#include <vector>

class TransitCloudLines : public FXBase
{
public:
	TransitCloudLines();
	virtual ~TransitCloudLines();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *raw1;
	unsigned char *raw2;
};

#endif // __TransitCloudLines_H__