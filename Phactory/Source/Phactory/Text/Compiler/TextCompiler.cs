using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Phactory.Text.Compiler
{
    public class TextCompiler
    {
        public bool Compile(string destFilename, string charset, string text, bool appendEndOfText, out int charsNotFound)
		{
            charsNotFound = 0;

            try
            {
                var characters = new List<byte>();

                for (int iChar = 0; iChar < text.Length; iChar++)
                {
                    char c = text[iChar];

                    if (c == 10)
                    {
                        byte byteToWrite = 0;
                        unchecked
                        {
                            byteToWrite = (byte)-2;
                        }
                        characters.Add(byteToWrite);
                    }
                    else
                    {
                        bool found = false;
                        for (int iCharset = 0; iCharset < charset.Length; iCharset++)
                        {
                            if (charset[iCharset] == c)
                            {
                                byte byteToWrite = (byte)iCharset;
                                characters.Add(byteToWrite);
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            charsNotFound++;
                        }
                    }
                }

                if (appendEndOfText)
                {
                    byte byteToWrite = 0xFF;
                    characters.Add(byteToWrite);
                }

                File.WriteAllBytes(destFilename, characters.ToArray());

                return true;
            }
            catch
            {
                return false;
            }
		}
    }
}
