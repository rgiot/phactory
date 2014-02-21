using System;
using System.Collections.Generic;
using System.Text;

namespace SDCC
{
    static class Plugin
    {
        static private SDCC.Controller.Compiler controllerCompiler;
        static public SDCC.Controller.Compiler ControllerCompiler
        {
            get { return controllerCompiler; }
            set { controllerCompiler = value; }
        }
    }
}
