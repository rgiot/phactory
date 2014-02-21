
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <vector>
#include <assert.h>

#include <SDL.h>

#include "config.h"
#include "pixel.h"
#include "window.h"

#include "Angel.h"

#define IMAGEWIDTH 308
#define IMAGEHEIGHT 205

#define BLOCKWIDTH 2
#define BLOCKHEIGHT 5
struct Block
{
	unsigned char Sprite[BLOCKWIDTH * BLOCKHEIGHT];
};

struct VerticalLineItem
{
	unsigned char Y;
	unsigned char BlockIndex;
	std::vector<bool> IgnoreLines;
};

struct VerticalLine
{
	std::vector< VerticalLineItem > Items;
	unsigned short offset;
};

std::vector<Block> Charset;

Block CreateBlock( unsigned char *baseBuffer, int posX, int posY )
{
	Block block;

	for ( int y = 0; y < BLOCKHEIGHT; y++ )
	{
		for ( int x = 0; x < BLOCKWIDTH; x++ )
		{
			bool isON = ( baseBuffer[ ((y+posY) * IMAGEWIDTH) + (x+posX) ] == 0xFF );

			block.Sprite[(y*BLOCKWIDTH)+x] = isON ? 0 : 255;
		}
	}

	return block;
}

bool DoesBlockEquals( Block block1, Block block2 )
{
	int countSame = 0;

	for ( int y = 0; y < BLOCKHEIGHT; y++ )
	{
		for ( int x = 0; x < BLOCKWIDTH; x++ )
		{
			if ( block1.Sprite[(y*BLOCKWIDTH)+x] == block2.Sprite[(y*BLOCKWIDTH)+x])
			{
				countSame++;
			}
		}
	}

	if ( countSame == BLOCKWIDTH*BLOCKHEIGHT )
	{
		return true;
	}

	return false;
}

std::vector<bool> GetIgnoreLines( Block block1, Block block2 )
{
	std::vector<bool> ignoreLines;

	for ( int y = 0; y < BLOCKHEIGHT; y++ )
	{
		int countSame = 0;

		for ( int x = 0; x < BLOCKWIDTH; x++ )
		{
			if (block1.Sprite[(y*BLOCKWIDTH)+x] == block2.Sprite[(y*BLOCKWIDTH)+x])
			{
				countSame++;
			}
		}

		bool ignoreLine = false;
		if ( countSame == BLOCKWIDTH )
		{
			ignoreLine = true;
		}
		ignoreLines.push_back(ignoreLine);
	}

	return ignoreLines;
}

bool DoesBlockExist( Block block )
{
	for ( int i = 0; i < Charset.size(); i++ )
	{
		Block currentBlock = Charset[i];

		if ( DoesBlockEquals( currentBlock, block ) )
		{
			return true;
		}
	}

	return false;
}

unsigned char GetBlockIndex( Block block )
{
	for ( int i = 0; i < Charset.size(); i++ )
	{
		int countSame = 0;

		Block currentBlock = Charset[i];

		for ( int y = 0; y < BLOCKHEIGHT; y++ )
		{
			for ( int x = 0; x < BLOCKWIDTH; x++ )
			{
				if ( currentBlock.Sprite[(y*BLOCKWIDTH)+x] == block.Sprite[(y*BLOCKWIDTH)+x])
				{
					countSame++;
				}
			}
		}

		if ( countSame == BLOCKWIDTH*BLOCKHEIGHT )
		{
			assert( i < 256 );
			return (unsigned char) i;
		}
	}

	assert(false);
	return -1;
}

unsigned short GetBlockScreenPtr( unsigned char yBlock )
{
	if ( yBlock < 100/5 )
	{
		return 0x8080 + 96 * ((unsigned short)(yBlock));
	}
	else
	{
		return 0xC000 + 96 * ((unsigned short)(yBlock-(100/5)));
	}
}

unsigned char entryBank = 0x00;
unsigned char exitBank = 0x00;

