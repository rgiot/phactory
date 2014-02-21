void initGfxLib() {
__asm
        JP InitGfxLib

TabAdrEcr:
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0000, &0000, &0000, &0000, &4040, &4040, &4040, &4040
        DW      &8080, &8080, &8080, &8080, &C0C0, &C0C0, &C0C0, &C0C0
        DW      &0800, &1810, &2820, &3830, &0800, &1810, &2820, &3830
        DW      &0800, &1810, &2820, &3830, &0800, &1810, &2820, &3830
        DW      &0901, &1911, &2921, &3931, &0901, &1911, &2921, &3931
        DW      &0901, &1911, &2921, &3931, &0901, &1911, &2921, &3931
        DW      &0A02, &1A12, &2A22, &3A32, &0A02, &1A12, &2A22, &3A32
        DW      &0A02, &1A12, &2A22, &3A32, &0A02, &1A12, &2A22, &3A32
        DW      &0B03, &1B13, &2B23, &3B33, &0B03, &1B13, &2B23, &3B33
        DW      &0B03, &1B13, &2B23, &3B33, &0B03, &1B13, &2B23, &3B33
        DW      &0C04, &1C14, &2C24, &3C34, &0C04, &1C14, &2C24, &3C34
        DW      &0C04, &1C14, &2C24, &3C34, &0C04, &1C14, &2C24, &3C34
        DW      &0D05, &1D15, &2D25, &3D35, &0D05, &1D15, &2D25, &3D35
        DW      &0D05, &1D15, &2D25, &3D35, &0D05, &1D15, &2D25, &3D35
        DW      &0E06, &1E16, &2E26, &3E36, &0E06, &1E16, &2E26, &3E36
        DW      &0E06, &1E16, &2E26, &3E36, &0E06, &1E16, &2E26, &3E36
        DW      &0F07, &1F17, &2F27, &3F37, &0F07, &1F17, &2F27, &3F37
        DW      &0F07, &1F17, &2F27, &3F37, &0F07, &1F17, &2F27, &3F37

TabPen0:
        DB      &77, &00, &BB, &00, &DD, &00, &EE, &00
        DB      &77, &80, &BB, &40, &DD, &20, &EE, &10
        DB      &77, &08, &BB, &04, &DD, &02, &EE, &01
        DB      &77, &88, &BB, &44, &DD, &22, &EE, &11
       
TabPen:
        DS      2

InitGfxLib:
        LD      BC,&BC01
        OUT     (C),C
        LD      BC,&BD20                ; CRTC reg1 = 32 (256 pixels width)
        OUT  (C),C
        LD      BC,&BC02
        OUT     (C),C
        LD      BC,&BD2A                ; Center the screen horizontally
        OUT     (C),C
        LD      BC,&BC06
        OUT     (C),C
        LD      BC,&BD20                ; CRTC reg6 = 32 (256 pixels height)
        OUT     (C),C
        LD      BC,&BC07
        OUT     (C),C
        LD      BC,&BD22                ; Center the screen vertically
        OUT     (C),C

__endasm;
}

void setColor(unsigned char color) {
color;
__asm
        LD      A,(IX+4)
        AND     3
        ADD     A,A
        ADD     A,A
        ADD     A,A
        LD      H,TabPen0/256
        LD      L,A
        LD      (TabPen),HL
__endasm;
}

