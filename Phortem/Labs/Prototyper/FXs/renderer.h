
#ifndef __RENDERER_H__
#define __RENDERER_H__

#include <SDL.h>
#include <vector>

struct Point
{
	float X;
	float Y;
};

struct Line
{
	float X1, Y1;
	float X2, Y2;
};

struct Triangle
{
	float X1, Y1;
	float X2, Y2;
	float X3, Y3;
	unsigned char index;
};

class Renderer
{
public:
	Renderer();
	~Renderer();

	void ClearFrame();
	void AddTriangle(Triangle srcTriangle);
	void RenderFrame(FILE *file);

	Line ExtendLineToScreenBoundaries( Line line );
	Point IntersectLines( Line line1, Line line2 );
	float GetDelta( Line line );
	float GetNormal( Triangle triangle );

	void SortY(Triangle &triangle);
	void SortX(Triangle &triangle);

	std::vector<Triangle> ClipTriangle(Triangle triangle);

	bool DoClipping;

private:
	std::vector<Triangle> m_triangles;

	float m_clipX1, m_clipY1, m_clipX2, m_clipY2;

	void ClipLeft(std::vector<Triangle> &triangleList);
	void ClipRight(std::vector<Triangle> &triangleList);
	void ClipTop(std::vector<Triangle> &triangleList);
	void ClipBottom(std::vector<Triangle> &triangleList);

	unsigned int RenderFrame_ClipTriangle_AddY_Render( int iIndex );
	void RenderFrame_WriteArray(FILE *file, int iIndex, unsigned int color);

	unsigned char GetLeftPixel(unsigned char y, unsigned int color);
	unsigned char GetRightPixel(unsigned char y, unsigned int color);
	unsigned char GetTopPixel(unsigned char x, unsigned int color);
	unsigned char GetBottomPixel(unsigned char x, unsigned int color);

	std::vector<int> Yarray;
	void AddY( unsigned char y );

	bool CheckFloatToUnsignedShort( float value );
	bool CheckXInScreen( unsigned char y );
	bool CheckYInScreen( unsigned char y );
};

#endif // __RENDERER_H__