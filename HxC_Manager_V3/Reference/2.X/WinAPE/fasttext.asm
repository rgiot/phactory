
	org &9A00
	nolist
	write "fasttext.bin"


;@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
;@                                                                            @
;@                   SymbOS SymShell Command Line Interface                   @
;@                                                                            @
;@                      F A S T   T E X T   O U T P U T                       @
;@                                                                            @
;@                   (c)oded 2005 by Prodatron / SymbiosiS                    @
;@                                                                            @
;@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

;### MFTOUT -> Plots up to 256 chars in Mode2 starting from a specified screen address
;###           Chars 0-31 and 128-255 won't be plotted, but you can add the single char-routines by yourself
;### Input      IX=text, IYL=number of chars, DE=screen address
;### Destroyed  AF,BC,DE,HL,IX,IYL
mftout  
mftout1 ld a,(ix+0)     
        or a
	ret z
	ld l, a
	xor a
	inc ix          ;3
        ld h,0          
        add hl,hl       ;3
        ld bc,mftchrtab ;3
        add hl,bc       ;3
        ld c,(hl)       ;2
        inc hl          ;2
        ld h,(hl)       ;2
        ld l,c          ;1
        ld bc,#800      ;3
        jp (hl)         ;1 29 + 41-49
mftout2 ld bc,-7*#800+1 ;3
        add hl,bc       ;3
        res 3,h         ;2
        ex de,hl        ;1
        jr mftout1   ;3 14 -> 84-92 NOPs / char
        ret

mftchrtab
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr033,mftchr034,mftchr035,mftchr036,mftchr037,mftchr038,mftchr039
dw mftchr040,mftchr041,mftchr042,mftchr043,mftchr044,mftchr045,mftchr046,mftchr047
dw mftchr048,mftchr049,mftchr050,mftchr051,mftchr052,mftchr053,mftchr054,mftchr055
dw mftchr056,mftchr057,mftchr058,mftchr059,mftchr060,mftchr061,mftchr062,mftchr063
dw mftchr064,mftchr065,mftchr066,mftchr067,mftchr068,mftchr069,mftchr070,mftchr071
dw mftchr072,mftchr073,mftchr074,mftchr075,mftchr076,mftchr077,mftchr078,mftchr079
dw mftchr080,mftchr081,mftchr082,mftchr083,mftchr084,mftchr085,mftchr086,mftchr087
dw mftchr088,mftchr089,mftchr090,mftchr091,mftchr092,mftchr093,mftchr094,mftchr095
dw mftchr096,mftchr097,mftchr098,mftchr099,mftchr100,mftchr101,mftchr102,mftchr103
dw mftchr104,mftchr105,mftchr106,mftchr107,mftchr108,mftchr109,mftchr110,mftchr111
dw mftchr112,mftchr113,mftchr114,mftchr115,mftchr116,mftchr117,mftchr118,mftchr119
dw mftchr120,mftchr121,mftchr122,mftchr123,mftchr124,mftchr125,mftchr126,mftchr127
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032
dw mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032,mftchr032


