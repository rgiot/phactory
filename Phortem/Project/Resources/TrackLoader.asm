;; we're going to do direct read access
;;org &4000
;;public load_file
;;public load_file
;;public set_drive
;;public init

	org &E800

jp load_file		;; +  0
jp set_drive		;; +  3
jp set_music_routine	;; + 6
jp music_off		;; +  9
jp recal_drive		;; + 12
jp cache_dir		;; + 15
jp disc_motor_on	;; + 18
jp disc_motor_off	;; + 21

;; FILE_ID			equ 0						;; 1 byte (file id)
 ;;FILE_TRACK_LOC  equ FILE_ID+1				;; 1 byte (track id)
 ;;FILE_SECTOR_LOC equ FILE_TRACK_LOC+1		;; 1 byte (sector id)
 ;;FILE_OFFSET 	equ FILE_SECTOR_LOC+1		;; 2 byte (offset in sector for start of file)
 ;;FILE_SIZE		equ FILE_OFFSET+2			;; 2 byte (length of data in bytes)
 ;;FILE_SIDE_LOC equ FILE_SIZE+2
 ;;FILE_UNUSED		equ FILE_SIDE_LOC+1
 ;;FILE_ENTRY_SIZE equ FILE_UNUSED+1

 FILE_TRACK_SIDE_LOC  equ 0				;; 1 byte (track id)
 FILE_SECTOR_LOC equ FILE_TRACK_SIDE_LOC+1		;; 1 byte (sector id)
 FILE_SIZE		equ FILE_SECTOR_LOC+1			;; 2 byte (length of data in bytes)
 FILE_ENTRY_SIZE equ FILE_SIZE+2

 DIR_NUM_SECTORS equ 2
 DIR_SECTOR_SIZE equ 512
 MAX_FILES equ ((DIR_SECTOR_SIZE*DIR_NUM_SECTORS)-4)/FILE_ENTRY_SIZE
 IDENT_SIZE equ 4
 EVERY_BYTE equ 0						;; 1 means use every byte we can, files can start and end part way through a sector
 DIR_TRACK equ 1
 DIR_SECTOR equ &ca-DIR_NUM_SECTORS
 
 NUM_SIDES equ 2
 
;; first track we can write to
 FIRST_TRACK equ 3			;; temp for musical loader
;; first sector we can write to
 FIRST_SECTOR equ 1

;;
 DATA_SECTOR_SIZE equ 512
;; DATA_SECTOR_SIZE equ 256


;if DATA_SECTOR_SIZE =512
 SECTORS_PER_TRACK equ 10
;endif

;if DATA_SECTOR_SIZE = 256
; SECTORS_PER_TRACK equ 16
;endif

 FIRST_SECTOR_ID equ 1

;; this is needed only when accessing a new drive. 
;; motor must be on, and disc must be inserted
;; after this is done FDC has a known state for the drive
;; read head position and can then use this for accurate seeks
recal_drive:
jp fdc_recalibrate

;; read the directory for the special format from the drive
cache_dir:
jp read_directory

;; turn off disc motor immediately
disc_motor_off:
xor a
ld bc,&fa7e
out (c),a
ret

;; set the drive and side the loader will use
;; drive in bits 0 and 1
;; side in bit 2
set_drive:
ld (drive),a
ret

simple_ret:
	ret
	
music_off:
ld hl,simple_ret

; hl = music routine

set_music_routine:
ld (play_music_rout),hl
ld (play_music_rout2+1),hl

call InstallIntHandler

noplay:
ret

int_handler_dest equ &8002

InstallIntHandler:
ld hl, InterruptPlayMusic
ld bc, InterruptPlayMusic_End - InterruptPlayMusic
ld de, int_handler_dest
ldir
di
ld a,&c3
ld (&0038),a
ld hl,int_handler_dest
ld (&0039),hl
xor a
ld (counter),a
ei
ret

InterruptPlayMusic:
	ld ( int_handler_dest - 2 ), sp
	ld sp, &8060
	push bc
	push af
	
	ld a, (&0000)
	push af
	
	ld bc, &7FC1	
	out (c), c
	ld a, &C1
	ld (&0000), a
	
	call int_handler
	
	pop af
	ld b, &7F
	ld c, a
	out (c), c	
	ld (&0000), a
	
	pop af
	pop bc
	ld sp, ( int_handler_dest - 2 )
	ei
	ret
InterruptPlayMusic_End:

