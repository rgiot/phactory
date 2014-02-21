using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

namespace Project
{
    static public class App
    {
        static private Controller.Controller controller;
        static public Controller.Controller Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string filename = "";
            if (Args.Length > 0)
            {
                filename = Args[0];
            }

            controller = new Controller.Controller(new View.View());
            controller.StartApplication(filename);
        }
    }
}
