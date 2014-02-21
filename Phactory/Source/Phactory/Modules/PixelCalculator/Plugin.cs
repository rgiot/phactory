using System;
using System.Collections.Generic;
using System.Text;

namespace PixelCalculator
{
    static class Plugin
    {
        static private PixelCalculator.Controller.Tool controller;
        static public PixelCalculator.Controller.Tool Controller
        {
            get { return controller; }
            set { controller = value; }
        }
    }
}