;; load a file
;; HL = address to load file to
;; A = file id to load
;; directory must be cached, because it is searched for this file
load_file:
	call load_file2
	ei
	ret

load_file2:
ld (file_data_ptr),hl

;; id -> directory entry
ld de,FILE_ENTRY_SIZE
ld ix,directory+4		;; skip ident
load_file2b:
dec a
jr z,load_file2a
add ix,de
jr load_file2b

load_file2a:
;;ei

ld a,(ix+FILE_TRACK_SIDE_LOC)
ld l,a
srl a
ld (track),a
ld a,l
and &1
ld (head),a
ld a,(ix+FILE_SECTOR_LOC)
ld (sector),a
ld l,(ix+FILE_SIZE+0)	
ld h,(ix+FILE_SIZE+1)
ld (length_remaining),hl
;; get side

call set_drive_and_side

;; set initial ignore
;;if EVERY_BYTE=1
;;ld c,(ix+FILE_OFFSET+0)
;;ld b,(ix+FILE_OFFSET+1)
;;endif
;;if EVERY_BYTE=0
;;ld bc,0
;;endif

read_sectors:
;; init loader interrupt
call InstallIntHandler

read_next_track:
;; enable interrupts when doing seek
ei

;; while seek play during interrupt
;; perform initial seek
call fdc_seek

;; interrupts are enabled, so play under interrupt control
ld hl,noplay
ld (mus_play_addr+1),hl

read_next_sector:
ld hl,DATA_SECTOR_SIZE
;; HL = actual size of data remaining in sector
ld bc,(length_remaining)
or a
sbc hl,bc
;; this will be positive if  length is less than size remaining
;; or negative if it is larger
jp p,write_block_init1

;; negative means larger so find original value and write that
or a
adc hl,bc

;; we're done reading
ld de,fdc_read_end
jr write_block_init

write_block_init1:
ld de,fdc_read_data_skip		;; skip rest
ld hl,(length_remaining)
write_block_init:
ld (read_length),hl
ld (fdc_read_jmp+1),de

read_sect_again:

ld bc,&fb7e					;; [3]

;; [40]+[2]+[5] = [47] per command byte
ld a,01000110b				;; [2]
call fdc_write_command		;; [5]
							;; [47]
							
ld a,(drive_and_side)				;; [4]
call fdc_write_command		;; [5]
							;; [49]
							
ld a,(track)				;; C
call fdc_write_command	
							;; [49]

ld a,(head)						;; H
call fdc_write_command
ld a,(sector)					;; R
call fdc_write_command
if DATA_SECTOR_SIZE=512
ld a,2
endif
if DATA_SECTOR_SIZE=256
ld a,1
endif
				;; N
call fdc_write_command
ld a,(sector)					;; EOT
call fdc_write_command
ld a,&19
call fdc_write_command
ld a,&ff
call fdc_write_command
						;; [480] for command
						
;; play music
mus_play_addr:
call noplay

;; not sure if this is needed
ld a,4
ld (counter),a

ld bc,&fb7e

;; now read sector
ld hl,(file_data_ptr)
ld de,(read_length)
;; wait for data to come
fdc_wait_data:
in a,(c)
jp p,fdc_wait_data
di
jr fdc_data_read2

fdc_data_read: 
in a,(c)				;; [4]
jp p,fdc_data_read		;; [3]
fdc_data_read2:
and &20					;; [2]
jr z,fdc_read_end 		;; [3]

inc c
in a,(c)
ld (hl),a
dec c
inc hl
dec de					;; [2]
ld a,d					;; [1]
or e					;; [1]
jr nz,fdc_data_read		;; [3]
fdc_read_jmp:
jp fdc_read_end

;; skip remaining in sector
fdc_read_data_skip:
in a,(c)
jp p,fdc_read_data_skip
fdc_read_data_skip2:
and &20
jr z,fdc_read_end
inc c
in a,(c)
dec c
jr fdc_read_data_skip

fdc_read_end:
call fdc_result
;; check result
ld ix,result_data
;;ld c,&54
ld a,(ix+0)
bit 3,a					;; not ready
jr nz,goterror
ld a,(ix+1)
and 00110101b			;; data error, overrun, no data, missing address mark
jr nz,goterror
ld a,(ix+2)
and 01110011b			;; read errors 
jr nz,goterror
jr nerr

goterror:
ei
ld hl,noplay
ld (mus_play_addr+1),hl

