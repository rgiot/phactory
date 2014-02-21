
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <vector>
#include <sstream>
#include <algorithm>

#include "config.h"
#include "window.h"
#include "pixel.h"
#include "renderer.h"

#include "fxbase.h"

Renderer::Renderer()
{
	const float borderSize = 0.0f;

	m_clipX1 = borderSize;
	m_clipY1 = borderSize;
	m_clipX2 = SCR_WIDTH-2-borderSize;
	m_clipY2 = SCR_HEIGHT-1-borderSize;

	DoClipping = true;
}

Renderer::~Renderer()
{
}

void Renderer::ClearFrame()
{
	m_triangles.clear();
}

void Renderer::AddTriangle(Triangle srcTriangle)
{
	Triangle tri;
	tri.X1 = srcTriangle.X1;
	tri.X2 = srcTriangle.X2;
	tri.X3 = srcTriangle.X3;
	tri.Y1 = srcTriangle.Y1;
	tri.Y2 = srcTriangle.Y2;
	tri.Y3 = srcTriangle.Y3;
	tri.index = srcTriangle.index;

	SortY( tri );

	std::vector<Triangle> triangleList;
	triangleList.push_back( tri );

	//ClipLeft( triangleList );
	//ClipRight( triangleList );
	//ClipTop( triangleList );
	//ClipBottom( triangleList );

	for( int iTriangle = 0; iTriangle < triangleList.size(); iTriangle++ )
	{
		m_triangles.push_back(triangleList[iTriangle]);
	}
}

unsigned int Renderer::RenderFrame_ClipTriangle_AddY_Render( int iIndex )
{
	unsigned int color = 0;
		
	for(unsigned int iTriangle=0; iTriangle<m_triangles.size(); iTriangle++)
	{
		Triangle baseTri = m_triangles[iTriangle];

		if ( baseTri.index != iIndex )
		{
			continue;
		}

		std::vector<Triangle> clippedTriangles = ClipTriangle( baseTri );
		for ( int iClipTriangle = 0; iClipTriangle < clippedTriangles.size(); iClipTriangle++ )
		{
			Triangle tri = clippedTriangles[iClipTriangle];

			SortY( tri );

			if ( tri.index == 0 )
			{
				color =  COLOR_INDEX_0;
			}
			else
			{
				color =  COLOR_INDEX_1;
			}

			unsigned char x1 = (unsigned char) (ceil(tri.X1));
			unsigned char x2 = (unsigned char) (ceil(tri.X2));
			unsigned char x3 = (unsigned char) (ceil(tri.X3));

			unsigned char y1 = (unsigned char) (ceil(tri.Y1));
			unsigned char y2 = (unsigned char) (ceil(tri.Y2));
			unsigned char y3 = (unsigned char) (ceil(tri.Y3));
			
			AddY( y1 );
			AddY( y2 );
			AddY( y3 );

			drawLine(x1, y1, x2, y2, color );
			drawLine(x2, y2, x3, y3, color );
			drawLine(x1, y1, x3, y3, color );
		}
	}

	return color;
}

void Renderer::RenderFrame(FILE *file)
{
	for ( int iIndex = 0; iIndex < 2; iIndex++ )
	{	
		Yarray.clear();
		drawRect(0, 0, SCR_WIDTH, SCR_HEIGHT, 0 );

		unsigned int color = RenderFrame_ClipTriangle_AddY_Render( iIndex );

		RenderFrame_WriteArray(file, iIndex, color);
	}

	unsigned char endTag = 199;
	fwrite(&endTag, sizeof(unsigned char), 1, file);
}

