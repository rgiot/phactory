
#include "fxbase.h"

FXBase::FXBase()
{
}

FXBase::~FXBase()
{
}

void FXBase::SendQuitEvent()
{
	/* Post a SDL_QUIT event */
    SDL_Event event;
    event.type = SDL_QUIT;
    SDL_PushEvent(&event);
}