void drawLine(unsigned short x1, unsigned short y1, unsigned short x2, unsigned short y2) {
x1;x2;y1;y2;
__asm
; B = X1
; C = Y1
; D = X2
; E = Y2
        LD      A,(IX+10)               ; y2
        LD      E,A
        LD      C,(IX+6)                ; y1
        CP      C
        JP      Z,drawLineHor
        LD      A,(IX+4)                ; x1
        LD      B,A
        LD      D,(IX+8)                ; x2
        CP      D
        JP      Z,drawLigneVert
        LD      HL,(TabPen)
        LD      (drawLineCoul+1),HL
        LD      A,E
        SUB     C
        JR      NC,drawLineTst1
        NEG
drawLineTst1:
        LD      H,A                     ; H = Abs(Y2-Y1)
        LD      A,D
        SUB     B
        JR      NC,drawLineTst2
        NEG
drawLineTst2:
        CP      H                       ; A=Abs(X2-X1) CP Abs(Y2-Y1)
        JR      C,drawLineV             ; Si A>=H -> Ligne par laxe horizontal
        LD      A,B
        LD      B,C
        LD      C,A                     ; Inversion X1 et Y1
        LD      A,D
        LD      D,E
        LD      E,A                     ; Inversion X2 et Y2
        LD      A,&68                   ; LD L,B
        LD      (drawLineOrd1),A
        LD      A,&79                   ; LD A,C
        JR      drawLine1
drawLineV:
        LD      A,&69                   ; LD L,C
        LD      (drawLineOrd1),A
        LD      A,&78                   ; LD A,B
drawLine1:
        LD      (drawLineAbs1),A
        LD      (drawLineAbs2),A
        LD      A,E
        CP      C
        JR      NC,drawLine2
        LD      E,C
        LD      C,A                     ; Echange Y1 et Y2
        LD      A,D
        lD      D,B
        LD      B,A                     ; Echange X1 et X2
drawLine2:
        LD      A,D
        SUB     B
        LD      L,&04                   ; INC B
        JR      NC,drawLine3
        NEG
        INC     L                       ; DEC B
drawLine3:
        EX      AF,AF'                  ; Stockage DX
        LD      A,L
        LD      (drawLineXincr),A       ; Stockage INC ou DEC B
        LD      A,C
        SUB     E
        EXX
        LD      E,A                     ; Stockage -DY
        LD      D,&FF
        EX      AF,AF'                  ; Récupère DX
        LD      L,A                     ; L = DX
        LD      H,&00
        ADD     HL,HL                   ; DX * 2
        LD      B,H
        LD      C,L                     ; Bincr = DX * 2 (BC)
        ADD     HL,DE                   ; - DY
        ADD     HL,DE                   ; - DY
        EX      DE,HL                   ; Aincr = DX * 2 - DY * 2 (DE)
        ADD     HL,BC                   ; delta = DX * 2 - DY
        EXX
        LD      A,E                     ; Récupère Y2
        ;
        ; faire un point aux coordonnées X(Reg.B), Y(Reg.C) OU Y,X
        ;
drawLineBcl:
        EX      AF,AF'                  ; Sauvegarde Y2
        LD      H,TabAdrEcr/256
drawLineOrd1:
        LD      L,C                     ; y to plot
drawLineAbs1:
        LD      A,B
        AND     A
        RRA
        AND     A
        RRA                             ; x/4
        ADD     A,(HL)
        LD      E,A
        INC     H                       ; Adresse des poids forts
        LD      A,(HL)
drawLineOffsetVideo:
        ADC     A,&C0
        LD      D,A                     ; Reg.DE = adresse mémoire écran (0,y)
drawLineAbs2:
        LD      A,B
        AND     3
        RLA
drawLineCoul:
        LD      HL,0
        ADD     A,L
        LD      L,A
        LD      A,(DE)                  ; Octet mémoire écran
        AND     (HL)                    ; Masque
        INC     L
        OR      (HL)                    ; Octet à écrire en mémoire écran
        LD      (DE),A
        EX      AF,AF'                  ; Récupère Y2
        INC     C                       ; Incrémenter Y1
        EXX
        BIT     7,H                     ; Delta négatif ?
        JR      Z,drawLineAincr
drawLineBincr:
        ADD     HL,BC                   ; Delta += Bincr
        EXX
        CP      C                       ; compare Y2 à Y1
        JR      NZ,drawLineBcl
        JR      drawLineDone
drawLineAincr:
        ADD     HL,DE                   ; Delta += Aincr
        EXX
drawLineXincr:
        INC     B                       ; ou dec b
        CP      C                       ; compare Y2-Y1
        JR      NZ,drawLineBcl
        JR      drawLineDone

drawLineHor:
        LD      A,(IX+4)                ; x1
        LD      E,A
        LD      A,(IX+8)                ; x2
        SUB     E
        RET     Z
        RET     C
        CALL    drawLigneHorInt
        JR      drawLineDone

;
; A = width (x2 - x1);
; E = x1
; (IX+6) = y
;
drawLineHorInt:
        LD      (DrawLigneHor3+1),A     ; x2- x1
        LD      HL,(TabPen)
        LD      (DrawLigneHor1+1),HL
        LD      A,(HL)
        LD      (DrawLigneHor7+1),A
        LD      C,A
        RRCA
        OR      C
        RRCA
        OR      C
        RRCA
        OR      C
        LD      (DrawLigneHor6+1),A
        LD      L,(IX+6)                ; LineY
        LD      BC,&C007                ; C0 = high byte of mem video, 07 = mask
        LD      A,L                     ; y to plot
        AND     C
        LD      H,A
        XOR     L
        LD      L,A
        ADD     HL,HL
        ADD     HL,HL
        ADD     HL,HL
        LD      A,E                     ; x to plot
        AND     3
        LD      D,A                     ; D = NumPixel
        LD      A,E
        RRA
        AND     A
        RRA
        LD      C,A                     ; x/4
        ADD     HL,BC                   ; B = high byte of mem video, C = x/4
        EX      DE,HL
        LD      C,H
DrawLigneHor1:
        LD      HL,0
        LD      B,(HL)
        LD      A,C
        AND     3
        LD      C,B
        EX      DE,HL                   ; HL = screen adr
        LD      B,4
DrawLigneHor2:
        AND     A
        JR      Z,DrawLigneHor3
        RRC     C
        DEC     B
        DEC     A
        JR      DrawLigneHor2
DrawLigneHor3:
        LD      DE,0
DrawLigneHor4:
        LD      A,(HL)
        OR      C
        LD      (HL),A
        RRC     C
        DJNZ    DrawLigneHor8
        LD      A,E
DrawLigneHor5:
        INC     HL
        SUB     5
        JR      C,DrawLigneHor7
        INC     A
        LD      E,A
DrawLigneHor6:
        LD      (HL),0
        AND     A
        JR      Z,drawLineDone
        JR      DrawLigneHor5
DrawLigneHor7:
        LD      BC,&400
DrawLigneHor8:
        DEC     E
        JR      NZ,DrawLigneHor4
        RET

drawLigneVert:
        CALL    drawLigneVertInt
        JR      drawLineDone

; B = x1
; C = y1
; E = y2
;
drawLigneVertInt:
        LD      HL,(TabPen)
        LD      (drawLineVertCoul+1),HL
        LD      A,E
        SUB     C
        JR      NC,drawLineVertTst1
        LD      A,C
        LD      C,E
        LD      E,A
        LD      A,E
        SUB     C
drawLineVertTst1:
        EX      AF,AF'                  ; Sauvegarde Abs(Y2-Y1)
        LD      A,B
        AND     3
        ADD     A,A
drawLineVertCoul:
        LD      HL,0
        ADD     A,L
        LD      L,A
        LD      A,(HL)
        LD      (drawLigneVertMask+1),A
        INC     HL
        LD      A,(HL)
        LD      (drawLigneVertValue+1),A
        SRL B
        SRL B                           ; B = x/4
        EX      AF,AF'
        LD      H,TabAdrEcr/256
        LD      L,C                     ; y to plot
drawLigneVertValue:
        LD      C,0                     ; Octet à écrire en mémoire écran
drawLigneVertBcl:
        EX      AF,AF'                  ; Sauvegarde Abs(Y2-Y1)
        LD      A,B                     ; x to plot
        ADD     A,(HL)
        LD      E,A
        INC     H                       ; Adresse des poids forts
        LD      A,(HL)
        DEC     H
drawLineVertOffsetVideo:
        ADC     A,&C0
        LD      D,A                     ; Reg.DE = adresse mémoire écran (x,y)
        LD      A,(DE)                  ; Octet mémoire écran
drawLigneVertMask:
        AND     0                       ; Masque
        OR      C
        LD      (DE),A
        INC     L
        EX      AF,AF'
        DEC     A
        JR      NZ,drawLigneVertBcl
        RET
drawLineDone:
__endasm;
}

