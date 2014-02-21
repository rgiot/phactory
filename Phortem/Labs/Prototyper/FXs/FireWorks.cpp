
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "FireWorks.h"

FireWorks::FireWorks() :
	FXBase()
{	
}

FireWorks::~FireWorks()
{
}

SDLKey FireWorks::GetKey()
{
	return SDLK_2;
}

const char *FireWorks::GetName()
{
	return "FireWorks";
}

#define MAX_FIREWORKS 2
#define MAX_DOTS 13
#define NextRandomValue ((float)rand()/(float)RAND_MAX)
#define BLOCK_WIDTH 2
#define BLOCK_HEIGHT 4
#define SEED 45592
#define FIREWORKS_VELY -3.2f
#define DELAY_START 70
static std::vector<SFireWork> s_fireWorks;

FILE *frameLeft = 0;
FILE *frameRight = 0;
FILE *frameFull = 0;
FILE *frameBlockLeft = 0;
FILE *frameBlockRight = 0;

static int frameBlockLeftSize = 0;

void FireWorks::Init()
{
	srand(SEED);

	frameBlockLeft = fopen("FireWorks_FrameBlockLeft1.raw", "wb" );
	frameBlockRight = fopen("FireWorks_FrameBlockRight.raw", "wb" );
	frameLeft = fopen("FireWorks_FrameLeft.raw", "wb" );
	frameRight = fopen("FireWorks_FrameRight.raw", "wb" );
	frameFull = fopen("FireWorks_FrameFull.raw", "wb" );
}

void FireWorks::Draw()
{
	drawRect( 0, 0, SCR_WIDTH, SCR_HEIGHT, PIXEL_BLACK );

	UpdateFireWorks();
	DrawFireWorks();
	WriteFireWorks();
}

void FireWorks::Destroy()
{
	if ( frameBlockLeft )
	{
		fclose(frameBlockLeft);
	}
	if ( frameBlockRight )
	{
		fclose(frameBlockRight);
	}
	if ( frameLeft )
	{
		fclose(frameLeft);
	}
	if ( frameRight )
	{
		fclose(frameRight);
	}
	if ( frameFull )
	{
		fclose(frameFull);
	}
}

static unsigned short GetScreenPtr( unsigned char y )
{
	unsigned char yBlock = y/5;

	unsigned short ptr = 0;

	if ( yBlock < 100/5 )
	{
		ptr = 0x8080 + 96 * ((unsigned short)(yBlock));
	}
	else
	{
		ptr = 0x4000 + 96 * ((unsigned short)(yBlock-(100/5)));
	}

	unsigned char lineInBlock = y - (yBlock*5);
	unsigned short offset = 0x800 * lineInBlock;

	ptr += offset;

	return ptr;
}

void FireWorks::DrawFXDot(float x, float y, unsigned int color)
{
	int startX = (int)x;
	int startY = (int)y;

	for ( int iY = 0; iY < BLOCK_HEIGHT; iY++ )
	{
		for ( int iX = 0; iX < BLOCK_WIDTH; iX++ )
		{
			putPixel(startX+iX, startY+iY, color);
		}
	}
}

bool FireWorks::IsFXDotASquare(float x, float y)
{
	int startX = (int)x;
	int startY = (int)y;

	int pixelCount = 0;
	
	for ( int iY = 0; iY < BLOCK_HEIGHT; iY++ )
	{
		for ( int iX = 0; iX < BLOCK_WIDTH; iX++ )
		{
			if ( getPixel(startX+iX, startY+iY) == PIXEL_WHITE) 
			{
				pixelCount++;
			}
		}
	}

	if ( pixelCount == BLOCK_WIDTH*BLOCK_HEIGHT )
	{
		return true;
	}
	return false;
}

