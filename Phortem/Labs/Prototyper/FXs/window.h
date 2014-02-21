
#ifndef __WINDOW_H__
#define __WINDOW_H__

struct SDL_Surface;

SDL_Surface *GetSDLSurface();
unsigned int *GetBuffer();

void CreateWindow(bool useGL, bool useBlocks);
void StartFrame();
void EndFrame();

unsigned int GetTicks();

#endif // __WINDOW_H__