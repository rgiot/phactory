				
		limit &BFFF

; ----------------------------------------------------------------------------
SCR_BLOCKSWIDTH_DIV2	equ 17 ; SCR_BLOCKSWIDTH/2
		
; ----------------------------------------------------------------------------
		ld a, &C0
		ld ( &0000 ), a
		ld bc, &7FC0
		out (c), c
		
		;all InitLines
		
		call FightInit
		
		ld hl, VIDEOSEGMENT0000
		ld de, VIDEOSEGMENT8000
		ld bc, VIDEOSEGMENT0000_SIZE
		ldir
		
		ld hl, VIDEOSEGMENT4000
		ld de, VIDEOSEGMENTC000
		ld bc, VIDEOSEGMENT4000_SIZE
		ldir
		
		call FightLoop
		ret
		
; ----------------------------------------------------------------------------
		read "../frameworkTiles.asm"

; ----------------------------------------------------------------------------
FightInit:
		ld bc, &7F8C ; MODE 0
		out (c), c
		
		ld hl, IntroPalette
		call SetPalette
		
		ret

; ----------------------------------------------------------------------------		
FightCRTC:
		db 2, 50
		db 0
		
; ----------------------------------------------------------------------------		
FightLoop:
		ld hl, FightCRTC
		call InitCRTC1
		
		di
		ld bc, &7FC0
		out (c), c
		ld a, c
		ld ( &0000 ), a
		ei
		
		call &B750
		
FightFrameLoop:
		
		ld bc, &7F00+16
		out (c), c
		ld a, &54	
		out (c), a

		ld a, 1
		call &B753
		
		call FlipScreen
		
		xor a
		call &B753
		
		call FlipScreen
		
FightFrameLoopCounter:
		ld hl, 162/2
		dec hl
		ld (FightFrameLoopCounter+1), hl
		ld a, h
		or l
		jp nz, FightFrameLoop
		;jp FightFrameLoop
		
		call FlipScreen
		
InfiniteLoop:
		jp InfiniteLoop
		
; ----------------------------------------------------------------------------
		;write "FightFXCode_B800.bin"		

		org &B750
		limit &BFFF
		
		jp FightCreateTables
		jp FightDrawFX
		
		read "FightTables.asm"
		read "FightDrawFX.asm"
		
		;write "DeleteMe.bin"
		
IntroPalette:
		incbin 	"Fight.cpcbitmap.palette"	

; ----------------------------------------------------------------------------
		write direct -1,-1, &C0		
		limit &FFFF
		org &0080
		incbin 	"FightBackground.bmp.topBin"
		org &4000
		incbin 	"FightBackground.bmp.bottomBin"
		
; ----------------------------------------------------------------------------
		write direct -1,-1, &C0
		org &6800
		limit &7FFF
		
		;write "FightFXDATA_C0_6800.bin"		
		
ScrollTablePtr:
		dw ScrollTable