SFireWork FireWorks::NewFireWorks()
{
	SFireWork fireWork;

	fireWork.DelayStart = 1 + s_fireWorks.size()*DELAY_START;

	fireWork.Exploded = false;
	fireWork.Ended = false;

	fireWork.PosX = (SCR_WIDTH/2) + (NextRandomValue-0.5f)*(SCR_WIDTH/3);
	fireWork.PosY = SCR_HEIGHT;

	fireWork.VelocityX = (NextRandomValue-0.5f)*2.0f;
	fireWork.VelocityY = FIREWORKS_VELY + NextRandomValue;

	fireWork.SpeedX = 1.0f;
	fireWork.SpeedY = 0.0f;

	return fireWork;
}

bool FireWorks::UpdateFireWorks(SFireWork &fireWork)
{
	fireWork.PosX += fireWork.VelocityX;
	fireWork.PosY += fireWork.VelocityY;

	fireWork.VelocityX *= fireWork.SpeedX;
	fireWork.VelocityY -= fireWork.SpeedY;

	if ( fireWork.VelocityY > 2.8)
	{
		return true;
	}

	fireWork.SpeedX -= 0.0001f;
	if ( fireWork.SpeedX < 0 )
	{
		fireWork.SpeedX = 0;
	}
	fireWork.SpeedY -= 0.001f;

	return false;
}

SDot FireWorks::NewDot(float startX, float startY)
{
	SDot dot;

	dot.Ended = false;

	dot.PosX = startX;
	dot.PosY = startY;

	dot.VelocityX = (NextRandomValue-0.5f)*1.3f;
	dot.VelocityY = -2.0f + NextRandomValue*1.9f;

	dot.SpeedX = 1.0f;
	dot.SpeedY = 0.0f;

	return dot;
}

bool FireWorks::UpdateDot(SDot &dot)
{
	dot.PosX += dot.VelocityX;
	dot.PosY += dot.VelocityY;

	dot.VelocityX *= dot.SpeedX;
	dot.VelocityY -= dot.SpeedY;

	if ( dot.VelocityY > 9)
	{
		return true;
	}

	dot.SpeedX -= 0.0001f;
	if ( dot.SpeedX < 0 )
	{
		dot.SpeedX = 0;
	}
	dot.SpeedY -= 0.001f;

	return false;
}

void FireWorks::UpdateFireWorks()
{
	while ( s_fireWorks.size() != MAX_FIREWORKS )
	{
		SFireWork firework = NewFireWorks();
		s_fireWorks.push_back(firework);
	}

	int allEndedCount = 0;
	for( int i = 0; i < MAX_FIREWORKS; i++)
	{
		SFireWork &firework = s_fireWorks[i];

		if ( firework.Ended )
		{
			allEndedCount++;
			/*s_fireWorks[i] = NewFireWorks();
			firework = s_fireWorks[i];*/
		}
		else
		{
			firework.DelayStart--;
			if ( firework.DelayStart == -1 )
			{
				firework.DelayStart = 0;
			}

			if ( firework.DelayStart == 0 )
			{
				if ( !firework.Exploded )
				{
					if ( UpdateFireWorks(firework) )
					{
						firework.Exploded = true;

						for ( int iDot = 0; iDot < MAX_DOTS; iDot++ )
						{
							SDot dot = NewDot(firework.PosX, firework.PosY);
							firework.Dots.push_back(dot);
						}
					}
				}
				else
				{
					int endedCount = 0;
					for ( int iDot = 0; iDot < firework.Dots.size(); iDot++ )
					{
						SDot &dot = firework.Dots[iDot];
						if ( !dot.Ended )
						{
							if ( UpdateDot(dot) )
							{
								dot.Ended = true;
							}
						}
						else
						{
							endedCount++;
						}
					}

					if ( endedCount == MAX_DOTS )
					{
						firework.Ended = true;
					}
				}
			}
		}
	}

	if ( allEndedCount == MAX_FIREWORKS )
	{
		unsigned short endFrame = 0x0001;
		
		if ( frameBlockLeft )
		{
			fwrite(&endFrame, sizeof(unsigned short), 1, frameBlockLeft );
			frameBlockLeftSize+=2;
			fclose( frameBlockLeft );
			frameBlockLeft = 0;
		}

		if ( frameBlockRight )
		{
			fwrite(&endFrame, sizeof(unsigned short), 1, frameBlockRight );
			fclose( frameBlockRight );
			frameBlockRight = 0;
		}

		if ( frameLeft )
		{
			fwrite(&endFrame, sizeof(unsigned short), 1, frameLeft );
			fclose( frameLeft );
			frameLeft = 0;
		}

		if ( frameRight )
		{
			fwrite(&endFrame, sizeof(unsigned short), 1, frameRight );
			fclose( frameRight );
			frameRight = 0;
		}

		if ( frameFull )
		{
			fwrite(&endFrame, sizeof(unsigned short), 1, frameFull );
			fclose( frameFull );
			frameFull = 0;
		}
	}
}

