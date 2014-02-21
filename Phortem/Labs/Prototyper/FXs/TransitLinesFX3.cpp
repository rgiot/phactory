
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "TransitLines.h"

static float _degLeft = -180.0f;
static float _degRight = -180.0f;
static int _frameCount = 0;

static Line _lines[3]; // 0=current 1=prev 2=prevPrev

#include "..\Track.h"

static Track *g_trackX1;
static Track *g_trackY1;
static double g_time1;
static Track *g_trackX2;
static Track *g_trackY2;
static double g_time2;

static Track *g_trackProgress;
static double g_timeProgress;


static void AddKey1(double time, float x, float y)
{
	TrackKey xKey;
	xKey.value = x;
	xKey.time = g_time1;

	TrackKey yKey;
	yKey.value = y;
	yKey.time = g_time1;

	g_trackX1->setKey(xKey);
	g_trackY1->setKey(yKey);

	g_time1 += time;
}

static void AddKey2(double time, float x, float y)
{
	TrackKey xKey;
	xKey.value = x;
	xKey.time = g_time2;

	TrackKey yKey;
	yKey.value = y;
	yKey.time = g_time2;

	g_trackX2->setKey(xKey);
	g_trackY2->setKey(yKey);

	g_time2 += time;
}

static void AddKeyProgress(double time, float x)
{
	TrackKey xKey;
	xKey.value = x;
	xKey.time = g_timeProgress;

	g_trackProgress->setKey(xKey);

	g_timeProgress += time;
}

static Triangle ReverseTri(Triangle tri)
{
	return tri;

	Triangle out;
	out.index = tri.index;
	out.X1 = tri.Y1;
	out.Y1 = tri.X1;
	out.X2 = tri.Y2;
	out.Y2 = tri.X2;
	out.X3 = tri.Y3;
	out.Y3 = tri.X3;
	return out;
}

static float t = 0.0f;
static Triangle prevTri;
static float oldProgress = -1;
static float progress = -1;

