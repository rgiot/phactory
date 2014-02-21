
// ----------------------------------------------------------------------------
#include "Config.h"
#include "TrackLoader.cxx"
#include "Phortem.cpcdsk.h"

#include "memset.cxx"
#include "memcpy.cxx"
#include "WaitVBL.cxx"
#include "Video.cxx"
#include "Bank.cxx"
#include "Flipping.cxx"
#include "Unpack.cxx"
#include "LoadFile.cxx"

// ----------------------------------------------------------------------------
void RunPart( unsigned char partFileId )
{	
	LoadFile( partFileId, CODESEGMENT0000 );
	Unpack( 0xB200, CODESEGMENT0000 );
	
__asm
	call 0xB200
	jr EndRunPart

RunPartList:
Part1:
	db INTROTITLEBINPCK_TRACKFILEID, INTROBINPCK_TRACKFILEID
	db LENSTITLEBINPCK_TRACKFILEID, LENSBINPCK_TRACKFILEID
	db MANGATITLEBINPCK_TRACKFILEID, MANGABINPCK_TRACKFILEID
	db RUBBERTITLEBINPCK_TRACKFILEID, RUBBERBINPCK_TRACKFILEID
	db PLASMATITLEBINPCK_TRACKFILEID, PLASMABINPCK_TRACKFILEID
	db ANGELTITLEBINPCK_TRACKFILEID, ANGELTITLE2BINPCK_TRACKFILEID, ANGELBINPCK_TRACKFILEID
Part2:
	db BATMANTITLEBINPCK_TRACKFILEID, BATMANBINPCK_TRACKFILEID
	db COSTIXTITLEBINPCK_TRACKFILEID
	db INFINITEZOOMTITLEBINPCK_TRACKFILEID, INFINITEZOOMBINPCK_TRACKFILEID
	db FIGHTTITLEBINPCK_TRACKFILEID, FIGHTBINPCK_TRACKFILEID
	db CHARONTITLEBINPCK_TRACKFILEID
Part3:
	db ENDPARTBINPCK_TRACKFILEID
	db 0
	
Parts:
	dw Part1
	dw Part2
	dw Part3
	
EndRunPart:	
__endasm;	
}

// ----------------------------------------------------------------------------
void main()
{	
	PushBank( 0xC1 );
	
	// Set floppy head to right track
	//LoadFile( INTROTITLEBINPCK_TRACKFILEID, VIDEOSEGMENTC000 );
	
__asm
	ld a, (&8060)
	ld b, 0
	ld c, a	
	ld hl, (TrackListPtr + 1)
	add hl, bc
	ld (TrackListPtr + 1), hl
	
	ld hl, Parts
	add hl, bc	
	ld a, (hl)
	inc hl
	ld h, (hl)
	ld l, a
	
RunPartLoop:
	ld a, (&0004)
	or a
	jr z, RunPartLoop_NoNextMusic
	cp 2
	jr z, RunPartLoop_NoNextMusic
	
	push hl
	
	; already set by mus_init
	;xor a
	;ld ( &0004 ), a
		
TrackListPtr:
	ld hl, TrackList
	ld a, (hl)	
	or a
	jr nz, SkipResetTrack
	ld hl, TrackList
	ld a, (hl)
SkipResetTrack:	
	inc hl
	ex af, af'
	ld a, (hl)
	ld (NextMusicPlayerToLoad+1), a
	ex af, af'
	inc hl	
	ld ( TrackListPtr+1 ), hl
	ld hl, VIDEOSEGMENTC000
	push hl
	call LoadFileDirect
UnpackMusic:
	; Unpack( VIDEOSEGMENT0000+100, VIDEOSEGMENTC000 );
	ld	hl, VIDEOSEGMENTC000
	push	hl
	ld	hl, VIDEOSEGMENT0000+100
	push	hl
	call	_Unpack
	pop	af
	pop	af
	
	pop hl
NextMusicPlayerToLoad:
	ld a, 3
	call LoadFileDirect
UnpackMusicPlayer:
	; Unpack( 0x3800, VIDEOSEGMENTC000 );
	ld	hl, VIDEOSEGMENTC000
	push	hl
	ld	h,  &38
	push	hl
	call	_Unpack
	pop	af
	pop	af
	
	ld hl, RelocateInitNewMusic
	ld de, VIDEOSEGMENT0000
	ld bc, RelocateInitNewMusic_End-RelocateInitNewMusic
	ldir	
	call VIDEOSEGMENT0000
	
	pop hl
	
RunPartLoop_NoNextMusic:
	call NextPart
	jr RunPartLoop
	
TrackList:
	db METALAYCPCK_TRACKFILEID, MUSICPLAYERBINPCK_TRACKFILEID ; metal
	db DUBSTEPAYCPCK_TRACKFILEID, MUSICPLAYERBINPCK_TRACKFILEID_2 ; dubstep
	db 0
	
RelocateInitNewMusic:	
	di
	ld bc, &7FC5
	out (c), c
	ld hl, VIDEOSEGMENT0000+100
	ld de, &5800
	ld bc, &2700
	ldir
	ld hl, &3800
	ld d, &51
	ld b, &07
	ldir
	call &5100
	ld b, &7F
	ld a, (&0000)
	ld c, a
	out (c), c
	ei
	ret
RelocateInitNewMusic_End:
	
NextPart:
	push hl
	
	ld a, (hl)
	push af
	inc	sp
	call _RunPart
	inc	sp
	
	pop hl
	inc hl
	
	ld a, (hl)	
	or a
	ret nz
	
	ld hl, FileOrderPtr+1
	ld (IncFilePtr+1), hl
	
	ld hl, RunPartList
	ret
__endasm;
}