void Renderer::RenderFrame_WriteArray(FILE *file, int iIndex, unsigned int color)
{
	if ( Yarray.size() <= 1 )
	{
		return;
	}
		
	std::sort(Yarray.begin(), Yarray.end());

	for ( int yIndex = 0; yIndex < Yarray.size()-1; yIndex++ )
	{
		unsigned char startY = Yarray[yIndex];
		unsigned char endY = Yarray[yIndex+1];

		Line lineLeft;
		lineLeft.X1 = GetLeftPixel( startY, color );
		lineLeft.Y1 = startY;
		lineLeft.X2 = GetLeftPixel( endY, color );
		lineLeft.Y2 = endY;
		float deltaLeft = GetDelta(lineLeft);
		
		Line lineRight;
		lineRight.X1 = GetRightPixel( startY, color );
		lineRight.Y1 = startY;
		lineRight.X2 = GetRightPixel( endY, color );
		lineRight.Y2 = endY;
		float deltaRight = GetDelta(lineRight);

		CheckFloatToUnsignedShort( deltaLeft );
		CheckFloatToUnsignedShort( deltaRight );
		CheckXInScreen( lineLeft.X1 );
		CheckXInScreen( lineRight.X1 );

		if ( deltaLeft<-127 )
		{
			deltaLeft = -127;
		}
		else if ( deltaLeft>127)
		{
			deltaLeft = 127;
		}
		if ( deltaRight<-127 )
		{
			deltaRight = -127;
		}
		else if ( deltaRight>127)
		{
			deltaRight = 127;
		}
		
		unsigned short delta1 = (unsigned short)( deltaLeft * 256.0f );
		unsigned char X1 = (unsigned char) lineLeft.X1;
		unsigned short delta2 = (unsigned short)( deltaRight * 256.0f );
		unsigned char X2 = (unsigned char) lineRight.X1;

		fwrite( &X1, sizeof( unsigned char ), 1, file );
		fwrite( &delta1, sizeof( unsigned short ), 1, file );
		fwrite( &X2, sizeof( unsigned char ), 1, file );
		fwrite( &delta2, sizeof( unsigned short ), 1, file );

		fwrite( &startY, sizeof( unsigned char ), 1, file );

		endY++;
		fwrite( &endY, sizeof( unsigned char ), 1, file );

		CheckYInScreen( startY );
		CheckYInScreen( endY );

		unsigned char triIndex = iIndex;
		fwrite( &triIndex, sizeof( unsigned char ), 1, file );
	}
}

bool Renderer::CheckFloatToUnsignedShort( float value )
{
	if ( value < -127.0f )
	{
		return false;
	}
	if ( value >= 127.0f )
	{
		return false;
	}

	return true;
}

bool Renderer::CheckXInScreen( unsigned char y )
{
	if ( y > 195 )
	{
		return false;
	}

	return true;
}

bool Renderer::CheckYInScreen( unsigned char y )
{
	if ( y > 205 )
	{
		return false;
	}

	return true;
}

unsigned char Renderer::GetLeftPixel(unsigned char y, unsigned int color)
{
	for ( unsigned char x = 0; x < SCR_WIDTH; x++ )
	{
		if ( getPixel(x, y) == color )
		{
			return x;
		}
	}

	for ( unsigned char x = 0; x < SCR_WIDTH; x++ )
	{
		if ( getPixel(x, y-1) == color )
		{
			return x;
		}
	}
	
	return -1;
}

unsigned char Renderer::GetRightPixel(unsigned char y, unsigned int color)
{
	for ( unsigned char x = SCR_WIDTH-1; x != (unsigned char)-1; x-- )
	{
		if ( getPixel(x, y) == color )
		{
			return x;
		}
	}

	for ( unsigned char x = SCR_WIDTH-1; x != (unsigned char)-1; x-- )
	{
		if ( getPixel(x, y-1) == color )
		{
			return x;
		}
	}
	
	return -1;
}

unsigned char Renderer::GetTopPixel(unsigned char x, unsigned int color)
{
	for ( unsigned char y = 0; y < SCR_HEIGHT; y++ )
	{
		if ( getPixel(x, y) == color )
		{
			return y;
		}
	}

	return -1;
}

unsigned char Renderer::GetBottomPixel(unsigned char x, unsigned int color)
{
	for ( unsigned char y = SCR_HEIGHT-1; y != (unsigned char)-1; y-- )
	{
		if ( getPixel(x, y) == color )
		{
			return y;
		}
	}

	return -1;
}

void Renderer::AddY( unsigned char y)
{
	bool found;
	int i;

	found = false;
	for ( i = 0; i < Yarray.size(); i++ )
	{
		if ( Yarray[i] == y )
		{
			found = true;
		}
	}
	if ( !found )
	{
		Yarray.push_back(y);
	}
}

