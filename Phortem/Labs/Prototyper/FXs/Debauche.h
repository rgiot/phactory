
#ifndef __DEBAUCHE_H__
#define __DEBAUCHE_H__

#include <SDL.h>
#include "fxbase.h"

class Debauche : public FXBase
{
public:
	Debauche();
	virtual ~Debauche();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:	
	unsigned char *raw;
	unsigned char *highLines;

	void DrawImage();	
	void CreateZones();
	void WriteResultBitmap();
};

#endif // __DEBAUCHE_H__