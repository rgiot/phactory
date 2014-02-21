char read_sector(unsigned char * buffer,unsigned char drive,unsigned char track,unsigned char sector);
char write_sector(unsigned char * buffer,unsigned char drive,unsigned char track,unsigned char sector);
char wait_key();
char wait_key2();
char hasOtherChar();
void init_keys();
void restore_keys();
char reboot();
void cfg_disk_drive();
void move_to_track(unsigned char track);

void fastPrintString(unsigned char *screenBuffer, unsigned char *string );
void clear_line(unsigned char y_pos);
void invert_line(unsigned char y_pos);
