using System;
using System.Collections.Generic;
using System.Text;

namespace HxCConverter
{
    static class Plugin
    {
        static private HxCConverter.Controller.Tool controller;
        static public HxCConverter.Controller.Tool Controller
        {
            get { return controller; }
            set { controller = value; }
        }
    }
}