ScrollTable:
		db &C0 : dw Scroll01
		db &C0 : dw Scroll02
		db &C0 : dw Scroll03
		db &C0 : dw Scroll04
		db &C0 : dw Scroll05
		db &C0 : dw Scroll06
		db &C0 : dw Scroll07
		db &C0 : dw Scroll08
		db &C0 : dw Scroll09
		db &C0 : dw Scroll10
		db &C0 : dw Scroll11
		db &C0 : dw Scroll12
		db &C0 : dw Scroll13
		db &C0 : dw Scroll14
		db &C0 : dw Scroll15
		db &C0 : dw Scroll16
		db &C0 : dw Scroll17
		db &C0 : dw Scroll18
		db &C0 : dw Scroll19
		db &C0 : dw Scroll20
		db &C0 : dw Scroll21
		db &C0 : dw Scroll22
		db &C0 : dw Scroll23
		db &C0 : dw Scroll24
		db &C0 : dw Scroll25
		db &C0 : dw Scroll26
		db &C0 : dw Scroll27
		db &C0 : dw Scroll28
		db &C0 : dw Scroll29
		db &C0 : dw Scroll30
		db &C0 : dw Scroll31
		db &C0 : dw Scroll32
		db &C0 : dw Scroll33
		db &C0 : dw Scroll34
		db &C0 : dw Scroll35
		db &C0 : dw Scroll36
		db &C0 : dw Scroll37
		db &C0 : dw Scroll38
		db &C0 : dw Scroll39
		db &C0 : dw Scroll40
		db &C0 : dw Scroll41
		db &C0 : dw Scroll42
		db &C0 : dw Scroll43
		db &C0 : dw Scroll44
		db &C0 : dw Scroll45
		db &C0 : dw Scroll46
		db &C0 : dw Scroll47
		db &C0 : dw Scroll48
		db &C0 : dw Scroll49
		db &C0 : dw Scroll50
		db &C0 : dw Scroll51
		db &C0 : dw Scroll52
		db &C0 : dw Scroll53
		db &C0 : dw Scroll54
		db &C0 : dw Scroll55
		db &C0 : dw Scroll56
		db &C0 : dw Scroll57
		db &C0 : dw Scroll58
		db &C0 : dw Scroll59
		db &C0 : dw Scroll60
		db &C0 : dw Scroll61
		db &C0 : dw Scroll62
		db &C0 : dw Scroll63
		db &C0 : dw Scroll64
		db &C0 : dw Scroll65
		db &C0 : dw Scroll66
		db &C0 : dw Scroll67
		db &C0 : dw Scroll68
		db &C0 : dw Scroll69
		db &C0 : dw Scroll70
		db &C0 : dw Scroll71
		db &C0 : dw Scroll72
		db &C0 : dw Scroll73
		db &C0 : dw Scroll74
		db &C0 : dw Scroll75
		db &C0 : dw Scroll76
		db &C0 : dw Scroll77
		db &C0 : dw Scroll78
		db &C0 : dw Scroll79
		db &C0 : dw Scroll80
		db &C0 : dw Scroll81
		db &C0 : dw Scroll82
		db &C0 : dw Scroll83
		db &C0 : dw Scroll84
		db &C0 : dw Scroll85
		db &C0 : dw Scroll86
		db &C0 : dw Scroll87
		db &C0 : dw Scroll88
		db &C0 : dw Scroll89
		db &C0 : dw Scroll90
		db &C0 : dw Scroll91
		db &C0 : dw Scroll92
		db &C0 : dw Scroll93
		db &C0 : dw Scroll94
		db &C0 : dw Scroll95
		db &C0 : dw Scroll96
		db &C0 : dw Scroll97
		db &C0 : dw Scroll98
		db &C0 : dw Scroll99
		db &C0 : dw Scroll100
		db &C0 : dw Scroll101
		db &C0 : dw Scroll102
		db &C0 : dw Scroll103
		db &C0 : dw Scroll104
		db &C0 : dw Scroll105
		db &C0 : dw Scroll106
		db &C0 : dw Scroll107
		db &C4 : dw Scroll108
		db &C4 : dw Scroll109
		db &C4 : dw Scroll110
		db &C4 : dw Scroll111
		db &C4 : dw Scroll112
		db &C4 : dw Scroll113
		db &C4 : dw Scroll114
		db &C4 : dw Scroll115
		db &C4 : dw Scroll116
		db &C4 : dw Scroll117
		db &C4 : dw Scroll118
		db &C4 : dw Scroll119
		db &C4 : dw Scroll120
		db &C4 : dw Scroll121
		db &C4 : dw Scroll122
		db &C4 : dw Scroll123
		db &C4 : dw Scroll124
		db &C4 : dw Scroll125
		db &C4 : dw Scroll126
		db &C4 : dw Scroll127
		db &C4 : dw Scroll128
		db &C4 : dw Scroll129
		db &C4 : dw Scroll130
		db &C4 : dw Scroll131
		db &C4 : dw Scroll132
		db &C4 : dw Scroll133
		db &C4 : dw Scroll134
		db &C4 : dw Scroll135
		db &C4 : dw Scroll136
		db &C4 : dw Scroll137
		db &C4 : dw Scroll138
		db &C4 : dw Scroll139
		db &C4 : dw Scroll140
		db &C4 : dw Scroll141
		db &C4 : dw Scroll142
		db &C4 : dw Scroll143
		db &C4 : dw Scroll144
		db &C4 : dw Scroll145
		db &C4 : dw Scroll146
		db &C4 : dw Scroll147
		db &C4 : dw Scroll148
		db &C4 : dw Scroll149
		db &C4 : dw Scroll150		
		db &C4 : dw Scroll151	
		db &C4 : dw Scroll152	
		db &C4 : dw Scroll153	
		db &C4 : dw Scroll154	
		db &C4 : dw Scroll155	
		db &C4 : dw Scroll156	
		db &C4 : dw Scroll157	
		db &C4 : dw Scroll158	
		db &C4 : dw Scroll159	
		db &C4 : dw Scroll160	
		db &C4 : dw Scroll161	
		db &C4 : dw Scroll162
		db &C4 : dw Scroll162	
		db 0
		
Scroll01:
		incbin 	"GL3DScroll01.bin.pck"
