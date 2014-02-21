using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Project;

namespace CPCBitmap.Controller
{
    public class BitmapCompiler
    {
        public static BitmapCompilerInterface CreateCompiler(int videoMode)
        {
            switch (videoMode)
            {
                case 1:
                    return new BitmapCompilerMode1();
                case 2:
                    return new BitmapCompilerMode2();
                case 0:
                default:
                    return new BitmapCompilerMode0();
            }
        }
    }
}
