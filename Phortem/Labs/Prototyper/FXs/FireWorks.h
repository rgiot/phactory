
#ifndef __FireWorks_H__
#define __FireWorks_H__

#include <SDL.h>
#include "fxbase.h"

#include <vector>

struct SDot
{
	bool Ended;
	
	float PosX;
	float PosY;
	float VelocityX;
	float VelocityY;
	float SpeedX;
	float SpeedY;
};

struct SFireWork
{
	int DelayStart;

	bool Exploded; // single dot at start exploded
	bool Ended; // all dots exdended
	
	float PosX;
	float PosY;
	float VelocityX;
	float VelocityY;
	float SpeedX;
	float SpeedY;

	std::vector<SDot> Dots;
};

class FireWorks : public FXBase
{
public:
	FireWorks();
	virtual ~FireWorks();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	void DrawFXDot(float x, float y, unsigned int color);
	bool IsFXDotASquare(float x, float y);
	void UpdateFireWorks();
	SFireWork NewFireWorks();
	SDot NewDot(float startX, float startY);
	bool UpdateFireWorks(SFireWork &fireWork);
	bool UpdateDot(SDot &dot);
	void DrawFireWorks();
	void WriteFireWorks();
};

#endif // __FireWorks_H__