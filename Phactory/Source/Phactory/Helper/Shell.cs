using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Project.Helper
{
    static public class Shell
    {
        public static String LastStandardOutput = null;
        public static String LastErrorOutput = null;

        public class InitWorker
        {
            private string filename;
            private string arguments;
            private string workingDirectory;
            private bool waitForExit;
            private bool infiniteWaitForExit;
            private bool exited = false;

            public bool IsOK = false;
            
            public InitWorker(string filename, string arguments, string workingDirectory, bool waitForExit, bool infiniteWaitForExit)
            {
                this.filename = filename;
                this.arguments = arguments;
                this.workingDirectory = workingDirectory;
                this.waitForExit = waitForExit;
                this.infiniteWaitForExit = infiniteWaitForExit;
            }

            private void StandardOutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    LastStandardOutput += e.Data + "\n";
                }
            }

            private void ErrorOutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    LastErrorOutput += e.Data + "\n";
                }
            }

            private void Exited(object sender, EventArgs e)
            {
                exited = true;
            }

            public void DoWork()
            {
                try
                {
                    ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo(filename, arguments);
                    processStartInfo.RedirectStandardError = true;
                    processStartInfo.RedirectStandardOutput = true;
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.ErrorDialog = false;
                    if (workingDirectory != null)
                    {
                        processStartInfo.WorkingDirectory = workingDirectory;
                    }

                    Process process = new Process();
                    process.StartInfo = processStartInfo;
                    LastStandardOutput = String.Empty;
                    LastErrorOutput = String.Empty;
                    process.OutputDataReceived += StandardOutputDataReceived;
                    process.ErrorDataReceived += ErrorOutputDataReceived;
                    if (infiniteWaitForExit)
                    {
                        process.EnableRaisingEvents = true;
                    }
                    process.Exited += Exited;
                    process.Start();
                    process.BeginErrorReadLine();
                    process.BeginOutputReadLine();

                    if (infiniteWaitForExit)
                    {
                        while (!exited)
                        {
                            Thread.Sleep(50);
                        }
                    }
                    else if (waitForExit)
                    {
                        int tickCount = Environment.TickCount;

                        var processTimeout = 1000 * App.Controller.UserConfig.ProcessTimeoutInSec;

                        while (process.HasExited == false)
                        {
                            if (Environment.TickCount > (tickCount + processTimeout))
                            {
                                process.Kill();
                                App.Controller.Log.Append("Process killed after timeout (filename: " + filename + ", arguments: " + arguments + ")");
                                IsOK = false;
                                return;
                            }

                            Thread.Sleep(50);
                        }
                    }

                    if (App.Controller.UserConfig.VerboseOutput)
                    {
                        if (LastStandardOutput != String.Empty)
                        {
                            App.Controller.Log.Append(LastStandardOutput);
                        }
                        if (LastErrorOutput != String.Empty)
                        {
                            App.Controller.Log.Append(LastErrorOutput);
                        }
                    } 
                    
                    process.Close();

                    IsOK = true;
                }
                catch (Exception ex)
                {
                    App.Controller.Log.Append(ex.Message);
                    Console.WriteLine(ex.Message);

                    IsOK = false;
                }
            }
        }

        public static bool StartProcess(string filename, string arguments, string workingDirectory, bool waitForExit, bool infiniteWaitForExit)
        {
            App.Controller.View.AppDoEvents(); 
            
            if (App.Controller.UserConfig.VerboseOutput)
            {
                App.Controller.Log.Append(new FileInfo(filename).Name + " " + arguments);
                App.Controller.Log.Append("@ " + workingDirectory);
            }

            InitWorker worker = new InitWorker(filename, arguments, workingDirectory, waitForExit, infiniteWaitForExit);

            Thread t = new Thread(worker.DoWork);
            t.Start();
            while (t.IsAlive)
            {
                App.Controller.View.AppDoEvents();
            }

            if (App.Controller.UserConfig.VerboseOutput)
            {
                if (worker.IsOK)
                {
                    App.Controller.Log.Append(new FileInfo(filename).Name + " ended.");
                }
                else
                {
                    App.Controller.Log.Append("Problem occurred with " + new FileInfo(filename).Name + " !");
                }
            }
                
            return worker.IsOK;
        }
    }
}