void FireWorks::DrawFireWorks()
{
	for( int i = 0; i < MAX_FIREWORKS; i++)
	{
		SFireWork &firework = s_fireWorks[i];

		if ( !firework.Exploded )
		{
			DrawFXDot( firework.PosX,  firework.PosY, PIXEL_WHITE );
		}
		else
		{
			for ( int iDot = 0; iDot < firework.Dots.size(); iDot++ )
			{
				SDot &dot = firework.Dots[iDot];
				if ( !dot.Ended )
				{
					DrawFXDot( dot.PosX,  dot.PosY, PIXEL_WHITE );
				}
			}
		}
	}
}

void FireWorks::WriteFireWorks()
{
	int allEndedCount = 0;
	for( int i = 0; i < MAX_FIREWORKS; i++)
	{
		SFireWork &firework = s_fireWorks[i];
		if ( firework.Ended )
		{
			allEndedCount++;
		}
	}
	if ( allEndedCount == MAX_FIREWORKS )
	{
		return;
	}

	// 1ST PASS : SQUARES
	for ( int y = 0; y < SCR_HEIGHT-BLOCK_HEIGHT; y++ )
	{
		for ( int x = 0; x < SCR_WIDTH-BLOCK_WIDTH; x++ )
		{
			if ( IsFXDotASquare(x, y) )
			{
				DrawFXDot(x, y, PIXEL_BLACK);

				unsigned short ptr = (unsigned short(y)<<8) + (x/2);
				
				if ((x&1)==0)
				{
					fwrite(&ptr, sizeof(unsigned short), 1, frameBlockLeft );
					frameBlockLeftSize += 2;
				}
				else
				{
					fwrite(&ptr, sizeof(unsigned short), 1, frameBlockRight );
				}
			}
		}
	}

	// 2ND PASS : ISOLATED PIXELS
	for ( int y = 0; y < SCR_HEIGHT; y++ )
	{
		for ( int x = 0; x < SCR_WIDTH; x++ )
		{
			if ( getPixel(x, y) != PIXEL_BLACK )
			{
				unsigned short ptr = GetScreenPtr(y) + (x/2);
				
				if ( ((x&1)==0) && ( getPixel(x+1, y) != PIXEL_BLACK ) )
				{
					fwrite(&ptr, sizeof(unsigned short), 1, frameFull );
					x++;
				}
				else
				{
					if ((x&1)==0)
					{
						fwrite(&ptr, sizeof(unsigned short), 1, frameLeft );
					}
					else
					{
						fwrite(&ptr, sizeof(unsigned short), 1, frameRight );
					}
				}
			}
		}
	}

	unsigned short endFrame = 0;
	fwrite(&endFrame, sizeof(unsigned short), 1, frameBlockLeft );
	frameBlockLeftSize += 2;
	fwrite(&endFrame, sizeof(unsigned short), 1, frameBlockRight );
	fwrite(&endFrame, sizeof(unsigned short), 1, frameLeft );
	fwrite(&endFrame, sizeof(unsigned short), 1, frameRight );
	fwrite(&endFrame, sizeof(unsigned short), 1, frameFull );

	if ( frameBlockLeftSize >= 0x1700 )
	{
		fclose( frameBlockLeft );
		frameBlockLeft = fopen("FireWorks_FrameBlockLeft2.raw", "wb" );
		frameBlockLeftSize = 0;
	}
}