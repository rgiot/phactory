
#ifndef __Costix_H__
#define __Costix_H__

#include <SDL.h>
#include <vector>
#include "fxbase.h"

class Costix : public FXBase
{
public:
	Costix();
	virtual ~Costix();

	virtual SDLKey GetKey();

	virtual const char *GetName();

	virtual void Init();
	virtual void Draw();
	virtual void Destroy();

private:
	unsigned char *raw;
	std::vector<unsigned char *> m_costixTable;
	unsigned char *m_costixMask;

	void AddCostix(char *filename);
	void CalcCostixEdges();
	void CalcCostixFrame(int frame);

	void DrawImage();
	void FilterBlocks();
	void WriteResultBitmap();
};

#endif // __Costix_H__