unsigned short GetCodePtr( bool isTop, unsigned char type, unsigned char yInBlock, bool ignoreLine )
{
	unsigned short ptr = 0;
	unsigned short bankOffset = 0;

	unsigned short basePtr = 0xBA00;

	if ( ignoreLine )
	{
		if ( isTop && ( yInBlock == 0 ) ) return basePtr + 0x0000;
		if ( isTop && ( yInBlock == 1 ) ) return basePtr + 0x0010;
		if ( isTop && ( yInBlock == 2 ) ) return basePtr + 0x0020;
		if ( isTop && ( yInBlock == 3 ) ) return basePtr + 0x0030;
		if ( isTop && ( yInBlock == 4 ) ) return basePtr + 0x0040;

		if ( !isTop && ( yInBlock == 0 ) ) return basePtr + 0x0050;
		if ( !isTop && ( yInBlock == 1 ) ) return basePtr + 0x0060;
		if ( !isTop && ( yInBlock == 2 ) ) return basePtr + 0x0070;
		if ( !isTop && ( yInBlock == 3 ) ) return basePtr + 0x0080;
		if ( !isTop && ( yInBlock == 4 ) ) return basePtr + 0x0090;
	}

	basePtr += 0x0100;

	if ( isTop && ( type == 0 ) ) entryBank = 0xC6;
	if ( isTop && ( type == 1 ) ) entryBank = 0xC6;
	if ( isTop && ( type == 2 ) ) entryBank = 0xC6;
	if ( isTop && ( type == 3 ) ) entryBank = 0xC4;
	
	if ( !isTop && ( type == 0 ) ) entryBank = 0xC7;
	if ( !isTop && ( type == 1 ) ) entryBank = 0xC7;
	if ( !isTop && ( type == 2 ) ) entryBank = 0xC7;
	if ( !isTop && ( type == 3 ) ) entryBank = 0xC0;

	if ( (exitBank != 0x00) && ( entryBank == exitBank ) )
	{
		bankOffset = 4; // skip LD C, XX  OUT (C), C
	}
	
	if ( isTop && ( type == 0 ) ) exitBank = 0xC6;
	if ( isTop && ( type == 1 ) ) exitBank = 0xC4;
	if ( isTop && ( type == 2 ) ) exitBank = 0xC4;
	if ( isTop && ( type == 3 ) ) exitBank = 0xC4;
	
	if ( !isTop && ( type == 0 ) ) exitBank = 0xC0;
	if ( !isTop && ( type == 1 ) ) exitBank = 0xC0;
	if ( !isTop && ( type == 2 ) ) exitBank = 0xC0;
	if ( !isTop && ( type == 3 ) ) exitBank = 0xC0;

	basePtr += bankOffset;

	if ( isTop && ( type == 0 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x0000;
	if ( isTop && ( type == 1 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x0020;
	if ( isTop && ( type == 2 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x0040;
	if ( isTop && ( type == 3 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x0060;

	if ( isTop && ( type == 0 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x0080;
	if ( isTop && ( type == 1 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x00A0;
	if ( isTop && ( type == 2 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x00C0;
	if ( isTop && ( type == 3 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x00E0;

	basePtr += 0x0100;

	if ( isTop && ( type == 0 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x0000;
	if ( isTop && ( type == 1 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x0020;
	if ( isTop && ( type == 2 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x0040;
	if ( isTop && ( type == 3 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x0060;

	if ( isTop && ( type == 0 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x0080;
	if ( isTop && ( type == 1 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x00A0;
	if ( isTop && ( type == 2 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x00C0;
	if ( isTop && ( type == 3 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x00E0;

	basePtr += 0x0100;

	if ( isTop && ( type == 0 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x0000;
	if ( isTop && ( type == 1 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x0020;
	if ( isTop && ( type == 2 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x0040;
	if ( isTop && ( type == 3 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x0060;

	if ( !isTop && ( type == 0 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x0080;
	if ( !isTop && ( type == 1 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x00A0;
	if ( !isTop && ( type == 2 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x00C0;
	if ( !isTop && ( type == 3 ) && ( yInBlock == 0 ) ) ptr = basePtr + 0x00E0;

	basePtr += 0x0100;

	if ( !isTop && ( type == 0 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x0000;
	if ( !isTop && ( type == 1 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x0020;
	if ( !isTop && ( type == 2 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x0040;
	if ( !isTop && ( type == 3 ) && ( yInBlock == 1 ) ) ptr = basePtr + 0x0060;

	if ( !isTop && ( type == 0 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x0080;
	if ( !isTop && ( type == 1 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x00A0;
	if ( !isTop && ( type == 2 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x00C0;
	if ( !isTop && ( type == 3 ) && ( yInBlock == 2 ) ) ptr = basePtr + 0x00E0;

	basePtr += 0x0100;

	if ( !isTop && ( type == 0 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x0000;
	if ( !isTop && ( type == 1 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x0020;
	if ( !isTop && ( type == 2 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x0040;
	if ( !isTop && ( type == 3 ) && ( yInBlock == 3 ) ) ptr = basePtr + 0x0060;

	if ( !isTop && ( type == 0 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x0080;
	if ( !isTop && ( type == 1 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x00A0;
	if ( !isTop && ( type == 2 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x00C0;
	if ( !isTop && ( type == 3 ) && ( yInBlock == 4 ) ) ptr = basePtr + 0x00E0;

	assert( ptr != 0 );

	return ptr;
}

void GenerateScroll()
{
	FILE *file = fopen("Hope_308x205x1.raw", "rb" );
	fseek(file, 0L, SEEK_END);
	int fileSize = ftell(file);
	fseek(file, 0L, SEEK_SET);
	unsigned char *baseBuffer = new unsigned char[fileSize];
	fread(baseBuffer, 1, fileSize, file);
	fclose(file);

	// Create Charset
	for ( int baseY = 0 ; baseY < IMAGEHEIGHT; baseY += BLOCKHEIGHT )
	{
		for ( int baseX = 0 ; baseX < IMAGEWIDTH; baseX += BLOCKWIDTH )
		{
			Block block = CreateBlock( baseBuffer, baseX, baseY );

			if ( !DoesBlockExist( block ) )
			{
				Charset.push_back( block );
			}
		}
	}

	assert(Charset.size()<256);

	int ignoreLineCount = 0;

	// Create Vertical Lines
	std::vector<VerticalLine> VerticalLines;
	for ( int baseX = 0 ; baseX < IMAGEWIDTH; baseX += BLOCKWIDTH )
	{
		VerticalLine verticalLine;

		for ( int baseY = 0 ; baseY < IMAGEHEIGHT; baseY += BLOCKHEIGHT )
		{
			Block block = CreateBlock( baseBuffer, baseX, baseY );

			VerticalLineItem item;
			item.Y = baseY / BLOCKHEIGHT;
			item.BlockIndex = GetBlockIndex( block );
			
			if ( baseX == 0 )
			{
				item.IgnoreLines.push_back(false);
				item.IgnoreLines.push_back(false);
				item.IgnoreLines.push_back(false);
				item.IgnoreLines.push_back(false);
				item.IgnoreLines.push_back(false);
				verticalLine.Items.push_back(item);
			}
			else
			{
				Block leftBlock = CreateBlock( baseBuffer, baseX-BLOCKWIDTH, baseY );

				if ( DoesBlockEquals( leftBlock, block ) == false )
				{	
					item.IgnoreLines = GetIgnoreLines( leftBlock, block );
					verticalLine.Items.push_back(item);

					for ( int iIgnoreLine = 0; iIgnoreLine < BLOCKHEIGHT; iIgnoreLine++ )
					{
						if ( item.IgnoreLines[iIgnoreLine] )
						{
							ignoreLineCount++;
						}
					}
				}
			}
		}

		VerticalLines.push_back(verticalLine);
	}
	
	file = fopen("..\\..\\..\\trunk\\winape\\src\\angel\\AngelScrollImage.bin", "wb" );
	
	// Write Vertical Lines
	for ( int iVLine = 0 ; iVLine < VerticalLines.size(); iVLine++ )
	{
		VerticalLine vLine = VerticalLines[iVLine];

		unsigned short offset = (unsigned short)ftell(file);
		VerticalLines[iVLine].offset = 0x600 + offset;
		
		unsigned char countValue = vLine.Items.size();
		fwrite(&countValue, 1, 1, file);

		exitBank = 0x00;
		bool bottomReached = false;

		int writePaddingCount = 0;

		int interruptCount = 0;

		for ( int iItem = 0; iItem < vLine.Items.size(); iItem++ )
		{
			VerticalLineItem vLineItem = vLine.Items[iItem];

			unsigned char y = vLineItem.Y;
			bool isTop = ( y < 100/5 );

			if ( !isTop )
			{
				if ( !bottomReached )
				{
					bottomReached = true;
					exitBank = 0x00;
				}
			}

			//fwrite(&y, 1, 1, file);
			unsigned short blockScreenPtr = GetBlockScreenPtr(y);
			blockScreenPtr += 0x2000;
			
			unsigned char blockIndex = vLineItem.BlockIndex;
			//fwrite(&blockIndex, 1, 1, file);
			//unsigned short ptr = 0x0100 + ((unsigned short)blockIndex)*5;
			//fwrite(&ptr, 2, 1, file);

			std::vector<int> orderedY;
			orderedY.push_back(4);
			orderedY.push_back(0);
			orderedY.push_back(1);
			orderedY.push_back(3);
			orderedY.push_back(2);

			int yStart = 0;

			if ( vLineItem.IgnoreLines[ orderedY[0] ] )
			{
				yStart++;
				blockScreenPtr = GetBlockScreenPtr(y) + 0x0000;
				writePaddingCount += 2;

				if ( vLineItem.IgnoreLines[ orderedY[1] ] )
				{
					yStart++;
					blockScreenPtr = GetBlockScreenPtr(y) + 0x0800;
					writePaddingCount += 2;

					if ( vLineItem.IgnoreLines[ orderedY[2] ] )
					{
						yStart++;
						blockScreenPtr = GetBlockScreenPtr(y) + 0x1800;
						writePaddingCount += 2;

						if ( vLineItem.IgnoreLines[ orderedY[3] ] )
						{
							yStart++;
							blockScreenPtr = GetBlockScreenPtr(y) + 0x1000;
							writePaddingCount += 2;
						}
					}
				}
			}

			fwrite(&blockScreenPtr, 2, 1, file);
			
			Block block = Charset[blockIndex];
			for ( int iY = yStart; iY < BLOCKHEIGHT; iY++ )
			{
				int y = orderedY[iY];

				bool leftON = block.Sprite[ ( y * BLOCKWIDTH ) ] != 0;
				bool rightON = block.Sprite[ ( y * BLOCKWIDTH ) + 1 ] != 0;

				unsigned char type;
				if ( (!leftON) && (!rightON) )
				{
					type = 0;
				}
				else if ( (!leftON) && (rightON) )
				{
					type = 1;
				}
				else if ( (leftON) && (!rightON) )
				{
					type = 2;
				}
				else if ( (leftON) && (rightON) )
				{
					type = 3;
				}
				else
				{
					assert( false );
				}

				//fwrite(&type, 1, 1, file);

				bool areOtherLinesIgnored = true;
				int estimatePaddingCount = 0;
				for ( int iY2 = iY; iY2 < BLOCKHEIGHT; iY2++ )
				{
					int y2 = orderedY[iY2];

					if ( vLineItem.IgnoreLines[y2] == false )
					{
						areOtherLinesIgnored = false;
					}
					else
					{
						estimatePaddingCount += 2;
					}
				}

				unsigned short codePtr;

				if ( areOtherLinesIgnored )
				{
					codePtr = GetCodePtr( isTop, type, 2, true );

					writePaddingCount += (estimatePaddingCount-2);
					iY = BLOCKHEIGHT;
				}
				else
				{
					codePtr = GetCodePtr( isTop, type, y, vLineItem.IgnoreLines[y] );
				}

				interruptCount++;
				if ( interruptCount == 50 )
				{
					interruptCount = 0;

					unsigned short interruptJump = 0xBAA0;
					fwrite(&interruptJump, 2, 1, file);
				}

				fwrite(&codePtr, 2, 1, file);
			}
		}
	}
	
	fclose(file);

	file = fopen("..\\..\\..\\trunk\\winape\\src\\angel\\AngelScrollOffset.bin", "wb" );
	
	unsigned short baseOffset = 0x0600;

	// Write Dummy offsets 
	for ( int i = 0; i < 96; i++ )
	{
		VerticalLine vLine = VerticalLines[0];
		unsigned short firstOffset = VerticalLines[1].offset;
		fwrite(&firstOffset, 2, 1, file);
	}

	// Write Offset table 
	unsigned short offset = baseOffset;
	for ( int iVLine = 0 ; iVLine < VerticalLines.size(); iVLine++ )
	{
		VerticalLine vLine = VerticalLines[iVLine];
		
		unsigned short offset = vLine.offset;
		fwrite(&offset, 2, 1, file);
	}
	
	// Write Dummy offsets 
	for ( int i = 0; i < 96; i++ )
	{
		VerticalLine vLine = VerticalLines[0];
		unsigned short firstOffset = VerticalLines[1].offset;
		fwrite(&firstOffset, 2, 1, file);
	}

	fclose(file);

	exit(0);
}