jp read_sect_again


nerr:
ld hl,(play_music_rout)
ld (mus_play_addr+1),hl

;; set border depending on error or not
;;ld a,&10
;;ld b,&7f
;;out (c),a
;;out (c),c

ld de,(read_length)
ld hl,(file_data_ptr)
add hl,de
ld (file_data_ptr),hl

;; update length remaining
ld hl,(length_remaining)
or a
sbc hl,de
ld (length_remaining),hl
ld a,h
or l
ret z

;; increment track/sector
ld a,(sector)
inc a
ld (sector),a
last_sector:
cp FIRST_SECTOR_ID+SECTORS_PER_TRACK
jp c,read_next_sector

if NUM_SIDES=2
;; change side
ld a,FIRST_SECTOR_ID
ld (sector),a

;; head value first
ld a,(head)
xor 1
ld (head),a
;; and drive/side select
ld a,(drive_and_side)
xor %100
ld (drive_and_side),a
and %100
;; if we change to side 1, we don't do a seek and we can continue to read sectors
jp nz,read_next_sector
endif

;; went to side 0

;; increment track
ld a,(track)
inc a
ld (track),a

first_sector:
ld a,FIRST_SECTOR_ID
ld (sector),a

jp read_next_track


;;ignore_len:
;;defw 0

file_data_ptr:
defw 0

read_length:
defw 0

length_remaining:
defw 0

sector_size:
defw 0

;;noplay:
;;ret


play_music_rout:
defw noplay
;; current track/sector counts
track:
defb 0
sector:
defb 0
;;sector_n:
;;defb 0
drive:
defb 0
drive_and_side:
defb 0
head:
defb 0

if 0
dir_find:
;db &ed, &ff
ld ix,directory+IDENT_SIZE			;; location of directory in ram
ld b,MAX_FILES			;; max number of files

dir_find_free2:
cp (ix+FILE_ID)
ret z
ld de,FILE_ENTRY_SIZE
add ix,de
djnz dir_find_free2
inc b
ret
endif

;;read_directory:
;;ld a,DIR_TRACK
;;ld (track),a
;;
;;call fdc_seek
;;
;;ld a,DIR_SECTOR
;;ld (sector),a
;;
;;ld hl,DIR_SECTOR_SIZE
;;ld de,directory
;;di
;;call read_sector
;;ei
;;ret

set_drive_and_side:
;; set drive/side
ld a,(drive)
and &3
ld c,a
ld a,(head)
add a,a
add a,a
or c
ld (drive_and_side),a
ret


;; this uses musical loader for directory too
read_directory:
ld a,DIR_TRACK
ld (track),a
ld a,DIR_SECTOR
ld (sector),a
xor a         ;; directory head
ld (head),a
ld hl,directory
ld (file_data_ptr),hl
ld de,DIR_SECTOR_SIZE*DIR_NUM_SECTORS
ld (length_remaining),de
ld a,&c1
ld (first_sector+1),a
ld a,&ca
ld (last_sector+1),a

call set_drive_and_side

call read_sectors
ld a,FIRST_SECTOR_ID
ld (first_sector+1),a
ld a,FIRST_SECTOR_ID+SECTORS_PER_TRACK
ld (last_sector+1),a
ret

if 0
read_sector:
di
;;push de
ld bc,&fb7e					;; [3]

;; [40]+[2]+[5] = [47] per command byte
ld a,01000110b					;; [2]
call fdc_write_command		;; [5]
							;; [47]
							
ld a,(drive)				;; [4]
call fdc_write_command		;; [5]
							;; [49]
							
ld a,(track)				;; C
call fdc_write_command	
							;; [49]

ld a,0						;; H
call fdc_write_command
ld a,(sector)					;; R
call fdc_write_command
;;ld a,2

if DIR_SECTOR_SIZE=512
ld a,2
endif
if DIR_SECTOR_SIZE=256
ld a,1
endif
				;; N
call fdc_write_command
ld a,(sector)					;; EOT
call fdc_write_command
ld a,&2a
call fdc_write_command
ld a,&ff
call fdc_write_command

;;ld bc,&fb7e
;;pop de

fdc_data_read1: 
in a,(c)				;; [4]
jp p,fdc_data_read1		;; [3]
fdc_data_read12:
and &20					;; [2]
jp z,fdc_read_end2 		;; [3]

