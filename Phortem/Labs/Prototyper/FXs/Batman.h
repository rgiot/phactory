
#ifndef __Batman_H__
#define __Batman_H__

#include <SDL.h>
#include "fxbase.h"

class Batman : public FXBase
{
public:
	Batman();
	virtual ~Batman();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *rawImage;
	unsigned char *highLines;

	void DrawImage();
	void DrawImageFirstFrame();
	void ComputeAndWriteCircle(FILE *file, int offsetX, int clipXLeft, int clipXRight);
	
	int GetLeftOutX(unsigned char *p);
	int GetRightOutX(unsigned char *p);
};

#endif // __Batman_H__