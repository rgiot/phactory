
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "TransitLines.h"

static float _deg = -180.0f;
static int _frameCount = 0;

static Line _lines[3]; // 0=current 1=prev 2=prevPrev

extern bool YLessPrecision;

void TransitLines::TransitLinesFX1()
{
	if ( file == 0 )
	{
		file = fopen("..\\..\\winape\\src\\transit\\TransitLinesFX1.bin", "wb" );
	}

	YLessPrecision = true;

	drawRect(0, 0, SCR_WIDTH, SCR_HEIGHT, 0 );

	const float step = 1.7f;
	const float clipHeight = 0.0f;

	float rad = DEG2RAD(_deg);
	if (_deg >= (360.0f+70.0f+step)) // (180.0f+step))
	{
		SendQuitEvent();
	}
	_deg += step;
	float sinX = rad;

	float sinPosX1 ;
	float sinPosY1;
	float sinPosX2;
	float sinPosY2 ;
	
	if ( _deg<=(180.0f+step*25) )
	{
		 sinPosX1 = (SCR_WIDTH/2)+cos(sinX*1.0f)*372;
		 sinPosY1 = ((SCR_HEIGHT/2))+sin(sinX+0.1805f)*170;
		 sinPosX2 = (SCR_WIDTH/2)-cos(sinX+0.5f)*202;
		 sinPosY2 = ((SCR_HEIGHT/2))-sin(sinX+0.1805f)*352;
	}
	else
	{
		 sinPosX1 = (SCR_WIDTH/2)+cos(sinX*0.85f)*252;
		 sinPosY1 = 0;
		 sinPosX2 = (SCR_WIDTH/2)+cos(sinX+0.6f)*52;
		 sinPosY2 = 505;
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
		if ( (intersectPoint.Y>=clipHeight) && (intersectPoint.Y<(SCR_HEIGHT-clipHeight)) )
		{	
			tri.X1 = _lines[2].X1;
			tri.Y1 = _lines[2].Y1;
			tri.X2 = _lines[0].X1;
			tri.Y2 = _lines[0].Y1;
			tri.X3 = intersectPoint.X;
			tri.Y3 = intersectPoint.Y;

			z = m_renderer.GetNormal( tri );
			if ( z < 0.0f )
			{
				tri.index = 0;
			}
			else
			{
				tri.index = 1;
			}
			m_renderer.AddTriangle(tri);

			tri.X1 = intersectPoint.X;
			tri.Y1 = intersectPoint.Y;
			tri.X2 = _lines[0].X2;
			tri.Y2 = _lines[0].Y2;
			tri.X3 = _lines[2].X2;
			tri.Y3 = _lines[2].Y2;

			z = m_renderer.GetNormal( tri );
			if ( z < 0.0f )
			{
				tri.index = 0;
			}
			else
			{
				tri.index = 1;
			}
			m_renderer.AddTriangle(tri);
		}
		else
		{
			tri.X1 = _lines[0].X1;
			tri.Y1 = _lines[0].Y1;
			tri.X2 = _lines[0].X2;
			tri.Y2 = _lines[0].Y2;
			tri.X3 = _lines[2].X1;
			tri.Y3 = _lines[2].Y1;

			z = m_renderer.GetNormal( tri );
			if ( z < 0.0f )
			{
				tri.index = 0;
			}
			else
			{
				tri.index = 1;
			}
			m_renderer.AddTriangle(tri);

			tri.X1 = _lines[2].X1;
			tri.Y1 = _lines[2].Y1;
			tri.X2 = _lines[0].X2;
			tri.Y2 = _lines[0].Y2;
			tri.X3 = _lines[2].X2;
			tri.Y3 = _lines[2].Y2;

			z = m_renderer.GetNormal( tri );
			if ( z < 0.0f )
			{
				tri.index = 0;
			}
			else
			{
				tri.index = 1;
			}
			//m_renderer.AddTriangle(tri);
		}

		m_renderer.RenderFrame(file);
	}
		
	_frameCount++;
}