mftchr032 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ; 
mftchr033 ex de,hl:ld (hl),#18:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :jp mftout2 ;!
mftchr034 ex de,hl:ld (hl),#6C:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#28:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;"
mftchr035 ex de,hl:ld (hl),#6C:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),a  :jp mftout2 ;#
mftchr036 ex de,hl:ld (hl),#18:add hl,bc:ld (hl),#3E:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :jp mftout2 ;$
mftchr037 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :jp mftout2 ;%
mftchr038 ex de,hl:ld (hl),#38:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),#76:add hl,bc:ld (hl),#DC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#76:add hl,bc:ld (hl),a  :jp mftout2 ;&
mftchr039 ex de,hl:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;'
mftchr040 ex de,hl:ld (hl),#0C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),a  :jp mftout2 ;(
mftchr041 ex de,hl:ld (hl),#30:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),a  :jp mftout2 ;)
mftchr042 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#FF:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;*
mftchr043 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;+
mftchr044 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:jp mftout2 ;,
mftchr045 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;-
mftchr046 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :jp mftout2 ;.
mftchr047 ex de,hl:ld (hl),#06:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#80:add hl,bc:ld (hl),a  :jp mftout2 ;/
mftchr048 ex de,hl:ld (hl),#38:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),a  :jp mftout2 ;0
mftchr049 ex de,hl:ld (hl),#18:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),a  :jp mftout2 ;1
mftchr050 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#1C:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),a  :jp mftout2 ;2
mftchr051 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;3
mftchr052 ex de,hl:ld (hl),#1C:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#1E:add hl,bc:ld (hl),a  :jp mftout2 ;4
mftchr053 ex de,hl:ld (hl),#FE:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#FC:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;5
mftchr054 ex de,hl:ld (hl),#38:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#FC:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;6
mftchr055 ex de,hl:ld (hl),#FE:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),a  :jp mftout2 ;7
mftchr056 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;8
mftchr057 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;9
mftchr058 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :jp mftout2 ;:
mftchr059 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:jp mftout2 ;;
mftchr060 ex de,hl:ld (hl),#0C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),a  :jp mftout2 ;<
mftchr061 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;=
mftchr062 ex de,hl:ld (hl),#60:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),a  :jp mftout2 ;>
mftchr063 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :jp mftout2 ;?
mftchr064 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#DE:add hl,bc:ld (hl),#DE:add hl,bc:ld (hl),#DE:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#78:add hl,bc:ld (hl),a  :jp mftout2 ;@
mftchr065 ex de,hl:ld (hl),#38:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :jp mftout2 ;A
mftchr066 ex de,hl:ld (hl),#FC:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#FC:add hl,bc:ld (hl),a  :jp mftout2 ;B
mftchr067 ex de,hl:ld (hl),#3C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;C
mftchr068 ex de,hl:ld (hl),#F8:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#F8:add hl,bc:ld (hl),a  :jp mftout2 ;D
mftchr069 ex de,hl:ld (hl),#FE:add hl,bc:ld (hl),#62:add hl,bc:ld (hl),#68:add hl,bc:ld (hl),#78:add hl,bc:ld (hl),#68:add hl,bc:ld (hl),#62:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),a  :jp mftout2 ;E
mftchr070 ex de,hl:ld (hl),#FE:add hl,bc:ld (hl),#62:add hl,bc:ld (hl),#68:add hl,bc:ld (hl),#78:add hl,bc:ld (hl),#68:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#F0:add hl,bc:ld (hl),a  :jp mftout2 ;F
mftchr071 ex de,hl:ld (hl),#3C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#CE:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#3A:add hl,bc:ld (hl),a  :jp mftout2 ;G
mftchr072 ex de,hl:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :jp mftout2 ;H
mftchr073 ex de,hl:ld (hl),#3C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;I
mftchr074 ex de,hl:ld (hl),#1E:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#78:add hl,bc:ld (hl),a  :jp mftout2 ;J
mftchr075 ex de,hl:ld (hl),#E6:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#78:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#E6:add hl,bc:ld (hl),a  :jp mftout2 ;K
mftchr076 ex de,hl:ld (hl),#F0:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#62:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),a  :jp mftout2 ;L
mftchr077 ex de,hl:ld (hl),#C6:add hl,bc:ld (hl),#EE:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :jp mftout2 ;M
mftchr078 ex de,hl:ld (hl),#C6:add hl,bc:ld (hl),#E6:add hl,bc:ld (hl),#F6:add hl,bc:ld (hl),#DE:add hl,bc:ld (hl),#CE:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :jp mftout2 ;N
mftchr079 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;O
mftchr080 ex de,hl:ld (hl),#FC:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#F0:add hl,bc:ld (hl),a  :jp mftout2 ;P
mftchr081 ex de,hl:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#CE:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#0E:jp mftout2 ;Q
mftchr082 ex de,hl:ld (hl),#FC:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#E6:add hl,bc:ld (hl),a  :jp mftout2 ;R
mftchr083 ex de,hl:ld (hl),#3C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;S
mftchr084 ex de,hl:ld (hl),#7E:add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),#5A:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;T
mftchr085 ex de,hl:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;U
mftchr086 ex de,hl:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),a  :jp mftout2 ;V
mftchr087 ex de,hl:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),a  :jp mftout2 ;W
mftchr088 ex de,hl:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :jp mftout2 ;X
mftchr089 ex de,hl:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;Y
mftchr090 ex de,hl:ld (hl),#FE:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#8C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#32:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),a  :jp mftout2 ;Z
mftchr091 ex de,hl:ld (hl),#3C:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;[
mftchr092 ex de,hl:ld (hl),#C0:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#02:add hl,bc:ld (hl),a  :jp mftout2 ;\
mftchr093 ex de,hl:ld (hl),#3C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;]
mftchr094 ex de,hl:ld (hl),#10:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;^
mftchr095 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#FF:jp mftout2 ;_
mftchr096 ex de,hl:ld (hl),#30:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;`
mftchr097 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#78:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#76:add hl,bc:ld (hl),a  :jp mftout2 ;a
mftchr098 ex de,hl:ld (hl),#E0:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#DC:add hl,bc:ld (hl),a  :jp mftout2 ;b
mftchr099 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;c
mftchr100 ex de,hl:ld (hl),#1C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#76:add hl,bc:ld (hl),a  :jp mftout2 ;d
mftchr101 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;e
mftchr102 ex de,hl:ld (hl),#3C:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#F8:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#F8:add hl,bc:ld (hl),a  :jp mftout2 ;f
mftchr103 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#76:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#F8:jp mftout2 ;g
mftchr104 ex de,hl:ld (hl),#E0:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#76:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#E6:add hl,bc:ld (hl),a  :jp mftout2 ;h
mftchr105 ex de,hl:ld (hl),#18:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#38:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;i
mftchr106 ex de,hl:ld (hl),#06:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#3C:jp mftout2 ;j
mftchr107 ex de,hl:ld (hl),#E0:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#78:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#E6:add hl,bc:ld (hl),a  :jp mftout2 ;k
mftchr108 ex de,hl:ld (hl),#38:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#3C:add hl,bc:ld (hl),a  :jp mftout2 ;l
mftchr109 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#EC:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),a  :jp mftout2 ;m
mftchr110 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#DC:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),a  :jp mftout2 ;n
mftchr111 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),a  :jp mftout2 ;o
mftchr112 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#DC:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#66:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#F0:jp mftout2 ;p
mftchr113 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#76:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#0C:add hl,bc:ld (hl),#1E:jp mftout2 ;q
mftchr114 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#DC:add hl,bc:ld (hl),#76:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#60:add hl,bc:ld (hl),#F0:add hl,bc:ld (hl),a  :jp mftout2 ;r
mftchr115 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),#C0:add hl,bc:ld (hl),#7C:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#FC:add hl,bc:ld (hl),a  :jp mftout2 ;s
mftchr116 ex de,hl:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#FC:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#30:add hl,bc:ld (hl),#36:add hl,bc:ld (hl),#1C:add hl,bc:ld (hl),a  :jp mftout2 ;t
mftchr117 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#76:add hl,bc:ld (hl),a  :jp mftout2 ;u
mftchr118 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),a  :jp mftout2 ;v
mftchr119 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#D6:add hl,bc:ld (hl),#FE:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),a  :jp mftout2 ;w
mftchr120 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#38:add hl,bc:ld (hl),#6C:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),a  :jp mftout2 ;x
mftchr121 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#C6:add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),#06:add hl,bc:ld (hl),#FC:jp mftout2 ;y
mftchr122 ex de,hl:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),#4C:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#32:add hl,bc:ld (hl),#7E:add hl,bc:ld (hl),a  :jp mftout2 ;z
mftchr123 ex de,hl:ld (hl),#0E:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#70:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0E:add hl,bc:ld (hl),a  :jp mftout2 ;{
mftchr124 ex de,hl:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),a  :jp mftout2 ;|
mftchr125 ex de,hl:ld (hl),#70:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#0E:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#18:add hl,bc:ld (hl),#70:add hl,bc:ld (hl),a  :jp mftout2 ;}
mftchr126 ex de,hl:ld (hl),#76:add hl,bc:ld (hl),#DC:add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :add hl,bc:ld (hl),a  :jp mftout2 ;~
mftchr127 ex de,hl:ld (hl),#CC:add hl,bc:ld (hl),#33:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#33:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#33:add hl,bc:ld (hl),#CC:add hl,bc:ld (hl),#33:jp mftout2 ; 
