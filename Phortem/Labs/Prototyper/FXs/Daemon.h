
#ifndef __Daemon_H__
#define __Daemon_H__

#include <SDL.h>
#include "fxbase.h"

class Daemon : public FXBase
{
public:
	Daemon();
	virtual ~Daemon();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *raw;
	unsigned char *highLines;

	void DrawImage();
	void DrawScreenTicks();

	void CreateCircleOutBounds();
	void CreateCircleInBounds();

	int GetLeftOutX(unsigned char *p);
	int GetRightOutX(unsigned char *p);

	int GetLeftInX(unsigned char *p);
	int GetRightInX(unsigned char *p);

	void AddKey(double time, unsigned short x, unsigned short y);
	void MovePreca();
};

#endif // __Daemon_H__