float Renderer::GetDelta( Line line )
{
	double x;
	double y;

	if ( line.Y2 < line.Y1 )
	{
		y = line.Y1;
		line.Y1 = line.Y2;
		line.Y2 = y;

		x = line.X1;
		line.X1 = line.X2;
		line.X2 = x;
	}

	double d1 = line.X2 - line.X1;
	double d2 = line.Y2 - line.Y1;

	if ( d2 == 0.0f )
	{
		d2 = 0.0001f;
	}

	double fDelta = d1 / d2;

	return (float)fDelta;
}

float Renderer::GetNormal( Triangle triangle )
{
	Point a, b;
	a.X = triangle.X2-triangle.X1;
	a.Y = triangle.Y2-triangle.Y1;
	b.X = triangle.X3-triangle.X1;
	b.Y = triangle.Y3-triangle.Y1;

	float z = (a.X*b.Y)-(b.X*a.Y);
	return z;
}

Line Renderer::ExtendLineToScreenBoundaries( Line line )
{	
	Line outLine;

	if ( line.X2==line.X1 )
	{
		line.X2 += 0.0001f;
	}
	float slope = (line.Y2-line.Y1)/(line.X2-line.X1);

	if ( slope > 0.0f )
	{	
		if ( line.Y2==line.Y1 )
		{
			line.Y2 += 0.0001f;
		}

		slope = (line.X2-line.X1)/(line.Y2-line.Y1);
		
		float b = (slope*line.Y1)-line.X1;
		outLine.X1 = 0;
		outLine.Y1 = (outLine.X1+b)/slope;
		outLine.X2 = SCR_WIDTH-1;	
		outLine.Y2 = (outLine.X2+b)/slope;
	}
	else
	{
		float b = (slope*line.X1)-line.Y1;
		outLine.Y1 = 0;
		outLine.X1 = (outLine.Y1+b)/slope;
		outLine.Y2 = SCR_HEIGHT;	
		outLine.X2 = (outLine.Y2+b)/slope;
	}

	return outLine;
}

Point Renderer::IntersectLines( Line line1, Line line2 )
{
	float x0 = line1.X1;
	float y0 = line1.Y1;
	float x1 = line1.X2;
	float y1 = line1.Y2;

	float x2 = line2.X1;
	float y2 = line2.Y1;
	float x3 = line2.X2;
	float y3 = line2.Y2;

   // this function computes the intersection of the sent lines
   // and returns the intersection point, note that the function assumes
   // the lines intersect. the function can handle vertical as well
   // as horizontal lines. note the function isn't very clever, it simply
   //applies the math, but we don't need speed since this is a
   //pre-processing step
   
   float a1,b1,c1, // constants of linear equations     
   a2,b2,c2,      
   det_inv,  // the inverse of the determinant of the coefficientmatrix    
   m1,m2;    // the slopes of each line
   
   // compute slopes, note the cludge for infinity, however, this will
   // be close enough
   if ((x1-x0)!=0)   
	m1 = (y1-y0)/(x1-x0);
	else   
	m1 = (float)1e+10;   
	// close enough to infinity
	if ((x3-x2)!=0)   
		m2 = (y3-y2)/(x3-x2);
	else  
		m2 = (float)1e+10;  

	// close enough to infinity
	// compute constants
	a1 = m1;
	a2 = m2;b1 = -1;b2 = -1;c1 = (y0-m1*x0);c2 = (y2-m2*x2);
	// compute the inverse of the determinate
	det_inv = 1/(a1*b2 - a2*b1);
	
	// use Kramers rule to compute xi and yi
	Point point;
	point.X=((b1*c2 - b2*c1)*det_inv);
	point.Y=((a2*c1 - a1*c2)*det_inv);

	return point;
}

std::vector<Triangle> Renderer::ClipTriangle(Triangle tri)
{
	SortY( tri );

	std::vector<Triangle> triangleList;
	triangleList.push_back( tri );

	if ( DoClipping )
	{
		ClipLeft( triangleList );
		ClipRight( triangleList );
		ClipTop( triangleList );
		ClipBottom( triangleList );
	}

	return triangleList;
}

