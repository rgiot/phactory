				
		limit &BFFF

; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		call FireWorksInit
		
		call FireWorksLoop
		ret
		
; ----------------------------------------------------------------------------
FireWorksInit:
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ret

; ----------------------------------------------------------------------------		
FireWorksLoop:
		di
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		call &B800
		
		ld hl, VIDEOSEGMENT8000
		ld de, VIDEOSEGMENT0000
		ld bc, VIDEOSEGMENT8000_SIZE
		ldir
		
		ld ix, 0
		
FireWorksFrameLoop:
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a
		
		ld a, ScreenTablePtrC3/256
		call &B803		
		or a
		jr nz, Quit
		
		ld hl, &1000
		ld de, &1000
		ld bc, &100
		ldir
		
		call FlipScreen
		call WaitVBL
		
		ld a, ScreenTablePtrC1/256
		call &B803
		or a
		jr nz, Quit
		
		ld hl, &1000
		ld de, &1000
		ld bc, &100
		ldir
		
		call FlipScreen
		call WaitVBL
		
		jp FireWorksFrameLoop
		
; ----------------------------------------------------------------------------
Quit:
		jp Quit
		
; ----------------------------------------------------------------------------
		;write "FireWorksFXCode.bin"		

		org &B800
		limit &BFFF
		
		jp FireWorksCreateTables
		jp FireWorksDrawFX
		
		read "FireWorksTables.asm"
		read "FireWorksDrawFX.asm"
		
		;write "DeleteMe.bin"
		
IntroPalette:
		incbin 	"Fight.cpcbitmap.palette"	

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		limit &3FFF
		org &2800	
		
		align 256
ScreenTablePtrC1:
		ds 512
		
		;write "FireWorksFXData2.bin"
		
FrameBlockLeft1:
		incbin 	"FireWorks_FrameBlockLeft1.raw"
FrameBlockLeft1End:

		;write "DeleteMe.bin"
		
		write direct -1,-1, &C0
		limit &FFFF
		
		org &6800
ScreenTablePtrC3:
		ds 512
		
		;write "FireWorksFXData.bin"
				
FrameBlockRight:
		incbin 	"FireWorks_FrameBlockRight.raw"	
		
		;write "DeleteMe.bin"
			
; ----------------------------------------------------------------------------
		write direct -1,-1, &C0		
		limit &FFFF
		;org &0080
		;incbin 	"FightBackground.bmp.topBin"
		org &4000
		incbin 	"FightBackground.bmp.bottomBin"
		org &8080
		incbin 	"FightBackground.bmp.topBin"
		org &C000
		incbin 	"FightBackground.bmp.bottomBin"
		