using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Management;

namespace Project.Controller
{
    public class Log
    {
        private string logFilename;

        public Log(string logFilename)
        {
            this.logFilename = logFilename;
        }

        public void Init()
        {
            App.Controller.View.ClearLog();            
            File.Delete(logFilename);

            Append("===== Welcome to " + App.Controller.GetApplicationName() + " ! =====");
            Append("Build date: " + App.Controller.GetBuildDate());
            Append("Net:" + System.Environment.Version + ", CPU: " + System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") + " (x" + System.Environment.ProcessorCount + ")");
            Append(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private string[] SplitByLenght(string s, int split)
        {
            //Like using List because I can just add to it 
            List<string> list = new List<string>();

                        // Integer Division
            int TimesThroughTheLoop = s.Length/split;


            for (int i = 0; i < TimesThroughTheLoop; i++)
            {
                list.Add(s.Substring(i * split, split));
            }

            // Pickup the end of the string
            if (TimesThroughTheLoop * split != s.Length)
            {
                list.Add(s.Substring(TimesThroughTheLoop * split));
            }

            return list.ToArray();
        }

        private static object locker = new object();

        public void AppendOneLine(String statusText)
        {
            statusText = statusText.Replace("\r","");

            if (statusText.Length == 0)
            {
                return;
            }

            lock (locker)
            { 
                StreamWriter w = File.AppendText(logFilename);
            
                var lines = SplitByLenght(statusText, 160);
                foreach (var line in lines)
                {
                    Debug.WriteLine(line);

                    App.Controller.View.LogLine(line);

                    w.WriteLine(line);
                }
                w.Flush();
                w.Close();
            }
        }

        public void Append(String statusText)
        {
            string[] lines = statusText.Split('\n');
            foreach (string oneLine in lines)
            {
                AppendOneLine(oneLine);
            }
        }

        public void StartLongOperation()
        {
            Append("Operation started at " + GetDateTime());
        }

        public void FinishLongOperation()
        {
            Append("Operation finished at " + GetDateTime());
            Append("");
        }
        
        private string GetDateTime()
        {
            return DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
        }
    }
}