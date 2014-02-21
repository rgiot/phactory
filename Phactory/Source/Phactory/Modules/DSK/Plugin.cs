using System;
using System.Collections.Generic;
using System.Text;

namespace CPCDSK
{
    static class Plugin
    {
        static private CPCDSK.Controller.Compiler controllerCompiler;
        static public CPCDSK.Controller.Compiler ControllerCompiler
        {
            get { return controllerCompiler; }
            set { controllerCompiler = value; }
        }

        static private CPCDSK.Controller.Editor controllerEditor;
        static public CPCDSK.Controller.Editor ControllerEditor
        {
            get { return controllerEditor; }
            set { controllerEditor = value; }
        }

        static private CPCDSK.Controller.Runner controllerRunner;
        static public CPCDSK.Controller.Runner ControllerRunner
        {
            get { return controllerRunner; }
            set { controllerRunner = value; }
        }

        static private CPCDSK.Controller.Tool controllerTool;
        static public CPCDSK.Controller.Tool ControllerTool
        {
            get { return controllerTool; }
            set { controllerTool = value; }
        }

        static private CPCDSK.Controller.ManageDSK controllerManageDSK;
        static public CPCDSK.Controller.ManageDSK ControllerManageDSK
        {
            get { return controllerManageDSK; }
            set { controllerManageDSK = value; }
        }
    }
}
