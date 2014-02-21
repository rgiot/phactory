using System;
using System.Collections.Generic;
using System.Text;

namespace HexEdit
{
    static class Plugin
    {
        static private HexEdit.Controller.Editor controller;
        static public HexEdit.Controller.Editor Controller
        {
            get { return controller; }
            set { controller = value; }
        }
    }
}
