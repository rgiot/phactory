using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Phactory.Modules.BigFile.Compiler
{
    public class BigFileCompiler
    {
        public class SData
        {
	        public byte bank;
	        public int ptrInBank;
	        public byte packerType;
	        public int unpackedSize;
	        public int size;
	
	        public byte bank2;
	        public int ptrInBank2;
	        public int size2;

	        public bool lastFileInBank;
	        public int writeOffset;
        };

        public class SExData : SData
        {
	        public byte[] data;
	        public string filename;
	        public bool pad256;
	        public int address;
        };

        public class SC0Data
        {
	        public string filename;
	        public int size;
        };

        private string filename;

        private string GetFileNameBIN( byte bank )
        {
	        if ( bank == 0xc0 ) return filename+".c0";
	        if ( bank == 0xc4 ) return filename+".c4";
	        if ( bank == 0xc5 ) return filename+".c5";
	        if ( bank == 0xc6 ) return filename+".c6";
	        if ( bank == 0xc7 ) return filename+".c7";

	        return String.Empty;
        }

        private void createEmptyFile( string filename )
        {
            var allBytes = new List<byte>().ToArray();
            File.WriteAllBytes(filename, allBytes);
        }

        string convertFilenameToDefineName(string filename)
        {
	        int slashIndex = filename.LastIndexOf( "\\" );

	        string defineName = String.Empty;
			
	        if ( slashIndex != -1 )
	        {
		        defineName = filename.Substring( slashIndex + 1 );
	        }
	        else
	        {
		        defineName = filename;
	        }

            defineName = defineName.Replace(".", "");
            defineName = defineName.ToUpper();

	        return defineName;
        }

        public bool Compile(string baseFilename, string headerFilename, List<string> resourceFilenames, List<string> paddingFilenames, List<int> addresses, bool truncateFiles, bool filesInBank, int baseAddress )
		{
            try
            {
                filename = baseFilename;

	            int cumulatedDataSize = 0;
	            byte nbFiles = 0;

	            var dataInfo = new List< SExData >();

	            string TITLE = filesInBank ? "BANKCONFIG" : "MEMCONFIG";
	
	            // parse files to memory
	            for ( int iFile = 0; iFile < resourceFilenames.Count; ++iFile )
	            {
		            var exData = new SExData();
		            exData.filename = resourceFilenames[ iFile ];
		            exData.pad256 = ( paddingFilenames[ iFile ] == "true" );
		            exData.address = addresses[ iFile ];

		            exData.bank2 = 0;
		            exData.ptrInBank2 = 0;
		            exData.size2 = 0;

		            exData.unpackedSize = 0;

		            exData.lastFileInBank = false;

                    if ( !File.Exists(exData.filename) )
                    {
                        Project.App.Controller.Log.Append("Can't open " + exData.filename + " !");
                        return false;
                    }

		            exData.size = (int)new FileInfo(exData.filename).Length;
		            cumulatedDataSize += exData.size;
		        
                    var allBytes = File.ReadAllBytes(exData.filename);
                    
		            var pckFilename = exData.filename.ToLower();
                    if ( pckFilename.Contains(".pck"))
                    {
                        exData.packerType = allBytes[0];
                        exData.unpackedSize = (int)((uint)((allBytes[1]) + (uint)(allBytes[2]<<8)));
                    }
                
                    exData.data = new byte[ exData.size + 256 ];
                    for(int i=0; i < exData.size + 256; i++ )
                    {
                        exData.data[i] = 0;
                    }
                    for(int i=0; i < exData.size; i++ )
                    {
                        exData.data[i] = allBytes[i];
                    }
                
                    dataInfo.Add( exData );

		            nbFiles++;
	            }

	            // set bank/adress values
	            int offset = baseAddress;
	            byte bank = 0xC4;

	            for ( int iFile = 0; iFile < dataInfo.Count; ++iFile )
	            {
		            var data = dataInfo[ iFile ];

		            data.bank = bank;
		
		            if ( data.address != 0 )
		            {
			            int newOffset = data.address;
			            data.ptrInBank = newOffset;

			            data.writeOffset = newOffset - offset;

			            offset = newOffset;
		            }
		            else
		            if ( data.pad256 && ( ( offset & 0xFF ) != 0 ) )
		            {
			            int newOffset = ( offset & 0xFF00 ) + 256;
			            data.ptrInBank = newOffset;
				
			            data.writeOffset = newOffset-offset;

			            offset = newOffset;
		            }
		            else
		            {	
			            data.ptrInBank = offset;
		
			            data.writeOffset = 0;
		            }

		            offset += data.size;

		            if ( !filesInBank )
		            {
			            cumulatedDataSize = offset - baseAddress;
		            }
		            else
		            {
			            if (((ushort)(offset+data.size))<(ushort)0x8000)
			            {
				            //offset += data.size;
			            }
			            else
			            {
				            if ( truncateFiles )
				            {
					            int fullSize = data.size;

					            data.size2 = ( offset + data.size ) - 0x8000;
					            data.size -= data.size2;

					            bank++;
					            if ( bank == 0xC8 )
					            {
                                    Project.App.Controller.Log.Append("Not enough banks !");
                                    return false;
					            }

					            data.bank2 = bank;

					            offset = 0x4000;
			
					            data.ptrInBank2 = offset;

					            offset += data.size2;
				            }
				            else
				            {
					            int fullSize = data.size;

					            data.lastFileInBank = true;

					            bank++;
					            if ( bank == 0xC8 )
					            {
						            Project.App.Controller.Log.Append("Not enough banks !");
                                    return false;
					            }

					            data.bank = bank;

					            offset = 0x4000;
					            data.ptrInBank = offset;

					            offset += data.size;
				            }
			            }
		            }
	            }

	            if ( filesInBank )
	            {
		            // delete existing bank files
		            createEmptyFile( GetFileNameBIN( 0xC4 ) );
		            createEmptyFile( GetFileNameBIN( 0xC5 ) );
		            createEmptyFile( GetFileNameBIN( 0xC6 ) );
		            createEmptyFile( GetFileNameBIN( 0xC7 ) );
	            }
	            else
	            {
		            // delete existing big file
		            string bigfile = filename+".bigfile";
		            createEmptyFile( bigfile );
	            }

	            // write bank files
	            bank = 0xC4;
	            List<byte> outBytes = null;
                string outFilename = null;

	            if ( filesInBank )
	            {
                    outBytes = new List<byte>();
                    outFilename = GetFileNameBIN( bank );
	            }
	            else
	            {
                    outBytes = new List<byte>();
                    outFilename = filename+".bigfile";
	            }

	            var bankSize = new List<int>();

	            int cumul = 0;
	
	            for ( int iFile = 0; iFile < dataInfo.Count; ++iFile )
	            {
		            var data = dataInfo[ iFile ];

		            if ( data.lastFileInBank )
		            {
			            bankSize.Add( cumul );

			            bank++;

			            File.WriteAllBytes(outFilename, outBytes.ToArray());
			
			            outFilename = GetFileNameBIN( bank );
                        outBytes = new List<byte>();

                        for( int i=0; i < data.size; i++ )
                        {
                            outBytes.Add(data.data[i]);
                        }

			            cumul = data.ptrInBank + data.size - baseAddress;
		            }
		            else if ( data.bank2 != 0 )
		            {
			            for( int i=0; i < data.size; i++ )
                        {
                            outBytes.Add(data.data[i]);
                        }
                    
                        cumul = data.ptrInBank + data.size - baseAddress;
				
			            bankSize.Add( cumul );

			            bank++;

			            File.WriteAllBytes(outFilename, outBytes.ToArray());
			
			            outFilename = GetFileNameBIN( bank );

                        for( int i=0; i < data.size2; i++ )
                        {
                            outBytes.Add(data.data[data.size+i]);
                        }
                    
                        cumul = data.ptrInBank2 + data.size2 - baseAddress;
		            }
		            else
		            {
			            if ( !filesInBank )
			            {
				            if ( data.writeOffset > 0 )
				            {
					            for ( int i = 0; i < data.writeOffset; i++ )
					            {
                                    outBytes.Add(0);
					            }
				            }
			            }

			            for( int i=0; i < data.size; i++ )
                        {
                            outBytes.Add(data.data[i]);
                        }
                    
                        cumul = data.ptrInBank + data.size - baseAddress;
		            }
	            }

	            bankSize.Add( cumul );

                File.WriteAllBytes(outFilename, outBytes.ToArray());
	
	            int slashIndex = filename.LastIndexOf( "\\" );

	            string scopeName = convertFilenameToDefineName(headerFilename);

	            var headerContent = "// ----------------------------------------------------------------------------\n";
                headerContent += "#ifndef _" + scopeName + "_" + TITLE + "_H_\n";
	            headerContent += "#define _" + scopeName + "_" + TITLE + "_H_\n";
	            headerContent += "\n";
                headerContent += "// ----------------------------------------------------------------------------\n";
	        
                if ( !filesInBank )
	            {
		            headerContent += "#define " + scopeName + "_DATAPTR 0x" + String.Format("{0:X}", baseAddress) + "\n";	
	            }
	            headerContent += "#define " + scopeName + "_DATASIZE 0x" + String.Format("{0:X}", cumulatedDataSize) + "\n";
	            if ( !filesInBank )
	            {
		            headerContent += "#define " + scopeName + "_DATAENDPTR 0x" + String.Format("{0:X}", baseAddress+cumulatedDataSize) + "\n";
	            }
	            headerContent += "\n";
	            if ( filesInBank )
	            {
		            headerContent += "// ----------------------------------------------------------------------------\n";
		            if ( bank >= 0xC4 )
		            {
			            headerContent += "#define " + scopeName + "_" + TITLE + "_C4\n";
                        headerContent += "#define " + scopeName + "_" + TITLE + "_C4_SIZE 0x" + String.Format("{0:X}", bankSize[ 0 ]) + "\n";
		            }	
		            if ( bank >= 0xC5 )
		            {
                        headerContent += "#define " + scopeName + "_" + TITLE + "_C5\n";
                        headerContent += "#define " + scopeName + "_" + TITLE + "_C5_SIZE 0x" + String.Format("{0:X}", bankSize[ 1 ]) + "\n";
		            }	
		            if ( bank >= 0xC6 )
		            {
                        headerContent += "#define " + scopeName + "_" + TITLE + "_C6\n";
                        headerContent += "#define " + scopeName + "_" + TITLE + "_C6_SIZE 0x" + String.Format("{0:X}", bankSize[ 2 ]) + "\n";
		            }	
		            if ( bank >= 0xC7 )
		            {   
                        headerContent += "#define " + scopeName + "_" + TITLE + "_C7\n";
                        headerContent += "#define " + scopeName + "_" + TITLE + "_C7_SIZE 0x" + String.Format("{0:X}", bankSize[ 3 ]) + "\n";
		            }	

		            headerContent += "\n";
	            }
	
	            headerContent += "// ----------------------------------------------------------------------------\n";
	            for ( int iFile = 0; iFile < dataInfo.Count; ++iFile )
	            {
		            var data = dataInfo[ iFile ];

		            string defineName = convertFilenameToDefineName(data.filename);
			
		            string dataFilename = data.filename.Replace("\\","\\\\");
	
		            string displayName = data.filename.Substring( slashIndex + 1 );	
		            headerContent += "#define " + defineName + "_FILENAME \"" + displayName + "\"\n";
			
		            if ( data.bank2 != 0 )
		            {
                        headerContent += "#define " + defineName + "_BANK1 0x" + String.Format("{0:X}", data.bank) + "\n";
                        headerContent += "#define " + defineName + "_PTRINBANK1 0x" + String.Format("{0:X}", data.ptrInBank) + "\n";
                        headerContent += "#define " + defineName + "_SIZE1 0x" + String.Format("{0:X}", data.size) + "\n";
                        headerContent += "#define " + defineName + "_BANK2 0x" + String.Format("{0:X}", data.bank2) + "\n";
                        headerContent += "#define " + defineName + "_PTRINBANK2 0x" + String.Format("{0:X}", data.ptrInBank2) + "\n";
                        headerContent += "#define " + defineName + "_SIZE2 0x" + String.Format("{0:X}", data.size2) + "\n";
                        headerContent += "#define " + defineName + "_ENDPTR 0x" + String.Format("{0:X}", data.ptrInBank2 + data.size2) + "\n";
		            }
		            else
		            {
			            if ( filesInBank )
			            {
				            headerContent += "#define " + defineName + "_BANK 0x" + String.Format("{0:X}", data.bank) + "\n";
                            headerContent += "#define " + defineName + "_PTRINBANK 0x" + String.Format("{0:X}", data.ptrInBank) + "\n";
			            }
			            else
			            {
                            headerContent += "#define " + defineName + "_PTR 0x" + String.Format("{0:X}", data.ptrInBank) + "\n";
			            }
                        headerContent += "#define " + defineName + "_SIZE 0x" + String.Format("{0:X}", data.size) + "\n";
                        headerContent += "#define " + defineName + "_ENDPTR 0x" + String.Format("{0:X}", data.ptrInBank + data.size) + "\n";
		            }

		            if (data.unpackedSize != 0)
		            {
                        headerContent += "#define " + defineName + "_UNPACKEDSIZE 0x" + String.Format("{0:X}", data.unpackedSize) + "\n";
		            }
		
		            headerContent += "\n";
	            }

	            headerContent += "// ----------------------------------------------------------------------------\n";
                headerContent += "#endif // _" + scopeName + "_" + TITLE + "_H_\n";
	
	            File.WriteAllText(headerFilename, headerContent);
            }
            catch(Exception e)
            {
                Project.App.Controller.Log.Append(e.ToString());
                return false;
            }

            return true;
		}
    }
}
