				
		limit &BFFF
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
StartFX:
		
		call PhysicsInit
		call PhysicsLoop
		ret

; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"
		
; ----------------------------------------------------------------------------
PhysicsInit:
		call PhysicsCreateTables

		ld bc, &7F8C ; MODE 0
		out (c), c
		
		;call ClearScreen
		
		ret
		
; ----------------------------------------------------------------------------	
ClearScreen:
		ld hl, VIDEOSEGMENT0000
		ld ( hl ), 0
		ld de, VIDEOSEGMENT0000+1
		ld bc, VIDEOSEGMENT0000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENT4000
		ld ( hl ), 0
		ld de, VIDEOSEGMENT4000+1
		ld bc, VIDEOSEGMENT4000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENT8000
		ld ( hl ), 0
		ld de, VIDEOSEGMENT8000+1
		ld bc, VIDEOSEGMENT8000_SIZE-1
		ldir
		ld hl, VIDEOSEGMENTC000
		ld ( hl ), 0
		ld de, VIDEOSEGMENTC000+1
		ld bc, VIDEOSEGMENTC000_SIZE-1
		ldir
		ret

; ----------------------------------------------------------------------------		
PhysicsLoop:		
		di
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		call FlipScreen
		
		ld hl, IntroPalette
		call SetPalette
		
PhysicsFrameLoop:
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
		;db &ed, &ff
		ld a, (IsFlipped)
		call PhysicsDrawFX
		;db &ed, &ff
		
		call FlipScreen
		
		call WaitVBL
		
PhysicsFrameLoopCounter:
		ld hl, 76
		dec hl
		ld (PhysicsFrameLoopCounter+1), hl
		ld a, h
		or l
		jp nz, PhysicsFrameLoop
	
		jp StartFX
		
IntroPalette:
		incbin 	"Fight.cpcbitmap.palette"	
		
; ----------------------------------------------------------------------------
		;write "PhysicsFXCode.z80.bin"

		org &BC00
		limit &BFFF
		
		jp PhysicsCreateTables
		jp PhysicsDrawFX

		read "PhysicsTables.asm"
		read "PhysicsDrawFX.asm"
		
		;write "DeleteMe.bin"

; ----------------------------------------------------------------------------
		
write direct -1,-1, &C0
		;write "PhysicsFXData.z80.bin"
		org &2800
		limit &3FFF
		
		ALIGN 256
CircleFullData:
		incbin "BatmanCircleFull.bin"
CircleFullDataEnd:
FrameData:
		incbin 	"PhysicData.bin"
FrameDataEnd:
				
		;write "DeleteMe.bin"

; ----------------------------------------------------------------------------
ScreenTableFlip1:
		ds 335*6
ScreenTableFlip2:
		ds 335*6
ScreenTableOffset:
		ds 205+65+65+205+65+65
		
ClipLine:
		ds 98
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C7
		org &4000
		limit &7FFF
		incbin 	"FightBackground.bmp.bottomBin"
		
write direct -1,-1, &C4
		org &4080
		incbin 	"FightBackground.bmp.topBin"
		
		org &6800
		; EMPTY!
		
; ----------------------------------------------------------------------------
write direct -1,-1, &C0
		org &0080
		limit &FFFF
		incbin 	"FightBackground.bmp.topBin"
		org &4000
		limit &FFFF
		incbin 	"FightBackground.bmp.bottomBin"
		org &8080
		limit &FFFF
		incbin 	"FightBackground.bmp.topBin"
		org &C000
		limit &FFFF
		incbin 	"FightBackground.bmp.bottomBin"
		