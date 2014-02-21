
LoadFile 			equ &E800
SetDrive 			equ LoadFile + 3
SetMusicRoutine 	equ SetDrive + 3
MusicOff 			equ SetMusicRoutine + 3
RecalDrive 			equ MusicOff + 3
CacheDir 			equ RecalDrive + 3
DiscMotorOn 		equ CacheDir + 3
DiscMotorOff 		equ DiscMotorOn + 3

FileOrderPtr		equ &EE00