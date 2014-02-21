
#ifndef __InfiniteZoom_H__
#define __InfiniteZoom_H__

#include <SDL.h>
#include "fxbase.h"

#include <vector>

struct CPCPixel
{
	unsigned char imageType;		
	unsigned char u;
	unsigned char v;
};

class InfiniteZoom : public FXBase
{
public:
	InfiniteZoom();
	virtual ~InfiniteZoom();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	void DrawBGImage();
	void DrawMaskImage(unsigned char *raw, unsigned char imageType, double ratio);

	unsigned char *bgRaw;

	unsigned char *raw1;
	unsigned char *raw2;
	unsigned char *raw3;
	unsigned char *raw4;

	CPCPixel *CpcPixels;

	void DrawImage(unsigned char *raw);
	void DrawZoomImage(unsigned char *raw, unsigned char imageType, double ratio); // from 0 to 2 (0% to 200%)

	unsigned int GetImagePixel( unsigned char *raw, int x, int y );

	void PutCpcPixelFrame( unsigned char imageType, int x, int y, int u, int v );
	void WriteCPCPixelFrame( unsigned char *filename, unsigned char *filenameLineEdges );
	unsigned int frameCount;

	bool canWriteFrame;
};

#endif // __InfiniteZoom_H__