void TransitLines::TransitLinesFX3()
{
	if ( file == 0 )
	{
		file = fopen("..\\..\\winape\\src\\transit\\TransitLinesFX3.bin", "wb" );
	}

	static bool firstFrame = true;
	if ( firstFrame )
	{
		prevTri.X1 = -1000;

		firstFrame = false;

		g_trackProgress = Track::createTrack( TRACK_TYPE_SPLINE );
		g_timeProgress = 0.0f;
		
		g_trackX1 = Track::createTrack( TRACK_TYPE_SPLINE );
		g_trackY1 = Track::createTrack( TRACK_TYPE_SPLINE );
		g_time1 = 0.0f;
		g_trackX2 = Track::createTrack( TRACK_TYPE_SPLINE );
		g_trackY2 = Track::createTrack( TRACK_TYPE_SPLINE );
		g_time2 = 0.0f;

		//AddKey1(5.0f, 0, 0); AddKey2(5.0f, 0, SCR_HEIGHT);
		//AddKey1(5.0f, SCR_WIDTH-2, 0); AddKey2(5.0f, SCR_WIDTH-2, SCR_HEIGHT);

		float xc = SCR_WIDTH/2;
		float yc = SCR_HEIGHT/2;
		float w = (SCR_WIDTH*0.6f);
		float h = (SCR_HEIGHT*1.1f);

		const float div = 6;
		
		AddKey1(5.0f, 0, -h);
		
		AddKey1(5.0f, w/2+w/div, -h/2-h/div);
		
		AddKey1(5.0f, w, 0);

		AddKey1(5.0f, w/2+w/div, h/2+h/div);

		AddKey1(5.0f, 0, h);

		AddKey1(5.0f, -w/2-w/div, h/2+h/div);

		AddKey1(5.0f, -w, 0);
		
		AddKey1(5.0f, -1-w/2-w/div, -1-h/2-h/div);
		
		AddKey1(5.0f, 0, -h);
		
		AddKey1(5.0f, w/2+w/div, -h/2-h/div);
		
		g_time1 -= 9.9f;

		AddKeyProgress( 0.4f, 0.0f );
		AddKeyProgress( 0.4f, 0.65f );
		AddKeyProgress( 0.4f, 0.33f );
		AddKeyProgress( 0.2f, 1.0f );
	}

	drawRect(0, 0, SCR_WIDTH, SCR_HEIGHT, 0 );
	const float clipHeight = 0.0f;

	//if (_frameCount > 50 )
	{
		//SendQuitEvent();
		//return;
	}

	m_renderer.DoClipping = false;

	oldProgress = progress;
	progress = g_trackProgress->getValue(t);	
	progress *= g_time1;

	double sinPosX1 = g_trackX1->getValue(progress);
	double sinPosY1 = g_trackY1->getValue(progress);
	double sinPosX2 = -sinPosX1;// g_trackX2->getValue(t);
	double sinPosY2 = -sinPosY1; //g_trackY2->getValue(t);

	sinPosX1 += SCR_WIDTH/2;
	sinPosX2 += SCR_WIDTH/2;
	sinPosY1 += SCR_HEIGHT/2;
	sinPosY2 += SCR_HEIGHT/2;

	t += 0.0050f;
	//if ( t >= (g_timeProgress-0.02f) )
	if ( _frameCount > 239 )
	{
		SendQuitEvent();
		return;
	}

	if ( sinPosY1 < 0 )
	{
		sinPosY1 = 0;
	}
	if ( sinPosY2 < 0 )
	{
		sinPosY2 = 0;
	}
	if ( sinPosY1 >= (SCR_HEIGHT-1) )
	{
		sinPosY1 = SCR_HEIGHT-1;
	}
	if ( sinPosY2 >= (SCR_HEIGHT-1) )
	{
		sinPosY2 = SCR_HEIGHT-1;
	}

	if ( sinPosX1 < 0 )
	{
		sinPosX1 = 0;
	}
	if ( sinPosX2 < 0 )
	{
		sinPosX2 = 0;
	}
	if ( sinPosX1 >= (SCR_WIDTH-2) )
	{
		sinPosX1 = SCR_WIDTH-2;
	}
	if ( sinPosX2 >= (SCR_WIDTH-2) )
	{
		sinPosX2 = SCR_WIDTH-2;
	}

	if ( _frameCount==0)
	{
		_lines[1].X1 = 0;
		_lines[1].Y1 = 0;
		_lines[1].X2 = 0;
		_lines[1].Y2 = SCR_HEIGHT-1;
		_lines[0].X1 = 0;
		_lines[0].Y1 = 0;
		_lines[0].X2 = 0;
		_lines[0].Y2 = SCR_HEIGHT-1;
	}

	_lines[2] = _lines[1];
	_lines[1] = _lines[0];
	_lines[0].X1 = sinPosX1;
	_lines[0].Y1 = sinPosY1;
	_lines[0].X2 = sinPosX2;
	_lines[0].Y2 = sinPosY2;

	if ( _frameCount >= 2 )
	{
		Triangle tri;
		float z;
	
		m_renderer.ClearFrame();

		Point intersectPoint = m_renderer.IntersectLines( _lines[0], _lines[2] );
		
		tri.X1 = _lines[1].X1;
		tri.Y1 = _lines[1].Y1;
		tri.X2 = _lines[0].X1;
		tri.Y2 = _lines[0].Y1;
		tri.X3 = SCR_WIDTH/2;//_lines[2].X2;// intersectPoint.X;
		tri.Y3 = SCR_HEIGHT/2;//_lines[2].Y2;// intersectPoint.Y;

		bool doAdd = true;
		if ( prevTri.X1 != -1000 )
		{
			if ( ( prevTri.X1 == tri.X1 )
				&& ( prevTri.X2 == tri.X2 )
				&& ( prevTri.X3 == tri.X3 )
				&& ( prevTri.Y1 == tri.Y1 )
				&& ( prevTri.Y2 == tri.Y2 )
				&& ( prevTri.Y3 == tri.Y3 ) )
			{
				doAdd = false;
				//t -= 0.05f;
				t += 0.01f;
			}
		}
		else
		{
			tri.X1 = SCR_WIDTH/2;
			tri.Y1 = 0;
		}

		prevTri = tri;

		if ( doAdd )
		{
			if ( progress >= oldProgress )
			{
				tri.index = 1;
			}
			else
			{
				tri.index = 0;
			}
			m_renderer.AddTriangle(tri);

			m_renderer.RenderFrame(file);
		}
	}
		
	_frameCount++;
}