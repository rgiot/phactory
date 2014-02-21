
#ifndef __TransitBouncing_H__
#define __TransitBouncing_H__

#include <SDL.h>
#include "fxbase.h"

#include <vector>

#define TUBE_HEIGHT 56

enum LineType
{
	RAW1,
	RAW2
};
struct LineInfo
{
	LineType Type; // RAW1 or RAW2
	unsigned char TextureY;
	unsigned char ScreenY;
};

class TransitBouncing : public FXBase
{
public:
	TransitBouncing();
	virtual ~TransitBouncing();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *raw1;
	unsigned char *raw2;
	void DrawImage();

	float frameSinPos;

	std::vector<LineInfo> FrameLines;

	LineInfo RawLines[205+TUBE_HEIGHT+TUBE_HEIGHT];
	LineInfo RawLinesPrev1[205+TUBE_HEIGHT+TUBE_HEIGHT];
	LineInfo RawLinesPrev2[205+TUBE_HEIGHT+TUBE_HEIGHT];

	LineInfo GetRaw1Ptr( int y );
	LineInfo GetRaw2Ptr( int y );

	void CreateRawLines1();
	void CreateRawLines2();
	void CreateDiffLines();
	void RenderRawLines();
};

#endif // __TransitBouncing_H__