void drawRect(unsigned short x1, unsigned short y1, unsigned short width, unsigned short height) {
x1;y1;width;height;
__asm
        LD      A,(IX+8)                ; width
        LD      E,(IX+4)                ; x1
        CALL    drawLineHorInt

        LD      B,(IX+4)                ; x1
        LD      C,(IX+6)                ; y1
        LD      A,(IX+8)                ; height
        ADD     A,C
        LD      E,A                     ; y2 = y1 + height
        CALL    drawLineVertInt

        LD      A,(IX+4)                ; x1
        ADD     A,(IX+8)                ; width
        LD      B,A                     ; x1 + width
        LD      C,(IX+6)                ; y1
        LD      A,(IX+10)               ; height
        ADD     A,C
        LD      E,A                     ; y2 = y1 + height
        CALL    drawLineVertInt

        LD     A,(IX+6)
        ADD     A,(IX+10)
        LD      (IX+6),A
        LD      A,(IX+8)                ; width
        LD      E,(IX+4)                ; x1
        CALL    drawLineHorInt
drawRectDone:
__endasm;
}

void fillRect(unsigned short x1, unsigned short y1, unsigned short width, unsigned short height) {
x1;y1;width;height;
__asm
        LD      A,(IX+4)
        OR      (IX+6)
        JR      NZ,fillRectInit
        LD      A,(IX+8)
        AND     (IX+10)
        CP      &FF
        JR      Z,fillFullScreen
fillRectInit:
        LD      HL,(TabPen)
        LD      (fillRectPen+1),HL
        INC     HL
        LD      A,(HL)
        LD      (fillRect7+1),A
        LD      C,A
        RRCA
        OR      C
        RRCA
        OR      C
        RRCA
        OR      C
        LD      (fillRect5+1),A
        LD      A,(IX+4)                ; x1
        LD      (fillRectBcl+1),A
        LD      L,(IX+6)                ; y1
        LD      A,(IX+8)                ; width
        LD      (fillRect2+1),A
        LD      H,(IX+10)               ; height
fillRectBcl:
        LD      E,0                     ; rectX1
        PUSH    HL                      ; H = height, L = y1
        LD      BC,&C007                ; C0 = high byte of mem video, 07 = mask
        LD      A,L                     ; y to plot
        AND     C
        LD      H,A
        XOR     L
        LD      L,A
        ADD     HL,HL
        ADD     HL,HL
        ADD     HL,HL
        LD      A,E                     ; x to plot
        AND     3
        LD      D,A                     ; D = NumPixel
        LD      A,E
        RRA
        AND     A
        RRA
        LD      C,A                     ; x/4
        ADD     HL,BC                   ; B = high byte of mem video, C = x/4
        EX      DE,HL
        LD      C,H
fillRectPen:
        LD      HL,0
        LD      B,(HL)
        LD      A,C
        AND     3
        LD      C,B
        EX      DE,HL                   ; HL = screen adr
        LD      B,4
fillRect1:
        AND     A
        JR      Z,fillRect2
        RRC     C
        DEC     B
        DEC     A
        JR      fillRect1
fillRect2:
        LD      DE,0
fillRect3:
        LD      A,(HL)
        OR      C
        LD      (HL),A
        RRC     C
        DJNZ    fillRect8
        LD      A,E
fillRect4:
        INC     HL
        SUB     5
        JR      C,fillRect7
        INC     A
        LD      E,A
fillRect5:
        LD      (HL),0
        AND     A
        JR      Z,fillRect9
        JR      fillRect4
fillRect7:
        LD      BC,&400
fillRect8:
        DEC     E
        JR      NZ,fillRect3
fillRect9:
        POP     HL
        INC     L
        DEC     H
        JR      NZ,fillRectBcl
        JR     fillRectDone

fillFullScreen:
        LD      HL,(TabPen)
        INC     HL
        LD      A,(HL)
        INC     HL
        INC     HL
        OR     (HL)
        INC     HL
        INC     HL
        OR     (HL)
        INC     HL
        INC     HL
        OR      (HL)
        LD      L,A
        LD      H,A
        LD      (fillFullScreenSP+1),SP
        LD      SP,0
        LD      B,0
fillFullScreenBcl:
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        PUSH    HL
        DJNZ    fillFullScreenBcl
fillFullScreenSP:
        LD    SP,0

fillRectDone:
__endasm;
}
