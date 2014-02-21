using System;
using System.Collections.Generic;
using System.Text;

namespace SourceEdit
{
    static class Plugin
    {
        static private SourceEdit.Editor.Editor controllerEditor;
        static public SourceEdit.Editor.Editor ControllerEditor
        {
            get { return controllerEditor; }
            set { controllerEditor = value; }
        }
    }   
}