Scroll02:
		incbin 	"GL3DScroll02.bin.pck"
Scroll03:
		incbin 	"GL3DScroll03.bin.pck"
Scroll04:
		incbin 	"GL3DScroll04.bin.pck"
Scroll05:
		incbin 	"GL3DScroll05.bin.pck"
Scroll06:
		incbin 	"GL3DScroll06.bin.pck"
Scroll07:
		incbin 	"GL3DScroll07.bin.pck"
Scroll08:
		incbin 	"GL3DScroll08.bin.pck"
Scroll09:
		incbin 	"GL3DScroll09.bin.pck"
Scroll10:
		incbin 	"GL3DScroll10.bin.pck"
Scroll11:
		incbin 	"GL3DScroll11.bin.pck"
Scroll12:
		incbin 	"GL3DScroll12.bin.pck"
Scroll13:
		incbin 	"GL3DScroll13.bin.pck"
Scroll14:
		incbin 	"GL3DScroll14.bin.pck"
Scroll15:
		incbin 	"GL3DScroll15.bin.pck"
Scroll16:
		incbin 	"GL3DScroll16.bin.pck"
Scroll17:
		incbin 	"GL3DScroll17.bin.pck"
Scroll18:
		incbin 	"GL3DScroll18.bin.pck"
Scroll19:
		incbin 	"GL3DScroll19.bin.pck"
Scroll20:
		incbin 	"GL3DScroll20.bin.pck"
Scroll21:
		incbin 	"GL3DScroll21.bin.pck"
Scroll22:
		incbin 	"GL3DScroll22.bin.pck"
Scroll23:
		incbin 	"GL3DScroll23.bin.pck"
Scroll24:
		incbin 	"GL3DScroll24.bin.pck"
Scroll25:
		incbin 	"GL3DScroll25.bin.pck"
Scroll26:
		incbin 	"GL3DScroll26.bin.pck"
Scroll27:
		incbin 	"GL3DScroll27.bin.pck"
Scroll28:
		incbin 	"GL3DScroll28.bin.pck"
Scroll29:
		incbin 	"GL3DScroll29.bin.pck"
Scroll30:
		incbin 	"GL3DScroll30.bin.pck"
Scroll31:
		incbin 	"GL3DScroll31.bin.pck"
Scroll32:
		incbin 	"GL3DScroll32.bin.pck"
Scroll33:
		incbin 	"GL3DScroll33.bin.pck"
Scroll34:
		incbin 	"GL3DScroll34.bin.pck"
Scroll35:
		incbin 	"GL3DScroll35.bin.pck"
Scroll36:
		incbin 	"GL3DScroll36.bin.pck"
Scroll37:
		incbin 	"GL3DScroll37.bin.pck"
Scroll38:
		incbin 	"GL3DScroll38.bin.pck"
Scroll39:
		incbin 	"GL3DScroll39.bin.pck"
Scroll40:
		incbin 	"GL3DScroll40.bin.pck"
Scroll41:
		incbin 	"GL3DScroll41.bin.pck"
Scroll42:
		incbin 	"GL3DScroll42.bin.pck"
Scroll43:
		incbin 	"GL3DScroll43.bin.pck"
Scroll44:
		incbin 	"GL3DScroll44.bin.pck"
Scroll45:
		incbin 	"GL3DScroll45.bin.pck"
Scroll46:
		incbin 	"GL3DScroll46.bin.pck"
Scroll47:
		incbin 	"GL3DScroll47.bin.pck"
Scroll48:
		incbin 	"GL3DScroll48.bin.pck"
Scroll49:
		incbin 	"GL3DScroll49.bin.pck"
Scroll50:
		incbin 	"GL3DScroll50.bin.pck"
Scroll51:
		incbin 	"GL3DScroll51.bin.pck"		
Scroll52:
		incbin 	"GL3DScroll52.bin.pck"
Scroll53:
		incbin 	"GL3DScroll53.bin.pck"		
Scroll54:
		incbin 	"GL3DScroll54.bin.pck"
Scroll55:
		incbin 	"GL3DScroll55.bin.pck"
Scroll56:
		incbin 	"GL3DScroll56.bin.pck"
Scroll57:
		incbin 	"GL3DScroll57.bin.pck"
Scroll58:
		incbin 	"GL3DScroll58.bin.pck"
		
		;write "DeleteMe.bin"	
		
		org &E800
		limit &FFA0
		
		;write "FightFXDATA_C0_E800.bin"	
Scroll59:
		incbin 	"GL3DScroll59.bin.pck"
Scroll60:
		incbin 	"GL3DScroll60.bin.pck"
