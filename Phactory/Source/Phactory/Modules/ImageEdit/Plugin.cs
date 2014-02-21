using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEdit
{
    static class Plugin
    {
        static private ImageEdit.Controller.Editor controller;
        static public ImageEdit.Controller.Editor Controller
        {
            get { return controller; }
            set { controller = value; }
        }
    }
}