inc c					;; BC = I/O address for FDC data register
in a,(c)				;; read from FDC data register
ld (de),a				;; write to memory
dec c					;; BC = I/O address for FDC main status register
inc de					;; increment memory pointer
jp fdc_data_read1

fdc_read_end2:
call fdc_result
ret
endif

fdc_seek:
ld bc,&fb7e					;; [3]
ld a,00001111b    ;; seek command
call fdc_write_command
ld a,(drive)
call fdc_write_command
ld a,(track)
call fdc_write_command

;; wait for a vsync before checking status
seek_wait:
call seek_or_recalibrate
ld hl,result_data
bit 4,(hl)					;; equipment check?
jr nz,fdc_seek
ret


fdc_recalibrate:
;;xor a
;;ld (recalibrate_count),a

fdc_recalibrate2:

;; seek to track 0
ld bc,&fb7e					;; [3]
ld a,111b						;; recalibrate
call fdc_write_command
ld a,(drive)					;; drive
call fdc_write_command

call seek_or_recalibrate
ld hl,result_data
bit 4,(hl)					;; equipment check?
jr nz,fdc_recalibrate2
ret


seek_or_recalibrate:
;; wait for vsync, write ay
;;call update_ay_vsync
;; wait for vsync end
;;call wait_vsync_end

;; do sense interrupt status
call sense_interrupt_status
ld hl,result_data
ld a,(hl)
cp &80							;; invalid (seek not complete yet)?
jr z,seek_or_recalibrate
bit 3,a
jr nz,seek_or_recalibrate		;; ready state changed?
bit 5,a							;; seek end?
jr z,seek_or_recalibrate
ret

sense_interrupt_status:

ld bc,&fb7e					;; [3]
ld a,1000b						;; sense interrupt status
call fdc_write_command
jp fdc_result
;;ret

disc_motor_on:
;; turn on motor (SKIP THIS, STARTED FROM BOOT)
;ld bc,&fa7e
;ld a,1
;out (c),a

;; wait for drive to be full speed....
;; maybe we could speed this up by checking each drive is ready...?

;; equivalent to 4 seconds (4 seconds is default)
ld a, (&0030) ; see also end of routine for post init
;ld de,1 ; 50*4
ld d, 0
ld e, a

ld b,&f5
w1:
in a,(c)
rra
jr nc,w1
w2:
in a,(c)
rra
jr c,w2

dec de
ld a,d
or e
jr nz,w1

ret
  

int_handler:
push af

ld a,(counter)
inc a
cp 6
jr nz,inthandler2
	push ix
	push iy
	push af
	push bc
	push de
	push hl
	exx
	ex af, af'
	push ix
	push iy
	push af
	push bc
	push de
	push hl
	
play_music_rout2:
call noplay

	pop hl
	pop de
	pop bc
	pop af
	pop iy
	pop ix
	ex af, af'
	exx
	pop hl
	pop de
	pop bc
	pop af
	pop iy
	pop ix

xor a

inthandler2:
ld (counter),a
pop af
;ei
ret

counter:
defb 0

sense_drive_status:
ld bc,&fb7e					;; [3]
ld a,100b						;; sense drive status
call fdc_write_command
ld a,(drive)					;; drive status of connected drive
call fdc_write_command
jp fdc_result			;; status should show that drive is NOT READY!
;;ret
;;===============================================
;; send command to fdc
;;
;; 40 per byte

fdc_write_command:
push af
fwc1:
in a,(c)					;; [4]
jp p,fwc1					;; [3]
pop af
inc c						;; [1]
out (c),a					;; [4]
dec c
ld a,5
fwc2:
dec a
jr nz,fwc2
ret							;; [3]

;;===============================================
;; get result phase of command
;;
;; [373] for read data result

fdc_result:
	ld hl, result_data 			;; [3]
	ld bc, &fb7e					;; [3]
fr1:
	in a,(c)					;; [4]
	cp &c0 						;; [2]
	jp c,fr1					;; [3]
 
	inc c 						;; [1]
	in a,(c) 					;; [4]
	dec c 						;; [1]
	ld (hl),a 					;; [2]
	inc hl 						;; [2]

	ld a, 5
fwc3:
	dec a
	jr nz, fwc3

	in a, (c) 					;; [4]
	and &10 					;; [2]
	jp nz, fr1					;; [3]

;; 52 per byte
;; 52 * 7 = 364

	ret

result_data:
	defs 8

directory:
	defs DIR_SECTOR_SIZE*DIR_NUM_SECTORS