Scroll61:
		incbin 	"GL3DScroll61.bin.pck"
Scroll62:
		incbin 	"GL3DScroll62.bin.pck"
Scroll63:
		incbin 	"GL3DScroll63.bin.pck"		
Scroll64:
		incbin 	"GL3DScroll64.bin.pck"
Scroll65:
		incbin 	"GL3DScroll65.bin.pck"
Scroll66:
		incbin 	"GL3DScroll66.bin.pck"		
Scroll67:
		incbin 	"GL3DScroll67.bin.pck"
Scroll68:
		incbin 	"GL3DScroll68.bin.pck"		
Scroll69:
		incbin 	"GL3DScroll69.bin.pck"
Scroll70:
		incbin 	"GL3DScroll70.bin.pck"
Scroll71:
		incbin 	"GL3DScroll71.bin.pck"
Scroll72:
		incbin 	"GL3DScroll72.bin.pck"
Scroll73:
		incbin 	"GL3DScroll73.bin.pck"
Scroll74:
		incbin 	"GL3DScroll74.bin.pck"
Scroll75:
		incbin 	"GL3DScroll75.bin.pck"
Scroll76:
		incbin 	"GL3DScroll76.bin.pck"
Scroll77:
		incbin 	"GL3DScroll77.bin.pck"
Scroll78:
		incbin 	"GL3DScroll78.bin.pck"
Scroll79:
		incbin 	"GL3DScroll79.bin.pck"
Scroll80:
		incbin 	"GL3DScroll80.bin.pck"
Scroll81:
		incbin 	"GL3DScroll81.bin.pck"
Scroll82:
		incbin 	"GL3DScroll82.bin.pck"
Scroll83:
		incbin 	"GL3DScroll83.bin.pck"
Scroll84:
		incbin 	"GL3DScroll84.bin.pck"
Scroll85:
		incbin 	"GL3DScroll85.bin.pck"
Scroll86:
		incbin 	"GL3DScroll86.bin.pck"
Scroll87:
		incbin 	"GL3DScroll87.bin.pck"
Scroll88:
		incbin 	"GL3DScroll88.bin.pck"
Scroll89:
		incbin 	"GL3DScroll89.bin.pck"
Scroll90:
		incbin 	"GL3DScroll90.bin.pck"
Scroll91:
		incbin 	"GL3DScroll91.bin.pck"
Scroll92:
		incbin 	"GL3DScroll92.bin.pck"
Scroll93:
		incbin 	"GL3DScroll93.bin.pck"
Scroll94:
		incbin 	"GL3DScroll94.bin.pck"
Scroll95:
		incbin 	"GL3DScroll95.bin.pck"
Scroll96:
		incbin 	"GL3DScroll96.bin.pck"
Scroll97:
		incbin 	"GL3DScroll97.bin.pck"
Scroll98:
		incbin 	"GL3DScroll98.bin.pck"
Scroll99:
		incbin 	"GL3DScroll99.bin.pck"
Scroll100:
		incbin 	"GL3DScroll100.bin.pck"		
Scroll101:
		incbin 	"GL3DScroll101.bin.pck"			
Scroll102:
		incbin 	"GL3DScroll102.bin.pck"		
Scroll103:
		incbin 	"GL3DScroll103.bin.pck"		
Scroll104:
		incbin 	"GL3DScroll104.bin.pck"		
Scroll105:
		incbin 	"GL3DScroll105.bin.pck"		
Scroll106:
		incbin 	"GL3DScroll106.bin.pck"		
Scroll107:
		incbin 	"GL3DScroll107.bin.pck"	
		
		;write "DeleteMe.bin"	
		
		write direct -1,-1, &C4
		org &6800
		limit &7FFF
		
		;write "FightFXDATA_C4_6800.bin"		
Scroll108:
		incbin 	"GL3DScroll108.bin.pck"				
Scroll109:
		incbin 	"GL3DScroll109.bin.pck"	
Scroll110:
		incbin 	"GL3DScroll110.bin.pck"			
Scroll111:
		incbin 	"GL3DScroll111.bin.pck"		
Scroll112:
		incbin 	"GL3DScroll112.bin.pck"		
Scroll113:
		incbin 	"GL3DScroll113.bin.pck"			
Scroll114:
		incbin 	"GL3DScroll114.bin.pck"		
Scroll115:
		incbin 	"GL3DScroll115.bin.pck"		
Scroll116:
		incbin 	"GL3DScroll116.bin.pck"			
Scroll117:
		incbin 	"GL3DScroll117.bin.pck"		
