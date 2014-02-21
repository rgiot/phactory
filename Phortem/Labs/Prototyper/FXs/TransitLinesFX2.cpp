
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

double targetLeft = (SCR_HEIGHT*0.9f);
double targetRight = (SCR_HEIGHT*0.9f);
bool leftWait = true;
bool rightWait = true;
bool SendQuit = false;

void TransitLines::TransitLinesFX2()
{
	if ( file == 0 )
	{
		file = fopen("..\\..\\winape\\src\\transit\\TransitLinesFX2.bin", "wb" );
	}

	drawRect(0, 0, SCR_WIDTH, SCR_HEIGHT, 0 );

	const float speed = 0.95f;

	if (_frameCount > 202*speed ) // 180
	{
		SendQuitEvent();
		return;
	}

	m_renderer.DoClipping = false;	

	double stepLeft = 5.98f/1.5f; // 1.5f
	double stepRight = 4.64f/2.1f; // 2.1f
	const double clipHeight = 0.0f;

	stepLeft *= 1.0f+(1.0-speed);
	stepRight *= 1.0f+(1.0-speed);

	_degLeft += stepLeft;
	_degRight += stepRight;

	double rad = DEG2RAD(_degLeft);
	double sinXLeft = rad;

	rad = DEG2RAD(_degRight);
	double sinXRight = rad;

	double sinPosX1 ;
	double sinPosY1;
	double sinPosX2;
	double sinPosY2 ;

	double leftSinRes = cos(sinXLeft);
	double left = leftSinRes*targetLeft;
	if ( leftWait )
	{
		if ( left > 0 )
		{
			leftWait = false;
			targetLeft *= 0.51f;
		}
	}
	else if ( left <= 0 )
	{
		targetLeft *= 0.51f;
		leftWait = true;
	}
	sinPosY1 = (SCR_HEIGHT)-abs(left);

	double right = cos(sinXRight)*targetRight;
	if ( rightWait )
	{
		if ( right > 0 )
		{
			rightWait = false;
			targetRight *= 0.51f;	
		}
	}
	else if ( right <= 0 )
	{
		targetRight *= 0.51f;
		rightWait = true;
	}
	sinPosY2 = (SCR_HEIGHT)-abs(right);

	if ( SendQuit )
	{
		sinPosY1 = SCR_HEIGHT;
		sinPosY2 = SCR_HEIGHT;
	}

	if ( sinPosY1 == sinPosY2 )
	{
		sinPosY2 += 1.0f;
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

	if ( _frameCount==0)
	{
		_lines[1].X1 = 0;
		_lines[1].Y1 = 0;
		_lines[1].X2 = SCR_WIDTH-2;
		_lines[1].Y2 = 0;
		_lines[0].X1 = 0;
		_lines[0].Y1 = 0;
		_lines[0].X2 = SCR_WIDTH-2;
		_lines[0].Y2 = 0;
	}

	_lines[2] = _lines[1];
	_lines[1] = _lines[0];
	_lines[0].X1 = 0;
	_lines[0].Y1 = sinPosY1;
	_lines[0].X2 = SCR_WIDTH-2;
	_lines[0].Y2 = sinPosY2;
		
	_frameCount++;

	Triangle tri;
	float z;
	
	m_renderer.ClearFrame();

	Point intersectPoint = m_renderer.IntersectLines( _lines[0], _lines[2] );

	if ( (intersectPoint.X < 0) || ( intersectPoint.X >= ( SCR_WIDTH-2) ) )
	{
		tri.X1 = _lines[2].X1;
		tri.Y1 = _lines[2].Y1;
		tri.X2 = _lines[0].X1;
		tri.Y2 = _lines[0].Y1;
		tri.X3 = _lines[2].X2;
		tri.Y3 = _lines[2].Y2;

		z = m_renderer.GetNormal( (tri) );
		if ( z < 0.0f )
		{
			tri.index = 1;
			tri.Y1--;
			if ( tri.Y1<0 ) tri.Y1 = 0;
			tri.Y2--;
			if ( tri.Y2<0 ) tri.Y2 = 0;
			tri.Y3--;
			if ( tri.Y3<0 ) tri.Y3 = 0;
		}
		else
		{
			tri.index = 0;
			tri.Y1++;
			if ( tri.Y1>=SCR_HEIGHT-1 ) tri.Y1 = SCR_HEIGHT-1;
			tri.Y2++;
			if ( tri.Y2>=SCR_HEIGHT-1 ) tri.Y2 = SCR_HEIGHT-1;
			tri.Y3++;
			if ( tri.Y3>=SCR_HEIGHT-1 ) tri.Y3 = SCR_HEIGHT-1;
		}

		m_renderer.SortY(tri);
		if ( tri.Y1 != tri.Y3 )
		{
			m_renderer.AddTriangle(tri);
		}
		else
		{
			SendQuitEvent();
			return;
		}

		tri.X1 = _lines[0].X1;
		tri.Y1 = _lines[0].Y1;
		tri.X2 = _lines[2].X2;
		tri.Y2 = _lines[2].Y2;
		tri.X3 = _lines[0].X2;
		tri.Y3 = _lines[0].Y2;

		z = m_renderer.GetNormal( (tri) );
		if ( z >= 0.0f )
		{
			tri.index = 1;
			tri.Y1--;
			if ( tri.Y1<0 ) tri.Y1 = 0;
			tri.Y2--;
			if ( tri.Y2<0 ) tri.Y2 = 0;
			tri.Y3--;
			if ( tri.Y3<0 ) tri.Y3 = 0;
		}
		else
		{
			tri.index = 0;
			tri.Y1++;
			if ( tri.Y1>=SCR_HEIGHT-1 ) tri.Y1 = SCR_HEIGHT-1;
			tri.Y2++;
			if ( tri.Y2>=SCR_HEIGHT-1 ) tri.Y2 = SCR_HEIGHT-1;
			tri.Y3++;
			if ( tri.Y3>=SCR_HEIGHT-1 ) tri.Y3 = SCR_HEIGHT-1;
		}
		
		m_renderer.SortY(tri);
		if ( tri.Y1 != tri.Y3 )
		{
			m_renderer.AddTriangle(tri);
		}
	}
	else
	{
		tri.X1 = _lines[2].X1;
		tri.Y1 = _lines[2].Y1;
		tri.X2 = _lines[0].X1;
		tri.Y2 = _lines[0].Y1;
		tri.X3 = intersectPoint.X;
		tri.Y3 = intersectPoint.Y;

		z = m_renderer.GetNormal( ReverseTri(tri) );
		if ( z < 0.0f )
		{
			tri.index = 1;

			tri.Y1--;
			if ( tri.Y1<0 ) tri.Y1 = 0;
			tri.Y2--;
			if ( tri.Y2<0 ) tri.Y2 = 0;
			tri.Y3--;
			if ( tri.Y3<0 ) tri.Y3 = 0;
		}
		else
		{
			tri.index = 0;
			tri.Y1++;
			if ( tri.Y1>=SCR_HEIGHT-1 ) tri.Y1 = SCR_HEIGHT-1;
			tri.Y2++;
			if ( tri.Y2>=SCR_HEIGHT-1 ) tri.Y2 = SCR_HEIGHT-1;
			tri.Y3++;
			if ( tri.Y3>=SCR_HEIGHT-1 ) tri.Y3 = SCR_HEIGHT-1;
		}
		m_renderer.SortY(tri);
		if ( tri.Y1 != tri.Y3 )
		{
			m_renderer.AddTriangle(tri);
		}

		tri.X1 = intersectPoint.X;
		tri.Y1 = intersectPoint.Y;
		tri.X2 = _lines[0].X2;
		tri.Y2 = _lines[0].Y2;
		tri.X3 = _lines[2].X2;
		tri.Y3 = _lines[2].Y2;

		z = m_renderer.GetNormal( ReverseTri(tri) );
		if ( z < 0.0f )
		{
			tri.index = 1;
			tri.Y1--;
			if ( tri.Y1<0 ) tri.Y1 = 0;
			tri.Y2--;
			if ( tri.Y2<0 ) tri.Y2 = 0;
			tri.Y3--;
			if ( tri.Y3<0 ) tri.Y3 = 0;
			tri.Y1--;
			if ( tri.Y1<0 ) tri.Y1 = 0;
			tri.Y2--;
			if ( tri.Y2<0 ) tri.Y2 = 0;
			tri.Y3--;
			if ( tri.Y3<0 ) tri.Y3 = 0;
		}
		else
		{
			tri.index = 0;
			tri.Y1++;
			if ( tri.Y1>=SCR_HEIGHT-1 ) tri.Y1 = SCR_HEIGHT-1;
			tri.Y2++;
			if ( tri.Y2>=SCR_HEIGHT-1 ) tri.Y2 = SCR_HEIGHT-1;
			tri.Y3++;
			if ( tri.Y3>=SCR_HEIGHT-1 ) tri.Y3 = SCR_HEIGHT-1;
		}
		m_renderer.SortY(tri);

		if ( tri.Y1 != tri.Y3 )
		{
			m_renderer.AddTriangle(tri);
		}
		else
		{
			//SendQuitEvent();
			SendQuit = true;
			return;
		}
	}

	m_renderer.RenderFrame(file);
}