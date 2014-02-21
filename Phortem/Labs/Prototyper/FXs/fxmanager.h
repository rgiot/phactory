
#ifndef __FXMANAGER_H__
#define __FXMANAGER_H__

#include <SDL.h>
#include <vector>
#include "fxbase.h"

class FXManager
{
private:
	std::vector< FXBase * > m_fxs;
	FXBase *m_current;

public:
	FXManager();
	~FXManager();

	void RegisterFX( FXBase *fx );

	void KeyPress( SDLKey key );

	void InitFX();
	void DrawFX();
	void DestroyFX();

	void Release();
};

FXManager *GetFXManager();

#endif // __FXMANAGER_H__