Scroll118:
		incbin 	"GL3DScroll118.bin.pck"		
Scroll119:
		incbin 	"GL3DScroll119.bin.pck"			
Scroll120:
		incbin 	"GL3DScroll120.bin.pck"		
Scroll121:
		incbin 	"GL3DScroll121.bin.pck"			
Scroll122:
		incbin 	"GL3DScroll122.bin.pck"		
Scroll123:
		incbin 	"GL3DScroll123.bin.pck"		
Scroll124:
		incbin 	"GL3DScroll124.bin.pck"		
Scroll125:
		incbin 	"GL3DScroll125.bin.pck"		
Scroll126:
		incbin 	"GL3DScroll126.bin.pck"		
Scroll127:
		incbin 	"GL3DScroll127.bin.pck"		
Scroll128:
		incbin 	"GL3DScroll128.bin.pck"		
Scroll129:
		incbin 	"GL3DScroll129.bin.pck"			
Scroll130:
		incbin 	"GL3DScroll130.bin.pck"		
Scroll131:
		incbin 	"GL3DScroll131.bin.pck"			
Scroll132:
		incbin 	"GL3DScroll132.bin.pck"			
Scroll133:
		incbin 	"GL3DScroll133.bin.pck"			
Scroll134:
		incbin 	"GL3DScroll134.bin.pck"			
Scroll135:
		incbin 	"GL3DScroll135.bin.pck"			
Scroll136:
		incbin 	"GL3DScroll136.bin.pck"			
Scroll137:
		incbin 	"GL3DScroll137.bin.pck"			
Scroll138:
		incbin 	"GL3DScroll138.bin.pck"			
Scroll139:
		incbin 	"GL3DScroll139.bin.pck"			
Scroll140:
		incbin 	"GL3DScroll140.bin.pck"			
Scroll141:
		incbin 	"GL3DScroll141.bin.pck"			
Scroll142:
		incbin 	"GL3DScroll142.bin.pck"			
Scroll143:
		incbin 	"GL3DScroll143.bin.pck"			
Scroll144:
		incbin 	"GL3DScroll144.bin.pck"			
Scroll145:
		incbin 	"GL3DScroll145.bin.pck"			
Scroll146:
		incbin 	"GL3DScroll146.bin.pck"			
Scroll147:
		incbin 	"GL3DScroll147.bin.pck"			
Scroll148:
		incbin 	"GL3DScroll148.bin.pck"			
Scroll149:
		incbin 	"GL3DScroll149.bin.pck"			
Scroll150:
		incbin 	"GL3DScroll150.bin.pck"			
Scroll151:
		incbin 	"GL3DScroll151.bin.pck"			
Scroll152:
		incbin 	"GL3DScroll152.bin.pck"			
Scroll153:
		incbin 	"GL3DScroll153.bin.pck"			
Scroll154:
		incbin 	"GL3DScroll154.bin.pck"			
Scroll155:
		incbin 	"GL3DScroll155.bin.pck"			
Scroll156:
		incbin 	"GL3DScroll156.bin.pck"			
Scroll157:
		incbin 	"GL3DScroll157.bin.pck"			
Scroll158:
		incbin 	"GL3DScroll158.bin.pck"			
Scroll159:
		incbin 	"GL3DScroll159.bin.pck"			
Scroll160:
		incbin 	"GL3DScroll160.bin.pck"			
Scroll161:
		incbin 	"GL3DScroll161.bin.pck"			
Scroll162:
		incbin 	"GL3DScroll162.bin.pck"	
	
	;write "DeleteMe.bin"	
		
; ----------------------------------------------------------------------------
	org &3800
	
	;write "FightFXCode_3800.bin"
	
	ALIGN 256
Degrade2:
	incbin 	"FightDegrade.bmp.sprRawData1"
	
	ALIGN 256
SinX1:
	incbin 	"GL3DScrollSinX1.bin"
	ALIGN 256
SinX2:
	incbin 	"GL3DScrollSinX2.bin"
	
	ALIGN 256
Degrade:
	incbin 	"FightDegrade2.bmp.sprRawData1"
	
	;write "DeleteMe.bin"

; ----------------------------------------------------------------------------
ScreenTable1 equ &2D72
	;ds 41*6*5

; ----------------------------------------------------------------------------
ScreenTable2 equ &3272
	;ds 41*6*5
		
; ----------------------------------------------------------------------------
		write direct -1,-1, &C4
		org &4000
		limit &FFFF	
		incbin 	"FightBackground.bmp.topBin"
		write direct -1,-1, &C7
		org &4000
		incbin 	"FightBackground.bmp.bottomBin"
	
	