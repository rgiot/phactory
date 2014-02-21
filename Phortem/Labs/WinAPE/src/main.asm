
		nolist
		
		read "frameworkConst.asm"
		
		;org &2800 ; 
		org LINES_END ; angel		
		
		run EntryPoint		
EntryPoint:
		jp Start
		read "framework.asm"
		
Start:
		ld sp, &0080
		
		di
		ld hl, &C9FB
		ld ( &0038 ), hl
		ei
	
		call InitCRTC
		
		;read "transit\transit.asm" ;; USE org &2800		
		;read "rubber\rubber.asm" ;; USE org &2800	
		
		;read "intro\intro.asm"
		;read "angel\angel.asm"
		;read "batman\batman.asm"
		;read "bouncing\bouncing.asm" ; tube
		;read "bouncing2\bouncing2.asm" ; simple Y move to up
		;read "vertTransit\transit.asm" ; display vertical line for transit FX
		;read "horizTransit\transit.asm" ; display horizontal line for transit FX
		;read "costix\costix.asm" ; display Costix FX		 
		;read "cubeTransit\transit.asm" ; display cube to clear screen for transit FX
		;read "infiniteZoom\InfiniteZoom.asm" ; display Infinite Zoom FX
		;read "rectTransit\transit.asm" ; sinus rectangle Transit
		read "Fight\Fight.asm" ; display 3D Scroll
		;read "maskTile\MaskTile.asm" ; display mask tile
		;read "FireWorks\FireWorks.asm" ; display FireWorks FX		
		;read "doomTransit\Transit.asm" ; display Doom Transit (Costix)
		;read "doomTransit2\Transit.asm" ; display Doom Transit
		;read "kanceTransit\Transit.asm" ; display Kance Horizontal Transit
		;read "physics\Physics.asm" ; display Physics FX
		;read "horizTransit2\transit.asm" ; display horizontal transit to be used for Infinite PArt
		;read "circleTransit\Batman.asm" ; display Circle Transit (Beyond Infinity)
		;read "oblicTransit\Transit.asm" ; display Oblic Transit (Infinite Zoom background)
		;read "charonTransit\Transit.asm" ; display Charon Transit
		;read "vertTransit2\transit.asm" ; display vertical line for transit FX
		