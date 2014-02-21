
#ifndef __Rubber_H__
#define __Rubber_H__

#include <SDL.h>
#include "fxbase.h"

class Rubber : public FXBase
{
public:
	Rubber();
	virtual ~Rubber();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *rawImage;
	unsigned char *highLines;

	void DrawImage();
	void CreateRubberEdges(bool isPixelRight, char *filenameLeftPixelPos, char *filenameRelativeJump, char *filenameDoRightPixel);

	int GetLeftOutX(unsigned char *p, bool isRight);
	int GetRightOutX(unsigned char *p, bool isRight);

	void CreateRubberScanlines(unsigned short *outPtr, unsigned char *rubberPtr, unsigned char scanlineWidth);

	void CreateRubberPreca(bool isPixelRight);
};

#endif // __Rubber_H__