void Renderer::ClipLeft(std::vector<Triangle> &triangleList)
{	
	int iTriangle;
	float mul;

	for ( iTriangle = 0; iTriangle < triangleList.size(); iTriangle++ )
	{
		Triangle tri = triangleList[iTriangle];

		if ( (tri.X1 < m_clipX1) || (tri.X2 < m_clipX1) || (tri.X3 < m_clipX1) )
		{
			SortX( tri );
			
			triangleList.erase( triangleList.begin() + iTriangle );
			iTriangle = -1;
					
			if (tri.X3 >= m_clipX1)
			{
				if (tri.X2 < m_clipX1)
				{
					Triangle newTri;
					newTri.X1 = tri.X3;
					newTri.Y1 = tri.Y3;

					if ( tri.X3 == tri.X1 )
					{
						mul = (tri.X3 - m_clipX1 ) / ( tri.X3 - tri.X1 + 0.000001f);
					}
					else
					{
						
						mul = (tri.X3 - m_clipX1 ) / ( tri.X3 - tri.X1 );
					}

					newTri.X2 = m_clipX1;
					newTri.Y2 = tri.Y3 + mul * ( tri.Y1 - tri.Y3);

					if (tri.X3 == tri.X2 )
					{
						mul = ( tri.X3 - m_clipX1 ) / ( tri.X3 - tri.X2 + 0.000001f );
					}
					else
					{
						mul = ( tri.X3 - m_clipX1 ) / ( tri.X3 - tri.X2 );
					}

					newTri.X3 = m_clipX1;
					newTri.Y3 = tri.Y3 + mul * ( tri.Y2 - tri.Y3 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
				else if ( tri.X1 < m_clipX1 )
				{
					Triangle newTri;
					newTri.X1 = tri.X3;
					newTri.Y1 = tri.Y3;
					
					newTri.X2 = tri.X2;
					newTri.Y2 = tri.Y2;

					if ( tri.X3 == tri.X1 )
					{
						mul = ( tri.X3 - m_clipX1 ) / ( tri.X3 - tri.X1 + 0.000001f );
					}
					else
					{
						mul = ( tri.X3 - m_clipX1 ) / ( tri.X3 - tri.X1 );
					}

					newTri.X3 = m_clipX1;
					newTri.Y3 = tri.Y3 + mul * ( tri.Y1 - tri.Y3 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );

					
					if (tri.X2 == tri.X1 )
					{
						mul = ( tri.X2 - m_clipX1 ) / ( tri.X2 - tri.X1 + 0.000001f );
					}
					else
					{
						mul = ( tri.X2 - m_clipX1 ) / ( tri.X2 - tri.X1 );
					}

					newTri.X1 = m_clipX1;
					newTri.Y1 = tri.Y2 + mul * ( tri.Y1 - tri.Y2 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
			}
		}
	}
}

void Renderer::ClipRight(std::vector<Triangle> &triangleList)
{
	int iTriangle;
	float t;
	float mul;

	for ( iTriangle = 0; iTriangle < triangleList.size(); iTriangle++ )
	{
		Triangle tri = triangleList[iTriangle];

		if ( (tri.X1 > m_clipX2) || (tri.X2 > m_clipX2) || (tri.X3 > m_clipX2) )
		{
			SortX( tri );

			triangleList.erase( triangleList.begin() + iTriangle );
			iTriangle = -1;

			if (tri.X1 < m_clipX2)
			{
				if (tri.X2 > m_clipX2)
				{
					Triangle newTri;
					newTri.X1 = tri.X1;
					newTri.Y1 = tri.Y1;

					if ( tri.X2 == tri.X1 )
					{
						mul = ( m_clipX2 - tri.X1 ) / ( tri.X2 - tri.X1 + 0.000001f);
					}
					else
					{
						
						mul = ( m_clipX2 - tri.X1 ) / ( tri.X2 - tri.X1 );
					}

					newTri.X2 = m_clipX2;
					newTri.Y2 = tri.Y1 + mul * ( tri.Y2 - tri.Y1);

					if (tri.X3 == tri.X1)
					{
						mul = ( m_clipX2 - tri.X1 ) / ( tri.X3 - tri.X1 + 0.000001f );
					}
					else
					{
						mul = ( m_clipX2 - tri.X1 ) / ( tri.X3 - tri.X1 );
					}

					newTri.X3 = m_clipX2;
					newTri.Y3 = tri.Y1 + mul * ( tri.Y3 - tri.Y1 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
				else if ( tri.X3 > m_clipX2 )
				{
					Triangle newTri;
					newTri.X1 = tri.X1;
					newTri.Y1 = tri.Y1;
					
					newTri.X2 = tri.X2;
					newTri.Y2 = tri.Y2;

					if ( tri.X3 == tri.X1 )
					{
						mul = ( m_clipX2 - tri.X1 ) / ( tri.X3 - tri.X1 + 0.000001f );
					}
					else
					{
						mul = ( m_clipX2 - tri.X1 ) / ( tri.X3 - tri.X1 );
					}

					newTri.X3 = m_clipX2;
					newTri.Y3 = tri.Y1 + mul * ( tri.Y3 - tri.Y1 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );

					
					if (tri.X3 == tri.X2 )
					{
						mul = ( m_clipX2 - tri.X2 ) / ( tri.X3 - tri.X2 + 0.000001f );
					}
					else
					{
						mul = ( m_clipX2 - tri.X2 ) / ( tri.X3 - tri.X2 );
					}

					newTri.X1 = m_clipX2;
					newTri.Y1 = tri.Y2 + mul * ( tri.Y3 - tri.Y2 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
			}
		}
	}	
}

void Renderer::ClipTop(std::vector<Triangle> &triangleList)
{	
	int iTriangle;
	float t;
	float mul;

	for ( iTriangle = 0; iTriangle < triangleList.size(); iTriangle++ )
	{
		Triangle tri = triangleList[iTriangle];

		if ( (tri.Y1 < m_clipY1) || (tri.Y2 < m_clipY1) || (tri.Y3 < m_clipY1) )
		{
			SortY( tri );

			triangleList.erase( triangleList.begin() + iTriangle );
			iTriangle = -1;

			if (tri.Y3 >= m_clipY1)
			{
				if (tri.Y2 < m_clipY1)
				{
					Triangle newTri;
					newTri.X1 = tri.X3;
					newTri.Y1 = tri.Y3;

					if ( tri.Y3 == tri.Y1 )
					{
						mul = (tri.Y3 - m_clipY1 ) / ( tri.Y3 - tri.Y1 + 0.000001f);
					}
					else
					{
						
						mul = (tri.Y3 - m_clipY1 ) / ( tri.Y3 - tri.Y1 );
					}

					newTri.Y2 = m_clipY1;
					newTri.X2 = tri.X3 + mul * ( tri.X1 - tri.X3);

					if (tri.Y3 == tri.Y2 )
					{
						mul = ( tri.Y3 - m_clipY1 ) / ( tri.Y3 - tri.Y2 + 0.000001f );
					}
					else
					{
						mul = ( tri.Y3 - m_clipY1 ) / ( tri.Y3 - tri.Y2 );
					}

					newTri.Y3 = m_clipY1;
					newTri.X3 = tri.X3 + mul * ( tri.X2 - tri.X3 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
				else if ( tri.Y1 < m_clipY1 )
				{
					Triangle newTri;
					newTri.X1 = tri.X3;
					newTri.Y1 = tri.Y3;
					
					newTri.X2 = tri.X2;
					newTri.Y2 = tri.Y2;

					if ( tri.Y3 == tri.Y1 )
					{
						mul = ( tri.Y3 - m_clipY1 ) / ( tri.Y3 - tri.Y1 + 0.000001f );
					}
					else
					{
						mul = ( tri.Y3 - m_clipY1 ) / ( tri.Y3 - tri.Y1 );
					}

					newTri.Y3 = m_clipY1;
					newTri.X3 = tri.X3 + mul * ( tri.X1 - tri.X3 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );

					
					if (tri.Y2 == tri.Y1 )
					{
						mul = ( tri.Y2 - m_clipY1 ) / ( tri.Y2 - tri.Y1 + 0.000001f );
					}
					else
					{
						mul = ( tri.Y2 - m_clipY1 ) / ( tri.Y2 - tri.Y1 );
					}

					newTri.Y1 = m_clipY1;
					newTri.X1 = tri.X2 + mul * ( tri.X1 - tri.X2 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
			}
		}
	}
}

void Renderer::ClipBottom(std::vector<Triangle> &triangleList)
{	
	int iTriangle;
	float t;
	float mul;

	for ( iTriangle = 0; iTriangle < triangleList.size(); iTriangle++ )
	{
		Triangle tri = triangleList[iTriangle];

		if ( (tri.Y1 > m_clipY2) || (tri.Y2 > m_clipY2) || (tri.Y3 > m_clipY2) )
		{
			SortY( tri );
			
			triangleList.erase( triangleList.begin() + iTriangle );
			iTriangle = -1;

			if (tri.Y1 < m_clipY2)
			{
				if (tri.Y2 > m_clipY2)
				{
					Triangle newTri;
					newTri.X1 = tri.X1;
					newTri.Y1 = tri.Y1;

					if ( tri.Y2 == tri.Y1 )
					{
						mul = ( m_clipY2 - tri.Y1 ) / ( tri.Y2 - tri.Y1 + 0.000001f);
					}
					else
					{
						
						mul = ( m_clipY2 - tri.Y1 ) / ( tri.Y2 - tri.Y1 );
					}

					newTri.Y2 = m_clipY2;
					newTri.X2 = tri.X1 + mul * ( tri.X2 - tri.X1);

					if (tri.Y3 == tri.Y1)
					{
						mul = ( m_clipY2 - tri.Y1 ) / ( tri.Y3 - tri.Y1 + 0.000001f );
					}
					else
					{
						mul = ( m_clipY2 - tri.Y1 ) / ( tri.Y3 - tri.Y1 );
					}

					newTri.Y3 = m_clipY2;
					newTri.X3 = tri.X1 + mul * ( tri.X3 - tri.X1 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
				else if ( tri.Y3 > m_clipY2 )
				{
					Triangle newTri;
					newTri.X1 = tri.X1;
					newTri.Y1 = tri.Y1;
					
					newTri.X2 = tri.X2;
					newTri.Y2 = tri.Y2;

					if ( tri.Y3 == tri.Y1 )
					{
						mul = ( m_clipY2 - tri.Y1 ) / ( tri.Y3 - tri.Y1 + 0.000001f );
					}
					else
					{
						mul = ( m_clipY2 - tri.Y1 ) / ( tri.Y3 - tri.Y1 );
					}

					newTri.Y3 = m_clipY2;
					newTri.X3 = tri.X1 + mul * ( tri.X3 - tri.X1 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );

					
					if (tri.Y3 == tri.Y2 )
					{
						mul = ( m_clipY2 - tri.Y2 ) / ( tri.Y3 - tri.Y2 + 0.000001f );
					}
					else
					{
						mul = ( m_clipY2 - tri.Y2 ) / ( tri.Y3 - tri.Y2 );
					}

					newTri.Y1 = m_clipY2;
					newTri.X1 = tri.X2 + mul * ( tri.X3 - tri.X2 );

					newTri.index = tri.index;

					triangleList.push_back( newTri );
				}
			}
		}
	}	
}

void Renderer::SortY(Triangle &tri)
{
	float t;
	if ( tri.Y2 < tri.Y1 )
	{
		t = tri.X1;
		tri.X1 = tri.X2;
		tri.X2 = t;
		t = tri.Y1;
		tri.Y1 = tri.Y2;
		tri.Y2 = t;
	}
	if ( tri.Y3 < tri.Y2 )
	{
		t = tri.X2;
		tri.X2 = tri.X3;
		tri.X3 = t;
		t = tri.Y2;
		tri.Y2 = tri.Y3;
		tri.Y3 = t;
	}
	if ( tri.Y2 < tri.Y1 )
	{
		t = tri.X1;
		tri.X1 = tri.X2;
		tri.X2 = t;
		t = tri.Y1;
		tri.Y1 = tri.Y2;
		tri.Y2 = t;
	}
}

void Renderer::SortX(Triangle &tri)
{
	float t;
	if ( tri.X2 < tri.X1 )
	{
		t = tri.X1;
		tri.X1 = tri.X2;
		tri.X2 = t;
		t = tri.Y1;
		tri.Y1 = tri.Y2;
		tri.Y2 = t;
	}
	if ( tri.X3 < tri.X2 )
	{
		t = tri.X2;
		tri.X2 = tri.X3;
		tri.X3 = t;
		t = tri.Y2;
		tri.Y2 = tri.Y3;
		tri.Y3 = t;
	}
	if ( tri.X2 < tri.X1 )
	{
		t = tri.X1;
		tri.X1 = tri.X2;
		tri.X2 = t;
		t = tri.Y1;
		tri.Y1 = tri.Y2;
		tri.Y2 = t;
	}
}